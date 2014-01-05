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
    [DataContract(Name="DrugList")]
    public class DrugList:ViewModel
    {

        [DataMember(Name = "perscriptionId")]
        public string PerscriptionId
        {
            get;
            set;
        }

        [DataMember(Name = "drugId")]
        public int DrugId
        {
            get;
            set;
        }

        private static IMobileServiceTable<DrugList> drugList = Connection.MobileService.GetTable<DrugList>();
        public static async void InsertNewDrugList(DrugList dl)
        {
            try
            {
                await drugList.InsertAsync(dl);
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Debug.WriteLine(e.Message);
            }

        }

        //public static List<User> users = new List<User>();
        public static async void ReadDrugListsList(List<DrugList> drugLists)
        {

            drugLists = await drugList.ToListAsync();

        }

        public static async void DeleteDrugList(DrugList dl)
        {
            await drugList.DeleteAsync(dl);
        }

        public static async void UpdateDrugList(DrugList dl)
        {
            await drugList.UpdateAsync(dl);
        }

    }
}
