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
    [DataContract(Name="Perscription")]
    public class Perscription:ViewModel
    {
        [DataMember(Name = "perscriptionId")]
        public int PerscriptionId
        {
            get;
            set;
        }

        [DataMember(Name = "patientId")]
        public int PatientID
        {
            get;
            set;
        }

        [DataMember(Name = "DoctorId")]
        public int DoctorId
        {
            get;
            set;
        }

        private static IMobileServiceTable<Perscription> perscription = Connection.MobileService.GetTable<Perscription>();
        public static async void InsertNewPerscription(Perscription p)
        {
            try
            {
                await perscription.InsertAsync(p);
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Debug.WriteLine(e.Message);
            }

        }


        public static async void ReadPerscriptionList(List<Perscription> perscriptions)
        {

            perscriptions = await perscription.ToListAsync();

        }

        public static async void DeletePerscription(Perscription p)
        {
            await perscription.DeleteAsync(p);
        }

        public static async void UpdatePerscription(Perscription p)
        {
            await perscription.UpdateAsync(p);
        }

    }
}
