using System;
using System.Linq;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.Graphics.Printing;
using Windows.UI.Xaml.Printing;
using Windows.UI.Xaml.Documents;
using Clinic.Model;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Clinic
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Report : BasePrintPage
    {
        public static bool pressed = false;
        public Report()
        {
            this.InitializeComponent();
           // RegisterForPrinting();
        }

         private async void InvokePrintButtonClick(object sender, RoutedEventArgs e)
        {
            if (invokePrintingButton.Content.Equals("Print..."))
            {
                await Windows.Graphics.Printing.PrintManager.ShowPrintUIAsync();
                invokePrintingButton.Content = "Hide";
            }
            else if(invokePrintingButton.Content.Equals("Hide"))
            {
            pressed = true;
            }
             
        }
         Patient patient = null;
         protected override void OnNavigatedTo(NavigationEventArgs e)
         {
             if (e.Parameter != null)
             {
                 patient = ((Dictionary<Page, Patient>)e.Parameter).Values.First();

             }
             base.OnNavigatedTo(e);

             
             // init printing 
           //  RegisterForPrinting();
             
         }

        /// <summary>
        /// Provide print content for scenario 2 first page
        /// </summary>
        protected override void PreparePrintContent()
        {


            if (firstPage == null)
            {
                firstPage = new Report2(patient);
                StackPanel header = (StackPanel)firstPage.FindName("header");
                header.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }

            // Add the (newley created) page to the printing root which is part of the visual tree and force it to go
            // through layout so that the linked containers correctly distribute the content inside them.
            PrintingRoot.Children.Add(firstPage);
            PrintingRoot.InvalidateMeasure();
            PrintingRoot.UpdateLayout();
        }
 
             
            
    }
}
