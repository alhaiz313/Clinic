using Clinic.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public sealed partial class Staff : Common.LayoutAwarePage, INotifyPropertyChanged
    {

        public static int flip = 0;
        public static int flip2 = 0;
        public static int selectedMonthNumber;

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
        Windows.Globalization.Calendar calendar = new Windows.Globalization.Calendar();

        public DateTime CurrentDate;
        public Staff()
        {
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


            MonthNameTextBlock.DataContext = this;
            YearNameTextBlock.DataContext = this;
            CurrentDate = DateTime.Now;
            Output.DataContext = this;
            AllEmployees = true;
            TitlesGrid2.DataContext = this;
            TitlesGrid.DataContext = this;
            EmployeeGridView.DataContext = this;
            //TitlesGrid2.Visibility = Visibility.Collapsed;
            MonthNameTextBlock2.DataContext = this;
            YearNameTextBlock2.DataContext = this;
            MonthNameTextBlock3.DataContext = this;
            YearNameTextBlock3.DataContext = this;
            CurrentDate = DateTime.Now;
            Output2.DataContext = this;
            fillDragMenuGridView();
            fillDragMenuGridView2();


            //Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 2);
            //myGrid.Opacity = 0.5;

            fillWeeklyCalendar();

            //myGrid.Opacity = 1;
            //Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 2);

        }

        private User selectedUser;
        public User SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }

        List<StaffItem> siList;
        bool stay = false;
        public void fillDragMenuGridView()
        {
            //List<WorkHours> whList = new List<WorkHours>();
            siList = new List<StaffItem>();
            WorkHours wh = new WorkHours() { From = new DateTime(0001, 01, 01) + new TimeSpan(7, 0, 0), To = new DateTime(0001, 01, 01) + new TimeSpan(15, 0, 0) };
            StaffItem si = new StaffItem() { workHours = wh, Color = "#99FF99" };
            siList.Add(si);
            WorkHours wh2 = new WorkHours() { From = new DateTime(0001, 01, 01) + new TimeSpan(15, 0, 0), To = new DateTime(0001, 01, 01) + new TimeSpan(23, 0, 0) };
            StaffItem si1 = new StaffItem() { workHours = wh2, Color = "#99CCFF" };
            siList.Add(si1);
            WorkHours wh3 = new WorkHours() { From = new DateTime(0001, 01, 01) + new TimeSpan(23, 0, 0), To = new DateTime(0001, 01, 01) + new TimeSpan(7, 0, 0) };
            StaffItem si2 = new StaffItem() { workHours = wh3, Color = "Wheat" };
            siList.Add(si2);
            DragMenuGridView.ItemsSource = siList;
        }

        List<StaffItem> siList2;
        public void fillDragMenuGridView2()
        {
            //List<WorkHours> whList = new List<WorkHours>();
            siList2 = new List<StaffItem>();
            //WorkHours wh = new WorkHours() { From = new DateTime(0001, 01, 01) + new TimeSpan(7, 0, 0), To = new DateTime(0001, 01, 01) + new TimeSpan(15, 0, 0) };
            //StaffItem si = new StaffItem() { workHours = wh, Color = "#99FF99" };
            //siList2.Add(si);
            //WorkHours wh2 = new WorkHours() { From = new DateTime(0001, 01, 01) + new TimeSpan(15, 0, 0), To = new DateTime(0001, 01, 01) + new TimeSpan(23, 0, 0) };
            //StaffItem si1 = new StaffItem() { workHours = wh2, Color = "#99CCFF" };
            //siList2.Add(si1);
            //WorkHours wh3 = new WorkHours() { From = new DateTime(0001, 01, 01) + new TimeSpan(23, 0, 0), To = new DateTime(0001, 01, 01) + new TimeSpan(7, 0, 0) };
            //StaffItem si2 = new StaffItem() { workHours = wh3, Color = "Wheat" };
            WorkHours wh = new WorkHours() { TimeOffReason = "Time Off" };
            StaffItem si = new StaffItem() { workHours = wh };
            siList2.Add(si);
            wh = new WorkHours() { TimeOffReason = "Vacation" };
            si = new StaffItem() { workHours = wh };
            siList2.Add(si);
            wh = new WorkHours() { TimeOffReason = "Business Related" };
            si = new StaffItem() { workHours = wh };
            siList2.Add(si);
            wh = new WorkHours() { TimeOffReason = "Sick Leave" };
            si = new StaffItem() { workHours = wh };
            siList2.Add(si);
            wh = new WorkHours() { TimeOffReason = "Family Emergency" };
            si = new StaffItem() { workHours = wh };
            siList2.Add(si);
            wh = new WorkHours() { TimeOffReason = "Absent" };
            si = new StaffItem() { workHours = wh };
            siList2.Add(si);


            DragMenuGridView2.ItemsSource = siList2;
        }

        List<User> empList;
        List<StaffItem> dList;
        List<WeekDay> weekList = new List<WeekDay>(7);
        public async void fillWeeklyCalendar()
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 2);
            myGrid.Opacity = 0.5;

            if(SelectedWeek!=0 && flip2==0){
                    stay = true;
	            }
             if (!stay)
             {
                 LeftMonth = String.Empty;
                 LeftYear = String.Empty;
                 RightMonth = String.Empty;
                 RightYear = String.Empty;
             }
            Windows.Globalization.Calendar calendar = new Windows.Globalization.Calendar();


            dList = new List<StaffItem>();

           

            if (flip2 == 0)
            {
                if (String.IsNullOrEmpty(SelectedMonth))
                    SelectedMonth = calendar.MonthAsSoloString();
                if (String.IsNullOrEmpty(SelectedYear))
                    SelectedYear = calendar.Year.ToString();

                if (!stay)
                    SelectedWeek = 1;
                //else
                //   stay = true;

              

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

            if(!stay)
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

                    //fill dList


                }
            }
            else if (SelectedWeek == 2 || SelectedWeek == 3 || SelectedWeek == 4)
            {
                if (!stay) { 
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

            empList = await User.ReadUsersList();
            List<WorkHours> whList = await WorkHours.ReadListAsync();

            int total = 0;
            for (int i = 0; i < empList.Count * 9; i++)
            {

                OutLookEvent myOEvent = null;
                if (i % 9 == 0)
                {
                    //dList.Add(new NameValueItem3() { timeItem = true, time = new TimeSpan(i / 9, 0, 0), Width = 4, timeStr = "Employee Name", Color = "AliceBlue" }); //( new TimeSpan(i / 9, 0, 0)).ToString()
                    dList.Add(new StaffItem() { Employee = empList.ElementAt(i / 9), Color = "#BFB5DF", Width = 4, i = i });
                    //fill employee names
                }

                else if ((i % 9) - 1 == 7)
                {
                    //calculate work hours
                    //dList.Add(new NameValueItem3() { timeItem = true, time = new TimeSpan(i / 9, 0, 0), Width = 1, timeStr = "Total", Color = "AliceBlue" }); //( new TimeSpan(i / 9, 0, 0)).ToString()
                    dList.Add(new StaffItem() { Total = total, Width = 1, Color = "#BFB5DF", i = i });
                    total = 0;
                }

                else
                {

                    DateTime loopDate = new DateTime(Convert.ToInt32(weekList.ElementAt((i % 9) - 1).Year),
                           DateTime.ParseExact(weekList.ElementAt((i % 9) - 1).Month, "MMMM", CultureInfo.CurrentCulture).Month,
                           weekList.ElementAt((i % 9) - 1).Day);

                    WorkHours temp;
                    try
                    {
                        temp = whList.Where(w => w.EmployeeId == empList.ElementAt(i / 9).UserId && (w.From.ToString("MMMM dd, yyyy").Equals(loopDate.ToString("MMMM dd, yyyy")) || w.TimeOffFrom.ToString("MMMM dd, yyyy").Equals(loopDate.ToString("MMMM dd, yyyy")))).First(); ;
                        total = (String.IsNullOrEmpty(temp.TimeOffReason) ? (total + 8) : total);


                    }
                    catch
                    {
                        temp = null;
                    }
                    //  dList.Add(new NameValueItem3() { timeItem = false, time = new TimeSpan(i / 9, 0, 0), Width = 3, weekDay = weekList.ElementAt((i % 9) - 1), Color = (myOEvent != null ? "Wheat" : "AliceBlue"), myOutLookEvent = myOEvent });

                    dList.Add(new StaffItem() { workHours = temp, Color = (temp == null ? "#BFB5DF" : (temp.From.TimeOfDay.Hours == 7 ? "#99FF99" : (temp.From.TimeOfDay.Hours == 15 ? "#99CCFF" : (temp.From.TimeOfDay.Hours == 23 ? "Wheat" : "#BFB5DF")))), Width = 3, i = i, date = loopDate, border = (temp != null ? new Thickness(1) : new Thickness(0)) });

                    //increment total

                    if (i > 0 || i < 8)
                    {
                        //  if(myOEvent.isAllDay)
                        switch (i)
                        {
                            case 1: Sunday.Text = (new DateTime(Convert.ToInt32(weekList.ElementAt((i % 9) - 1).Year),
                                DateTime.ParseExact(weekList.ElementAt((i % 9) - 1).Month, "MMMM", CultureInfo.CurrentCulture).Month,
                                weekList.ElementAt((i % 9) - 1).Day)
                                ).DayOfWeek.ToString() + " " + weekList.ElementAt((i % 9) - 1).Day;

                                break;
                            case 2: Monday.Text = (new DateTime(Convert.ToInt32(weekList.ElementAt((i % 9) - 1).Year),
                                 DateTime.ParseExact(weekList.ElementAt((i % 9) - 1).Month, "MMMM", CultureInfo.CurrentCulture).Month,
                                 weekList.ElementAt((i % 9) - 1).Day)
                                 ).DayOfWeek.ToString() + " " + weekList.ElementAt((i % 9) - 1).Day;

                                break;
                            case 3: Tuesday.Text = (new DateTime(Convert.ToInt32(weekList.ElementAt((i % 9) - 1).Year),
                                DateTime.ParseExact(weekList.ElementAt((i % 9) - 1).Month, "MMMM", CultureInfo.CurrentCulture).Month,
                                weekList.ElementAt((i % 9) - 1).Day)
                                ).DayOfWeek.ToString() + " " + weekList.ElementAt((i % 9) - 1).Day;

                                break;
                            case 4: Wednesday.Text = (new DateTime(Convert.ToInt32(weekList.ElementAt((i % 9) - 1).Year),
                                DateTime.ParseExact(weekList.ElementAt((i % 9) - 1).Month, "MMMM", CultureInfo.CurrentCulture).Month,
                                weekList.ElementAt((i % 9) - 1).Day)
                                ).DayOfWeek.ToString() + " " + weekList.ElementAt((i % 9) - 1).Day;

                                break;
                            case 5: Thursday.Text = (new DateTime(Convert.ToInt32(weekList.ElementAt((i % 9) - 1).Year),
                                DateTime.ParseExact(weekList.ElementAt((i % 9) - 1).Month, "MMMM", CultureInfo.CurrentCulture).Month,
                                weekList.ElementAt((i % 9) - 1).Day)
                                ).DayOfWeek.ToString() + " " + weekList.ElementAt((i % 9) - 1).Day;

                                break;
                            case 6: Friday.Text = (new DateTime(Convert.ToInt32(weekList.ElementAt((i % 9) - 1).Year),
                                DateTime.ParseExact(weekList.ElementAt((i % 9) - 1).Month, "MMMM", CultureInfo.CurrentCulture).Month,
                                weekList.ElementAt((i % 9) - 1).Day)
                                ).DayOfWeek.ToString() + " " + weekList.ElementAt((i % 9) - 1).Day;

                                break;
                            case 7:


                                Saturday.Text = (new DateTime(Convert.ToInt32(weekList.ElementAt((i % 9) - 1).Year),
                                DateTime.ParseExact(weekList.ElementAt((i % 9) - 1).Month, "MMMM", CultureInfo.CurrentCulture).Month,
                                weekList.ElementAt((i % 9) - 1).Day)
                                ).DayOfWeek.ToString() + " " + weekList.ElementAt((i % 9) - 1).Day;

                                break;


                        }

                    }
                }


            }

            flip2 = 0;
            backward = false;
            WeekGridView.ItemsSource = dList;
            stay  = false;
          //  weekList.Clear();

            //Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            //{
            //    WeekGridView.ScrollIntoView(dList.ElementAt(147));
            //});
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

        public WeekDay ReachedDayInWeeklyCalendar = null;
        public WeekDay StartDayInWeeklyCalendar = null;

        private void WeekGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            SelectedString = ((StaffItem)e.ClickedItem);
            //myCheckBox.IsChecked = false;
            try
            {
                myCheckBox.IsChecked = SelectedString.workHours.OnCallOnly;
            }
            catch
            {
                myCheckBox.IsChecked = false;
            }

            if (SelectedString.workHours != null && SelectedString.workHours.From.TimeOfDay.Hours == 7)
            {
                DayShiftCheckBox.IsChecked = true;
            }
            else
            {
                DayShiftCheckBox.IsChecked = false;
            }

            if (SelectedString.workHours != null && SelectedString.workHours.From.TimeOfDay.Hours == 15)
            {
                SwingShiftCheckBox.IsChecked = true;
            }
            else
            {
                SwingShiftCheckBox.IsChecked = false;
            }

            if (SelectedString.workHours != null && SelectedString.workHours.From.TimeOfDay.Hours == 23)
            {
                NightShiftCheckBox.IsChecked = true;
            }
            else
            {
                NightShiftCheckBox.IsChecked = false;
            }

            if (SelectedString.workHours != null && !String.IsNullOrEmpty(SelectedString.workHours.TimeOffReason))
            {
                TimeOffCheckBox.IsChecked = true;
            }
            else
            {
                TimeOffCheckBox.IsChecked = false;
            }

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

            LightDismissAnimatedPopup.VerticalOffset = myPoint.Y;

            LightDismissAnimatedPopup.HorizontalOffset = myPoint.X;

            if (!LightDismissAnimatedPopup.IsOpen) { LightDismissAnimatedPopup.IsOpen = true; }



        }

        private StaffItem selectedString;
        public StaffItem SelectedString
        {
            get { return selectedString; }
            set
            {
                selectedString = value;
                OnPropertyChanged("SelectedString");
            }
        }





        private static double DipToPixel(double dip)
        {
            return (dip * DisplayProperties.LogicalDpi) / 96.0;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)//Day
        {


            WorkHours wh = SelectedString.workHours;
            if ((bool)DayShiftCheckBox.IsChecked && wh == null)
            {

                List<WorkHours> whList = await WorkHours.ReadListAsync();
                int count = whList.Where(w => w.From.ToString("MMMM dd, yyyy").Equals(SelectedString.date.ToString("MMMM dd, yyyy")) && w.From.TimeOfDay.Hours == 7).Count();

                TimeSpan ts1 = new TimeSpan(7, 0, 0);
                TimeSpan ts2 = new TimeSpan(15, 0, 0);
                wh = new WorkHours() { EmployeeId = empList.ElementAt(SelectedString.i / 9).UserId, From = SelectedString.date + ts1, To = SelectedString.date + ts2, OnCallOnly = false, BreakTimeFrom = new TimeSpan(ts1.Hours + count, 0, 0), BreakTimeTo = new TimeSpan(ts2.Hours + count + 1, 0, 0), TimeOffReason = String.Empty };

                //Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 2);
                //myGrid.Opacity = 0.5;
                await WorkHours.InsertNewWorkHours(wh);

                fillWeeklyCalendar();
                //myGrid.Opacity = 1;
                //Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 2);


            }
            else if (!(bool)DayShiftCheckBox.IsChecked)
            {
                //Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 2);
                //myGrid.Opacity = 0.5;

                await WorkHours.DeleteWorkHours(wh);
                fillWeeklyCalendar();

                //myGrid.Opacity = 1;
                //Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 2);

            }

        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)//Swing
        {

            WorkHours wh = SelectedString.workHours;
            if ((bool)SwingShiftCheckBox.IsChecked && wh == null)
            {

                List<WorkHours> whList = await WorkHours.ReadListAsync();
                int count = whList.Where(w => w.From.ToString("MMMM dd, yyyy").Equals(SelectedString.date.ToString("MMMM dd, yyyy")) && w.From.TimeOfDay.Hours == 15).Count();

                TimeSpan ts1 = new TimeSpan(15, 0, 0);
                TimeSpan ts2 = new TimeSpan(23, 0, 0);
                wh = new WorkHours() { EmployeeId = empList.ElementAt(SelectedString.i / 9).UserId, From = SelectedString.date + ts1, To = SelectedString.date + ts2, OnCallOnly = false, BreakTimeFrom = new TimeSpan(16 + count, 0, 0), BreakTimeTo = new TimeSpan(16 + count + 1, 0, 0), TimeOffReason = String.Empty };

                //Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 2);
                //myGrid.Opacity = 0.5;

                await WorkHours.InsertNewWorkHours(wh);
                fillWeeklyCalendar();

                //myGrid.Opacity = 1;
                //Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 2);

            }
            else if (!(bool)SwingShiftCheckBox.IsChecked)
            {
                //Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 2);
                //myGrid.Opacity = 0.5;

                await WorkHours.DeleteWorkHours(wh);
                fillWeeklyCalendar();

                //myGrid.Opacity = 1;
                //Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 2);

            }
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)//Night
        {
            WorkHours wh = SelectedString.workHours;
            if ((bool)NightShiftCheckBox.IsChecked && wh == null)
            {
                List<WorkHours> whList = await WorkHours.ReadListAsync();
                int count = whList.Where(w => w.From.ToString("MMMM dd, yyyy").Equals(SelectedString.date.ToString("MMMM dd, yyyy")) && w.From.TimeOfDay.Hours == 15).Count();

                TimeSpan ts1 = new TimeSpan(23, 0, 0);
                TimeSpan ts2 = new TimeSpan(7, 0, 0);
                wh = new WorkHours() { EmployeeId = empList.ElementAt(SelectedString.i / 9).UserId, From = SelectedString.date + ts1, To = SelectedString.date.AddDays(1) + ts2, OnCallOnly = false, BreakTimeFrom = new TimeSpan(0 + count, 0, 0), BreakTimeTo = new TimeSpan(0 + count + 1, 0, 0), TimeOffReason = String.Empty };

                //Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 2);
                //myGrid.Opacity = 0.5;

                await WorkHours.InsertNewWorkHours(wh);
                fillWeeklyCalendar();

                //myGrid.Opacity = 1;
                //Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 2);

            }
            else if (!(bool)NightShiftCheckBox.IsChecked)
            {

                //Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 2);
                //myGrid.Opacity = 0.5;

                await WorkHours.DeleteWorkHours(wh);
                fillWeeklyCalendar();

                //myGrid.Opacity = 1;
                //Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 2);

            }
        }

        private async void Button_Click_3(object sender, RoutedEventArgs e) // Only On Call
        {
            WorkHours wh = SelectedString.workHours;
            wh.OnCallOnly = (bool)myCheckBox.IsChecked;

            //Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 2);
            //myGrid.Opacity = 0.5;

            await WorkHours.UpdateWorkHours(wh);
            fillWeeklyCalendar();


            //myGrid.Opacity = 1;
            //Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 2);

        }

        private async void Button_Click_4(object sender, RoutedEventArgs e) //time off
        {

            WorkHours wh = SelectedString.workHours;
            if ((bool)TimeOffCheckBox.IsChecked && wh == null)
            {
                wh = new WorkHours() { EmployeeId = empList.ElementAt(SelectedString.i / 9).UserId, TimeOffFrom = SelectedString.date, TimeOffTo = SelectedString.date + new TimeSpan(23, 0, 0), TimeOffReason = "Time Off" };

                //Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 2);
                //myGrid.Opacity = 0.5;

                await WorkHours.InsertNewWorkHours(wh);
                fillWeeklyCalendar();

                //myGrid.Opacity = 1;
                //Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 2);


            }
            else if (!(bool)TimeOffCheckBox.IsChecked)
            {

                //Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 2);
                //myGrid.Opacity = 0.5;

                await WorkHours.DeleteWorkHours(wh);
                fillWeeklyCalendar();

                //myGrid.Opacity = 1;
                //Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 2);

            }
        }


        private async void DragMenuGridView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            //  e.Data.SetData("itemIndex", storeData.Collection.IndexOf(e.Items[0] as Item).ToString());
            e.Data.SetData("ItemNumber", siList.IndexOf(e.Items[0] as StaffItem).ToString());
            //await new MessageDialog(siList.IndexOf(e.Items[0] as StaffItem).ToString()).ShowAsync();
        }

        private async void WeekGridView_Drop(object sender, DragEventArgs e)
        {
            bool success = false;
            StaffItem si = null;

            try
            {
                string itemIndexString = await e.Data.GetView().GetTextAsync("ItemNumber"); //which shift
                si = siList.ElementAt(int.Parse(itemIndexString));
                success = true;
            }
            catch
            {
                //string itemIndexString = await e.Data.GetView().GetTextAsync("ItemNumber2"); //which shift
                //StaffItem si = siList2.ElementAt(int.Parse(itemIndexString));
                success = false;
            }
            if (!success)
            {
                string itemIndexString = await e.Data.GetView().GetTextAsync("ItemNumber2"); //which shift
                si = siList2.ElementAt(int.Parse(itemIndexString));
            }

            ////////////////////////////////
            //List<StaffItem> temp = (List<StaffItem>)WeekGridView.ItemsSource;
            //temp.ElementAt(0).

            ////////////////////////////////
            FrameworkElement root = Window.Current.Content as FrameworkElement;

            Point position = this.TransformToVisual(root).TransformPoint(e.GetPosition(this));

            int newIndex = 0;

            // check items directly under the pointer
            foreach (var element in VisualTreeHelper.FindElementsInHostCoordinates(position, root))
            {
                // assume horizontal orientation
                var container = element as ContentControl;
                if (container == null)
                {
                    continue;
                }

                int tempIndex = WeekGridView.IndexFromContainer(container);
                if (tempIndex >= 0)
                {

                    newIndex = tempIndex;
                    // adjust index depending on pointer position
                    Point center = container.TransformToVisual(root).TransformPoint(new Point(container.ActualWidth / 2, container.ActualHeight / 2));
                    if (position.Y > center.Y)
                    {
                        newIndex++;
                    }
                    break;
                }
            }
            if (newIndex < 0)
            {
                // if we haven't found item under the pointer, check items in the rectangle to the left from the pointer position
                foreach (var element in GetIntersectingItems(position, root))
                {
                    // assume horizontal orientation
                    var container = element as ContentControl;
                    if (container == null)
                    {
                        continue;
                    }

                    // int tempIndex = WeekGridView.ItemContainerGenerator.IndexFromContainer(container);
                    int tempIndex = WeekGridView.IndexFromContainer(container);
                    if (tempIndex < 0)
                    {
                        // we only need GridViewItems belonging to this GridView control
                        // so skip all elements which are not
                        continue;
                    }
                    Rect bounds = container.TransformToVisual(root).TransformBounds(new Rect(0, 0, container.ActualWidth, container.ActualHeight));

                    if (bounds.Left <= position.X && bounds.Top <= position.Y && tempIndex > newIndex)
                    {
                        //_currentOverGroup = GetItemGroup(container.Content);
                        newIndex = tempIndex;
                        // adjust index depending on pointer position
                        if (position.Y > bounds.Top + container.ActualHeight / 2)
                        {
                            newIndex++;
                        }
                        if (bounds.Right > position.X && bounds.Bottom > position.Y)
                        {
                            break;
                        }
                    }
                }
            }
            if (newIndex < 0)
            {
                newIndex = 0;
            }
            if (newIndex >= empList.Count * 9)
            {
                newIndex = empList.Count * 9 - 1;
            }
            //empList = await User.ReadUsersList();
           // await new MessageDialog(newIndex.ToString()).ShowAsync();
            StaffItem selectedStaffItem = dList.ElementAt(newIndex);
            if (selectedStaffItem.workHours == null)
            {
                if (!String.IsNullOrEmpty(si.workHours.TimeOffReason))
                { //timeOff

                    //            WorkHours wh = new WorkHours() { EmployeeId = empList.ElementAt(SelectedString.i / 9).UserId,TimeOffFrom= SelectedString.date,TimeOffTo=SelectedString.date+new TimeSpan(23,0,0), TimeOffReason="Time Off"};
                    WorkHours wh = new WorkHours();
                    //wh = selectedStaffItem.workHours;
                    wh.TimeOffReason = si.workHours.TimeOffReason;
                    wh.TimeOffFrom = selectedStaffItem.date;
                    wh.TimeOffTo = selectedStaffItem.date + new TimeSpan(23, 0, 0);
                    wh.EmployeeId = empList.ElementAt(selectedStaffItem.i / 9).UserId;

                    //Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 2);
                    //myGrid.Opacity = 0.5;

                    await WorkHours.InsertNewWorkHours(wh);
                    fillWeeklyCalendar();


                    //myGrid.Opacity = 1;
                    //Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 2);

                }
                else
                { // Shift
                    //            WorkHours wh = new WorkHours() { EmployeeId = empList.ElementAt(SelectedString.i / 9).UserId, From = SelectedString.date + ts1, To = SelectedString.date + ts2 , OnCallOnly = false, BreakTimeFrom= new TimeSpan(8+count,0,0), BreakTimeTo=new TimeSpan(8+count+1,0,0),TimeOffReason=String.Empty };

                    WorkHours wh = new WorkHours();
                    //wh = selectedStaffItem.workHours;
                    wh.From = selectedStaffItem.date +  si.workHours.From.TimeOfDay;
                    wh.To = (si.workHours.From.TimeOfDay.Hours != 23) ? selectedStaffItem.date +si.workHours.To.TimeOfDay : selectedStaffItem.date.AddDays(1) + si.workHours.To.TimeOfDay;
                    wh.EmployeeId = empList.ElementAt(selectedStaffItem.i / 9).UserId;
                    wh.OnCallOnly = false;
                    wh.BreakTimeFrom = new TimeSpan(si.workHours.From.TimeOfDay.Hours + empList.Count, 0, 0);
                    wh.BreakTimeTo = new TimeSpan(si.workHours.From.TimeOfDay.Hours + empList.Count + 1, 0, 0);
                    wh.TimeOffReason = String.Empty;

                    //Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 2);
                    //myGrid.Opacity = 0.5;

                    await WorkHours.InsertNewWorkHours(wh);
                    fillWeeklyCalendar();

                    //myGrid.Opacity = 1;
                    //Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 2);

                }
            }
            else
            {
                await new MessageDialog("Conflict").ShowAsync();
            }

        }

        private static IEnumerable<UIElement> GetIntersectingItems(Point intersectingPoint, FrameworkElement root)
        {
            Rect rect = new Rect(0, 0, intersectingPoint.X, root.ActualHeight);
            return VisualTreeHelper.FindElementsInHostCoordinates(rect, root);
        }

        private async void DragMenuGridView2_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            e.Data.SetData("ItemNumber2", siList2.IndexOf(e.Items[0] as StaffItem).ToString());
            //  await new MessageDialog(siList2.IndexOf(e.Items[0] as StaffItem).ToString()).ShowAsync();
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e) //All
        {
            AllEmployees = true;
            SingleEmployee = false;
            fillWeeklyCalendar();
        }

        private async void AppBarButton_Click_1(object sender, RoutedEventArgs e) //Single
        {
            AllEmployees = false;
            SingleEmployee = true;
            List<User> uList = await User.ReadUsersList(); ;
            EmployeeGridView.ItemsSource = uList;
            SelectedUser = uList.First();

            fillCalendar();
        }
        List<WorkHours> whList = new List<WorkHours>();
        public async void fillCalendar()
        {
            Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 2);
            myGrid.Opacity = 0.5;

            //Windows.Globalization.Calendar calendar = new Windows.Globalization.Calendar();
            whList = await WorkHours.ReadListAsync();
            List<NameValueItem> dList = new List<NameValueItem>();

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

                //OutLookEvent myOEvent = null;
                //try
                //{
                //    myOEvent = eventList.Where(e => (e.start_time.ToString("MMMM dd, yyyy")).Equals(loopDate.ToString("MMMM dd, yyyy"))).ToArray().First();
                //}
                //catch
                //{

                //}
                WorkHours wh = null;
                try
                {
                    wh = whList.Where(w => w.EmployeeId == SelectedUser.UserId && (w.From.ToString("MMMM dd, yyyy").Equals(loopDate.ToString("MMMM dd, yyyy")) || w.TimeOffFrom.ToString("MMMM dd, yyyy").Equals(loopDate.ToString("MMMM dd, yyyy")))).First();
                }
                catch
                {

                }

                if (loopDate.ToString("MMMM dd, yyyy").Equals(CurrentDate.ToString("MMMM dd, yyyy")))
                {
                    dList.Add(new NameValueItem { Name = (new DateTime(Convert.ToInt32(SelectedYear), DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month, i).ToString("dddd")), Value = i.ToString(), Info = String.Empty, Color = (wh == null ? "#BFB5DF" : (wh.From.TimeOfDay.Hours == 7 ? "#99FF99" : (wh.From.TimeOfDay.Hours == 15 ? "#99CCFF" : (wh.From.TimeOfDay.Hours == 23 ? "Wheat" : "#BFB5DF")))), date = loopDate, workHours = wh });

                }
                else
                {

                    dList.Add(new NameValueItem { Name = (new DateTime(Convert.ToInt32(SelectedYear), DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month, i).ToString("dddd")), Value = i.ToString(), Info = String.Empty, Color = (wh == null ? "#BFB5DF" : (wh.From.TimeOfDay.Hours == 7 ? "#99FF99" : (wh.From.TimeOfDay.Hours == 15 ? "#99CCFF" : (wh.From.TimeOfDay.Hours == 23 ? "Wheat" : "#BFB5DF")))), date = loopDate, workHours = wh });


                }
            }
            flip = 0;
            MonthGridView.ItemsSource = dList;


            myGrid.Opacity = 1;
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



        private bool allEmployees = true;
        public bool AllEmployees
        {
            get { return allEmployees; }
            set
            {
                allEmployees = value;
                OnPropertyChanged("AllEmployees");
            }
        }

        private bool singleEmployee = false;
        public bool SingleEmployee
        {
            get { return singleEmployee; }
            set
            {
                singleEmployee = value;
                OnPropertyChanged("SingleEmployee");
            }
        }

        private void EmployeeGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            SelectedUser = (User)e.ClickedItem;
            fillCalendar();
        }

   



    }

    public class StaffItem
    {
        //total
        public int Total { get; set; }
        //employee
        public User Employee { get; set; }
        //color
        public string Color { get; set; }
        //width
        public int Width { get; set; }

        public Thickness border { get; set; }
        //WorkHours
        public WorkHours workHours { get; set; }
        public DateTime date { get; set; }
        public int i { get; set; }
    }


}
