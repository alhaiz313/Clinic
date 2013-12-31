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
using Windows.UI.Core;
using Clinic.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Clinic
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public  sealed partial class MainPage4 : Clinic.Common.LayoutAwarePage, INotifyPropertyChanged 
    {
        public static int flip = 0;
        public static int flip2 = 0;
        public static  int selectedMonthNumber;

        enum ViewMode
        {
            Weekly,
            Monthly,
        }

        public static bool backward = false;

       private  string currentViewMode;
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

        public DateTime CurrentDate;

        public MainPage4()
        {
            this.InitializeComponent();
            this.logIn.Text = Connection.UserName;
            //MonthNameTextBlock.DataContext = this;
            //YearNameTextBlock.DataContext = this;
            MonthNameTextBlock2.DataContext = this;
            YearNameTextBlock2.DataContext = this;
            MonthNameTextBlock3.DataContext = this;
            YearNameTextBlock3.DataContext = this;
            CurrentDate = DateTime.Now;
           // Output.DataContext = this;
            Output2.DataContext = this;
           // fillCalendar();

            //PropertyChanged += (sender, e) =>
            //{
            //    if (String.Equals(e.PropertyName, "ShowWeeklyCalendar"))
            //    {
            //        //new MessageDialog("property changed").ShowAsync();
            //        fillWeeklyCalendar();
            //    }
            //};
            fillWeeklyCalendar();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string PropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        }


        //public void fillCalendar()
        //{
        //    Windows.Globalization.Calendar calendar = new Windows.Globalization.Calendar();
            
        //    List<NameValueItem> dList = new List<NameValueItem>();

        //    int noOfDaysInTheSelectedMonth = 0;
        //    int shift = 0;
        //    string firstDay=String.Empty;

        //    if(flip == 0)
        //    {
                
        //        SelectedMonth = calendar.MonthAsSoloString();
                
        //        SelectedYear = calendar.Year.ToString();
        //       // firstDay = new DateTime(Convert.ToInt32(SelectedYear), DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month, 1).ToString("dddd");
        //    }
            
        //    if(flip == -1)
        //    {
        //         int temp = DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month;
        //         if (temp == 1)
        //         {
        //             temp = 12;
        //         }
        //         else
        //         {
        //             temp--;
        //         }
        //        SelectedMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(temp);
        //        if(temp!=12 )
        //        {
        //            //SelectedYear = calendar.Year.ToString();
        //        }
        //        else
        //        {
        //            int tempYear = Convert.ToInt32(SelectedYear);
        //            tempYear--;
        //            SelectedYear = tempYear.ToString();

        //        }

                
        //    }

        //    if(flip ==1)
        //    {
        //        {
        //            int temp = DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month;
        //            if (temp == 12)
        //            { 
        //                temp = 1; 
        //            }
        //            else
        //            {
        //                temp++;
        //            }
        //            SelectedMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(temp);
        //            if (temp != 1)
        //            {
        //            }
        //            else
        //            {
        //                int tempYear =Convert.ToInt32(SelectedYear);
        //                tempYear++;
        //                SelectedYear = tempYear.ToString();

        //            }


        //        }
        //    }


        //    noOfDaysInTheSelectedMonth = DateTime.DaysInMonth(Convert.ToInt32(SelectedYear), DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month);
        //    firstDay = new DateTime(Convert.ToInt32(SelectedYear), DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month, 1).ToString("dddd");

        //    switch (firstDay)
        //    {
        //        case "Sunday": shift = 0;
        //            break;
        //        case "Monday": shift = 1;
        //            break;
        //        case "Tuesday": shift = 2;
        //            break;
        //        case "Wednesday": shift = 3;
        //            break;
        //        case "Thursday": shift = 4;
        //            break;
        //        case "Friday": shift = 5;
        //            break;
        //        case "Saturday": shift = 6;
        //            break;
        //    }

        //    while (shift > 0)
        //    {
        //        dList.Add(new NameValueItem());
        //        shift--;
        //    }

        //    for (int i = 1; i <= noOfDaysInTheSelectedMonth; i++)
        //    {
        //        DateTime loopDate = new DateTime(Convert.ToInt32(SelectedYear), DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month, i);
        //        if (loopDate.ToString("MMMM dd, yyyy").Equals(CurrentDate.ToString("MMMM dd, yyyy")))
        //        {
        //            dList.Add(new NameValueItem { Name = (new DateTime(Convert.ToInt32(SelectedYear), DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month, i).ToString("dddd")), Value = i.ToString(), Info = String.Empty, Color = "Blue", date = DateTime.Now });

        //        }
        //        else
        //        {
        //            if (i == 6)
        //            {
        //                dList.Add(new NameValueItem { Name = (new DateTime(Convert.ToInt32(SelectedYear), DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month, i).ToString("dddd")), Value = i.ToString(), Info = "Abdominal Pain", Color = "Pink", date = DateTime.Now });

        //            }
        //            else if(i==15)
        //            {
        //                dList.Add(new NameValueItem { Name = (new DateTime(Convert.ToInt32(SelectedYear), DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month, i).ToString("dddd")), Value = i.ToString(), Info = "Leg Fracture", Color = "Pink", date = DateTime.Now });
        //            }
        //            else if (i == 19)
        //            {
        //                dList.Add(new NameValueItem { Name = (new DateTime(Convert.ToInt32(SelectedYear), DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month, i).ToString("dddd")), Value = i.ToString(), Info = "Feaver", Color = "Pink", date = DateTime.Now });
        //            }
        //            else
        //            {
        //                dList.Add(new NameValueItem { Name = (new DateTime(Convert.ToInt32(SelectedYear), DateTime.ParseExact(SelectedMonth, "MMMM", CultureInfo.CurrentCulture).Month, i).ToString("dddd")), Value = i.ToString(), Info = String.Empty, Color = "AliceBlue", date = DateTime.Now });

        //            }
        //        }
        //    }

        //    MonthGridView.ItemsSource = dList;
        //}

        //private void previousMonthButton_Click(object sender, RoutedEventArgs e)
        //{
        //    flip = -1;
        //    fillCalendar();

        //}

        //private void nextMonthButton_Click(object sender, RoutedEventArgs e)
        //{
        //    flip = 1;
        //    fillCalendar();
        //}



        //private async void MonthGridView_ItemClick(object sender, ItemClickEventArgs e)
        //{

        //    if (((NameValueItem)e.ClickedItem).Value!= null)
        //    {

        //        Window currentWindow = Window.Current;

        //        Point point, myPoint;

        //        try
        //        {
        //            point = currentWindow.CoreWindow.PointerPosition;
        //        }
        //        catch (UnauthorizedAccessException)
        //        {
        //            myPoint = new Point(double.NegativeInfinity, double.NegativeInfinity);
        //        }

        //        Rect bounds = currentWindow.Bounds;

        //        myPoint = new Point(DipToPixel(point.X - bounds.X), DipToPixel(point.Y - bounds.Y));

        //        LightDismissAnimatedPopup.VerticalOffset = myPoint.Y - 200;

        //        LightDismissAnimatedPopup.HorizontalOffset = myPoint.X - 310;


        //        if (!LightDismissAnimatedPopup.IsOpen) { LightDismissAnimatedPopup.IsOpen = true; }
        //        LightDismissAnimatedPopupTextBlock.Text = ((NameValueItem)e.ClickedItem).Value;
        //        LightDismissAnimatedPopupTextBlock2.Text = ((NameValueItem)e.ClickedItem).date.ToString();
        //    }
        //}
        //private static double DipToPixel(double dip)
        //{
        //    return (dip * DisplayProperties.LogicalDpi) / 96.0;
        //}

     
        //// Handles the Click event on the Button within the simple Popup control and simply closes it.
        //private void CloseAnimatedPopupClicked(object sender, RoutedEventArgs e)
        //{
        //    if (LightDismissAnimatedPopup.IsOpen) { LightDismissAnimatedPopup.IsOpen = false; }
        //}

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    CurrentViewMode = ViewMode.Weekly.ToString();
        //    ShowMonthlyCalendar = Visibility.Collapsed;
        //    ShowWeeklyCalendar = Visibility.Visible;
        //   // Output.Visibility = ShowMonthlyCalendar;
        //}


        public async void fillWeeklyCalendar()
        {

            LeftMonth = String.Empty;
            LeftYear = String.Empty;
            RightMonth = String.Empty;
            RightYear = String.Empty;

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

            List<User> empList = await  User.ReadUsersList();


            for (int i = 0; i < empList.Count*9; i++)
            {
                OutLookEvent myOEvent = null;
                if (i % 9 == 0)
                {
                    dList.Add(new NameValueItem3() { timeItem = true, time = new TimeSpan(i / 9, 0, 0), Width = 3, timeStr = "Employee Name", Color1 = "AliceBlue" }); //( new TimeSpan(i / 9, 0, 0)).ToString()
               
                //fill employee names
                }

                else if ((i % 9)-1 == 7)
                {
                    //calculate work hours
                    dList.Add(new NameValueItem3() { timeItem = true, time = new TimeSpan(i / 9, 0, 0), Width = 1, timeStr = "Total", Color1 = "AliceBlue" }); //( new TimeSpan(i / 9, 0, 0)).ToString()

                }

                else 
                {

                    DateTime loopDate = new DateTime(Convert.ToInt32(weekList.ElementAt((i % 9) - 1).Year),
                           DateTime.ParseExact(weekList.ElementAt((i % 9) - 1).Month, "MMMM", CultureInfo.CurrentCulture).Month,
                           weekList.ElementAt((i % 9) - 1).Day);

                  
                    dList.Add(new NameValueItem3() { timeItem = false, time = new TimeSpan(i / 9, 0, 0), Width = 3, weekDay = weekList.ElementAt((i % 9) - 1), Color1 = (myOEvent != null ? "Wheat" : "AliceBlue"), myOutLookEvent = myOEvent });
                  
                    
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
            weekList.Clear();
            Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
            {
                WeekGridView.ScrollIntoView(dList.ElementAt(147));
            });

        }

        private Visibility showMonthlyCalendar = Visibility.Visible;
        public Visibility ShowMonthlyCalendar
        {
            get { return showMonthlyCalendar; }
            set { showMonthlyCalendar = value;
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CurrentViewMode = ViewMode.Monthly.ToString();
            ShowMonthlyCalendar = Visibility.Visible;
            ShowWeeklyCalendar = Visibility.Collapsed;
          //  Output.Visibility = ShowMonthlyCalendar;

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

    }

    //public class NameValueItem
    //{
    //    public string Name { get; set; }
    //    public string Value { get; set; }

    //    public string Info { get; set; }

    //    public DateTime date { get; set; }

    //    public OutLookEvent myOutLookEvent { get; set; }

    //    public string Color { get; set; }
    //}

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

    //    public OutLookEvent myOutLookEvent { get; set; }

    //    public int Width { get;  set; }
    //}

    //public class WeekDay
    //{
    //    public string Year {get; set;}
    //    public string Month { get; set; }

    //    public int Day { get; set; }
    //}
}
