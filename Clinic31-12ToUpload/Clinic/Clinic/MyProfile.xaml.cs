using Clinic.Model;
using Clinic.myBehavior;
using Microsoft.Live;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Clinic
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MyProfile : Common.LayoutAwarePage, INotifyPropertyChanged
    {
        public static int flip = 0;
        public static int flip2 = 0;
        public static int selectedMonthNumber;

        enum ViewMode
        {
            Weekly,
            Monthly,
        }

        public static bool backward = false;

        private string currentViewMode;
        public string CurrentViewMode
        {
            get { return currentViewMode; }
            set
            {
                currentViewMode = value;
                OnPropertyChanged("CurrentViewMode");
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

        private bool fetchEvents =false;
        public bool FetchEvents
        {
            get { return fetchEvents; }
            set { fetchEvents = value;
            OnPropertyChanged("FetchEvents");
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

        public DateTime CurrentDate;

        Windows.Globalization.Calendar calendar = new Windows.Globalization.Calendar();
        List<WorkHours> whList = new List<WorkHours>();
        public MyProfile()
        {
            this.InitializeComponent();
            this.logIn.Text = Connection.UserName;
            try
            {
                Uri uri = new Uri(Connection.ImageUri, UriKind.Absolute);
                ImageSource imgSource = new BitmapImage(uri);
                myImage.Source = imgSource;
                ProfileImage.Source = imgSource;
            }
            catch (Exception e)
            {
                new MessageDialog(e.Message).ShowAsync();
            }

            UseInfoGrid.DataContext = LoggedInUser;
            //WeekGridView.DataContext = this;
            MonthNameTextBlock.DataContext = this;
            YearNameTextBlock.DataContext = this;
            MonthNameTextBlock2.DataContext = this;
            YearNameTextBlock2.DataContext = this;
            MonthNameTextBlock3.DataContext = this;
            YearNameTextBlock3.DataContext = this;
            CurrentDate = DateTime.Now;
            Output.DataContext = this;
            Output2.DataContext = this;
            LightDismissAnimatedPopup.DataContext = this;
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 2);
            myGrid.Opacity = 0.5;
            fetchOutlookEvents();
           
           // fillCalendar();

            PropertyChanged += (sender, e) =>
            {
                if (String.Equals(e.PropertyName, "FetchEvents"))
                {
                    //new MessageDialog("property changed").ShowAsync();
                    fillCalendar();
                }
            };

            PropertyChanged += (sender, e) =>
            {
                if (String.Equals(e.PropertyName, "ShowWeeklyCalendar"))
                {
                    //new MessageDialog("property changed").ShowAsync();
                    fillWeeklyCalendar();
                }
            };

            PropertyChanged += (sender, e) =>
            {
                if (String.Equals(e.PropertyName, "ShowMonthlyCalendar"))
                {
                    //new MessageDialog("property changed").ShowAsync();
                    fillCalendar();
                }
            };

            

        }

        List<OutLookEvent> eventList = new List<OutLookEvent>();
        public async void fetchOutlookEvents()
        {
            LiveConnectClient client = new LiveConnectClient(Connection.Session);
            //await new MessageDialog("me/events?start_time=" + calendar.Year.ToString() + "-" + (DateTime.Now.AddMonths(-6).Month).ToString() + "-" + calendar.Day.ToString() + "T00:00:00Z&end_time=" + calendar.Year.ToString() + "-" + (calendar.Month).ToString() + "-" + calendar.Day.ToString() + "T00:00:00Z").ShowAsync();
            string str = "/me/events?start_time=" + (DateTime.Now.AddMonths(-2).Year).ToString() + "-" + (DateTime.Now.AddMonths(-2).Month).ToString("d2") + "-" + (DateTime.Now.AddMonths(-2).Day).ToString("d2") + "T00:00:00Z&end_time=" + DateTime.Now.AddMonths(2).Year.ToString() + "-" + (DateTime.Now.AddMonths(2).Month).ToString("d2") + "-" + DateTime.Now.AddMonths(2).Day.ToString("d2") + "T00:00:00Z";
            Debug.WriteLine(str);
            LiveOperationResult lor = await client.GetAsync(str);
           // LiveOperationResult lor = await client.GetAsync("/me/events?start_time=2013-06-05T00:00:00Z&end_time=2014-04-28T00:00:00Z");
            dynamic d = lor.Result;
            List<object> data = null;
            IDictionary<string, object> myEvent = null;

            //string msg = "Events names: ";
            //Debug.WriteLine(msg);

            if (lor.Result.ContainsKey("data"))
            {
                data = (List<object>)lor.Result["data"];
                for (int i = 0; i < data.Count; i++)
                {
                    myEvent = (IDictionary<string, object>)data[i];
                    if (myEvent.ContainsKey("name"))
                    {
                        string Name = (string)myEvent["name"];
                        string ID = (string)myEvent["id"];
                        DateTime start_time = Convert.ToDateTime( myEvent["start_time"]);
                        DateTime end_time = Convert.ToDateTime(myEvent["end_time"]);
                        bool allDay = (bool)myEvent["is_all_day_event"];
                        if (Name.Count()>6 )
                        {
                            if(!Name.Substring(0, 6).Equals("Clinic"))
                            eventList.Add(new OutLookEvent() { Name = Name, ID = ID, start_time = start_time, end_time = end_time, isAllDay = allDay });
                        }
                        else
                        {
                            eventList.Add(new OutLookEvent() { Name = Name, ID = ID, start_time = start_time, end_time = end_time, isAllDay = allDay });
                       
                        }
                    }
                }
            }

            //List<User> uList =  await User.ReadUsersList();
            //  foreach (User u in uList)
            //  {
            //      if (!Connection.User.LiveSDKID.Equals(u.LiveSDKID))
            //      {
            //         // string str = "/" + u.LiveSDKID + "/events"; tried calendars
            //          string str = "me/contacts/calendars";
            //          lor = await client.GetAsync(str);

            //      }

            //  }

            FetchEvents = true;

        }
        public async void fillCalendar()
        {
            //Windows.Globalization.Calendar calendar = new Windows.Globalization.Calendar();

            //Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 2);
            //myGrid.Opacity = 0.5;

            List<NameValueItem> dList = new List<NameValueItem>();
            whList = (await WorkHours.ReadListAsync()).Where(w => w.EmployeeId == Connection.User.UserId ).ToList();
            int noOfDaysInTheSelectedMonth = 0;
            int shift = 0;
            string firstDay = String.Empty;

            if (flip == 0)
            {
                if (String.IsNullOrEmpty(SelectedMonth))
                    SelectedMonth = calendar.MonthAsSoloString();
                if (String.IsNullOrEmpty(SelectedYear))
                    SelectedYear = calendar.Year.ToString();
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

                flip = 0;
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
                flip = 0;
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
                dList.Add(new NameValueItem());
                shift--;
            }

            for (int i = 1; i <= noOfDaysInTheSelectedMonth; i++)
            {
                DateTime loopDate = new DateTime(Convert.ToInt32(SelectedYear), DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month, i);

                OutLookEvent myOEvent = null;
                OutLookEvent[] myOEvent2 = null;
                try
                {

                      //if (myOEvent.Name.Substring(0, 6).Equals("Clinic"))
                      //  {
                      //      return String.Empty;
                      //  }

                    myOEvent = eventList.Where(e => (e.start_time.ToString("MMMM dd, yyyy")).Equals(loopDate.ToString("MMMM dd, yyyy")) ).ToArray().First();

                    myOEvent2 = eventList.Where(e => (e.start_time.ToString("MMMM dd, yyyy")).Equals(loopDate.ToString("MMMM dd, yyyy"))).ToArray(); //&& !e.Name.Substring(0, 6).Equals("Clinic")
                }
                catch
                {

                }

                WorkHours wh = null;
                try
                {
                    wh = whList.Where(w => (w.From.ToString("MMMM dd, yyyy").Equals(loopDate.ToString("MMMM dd, yyyy")) || w.TimeOffFrom.ToString("MMMM dd, yyyy").Equals(loopDate.ToString("MMMM dd, yyyy")))).First();
                }
                catch
                {

                }

                NameValueItem nvi;

                    
                if (loopDate.ToString("MMMM dd, yyyy").Equals(CurrentDate.ToString("MMMM dd, yyyy")))
                {

                    nvi = new NameValueItem { Name = (new DateTime(Convert.ToInt32(SelectedYear), DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month, i).ToString("dddd")), 
                                                Value = i.ToString(),
                                                Info = String.Empty, 
                                                Color1 = (myOEvent != null ? "Wheat" : "White"),
                                                date = loopDate,
                                                myOutLookEvent = myOEvent, 
                                                myOutLookEvent2 = myOEvent2,
                                              Color2 = (wh == null ? "#BFB5DF" : (wh.From.TimeOfDay.Hours == 7 ? "#99FF99" : (wh.From.TimeOfDay.Hours == 15 ? "#99CCFF" : (wh.From.TimeOfDay.Hours == 23 ? "Wheat" : "#BFB5DF")))),
                                                workHours = wh ,
                                                 Height1 = (myOEvent == null && wh != null ? 0 : (myOEvent != null && wh != null) ? 45 : 90),
                                                 Height2 = (wh !=null && myOEvent == null? 90 :(wh !=null && myOEvent != null)?45:0 ),
                                              Border = (myOEvent != null || wh != null ? new Thickness(1) : new Thickness(0))
                    };
                    dList.Add(nvi); 

                }
                else
                {
                    nvi = new NameValueItem { Name = (new DateTime(Convert.ToInt32(SelectedYear), DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month, i).ToString("dddd")), 
                                              Value = i.ToString(),
                                              Info = String.Empty,
                                              Color1 = (myOEvent != null ? "Wheat" : "#BFB5DF"),
                                              date = loopDate,
                                              myOutLookEvent = myOEvent,
                                              myOutLookEvent2 = myOEvent2,
                                              Color2 = (wh == null ? "#BFB5DF" : (wh.From.TimeOfDay.Hours == 7 ? "#99FF99" : (wh.From.TimeOfDay.Hours == 15 ? "#99CCFF" : (wh.From.TimeOfDay.Hours == 23 ? "Wheat" : "#BFB5DF")))),
                                              workHours = wh ,
                                              //Height1 = (myOEvent == null && wh != null ? 0 : (myOEvent != null && wh != null) ? 30 : 60),
                                              //Height2 = (wh != null && myOEvent == null ? 60 : (wh != null && myOEvent != null) ? 30 : 0),
                                              Height1 = (myOEvent == null && wh != null ? 0 : (myOEvent != null && wh != null) ? 45 : 90),
                                              Height2 = (wh != null && myOEvent == null ? 90 : (wh != null && myOEvent != null) ? 45 : 0),
                                              Border = (myOEvent != null || wh != null ? new Thickness(1) : new Thickness(0))
                    };
                    dList.Add(nvi);
                  
                  
                }
            }
            flip = 0;
            MonthGridView.ItemsSource = dList;

            myGrid.Opacity = 1;
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 2);
            
        }



        private User loggedInUser = Connection.User;
        public User LoggedInUser
        {
            get { return loggedInUser; }
            set
            {
                loggedInUser = value;
                OnPropertyChanged("LoggedInUser");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
                //Connection.User = LoggedInUser;
                //User.UpdateUser(Connection.User);
                //new MessageDialog("Updated Successfully").ShowAsync();
            }
        }

        private async void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {

                LoggedInUser.Type = (TypeTextBox.Text);
                 Connection.User = await User.UpdateUser(LoggedInUser);

                //switch (TypeTextBox.Text)
                //{
                //    case "Doctor": LoggedInUser.Type = (User.UserType.Doctor);
                //        Connection.User = await User.UpdateUser(LoggedInUser);

                //        // new MessageDialog("Updated Successfully").ShowAsync();
                //        break;
                //    case "Nurse": LoggedInUser.Type = User.UserType.Nurse;
                //        Connection.User = await User.UpdateUser(LoggedInUser);

                //        //new MessageDialog("Updated Successfully").ShowAsync();
                //        break;
                //    case "Secretary": LoggedInUser.Type = User.UserType.Secretary;
                //        Connection.User = await User.UpdateUser(LoggedInUser);

                //        //new MessageDialog("Updated Successfully").ShowAsync();
                //        break;
                //    case "Manager": LoggedInUser.Type = User.UserType.Manager;
                //        Connection.User = await User.UpdateUser(LoggedInUser);

                //        // new MessageDialog("Updated Successfully").ShowAsync();
                //        break;
                //    default: LoggedInUser.Type = User.UserType.Undefined;
                //        Connection.User = await User.UpdateUser(LoggedInUser);

                //        //new MessageDialog("Updated Successfully").ShowAsync();
                //        break;
                //}
                // TypeTextBox.Focus(FocusState.Unfocused);

            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e) //Weekly
        {
            CurrentViewMode = ViewMode.Weekly.ToString();
            ShowMonthlyCalendar = Visibility.Collapsed;
            ShowWeeklyCalendar = Visibility.Visible;
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e) //Monthly
        {
            CurrentViewMode = ViewMode.Monthly.ToString();
            ShowMonthlyCalendar = Visibility.Visible;
            ShowWeeklyCalendar = Visibility.Collapsed;
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

        private async void MonthGridView_ItemClick(object sender, ItemClickEventArgs e)
        {

            if (((NameValueItem)e.ClickedItem).Value != null)
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

                LightDismissAnimatedPopup.HorizontalOffset = myPoint.X - 390;

                 List<NameValueItem> nviList = new List<NameValueItem>();

                PopupWidth = 200;
                PopupHeight = 0;

                if (((NameValueItem)e.ClickedItem).workHours != null)
                {
                    //popupWidth = popupWidth + 200;
                    PopupHeight = PopupHeight + 60;
                    nviList.Add(new NameValueItem() { workHours = ((NameValueItem)e.ClickedItem).workHours });

                }
                if (((NameValueItem)e.ClickedItem).myOutLookEvent != null)
                {
                    if (((NameValueItem)e.ClickedItem).myOutLookEvent2.Count() > 1)
                    {
                        PopupHeight = 60 * ((NameValueItem)e.ClickedItem).myOutLookEvent2.Count();
                        for(int i  = 0 ; i< ((NameValueItem)e.ClickedItem).myOutLookEvent2.Count();i++){
                            nviList.Add(new NameValueItem() { myOutLookEvent = ((NameValueItem)e.ClickedItem).myOutLookEvent2[i] });
                        }
                    }
                    else
                    {
                        PopupHeight = PopupHeight + 60;
                        nviList.Add(new NameValueItem(){ myOutLookEvent  = ((NameValueItem)e.ClickedItem).myOutLookEvent });
                    }

                }
               
               

                PopupGridView.ItemsSource = nviList;

                if (!LightDismissAnimatedPopup.IsOpen) { LightDismissAnimatedPopup.IsOpen = true; }

                //LightDismissAnimatedPopupTextBlock.Text = ((NameValueItem)e.ClickedItem).Value;
                //LightDismissAnimatedPopupTextBlock2.Text = ((NameValueItem)e.ClickedItem).date.ToString();
            }
        }
        private static double DipToPixel(double dip)
        {
            return (dip * DisplayProperties.LogicalDpi) / 96.0;
        }


        // Handles the Click event on the Button within the simple Popup control and simply closes it.
        //private void CloseAnimatedPopupClicked(object sender, RoutedEventArgs e)
        //{
        //    if (LightDismissAnimatedPopup.IsOpen) { LightDismissAnimatedPopup.IsOpen = false; }
        //}

        private int popupWidth = 0;
        public int PopupWidth
        {
            get { return popupWidth; }
            set
            {

                popupWidth = value;
                OnPropertyChanged("PopupWidth");
            }
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

        private Visibility showMonthlyCalendar = Visibility.Visible;
        public Visibility ShowMonthlyCalendar
        {
            get { return showMonthlyCalendar; }
            set
            {
                showMonthlyCalendar = value;
                OnPropertyChanged("ShowMonthlyCalendar");
            }
        }

        private Visibility showWeeklyCalendar = Visibility.Collapsed;
        public Visibility ShowWeeklyCalendar
        {
            get { return showWeeklyCalendar; }
            set
            {
                showWeeklyCalendar = value;
                OnPropertyChanged("ShowWeeklyCalendar");

            }
        }

        public WeekDay ReachedDayInWeeklyCalendar = null;
        public WeekDay StartDayInWeeklyCalendar = null;

      //  List<WorkHours> whList = new List<WorkHours>();
        public  void fillWeeklyCalendar()
        {


            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 2);
            myGrid.Opacity = 0.5;

            LeftMonth = String.Empty;
            LeftYear = String.Empty;
            RightMonth = String.Empty;
            RightYear = String.Empty;
         //   whList = (await WorkHours.ReadListAsync()).Where(w => w.EmployeeId == Connection.User.UserId).ToList();

            Windows.Globalization.Calendar calendar = new Windows.Globalization.Calendar();


            List<NameValueItem3> dList = new List<NameValueItem3>();

            List<WeekDay> weekList = new List<WeekDay>(7);

            if (flip2 == 0)
            {
                if (String.IsNullOrEmpty(SelectedMonth))
                    SelectedMonth = calendar.MonthAsSoloString();
                if (String.IsNullOrEmpty(SelectedYear))
                    SelectedYear = calendar.Year.ToString();



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


           

            if (SelectedWeek == 1)
            {
                //int noOfDaysInTheSelectedMonth = 0;
                //int shift = 0;
                //string firstDay = String.Empty;

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

                //fill dList

             
              //  weekList.Clear();


            }
            else if (SelectedWeek == 2 || SelectedWeek == 3 || SelectedWeek == 4)
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




            for (int i = 0; i < 192; i++)
            {
                OutLookEvent[] myOEvent2 = null;
                OutLookEvent myOEvent = null;
                WorkHours wh = null;
                if (i % 8 == 0)
                {
                    dList.Add(new NameValueItem3() { timeItem = true, time = new TimeSpan(i / 8, 0, 0), Width = 1, timeStr = (new TimeSpan(i / 8, 0, 0)).ToString(), Color1 = "#BFB5DF", Width1 = 120 }); //( new TimeSpan(i / 8, 0, 0)).ToString()
                }

                else
                {

                    DateTime loopDate = new DateTime(Convert.ToInt32(weekList.ElementAt((i % 8) - 1).Year),
                           DateTime.ParseExact(weekList.ElementAt((i % 8) - 1).Month, "MMMM", CultureInfo.CurrentCulture).Month,
                           weekList.ElementAt((i % 8) - 1).Day);

                    try
                    {
                        //  myOEvent = eventList.Where(e => (e.start_time.ToString("MMMM dd, yyyy")).Equals(loopDate.ToString("MMMM dd, yyyy"))).ToArray().First();
                        //   myOEvent = eventList.Where(e => (e.start_time.ToString("MMMM dd, yyyy")).Equals(loopDate.ToString("MMMM dd, yyyy")) && (e.start_time.TimeOfDay.Equals(new TimeSpan(i / 8, 0, 0)) || e.end_time.TimeOfDay >= (new TimeSpan(i / 8, 0, 0)))).ToArray().First();
                        //   myOEvent = eventList.Where(e => (e.start_time.ToString("MMMM dd, yyyy")).Equals(loopDate.ToString("MMMM dd, yyyy")) && (new TimeSpan(e.end_time.TimeOfDay.Hours,0,0) >= (new TimeSpan(i / 8, 0, 0)) || e.isAllDay == true)).ToArray().First();
                       // myOEvent = eventList.Where(e => (e.start_time.ToString("MMMM dd, yyyy")).Equals(loopDate.ToString("MMMM dd, yyyy")) && (((new TimeSpan(e.end_time.TimeOfDay.Hours, 0, 0) > (new TimeSpan(i / 8, 0, 0))) && (new TimeSpan(e.start_time.TimeOfDay.Hours, 0, 0) <= (new TimeSpan(i / 8, 0, 0)))) || e.isAllDay == true || ((new TimeSpan(e.start_time.TimeOfDay.Hours, 0, 0) <= (new TimeSpan(i / 8, 0, 0)))&&(TimeSpan)(e.end_time - e.start_time) <= new TimeSpan(1, 0, 0)))).ToArray().First();
                        myOEvent = eventList.Where(e => (e.start_time.ToString("MMMM dd, yyyy")).Equals(loopDate.ToString("MMMM dd, yyyy")) && (((new TimeSpan(e.end_time.TimeOfDay.Hours, 0, 0) > (new TimeSpan(i / 8, 0, 0))) && (new TimeSpan(e.start_time.TimeOfDay.Hours, 0, 0) <= (new TimeSpan(i / 8, 0, 0)))) || e.isAllDay == true || ((new TimeSpan(e.start_time.TimeOfDay.Hours, 0, 0) == (new TimeSpan(i / 8, 0, 0))) && ((TimeSpan)(e.end_time - e.start_time)).Hours <= 1))).ToArray().First();
                        myOEvent2 = eventList.Where(e => (e.start_time.ToString("MMMM dd, yyyy")).Equals(loopDate.ToString("MMMM dd, yyyy")) && (((new TimeSpan(e.end_time.TimeOfDay.Hours, 0, 0) > (new TimeSpan(i / 8, 0, 0))) && (new TimeSpan(e.start_time.TimeOfDay.Hours, 0, 0) <= (new TimeSpan(i / 8, 0, 0)))) || e.isAllDay == true || ((new TimeSpan(e.start_time.TimeOfDay.Hours, 0, 0) == (new TimeSpan(i / 8, 0, 0))) && ((TimeSpan)(e.end_time - e.start_time)).Hours <= 1))).ToArray();
                    }
                    catch
                    {

                    }

                    try
                    {
                        wh  = whList.Where(w =>(( w.From.ToString("MMMM dd, yyyy").Equals(loopDate.ToString("MMMM dd, yyyy")) && w.From.TimeOfDay.Hours<=(i / 8) && w.To.ToString("MMMM dd, yyyy").Equals(loopDate.ToString("MMMM dd, yyyy")) && w.To.TimeOfDay.Hours>=(i / 8)) || ( w.From.ToString("MMMM dd, yyyy").Equals(loopDate.ToString("MMMM dd, yyyy")) && w.From.TimeOfDay.Hours<=i / 8 && w.To.Date.Equals(loopDate.AddDays(1))) || (w.From.Date.Equals(loopDate.AddDays(-1).Date) && w.To.Date.Equals(loopDate.Date) && w.To.TimeOfDay.Hours >= (i/8)) || (w.TimeOffFrom.Date.Equals(loopDate.Date) && w.TimeOffFrom.TimeOfDay.Hours<=(i/8) && w.TimeOffTo.TimeOfDay.Hours>=(i/8) ) ) ).ToArray().First();
                    }
                    catch
                    {

                    }

                    dList.Add(new NameValueItem3()
                    {
                        timeItem = false,
                        time = new TimeSpan(i / 8, 0, 0),
                        Width = 3,
                        weekDay = weekList.ElementAt((i % 8) - 1),
                        Color1 = (myOEvent != null ? "Wheat" : "#BFB5DF"),
                        myOutLookEvent = myOEvent,
                        myOutLookEvent2 = myOEvent2,
                        workHours = wh,
                        Border = (myOEvent != null || wh != null ? new Thickness(1) : new Thickness(0)),
                        Color2 = (wh == null ? "#BFB5DF" : (wh.From.TimeOfDay.Hours == 7 ? "#99FF99" : (wh.From.TimeOfDay.Hours == 15 ? "#99CCFF" : (wh.From.TimeOfDay.Hours == 23 ? "Wheat" : "#BFB5DF")))),
                        Width1 = (myOEvent == null && wh != null ? 0 : (myOEvent != null && wh != null) ? 60 : 120),
                        Width2 = (wh != null && myOEvent == null ? 120 : (wh != null && myOEvent != null) ? 60 : 0),
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
                                try
                                {
                                    Sunday2.Text = (myOEvent.isAllDay ? myOEvent.Name : null);
                                }
                                catch
                                {
                                    Sunday2.Text = String.Empty;
                                }
                                break;
                            case 2: Monday.Text = (new DateTime(Convert.ToInt32(weekList.ElementAt((i % 8) - 1).Year),
                                 DateTime.ParseExact(weekList.ElementAt((i % 8) - 1).Month, "MMMM", CultureInfo.CurrentCulture).Month,
                                 weekList.ElementAt((i % 8) - 1).Day)
                                 ).DayOfWeek.ToString() + " " + weekList.ElementAt((i % 8) - 1).Day;
                                try
                                {
                                    Monday2.Text = (myOEvent.isAllDay ? myOEvent.Name : null);
                                }
                                catch
                                {
                                    Monday2.Text = String.Empty;
                                }
                                break;
                            case 3: Tuesday.Text = (new DateTime(Convert.ToInt32(weekList.ElementAt((i % 8) - 1).Year),
                                DateTime.ParseExact(weekList.ElementAt((i % 8) - 1).Month, "MMMM", CultureInfo.CurrentCulture).Month,
                                weekList.ElementAt((i % 8) - 1).Day)
                                ).DayOfWeek.ToString() + " " + weekList.ElementAt((i % 8) - 1).Day;
                                try
                                {
                                    Tuesday2.Text = (myOEvent.isAllDay ? myOEvent.Name : null);
                                }
                                catch
                                {
                                    Tuesday2.Text = String.Empty;
                                }
                                break;
                            case 4: Wednesday.Text = (new DateTime(Convert.ToInt32(weekList.ElementAt((i % 8) - 1).Year),
                                DateTime.ParseExact(weekList.ElementAt((i % 8) - 1).Month, "MMMM", CultureInfo.CurrentCulture).Month,
                                weekList.ElementAt((i % 8) - 1).Day)
                                ).DayOfWeek.ToString() + " " + weekList.ElementAt((i % 8) - 1).Day;
                                try
                                {
                                    Wednesday2.Text = (myOEvent.isAllDay ? myOEvent.Name : null);
                                }
                                catch
                                {
                                    Wednesday2.Text = String.Empty;
                                }
                                break;
                            case 5: Thursday.Text = (new DateTime(Convert.ToInt32(weekList.ElementAt((i % 8) - 1).Year),
                                DateTime.ParseExact(weekList.ElementAt((i % 8) - 1).Month, "MMMM", CultureInfo.CurrentCulture).Month,
                                weekList.ElementAt((i % 8) - 1).Day)
                                ).DayOfWeek.ToString() + " " + weekList.ElementAt((i % 8) - 1).Day;
                                try
                                {
                                    Thursday2.Text = (myOEvent.isAllDay ? myOEvent.Name : null);
                                }
                                catch
                                {
                                    Thursday2.Text = String.Empty;
                                }
                                break;
                            case 6: Friday.Text = (new DateTime(Convert.ToInt32(weekList.ElementAt((i % 8) - 1).Year),
                                DateTime.ParseExact(weekList.ElementAt((i % 8) - 1).Month, "MMMM", CultureInfo.CurrentCulture).Month,
                                weekList.ElementAt((i % 8) - 1).Day)
                                ).DayOfWeek.ToString() + " " + weekList.ElementAt((i % 8) - 1).Day;
                                try
                                {
                                    Friday2.Text = (myOEvent.isAllDay ? myOEvent.Name : null);
                                }
                                catch
                                {
                                    Friday2.Text = String.Empty;
                                }
                                break;
                            case 7:


                                Saturday.Text = (new DateTime(Convert.ToInt32(weekList.ElementAt((i % 8) - 1).Year),
                                DateTime.ParseExact(weekList.ElementAt((i % 8) - 1).Month, "MMMM", CultureInfo.CurrentCulture).Month,
                                weekList.ElementAt((i % 8) - 1).Day)
                                ).DayOfWeek.ToString() + " " + weekList.ElementAt((i % 8) - 1).Day;
                                try
                                {
                                    Saturday2.Text = (myOEvent.isAllDay ? myOEvent.Name : null);
                                }
                                catch
                                {
                                    Saturday2.Text = String.Empty;
                                }
                                break;


                        }
                        // weekList.ElementAt((i-1)%8)
                    }
                }
            }

            flip2 = 0;
            backward = false;
            WeekGridView.ItemsSource = dList;
            weekList.Clear();
            Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
                WeekGridView.ScrollIntoView(dList.ElementAt(147));
            });


            myGrid.Opacity = 1;
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 2);
            

        }

        private void previousMonthButton2_Click(object sender, RoutedEventArgs e)
        {
            flip2 = -1;
            backward = true;
            fillWeeklyCalendar();
        }

        private void nextMonthButton2_Click(object sender, RoutedEventArgs e)
        {
            flip2 = 1;
            backward = false;
            fillWeeklyCalendar();
        }

        //private async void MonthGridView_Tapped(object sender, TappedRoutedEventArgs e)
        //{
            //// The y coordinate of the tapped position
            //double y = e.GetPosition((UIElement)sender).Y;

            //// The x coordinate of the tapped position
            //double x = e.GetPosition((UIElement)sender).X;

            //GridView eventsContainer = sender as GridView;

            //eventsContainer.Measure(new Size(123.49, 60));
            //Size size = eventsContainer.DesiredSize;

            //// The tapped item's x index
            //int itemX = (int)(x / size.Width);

            //// The tapped item's y index
            //int itemY = (int)(y / size.Height);

            //// Get the index of tapped item
            //int index = (int)(itemY * (int)(MonthGridView.ActualWidth / size.Width)) + itemX;

            //if (index < MonthGridView.Items.Count)
            //{
            //    NameValueItem nvi = MonthGridView.Items[index] as NameValueItem;
               
            //        GridViewItem tappedItem = eventsContainer.ItemContainerGenerator.ContainerFromIndex(index) as GridViewItem;
            //        eventsContainer.SelectionMode = ListViewSelectionMode.Multiple;
               
            //        tappedItem.IsSelected = true;

            //        //eventsContainer.SelectedItems.OfType<NameValueItem>()
            //        foreach (object o in eventsContainer.SelectedItems)
            //        {
            //            await new MessageDialog(((NameValueItem)o).date.ToString()).ShowAsync();
            //        }
              
            //}


        //}

        private async void WeekGridView_ItemClick(object sender, ItemClickEventArgs e)
        {

            // await new MessageDialog(((NameValueItem3)e.ClickedItem).weekDay.Year + ((NameValueItem3)e.ClickedItem).weekDay.Month + ((NameValueItem3)e.ClickedItem).weekDay.Day).ShowAsync();

            //try
            //{
                
               // SelectedItems.Add(SelectedString);
            //}
            //catch (Exception eee)
            //{

            //}
           // await new MessageDialog(((NameValueItem3)e.ClickedItem).weekDay.Year + ((NameValueItem3)e.ClickedItem).weekDay.Month + ((NameValueItem3)e.ClickedItem).weekDay.Day).ShowAsync();

            // await new MessageDialog(((NameValueItem3)e.ClickedItem).weekDay.Year + ((NameValueItem3)e.ClickedItem).weekDay.Month + ((NameValueItem3)e.ClickedItem).weekDay.Day).ShowAsync();
            //foreach (object o in WeekGridView.SelectedItems)
            //{
            //    await new MessageDialog(((NameValueItem3)o).weekDay.Year + ((NameValueItem3)o).weekDay.Month + ((NameValueItem3)o).weekDay.Day).ShowAsync();
            //}

            SelectedString = ((NameValueItem3)e.ClickedItem);
            if (((NameValueItem3)e.ClickedItem) != null)
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

                LightDismissAnimatedPopup2.VerticalOffset = myPoint.Y - 200;

                LightDismissAnimatedPopup2.HorizontalOffset = myPoint.X - 390;

                List<NameValueItem3> nviList = new List<NameValueItem3>();

                PopupWidth = 200;
                PopupHeight = 0;

                if (((NameValueItem3)e.ClickedItem).workHours != null)
                {
                    //popupWidth = popupWidth + 200;
                    PopupHeight = PopupHeight + 60;
                    nviList.Add(new NameValueItem3() { workHours = ((NameValueItem3)e.ClickedItem).workHours });

                }
                if (((NameValueItem3)e.ClickedItem).myOutLookEvent != null)
                {
                    if (((NameValueItem3)e.ClickedItem).myOutLookEvent2.Count() > 1)
                    {
                        PopupHeight = 60 * ((NameValueItem3)e.ClickedItem).myOutLookEvent2.Count();
                        for (int i = 0; i < ((NameValueItem3)e.ClickedItem).myOutLookEvent2.Count(); i++)
                        {
                            nviList.Add(new NameValueItem3() { myOutLookEvent = ((NameValueItem3)e.ClickedItem).myOutLookEvent2[i] });
                        }
                    }
                    else
                    {
                        PopupHeight = PopupHeight + 60;
                        nviList.Add(new NameValueItem3() { myOutLookEvent = ((NameValueItem3)e.ClickedItem).myOutLookEvent });
                    }

                }



                PopupGridView2.ItemsSource = nviList;

                if (!LightDismissAnimatedPopup2.IsOpen) { LightDismissAnimatedPopup2.IsOpen = true; }

                //LightDismissAnimatedPopupTextBlock.Text = ((NameValueItem)e.ClickedItem).Value;
                //LightDismissAnimatedPopupTextBlock2.Text = ((NameValueItem)e.ClickedItem).date.ToString();
            }

        }

        //public async void readWeeklySelectedItems()
        //{
        //    MultiSelectBehavior msb = new MultiSelectBehavior();
        //    foreach(object o in msb.SelectedItems)
        //    {
        //        await new MessageDialog(((NameValueItem3)o).weekDay.Month).ShowAsync();
        //    }
        //}

        private NameValueItem3 selectedString;
        public NameValueItem3 SelectedString
        {
            get { return selectedString; }
            set
            {
                selectedString = value;
                OnPropertyChanged("SelectedString"); }
        }

        private async void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
           
            //foreach (object o in WeekGridView.SelectedItems)
            //    {
            //        await new MessageDialog(((NameValueItem3)o).weekDay.Month).ShowAsync();
            //    }

           
        }

        //private async void WeekGridView_PointerPressed(object sender, PointerRoutedEventArgs e)
        //{
        //    MultiSelectBehavior msb = new MultiSelectBehavior();
        //    foreach(object o in msb.SelectedItems)
        //    {
        //        await new MessageDialog(((NameValueItem3)o).weekDay.Month).ShowAsync();
        //    }
        //}

        //List<NameValueItem3> SelectedItems = new List<NameValueItem3>();

        //private void WeekGridView_Tapped(object sender, TappedRoutedEventArgs e)
        //{
        //    // The y coordinate of the tapped position
        //    double y = e.GetPosition((UIElement)sender).Y;

        //    // The x coordinate of the tapped position
        //    double x = e.GetPosition((UIElement)sender).X;

        //    GridView eventsContainer = sender as GridView;

        //    eventsContainer.Measure(new Size(120, 40));
        //    Size size = eventsContainer.DesiredSize;

        //    // The tapped item's x index
        //    int itemX = (int)(x / size.Width);

        //    // The tapped item's y index
        //    int itemY = (int)(y / size.Height);

        //    // Get the index of tapped item
        //    int index = (int)(itemY * (int)(WeekGridView.ActualWidth / size.Width)) + itemX;

        //    if ((index < WeekGridView.Items.Count) && (index % 8 != 0))
        //    {
        //        NameValueItem3 nvi = WeekGridView.Items[index] as NameValueItem3;

        //        GridViewItem tappedItem = eventsContainer.ItemContainerGenerator.ContainerFromIndex(index) as GridViewItem;
        //        eventsContainer.SelectionMode = ListViewSelectionMode.Multiple;

        //        tappedItem.IsSelected = true;

        //        //eventsContainer.SelectedItems.OfType<NameValueItem>()
        //        // foreach (object o in eventsContainer.SelectedItems)
        //        //{
        //        //    await new MessageDialog(((NameValueItem)o).date.ToString()).ShowAsync();
        //        //}

        //    }
        //}


    }

    //    LiveOperationResult lor = await client.GetAsync("me/events");
    //    dynamic d = lor.Result;
    //    List<object> data = null;
    //    IDictionary<string, object> myEvent = null;

    //    string msg = "Events names: ";
    //    Debug.WriteLine(msg);

    //    if (lor.Result.ContainsKey("data"))
    //    {
    //        data = (List<object>)lor.Result["data"];
    //        for (int i = 0; i < data.Count; i++)
    //        {
    //            myEvent = (IDictionary<string, object>)data[i];
    //            if (myEvent.ContainsKey("name"))
    //            {
    //                string Name = (string)myEvent["name"];
    //                string ID = (string)myEvent["id"];
    //                msg = string.Format("Name = {0}, ID = {1}", Name, ID);
    //                Debug.WriteLine(msg);
    //            }
    //        }
    //    }












    //public class NameValueItem
    //  {
    //      public string Name { get; set; }
    //      public string Value { get; set; }

    //      public string Info { get; set; }

    //      public DateTime date { get; set; }

    //      public string Color { get; set; }
    //  }

    //class SpecificGridView : GridView
    //{
    //    protected override void PrepareContainerForItemOverride(
    //      DependencyObject element, object item)
    //    {
    //        // being lazy because I bound to an anonymous data type  
    //        dynamic lateBoundItem = item;

    //        int sizeFactor = (int)lateBoundItem.Width;

    //        element.SetValue(VariableSizedWrapGrid.ColumnSpanProperty, sizeFactor);
    //        //element.SetValue(VariableSizedWrapGrid.RowSpanProperty, sizeFactor);

    //        base.PrepareContainerForItemOverride(element, item);


    //    }
    //}

    //public class NameValueItem3
    //{

    //    public string Color { get; set; }

    //    public bool timeItem { get; set; }

    //    public WeekDay weekDay { get; set; }

    //    public TimeSpan time { get; set; }

    //    public string timeStr { get; set; }

    //    public int Width { get; set; }
    //}

    //public class WeekDay
    //{
    //    public string Year { get; set; }
    //    public string Month { get; set; }

    //    public int Day { get; set; }
    //}

    public class OutLookEvent
    {
        public string Name { get; set; }
        public string ID { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
        public bool isAllDay { get; set; }
    }

}
