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
    [DataContract(Name="ClinicInfo")]
    public class ClinicInfo:ViewModel
    {
        [DataMember(Name = "id")]
        public int Id
        {
            get;
            set;
        }

        private string applicationKey;
        [DataMember(Name="applicationKey")]
        public string ApplicationKey
        {
            get { return applicationKey; }
            set { SetValue(ref applicationKey, value); }
        }

        private string applicationURL;
        [DataMember(Name="applicationURL")]
        public string ApplicationURL
        {
            get { return applicationURL; }
            set { SetValue(ref applicationURL, value); }
        }

        private string name;
        [DataMember(Name="name")]
        public string Name
        {
            get { return name; }
            set { SetValue(ref name, value); }
        }

        private string phone;
        [DataMember(Name = "phone")]
        public string Phone
        {
            get { return phone; }
            set { SetValue(ref phone, value); }
        }

        private string email;
        [DataMember(Name = "email")]
        public string Email
        {
            get { return email; }
            set { SetValue(ref email, value); }
        }

        private static IMobileServiceTable<ClinicInfo> clinicInfo = Connection.MobileService.GetTable<ClinicInfo>();
        public static async void InsertNewClinicInfo(ClinicInfo ci)
        {
            try
            {
                await clinicInfo.InsertAsync(ci);
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Debug.WriteLine(e.Message);
            }

        }

        //public static List<User> users = new List<User>();
        public static async void ReadClinicInfosList(List<ClinicInfo> clinicInfos)
        {

            clinicInfos = await clinicInfo.ToListAsync();

        }

        public static async void DeleteClinicInfo(ClinicInfo ci)
        {
            await clinicInfo.DeleteAsync(ci);
        }

        public static async void UpdateUser(ClinicInfo ci)
        {
            await clinicInfo.UpdateAsync(ci);
        }

    }
}
