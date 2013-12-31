using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class About : Common.LayoutAwarePage
    {
        public About()
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
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var sampleData = new SampleDataSource();
            FlipView1.ItemsSource = sampleData.Items;
        }

      
    }
    public abstract class SampleDataCommon : Common.BindableBase
    {
        private static Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String title, String type, String picture)
        {
            this._title = title;
            this._type = type;
            this._picture = picture;
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return this._title; }
            set { this.SetProperty(ref this._title, value); }
        }

        private string _type = string.Empty;
        public string Type
        {
            get { return this._type; }
            set { this.SetProperty(ref this._type, value); }
        }

        private Uri _image = null;
        private String _picture = null;
        public Uri Image
        {
            get
            {
                return new Uri(SampleDataCommon._baseUri, this._picture);
            }

            set
            {
                this._picture = null;
                this.SetProperty(ref this._image, value);
            }
        }
    }

    /// <summary>
    /// Generic item data model.
    /// </summary>
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String title, String type, String picture)
            : base(title, type, picture)
        {
        }

    }

    /// <summary>
    /// Creates a collection of groups and items with hard-coded content.
    /// </summary>
    public sealed class SampleDataSource
    {
        private ObservableCollection<object> _items = new ObservableCollection<object>();
        public ObservableCollection<object> Items
        {
            get { return this._items; }
        }

        public SampleDataSource()
        {
            Items.Add(new SampleDataItem("Patients",
                    "item",
                    "Screenshots/Patients.png"
                    ));
            Items.Add(new SampleDataItem("Patient History Calendar",
                    "item",
                    "Screenshots/Patients-Calendar.png"
                    ));
            Items.Add(new SampleDataItem("Profile Monthly Calendar",
                    "item",
                    "Screenshots/Profile-Monthly.png"
                    ));
            Items.Add(new SampleDataItem("Profile Weekly Calendar",
                    "item",
                    "Screenshots/Profile-Weekly.png"
                    ));
            Items.Add(new SampleDataItem("Staff Work Hours",
                    "item",
                    "Screenshots/staff-All.png"
                    ));
            Items.Add(new SampleDataItem("Single Employee Work Hours",
                  "item",
                  "Screenshots/Staff-Monthly.png"
                  ));
            Items.Add(new SampleDataItem("Appointments",
                 "item",
                 "Screenshots/Appointment.png"
                 ));
            Items.Add(new SampleDataItem("Appointments Booking",
                 "item",
                 "Screenshots/Appointment-Book.png"
                 ));
        }
    }
}
