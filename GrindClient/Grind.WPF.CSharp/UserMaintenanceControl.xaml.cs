﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Grind.Common;

namespace Grind.WPF.CSharp
{
    /// <summary>
    /// Interaction logic for UserMaintenanceControl.xaml
    /// </summary>
    public partial class UserMaintenanceControl : UserControl
    {
        public UserMaintenanceControl()
        {
            InitializeComponent();
        }
        Session Session;
        public Person person = new Person();
        
        bool Refreshed = false;

        internal void SetSession(Session session)
        {
        Session=session;
        }
        private void FillForm(Person person)
        {
            if (person == null) return;
            txtName.Text = person.name;
            txtTrigram.Text = person.trigram;
            txtIntObjId.Text = person.internal_object_id;
            chkActive.IsChecked = person.active;
            cbLevel.SelectedIndex = (int)person.level;
            txtPassword.Text = person.password;
        }
        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((txtName.Text.Trim().Length == 0) || (txtTrigram.Text.Trim().Length == 0) || (txtIntObjId.Text.Trim().Length == 0) || (cbLevel.SelectedIndex==-1))
            {
                btnAdd.IsEnabled= false;
                btnUpdate.IsEnabled = false;
            }
            else
            {
                btnAdd.IsEnabled = true;
                if (lbPeople.SelectedIndex != -1)
                    btnUpdate.IsEnabled = true;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            person = new Person();
            person.name = txtName.Text;
            person.trigram = txtTrigram.Text;
            person.internal_object_id = txtIntObjId.Text;
            person.active = (bool)chkActive.IsChecked;
            person.level = (eLevel)cbLevel.SelectedIndex;
            person.password = txtPassword.Text;
            Session.Controllers.CreatePerson(person);
            btnRefresh_Click(sender,e);
            Refreshed = true;
            lbPeople.SelectedIndex = lbPeople.Items.Count - 1;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            int OldSelectedIndex = lbPeople.SelectedIndex;
            person.name = txtName.Text;
            person.trigram = txtTrigram.Text;
            person.internal_object_id = txtIntObjId.Text;
            person.active = (bool)chkActive.IsChecked;
            person.level = (eLevel)cbLevel.SelectedIndex;
            person.password = txtPassword.Text;
            Session.Controllers.UpdatePerson(person);
            btnRefresh_Click(sender,e);
            Refreshed = true;
            lbPeople.SelectedIndex = OldSelectedIndex;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            int OldSelectedIndex = lbPeople.SelectedIndex;
            if (lbPeople.SelectedIndex >= 0)
            {
                person = Globals.People[lbPeople.SelectedIndex];
                Session.Controllers.DeletePerson(person);
            }
            btnRefresh_Click(sender, e);
            Refreshed = true;
            if (OldSelectedIndex > lbPeople.Items.Count - 1)
                OldSelectedIndex = lbPeople.Items.Count - 1;
            lbPeople.SelectedIndex = OldSelectedIndex;

        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Refreshed = false;
            Session.ReadPeople();
            lbPeople.ItemsSource = Globals.DictIdToPersonName.Values.ToList();
            //if (OldSelectedIndex > lbPeople.Items.Count-1)
            //    OldSelectedIndex = lbPeople.Items.Count - 1;
            //else if ((OldSelectedIndex < lbPeople.Items.Count-1) && (OldSelectedIndex == -1))
            //{
            //    OldSelectedIndex = 0;
            //}
            //else 
            if (sender != null) Refreshed = true;
            if (lbPeople.SelectedIndex == -1)
            {
                FillForm(new Person());
                btnUpdate.IsEnabled = false;
                btnDelete.IsEnabled = false;
            }
            else
            {
                btnUpdate.IsEnabled = true;
                btnDelete.IsEnabled = true;
            }
            Refreshed = true;

        }

        private void lbPeople_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Refreshed)
            {
                person = Globals.People[lbPeople.SelectedIndex];
                FillForm(person);
            }
        }
    }
}
