using Clinic.Common;
using Clinic.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Clinic
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewPatient : Clinic.Common.LayoutAwarePage, INotifyPropertyChanged 
    {
        public static int flip = 0;
        public static int selectedMonthNumber;




        private string selectedMonth;
        public string SelectedMonth
        {
            get { return selectedMonth; }
            set
            {
                selectedMonth = value;
                OnPropertyChanged("SelectedMonth");
            }
        }

        private string selectedYear;
        public string SelectedYear
        {
            get { return selectedYear; }
            set
            {
                selectedYear = value;
                OnPropertyChanged("SelectedYear");
            }
        }

        public DateTime CurrentDate;

        private PatientViewModel patientViewModel;
       
        
        public NewPatient()
        {
            this.InitializeComponent();
            
            patientViewModel = new PatientViewModel();

            DataContext = patientViewModel;
            patientViewModel.createPatient();
            patientViewModel.createPatientHistory();
            patientViewModel.createPatientDocument();
            VerticalPatientsView.DataContext = this;
            
            MonthNameTextBlock.DataContext = this;
            YearNameTextBlock.DataContext = this;
            CurrentDate = DateTime.Now;
           
            this.logIn.Text = Connection.UserName;
            try
            {
                Uri uri = new Uri(Connection.ImageUri, UriKind.Absolute);
                ImageSource imgSource = new BitmapImage(uri);
                myImage.Source = imgSource;
            }
            catch (Exception e)
            {
                new MessageDialog(e.Message).ShowAsync();
            }


            fillGridView();

            patientViewModel.PropertyChanged += (sender, e) =>
            {
                if (String.Equals(e.PropertyName, "RefreshFlag")) //added new patient
                {
                    fillGridView();
                    patientViewModel.createPatient();
                    patientViewModel.createPatientHistory();
                    patientViewModel.createPatientDocument();
                }
            };

            patientViewModel.PropertyChanged += (sender, e) =>
            {
                if (String.Equals(e.PropertyName, "RefreshFlag2")) // added new info or doc
                {
                    fillPatientInfoView();
                }
            };

            this.PropertyChanged += (sender, e) =>
                {
                    if (String.Equals(e.PropertyName, "SelectedPatient")) //changed selected patient
                    {
                        fillPatientInfoView();
                    }
                };

           // DataContext = this;
            PatientEncounterGrid.DataContext = this;
            AddEncounterView.DataContext = this;
            SetupCommands();


            //this.PropertyChanged += (sender, e) =>
            //{
            //    if (String.Equals(e.PropertyName, "SelectedNVI"))
            //    {
                    
            //        //if (!SelectedNVI.Equals(OldSelectedNVI))
            //            countClick = 0;
            //        //OldSelectedNVI = SelectedNVI;
            //    }
            //};
        }

        private NameValueItem2 oldSelectedNVI = null;
        public NameValueItem2 OldSelectedNVI 
        {
            get { return oldSelectedNVI; }
            set
            {
                oldSelectedNVI = value;
                OnPropertyChanged("OldSelectedNVI");
            }
        }


        static List<User> userList = new List<User>();
        Patient pa = null;
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {

            base.OnNavigatedTo(e);
            userList = await User.ReadUsersList();
            if (e.Parameter != null)
            {
                string patientID = (string)e.Parameter;

                //if (!(patientID).Equals(0))
                //{
                pa = Scheduling.getPatient(patientID);
                //}
                await Task.Delay(1000);

                SelectedPatient = pList.Where(p => p.PatientID.Equals(pa.PatientID)).ToList().First();
            }
            //VerticalPatientsView.SelectedItem = SelectedPatient;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        static List<Appointment> appList = null;
        static List<PatientEncounter> peList =null;
        bool stay = false;
        public async void fillCalendar()
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 2);
           
            Windows.Globalization.Calendar calendar = new Windows.Globalization.Calendar();

            List<NameValueItem2> dList = new List<NameValueItem2>();
            try
            {
                if ((!SelectedMonth.Equals(calendar.MonthAsSoloString()) || !SelectedYear.Equals(calendar.Year.ToString())) && flip == 0)
                    stay = true;
            }
            catch
            {

            }

            int noOfDaysInTheSelectedMonth = 0;
            int shift = 0;
            string firstDay = String.Empty;

            if (flip == 0)
            {
                if (!stay)
                {
                    SelectedMonth = calendar.MonthAsSoloString();

                    SelectedYear = calendar.Year.ToString();
                }
                // firstDay = new DateTime(Convert.ToInt32(SelectedYear), DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month, 1).ToString("dddd");
            }

            if (flip == -1)
            {
                int temp = DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month;
                if (temp == 1)
                {
                    temp = 12;
                }
                else
                {
                    temp--;
                }
                SelectedMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(temp);
                if (temp != 12)
                {
                    //SelectedYear = calendar.Year.ToString();
                }
                else
                {
                    int tempYear = Convert.ToInt32(SelectedYear);
                    tempYear--;
                    SelectedYear = tempYear.ToString();

                }


            }

            if (flip == 1)
            {
                {
                    int temp = DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month;
                    if (temp == 12)
                    {
                        temp = 1;
                    }
                    else
                    {
                        temp++;
                    }
                    SelectedMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(temp);
                    if (temp != 1)
                    {
                    }
                    else
                    {
                        int tempYear = Convert.ToInt32(SelectedYear);
                        tempYear++;
                        SelectedYear = tempYear.ToString();

                    }


                }
            }


            noOfDaysInTheSelectedMonth = DateTime.DaysInMonth(Convert.ToInt32(SelectedYear), DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month);
            firstDay = new DateTime(Convert.ToInt32(SelectedYear), DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month, 1).ToString("dddd");

            switch (firstDay)
            {
                case "Sunday": shift = 0;
                    break;
                case "Monday": shift = 1;
                    break;
                case "Tuesday": shift = 2;
                    break;
                case "Wednesday": shift = 3;
                    break;
                case "Thursday": shift = 4;
                    break;
                case "Friday": shift = 5;
                    break;
                case "Saturday": shift = 6;
                    break;
            }

            while (shift > 0)
            {
                dList.Add(new NameValueItem2());
                shift--;
            }
            appList = await Appointment.ReadAppointmentsList();
            appList = appList.Where(ap => ap.PatientID == SelectedPatient.PatientID).ToList();
            try
            {
                peList = (await PatientEncounter.ReadPatientEncountersList()).Where(pe => (getAppointment(pe.AppointmentId)).PatientID == SelectedPatient.PatientID).ToList();
            }
            catch
            {

            }
                List<PatientHistory> pList = await patientViewModel.patientsInfoTable.Where(
                ph => (ph.PatientId == SelectedPatient.PatientID 
                && 
                ph.InfoDate > (new DateTime(Convert.ToInt32(SelectedYear), DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month, 1))
                &&
                ph.InfoDate < (new DateTime(Convert.ToInt32(SelectedYear), DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month, noOfDaysInTheSelectedMonth))
                )).ToListAsync();
            //Where(p => p.FName == ComboPatient).Select(patient => patient.PatientID).ToListAsync()).First();
            for (int i = 1; i <= noOfDaysInTheSelectedMonth; i++)
            {
                DateTime loopDate = new DateTime(Convert.ToInt32(SelectedYear), DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month, i);

                PatientHistory temp;
                try
                {
                    temp = pList.Where(ph => ph.InfoDate.ToString("MMMM dd, yyyy") == loopDate.ToString("MMMM dd, yyyy")).ToList().First();
                }
                catch
                {
                  temp  = new PatientHistory();
                }
                Appointment app;

                try
                {
                    app = appList.Where(ap => ap.Date.Date.Equals(loopDate.Date.Date)).ToList().First();
                }
                catch
                {
                   app = new Appointment();
                }
                PatientEncounter pee;

                try
                {
                    pee = peList.Where(pe => (getAppointment(pe.AppointmentId)).Date.Date.Equals(loopDate.Date.Date)).ToList().First();
                }
                catch
                {
                    pee = new PatientEncounter();
                }


                dList.Add(new NameValueItem2 { Value = i.ToString(), date = new DateTime(Convert.ToInt32(SelectedYear), DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month, i), pHistory = temp, Color = ((!String.IsNullOrEmpty(temp.PatientId)) ? "Pink" : (pee.AppointmentId != null) ? "LightBlue" : (!String.IsNullOrEmpty(app.PatientID) ? "#BFB5DF" : "White")), appointment = app, patientEncounter = pee, Width1 = ((!String.IsNullOrEmpty(temp.PatientId)) || (String.IsNullOrEmpty(temp.PatientId) && String.IsNullOrEmpty(app.PatientID)) ? 98 : 0), Width2 = (!String.IsNullOrEmpty((app.PatientID)) && String.IsNullOrEmpty(temp.PatientId ) && pee.AppointmentId == null ? 98 : 0), Width3 = (pee.AppointmentId != null ? 98 : 0) });
                    
               
            }

            MonthGridView.ItemsSource = dList;
            flip = 0;

            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 2);
           
        }

        private void previousMonthButton_Click(object sender, RoutedEventArgs e)
        {
            flip = -1;
            fillCalendar();

        }

        private void nextMonthButton_Click(object sender, RoutedEventArgs e)
        {
            flip = 1;
            fillCalendar();
        }

        private NameValueItem2 selectedNVI = null;
        public NameValueItem2 SelectedNVI
        {
            get { return selectedNVI; }
            set
            {
                selectedNVI = value;
                OnPropertyChanged("SelectedNVI");
            }
        }
        int countClick = 0;
        private async void MonthGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            
            SelectedNVI = ((NameValueItem2)e.ClickedItem);

              //  if(countClick==1){

            if (((NameValueItem2)e.ClickedItem).pHistory != null || ((NameValueItem2)e.ClickedItem).appointment != null || ((NameValueItem2)e.ClickedItem).patientEncounter != null)
            {

                Window currentWindow = Window.Current;

                Point point, myPoint;

                try
                {
                    point = currentWindow.CoreWindow.PointerPosition;
                }
                catch (UnauthorizedAccessException)
                {
                    myPoint = new Point(double.NegativeInfinity, double.NegativeInfinity);
                }

                Rect bounds = currentWindow.Bounds;

                myPoint = new Point(DipToPixel(point.X - bounds.X), DipToPixel(point.Y - bounds.Y));

                LightDismissAnimatedPopup.VerticalOffset = myPoint.Y - 400;

                LightDismissAnimatedPopup.HorizontalOffset = myPoint.X - 800;


                
                if (!String.IsNullOrEmpty(((NameValueItem2)e.ClickedItem).pHistory.PatientId ))
                {
                    LightDismissAnimatedPopupTextBlock.Text = "Cholestrol Level is " + ((NameValueItem2)e.ClickedItem).pHistory.Cholestrol;
                    LightDismissAnimatedPopupTextBlock3.Text = "Blood Pressure Level is " + ((NameValueItem2)e.ClickedItem).pHistory.BloodPressure;
                    LightDismissAnimatedPopupTextBlock4.Text = "Temprature is " + ((NameValueItem2)e.ClickedItem).pHistory.Temperature;

                    LightDismissAnimatedPopupTextBlock2.Text = ((NameValueItem2)e.ClickedItem).date.ToString();
                    if (!LightDismissAnimatedPopup.IsOpen) { LightDismissAnimatedPopup.IsOpen = true; }
                }
                else if(((NameValueItem2)e.ClickedItem).patientEncounter.AppointmentId!=null)
                {
                    countClick++;
                    if (countClick == 1)
                    {
                        Appointment app = getAppointment(SelectedNVI.patientEncounter.AppointmentId);
                        User u = getUser(app.UserID);
                        //  Patient p = getPatient(app.PatientID);
                        LightDismissAnimatedPopupTextBlock.Text = "Doctor is " + u.LName + ", " + u.FName;
                        LightDismissAnimatedPopupTextBlock3.Text = "Complaint is" + app.Complaint;//((NameValueItem2)e.ClickedItem).appointment.Complaint;
                        LightDismissAnimatedPopupTextBlock4.Text = "Diagnostics is " + SelectedNVI.patientEncounter.Diagnostics; //app.Date.ToString("MMMM dd, yyyy");
                        LightDismissAnimatedPopupTextBlock5.Text = "Drug is " + SelectedNVI.patientEncounter.Drugs;
                        LightDismissAnimatedPopupTextBlock6.Text = "Notes are " + SelectedNVI.patientEncounter.Notes;
                        LightDismissAnimatedPopupTextBlock7.Text = "Date is " + app.Date.ToString("MMMM dd, yyyy");


                        LightDismissAnimatedPopupTextBlock2.Text = ((NameValueItem2)e.ClickedItem).appointment.TimeFrom.Hours + ":" + ((NameValueItem2)e.ClickedItem).appointment.TimeFrom.Minutes;
                        if (!LightDismissAnimatedPopup.IsOpen) { LightDismissAnimatedPopup.IsOpen = true; }
                    }
                    else
                    {
                        BindEncounter = ((NameValueItem2)e.ClickedItem).patientEncounter;
                            ShowAddEncounter = Visibility.Visible;
                    
                            
                            countClick = 0;
                    }

                }
                else if (!String.IsNullOrEmpty(((NameValueItem2)e.ClickedItem).appointment.PatientID ) )
                {  countClick++;
                    if(countClick==1){
                    
                    User u  = (await User.getUser(((NameValueItem2)e.ClickedItem).appointment.UserID));
                    LightDismissAnimatedPopupTextBlock.Text = "Doctor is " +u.LName+", "+u.FName ;
                    LightDismissAnimatedPopupTextBlock3.Text = "Complaint is" + ((NameValueItem2)e.ClickedItem).appointment.Complaint;
                    LightDismissAnimatedPopupTextBlock4.Text = "Date is " + ((NameValueItem2)e.ClickedItem).appointment.Date.ToString("MMMM dd, yyyy") ;

                    LightDismissAnimatedPopupTextBlock2.Text = ((NameValueItem2)e.ClickedItem).appointment.TimeFrom.Hours + ":" + ((NameValueItem2)e.ClickedItem).appointment.TimeFrom.Minutes;
                    if (!LightDismissAnimatedPopup.IsOpen) { LightDismissAnimatedPopup.IsOpen = true; }
                    }
                   else { // countClick ==2
                        if(!String.IsNullOrEmpty(SelectedNVI.appointment.PatientID) && SelectedNVI.patientEncounter.AppointmentId ==null){
                            PatientEncounter pe = new PatientEncounter();
                            pe.AppointmentId = SelectedNVI.appointment.Id;
                            BindEncounter = pe;
                            ShowAddEncounter = Visibility.Visible;
                    
                            }
                            countClick = 0;
                       }
                
                }
                

                 }
                //}
              
                    
        }
        private static double DipToPixel(double dip)
        {
            return (dip * DisplayProperties.LogicalDpi) / 96.0;
        }


        // Handles the Click event on the Button within the simple Popup control and simply closes it.
        private void CloseAnimatedPopupClicked(object sender, RoutedEventArgs e)
        {
            if (LightDismissAnimatedPopup.IsOpen) { LightDismissAnimatedPopup.IsOpen = false; }
        }

        private Random _random = new Random();

        public static  List<Patient> pList;
        public static List<Patient> newList;
        private Patient selectedPatient;
        public Patient SelectedPatient
        {
            get { return selectedPatient; }
            set { selectedPatient = value;
            OnPropertyChanged("SelectedPatient");
            }
        }

        async void  fillGridView()
        {
            pList = await patientViewModel.getList();
            this.VerticalPatientsView.ItemsSource = pList;
            

           //this.VerticalPatientsView.SelectionChanged += VerticalPatientsView_SelectionChanged;
           // this.VerticalPatientsView.ItemClick+= ItemView_ItemClick;
           // this.VerticalPatientsView.RightTapped += VerticalPatientsView_RightTapped;
            try
            {
                SelectedPatient = pList.First();
            }
            catch{
            }

        }
       

        //Add New Patient
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
           // Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 2);
                       
            patientViewModel.ShowAddPatientTemplate = true;
            patientViewModel.NotDisplayPatientForm = false;
            patientViewModel.DisplayRegistrationColor = "Gray";


        }

        //private async void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        //{

        //    selectedPatient = ((Patient)e.ClickedItem);
           
          
            
        //    fillPatientInfoView();
         
        //}

        private  void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {

            
           // // The y coordinate of the tapped position
           // double y = e.GetPosition((UIElement)sender).Y;

           // // The x coordinate of the tapped position
           // double x = e.GetPosition((UIElement)sender).X;

           // GridView patientsContainer = sender as GridView;
           // patientsContainer.Measure(new Size(400, 90));
           // Size size = patientsContainer.DesiredSize;

           // // The tapped item's x index
           // int itemX =  (int)(x / size.Width);

           // // The tapped item's y index
           // int itemY = (int)(y / size.Height);

           // // Get the index of tapped item
           // int index = (int)(itemY * (int)(VerticalPatientsView.ActualWidth / size.Width)) + itemX;
           //// int index = (int)(itemX * (int)(VerticalPatientsView.ActualHeight / size.Height)) + itemY;

           // if (index < VerticalPatientsView.Items.Count)
           // {
           //     selectedPatient = VerticalPatientsView.Items[index] as Patient;
                
           //         // Get the tapped item
           //         GridViewItem tappedItem = patientsContainer.ItemContainerGenerator.ContainerFromIndex(index) as GridViewItem;
           //         patientsContainer.SelectionMode = ListViewSelectionMode.Single;
           //         tappedItem.IsSelected = true;
               
           // }
            SelectedPatient = ((Patient)e.ClickedItem);



           // fillPatientInfoView();

        }

      
        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            patientViewModel.ShowAddPatientInfoTemplate = true;
            patientViewModel.NotDisplayPatientForm = false;
            patientViewModel.DisplayRegistrationColor = "Gray";
        }


        //refresh
        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            fillGridView();
        }

        private void filterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

           //  pList = await patientViewModel.getList();
           // this.VerticalPatientsView.ItemsSource = pList;
          
             newList = pList.Where(patient => (patient.FName).Contains(filterTextBox.Text)).ToList();  //from patient in pList where patient.FName 
             this.VerticalPatientsView.ItemsSource = newList;
             SelectedPatient = newList.First();
             //this.VerticalPatientsView.SelectedItem = SelectedPatient;
             //this.VerticalPatientsView.DataContext = this;
        }


        public class NameValueItem
        {
            public string Name { get; set; }
            public double Value { get; set; }
        }

        private void AppBarButton_Click_3(object sender, RoutedEventArgs e)
        {
            patientViewModel.ShowAddPatientDocumentTemplate= true;
            patientViewModel.NotDisplayPatientForm = false;
            patientViewModel.DisplayRegistrationColor = "Gray";
        }

        private async void docGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
          //  await new MessageDialog("I was clicked").ShowAsync();

            //var uri = new Uri(((PatientDocument)e.ClickedItem).ImageUri);
            //var success = await Windows.System.Launcher.LaunchUriAsync(uri);    

            StorageFolder folder = ApplicationData.Current.LocalFolder;
            string fileName =   (((PatientDocument)e.ClickedItem).ImageUri).Substring(((PatientDocument)e.ClickedItem).ImageUri.LastIndexOf("/")+1);

            bool notExist = false;
            try
            {

                var file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
                var downloader = new BackgroundDownloader();
                var download = downloader.CreateDownload(
                    new Uri(((PatientDocument)e.ClickedItem).ImageUri),
                    file);

                var res = await download.StartAsync();
                var success = await Windows.System.Launcher.LaunchFileAsync(file);
                notExist = true;

            }
            catch(Exception ee)
            {

                notExist = false;
            }

            if (!notExist)
            {
                var file = await folder.GetFileAsync(fileName);
                var success = await Windows.System.Launcher.LaunchFileAsync(file);
            }
        }

        private async void fillPatientInfoView()
        {

            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 2);
            myGrid.Opacity = 0.5;

            patientViewModel.ShowChart = false;
            //patientViewModel.SelectedPatient = ((Patient)e.ClickedItem);

            //selectedPatientTextBlock3.Text = "item click";
           // selectedTextBlock.Text = selectedPatient.LName + ", " + selectedPatient.FName;
            upperStackPanel.DataContext = SelectedPatient;
            List<NameValueItem> items = new List<NameValueItem>();

            List<NameValueItem> items2 = new List<NameValueItem>();
            List<NameValueItem> items3 = new List<NameValueItem>();

            List<PatientHistory> PhList = await(patientViewModel.patientsInfoTable.Where(ph => ph.PatientId == SelectedPatient.PatientID ).OrderByDescending(d => d.InfoDate).Take(12).ToListAsync());
            PhList = PhList.OrderBy(d => d.InfoDate).ToList();
            for (int i = 0; i < PhList.Count; i++)
            {
                try
                {
                    //items2.Add(new NameValueItem { Name = PhList.ElementAt(i).InfoDate.Year.ToString(), Value = Convert.ToInt32(PhList.ElementAt(i).Cholestrol) });
                    items2.Add(new NameValueItem { Name = (PhList.ElementAt(i).InfoDate.Year.ToString() + "/" + PhList.ElementAt(i).InfoDate.Month.ToString() + "/" + PhList.ElementAt(i).InfoDate.Day.ToString()), Value = Convert.ToDouble(PhList.ElementAt(i).Cholestrol) });

                }
                catch
                {

                }
                try
                {
                    //items.Add(new NameValueItem { Name = PhList.ElementAt(i).InfoDate.Year.ToString(), Value = Convert.ToInt32(PhList.ElementAt(i).Temperature) });
                    items.Add(new NameValueItem { Name = (PhList.ElementAt(i).InfoDate.Year.ToString() + "/" + PhList.ElementAt(i).InfoDate.Month.ToString() + "/" + PhList.ElementAt(i).InfoDate.Day.ToString()), Value = Convert.ToDouble(PhList.ElementAt(i).Temperature) });

                }
                catch
                {
                }

                try
                {
                    //items3.Add(new NameValueItem { Name = PhList.ElementAt(i).InfoDate.Year.ToString(), Value = Convert.ToInt32(PhList.ElementAt(i).BloodPressure) });
                    items3.Add(new NameValueItem { Name = (PhList.ElementAt(i).InfoDate.Year.ToString() + "/" + PhList.ElementAt(i).InfoDate.Month.ToString() + "/" + PhList.ElementAt(i).InfoDate.Day.ToString()), Value = Convert.ToDouble(PhList.ElementAt(i).Pulse) });

                }
                catch
                {

                }

            }

            
            
            ((LineSeries)this.LineChart.Series[0]).ItemsSource = items2;
            ((LineSeries)this.LineChart3.Series[0]).ItemsSource = items;
            ((LineSeries)this.LineChart2.Series[0]).ItemsSource = items3;




            List<PatientDocument> PdList = await(patientViewModel.patientsDocumentTable.Where(pd => pd.PatientID == SelectedPatient.PatientID).ToListAsync());

            docGridView.ItemsSource = PdList;

            patientViewModel.ShowChart = true;
            fillCalendar();

            myGrid.Opacity = 1;
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 2);
            

        }

        //private  async void docGridView_PointerPressed(object sender, PointerRoutedEventArgs e)
        //{
        //    await new MessageDialog("I was clicked").ShowAsync();
        //    //var uri = new Uri(((PatientDocument)sender).ImageUri);
        //    //var success = await Windows.System.Launcher.LaunchUriAsync(uri);    
        //}

        //private void AppBarButton_Click_4(object sender, RoutedEventArgs e) // Add Patient Encounter
        //{

        //}

        private PatientEncounter bindEncounter;
        public PatientEncounter BindEncounter
        {
            get { return bindEncounter; }
            set
            {
                bindEncounter = value;
                OnPropertyChanged("BindEncounter");
            }
        }

        private Visibility showAddEncounter = Visibility.Collapsed;
        public Visibility ShowAddEncounter 
        {
            get { return showAddEncounter; }
            set
            {
                showAddEncounter = value;
                OnPropertyChanged("ShowAddEncounter");
            }
        }

        public static Appointment getAppointment(string id)
        {
            Appointment app = appList.Where(ap => ap.Id.Equals(id)).ToList().First();
            return app;
        }

        public DelegateCommand Add_Patient_Encounter { get; private set; }
        public DelegateCommand Cancel_Patient_Encounter { get; private set; }
        public DelegateCommand Delete_Patient_Encounter { get; private set; }

        private void SetupCommands()
        {
            Add_Patient_Encounter = new DelegateCommand(async () =>
            { MessageDialog mm;

            if (string.IsNullOrWhiteSpace(BindEncounter.Diagnostics) )
                {
                    //show message
                    mm = new MessageDialog("Please fill in all the fields");
                    await mm.ShowAsync();
                    return;
                }
            if (BindEncounter.Id==0)
            {

                PatientEncounter.InsertNewPatientEncounter(BindEncounter);
            }
            else
            {
                PatientEncounter.UpdatePatientEncounter(BindEncounter);
                await Task.Delay(1000);
            }
            ShowAddEncounter = Visibility.Collapsed;
            BindEncounter = null;
            fillCalendar();

            }, true);

            Cancel_Patient_Encounter = new DelegateCommand(async () =>
            {


                BindEncounter = null;
                ShowAddEncounter = Visibility.Collapsed;

            }, true);

            Delete_Patient_Encounter = new DelegateCommand(async () =>
            {


                PatientEncounter.DeletePatientEncounter(BindEncounter);
                BindEncounter = null;
                ShowAddEncounter = Visibility.Collapsed;
                fillCalendar();

            }, true);


        }

        public static User getUser(string id)
        {
            User user =userList.Where(p => p.UserId.Equals(id)).ToList().First();
            return user;
        }
        public static Patient getPatient(string id)
        {
            Patient pp = pList.Where(p => p.PatientID.Equals(id)).ToList().First();
            return pp;
        }
    }

    public class NameValueItem2
    {
        //public string Name { get; set; }
      
        public string Value { get; set; }

        //public string Info { get; set; }
        public PatientHistory pHistory { get; set; }

        public DateTime date { get; set; }

        public string Color { get; set; }

        public Appointment appointment { get; set; }

        public PatientEncounter patientEncounter { get; set; }
        public int Width1 { get; set; }

        public int Width2 { get; set; }

        public int Width3 { get; set;}
        //patient History
    }
}
