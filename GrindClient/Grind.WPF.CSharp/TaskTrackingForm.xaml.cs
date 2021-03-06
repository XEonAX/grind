﻿using System;
using System.Collections.Generic;
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
using System.Linq;
using Grind.Common;
using System.IO;
using System.Windows.Markup;
using System.Collections.ObjectModel;
namespace Grind.WPF.CSharp
{
    /// <summary>
    /// Interaction logic for TaskTrackingFormWPF.xaml
    /// </summary>
    public partial class TaskTrackingForm : UserControl
    {
        public TaskTrackingForm()
        {
            this.InitializeComponent();
        }
        Session Session;
        bool UserChange;
        internal void SetSession(Session session)
        {
            Session = session;
        }
        public void FillPeopleDropDown(ObservableCollection<Person> People)
        {
            cbExecutor.Items.Clear();
            cbExecutor.Items.Add("Executor");
            cbExecutor.Items.Add("-------------------------");
            cbReviewer.Items.Clear();
            cbReviewer.Items.Add("Reviewer");
            cbReviewer.Items.Add("-------------------------");
            People
                .Select(x => x.name)
                .ToList()
                .ForEach(
                    item =>
                    {
                        cbExecutor.Items.Add(item);
                        cbReviewer.Items.Add(item);
                    }
                );
        }

        public void FillFormfromTask(Task task)
        {
            if (task == null)
                return;
            UserChange = false;
            txtName.Text = task.name;
            txtTitle.Text = task.title;
            //dtpOpen.IsEnabled = true;
            //dtpAnalysis.IsEnabled = false;
            //dtpReview.IsEnabled = false;
            //dtpCorrection.IsEnabled = false;
            //dtpPromotion.IsEnabled = false;
            //dtpCollection.IsEnabled = false;
            //dtpClosed.IsEnabled = false;
            dtpOpen.SelectedDate = task.open_date.Date;
            dtpAnalysis.SelectedDate = task.analysis_date.Date;
            dtpReview.SelectedDate = task.review_date.Date;
            dtpCorrection.SelectedDate = task.correction_date.Date;
            dtpPromotion.SelectedDate = task.promotion_date.Date;
            dtpCollection.SelectedDate = task.collection_date.Date;
            dtpClosed.SelectedDate = task.closed_date.Date;
            dtpTargetDate.SelectedDate = task.target_date.Date;
            if (task.is_bug)
                rbBug.IsChecked = true;
            else
                rbHL.IsChecked = true;
            switch (task.bug_type)
            {
                case eBugType.HMA:
                    rbHMA.IsChecked = true;
                    break;
                case eBugType.BackLog:
                    rbBacklog.IsChecked = true;
                    break;
                case eBugType.CRITSIT:
                    rbCRITSIT.IsChecked = true;
                    break;
                case eBugType.Others:
                    rbOthers.IsChecked = true;
                    break;
                default:
                    break;
            }
            sldTaskStatus.IsEnabled = true;
            sldTaskStatus.Value = (int)task.task_status;
            cbExecutor.Text = Globals.GetPersonNameFromId(task.developer_id);
            cbReviewer.Text = Globals.GetPersonNameFromId(task.reviewer_id);
            //if (task.description == null) task.description = "";
            //if (task.analysis == null) task.analysis = new object();
            //if (task.review == null) task.review = new object();

            //rtbDescription.Document = Globals.XamlPackageToWPFRichText(task.description);
            //rtbAnalysis.Document = Globals.XamlPackageToWPFRichText(task.analysis);
            //rtbReview.Document = Globals.XamlPackageToWPFRichText(task.review);

            //rtbDescription.Rtf = task.description;
            //rtbAnalysis.Rtf = task.analysis;
            //rtbReview.Rtf = task.review;

            txtDescription.Text = task.description;
            txtAnalysis.Text = task.analysis;
            txtReview.Text = task.review;
            UserChange = true;
        }

        public void BuildTaskfromTaskFilledForm(ref Task task)
        {
            task.name = txtName.Text;
            task.title = txtTitle.Text;

            if ((bool)rbBug.IsChecked)
                task.is_bug = true;
            else
                task.is_bug = false;

            if ((bool)rbHMA.IsChecked)
                task.bug_type = eBugType.HMA;
            else if ((bool)rbCRITSIT.IsChecked)
                task.bug_type = eBugType.CRITSIT;
            else if ((bool)rbBacklog.IsChecked)
                task.bug_type = eBugType.BackLog;
            else
                task.bug_type = eBugType.Others;
            task.task_status = (eTaskStatus)sldTaskStatus.Value;
            task.approved = (bool)chkApproval.IsChecked;
            task.developer_id = Globals.GetPersonIdFromName(cbExecutor.SelectedItem.ToString());
            task.reviewer_id = Globals.GetPersonIdFromName(cbReviewer.SelectedItem.ToString());
            task.open_date = ((DateTime)dtpOpen.SelectedDate).Date.Add(TimeZoneInfo.Local.BaseUtcOffset);
            task.analysis_date = ((DateTime)dtpAnalysis.SelectedDate).Date.Add(TimeZoneInfo.Local.BaseUtcOffset);
            task.review_date = ((DateTime)dtpReview.SelectedDate).Date.Add(TimeZoneInfo.Local.BaseUtcOffset);
            task.correction_date = ((DateTime)dtpCorrection.SelectedDate).Date.Add(TimeZoneInfo.Local.BaseUtcOffset);
            task.promotion_date = ((DateTime)dtpPromotion.SelectedDate).Date.Add(TimeZoneInfo.Local.BaseUtcOffset);
            task.collection_date = ((DateTime)dtpCollection.SelectedDate).Date.Add(TimeZoneInfo.Local.BaseUtcOffset);
            task.closed_date = ((DateTime)dtpClosed.SelectedDate).Date.Add(TimeZoneInfo.Local.BaseUtcOffset);
            task.target_date = ((DateTime)dtpTargetDate.SelectedDate).Date.Add(TimeZoneInfo.Local.BaseUtcOffset);
            //task.description = Globals.WPFRichTextToXamlPackage(rtbDescription.Document);
            //task.analysis = Globals.WPFRichTextToXamlPackage(rtbAnalysis.Document);
            //task.review = Globals.WPFRichTextToXamlPackage(rtbReview.Document);



            task.description = txtDescription.Text;
            task.analysis = txtAnalysis.Text;
            task.review = txtReview.Text;

            //task.description = rtbDescription.Rtf;
            //task.analysis = rtbAnalysis.Rtf;
            //task.review = rtbReview.Rtf;
        }

        public void EnableForm()
        {
            gbTaskOptions.IsEnabled = true;
            gbTaskStatus.IsEnabled = true;
            txtName.IsReadOnly = false;
            txtTitle.IsReadOnly = false;
            //rtbAnalysis.IsReadOnly = false;
            //rtbDescription.IsReadOnly = false;
            //rtbReview.IsReadOnly = false;
            txtAnalysis.IsReadOnly = false;
            txtDescription.IsReadOnly = false;
            txtReview.IsReadOnly = false;


            //rtbAnalysis.ReadOnly = false;
            //rtbDescription.ReadOnly = false;
            //rtbReview.ReadOnly = false;
        }

        public void DisableForm()
        {
            gbTaskOptions.IsEnabled = false;
            gbTaskStatus.IsEnabled = false;
            txtName.IsReadOnly = true;
            txtTitle.IsReadOnly = true;
            //rtbAnalysis.IsReadOnly = true;
            //rtbDescription.IsReadOnly = true;
            //rtbReview.IsReadOnly = true;
            txtAnalysis.IsReadOnly = true;
            txtDescription.IsReadOnly = true;
            txtReview.IsReadOnly = true;
            //rtbAnalysis.ReadOnly = true;
            //rtbDescription.ReadOnly = true;
            //rtbReview.ReadOnly = true;
        }

    }
    #region  Aesthetics
    public class SliderLeftRightMarginConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (System.Convert.ToDouble(value) < 10.0) value = 0.0;
            return new Thickness((System.Convert.ToDouble(value) / 2) - 5, 0, (System.Convert.ToDouble(value) / 2) - 5, 0);
        }
        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    public class IsEnabledConverter : IMultiValueConverter
    {
        public object Convert(object[] value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (System.Convert.ToInt32(value[0])>=Grid.GetColumn((DatePicker)value[1]))
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
        public object[] ConvertBack(object value, System.Type[] targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    #endregion


}