using Clinic.Common;
using Clinic.Model;
using Microsoft.Live;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace Clinic
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        
       
        // public static MobileServiceClient mobileService = new MobileServiceClient(
        //   "https://zainabalhaidary5.azure-mobile.net/",
        //   "CiywNDvdJVFfrdkOZUXcpAItqDEyAk32");

        ////public static MobileServiceClient mobileService;

        //public static MobileServiceClient MobileService
        //{
        //    get { return mobileService; }
        //}

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        /// 

        public App()
        {


            this.InitializeComponent();
            this.Suspending += OnSuspending;
            //try
            //{
            //    SettingsPane.GetForCurrentView().CommandsRequested += onCommandsRequested;
            //}
            //catch (Exception e)
            //{
            //    Debug.WriteLine(e.Message);
            //}
           
        }

        //protected override void onWindowCreated(WindowCreatedEventArgs args)
        //{
        //    SettingsPane.GetForCurrentView().CommandsRequested += onCommandsRequested;
        //}

        void onCommandsRequested(SettingsPane settingsPane, SettingsPaneCommandsRequestedEventArgs e)//SettingPaneCommandsRequestedEventArgs 
        {
            SettingsCommand privacyCommand = new SettingsCommand("privacy", "Privacy", (handler) =>
            {
                Privacy privacyFlyout = new Privacy();
                privacyFlyout.Show();
            });
            e.Request.ApplicationCommands.Add(privacyCommand);


             SettingsCommand accountCommand = new SettingsCommand("account", "Account", (handler) =>
            {
                Account accountFlyout = new Account();
                accountFlyout.Show();
            });
            e.Request.ApplicationCommands.Add(accountCommand);
        }

        

        //private static LiveConnectSession _session;


        //public static LiveConnectSession Session
        //{
        //    get
        //    {
        //        return _session;
        //    }
        //    set
        //    {
        //        _session = value;

        //    }
        //}

        //private static string _userName;
        //public static string UserName
        //{
        //    get
        //    {
        //        return _userName;
        //    }
        //    set
        //    {
        //        _userName = value;

        //    }
        //}

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {


            // Do not repeat app initialization when already running, just ensure that
            // the window is active
            if (args.PreviousExecutionState == ApplicationExecutionState.Running)
            {
                Window.Current.Activate();
                return;
            }

            // Create a Frame to act as the navigation context and associate it with
            // a SuspensionManager key
            var rootFrame = new Frame();
            SuspensionManager.RegisterFrame(rootFrame, "AppFrame");

            //if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            //{
            //    // Restore the saved session state only when appropriate
            //    await SuspensionManager.RestoreAsync();
            //}

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), "AllGroups"))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Place the frame in the current Window and ensure that it is active

           // await Task.Delay(2000);
            Window.Current.Content = rootFrame;
            Window.Current.Activate();
            //################################################################################## extended
            //if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            //{
            //    bool loadState = (args.PreviousExecutionState == ApplicationExecutionState.Terminated);
            //    Splash extendedSplash = new Splash(args.SplashScreen, loadState);
            //    Window.Current.Content = extendedSplash;
            //}

            //Window.Current.Activate();

            try
            {
                SettingsPane.GetForCurrentView().CommandsRequested += onCommandsRequested;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();

            StorageFolder folder = ApplicationData.Current.LocalFolder;
            var files = await folder.GetFilesAsync();
            foreach (var file in files)
            {
                if(!file.Name.Contains(".profile"))
                    await file.DeleteAsync();
            }
        }


       
    }
}
