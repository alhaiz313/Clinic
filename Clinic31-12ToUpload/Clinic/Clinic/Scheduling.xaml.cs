using Clinic.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
//using WindowsInput;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Clinic
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Scheduling : Common.LayoutAwarePage, INotifyPropertyChanged
    {
        public static int appIncrease = 0;
        Appointment app = null;
        public Scheduling()
        {
            oldChanged = Changed;
            this.InitializeComponent();
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
            DoctorsComboBox.DataContext = this;

            this.PropertyChanged += (sender, e) =>
            {
                if (String.Equals(e.PropertyName, "SelectedDoctor"))
                {
                    fillWeeklyCalendar();
                }
            };

         


            this.PropertyChanged += (sender, e) =>
            {
                if (String.Equals(e.PropertyName, "SelectedNVI"))
                {
                    try
                    {
                        OldSelectedNVI.EnableTextBlockes = Visibility.Visible;
                        OldSelectedNVI.EnableTextBoxes = Visibility.Collapsed;
                        OldSelectedNVI.EnableButton = Visibility.Collapsed;
                        OldSelectedNVI.EnableDelButton = Visibility.Collapsed;

                        OldSelectedNVI.EnableTextBlockes2 = Visibility.Visible;
                        OldSelectedNVI.EnableTextBoxes2 = Visibility.Collapsed;
                        OldSelectedNVI.EnableButton2 = Visibility.Collapsed;
                        OldSelectedNVI.EnableDelButton2 = Visibility.Collapsed;

                        OldSelectedNVI.EnableTextBlockes3 = Visibility.Visible;
                        OldSelectedNVI.EnableTextBoxes3 = Visibility.Collapsed;
                        OldSelectedNVI.EnableButton3 = Visibility.Collapsed;
                        OldSelectedNVI.EnableDelButton3 = Visibility.Collapsed;

                        OldSelectedNVI.EnableTextBlockes4 = Visibility.Visible;
                        OldSelectedNVI.EnableTextBoxes4 = Visibility.Collapsed;
                        OldSelectedNVI.EnableButton4 = Visibility.Collapsed;
                        OldSelectedNVI.EnableDelButton4 = Visibility.Collapsed;

                        ShowAddAppointment = Visibility.Collapsed;
                    }
                    catch
                    {

                    }
                    if (!SelectedNVI.Equals(OldSelectedNVI))
                        countClick = 0;
                    OldSelectedNVI = SelectedNVI;
                }
            };


            MonthNameTextBlock2.DataContext = this;
            YearNameTextBlock2.DataContext = this;
            MonthNameTextBlock3.DataContext = this;
            YearNameTextBlock3.DataContext = this;
            CurrentDate = DateTime.Now;
            VerticalPatientsView.DataContext = this;
            Output2.DataContext = this;

            AddAppointmentView.DataContext = this;
            DataContext = this;
           app = (new Appointment());
          
            SetupCommands();
            fillDoctors();
            fillGridView();

        }

        private User selectedDoctor;
        public User SelectedDoctor
        {
            get { return selectedDoctor; }
            set
            {
                selectedDoctor = value;
                OnPropertyChanged("SelectedDoctor");
                //  new MessageDialog(selectedDoctor.FName).ShowAsync();
            }
        }

        async void fillGridView()
        {
            pList = await Patient.ReadPatientsList();// await patientViewModel.getList();
            // List<String> tempList = pList.Select(u => u.FName).ToList();
            this.VerticalPatientsView.ItemsSource = pList;
            //this.VerticalPatientsView.ItemsSource = tempList;


            //this.VerticalPatientsView.SelectionChanged += VerticalPatientsView_SelectionChanged;
            this.VerticalPatientsView.ItemClick += VerticalPatientsView_ItemClick;
            // this.VerticalPatientsView.RightTapped += VerticalPatientsView_RightTapped;

            SelectedPatient = pList.First();
        }




        public static List<User> doctorsList = new List<User>();
        List<string> tempList;
        async void fillDoctors()
        {
            doctorsList = await User.ReadUsersList();
            tempList = doctorsList.Select(u => (u.LName + ", " + u.FName)).ToList();
            DoctorsComboBox.ItemsSource = tempList;
            DoctorsComboBox.SelectedIndex = 0;

            Doc = (string)DoctorsComboBox.SelectedValue;
            string ln = Doc.Substring(0, doc.IndexOf(","));
            string fn = Doc.Substring(doc.IndexOf(",") + 2);
            try
            {
                User temp = doctorsList.Where(u => (u.LName.Equals(ln) && u.FName.Equals(fn))).ToList().First();
                //whList = (await WorkHours.ReadListAsync()).Where(w => w.EmployeeId == temp.UserId).ToList();
                //appList = (await Appointment.ReadAppointmentsList()).Where(a => a.UserID == temp.UserId).ToList();
                //invList = (await Invitation.ReadInvitationsList()).Where(i => i.ToUserId == temp.UserId).ToList();

                SelectedDoctor = temp;
            }
            catch
            {

            }

            await Dispatcher.RunAsync(CoreDispatcherPriority.Low, async() =>
            {
                while (true)
                {
                    await Task.Delay(1000);
                    if (oldChanged != Changed)
                    {
                        fillWeeklyCalendar();
                        oldChanged = Changed;
                    }
                }
            });

           
        }

        public static List<Patient> pList;

        private Patient selectedPatient;
        public Patient SelectedPatient
        {
            get { return selectedPatient; }
            set
            {
                selectedPatient = value;
                OnPropertyChanged("SelectedPatient");
            }
        }
        private void filterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            //pList = await Patient.ReadPatientsList();..
            // this.VerticalPatientsView.ItemsSource = pList;
            List<Patient> newList = pList.Where(patient => (patient.FName).Contains(filterTextBox.Text)).ToList();  //from patient in pList where patient.FName 
            this.VerticalPatientsView.ItemsSource = newList;
        }

        private void VerticalPatientsView_ItemClick(object sender, ItemClickEventArgs e)
        {
            VerticalPatientsView.DataContext = this;
            SelectedPatient = (Patient)e.ClickedItem;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));

            }
        }

        private string doc;
        public string Doc
        {
            get { return doc; }
            set
            {

                doc = value;
                OnPropertyChanged("Doc");
            }
        }


        private async void DoctorsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Doc = (string)DoctorsComboBox.SelectedValue;
            string ln = Doc.Substring(0, doc.IndexOf(","));
            string fn = Doc.Substring(doc.IndexOf(",") + 2);
            try
            {//&& u.FName.Equals(fn)
                SelectedDoctor = doctorsList.Where(u => (u.LName.Equals(ln) && u.FName.Equals(fn))).ToList().First();
                //await new MessageDialog(SelectedDoctor.FName).ShowAsync();
            }
            catch
            {

            }
        }


        private string leftMonth;
        public string LeftMonth
        {
            get { return leftMonth; }
            set
            {
                leftMonth = value;
                OnPropertyChanged("LeftMonth");
            }
        }

        public static bool backward = false;

        private string leftYear;
        public string LeftYear
        {
            get { return leftYear; }
            set
            {
                leftYear = value;
                OnPropertyChanged("LeftYear");
            }
        }

        private string rightMonth;
        public string RightMonth
        {
            get { return rightMonth; }
            set
            {
                rightMonth = value;
                OnPropertyChanged("RightMonth");
            }
        }

        private string rightYear;
        public string RightYear
        {
            get { return rightYear; }
            set
            {
                rightYear = value;
                OnPropertyChanged("RightYear");
            }
        }

        private int selectedWeek;
        public int SelectedWeek
        {
            get { return selectedWeek; }
            set
            {
                selectedWeek = value;
                OnPropertyChanged("SelectedWeek");
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

        public static int flip2 = 0;
        public static int selectedMonthNumber;

        public DateTime CurrentDate;

        Windows.Globalization.Calendar calendar = new Windows.Globalization.Calendar();
        List<WorkHours> whList = new List<WorkHours>();
        List<Appointment> appList = new List<Appointment>();
        List<Invitation> invList = new List<Invitation>();

        public WeekDay ReachedDayInWeeklyCalendar = null;
        public WeekDay StartDayInWeeklyCalendar = null;


        List<WeekDay> weekList = new List<WeekDay>(7);
        bool stay = false;
        public async void fillWeeklyCalendar()
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 2);
            myGrid.Opacity = 0.5;
            appIncrease++;


            whList = (await WorkHours.ReadListAsync()).Where(w => w.EmployeeId == SelectedDoctor.UserId).ToList();
            appList = (await Appointment.ReadAppointmentsList()).Where(a => a.UserID == SelectedDoctor.UserId).ToList();
            invList = (await Invitation.ReadInvitationsList()).Where(i => i.ToUserId == SelectedDoctor.UserId).ToList();

            if (SelectedWeek != 0 && flip2 == 0)
            {
                stay = true;
            }
            if (!stay)
            {
                LeftMonth = String.Empty;
                LeftYear = String.Empty;
                RightMonth = String.Empty;
                RightYear = String.Empty;
            }
            //   whList = (await WorkHours.ReadListAsync()).Where(w => w.EmployeeId == Connection.User.UserId).ToList();

            Windows.Globalization.Calendar calendar = new Windows.Globalization.Calendar();


            List<NameValueItem3> dList = new List<NameValueItem3>();

           // List<WeekDay> weekList = new List<WeekDay>(7);

            if (flip2 == 0)
            {
                if (String.IsNullOrEmpty(SelectedMonth))
                    SelectedMonth = calendar.MonthAsSoloString();
                if (String.IsNullOrEmpty(SelectedYear))
                    SelectedYear = calendar.Year.ToString();



                if (!stay)
                    SelectedWeek = 1;

            }
            if (flip2 == -1)
            {


                if (SelectedWeek == 1)
                {
                    SelectedWeek = 4;

                    if ((DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month) == 1)
                    {
                        SelectedMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(12);
                        SelectedYear = ((Convert.ToInt32(SelectedYear)) - 1).ToString();
                    }
                    else
                    {
                        SelectedMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month - 1);
                    }

                }
                else
                {
                    SelectedWeek--;
                }




            }

            if (flip2 == 1)
            {



                if (SelectedWeek == 4)
                {
                    SelectedWeek = 1;

                    if ((DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month) == 12)
                    {
                        SelectedMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(1);
                        SelectedYear = ((Convert.ToInt32(SelectedYear)) + 1).ToString();
                    }
                    else
                    {
                        SelectedMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month + 1);
                    }

                }
                else
                {
                    SelectedWeek++;
                }
            }
            int noOfDaysInTheSelectedMonth = 0;
            int shift = 0;
            string firstDay = String.Empty;


            if (!stay)
                weekList.Clear();

            if (SelectedWeek == 1)
            {
                //int noOfDaysInTheSelectedMonth = 0;
                //int shift = 0;
                //string firstDay = String.Empty;

                if (!stay)
                {
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

                int NumberOfPreviousMonthDays;
                string previuosMonth;
                string previousMonthYear;

                if (shift != 0)
                {

                    if (DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month != 1)
                    {
                        NumberOfPreviousMonthDays = DateTime.DaysInMonth(Convert.ToInt32(SelectedYear), DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month - 1);
                        previuosMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month - 1);//
                        previousMonthYear = SelectedYear;
                    }
                    else
                    {
                        NumberOfPreviousMonthDays = DateTime.DaysInMonth(Convert.ToInt32(SelectedYear) - 1, 12);
                        previuosMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(12);// CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month - 1);//
                        previousMonthYear = (Convert.ToInt32(SelectedYear) - 1).ToString();

                    }

                    for (int i = shift - 1; i >= 0; i--)
                    {
                        weekList.Add(new WeekDay { Year = previousMonthYear, Month = previuosMonth, Day = NumberOfPreviousMonthDays - i });
                    }

                    for (int i = 1; i <= 7 - shift; i++)
                    {
                        weekList.Add(new WeekDay { Year = SelectedYear, Month = SelectedMonth, Day = i });
                        if (i == 7 - shift)
                        {
                            ReachedDayInWeeklyCalendar = weekList.Last();
                            StartDayInWeeklyCalendar = weekList.First();

                        }
                    }

                    LeftMonth = previuosMonth;
                    LeftYear = previousMonthYear;

                    RightMonth = SelectedMonth;
                    //  MonthNameTextBlock3.Text =SelectedMonth  ;
                    RightYear = selectedYear;
                }
                if (shift == 0)
                {
                    for (int i = 1; i <= 7; i++)
                    {

                        weekList.Add(new WeekDay { Year = SelectedYear, Month = SelectedMonth, Day = i });
                        if (i == 7)
                        {
                            ReachedDayInWeeklyCalendar = weekList.Last();
                            StartDayInWeeklyCalendar = weekList.First();
                        }
                    }

                    LeftMonth = SelectedMonth;
                    LeftYear = SelectedYear;

                    RightMonth = String.Empty;
                    RightYear = String.Empty;
                }

             

            }
            }
            else if (SelectedWeek == 2 || SelectedWeek == 3 || SelectedWeek == 4)
            {
                if (!stay)
                {
                    if (!backward)
                    {
                        for (int i = ReachedDayInWeeklyCalendar.Day + 1; i <= (ReachedDayInWeeklyCalendar.Day + 7); i++)
                        {
                            weekList.Add(new WeekDay { Year = SelectedYear, Month = SelectedMonth, Day = i });
                            if (i == ReachedDayInWeeklyCalendar.Day + 7)
                            {
                                ReachedDayInWeeklyCalendar = weekList.Last();
                                StartDayInWeeklyCalendar = weekList.First();
                                break;
                            }
                        }



                    }

                    else //backward true
                    {

                        if (StartDayInWeeklyCalendar.Day - 1 != 0)
                        {
                            for (int i = StartDayInWeeklyCalendar.Day - 7; i <= (StartDayInWeeklyCalendar.Day - 1); i++)
                            {

                                weekList.Add(new WeekDay { Year = SelectedYear, Month = SelectedMonth, Day = i });
                                if (i == StartDayInWeeklyCalendar.Day - 1)
                                {
                                    ReachedDayInWeeklyCalendar = weekList.Last();
                                    StartDayInWeeklyCalendar = weekList.First();
                                    break;
                                }


                            }
                        }
                        else
                        {
                            if (DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month != 1)
                            {
                                noOfDaysInTheSelectedMonth = DateTime.DaysInMonth(Convert.ToInt32(SelectedYear), DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month);
                                //SelectedMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month - 1);//
                                //SelectedYear = SelectedYear;
                            }
                            else
                            {
                                noOfDaysInTheSelectedMonth = DateTime.DaysInMonth(Convert.ToInt32(SelectedYear) - 1, 12);
                                //SelectedMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(12);// CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month - 1);//
                                //SelectedYear = (Convert.ToInt32(SelectedYear) - 1).ToString();

                            }

                            for (int i = noOfDaysInTheSelectedMonth - 6; i <= (noOfDaysInTheSelectedMonth - 0); i++)
                            {

                                weekList.Add(new WeekDay { Year = SelectedYear, Month = SelectedMonth, Day = i });
                                if (i == (noOfDaysInTheSelectedMonth - 0))
                                {
                                    ReachedDayInWeeklyCalendar = weekList.Last();
                                    StartDayInWeeklyCalendar = weekList.First();
                                    break;
                                }


                            }



                        }



                    }


                    LeftMonth = SelectedMonth;
                    LeftYear = SelectedYear;
                    RightMonth = String.Empty;
                    RightYear = String.Empty;

                }

            }




            for (int i = 0; i < 192; i++)
            {
                OutLookEvent[] myOEvent2 = null;
                OutLookEvent myOEvent = null;
                WorkHours wh = null;

                Appointment app1 = null;
                Appointment app2 = null;
                Appointment app3 = null;
                Appointment app4 = null;
                Invitation inv1 = null;
                Invitation inv2 = null;
                Invitation inv3 = null;
                Invitation inv4 = null;

                if (i % 8 == 0)
                {
                    dList.Add(new NameValueItem3() { timeItem = true, time = new TimeSpan(i / 8, 0, 0), Width = 1, timeStr = (new TimeSpan(i / 8, 0, 0)).ToString().Substring(0, (new TimeSpan(i / 8, 0, 0)).ToString().LastIndexOf(":")) + "_", Color1 = "#BFB5DF", Width1 = 45 }); //( new TimeSpan(i / 8, 0, 0)).ToString()
                }

                else
                {

                    DateTime loopDate = new DateTime(Convert.ToInt32(weekList.ElementAt((i % 8) - 1).Year),
                           DateTime.ParseExact(weekList.ElementAt((i % 8) - 1).Month, "MMMM", CultureInfo.CurrentCulture).Month,
                           weekList.ElementAt((i % 8) - 1).Day);

                    //try
                    //{
                    //    //  myOEvent = eventList.Where(e => (e.start_time.ToString("MMMM dd, yyyy")).Equals(loopDate.ToString("MMMM dd, yyyy"))).ToArray().First();
                    //    //   myOEvent = eventList.Where(e => (e.start_time.ToString("MMMM dd, yyyy")).Equals(loopDate.ToString("MMMM dd, yyyy")) && (e.start_time.TimeOfDay.Equals(new TimeSpan(i / 8, 0, 0)) || e.end_time.TimeOfDay >= (new TimeSpan(i / 8, 0, 0)))).ToArray().First();
                    //    //   myOEvent = eventList.Where(e => (e.start_time.ToString("MMMM dd, yyyy")).Equals(loopDate.ToString("MMMM dd, yyyy")) && (new TimeSpan(e.end_time.TimeOfDay.Hours,0,0) >= (new TimeSpan(i / 8, 0, 0)) || e.isAllDay == true)).ToArray().First();
                    //    // myOEvent = eventList.Where(e => (e.start_time.ToString("MMMM dd, yyyy")).Equals(loopDate.ToString("MMMM dd, yyyy")) && (((new TimeSpan(e.end_time.TimeOfDay.Hours, 0, 0) > (new TimeSpan(i / 8, 0, 0))) && (new TimeSpan(e.start_time.TimeOfDay.Hours, 0, 0) <= (new TimeSpan(i / 8, 0, 0)))) || e.isAllDay == true || ((new TimeSpan(e.start_time.TimeOfDay.Hours, 0, 0) <= (new TimeSpan(i / 8, 0, 0)))&&(TimeSpan)(e.end_time - e.start_time) <= new TimeSpan(1, 0, 0)))).ToArray().First();
                    //    myOEvent = eventList.Where(e => (e.start_time.ToString("MMMM dd, yyyy")).Equals(loopDate.ToString("MMMM dd, yyyy")) && (((new TimeSpan(e.end_time.TimeOfDay.Hours, 0, 0) > (new TimeSpan(i / 8, 0, 0))) && (new TimeSpan(e.start_time.TimeOfDay.Hours, 0, 0) <= (new TimeSpan(i / 8, 0, 0)))) || e.isAllDay == true || ((new TimeSpan(e.start_time.TimeOfDay.Hours, 0, 0) == (new TimeSpan(i / 8, 0, 0))) && ((TimeSpan)(e.end_time - e.start_time)).Hours <= 1))).ToArray().First();
                    //    myOEvent2 = eventList.Where(e => (e.start_time.ToString("MMMM dd, yyyy")).Equals(loopDate.ToString("MMMM dd, yyyy")) && (((new TimeSpan(e.end_time.TimeOfDay.Hours, 0, 0) > (new TimeSpan(i / 8, 0, 0))) && (new TimeSpan(e.start_time.TimeOfDay.Hours, 0, 0) <= (new TimeSpan(i / 8, 0, 0)))) || e.isAllDay == true || ((new TimeSpan(e.start_time.TimeOfDay.Hours, 0, 0) == (new TimeSpan(i / 8, 0, 0))) && ((TimeSpan)(e.end_time - e.start_time)).Hours <= 1))).ToArray();
                    //}
                    //catch
                    //{

                    //}

                    try
                    {
                        wh = whList.Where(w => ((w.From.ToString("MMMM dd, yyyy").Equals(loopDate.ToString("MMMM dd, yyyy")) && w.From.TimeOfDay.Hours <= (i / 8) && w.To.ToString("MMMM dd, yyyy").Equals(loopDate.ToString("MMMM dd, yyyy")) && w.To.TimeOfDay.Hours >= (i / 8)) || (w.From.ToString("MMMM dd, yyyy").Equals(loopDate.ToString("MMMM dd, yyyy")) && w.From.TimeOfDay.Hours <= i / 8 && w.To.Date.Equals(loopDate.AddDays(1))) || (w.From.Date.Equals(loopDate.AddDays(-1).Date) && w.To.Date.Equals(loopDate.Date) && w.To.TimeOfDay.Hours >= (i / 8)) || (w.TimeOffFrom.Date.Equals(loopDate.Date) && w.TimeOffFrom.TimeOfDay.Hours <= (i / 8) && w.TimeOffTo.TimeOfDay.Hours >= (i / 8)))).ToArray().First();
                        if (!String.IsNullOrEmpty(wh.TimeOffReason))
                            wh = null;
                    }
                    catch
                    {

                    }

                    try
                    {
                        app1 = appList.Where(a => (a.Date.Date.Equals(loopDate.Date) && (a.TimeFrom.Hours == (i / 8)) && (a.TimeFrom.Minutes == 0) && (a.TimeTo.Minutes == 15))).ToList().First();
                    }
                    catch
                    {
                        app1 = new Appointment();
                        // app1.Complaint = "hiii2";

                    }

                    try
                    {
                        app2 = appList.Where(a => (a.Date.Date.Equals(loopDate.Date) && (a.TimeFrom.Hours == (i / 8)) && (a.TimeFrom.Minutes == 15) && (a.TimeTo.Minutes == 30))).ToList().First();
                    }
                    catch
                    {
                        app2 = new Appointment();

                    }

                    try
                    {
                        app3 = appList.Where(a => (a.Date.Date.Equals(loopDate.Date) && (a.TimeFrom.Hours == (i / 8)) && (a.TimeFrom.Minutes == 30) && (a.TimeTo.Minutes == 45))).ToList().First();
                    }
                    catch
                    {
                        app3 = new Appointment();

                    }

                    try
                    {
                        app4 = appList.Where(a => (a.Date.Date.Equals(loopDate.Date) && (a.TimeFrom.Hours == (i / 8)) && (a.TimeFrom.Minutes == 45) && (a.TimeTo.Minutes == 0))).ToList().First();
                    }
                    catch
                    {
                        app4 = new Appointment();


                    }

                    try
                    {
                        inv1 = invList.Where(a => (a.Date.Date.Equals(loopDate.Date) && (a.TimeFrom.Hours == (i / 8)) && (a.TimeFrom.Minutes == 0) && (a.TimeTo.Minutes == 15))).ToList().First();
                    }
                    catch
                    {

                        inv1 = new Invitation();

                    }

                    try
                    {
                        inv2 = invList.Where(a => (a.Date.Date.Equals(loopDate.Date) && (a.TimeFrom.Hours == (i / 8)) && (a.TimeFrom.Minutes == 15) && (a.TimeTo.Minutes == 30))).ToList().First();
                    }
                    catch
                    {

                        inv2 = new Invitation();

                    }

                    try
                    {
                        inv3 = invList.Where(a => (a.Date.Date.Equals(loopDate.Date) && (a.TimeFrom.Hours == (i / 8)) && (a.TimeFrom.Minutes == 30) && (a.TimeTo.Minutes == 45))).ToList().First();
                    }
                    catch
                    {

                        inv3 = new Invitation();

                    }

                    try
                    {
                        inv4 = invList.Where(a => (a.Date.Date.Equals(loopDate.Date) && (a.TimeFrom.Hours == (i / 8)) && (a.TimeFrom.Minutes == 45) && (a.TimeTo.Minutes == 0))).ToList().First();
                    }
                    catch
                    {

                        inv4 = new Invitation();

                    }


                    dList.Add(new NameValueItem3()
                    {
                        timeItem = false,
                        time = new TimeSpan(i / 8, 0, 0),
                        timeStr = null,
                        Width = 3,
                        weekDay = weekList.ElementAt((i % 8) - 1),
                        Color1 = (myOEvent != null ? "Wheat" : "#BFB5DF"),
                        myOutLookEvent = myOEvent,
                        myOutLookEvent2 = myOEvent2,
                        workHours = wh,
                        Border = (myOEvent != null || wh != null ? new Thickness(1) : new Thickness(0)),
                        //Color2 = (wh == null ? "AliceBlue" : (wh.From.TimeOfDay.Hours == 7 ? "#99FF99" : (wh.From.TimeOfDay.Hours == 15 ? "#99CCFF" : (wh.From.TimeOfDay.Hours == 23 ? "Wheat" : "AliceBlue")))),
                        Color2 = (wh == null ? "#BFB5DF" : (wh.From.TimeOfDay.Hours == 7 ? "#A3F1A7" : (wh.From.TimeOfDay.Hours == 15 ? "#A3F1A7" : (wh.From.TimeOfDay.Hours == 23 ? "#A3F1A7" : "#BFB5DF")))),

                        Width1 = (myOEvent == null && wh != null ? 0 : (myOEvent != null && wh != null) ? 67 : 135),
                        Width2 = (wh != null && myOEvent == null ? 135 : (wh != null && myOEvent != null) ? 67 : 0),
                        Appointment1 = app1,
                        Appointment2 = app2,
                        Appointment3 = app3,
                        Appointment4 = app4,
                        Invitation1 = inv1,
                        Invitation2 = inv2,
                        Invitation3 = inv3,
                        Invitation4 = inv4,

                        EnableTextBlockes = Visibility.Visible,
                        EnableTextBlockes2 = Visibility.Visible,
                        EnableTextBlockes3 = Visibility.Visible,
                        EnableTextBlockes4 = Visibility.Visible,



                        EnableTextBoxes = Visibility.Collapsed,
                        EnableTextBoxes2 = Visibility.Collapsed,
                        EnableTextBoxes3 = Visibility.Collapsed,
                        EnableTextBoxes4 = Visibility.Collapsed,



                        EnableButton = Visibility.Collapsed,
                        EnableButton2 = Visibility.Collapsed,
                        EnableButton3 = Visibility.Collapsed,
                        EnableButton4 = Visibility.Collapsed,


                        EnableDelButton = Visibility.Collapsed,
                        EnableDelButton2 = Visibility.Collapsed,
                        EnableDelButton3 = Visibility.Collapsed,
                        EnableDelButton4 = Visibility.Collapsed,


                    });
                    if (i > 0 || i < 8)
                    {
                        //  if(myOEvent.isAllDay)
                        switch (i)
                        {
                            case 1: Sunday.Text = (new DateTime(Convert.ToInt32(weekList.ElementAt((i % 8) - 1).Year),
                                DateTime.ParseExact(weekList.ElementAt((i % 8) - 1).Month, "MMMM", CultureInfo.CurrentCulture).Month,
                                weekList.ElementAt((i % 8) - 1).Day)
                                ).DayOfWeek.ToString() + " " + weekList.ElementAt((i % 8) - 1).Day;
                                //try
                                //{
                                //    Sunday2.Text = (myOEvent.isAllDay ? myOEvent.Name : null);
                                //}
                                //catch
                                //{
                                //    Sunday2.Text = String.Empty;
                                //}
                                break;
                            case 2: Monday.Text = (new DateTime(Convert.ToInt32(weekList.ElementAt((i % 8) - 1).Year),
                                 DateTime.ParseExact(weekList.ElementAt((i % 8) - 1).Month, "MMMM", CultureInfo.CurrentCulture).Month,
                                 weekList.ElementAt((i % 8) - 1).Day)
                                 ).DayOfWeek.ToString() + " " + weekList.ElementAt((i % 8) - 1).Day;
                                //try
                                //{
                                //    Monday2.Text = (myOEvent.isAllDay ? myOEvent.Name : null);
                                //}
                                //catch
                                //{
                                //    Monday2.Text = String.Empty;
                                //}
                                break;
                            case 3: Tuesday.Text = (new DateTime(Convert.ToInt32(weekList.ElementAt((i % 8) - 1).Year),
                                DateTime.ParseExact(weekList.ElementAt((i % 8) - 1).Month, "MMMM", CultureInfo.CurrentCulture).Month,
                                weekList.ElementAt((i % 8) - 1).Day)
                                ).DayOfWeek.ToString() + " " + weekList.ElementAt((i % 8) - 1).Day;
                                //try
                                //{
                                //    Tuesday2.Text = (myOEvent.isAllDay ? myOEvent.Name : null);
                                //}
                                //catch
                                //{
                                //    Tuesday2.Text = String.Empty;
                                //}
                                break;
                            case 4: Wednesday.Text = (new DateTime(Convert.ToInt32(weekList.ElementAt((i % 8) - 1).Year),
                                DateTime.ParseExact(weekList.ElementAt((i % 8) - 1).Month, "MMMM", CultureInfo.CurrentCulture).Month,
                                weekList.ElementAt((i % 8) - 1).Day)
                                ).DayOfWeek.ToString() + " " + weekList.ElementAt((i % 8) - 1).Day;
                                //try
                                //{
                                //    Wednesday2.Text = (myOEvent.isAllDay ? myOEvent.Name : null);
                                //}
                                //catch
                                //{
                                //    Wednesday2.Text = String.Empty;
                                //}
                                break;
                            case 5: Thursday.Text = (new DateTime(Convert.ToInt32(weekList.ElementAt((i % 8) - 1).Year),
                                DateTime.ParseExact(weekList.ElementAt((i % 8) - 1).Month, "MMMM", CultureInfo.CurrentCulture).Month,
                                weekList.ElementAt((i % 8) - 1).Day)
                                ).DayOfWeek.ToString() + " " + weekList.ElementAt((i % 8) - 1).Day;
                                //try
                                //{
                                //    Thursday2.Text = (myOEvent.isAllDay ? myOEvent.Name : null);
                                //}
                                //catch
                                //{
                                //    Thursday2.Text = String.Empty;
                                //}
                                break;
                            case 6: Friday.Text = (new DateTime(Convert.ToInt32(weekList.ElementAt((i % 8) - 1).Year),
                                DateTime.ParseExact(weekList.ElementAt((i % 8) - 1).Month, "MMMM", CultureInfo.CurrentCulture).Month,
                                weekList.ElementAt((i % 8) - 1).Day)
                                ).DayOfWeek.ToString() + " " + weekList.ElementAt((i % 8) - 1).Day;
                                //try
                                //{
                                //    Friday2.Text = (myOEvent.isAllDay ? myOEvent.Name : null);
                                //}
                                //catch
                                //{
                                //    Friday2.Text = String.Empty;
                                //}
                                break;
                            case 7:


                                Saturday.Text = (new DateTime(Convert.ToInt32(weekList.ElementAt((i % 8) - 1).Year),
                                DateTime.ParseExact(weekList.ElementAt((i % 8) - 1).Month, "MMMM", CultureInfo.CurrentCulture).Month,
                                weekList.ElementAt((i % 8) - 1).Day)
                                ).DayOfWeek.ToString() + " " + weekList.ElementAt((i % 8) - 1).Day;
                                //try
                                //{
                                //    Saturday2.Text = (myOEvent.isAllDay ? myOEvent.Name : null);
                                //}
                                //catch
                                //{
                                //    Saturday2.Text = String.Empty;
                                //}
                                break;


                        }
                        // weekList.ElementAt((i-1)%8)
                    }
                }
            }

            flip2 = 0;
            backward = false;
            WeekGridView.ItemsSource = dList;
            //weekList.Clear();
            stay = false;
            //Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            //{
            //    WeekGridView.ScrollIntoView(dList.ElementAt(147));
            //});


            myGrid.Opacity = 1;
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 2);


        }

      

        private  void previousMonthButton2_Click(object sender, RoutedEventArgs e)
        {
            
            flip2 = -1;
            backward = true;
          
            fillWeeklyCalendar();
           
        }

        private  void nextMonthButton2_Click(object sender, RoutedEventArgs e)
        {
          
            flip2 = 1;
            backward = false;

          
            fillWeeklyCalendar();
           
        }


        string oldApp1 = null;
        string oldApp2 = null;
        string oldApp3 = null;
        string oldApp4 = null;


        private NameValueItem3 selectedNVI;
        public NameValueItem3 SelectedNVI
        {
            get { return selectedNVI; }
            set
            {
                selectedNVI = value;
                OnPropertyChanged("SelectedNVI");
            }
        }

        private NameValueItem3 oldSelectedNVI;
        public NameValueItem3 OldSelectedNVI
        {
            get { return oldSelectedNVI; }
            set
            {
                oldSelectedNVI = value;
                OnPropertyChanged("OldSelectedNVI");
            }
        }

        private static double DipToPixel(double dip)
        {
            return (dip * DisplayProperties.LogicalDpi) / 96.0;
        }

        int countClick = 0;

        private void WeekGridView_ItemClick(object sender, ItemClickEventArgs e)
        {


            SelectedNVI = ((NameValueItem3)e.ClickedItem);
            //SelectedNVI.Date = (new DateTime(Convert.ToInt32(SelectedNVI.weekDay.Year),
            //                         DateTime.ParseExact(SelectedNVI.weekDay.Month, "MMMM", CultureInfo.CurrentCulture).Month,
            //                         SelectedNVI.weekDay.Day)
            //                         );

            countClick++;

            if (countClick == 1)
            {
                PopupHeight = 0;
                if (SelectedNVI.Appointment1.UserID != null || SelectedNVI.Appointment2.UserID != null || SelectedNVI.Appointment3.UserID != null || SelectedNVI.Appointment4.UserID != null || SelectedNVI.Invitation1.FromUserId != null || SelectedNVI.Invitation2.FromUserId != null || SelectedNVI.Invitation3.FromUserId != null || SelectedNVI.Invitation4.FromUserId != null)
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

                    LightDismissAnimatedPopup.VerticalOffset = myPoint.Y - 200;

                    LightDismissAnimatedPopup.HorizontalOffset = myPoint.X - 300;

                    List<NameValueItem3> nviList = new List<NameValueItem3>();
                    if (selectedNVI.Appointment1.UserID != null)
                    {
                        nviList.Add(new NameValueItem3() { Appointment1 = selectedNVI.Appointment1 });
                        PopupHeight = PopupHeight + 100;
                    }
                    if (selectedNVI.Appointment2.UserID != null)
                    {
                        nviList.Add(new NameValueItem3() { Appointment2 = selectedNVI.Appointment2 });
                        PopupHeight = PopupHeight + 100;
                    }
                    if (selectedNVI.Appointment3.UserID != null)
                    {
                        nviList.Add(new NameValueItem3() { Appointment3 = selectedNVI.Appointment3 });
                           PopupHeight = PopupHeight + 100;
                    }
                    if (selectedNVI.Appointment4.UserID != null)
                    {
                        nviList.Add(new NameValueItem3() { Appointment4 = selectedNVI.Appointment4 });
                          PopupHeight = PopupHeight + 100;
                    }

                    if (selectedNVI.Invitation1.FromUserId != null)
                    {
                        nviList.Add(new NameValueItem3() { Invitation1 = selectedNVI.Invitation1 });
                        PopupHeight = PopupHeight + 100;
                    }

                    if (selectedNVI.Invitation2.FromUserId != null)
                    {
                        nviList.Add(new NameValueItem3() { Invitation2 = selectedNVI.Invitation2 });
                        PopupHeight = PopupHeight + 100;
                    }
                    if (selectedNVI.Invitation3.FromUserId != null)
                    {
                        nviList.Add(new NameValueItem3() { Invitation3 = selectedNVI.Invitation3 });
                          PopupHeight = PopupHeight + 100;
                    }
                    if (selectedNVI.Invitation4.FromUserId != null)
                    {
                        nviList.Add(new NameValueItem3() { Invitation4 = selectedNVI.Invitation4 });
                          PopupHeight = PopupHeight + 100;
                    }

                    PopupGridView.ItemsSource = nviList;

                    if (!LightDismissAnimatedPopup.IsOpen) { LightDismissAnimatedPopup.IsOpen = true; }

                }


            }
            if (countClick == 2)
            {
                //SelectedNVI.EnableTextBlockes = Visibility.Collapsed;
                // SelectedNVI.EnableTextBoxes = Visibility.Visible;
                countClick = 0;

                if ((SelectedNVI.Appointment1.UserID != null) && (!SelectedDoctor.UserId.Equals(Connection.User.UserId)))
                {
                    SelectedNVI.EnableTextBlockes = Visibility.Visible;
                    SelectedNVI.EnableTextBoxes = Visibility.Collapsed;
                }
                else
                {
                    SelectedNVI.EnableTextBoxes = Visibility.Visible;
                    SelectedNVI.EnableTextBlockes = Visibility.Collapsed;
                }

                if ((SelectedNVI.Appointment2.UserID != null) && (!SelectedDoctor.UserId.Equals(Connection.User.UserId)))
                {
                    SelectedNVI.EnableTextBlockes2 = Visibility.Visible;
                    SelectedNVI.EnableTextBoxes2 = Visibility.Collapsed;
                }
                else
                {
                    SelectedNVI.EnableTextBoxes2 = Visibility.Visible;
                    SelectedNVI.EnableTextBlockes2 = Visibility.Collapsed;
                }

                if ((SelectedNVI.Appointment3.UserID != null) && (!SelectedDoctor.UserId.Equals(Connection.User.UserId)))
                {
                    SelectedNVI.EnableTextBlockes3 = Visibility.Visible;
                    SelectedNVI.EnableTextBoxes3 = Visibility.Collapsed;
                }
                else
                {
                    SelectedNVI.EnableTextBoxes3 = Visibility.Visible;
                    SelectedNVI.EnableTextBlockes3 = Visibility.Collapsed;

                }


                if ((SelectedNVI.Appointment4.UserID != null) && (!SelectedDoctor.UserId.Equals(Connection.User.UserId)))
                {
                    SelectedNVI.EnableTextBlockes4 = Visibility.Visible;

                    SelectedNVI.EnableTextBoxes4 = Visibility.Collapsed;
                }
                else
                {
                    SelectedNVI.EnableTextBlockes4 = Visibility.Collapsed;

                    SelectedNVI.EnableTextBoxes4 = Visibility.Visible;
                }





                if ((SelectedDoctor.UserId.Equals(Connection.User.UserId)) && (SelectedNVI.Invitation1.Complaint != null)) // show button only for the desired doctor
                {
                    //  if ((SelectedNVI.Appointment1.TimeFrom) == null || SelectedDoctor.UserId.Equals(Connection.User.UserId))

                    SelectedNVI.EnableButton = Visibility.Visible;
                    SelectedNVI.EnableDelButton = Visibility.Visible;

                }
                else if (SelectedNVI.Invitation1.FromUserId != null && SelectedNVI.Invitation1.FromUserId.Equals(Connection.User.UserId))
                {
                    SelectedNVI.EnableButton = Visibility.Collapsed;
                    SelectedNVI.EnableDelButton = Visibility.Visible;
                }
                else
                {
                    SelectedNVI.EnableButton = Visibility.Collapsed;
                    SelectedNVI.EnableDelButton = Visibility.Collapsed;
                }

                if ((SelectedDoctor.UserId.Equals(Connection.User.UserId)) && (SelectedNVI.Invitation2.Complaint != null)) // show button only for the desired doctor
                {
                    SelectedNVI.EnableButton2 = Visibility.Visible;
                    SelectedNVI.EnableDelButton2 = Visibility.Visible;
                }
                else if (SelectedNVI.Invitation2.FromUserId != null && SelectedNVI.Invitation2.FromUserId.Equals(Connection.User.UserId))
                {
                    SelectedNVI.EnableButton2 = Visibility.Collapsed;
                    SelectedNVI.EnableDelButton2 = Visibility.Visible;
                }
                else
                {
                    SelectedNVI.EnableButton2 = Visibility.Collapsed;
                    SelectedNVI.EnableDelButton2 = Visibility.Collapsed;
                }

                if ((SelectedDoctor.UserId.Equals(Connection.User.UserId)) && (SelectedNVI.Invitation3.Complaint != null)) // show button only for the desired doctor
                {
                    SelectedNVI.EnableButton3 = Visibility.Visible;
                    SelectedNVI.EnableDelButton3 = Visibility.Visible;
                }
                else if (SelectedNVI.Invitation3.FromUserId != null && SelectedNVI.Invitation3.FromUserId.Equals(Connection.User.UserId))
                {
                    SelectedNVI.EnableButton3 = Visibility.Collapsed;
                    SelectedNVI.EnableDelButton3 = Visibility.Visible;
                }
                else
                {
                    SelectedNVI.EnableButton3 = Visibility.Collapsed;
                    SelectedNVI.EnableDelButton3 = Visibility.Collapsed;
                }

                if ((SelectedDoctor.UserId.Equals(Connection.User.UserId)) && (SelectedNVI.Invitation4.Complaint != null)) // show button only for the desired doctor
                {
                    SelectedNVI.EnableButton4 = Visibility.Visible;
                    SelectedNVI.EnableDelButton4 = Visibility.Visible;
                }
                else if (SelectedNVI.Invitation4.FromUserId != null && SelectedNVI.Invitation4.FromUserId.Equals(Connection.User.UserId))
                {
                    SelectedNVI.EnableButton4 = Visibility.Collapsed;
                    SelectedNVI.EnableDelButton4 = Visibility.Visible;
                }
                else
                {
                    SelectedNVI.EnableButton4 = Visibility.Collapsed;
                    SelectedNVI.EnableDelButton4 = Visibility.Collapsed;

                }


            }
        }






        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            //whList = (await WorkHours.ReadListAsync()).Where(w => w.EmployeeId == SelectedDoctor.UserId).ToList();
            //appList = (await Appointment.ReadAppointmentsList()).Where(a => a.UserID == SelectedDoctor.UserId).ToList();
            //invList = (await Invitation.ReadInvitationsList()).Where(i => i.ToUserId == SelectedDoctor.UserId).ToList();
            fillWeeklyCalendar();
        }

        private async void Button_Click(object sender, RoutedEventArgs e) // 0-15
        {
            //SelectedNVI.Invitation1 

            Appointment app = new Appointment();
            app.Complaint = "_" + SelectedNVI.Invitation1.Complaint;
            app.Date = (new DateTime(Convert.ToInt32(SelectedNVI.weekDay.Year),
                            DateTime.ParseExact(SelectedNVI.weekDay.Month, "MMMM", CultureInfo.CurrentCulture).Month,
                            SelectedNVI.weekDay.Day)
                            );
            app.PatientID = SelectedNVI.Invitation1.PatientID;
            app.TimeFrom = new TimeSpan(SelectedNVI.time.Hours, 0, 0);
            app.TimeTo = new TimeSpan(SelectedNVI.time.Hours, 15, 0);
            app.UserID = SelectedNVI.Invitation1.ToUserId;
            app.AppoitmentID = String.Empty;
            Appointment.InsertNewAppointment(app);

            Invitation temp = await Invitation.getInvitation(SelectedNVI.Invitation1);
            Invitation.DeleteInvitation(temp);
           // whList = (await WorkHours.ReadListAsync()).Where(w => w.EmployeeId == SelectedDoctor.UserId).ToList();
            //appList = (await Appointment.ReadAppointmentsList()).Where(a => a.UserID == SelectedDoctor.UserId).ToList();
            //invList = (await Invitation.ReadInvitationsList()).Where(i => i.ToUserId == SelectedDoctor.UserId).ToList();

            fillWeeklyCalendar();

        }

        private async void Button_Click_1(object sender, RoutedEventArgs e) //15-30
        {
            Appointment app = new Appointment();
            app.Complaint = "_" + SelectedNVI.Invitation2.Complaint;
            app.Date = (new DateTime(Convert.ToInt32(SelectedNVI.weekDay.Year),
                            DateTime.ParseExact(SelectedNVI.weekDay.Month, "MMMM", CultureInfo.CurrentCulture).Month,
                            SelectedNVI.weekDay.Day)
                            );
            app.PatientID = SelectedNVI.Invitation2.PatientID;
            app.TimeFrom = new TimeSpan(SelectedNVI.time.Hours, 15, 0);
            app.TimeTo = new TimeSpan(SelectedNVI.time.Hours, 30, 0);
            app.UserID = SelectedNVI.Invitation2.ToUserId;
            app.AppoitmentID = String.Empty;
            Appointment.InsertNewAppointment(app);

            Invitation temp = await Invitation.getInvitation(SelectedNVI.Invitation2);
            Invitation.DeleteInvitation(temp);

           // whList = (await WorkHours.ReadListAsync()).Where(w => w.EmployeeId == SelectedDoctor.UserId).ToList();
            //appList = (await Appointment.ReadAppointmentsList()).Where(a => a.UserID == SelectedDoctor.UserId).ToList();
            //invList = (await Invitation.ReadInvitationsList()).Where(i => i.ToUserId == SelectedDoctor.UserId).ToList();
            fillWeeklyCalendar();
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)// 30-45
        {
            Appointment app = new Appointment();
            app.Complaint = "_" + SelectedNVI.Invitation3.Complaint;
            app.Date = (new DateTime(Convert.ToInt32(SelectedNVI.weekDay.Year),
                            DateTime.ParseExact(SelectedNVI.weekDay.Month, "MMMM", CultureInfo.CurrentCulture).Month,
                            SelectedNVI.weekDay.Day)
                            );
            app.PatientID = SelectedNVI.Invitation3.PatientID;
            app.TimeFrom = new TimeSpan(SelectedNVI.time.Hours, 30, 0);
            app.TimeTo = new TimeSpan(SelectedNVI.time.Hours, 45, 0);
            app.UserID = SelectedNVI.Invitation3.ToUserId;
            app.AppoitmentID = String.Empty;
            Appointment.InsertNewAppointment(app);

            Invitation temp = await Invitation.getInvitation(SelectedNVI.Invitation3);
            Invitation.DeleteInvitation(temp);
            //whList = (await WorkHours.ReadListAsync()).Where(w => w.EmployeeId == SelectedDoctor.UserId).ToList();
            //appList = (await Appointment.ReadAppointmentsList()).Where(a => a.UserID == SelectedDoctor.UserId).ToList();
            //invList = (await Invitation.ReadInvitationsList()).Where(i => i.ToUserId == SelectedDoctor.UserId).ToList();

            fillWeeklyCalendar();
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e)//45-0
        {
            Appointment app = new Appointment();
            app.Complaint = "_" + SelectedNVI.Invitation4.Complaint;
            app.Date = (new DateTime(Convert.ToInt32(SelectedNVI.weekDay.Year),
                            DateTime.ParseExact(SelectedNVI.weekDay.Month, "MMMM", CultureInfo.CurrentCulture).Month,
                            SelectedNVI.weekDay.Day)
                            );
            app.PatientID = SelectedNVI.Invitation4.PatientID;
            app.TimeFrom = new TimeSpan(SelectedNVI.time.Hours, 45, 0);
            app.TimeTo = new TimeSpan(SelectedNVI.time.Hours, 0, 0);
            app.UserID = SelectedNVI.Invitation4.ToUserId;
            app.AppoitmentID = String.Empty;
            Appointment.InsertNewAppointment(app);

            Invitation temp = await Invitation.getInvitation(SelectedNVI.Invitation4);
            Invitation.DeleteInvitation(temp);

           // whList = (await WorkHours.ReadListAsync()).Where(w => w.EmployeeId == SelectedDoctor.UserId).ToList();
            //appList = (await Appointment.ReadAppointmentsList()).Where(a => a.UserID == SelectedDoctor.UserId).ToList();
            //invList = (await Invitation.ReadInvitationsList()).Where(i => i.ToUserId == SelectedDoctor.UserId).ToList();
            fillWeeklyCalendar();

        }

        private async void Button_Click_4(object sender, RoutedEventArgs e) // Decline Invitation 1
        {
            MessageDialog md = new MessageDialog("Are you sure you want to delete invitation?");
            bool? result = null;
            md.Commands.Add(
               new UICommand("OK", new UICommandInvokedHandler((cmd) => result = true)));
            md.Commands.Add(
               new UICommand("Cancel", new UICommandInvokedHandler((cmd) => result = false)));

            await md.ShowAsync();
            if (result == true)
            {
                Invitation temp = await Invitation.getInvitation(SelectedNVI.Invitation1);
                Invitation.DeleteInvitation(temp);

                //whList = (await WorkHours.ReadListAsync()).Where(w => w.EmployeeId == SelectedDoctor.UserId).ToList();
                //appList = (await Appointment.ReadAppointmentsList()).Where(a => a.UserID == SelectedDoctor.UserId).ToList();
                //invList = (await Invitation.ReadInvitationsList()).Where(i => i.ToUserId == SelectedDoctor.UserId).ToList();
                fillWeeklyCalendar();
            }

        }

        private async void Button_Click_5(object sender, RoutedEventArgs e) //Decline Invitation 2
        {
            MessageDialog md = new MessageDialog("Are you sure you want to delete invitation?");
            bool? result = null;
            md.Commands.Add(
               new UICommand("OK", new UICommandInvokedHandler((cmd) => result = true)));
            md.Commands.Add(
               new UICommand("Cancel", new UICommandInvokedHandler((cmd) => result = false)));

            await md.ShowAsync();
            if (result == true)
            {
                Invitation temp = await Invitation.getInvitation(SelectedNVI.Invitation2);
                Invitation.DeleteInvitation(temp);

                //whList = (await WorkHours.ReadListAsync()).Where(w => w.EmployeeId == SelectedDoctor.UserId).ToList();
                //appList = (await Appointment.ReadAppointmentsList()).Where(a => a.UserID == SelectedDoctor.UserId).ToList();
               // invList = (await Invitation.ReadInvitationsList()).Where(i => i.ToUserId == SelectedDoctor.UserId).ToList();
                fillWeeklyCalendar();
            }
        }

        private async void Button_Click_6(object sender, RoutedEventArgs e)
        {
            MessageDialog md = new MessageDialog("Are you sure you want to delete invitation?");
            bool? result = null;
            md.Commands.Add(
               new UICommand("OK", new UICommandInvokedHandler((cmd) => result = true)));
            md.Commands.Add(
               new UICommand("Cancel", new UICommandInvokedHandler((cmd) => result = false)));

            await md.ShowAsync();
            if (result == true)
            {
                Invitation temp = await Invitation.getInvitation(SelectedNVI.Invitation3);
                Invitation.DeleteInvitation(temp);

                //whList = (await WorkHours.ReadListAsync()).Where(w => w.EmployeeId == SelectedDoctor.UserId).ToList();
               // appList = (await Appointment.ReadAppointmentsList()).Where(a => a.UserID == SelectedDoctor.UserId).ToList();
               // invList = (await Invitation.ReadInvitationsList()).Where(i => i.ToUserId == SelectedDoctor.UserId).ToList();

                fillWeeklyCalendar();
            }
        }

        private async void Button_Click_7(object sender, RoutedEventArgs e)
        {
            MessageDialog md = new MessageDialog("Are you sure you want to delete invitation?");
            bool? result = null;
            md.Commands.Add(
               new UICommand("OK", new UICommandInvokedHandler((cmd) => result = true)));
            md.Commands.Add(
               new UICommand("Cancel", new UICommandInvokedHandler((cmd) => result = false)));

            await md.ShowAsync();
            if (result == true)
            {
                Invitation temp = await Invitation.getInvitation(SelectedNVI.Invitation4);
                Invitation.DeleteInvitation(temp);

                //whList = (await WorkHours.ReadListAsync()).Where(w => w.EmployeeId == SelectedDoctor.UserId).ToList();
                //appList = (await Appointment.ReadAppointmentsList()).Where(a => a.UserID == SelectedDoctor.UserId).ToList();
               // invList = (await Invitation.ReadInvitationsList()).Where(i => i.ToUserId == SelectedDoctor.UserId).ToList();

                fillWeeklyCalendar();
            }
        }

        private Visibility showAddAppointment = Visibility.Collapsed;
        public Visibility ShowAddAppointment
        {
            get { return showAddAppointment; }
            set
            {
                showAddAppointment = value;
                OnPropertyChanged("ShowAddAppointment");
            }
        }

        private Appointment bindAppointment;
        public Appointment BindAppointment
        {
            get { return bindAppointment; }
            set
            {

                bindAppointment = value;
                OnPropertyChanged("BindAppointment");
            }
        }


        private Appointment selectedAppointment;
        public Appointment SelectedAppointment
        {
            get { return selectedAppointment; }
            set
            {

                selectedAppointment = value;
                OnPropertyChanged("SelectedAppointment");
            }
        }


        private void TextBlock_GotFocus(object sender, RoutedEventArgs e) // 0-15
        {
            //Specify time
            //visibility

            SelectedAppointment = SelectedNVI.Appointment1;

            if (SelectedNVI.Appointment1.UserID == null)
            {
                Appointment app = new Appointment();


                app.Complaint = SelectedNVI.Appointment1.Complaint;
                app.Date = (new DateTime(Convert.ToInt32(SelectedNVI.weekDay.Year),
                                DateTime.ParseExact(SelectedNVI.weekDay.Month, "MMMM", CultureInfo.CurrentCulture).Month,
                                SelectedNVI.weekDay.Day)
                                );
                app.PatientID = SelectedPatient.PatientID;
                app.TimeFrom = new TimeSpan(SelectedNVI.time.Hours, 0, 0);
                app.TimeTo = new TimeSpan(SelectedNVI.time.Hours, 15, 0);
                app.UserID = SelectedDoctor.UserId;
                app.AppoitmentID = String.Empty;

                BindAppointment = app;
                // BindAppointment = SelectedNVI.Appointment1;
            }
            else
            {
                BindAppointment = SelectedNVI.Appointment1;
                DeleteAppointmentVisibility = Visibility.Visible;
            }
            ShowAddAppointment = Visibility.Visible;
        }

        public DelegateCommand Add_Appointment { get; private set; }
        public DelegateCommand Cancel_Appointment { get; private set; }
        public DelegateCommand Delete_Appointment { get; private set; }

        public DelegateCommand Cal_Appointment { get; private set; }

        String appointmentId = String.Empty;
        private void SetupCommands()
        {

            Add_Appointment = new DelegateCommand(async () =>
            {
                MessageDialog mm;

                if (string.IsNullOrWhiteSpace(BindAppointment.Complaint))
                {
                    //show message
                    mm = new MessageDialog("Please fill in all the fields");
                    await mm.ShowAsync();
                    return;
                }
                WorkHours wh = null;


                try
                {
                    wh = whList.Where(w => ((w.From.Date.Equals(BindAppointment.Date.Date) && w.From.TimeOfDay.Hours <= (BindAppointment.TimeFrom.Hours) && w.To.Date.Equals(BindAppointment.Date.Date) && w.To.TimeOfDay.Hours >= (BindAppointment.TimeTo.Hours)) || (w.From.Date.Equals(BindAppointment.Date.Date) && w.From.TimeOfDay.Hours <= (BindAppointment.TimeFrom.Hours) && w.To.Date.Equals(BindAppointment.Date.AddDays(1))) || (w.From.Date.Equals(BindAppointment.Date.Date.AddDays(-1).Date) && w.To.Date.Equals(BindAppointment.Date.Date) && w.To.TimeOfDay.Hours >= (BindAppointment.TimeFrom.Hours)))).ToArray().First();

                    //    wh = whList.Where(w => (w.From.Date.Equals(BindAppointment.Date.Date) &&  )).ToArray().First();
                    if (!String.IsNullOrEmpty(wh.TimeOffReason))
                        wh = null;
                }
                catch
                {

                }

                if (wh != null)
                {
                    try
                    {
                        BindAppointment.Complaint = (BindAppointment.Complaint.ElementAt(0) == '_' ? BindAppointment.Complaint : ("_" + BindAppointment.Complaint));
                    }
                    catch
                    {
                        BindAppointment.Complaint = "_";
                    }

                    if (SelectedDoctor.UserId.Equals(Connection.User.UserId))
                    {

                        if (String.IsNullOrEmpty(SelectedAppointment.UserID))
                        {

                            //String appointmentId = String.Empty;
                           
                                
                            //    var appointment = new Windows.ApplicationModel.Appointments.Appointment();

                            //    // StartTime
                            //    var date = BindAppointment.Date.Date;
                            //    var time = BindAppointment.TimeFrom;
                            //    var timeZoneOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
                            //    var startTime = new DateTimeOffset(date.Year, date.Month, date.Day, time.Hours, time.Minutes, 0, timeZoneOffset);
                            //    appointment.StartTime = startTime;

                            //    // Subject
                            //    Patient p =Scheduling.getPatient(BindAppointment.PatientID);
                            //    appointment.Subject = "Clinic: "+p.LName+", "+p.FName;
                            //    // Details
                            //    appointment.Details = BindAppointment.Complaint;
                            //    appointment.Duration = TimeSpan.FromMinutes(15);

                                
                            //    Windows.UI.Xaml.Media.GeneralTransform buttonTransform = (this as FrameworkElement).TransformToVisual(null);
                            //    Windows.Foundation.Point point = buttonTransform.TransformPoint(new Windows.Foundation.Point());
                            //    var rect = new Windows.Foundation.Rect(point, new Windows.Foundation.Size((this as FrameworkElement).ActualWidth, (this as FrameworkElement).ActualHeight));
                            //    appointmentId = await Windows.ApplicationModel.Appointments.AppointmentManager.ShowAddAppointmentAsync(appointment, rect);

                         
                           // BindAppointment.AppoitmentID = appointmentId;

                            Appointment.InsertNewAppointment(BindAppointment);
                             appointmentId = String.Empty;
                           // appList = (await Appointment.ReadAppointmentsList()).Where(a => a.UserID == SelectedDoctor.UserId).ToList();
                        
                        }
                        else
                        {
                            // new MessageDialog(SelectedNVI.Appointment1.Complaint).ShowAsync();

                          app.UpdateAppointment(BindAppointment);
                          //  appList = (await Appointment.ReadAppointmentsList()).Where(a => a.UserID == SelectedDoctor.UserId).ToList();
                        
                        }
                       // whList = (await WorkHours.ReadListAsync()).Where(w => w.EmployeeId == SelectedDoctor.UserId).ToList();
                        //appList = (await Appointment.ReadAppointmentsList()).Where(a => a.UserID == SelectedDoctor.UserId).ToList();
                        ////invList = (await Invitation.ReadInvitationsList()).Where(i => i.ToUserId == SelectedDoctor.UserId).ToList();
                    }
                    else
                    {

                        Invitation inv = new Invitation();
                        inv.Complaint = BindAppointment.Complaint.Substring(1, (BindAppointment.Complaint.Count() - 1));
                        inv.Date = BindAppointment.Date;
                        inv.PatientID = BindAppointment.PatientID;
                        inv.TimeFrom = BindAppointment.TimeFrom;
                        inv.TimeTo = BindAppointment.TimeTo;
                        inv.ToUserId = BindAppointment.UserID;
                        inv.FromUserId = Connection.User.UserId;

                        Invitation.InsertNewInvitation(inv);
                       // whList = (await WorkHours.ReadListAsync()).Where(w => w.EmployeeId == SelectedDoctor.UserId).ToList();
                       // appList = (await Appointment.ReadAppointmentsList()).Where(a => a.UserID == SelectedDoctor.UserId).ToList();
                        //invList = (await Invitation.ReadInvitationsList()).Where(i => i.ToUserId == SelectedDoctor.UserId).ToList();

                    }

                    ShowAddAppointment = Visibility.Collapsed;
                    DeleteAppointmentVisibility = Visibility.Collapsed;
                    SelectedAppointment = null;
                    //invList = (await Invitation.ReadInvitationsList()).Where(i => i.ToUserId == SelectedDoctor.UserId).ToList();

                    //appList = (await Appointment.ReadAppointmentsList()).Where(a => a.UserID == SelectedDoctor.UserId).ToList();

                    fillWeeklyCalendar();
                }
                else
                {
                    //show message
                    mm = new MessageDialog("Appointment should be within the doctors work hours");
                    await mm.ShowAsync();
                    return;
                }

            }, true);

            Cancel_Appointment = new DelegateCommand(async () =>
            {


                OldSelectedNVI.EnableTextBlockes = Visibility.Visible;
                OldSelectedNVI.EnableTextBoxes = Visibility.Collapsed;
                OldSelectedNVI.EnableButton = Visibility.Collapsed;
                OldSelectedNVI.EnableDelButton = Visibility.Collapsed;

                OldSelectedNVI.EnableTextBlockes2 = Visibility.Visible;
                OldSelectedNVI.EnableTextBoxes2 = Visibility.Collapsed;
                OldSelectedNVI.EnableButton2 = Visibility.Collapsed;
                OldSelectedNVI.EnableDelButton2 = Visibility.Collapsed;

                OldSelectedNVI.EnableTextBlockes3 = Visibility.Visible;
                OldSelectedNVI.EnableTextBoxes3 = Visibility.Collapsed;
                OldSelectedNVI.EnableButton3 = Visibility.Collapsed;
                OldSelectedNVI.EnableDelButton3 = Visibility.Collapsed;



                OldSelectedNVI.EnableTextBlockes4 = Visibility.Visible;
                OldSelectedNVI.EnableTextBoxes4 = Visibility.Collapsed;
                OldSelectedNVI.EnableButton4 = Visibility.Collapsed;
                OldSelectedNVI.EnableDelButton4 = Visibility.Collapsed;

                BindAppointment = null;
                ShowAddAppointment = Visibility.Collapsed;
                DeleteAppointmentVisibility = Visibility.Collapsed;
                SelectedAppointment = null;
                // fillWeeklyCalendar();

            }, true);
            Delete_Appointment = new DelegateCommand(async () =>
            {

                Appointment.DeleteAppointment(BindAppointment);


                BindAppointment = null;
                ShowAddAppointment = Visibility.Collapsed;
                DeleteAppointmentVisibility = Visibility.Collapsed;
                SelectedAppointment = null;

                //whList = (await WorkHours.ReadListAsync()).Where(w => w.EmployeeId == SelectedDoctor.UserId).ToList();
               // appList = (await Appointment.ReadAppointmentsList()).Where(a => a.UserID == SelectedDoctor.UserId).ToList();
                //invList = (await Invitation.ReadInvitationsList()).Where(i => i.ToUserId == SelectedDoctor.UserId).ToList();

                fillWeeklyCalendar();

            }, true);

            Cal_Appointment = new DelegateCommand(async () =>
            {

                if (String.IsNullOrEmpty(BindAppointment.AppoitmentID))
                {

                    var appointment = new Windows.ApplicationModel.Appointments.Appointment();

                    // StartTime
                    var date = BindAppointment.Date.Date;
                    var time = BindAppointment.TimeFrom;
                    var timeZoneOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now);
                    var startTime = new DateTimeOffset(date.Year, date.Month, date.Day, time.Hours, time.Minutes, 0, timeZoneOffset);
                    appointment.StartTime = startTime;

                    // Subject
                    Patient p = Scheduling.getPatient(BindAppointment.PatientID);
                    appointment.Subject = "Clinic: " + p.LName + ", " + p.FName;
                    // Details
                    appointment.Details = BindAppointment.Complaint;
                    appointment.Duration = TimeSpan.FromMinutes(15);


                    Windows.UI.Xaml.Media.GeneralTransform buttonTransform = (this as FrameworkElement).TransformToVisual(null);
                    Windows.Foundation.Point point = buttonTransform.TransformPoint(new Windows.Foundation.Point());
                    var rect = new Windows.Foundation.Rect(point, new Windows.Foundation.Size((this as FrameworkElement).ActualWidth, (this as FrameworkElement).ActualHeight));
                    appointmentId = await Windows.ApplicationModel.Appointments.AppointmentManager.ShowAddAppointmentAsync(appointment, rect);


                    BindAppointment.AppoitmentID = appointmentId;
                }
                else
                {
                    await new MessageDialog("Appointment has already been added to the original calendar").ShowAsync();
                }

            }, true);



        }

        private void app2TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SelectedAppointment = SelectedNVI.Appointment2;
            if (SelectedNVI.Appointment2.UserID == null)
            {
                Appointment app = new Appointment();


                app.Complaint = SelectedNVI.Appointment2.Complaint;
                app.Date = (new DateTime(Convert.ToInt32(SelectedNVI.weekDay.Year),
                                DateTime.ParseExact(SelectedNVI.weekDay.Month, "MMMM", CultureInfo.CurrentCulture).Month,
                                SelectedNVI.weekDay.Day)
                                );
                app.PatientID = SelectedPatient.PatientID;
                app.TimeFrom = new TimeSpan(SelectedNVI.time.Hours, 15, 0);
                app.TimeTo = new TimeSpan(SelectedNVI.time.Hours, 30, 0);
                app.UserID = SelectedDoctor.UserId;
                app.AppoitmentID = String.Empty;

                BindAppointment = app;
            }
            else
            {
                BindAppointment = SelectedNVI.Appointment2;
                DeleteAppointmentVisibility = Visibility.Visible;
            }
            ShowAddAppointment = Visibility.Visible;
        }

        private void app3TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SelectedAppointment = SelectedNVI.Appointment3;

            if (SelectedNVI.Appointment3.UserID == null)
            {

                Appointment app = new Appointment();


                app.Complaint = SelectedNVI.Appointment3.Complaint;
                app.Date = (new DateTime(Convert.ToInt32(SelectedNVI.weekDay.Year),
                                DateTime.ParseExact(SelectedNVI.weekDay.Month, "MMMM", CultureInfo.CurrentCulture).Month,
                                SelectedNVI.weekDay.Day)
                                );
                app.PatientID = SelectedPatient.PatientID;
                app.TimeFrom = new TimeSpan(SelectedNVI.time.Hours, 30, 0);
                app.TimeTo = new TimeSpan(SelectedNVI.time.Hours, 45, 0);
                app.UserID = SelectedDoctor.UserId;
                app.AppoitmentID = String.Empty;

                BindAppointment = app;
            }
            else
            {
                BindAppointment = SelectedNVI.Appointment3;
                DeleteAppointmentVisibility = Visibility.Visible;
            }
            ShowAddAppointment = Visibility.Visible;
        }

        private void app4TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SelectedAppointment = SelectedNVI.Appointment4;

            if (SelectedNVI.Appointment4.UserID == null)
            {
                Appointment app = new Appointment();
                app.Complaint = SelectedNVI.Appointment4.Complaint;
                app.Date = (new DateTime(Convert.ToInt32(SelectedNVI.weekDay.Year),
                                DateTime.ParseExact(SelectedNVI.weekDay.Month, "MMMM", CultureInfo.CurrentCulture).Month,
                                SelectedNVI.weekDay.Day)
                                );
                app.PatientID = SelectedPatient.PatientID;
                app.TimeFrom = new TimeSpan(SelectedNVI.time.Hours, 45, 0);
                app.TimeTo = new TimeSpan(SelectedNVI.time.Hours, 0, 0);
                app.UserID = SelectedDoctor.UserId;
                app.AppoitmentID = String.Empty;

                BindAppointment = app;
            }
            else
            {
                BindAppointment = SelectedNVI.Appointment4;
                DeleteAppointmentVisibility = Visibility.Visible;
            }
            ShowAddAppointment = Visibility.Visible;
        }

        public static Patient getPatient(int id)
        {
            Patient pp = pList.Where(p => p.PatientID.Equals(id)).ToList().First();
            return pp;
        }

        public static User getUser(string id)
        {
            User user = doctorsList.Where(p => p.UserId.Equals(id)).ToList().First();
            return user;
        }


        private int popupHeight = 0;
        public int PopupHeight
        {
            get { return popupHeight; }
            set
            {

                popupHeight = value;
                OnPropertyChanged("PopupHeight");
            }
        }

        private static bool changed = false;

        public static bool Changed 
        {
            get { return changed; }
            set
            {
               changed = value;
               //OnPropertyChanged("PopupHeight");

            }
        }

        public static bool oldChanged;

        private Visibility deleteAppointmentVisibility = Visibility.Collapsed;
        public Visibility DeleteAppointmentVisibility
        {
            get { return deleteAppointmentVisibility; }
            set
            {
                deleteAppointmentVisibility = value;
                OnPropertyChanged("DeleteAppointmentVisibility");
            }
        }

        private Visibility calAppointmentVisibility = Visibility.Collapsed;
        public Visibility CalAppointmentVisibility
        {
            get { return calAppointmentVisibility; }
            set
            {
                calAppointmentVisibility = value;
                OnPropertyChanged("CalAppointmentVisibility");
            }
        }

        private void GoToPatient(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(MainPage));
            this.Frame.Navigate(typeof(NewPatient), (object)SelectedNVI.Appointment1.PatientID);
                //(typeof(NewPatient(SelectedNVI.Appointment1.PatientID)));
          //  this.Frame.Navigate()
        }

        private void GoToPatient2(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(MainPage));
            this.Frame.Navigate(typeof(NewPatient), (object)SelectedNVI.Invitation1.PatientID);
            //(typeof(NewPatient(SelectedNVI.Appointment1.PatientID)));
            //  this.Frame.Navigate()
        }

        private void GoToPatient3(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(MainPage));
            this.Frame.Navigate(typeof(NewPatient), (object)SelectedNVI.Appointment2.PatientID);
            //(typeof(NewPatient(SelectedNVI.Appointment1.PatientID)));
            //  this.Frame.Navigate()
        }

        private void GoToPatient4(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(MainPage));
            this.Frame.Navigate(typeof(NewPatient), (object)SelectedNVI.Invitation2.PatientID);
            //(typeof(NewPatient(SelectedNVI.Appointment1.PatientID)));
            //  this.Frame.Navigate()
        }

        private void GoToPatient5(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(MainPage));
            this.Frame.Navigate(typeof(NewPatient), (object)SelectedNVI.Appointment3.PatientID);
            //(typeof(NewPatient(SelectedNVI.Appointment1.PatientID)));
            //  this.Frame.Navigate()
        }

        private void GoToPatient6(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(MainPage));
            this.Frame.Navigate(typeof(NewPatient), (object)SelectedNVI.Invitation3.PatientID);
            //(typeof(NewPatient(SelectedNVI.Appointment1.PatientID)));
            //  this.Frame.Navigate()
        }
        private void GoToPatient7(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(MainPage));
            this.Frame.Navigate(typeof(NewPatient), (object)SelectedNVI.Appointment4.PatientID);
            //(typeof(NewPatient(SelectedNVI.Appointment1.PatientID)));
            //  this.Frame.Navigate()
        }

        private void GoToPatient8(object sender, RoutedEventArgs e)
        {
            //this.Frame.Navigate(typeof(MainPage));
            this.Frame.Navigate(typeof(NewPatient), (object)SelectedNVI.Invitation4.PatientID);
            //(typeof(NewPatient(SelectedNVI.Appointment1.PatientID)));
            //  this.Frame.Navigate()
        }

       


    }
}
