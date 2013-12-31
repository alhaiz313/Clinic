using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class Settings : Common.LayoutAwarePage
    {
        public Settings()
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
            //try
            //{
            //    this.AzureUrl.Text = Connection.Url;
            //    this.AzureKey.Password = Connection.Key;
            //}
            //catch (Exception e)
            //{
            //    new MessageDialog(e.Message).ShowAsync();
            //}
        }
    }
}
