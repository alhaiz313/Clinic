using Microsoft.Live;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinic
{
    class Connection
    {

      //  public static MobileServiceClient mobileService=null;

        //public static MobileServiceClient mobileService = new MobileServiceClient(
        //  "https://zainabalhaidary5.azure-mobile.net/",
        //  "CiywNDvdJVFfrdkOZUXcpAItqDEyAk32");

        public static MobileServiceClient mobileService; //= new MobileServiceClient(applicationURL,applicationKey);

        public Connection(string a, string b)
        {
            mobileService = new MobileServiceClient(a, b);
            key = b;
            Url = a;
        }

        public static MobileServiceClient MobileService
        {
            get { return mobileService; }
        }

        private static LiveConnectSession _session;


        public static LiveConnectSession Session
        {
            get
            {
                return _session;
            }
            set
            {
                _session = value;

            }
        }

          private static string _userName;
          public static string UserName
          {
              get
              {
                  return _userName;
              }
              set
              {
                  _userName = value;

              }
          }


          private static string _imageUri;
          public static string ImageUri
          {
              get
              {
                  return _imageUri;
              }
              set
              {
                  _imageUri = value;

              }
          }

          private static Model.User user;
          public static Model.User User
          {
              get ;
              set ;
          }

          private static string key;
          public static string Key
          {
              get;
              set;
          }

          private static string url;
          public static string Url
          {
              get;
              set;
          }
        
          //public event PropertyChangedEventHandler PropertyChanged;

          //public void OnPropertyChanged(string PropertyName)
          //{
          //    if (PropertyChanged != null)
          //        PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
          //}

    }
}
