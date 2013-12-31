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
    [DataContract(Name="Drug")]
    public class Drug:ViewModel
    {
        [DataMember(Name="drugId")]
        public int DrugId{
            get;
            set;
        }

        [DataMember(Name="drugName")]
        public string DrugName{
            get;
            set;
        }

        private static IMobileServiceTable<Drug> drug = Connection.MobileService.GetTable<Drug>();
        public static async void InsertNewDrug(Drug d)
        {
            try
            {
                await drug.InsertAsync(d);
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Debug.WriteLine(e.Message);
            }

        }

        //public static List<User> users = new List<User>();
        public static async void ReadDrugsList(List<Drug> drugs)
        {

            drugs = await drug.ToListAsync();

        }

        public static async void DeleteDrug(Drug d)
        {
            await drug.DeleteAsync(d);
        }

        public static async void UpdateDrug(Drug d)
        {
            await drug.UpdateAsync(d);
        }

    }
}
