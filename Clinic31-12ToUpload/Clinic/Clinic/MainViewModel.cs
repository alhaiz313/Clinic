using Clinic.Common;
using Clinic.Model;
using Microsoft.Live;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Networking.PushNotifications;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Clinic
{
    class MainViewModel : ViewModel
    {
        private SynchronizationContext synchronizationContext;
        StorageFolder folder = ApplicationData.Current.LocalFolder;
        StorageFile file;
        string name = ".profile";
       
      

        public DelegateCommand RegisterCommand { get; private set; }

      

        private void SetupCommands()
        {

             RegisterCommand = new DelegateCommand(async () =>
            {
                MessageDialog mm;

                if (string.IsNullOrWhiteSpace(Ci.ApplicationURL) ||
                   string.IsNullOrWhiteSpace(Ci.ApplicationKey) ||
                   string.IsNullOrWhiteSpace(Ci.Name))
                {
                    //show message
                    mm = new MessageDialog("Please fill in all the fields");
                    await mm.ShowAsync();
                    return;
                }

                try
                {
                    file = await folder.CreateFileAsync(name);
                   

                    Connection c = new Connection(Ci.ApplicationURL, Ci.ApplicationKey);
                   // string text = Ci.ApplicationURL + " " + Ci.ApplicationKey;
                    string t = Encrypt(Ci.ApplicationKey, "myKey");
                    string tt = Encrypt(Ci.ApplicationURL, "myKey");
                    string text = tt + " " + t;
                    await Windows.Storage.FileIO.WriteTextAsync(file, text);
                    DisplayRegistrationForm = false;
                    DisplayRegistrationColor = "#D5CDE9"; //was lightgray
                    SignedIn = false;
                    NotDisplayRegistrationForm = !DisplayRegistrationForm;
                    MainPage.Success = true;
                    Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 2);

                }catch(Exception ee)
                {
                    Debug.WriteLine(ee.Message);
                }
               
            }, true);


            
        }

        byte[] key = null;
      //  public static LiveAuthClient authClient = null;

        private static IMobileServiceTable<User> User = null; 
       
        
        private async Task LoginAsync()
        {
           


                try
                {
                    file = await folder.GetFileAsync(name);
                    MainPage.Success = true;

                   IList<string> read = await Windows.Storage.FileIO.ReadLinesAsync(file); 
                  
                    string[] arr = new string[2];
                    foreach(string s in read)
                    {                
                        arr = s.Split(' ');
                        string tt = Decrypt(arr[0], "myKey");
                       string t = Decrypt(arr[1] ,"myKey");

                        Connection c = new Connection(tt,t);

                        Debug.WriteLine("printing here "+s);
                        break;
                    }

                }
                catch(Exception e)
                {
                    Debug.WriteLine("In catch statement"+ e.Message);
                    RegisterClinic();
                }

                await Task.Run(() =>
                {
                    while (true)
                    {
                        if (MainPage.Success) // to insert azure
                            break;
                    }
                });

                if (!Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                {
                    //authClient = new LiveAuthClient();

                   // MobileServiceUser msu = await Connection.MobileService.LoginAsync(MobileServiceAuthenticationProvider.MicrosoftAccount);
                    //https://zainabalhaidary5.azure-mobile.net/


                    var authClient = new LiveAuthClient("https://zainabalhaidary5.azure-mobile.net/");
                    LiveLoginResult authResult = await authClient.LoginAsync(new List<string>() { "wl.basic", "wl.emails", "wl.offline_access", "wl.contacts_create", "wl.calendars"});//"wl.work_profile", "wl.postal_addresses", "wl.phone_numbers", "wl.events_create","wl.contacts_calendars"
                    if (authResult.Status == LiveConnectSessionStatus.Connected)
                    {
                        SignedIn = true;
                        Connection.Session = authResult.Session;
                       
                        var token = new JObject(new JProperty("authenticationToken", authResult.Session.AuthenticationToken));
                      
                        MobileServiceUser loginResult = await Connection.MobileService.LoginAsync(MobileServiceAuthenticationProvider.MicrosoftAccount,token);
                        //Connection.MobileService.CurrentUser.UserId;
                        //new MessageDialog(Connection.MobileService.CurrentUser.UserId).ShowAsync();
                    }
                    else
                    {
                        MessageDialog md = new MessageDialog("The program is exiting as you have not accepted the terms");
                        await md.ShowAsync();
                        App.Current.Exit();
                    }

                    LoadProfile();

                }

            
            return;
        }

        private async void LoadProfile()
        {
           
                LiveConnectClient client = new LiveConnectClient(Connection.Session);

                LiveOperationResult liveOpResult = await client.GetAsync("me");

                //IDictionary<string, object> myResult = liveOpResult.Result;
                //for (int i = 0; i < myResult.Count; i++)
                //{
                //    Debug.WriteLine(myResult.ElementAt(i).Key + " " + myResult.ElementAt(i).Value);
                //}
                dynamic dynResult = liveOpResult.Result;

                // logIn.Text = Connection.UserName;
                string id = Connection.MobileService.CurrentUser.UserId;
                string LiveSDKId = dynResult.id;
                string first_name = dynResult.first_name;
                string last_name = dynResult.last_name;
                string link = dynResult.link;
                string gender = dynResult.gender;
                string email = dynResult.emails.preferred;

                liveOpResult = await client.GetAsync("me/picture");
                dynamic dynResult2 = liveOpResult.Result;
                //dynResult2.location to get the profile photo

                List<User> usersList = new List<User>();
                User = Connection.MobileService.GetTable<User>();


                try
                {
                    usersList = await User.Where(p => p.UserId == id).ToListAsync();
                }
                catch (MobileServiceInvalidOperationException ex)
                {
                    Debug.WriteLine(ex.Message);
                }

                if (usersList.Count == 0)
                {
                    try
                    {
                        await User.InsertAsync(new User
                        {
                            //isAdmin  = false,
                            UserId = id,
                            LiveSDKID = LiveSDKId,
                            //UserId = App.MobileService.CurrentUser.UserId,
                            CreatedDate = DateTime.Now,
                            ImageUri = dynResult2.location,
                            FName = first_name,
                            LName = last_name,
                            Email = email
                        });

                        usersList = await User.Where(p => p.UserId == id).ToListAsync();
                        Connection.User = usersList.First();
                    }
                    catch (MobileServiceInvalidOperationException e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                }
                else
                {
                    Connection.User = usersList.First();
                }

                Connection.UserName = dynResult.name;
                Connection.ImageUri = dynResult2.location;



               //await  Windows.UI.Xaml.DependencyObject.Dispatcher.RunAsync(CoreDispatcherPriority.Low, () =>
               // {
                    RegisterDeviceAsync();

                    AddContacts();
               // });

                   
                
               
                

        }

        public async  void AddContacts()
        {
            //create new LiveSDK contact

            LiveConnectClient client = new LiveConnectClient(Connection.Session);

            LiveOperationResult liveOpResult = await client.GetAsync("me/contacts");

            IDictionary<string, object> myResult = liveOpResult.Result;
            List<object> data = null;
            IDictionary<string, object> contact;
            string contact_fname = String.Empty;
            string contact_lname = String.Empty;
            Dictionary<string, string> contactList = new Dictionary<string, string>();

            if (myResult.ContainsKey("data"))
            {
                 data = (List<object>)myResult["data"];
                 for (int i = 0; i < data.Count; i++)
                 {
                     contact = (IDictionary<string, object>)data[i];
                     if (contact.ContainsKey("first_name"))
                     {
                         //contactList.ElementAt(i).Key =
                        
                         contact_fname = (string) contact["first_name"];
                     }
                     if(contact.ContainsKey("last_name"))
                     {
                         contact_lname = (string)contact["last_name"];
                     }

                     if(contact_fname!=String.Empty && contact_lname!=String.Empty)
                     {
                         contactList.Add(contact_fname, contact_lname);

                         contact_fname = String.Empty;
                         contact_lname = String.Empty;
                     }

                 }
            }

            List<User> usersList = new List<User>();
            User = Connection.MobileService.GetTable<User>();

            usersList = await User.Where(p => p.UserId != Connection.MobileService.CurrentUser.UserId ).ToListAsync();
            for (int i = 0; i < usersList.Count; i++)
            {
                if (!(contactList.ContainsKey(usersList.ElementAt(i).FName) && contactList.ContainsValue(usersList.ElementAt(i).LName)))
                {
                    contact = new Dictionary<string, object>();
                    contact.Add("first_name", usersList.ElementAt(i).FName);
                    contact.Add("last_name", usersList.ElementAt(i).LName);
                    // contact.Add("emails.preferred", usersList.ElementAt(i).Email);
                    contact.Add("emails", new Dictionary<string, object> {
                {"account",usersList.ElementAt(i).Email },
                {"preferred",usersList.ElementAt(i).Email },
                { "personal", usersList.ElementAt(i).Email},
                {"business", usersList.ElementAt(i).Email},
                {"other", usersList.ElementAt(i).Email} 
                });

                    await client.PostAsync("me/contacts", contact);

                }
            }
            // get List of users loop
            //  var contact = new Dictionary<string, object>();
            // contact.Add("first_name", "Michael");
            // contact.Add("last_name", "Crump");
            //contact.Add( "emails.preferred", "z@z.com");
            //client.PostAsync("me/contacts", contact);
            MainPage.Success2 = true;

        }

        private IMobileServiceTable<Device> deviceTable;
        private async void RegisterDeviceAsync()
        {
            deviceTable = Connection.MobileService.GetTable<Device>();
            PushNotificationChannel channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            channel.PushNotificationReceived += channel_PushNotificationReceived;

            string installationId = InstallationId.GetInstallationIdAsync();

            // The server side script device.insert.js will ensure that it creates a new row only when a new User + Device combination is getting added. 
            await deviceTable.InsertAsync(
                    new Device
                    {
                        UserId = Connection.User.UserId,
                        ChannelUri = channel.Uri.ToString(),
                        InstallationId = installationId

                    });
        }



          void channel_PushNotificationReceived(PushNotificationChannel sender, PushNotificationReceivedEventArgs args)
        {
            synchronizationContext.Post(x =>
            {
                if (args.NotificationType == PushNotificationType.Toast)
                {
                    // We received a toast notification - let's check for invites
                    //LoadInvitesAsync();
                    //MessageDialog mm = new MessageDialog("Notification Received");
                    //mm.ShowAsync();
                   // Debug.WriteLine("push notification received");
                       // NewPatient.RefreshFlag = true;
                    
                    AddContacts();
                    
                }
                else if (args.NotificationType == PushNotificationType.Tile)
                {
                    // We received a tile - reload current items in case it was for this list
                    //LoadItemsAsync();
                }
            }, null);
        }


        private void RegisterClinic()
        {

            Ci = new ClinicInfo
            {
                ApplicationKey = String.Empty,
                ApplicationURL = String.Empty,
                Name = String.Empty,
                Phone = String.Empty,
                Email = String.Empty
            };
           
            DisplayRegistrationForm = true;
            SignedIn = true;
            DisplayRegistrationColor = "Gray";
            NotDisplayRegistrationForm = !DisplayRegistrationForm;

            
        }

        public MainViewModel(SynchronizationContext synchonizationContextArgument)
        {
            synchronizationContext = synchonizationContextArgument;

           // flyoutProvider = flyoutProviderArgument;
           SetupCommands();
            Debug.WriteLine(folder.Path);
           // new MessageDialog(folder.Path).ShowAsync();
           ;
            
        }

        public async void Initialize()
        {
            await LoginAsync();
        }

        private bool displayRegistrationForm = false;

        public bool DisplayRegistrationForm
        {
            get { return displayRegistrationForm; }
            set { SetValue(ref displayRegistrationForm, value); }
        }

        private string displayRegistrationColor = "#D5CDE9"; //was lightgray

        public string DisplayRegistrationColor
        {
            get { return displayRegistrationColor; }
            set { SetValue(ref displayRegistrationColor, value); }
        }


        private bool notdisplayRegistrationForm = true;

        public bool NotDisplayRegistrationForm
        {
            get { return notdisplayRegistrationForm; }
            set { SetValue(ref notdisplayRegistrationForm, value); }
        }

        private ClinicInfo ci;

        public ClinicInfo Ci
        {
            get { return ci; }
            set { SetValue(ref ci, value); }
        }


        private  bool singedIn = false;
        public  bool SignedIn
        {
            get { return singedIn; }
            set { SetValue(ref singedIn, value); }
        }

        //public static byte[] Encrypt(string plainText, string pw, string salt)
        //{
        //    IBuffer pwBuffer = CryptographicBuffer.ConvertStringToBinary(pw, BinaryStringEncoding.Utf8);
        //    IBuffer saltBuffer = CryptographicBuffer.ConvertStringToBinary(salt, BinaryStringEncoding.Utf16LE);
        //    IBuffer plainBuffer = CryptographicBuffer.ConvertStringToBinary(plainText, BinaryStringEncoding.Utf16LE);

        //    // Derive key material for password size 32 bytes for AES256 algorithm
        //    KeyDerivationAlgorithmProvider keyDerivationProvider = Windows.Security.Cryptography.Core.KeyDerivationAlgorithmProvider.OpenAlgorithm("PBKDF2_SHA1");
        //    // using salt and 1000 iterations
        //    KeyDerivationParameters pbkdf2Parms = KeyDerivationParameters.BuildForPbkdf2(saltBuffer, 1000);

        //    // create a key based on original key and derivation parmaters
        //    CryptographicKey keyOriginal = keyDerivationProvider.CreateKey(pwBuffer);
        //    IBuffer keyMaterial = CryptographicEngine.DeriveKeyMaterial(keyOriginal, pbkdf2Parms, 32);
        //    CryptographicKey derivedPwKey = keyDerivationProvider.CreateKey(pwBuffer);

        //    // derive buffer to be used for encryption salt from derived password key 
        //    IBuffer saltMaterial = CryptographicEngine.DeriveKeyMaterial(derivedPwKey, pbkdf2Parms, 16);

        //    // display the buffers – because KeyDerivationProvider always gets cleared after each use, they are very similar unforunately
        //    string keyMaterialString = CryptographicBuffer.EncodeToBase64String(keyMaterial);
        //    string saltMaterialString = CryptographicBuffer.EncodeToBase64String(saltMaterial);

        //    SymmetricKeyAlgorithmProvider symProvider = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
        //    // create symmetric key from derived password key
        //    CryptographicKey symmKey = symProvider.CreateSymmetricKey(keyMaterial);

        //    // encrypt data buffer using symmetric key and derived salt material
        //    IBuffer resultBuffer = CryptographicEngine.Encrypt(symmKey, plainBuffer, saltMaterial);
        //    byte[] result;
        //    CryptographicBuffer.CopyToByteArray(resultBuffer, out result);

        //    return result;
        //}

        //public static string Decrypt(byte[] encryptedData, string pw, string salt)
        //{
        //    IBuffer pwBuffer = CryptographicBuffer.ConvertStringToBinary(pw, BinaryStringEncoding.Utf8);
        //    IBuffer saltBuffer = CryptographicBuffer.ConvertStringToBinary(salt, BinaryStringEncoding.Utf16LE);
        //    IBuffer cipherBuffer = CryptographicBuffer.CreateFromByteArray(encryptedData);

        //    // Derive key material for password size 32 bytes for AES256 algorithm
        //    KeyDerivationAlgorithmProvider keyDerivationProvider = Windows.Security.Cryptography.Core.KeyDerivationAlgorithmProvider.OpenAlgorithm("PBKDF2_SHA1");
        //    // using salt and 1000 iterations
        //    KeyDerivationParameters pbkdf2Parms = KeyDerivationParameters.BuildForPbkdf2(saltBuffer, 1000);

        //    // create a key based on original key and derivation parmaters
        //    CryptographicKey keyOriginal = keyDerivationProvider.CreateKey(pwBuffer);
        //    IBuffer keyMaterial = CryptographicEngine.DeriveKeyMaterial(keyOriginal, pbkdf2Parms, 32);
        //    CryptographicKey derivedPwKey = keyDerivationProvider.CreateKey(pwBuffer);

        //    // derive buffer to be used for encryption salt from derived password key 
        //    IBuffer saltMaterial = CryptographicEngine.DeriveKeyMaterial(derivedPwKey, pbkdf2Parms, 16);

        //    // display the keys – because KeyDerivationProvider always gets cleared after each use, they are very similar unforunately
        //    string keyMaterialString = CryptographicBuffer.EncodeToBase64String(keyMaterial);
        //    string saltMaterialString = CryptographicBuffer.EncodeToBase64String(saltMaterial);

        //   // SymmetricKeyAlgorithmProvider symProvider = SymmetricKeyAlgorithmProvider.OpenAlgorithm("AES_CBC_PKCS7");
        //    SymmetricKeyAlgorithmProvider symProvider = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);
            
        //    // create symmetric key from derived password material
        //    CryptographicKey symmKey = symProvider.CreateSymmetricKey(keyMaterial);

        //    // encrypt data buffer using symmetric key and derived salt material
        //    IBuffer resultBuffer = CryptographicEngine.Decrypt(symmKey, cipherBuffer, saltMaterial);
        //    string result = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf16LE, resultBuffer);
        //    return result;
        //}

        private static IBuffer GetMD5Hash(string key)
        {
            // Convert the message string to binary data.
            IBuffer buffUtf8Msg = CryptographicBuffer.ConvertStringToBinary(key, BinaryStringEncoding.Utf8);

            // Create a HashAlgorithmProvider object.
            HashAlgorithmProvider objAlgProv = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5);

            // Hash the message.
            IBuffer buffHash = objAlgProv.HashData(buffUtf8Msg);

            // Verify that the hash length equals the length specified for the algorithm.
            if (buffHash.Length != objAlgProv.HashLength)
            {
                throw new Exception("There was an error creating the hash");
            }

            return buffHash;
        }

        /// <summary>
        /// Encrypt a string using dual encryption method. Returns an encrypted text.
        /// </summary>
        /// <param name="toEncrypt">String to be encrypted</param>
        /// <param name="key">Unique key for encryption/decryption</param>m>
        /// <returns>Returns encrypted string.</returns>
        public static string Encrypt(string toEncrypt, string key)
        {
            try
            {
                // Get the MD5 key hash (you can as well use the binary of the key string)
                var keyHash = GetMD5Hash(key);

                // Create a buffer that contains the encoded message to be encrypted.
                var toDecryptBuffer = CryptographicBuffer.ConvertStringToBinary(toEncrypt, BinaryStringEncoding.Utf8);

                // Open a symmetric algorithm provider for the specified algorithm.
                var aes = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);

                // Create a symmetric key.
                var symetricKey = aes.CreateSymmetricKey(keyHash);

                // The input key must be securely shared between the sender of the cryptic message
                // and the recipient. The initialization vector must also be shared but does not
                // need to be shared in a secure manner. If the sender encodes a message string
                // to a buffer, the binary encoding method must also be shared with the recipient.
                var buffEncrypted = CryptographicEngine.Encrypt(symetricKey, toDecryptBuffer, null);

                // Convert the encrypted buffer to a string (for display).
                // We are using Base64 to convert bytes to string since you might get unmatched characters
                // in the encrypted buffer that we cannot convert to string with UTF8.
                var strEncrypted = CryptographicBuffer.EncodeToBase64String(buffEncrypted);

                return strEncrypted;
            }
            catch (Exception ex)
            {
                // MetroEventSource.Log.Error(ex.Message);
                return "";
            }
        }

        /// <summary>
        /// Decrypt a string using dual encryption method. Return a Decrypted clear string
        /// </summary>
        /// <param name="cipherString">Encrypted string</param>
        /// <param name="key">Unique key for encryption/decryption</param>
        /// <returns>Returns decrypted text.</returns>
        public static string Decrypt(string cipherString, string key)
        {
            try
            {
                // Get the MD5 key hash (you can as well use the binary of the key string)
                var keyHash = GetMD5Hash(key);

                // Create a buffer that contains the encoded message to be decrypted.
                IBuffer toDecryptBuffer = CryptographicBuffer.DecodeFromBase64String(cipherString);

                // Open a symmetric algorithm provider for the specified algorithm.
                SymmetricKeyAlgorithmProvider aes = SymmetricKeyAlgorithmProvider.OpenAlgorithm(SymmetricAlgorithmNames.AesEcbPkcs7);

                // Create a symmetric key.
                var symetricKey = aes.CreateSymmetricKey(keyHash);

                var buffDecrypted = CryptographicEngine.Decrypt(symetricKey, toDecryptBuffer, null);

                string strDecrypted = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, buffDecrypted);

                return strDecrypted;
            }
            catch (Exception ex)
            {
                // MetroEventSource.Log.Error(ex.Message);
                //throw;
                return "";
            }
        }
     
    }
}
