using System;
using System.Collections.Generic;
using Windows.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.ComponentModel;
using System.Globalization;
using Windows.UI;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.UI.Popups;
using Clinic.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Clinic
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public  sealed partial class MainPage3 : Clinic.Common.LayoutAwarePage
    {

        public MainPage3()
        {
            this.InitializeComponent();
        }
      

    




    }

    public class NameValueItem
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public string Info { get; set; }

        public DateTime date { get; set; }

        public OutLookEvent myOutLookEvent { get; set; }

        public OutLookEvent[] myOutLookEvent2 { get; set; }

        public WorkHours workHours { get; set; }
        public string Color1 { get; set; }
        public string Color2 { get; set; }
        public string Color { get; set; }
        public int Height1 { get; set; }
        public int Height2 { get; set; }
        public Thickness Border { get; set; }

    }

    class SpecificGridView : GridView
    {
        protected override void PrepareContainerForItemOverride(
          DependencyObject element, object item)
        {
            // being lazy because I bound to an anonymous data type  
            dynamic lateBoundItem = item;

            int sizeFactor = (int)lateBoundItem.Width;

            element.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, sizeFactor);
            //element.SetValue(VariableSizedWrapGrid.RowSpanProperty, sizeFactor);

            base.PrepareContainerForItemOverride(element, item);

                
        }

    }  

    public class NameValueItem3:INotifyPropertyChanged
    {
           public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
              
            }
        }

       
        public string Color1 { get; set; }
        public string Color2 { get; set; }

        public bool timeItem { get; set; }

        public WeekDay weekDay { get; set; }

        public TimeSpan time { get; set; }

        public Thickness Border { get; set; }
        public string timeStr { get; set; }

        public OutLookEvent myOutLookEvent { get; set; }
        public OutLookEvent[]  myOutLookEvent2 { get; set; }
        public WorkHours workHours { get; set; }

        private Appointment appointment1;
        public Appointment Appointment1 { get { return appointment1; } set { appointment1 = value;
        OnPropertyChanged("Appointment1");
        }
        }
        //public Appointment appointment2 { get; set; }
        private Appointment appointment2;
        public Appointment Appointment2
        {
            get { return appointment2; }
            set
            {
                appointment2 = value;
                OnPropertyChanged("Appointment2");
            }
        }
        //public Appointment appointment3 { get; set; }
        private Appointment appointment3;
        public Appointment Appointment3
        {
            get { return appointment3; }
            set
            {
                appointment3 = value;
                OnPropertyChanged("Appointment3");
            }
        }
       // public Appointment appointment4 { get; set; }

        private Appointment appointment4;
        public Appointment Appointment4
        {
            get { return appointment4; }
            set
            {
                appointment4 = value;
                OnPropertyChanged("Appointment4");
            }
        }

        private Invitation invitation1;
        public Invitation Invitation1
        {
            get { return invitation1; }
            set
            {
                invitation1 = value;
                OnPropertyChanged("Invitation1");
            }
        }

        private Invitation invitation2;
        public Invitation Invitation2
        {
            get { return invitation2; }
            set
            {
                invitation2 = value;
                OnPropertyChanged("Invitation2");
            }
        }

        private Invitation invitation3;
        public Invitation Invitation3
        {
            get { return invitation3; }
            set
            {
                invitation3 = value;
                OnPropertyChanged("Invitation3");
            }
        }

        private Invitation invitation4;
        public Invitation Invitation4
        {
            get { return invitation4; }
            set
            {
                invitation4 = value;
                OnPropertyChanged("Invitation4");
            }
        }

        public int Width { get; set; }
        public int Width1 { get;  set; }
        public int Width2 { get; set; }

        private Visibility enableTextBlockes;
        public Visibility EnableTextBlockes {
            get { return enableTextBlockes; }
            set{
                enableTextBlockes = value;
                OnPropertyChanged("EnableTextBlockes");
            }
        }

        private Visibility enableTextBlockes2;
        public Visibility EnableTextBlockes2
        {
            get { return enableTextBlockes2; }
            set
            {
                enableTextBlockes2 = value;
                OnPropertyChanged("EnableTextBlockes2");
            }
        }

        private Visibility enableTextBlockes3;
        public Visibility EnableTextBlockes3
        {
            get { return enableTextBlockes3; }
            set
            {
                enableTextBlockes3 = value;
                OnPropertyChanged("EnableTextBlockes3");
            }
        }

        private Visibility enableTextBlockes4;
        public Visibility EnableTextBlockes4
        {
            get { return enableTextBlockes4; }
            set
            {
                enableTextBlockes4 = value;
                OnPropertyChanged("EnableTextBlockes4");
            }
        }

     

        private Visibility enableTextBoxes ;
        public Visibility EnableTextBoxes {
            get { return enableTextBoxes; }
            set{enableTextBoxes = value ;
        OnPropertyChanged("EnableTextBoxes");} }


        private Visibility enableTextBoxes2;
        public Visibility EnableTextBoxes2
        {
            get { return enableTextBoxes2; }
            set
            {
                enableTextBoxes2 = value;
                OnPropertyChanged("EnableTextBoxes2");
            }
        }

        private Visibility enableTextBoxes3;
        public Visibility EnableTextBoxes3
        {
            get { return enableTextBoxes3; }
            set
            {
                enableTextBoxes3 = value;
                OnPropertyChanged("EnableTextBoxes3");
            }
        }

        private Visibility enableTextBoxes4;
        public Visibility EnableTextBoxes4
        {
            get { return enableTextBoxes4; }
            set
            {
                enableTextBoxes4 = value;
                OnPropertyChanged("EnableTextBoxes4");
            }
        }


        private Visibility enableButton;
        public Visibility EnableButton
        {
            get { return enableButton; }
            set { enableButton = value;
            OnPropertyChanged("EnableButton");
            }
        }

        private Visibility enableButton2;
        public Visibility EnableButton2
        {
            get { return enableButton2; }
            set
            {
                enableButton2 = value;
                OnPropertyChanged("EnableButton2");
            }
        }

        private Visibility enableButton3;
        public Visibility EnableButton3
        {
            get { return enableButton3; }
            set
            {
                enableButton3 = value;
                OnPropertyChanged("EnableButton3");
            }
        }

        private Visibility enableButton4;
        public Visibility EnableButton4
        {
            get { return enableButton4; }
            set
            {
                enableButton4 = value;
                OnPropertyChanged("EnableButton4");
            }
        }





        private Visibility enableDelButton;
        public Visibility EnableDelButton
        {
            get { return enableDelButton; }
            set
            {
                enableDelButton = value;
                OnPropertyChanged("EnableDelButton");
            }
        }

        private Visibility enableDelButton2;
        public Visibility EnableDelButton2
        {
            get { return enableDelButton2; }
            set
            {
                enableDelButton2 = value;
                OnPropertyChanged("EnableDelButton2");
            }
        }

        private Visibility enableDelButton3;
        public Visibility EnableDelButton3
        {
            get { return enableDelButton3; }
            set
            {
                enableDelButton3 = value;
                OnPropertyChanged("EnableDelButton3");
            }
        }

        private Visibility enableDelButton4;
        public Visibility EnableDelButton4
        {
            get { return enableDelButton4; }
            set
            {
                enableDelButton4 = value;
                OnPropertyChanged("EnableDelButton4");
            }
        }

        private DateTime date;
        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }
    }

    public class WeekDay
    {
        public string Year {get; set;}
        public string Month { get; set; }

        public int Day { get; set; }
    }
}
