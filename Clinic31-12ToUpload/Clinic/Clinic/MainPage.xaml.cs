using Clinic.Model;
using Microsoft.Live;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Clinic.Common;
using System.Net;
using Windows.UI.Xaml.Media.Imaging;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Notifications;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Clinic
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Clinic.Common.LayoutAwarePage
    {




        private MainViewModel viewModel;
        private SynchronizationContext synchronizationContext;
      
      
        //private static MainPage instance;
        //StorageFolder folder = ApplicationData.Current.LocalFolder;
        //StorageFile file;
        //string name = ".profile";
       // bool success = false;

        private static bool success =false;

        public static  bool Success  // azure
        {
            get { return success; }
            set
            {
                success = value;
               
            }
        }


        private static bool success2 = false;

        public static bool Success2 //auto login
        {
            get { return success2; }
            set
            {
                success2 = value;

            }
        }

    
        public MainPage()
        {
            this.InitializeComponent();
           
           

                synchronizationContext = SynchronizationContext.Current;
                viewModel = new MainViewModel(synchronizationContext);

                DataContext = viewModel;
                if (Connection.User == null)
                {
                    viewModel.Initialize();
                }
                else
                {
                    viewModel.SignedIn = true;
                    viewModel.NotDisplayRegistrationForm = true ;
                    viewModel.DisplayRegistrationForm = false;
                    viewModel.DisplayRegistrationColor = "#D5CDE9"; //was light gray

                    MainPage.Success = true;
                    MainPage.Success2 = true;

                }

               

            
            InitAuth();
        }

      
        //public static LiveAuthClient authClient = null;
       
        //private static IMobileServiceTable<User> User =null; 
        public   async void InitAuth()
        
        {
            

            if (Connection.User == null)
            {
                if (Success) // azure already configured
                {
                    Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 2);
                    myGrid.Opacity = 0.5;
                }
              

                await Task.Run(() =>
                {
                    while (true)
                    {
                        if (Success2) // auto login
                            break;
                    }
                });

                myGrid.Opacity = 1;

                Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 2);
            }
            
            logIn.Text = Connection.UserName;
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

            pList = await Patient.ReadPatientsList();
            List<Appointment> appList =await  Appointment.ReadAppointmentsList();



            appList = appList.Where(ap => ap.UserID.Equals(Connection.User.UserId) && ap.Date.Date.Equals(DateTime.Now.Date) ).ToList();
           // string s2 = DateTime.Now.TimeOfDay.ToString("hh:mm");
           //string s = appList.ElementAt(0).TimeFrom.ToString("hh:mm");
            await Dispatcher.RunAsync(CoreDispatcherPriority.Low, async() =>
            {
                oldAppIncrease = Scheduling.appIncrease;
                while (true)
                {
                    await Task.Delay(60000);
                    if(oldAppIncrease!=Scheduling.appIncrease)
                    {
                        appList = await Appointment.ReadAppointmentsList();
                        appList = appList.Where(ap => ap.UserID.Equals(Connection.User.UserId) && ap.Date.Date.Equals(DateTime.Now.Date)  ).ToList();
                        oldAppIncrease = Scheduling.appIncrease;
                    }
                    foreach(Appointment app in appList)
                    {
                       // if (app.Date.Date.Equals(DateTime.Now.Date))
                       if (app.TimeFrom.Hours == DateTime.Now.TimeOfDay.Hours && app.TimeFrom.Minutes == DateTime.Now.TimeOfDay.Minutes)
                        {
                            string toastXmlString = String.Empty;
                            Patient temp = getPatient( app.PatientID);
                            toastXmlString = "<toast>"
                                + "<visual version='1'>"
                                + "<binding template='ToastText04'>"
                                + "<text id='1'> Appointment Reminder</text>"
                                + "<text id='2'>"+temp.LName+ ", " + temp.FName+"</text>"
                                + "<text id='3'>"+app.Complaint+"</text>"
                                //+ "<text id='4'>" + app.Date.Date+"-"+ app.TimeFrom+"-"+app.TimeTo + "</text>"
                                + "</binding>"
                                + "</visual>"
                                + "</toast>";
                            Windows.Data.Xml.Dom.XmlDocument toastDOM = new Windows.Data.Xml.Dom.XmlDocument();
                            toastDOM.LoadXml(toastXmlString);

                         

                            // Create a toast, then create a ToastNotifier object to show
                            // the toast
                            ToastNotification toast = new ToastNotification(toastDOM);

                            // If you have other applications in your package, you can specify the AppId of
                            // the app to create a ToastNotifier for that application
                            ToastNotificationManager.CreateToastNotifier().Show(toast);
                        }

                    }

                }
            });
        }

        static  List<Patient> pList = null;
        public static Patient getPatient(string id)
        {
            Patient pp = pList.Where(p => p.PatientID.Equals(id)).ToList().First();
            return pp;
        }


        public static int oldAppIncrease = 0;
          
               
            
        
       

        private void Grid1_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(NewPatient));
        }

        private void Grid4_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
           

        }

        private void Grid5_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            
        }

        private void Grid_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MyProfile));
        }

        private void Grid2_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Settings));
        }

        private void Grid_PointerPressed_1(object sender, PointerRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Staff));
        }

        private void Grid3_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Scheduling));
        }

        private void Grid_PointerPressed_2(object sender, PointerRoutedEventArgs e)// about page
        {
            this.Frame.Navigate(typeof(About));
        }

    }
}
