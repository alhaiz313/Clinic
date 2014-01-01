using Clinic.Common;
using Clinic.Model;
using Microsoft.Live;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
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
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.WindowsAzure;


using Microsoft.WindowsAzure.Storage;


namespace Clinic
{

    class PatientViewModel : ViewModel
    {

      //  string StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=patientimagestorage;AccountKey=CaynPcsbf2ilTFFCPHlNdpqbO4o91S6bGUydE2O2Fn3eRdrotEPXhDMGH/bshn/qvfFxufy0T4xLOYJEmDcLGA==";


        private  bool refreshFlag = false;
        public  bool RefreshFlag
        {
            get { return refreshFlag; }
            set
            {
                SetValue(ref refreshFlag, value);
            }
        }

        private bool refreshFlag2 = false;
        public bool RefreshFlag2
        {
            get { return refreshFlag2; }
            set
            {
                SetValue(ref refreshFlag2, value);
            }
        }

        //private SynchronizationContext synchronizationContext;
        StorageFile media;

        private IMobileServiceTable<Patient> patientsTable;
        public IMobileServiceTable<PatientHistory> patientsInfoTable;
        public IMobileServiceTable<PatientDocument> patientsDocumentTable;

        public DelegateCommand Add_Patient { get; private set; }

        public DelegateCommand Cancel_Add_Patient { get; private set; }

        public DelegateCommand Add_Patient_Info { get; private set; }

        public DelegateCommand Cancel_Add_Patient_Info { get; private set; }

        public DelegateCommand PhotoPicker { get; private set; }

        public DelegateCommand Add_Patient_Document { get; private set; }

        public DelegateCommand Cancel_Add_Patient_Document { get; private set; }

        public DelegateCommand FilePicker { get; private set; }



        private void SetupCommands()
        {
            Add_Patient = new DelegateCommand(async () =>
               {
                   Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 2);
           
                   MessageDialog mm = new MessageDialog("mission Started");
                   //await mm.ShowAsync();

                   // Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Wait, 2);

                   if (string.IsNullOrWhiteSpace(Pa.FName) ||
                      string.IsNullOrWhiteSpace(Pa.LName) ||
                      string.IsNullOrWhiteSpace(Pa.Gender) ||
                      string.IsNullOrWhiteSpace(Pa.MaritalStatus) ||
                      string.IsNullOrWhiteSpace(Pa.SocialID) ||
                      string.IsNullOrWhiteSpace(Pa.Email) ||
                      string.IsNullOrWhiteSpace(Pa.Phone))
                   {
                       //show message
                       mm = new MessageDialog("Please fill in all the fields");
                       await mm.ShowAsync();
                       return;
                   }

                  // Pa.Dob = Convert.ToDateTime(String.Format("{0:d}", Pa.Dob));
                   switch (Pa.Gender)
                   {
                       case "0":
                           Pa.Gender = "Male";
                           break;

                       case "1":
                           Pa.Gender = "Female";
                           break;
                   }

                   switch (Pa.MaritalStatus)
                   {
                       case "0":
                           Pa.MaritalStatus = "Single";
                           break;

                       case "1":
                           Pa.MaritalStatus = "Married";
                           break;
                   }

                   /////////////////////////////////////////////////////////////// save the image
                   string errorString = string.Empty;

                   if (media != null)
                   {
                       // Set blob properties of TodoItem.
                       Pa.ContainerName = "patientimagestorageimages";
                       Pa.ResourceName = media.Name;
                   }
                   else
                   {
                       if( Pa.Gender == "Male"){

                           var uri = new Uri("ms-appx:///Assets/Administrator-icon.png");

                           media = await StorageFile.GetFileFromApplicationUriAsync(uri);

                           Pa.ContainerName = "patientimagestorageimages";
                           Pa.ResourceName = media.Name;
                       }
                       else
                       {
                           var uri = new Uri("ms-appx:///Assets/Office-Girl-icon.png");

                           media = await StorageFile.GetFileFromApplicationUriAsync(uri);

                           Pa.ContainerName = "patientimagestorageimages";
                           Pa.ResourceName = media.Name;
                       }
                   }
                  



                   /////////////////////////////////////////////////////////////////
                   try
                   {
                       await patientsTable.InsertAsync(Pa);
                   }
                   catch (Exception e)
                   {
                       mm = new MessageDialog("error occured");
                       mm.ShowAsync();
                   }

                   //if (media != null)
                   //{
                   if (!string.IsNullOrEmpty(Pa.SasQueryString))
                   {
                       using (var fileStream = await media.OpenStreamForReadAsync())
                       {
                           StorageCredentials cred = new StorageCredentials(Pa.SasQueryString);
                           var imageUri = new Uri(Pa.ImageUri);
                           CloudBlobContainer container = new CloudBlobContainer(
                       new Uri(string.Format("https://{0}/{1}",
                           imageUri.Host, Pa.ContainerName)), cred);

                           // Upload the new image as a BLOB from the stream.
                           CloudBlockBlob blobFromSASCredential =
                               container.GetBlockBlobReference(Pa.ResourceName);
                           await blobFromSASCredential.UploadFromStreamAsync(fileStream.AsInputStream());
                       }
                   }
                   //}

                   /////////////////////////////////////////////////////////////////
                   MessageDialog m = new MessageDialog("Mission Done");
                  // await m.ShowAsync();

                   //  Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 2);


                   ShowAddPatientTemplate = false;
                   NotDisplayPatientForm = true;

                   DisplayRegistrationColor = "LightGray";

                   createPatient();
                 RefreshFlag = !RefreshFlag;
                 media = null;

                 Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 2);
           
               }, true);


            Cancel_Add_Patient = new DelegateCommand(() =>
           {
               ShowAddPatientTemplate = false;
               NotDisplayPatientForm = true;
               DisplayRegistrationColor = "LightGray";
               createPatient();

           }, true);


            PhotoPicker = new DelegateCommand(async () =>
            {


                FileOpenPicker open = new FileOpenPicker();
                open.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                open.ViewMode = PickerViewMode.Thumbnail;

                // Filter to include a sample subset of file types
                open.FileTypeFilter.Clear();
                open.FileTypeFilter.Add(".bmp");
                open.FileTypeFilter.Add(".png");
                open.FileTypeFilter.Add(".jpeg");
                open.FileTypeFilter.Add(".jpg");

                media = await open.PickSingleFileAsync();


                if (media != null)
                {
                    // Ensure the stream is disposed once the image is loaded
                    FileStream = await media.OpenAsync(Windows.Storage.FileAccessMode.Read);


                    await MybitmapImage.SetSourceAsync(FileStream);


                    //BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);
                    //PixelDataProvider pixelData = await decoder.GetPixelDataAsync();
                    //Pa.patientImage= pixelData.DetachPixelData();




                }
                open = null;
                // createPatient();
            }, true);


            Add_Patient_Info = new DelegateCommand(async () =>
            {

            //   await new MessageDialog("Adding info").ShowAsync();

             //  await new MessageDialog(ComboPatient).ShowAsync();
               try
               {
                  // Pi.PatientId = (await patientsTable.Where(p => p.FName == ComboPatient).Select(patient => patient.PatientID).ToListAsync()).First();
                   string ln = ComboPatient.Substring(0, ComboPatient.IndexOf(","));
                   string fn = ComboPatient.Substring(ComboPatient.IndexOf(",") + 2);
                   Pi.PatientId = (await patientsTable.Where(p => (p.LName == (ln) && p.FName == (fn))).Select(patient => patient.PatientID).ToListAsync()).First();

               }
               catch(Exception e)
               {
                   new MessageDialog(e.Message).ShowAsync();
                   return;
               }
                try
                {
                    await patientsInfoTable.InsertAsync(Pi);
                }
                catch(Exception e)
                {
                    new MessageDialog(e.Message).ShowAsync();
                    return;
                }
                await new MessageDialog("finished Adding info").ShowAsync();
                ShowAddPatientInfoTemplate = false;
                NotDisplayPatientForm = true;

                DisplayRegistrationColor = "LightGray";

                createPatientHistory();
                RefreshFlag2 = !RefreshFlag2;
                

            }, true);

            Cancel_Add_Patient_Info = new DelegateCommand(async () =>
            {
                ShowAddPatientInfoTemplate = false;
                NotDisplayPatientForm = true;

                DisplayRegistrationColor = "LightGray";

                createPatientHistory();

            }, true);

            Add_Patient_Document = new DelegateCommand(async () =>
            {
                if (string.IsNullOrWhiteSpace(ComboPatient) ||string.IsNullOrWhiteSpace(SelectedFile))
                      
                {
                    //show message
                    MessageDialog m = new MessageDialog("Please fill in all the fields");
                    await m.ShowAsync();
                    return;
                }
                await new MessageDialog("Adding info").ShowAsync();
                await new MessageDialog(ComboPatient).ShowAsync();
                string ln = ComboPatient.Substring(0, ComboPatient.IndexOf(","));
                string fn = ComboPatient.Substring(ComboPatient.IndexOf(",")+2);
                Pd.PatientID = (await patientsTable.Where(p =>( p.LName==(ln ) && p.FName==(fn) )).Select(patient => patient.PatientID).ToListAsync()).First();


                if (media != null)
                {
                    // Set blob properties of TodoItem.
                    Pd.ContainerName = "patientimagestorageimages";
                    Pd.ResourceName = media.Name;
                }

                Pd.CreatedAt = DateTime.Now;

                string temp = (Pd.ResourceName).Substring(Pd.ResourceName.IndexOf("."));

                switch (temp)
                {
                    case ".pdf": Pd.FileTypeImage = "Assets/FileType-Pdf-icon.png";
                        break;
                    case ".txt": Pd.FileTypeImage = "Assets/FileType-Txt-icon.png";
                        break;
                    case ".doc": Pd.FileTypeImage = "Assets/FileType-Doc-icon.png";
                        break;
                    case ".xls": Pd.FileTypeImage = "Assets/FileType-Xls-icon.png";
                        break;
                    case ".docx": Pd.FileTypeImage = "Assets/FileType-Doc-icon.png";
                        break;
                    case ".xlsx": Pd.FileTypeImage = "Assets/FileType-Xls-icon.png";
                        break;

                    default:    Pd.FileTypeImage = "Assets/Basic-Document-icon.png";
                        break;
                
                }

                try
                {
                    await patientsDocumentTable.InsertAsync(Pd);
                }
                catch (Exception e)
                {
                    new MessageDialog("error occured").ShowAsync();
                }

                if (!string.IsNullOrEmpty(Pd.SasQueryString))
                {
                    using (var fileStream = await media.OpenStreamForReadAsync())
                    {
                        

                        StorageCredentials cred = new StorageCredentials(Pd.SasQueryString);
                        var imageUri = new Uri(Pd.ImageUri);
                        CloudBlobContainer container = new CloudBlobContainer(
                    new Uri(string.Format("https://{0}/{1}",
                        imageUri.Host, Pd.ContainerName)), cred);

                     

                        // Upload the new image as a BLOB from the stream.
                        CloudBlockBlob blobFromSASCredential = container.GetBlockBlobReference(Pd.ResourceName);
                        await blobFromSASCredential.UploadFromStreamAsync(fileStream.AsInputStream());
                    }
                }
                //CloudStorageAccount storageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(StorageConnectionString); //.Parse(CloudConfigurationManager.GetSetting("MarketviewConnectionString"));
               // CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();


              

               // // Retrieve a reference to a container. 
               // CloudBlobContainer container = blobClient.GetContainerReference("mycontainer");

               // // Create the container if it doesn't already exist.
               // await container.CreateIfNotExistsAsync();

               // await container.SetPermissionsAsync(new BlobContainerPermissions
               //     {
               //         PublicAccess =
               //             BlobContainerPublicAccessType.Blob
               //     });

               // CloudBlockBlob blockBlob = container.GetBlockBlobReference("myblob");

               // // Create or overwrite the "myblob" blob with contents from a local file.
               // using (var fileStream = System.IO.File.OpenRead(@"path\myfile"))
               // {
               //    await  blockBlob.UploadFromStreamAsync(fileStream);
               // }

                /////////////////////////////////////////////////////////////////
                  new MessageDialog("Mission Done").ShowAsync();


                ShowAddPatientDocumentTemplate = false;
                NotDisplayPatientForm = true;

                DisplayRegistrationColor = "LightGray";

                Pd = null;
                createPatientDocument();
                
                media = null;
                SelectedFile =null;
                RefreshFlag2 = !RefreshFlag2;
            }, true);


            Cancel_Add_Patient_Document = new DelegateCommand(async () =>
            {
                ShowAddPatientDocumentTemplate = false;
                NotDisplayPatientForm = true;

                DisplayRegistrationColor = "LightGray";

                Pd = null;
                createPatientDocument();
                
                media = null;
                SelectedFile = null;
            }, true);


            FilePicker = new DelegateCommand(async () =>
            {


                FileOpenPicker open = new FileOpenPicker();
                open.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                open.ViewMode = PickerViewMode.Thumbnail;

                // Filter to include a sample subset of file types
                open.FileTypeFilter.Clear();
                open.FileTypeFilter.Add(".pdf");
                open.FileTypeFilter.Add(".doc");
                open.FileTypeFilter.Add(".docx");
                open.FileTypeFilter.Add(".txt");
                open.FileTypeFilter.Add(".xls");
                open.FileTypeFilter.Add(".xlsx");
                open.FileTypeFilter.Add(".bmp");
                open.FileTypeFilter.Add(".png");
                open.FileTypeFilter.Add(".jpeg");
                open.FileTypeFilter.Add(".jpg");
               

                media = await open.PickSingleFileAsync();


                if (media != null)
                {
                    // Ensure the stream is disposed once the image is loaded
                    FileStream = await media.OpenAsync(Windows.Storage.FileAccessMode.Read);

                    SelectedFile = media.Name;
                    

                }
                open = null;
              
            }, true);
        }


        public PatientViewModel()
        {

            SetupCommands();


        }

        private string displayRegistrationColor = "LightGray";

        public string DisplayRegistrationColor
        {
            get { return displayRegistrationColor; }
            set { SetValue(ref displayRegistrationColor, value); }
        }


        private bool showAddPatientTemplate = false;
        public bool ShowAddPatientTemplate
        {
            get { return showAddPatientTemplate; }
            set { SetValue(ref showAddPatientTemplate, value); }
        }

       

        private bool showAddPatientInfoTemplate = false;
        public bool ShowAddPatientInfoTemplate
        {
            get { return showAddPatientInfoTemplate; }
            set { SetValue(ref showAddPatientInfoTemplate, value); }
        }


        private bool showAddPatientDocumentTemplate = false;
        public bool ShowAddPatientDocumentTemplate 
        {
            get { return showAddPatientDocumentTemplate; }
            set { SetValue(ref showAddPatientDocumentTemplate, value); }
        }

        public void createPatient()
        {

            patientsTable = Connection.MobileService.GetTable<Patient>();
            Pa = new Patient
            {
                FName = String.Empty,
                LName = String.Empty,
                Dob = DateTime.Today,
                Gender = String.Empty,
                MaritalStatus = String.Empty,
                SocialID = String.Empty,
                Phone = String.Empty,
                Email = String.Empty,
                ImageUri = String.Empty,
                ResourceName = String.Empty,
                SasQueryString = String.Empty,
                ContainerName = String.Empty
            };

            try
            {
                Pa.Dob = new DateTime(1969, 7, 21) + new TimeSpan(2,2,2);
            }
            catch(Exception e)
            {
                new MessageDialog(e.Message).ShowAsync();
            }


            media = null;
            FileStream = null;
            createNewBitmapImage();
            MybitmapImage.ClearValue(BitmapImage.UriSourceProperty);

        }

       private List<string> items;
       public List<string> Items
       {
           get { return items; }
           set { SetValue(ref items, value); }
       }

        public async void createPatientHistory()
        {

            patientsInfoTable = Connection.MobileService.GetTable<PatientHistory>();
            Pi = new PatientHistory
            {
             PatientId = 0,  
             Cholestrol = String.Empty,
             InfoDate = DateTime.Now,
             BloodPressure = String.Empty,
             Pulse = String.Empty,
             Rr = String.Empty,
             Temperature = String.Empty,
             Weight = String.Empty,
             Height = String.Empty,
             Info1 = String.Empty,
             Info2 = String.Empty,
             Info3 = String.Empty,
 
            };

            Items = await getListOfNames();
            // Items = new List<string>() { "a","b" };

        }


        public async void createPatientDocument()
        {

            patientsDocumentTable = Connection.MobileService.GetTable<PatientDocument>();
            Pd = new PatientDocument
            {
               PatientID = 0
            };

            Items = await getListOfNames();
        }



        public void createNewBitmapImage()
        {
            MybitmapImage = new BitmapImage();
        }

        private Patient pa;

        public Patient Pa
        {
            get { return pa; }
            set { SetValue(ref pa, value); }
        }

        private PatientHistory pi;
        public PatientHistory Pi
        {
            get { return pi; }
            set { SetValue(ref pi, value); }
        }


        private PatientDocument pd;
        public PatientDocument Pd
        {
            get { return pd; }
            set { SetValue(ref pd, value); }
        }


        private BitmapImage mybitmapImage;
        public BitmapImage MybitmapImage
        {
            get { return mybitmapImage; }
            set { SetValue(ref mybitmapImage, value); }
        }

        private IRandomAccessStream fileStream;

        public IRandomAccessStream FileStream
        {
            get { return fileStream; }
            set { SetValue(ref fileStream, value); }
        }

        public async Task<List<Patient>> getList()
        {
            List<Patient> pList = new List<Patient>();
            pList = await patientsTable.Take(500).ToListAsync();
            //await new MessageDialog((pList.Count).ToString()).ShowAsync();
            return pList;

        }

        //public async Task<List<Patient>> getPatientDocumentList()
        //{
        //    List<PatientDocument> pdList = new List<PatientDocument>();
        //    pList = await patientsDocumentTable.Take(500).ToListAsync();
        //    //await new MessageDialog((pList.Count).ToString()).ShowAsync();
        //    return pList;

        //}

        public async Task<List<string>> getListOfNames()
        {
            List<string> pList = new List<string>();
            pList = await patientsTable.Select(patient=>( patient.LName + ", " + patient.FName)).Take(500).ToListAsync();
            //await new MessageDialog((pList.Count).ToString()).ShowAsync();
            return pList;

        }

        private Patient selectedPatient;
        public Patient SelectedPatient
        {
            get { return selectedPatient;}
            set { SetValue(ref selectedPatient, value); }
        }


        private bool notdisplayPatientForm = true;

        public bool NotDisplayPatientForm
        {
            get { return notdisplayPatientForm; }
            set { SetValue(ref notdisplayPatientForm, value); }
        }

        private string comboPatient;
        public string ComboPatient
        {
            get { return comboPatient; }
            set { SetValue(ref comboPatient, value); }
        }

        private string selectedFile;
        public string SelectedFile
        {
            get { return selectedFile; }
            set { SetValue(ref selectedFile, value); }
        }

          private bool showChart = false;
          public bool ShowChart
          {
              get { return showChart; }
              set { SetValue(ref showChart, value); }
          }
    }
}
