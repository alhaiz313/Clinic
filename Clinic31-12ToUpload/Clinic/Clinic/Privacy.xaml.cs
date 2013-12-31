using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
//using Windows.UI.ApplicationSettings;

// The Settings Flyout item template is documented at http://go.microsoft.com/fwlink/?LinkId=273769

namespace Clinic
{
    public  partial class Privacy : SettingsFlyout
    {
        public Privacy()
        {
            this.InitializeComponent();
           
            // this.Loaded += (sender, e) =>
            //{
            //    Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated += SettingsFlyout1_AcceleratorKeyActivated;
            //};
            //this.Unloaded += (sender, e) =>
            //{
            //    Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated -= SettingsFlyout1_AcceleratorKeyActivated;
            //};
        }

        //protected override void onWindowCreated(WindowCreatedEventArgs args)
        //{
        //    SettingsPane.GetForCurrentView().CommandsRequested += onCommandsRequested;
        //}

       


        //void SettingsFlyout1_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs args)
        //{
        //     SettingsCommand privacyCommand = new SettingsCommand("privacy", "Privacy", (handler) =>
        //    {
        //        Privacy privacyFlyout = new Privacy();
        //        privacyFlyout.Show();
        //    });

        //     // Only investigate further when Left is pressed
        //     if (args.EventType == CoreAcceleratorKeyEventType.SystemKeyDown &&
        //         args.VirtualKey == VirtualKey.Left)
        //     {
        //         var coreWindow = Window.Current.CoreWindow;
        //         var downState = CoreVirtualKeyStates.Down;

        //         // Check for modifier keys
        //         // The Menu VirtualKey signifies Alt
        //         bool menuKey = (coreWindow.GetKeyState(VirtualKey.Menu) & downState) == downState;
        //         bool controlKey = (coreWindow.GetKeyState(VirtualKey.Control) & downState) == downState;
        //         bool shiftKey = (coreWindow.GetKeyState(VirtualKey.Shift) & downState) == downState;

        //         if (menuKey && !controlKey && !shiftKey)
        //         {
        //             args.Handled = true;
        //             this.Hide();
        //         }
        //     }

        //}
    }
}
