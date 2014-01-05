using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Clinic.Model
{
    [DataContract(Name="Appointment")]
    public class Appointment:ViewModel
    {



        //public Appointment()
        //{
           
        //}

        [DataMember(Name = "id")]
        public string Id
        {
            get;
            set;
        }

        //private string patientID;
        [DataMember(Name = "patientID")]
        public string PatientID
        {
            get;
            set;
        }


        [DataMember(Name = "AppoitmentID")]
        public string AppoitmentID
        {
            get;
            set;
        }
       // private string userID;

        [DataMember(Name = "userID")]
        public string UserID
        {
            get;
            set;
        }

        private TimeSpan timeFrom;
        [DataMember(Name="timeFrom")]

        public TimeSpan TimeFrom
        {
            get { return timeFrom; }
            set { SetValue(ref timeFrom, value); }
        }

        private TimeSpan timeTo;
        [DataMember(Name = "timeTo")]

        public TimeSpan TimeTo
        {
            get { return timeTo; }
            set { SetValue(ref timeTo, value); }
        }


        private DateTime date;
        [DataMember(Name="date")]
        public DateTime Date
        {
            get { return date; }
            set { SetValue(ref date, value); }
        }

        private string complaint;
        [DataMember(Name = "Complaint")]
        public string Complaint
        {
            get { return complaint; }

            set { SetValue(ref complaint, value); }
        }

        [JsonProperty(PropertyName = "__version")]
        public string Version { set; get; }

        //[Version]
        //public string Version { get; set; }


        public static IMobileServiceTable<Appointment> appointment = Connection.MobileService.GetTable<Appointment>();
    

      
    

        public static async void InsertNewAppointment(Appointment a)
        {
            appointment.SystemProperties |= MobileServiceSystemProperties.Version;
            string str= String.Empty;
            try
            {
                await appointment.InsertAsync(a);
            }
            catch (Exception e)
            {
                str = e.Message;
                //MessageBox.Show(e.Message, "Update Failed", MessageBoxButton.OK);
            }
          //  await new MessageDialog(str).ShowAsync();

        }

       
        //public static List<User> users = new List<User>();
        public static async Task<List<Appointment>> ReadAppointmentsList()
        {
           appointment.SystemProperties |= MobileServiceSystemProperties.Version;
            List<Appointment> appointments = null;
            try
            {
                appointments = await appointment.ToListAsync();
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return appointments;
        }

        public static async void DeleteAppointment(Appointment a)
        {
            await appointment.DeleteAsync(a);
        }

        public async void UpdateAppointment(Appointment a)
        {
            
           
           // await appointment.UpdateAsync(a);

            Exception exception = null;
            try
            {
                //update at the remote table
                await appointment.UpdateAsync(a);
            }
            catch (MobileServicePreconditionFailedException<Appointment> writeException)
            {
                exception = writeException;
            }
            catch (Exception ex)
            {
                exception = ex;
            }
            if (exception != null)
            {
                if (exception is MobileServicePreconditionFailedException<Appointment>)
                {
                    //conflict detected, the item has changed since the last query
                    await ResolveConflict(a, ((MobileServicePreconditionFailedException<Appointment>)exception).Item);
                }
                else
                   await new MessageDialog(exception.Message).ShowAsync();
            }
         
        }

        private async Task ResolveConflict(Appointment localItem, Appointment serverItem)
        {
            // Ask user to choose between the local text value or leaving the 
            // server's updated text value
            MessageDialog md = new MessageDialog(String.Format("The item has already been updated on the server.\n\n" +
                                                                   "Server value: {0} \n" +
                                                                   "Local value: {1}\n\n" +
                                                                   "Press OK to update the server with the local value.\n\n" +
                                                                   "Press CANCEL to keep the server value.", serverItem.Complaint, localItem.Complaint),
                                                                   "CONFLICT DETECTED ");

            bool? result = null;
            md.Commands.Add(
               new UICommand("OK", new UICommandInvokedHandler((cmd) => result = true)));
            md.Commands.Add(
               new UICommand("Cancel", new UICommandInvokedHandler((cmd) => result = false)));
            await md.ShowAsync();

            // OK : After examining the updated text from the server, overwrite it
            //      with the changes made in this client.
            if (result ==true)
            {
                // Update the version of the item to the latest version
                // to resolve the conflict. Otherwise the exception
                // will be thrown again for the attempted update.
                localItem.Version = serverItem.Version;
                // Recursively updating just in case another conflict 
                // occurs while the user is deciding.
                 this.UpdateAppointment(localItem);
                 Scheduling.Changed = !Scheduling.Changed;
            }
            // CANCEL : After examining the updated text from the server, leave 
            // the server item intact and refresh this client's query discarding 
            // the proposed changes.
           
        }

        public async static Task<Appointment> getAppointment(string id)
        {
            Appointment app = (await appointment.ToListAsync()).Where(ap => ap.AppoitmentID == id).ToList().First();
            return app;
        }
      

    }
}
