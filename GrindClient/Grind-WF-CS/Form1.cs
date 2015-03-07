﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RestSharp;
using RestSharp.Deserializers;
using RestSharp.Serializers;
using System.Reflection;


namespace Grind_WF_CS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        public SortableBindingList<ClientTask> TaskList;
        Task RetrievedTask;
        bool UserChange = false;

        private void Form1_Load(object sender, System.EventArgs e)
        {
            dGridTasks.AutoGenerateColumns = false;
            dGridTasks.DataSource = TaskList;
            new Controllers("http://localhost:4567/");
            new Globals();
        }



        private void dGridTasks_SelectionChanged(System.Object sender, System.EventArgs e)
        {
            Controllers.ReadTask(TaskList[dGridTasks.CurrentRow.Index].id,ref RetrievedTask);
            FillTaskTrackingForm(RetrievedTask);
        }

        private void FillTaskTrackingForm(Task task)
        {

            UserChange = false;
            txtName.Text = task.name;
            txtTitle.Text = task.title;
            dtpOpen.Enabled = true;
            dtpAnalysis.Enabled = false;
            dtpReview.Enabled = false;
            dtpCorrection.Enabled = false;
            dtpPromotion.Enabled = false;
            dtpCollection.Enabled = false;
            dtpClosed.Enabled = false;
            dtpOpen.Value = task.open_date;
            dtpAnalysis.Value = task.analysis_date;
            dtpReview.Value = task.review_date;
            dtpCorrection.Value = task.correction_date;
            dtpPromotion.Value = task.promotion_date;
            dtpCollection.Value = task.collection_date;
            dtpClosed.Value = task.closed_date;
            if (task.is_bug)
                rbBug.Checked = true;
            else
                rbHL.Checked = true;
            switch (task.bug_type)
            {
                case eBugType.HMA:
                    rbHMA.Checked = true;
                    break;
                case eBugType.BackLog:
                    rbBacklog.Checked = true;
                    break;
                case eBugType.CRITSIT:
                    rbCRITSIT.Checked = true;
                    break;
                case eBugType.Others:
                    rbOthers.Checked = true;
                    break;
                default:
                    break;
            }
            trbTaskStatus.Enabled = true;
            trbTaskStatus.Value = (int)task.task_status;
            cobExecutor.Text = Globals.People.Find(x => x.id == task.developer_id).name;
            cobReviewer.Text = Globals.People.Find(x => x.id == task.reviewer_id).name;
            rtbDescription.Rtf = task.description;
            rtbAnalysis.Rtf = task.analysis;
            rtbReview.Rtf = task.review;

            UserChange = true;

        }

        private void BuildTaskFromForm(ref Task task)
        {

            task.name = txtName.Text;
            task.title = txtTitle.Text;

            if (rbBug.Checked)
                task.is_bug = true;
            else
                task.is_bug = false;

            if (rbHMA.Checked)
                task.bug_type = eBugType.HMA;
            else if (rbCRITSIT.Checked)
                task.bug_type = eBugType.CRITSIT;
            else if (rbBacklog.Checked)
                task.bug_type = eBugType.BackLog;
            else
                task.bug_type = eBugType.Others;
            task.task_status = (eTaskStatus)trbTaskStatus.Value;
            task.approved = cbApproved.Checked;
            task.developer_id = Globals.People.Find(x => x.name == cobExecutor.SelectedItem.ToString()).id;
            task.reviewer_id = Globals.People.Find(x => x.name == cobReviewer.SelectedItem.ToString()).id;
            task.open_date = dtpOpen.Value;
            task.analysis_date = dtpAnalysis.Value;
            task.review_date = dtpReview.Value;
            task.correction_date = dtpCorrection.Value;
            task.promotion_date = dtpPromotion.Value;
            task.collection_date = dtpCollection.Value;
            task.closed_date = dtpClosed.Value;
            task.description = rtbDescription.Rtf;
            task.analysis = rtbAnalysis.Rtf;
            task.review = rtbReview.Rtf;
        
        }

        private void trbTaskStatus_ValueChanged(object sender, EventArgs e)
        {
            int enableddtpCount = trbTaskStatus.Value;
            foreach (Control ctl in tlpTaskStatus.Controls.Cast<Control>().OrderBy(c => c.TabIndex))
            {
                if (ctl is DateTimePicker)
                {
                    if (enableddtpCount >= 0)
                    {
                        ctl.Enabled = true;
                        enableddtpCount--;
                    }
                    else
                    {
                        ctl.Enabled = false;
                        enableddtpCount--;
                    }
                }

            }
        }

        private void tsmiNew_Click(object sender, EventArgs e)
        {
            if (tsmiNew.Text == "New")
            {
                //New Task mode
                tsmiNew.Text = "Save";
                tsmiUpdate.Text = "Cancel";
                dGridTasks.Enabled = false;
                FillTaskTrackingForm(new Task());

            }
            else if (tsmiNew.Text == "Save")
            {
                //New Task Save
                Task task = new Task();
                BuildTaskFromForm(ref task);
                Controllers.CreateTask(ref task);
                Controllers.ReadTasks(ref TaskList);

                tsmiNew.Text = "New";
                tsmiUpdate.Text = "Update";
                dGridTasks.Enabled = true;

                FillTaskTrackingForm(task);
            }
            else if (tsmiNew.Text == "Cancel" && tsmiUpdate.Text == "Update")
            { 
                //Update Mode Cancel
                Controllers.ReadTasks(ref TaskList);
                tsmiNew.Text = "New";
                tsmiUpdate.Text = "Update";
                dGridTasks.Enabled = true;
                           
            }
            
        }
        
        private void tsmiRefresh_Click(System.Object sender, System.EventArgs e)
        {
            Controllers.ReadPeople(ref Globals.People);
            Controllers.ReadTasks(ref TaskList);
            cobExecutor.Items.AddRange(Globals.People.Select(x => x.name).ToArray());
            cobReviewer.Items.AddRange(Globals.People.Select(x => x.name).ToArray());

            dGridTasks.DataSource = TaskList;
        }

        private void rtb_TextChanged(object sender, EventArgs e)
        {
            if (sender is RichTextBox)
            {
                if (!UserChange && ((RichTextBox)sender).Rtf.Length > Math.Pow(2, 20))
                {
                    ((RichTextBox)sender).BackColor = Color.Red;
                }
                else
                {
                    ((RichTextBox)sender).BackColor = Color.White;
                }
            }
        }

        private void dGridTasks_RowCountChanged(object sender, DataGridViewRowsAddedEventArgs e)
        {
            RowCountChanged(sender, e);
        }

        private void dGridTasks_RowCountChanged(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            RowCountChanged(sender, e);
        }

        private void RowCountChanged(object sender, EventArgs e)
        {
            if (sender is DataGridView)
                if (((DataGridView)sender).RowCount == 0)
                    tsmiUpdate.Enabled = false;
                else
                    tsmiUpdate.Enabled = true;


        }
    }



}












