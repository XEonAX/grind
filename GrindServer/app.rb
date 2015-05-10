# app.rb
# try https://github.com/bbatsov/ruby-style-guide
require 'sinatra/base'
require 'sinatra/reloader'
require 'sinatra/activerecord'
require 'json'
require 'multi_json'
require 'rack/contrib'
require 'sinatra-websocket'
require 'thin'

# DB Start
ActiveRecord::Base.establish_connection(
  adapter: 'sqlite3',
  database: 'Grind.Server.sqlite3.db'
)
# Alternate Code Begin
# set :database, 'mysql://root@localhost/grindDB'
# ActiveRecord::Base.logger.level = 1
# Alternate Code End
# DB End

class ExceptionHandling
  def initialize(app)
    @app = app
  end

  def call(env)
    @app.call env
    rescue => ex
      env['rack.errors'].puts ex
      env['rack.errors'].puts ex.backtrace.join("\n")
      env['rack.errors'].flush

      hash = { Message: ex.to_s }
      [500, { 'Content-Type' => 'application/json' }, [MultiJson.dump(hash)]]
  end
end

def timestamp
  Time.now.strftime('%H:%M:%S')
end

# {Models}

class Person < ActiveRecord::Base
  has_many :unread_objects
  has_many :tasks
end

class Unread_Object < ActiveRecord::Base
  belongs_to :person, counter_cache: true
end

class Developer < Person
end
class Reviewer < Person
end

class Task < ActiveRecord::Base
  belongs_to :developer
  belongs_to :reviewer
  has_many :documents
  after_save :add_task_to_unread_objects

  protected

  def add_task_to_unread_objects
    Person.find_each do |person|
      Unread_Object.create(
        person_id: person.id,
        internal_object_id: internal_object_id,
        unread_cause: 'Create')
    end
  end

  public

  def task_params
    params.require(:task).permit(:name, :task_status, :bug_type)
  end
end

class Document < ActiveRecord::Base
  belongs_to :task, counter_cache: true
  belongs_to :developer, counter_cache: true
end

class Comment < ActiveRecord::Base
  belongs_to :task, counter_cache: true
  has_many :comments
end
# }{Models}

EventMachine.run do
  class App < Sinatra::Base
    use Rack::PostBodyContentTypeParser
    set :server, 'thin'
    set :sockets, []
    use ExceptionHandling
    configure do

      # Don't log them. We'll do that ourself
      set :dump_errors, false

      # Don't capture any errors. Throw them up the stack
      set :raise_errors, true

      # Disable internal middleware for presenting errors
      # as useful HTML pages
      set :show_exceptions, false
    end
    # end

    get '/' do
      'Hola World!'
    end

    # get '/tasks' do
    # Task.all.to_json
    # end

    # {GET Lists}
    get '/tasks' do
      Task.select(:id, :name, :task_status, :is_bug, :title, :bug_type, :target_date, :created_at, :updated_at, :approved, :developer_id, :reviewer_id).all.to_json
    end

    get '/timestamps/task' do
      Task.select(:id, :created_at, :updated_at).all.to_json
    end

    get '/people' do
      Person.all.to_json
    end

    get '/timestamps/person' do
      Person.select(:id, :created_at, :updated_at).all.to_json
    end
    # }{GET Lists}

    # {Person CRUD}
    post '/person' do
      @person = Person.new(params[:person].except('id', 'unread_objects_count', 'documents_count', 'tasks_count', 'created_at', 'updated_at'))
      redirect 'person/#{@person.id}' if @person.save
    end

    get '/person/:id' do
      Person.where(['id = ?', params[:id]]).first.to_json
    end

    put '/person/:id' do
      @person = Person.find(params[:id])
      if @person.update(params[:person].except('unread_objects_count', 'documents_count', 'tasks_count'))
        redirect 'person/#{@person.id}'
      end
    end

    delete '/person/:id' do
      @person = Person.find(params[:id])
      if @person.destroy
        content_type :json
        { Status: 'Person with ID ' + params[:id] + ' destroyed' }.to_json
      end
    end
    # }{Person CRUD}

    # {Task CRUD}
    post '/task' do
      @task = Task.new(params[:task].except('id', 'documents', 'created_at', 'updated_at'))
      redirect 'task/#{@task.id}' if @task.save
    end

    get '/task/:id' do
      Task.includes(:documents).where(['id = ?', params[:id]]).first.as_json(include: [:documents]).to_json
    end

    put '/task/:id' do
      @task = Task.find(params[:id])
      redirect "task/#{@task.id}" if @task.update(params[:task])
    end

    delete '/task/:id' do
      @task = Task.find(params[:id])
      if @task.destroy
        content_type :json
        { Status: 'Task with ID ' + params[:id] + ' destroyed' }.to_json
      end
    end

    get '/timestamp/task/:id' do
      Task.select(:id, :created_at, :updated_at).where(['id = ?', params[:id]]).first.to_json
    end
    # }{Task CRUD}

    # {GET Document}
    get '/document/:id' do
      Document.where(['id = ?', params[:id]]).first.to_json
    end

    get '/task/:id/documents' do
      # Task.where(['id = ?', params[:id]]).document.all.to_json
      # Document.where(['task_id = ?', params[:id]]).all.to_json
      Task.find(params[:id]).documents.all.to_json
    end
    # }{GET Document}

    error ActiveRecord::RecordNotFound do
      status 404
    end

  end

  @channel = EventMachine::Channel.new
  @users = {}
  @messages = []

  EventMachine::WebSocket.start(host: '0.0.0.0', port: 8080) do |ws|
    ws.onopen do
      puts 'New Connection Opened' + ws.inspect
      # Subscribe the new user to the channel with the callback function for the push action
      new_user = @channel.subscribe { |msg| ws.send msg }

      # Add the new user to the user list
      @users[ws.object_id] = new_user

      # Push the last messages to the user
      @messages.each do |message|
        ws.send message
      end

      # Broadcast the notification to all users
      @channel.push ({
        'nickname' => '',
        'message' => 'New user joined. #{@users.length} users in chat',
        'timestamp' => timestamp }.to_json)
    end

    ws.onmessage do |msg|
      puts 'Message received ' + msg
      # Add the timestamp to the message
      message = JSON.parse(msg).merge('timestamp' => timestamp).to_json

      # append the message at the end of the queue
      @messages << message
      @messages.shift if @messages.length > 10

      # Broadcast the message to all users connected to the channel
      @channel.push message
    end

    ws.onclose do
      puts 'Websocket Closed' + ws.inspect
      @channel.unsubscribe(@users[ws.object_id])
      @users.delete(ws.object_id)

      # Broadcast the notification to all users
      @channel.push ({
        'nickname' => '',
        'message' => "One user left. #{@users.length} users in chat",
        'timestamp' => timestamp }.to_json)
    end
  end

  Thin::Server.start App, '0.0.0.0', 4567
end
