using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Model
{
    [DataContract(Name="Invitation")]
    public class Invitation:ViewModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "fromUserId")]
        public string FromUserId { get; set; }

        [DataMember(Name = "toUserId")]
        public string ToUserId { get; set; }

        [DataMember(Name = "patientID")]
        public int PatientID { get; set; }

        [DataMember(Name = "Complaint")]
        public string Complaint { get; set; }

            
       
        [DataMember(Name="timeFrom")]

        public TimeSpan TimeFrom
        {
            get ;
            set ;
        }
        [DataMember(Name = "timeTo")]
        public TimeSpan TimeTo
        {
            get;
            set;
        }

       // private DateTime date;
        [DataMember(Name="date")]
        public DateTime Date
        {
            get ;
            set ;
        }

       private bool isAccepted;
        [DataMember(Name="isAccepted")]

       public bool IsAccepted
       {
           get { return isAccepted; }
           set { SetValue(ref isAccepted, value); }
       }


        private static IMobileServiceTable<Invitation> invitation = Connection.MobileService.GetTable<Invitation>();
        public static async void InsertNewInvitation(Invitation i)
        {
            try
            {
                await invitation.InsertAsync(i);
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Debug.WriteLine(e.Message);
            }

        }

        //public static List<User> users = new List<User>();
        public static async Task<List<Invitation>> ReadInvitationsList()
        {
            List<Invitation> invitations = await invitation.ToListAsync();
            return invitations;

        }

        public static async Task<Invitation> getInvitation(Invitation inv)
        {
           Invitation ii =  (await invitation.ToListAsync()).Where(i => i.Date.Equals(inv.Date) && i.ToUserId.Equals(inv.ToUserId) && i.PatientID.Equals(inv.PatientID) && i.Date.Equals(inv.Date) && i.TimeFrom.Equals(inv.TimeFrom)).ToList().First();
           return ii;
        }

        public static async void DeleteInvitation(Invitation i)
        {
            await invitation.DeleteAsync(i);
        }

        public static async void UpdateInvitation(Invitation i)
        {
            await invitation.UpdateAsync(i);
        }


    }
}
