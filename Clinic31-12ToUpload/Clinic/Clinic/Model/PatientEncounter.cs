using Microsoft.WindowsAzure.MobileServices;
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
    [DataContract(Name="PatientEncounter")]
    public class PatientEncounter:ViewModel
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name="appointmentId")]
        public string AppointmentId{
            get;
            set;
        }


        private string diagnostic;
        [DataMember(Name = "diagnostics")]
        public string Diagnostics
        {
            get { return diagnostic; }
            set { SetValue(ref diagnostic, value); }
        }

        private string notes;
        [DataMember(Name = "notes")]
        public string Notes
        {
            get { return notes; }
            set { SetValue(ref notes, value); }
        }

        private string drugs;
        [DataMember(Name = "drugs")]
        public string Drugs
        {
            get { return drugs; }
            set { SetValue(ref drugs, value); }
        }

        //private string perscriptionId;
        //[DataMember(Name = "perscriptionId")]
        //public string PerscriptionId
        //{
        //    get { return perscriptionId; }
        //    set { SetValue(ref perscriptionId, value); }
        //}





        private static IMobileServiceTable<PatientEncounter> patientEncounter = Connection.MobileService.GetTable<PatientEncounter>();
        public static async void InsertNewPatientEncounter(PatientEncounter pe)
        {
            try
            {
                await patientEncounter.InsertAsync(pe);
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Debug.WriteLine(e.Message);
            }

        }

        public static async Task<PatientEncounter> getPatientEncounter(Appointment app)
        {
            PatientEncounter pee = null;
            try
            {
                 pee = (await patientEncounter.Where(pe => pe.AppointmentId.Equals(app.Id)).ToListAsync()).First();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Debug.WriteLine(e.Message);
            }
            return pee;
        }


        public static async Task<List<PatientEncounter>> ReadPatientEncountersList()
        {

            List<PatientEncounter> patientEncounters = await patientEncounter.ToListAsync();
            return patientEncounters;

        }

        public static async void DeletePatientEncounter(PatientEncounter pe)
        {
            try
            {
                await patientEncounter.DeleteAsync(pe);
            }
            catch
            {
                new MessageDialog("Deleting error").ShowAsync();
            }
        }

        public static async void UpdatePatientEncounter(PatientEncounter pe)
        {
            await patientEncounter.UpdateAsync(pe);
        }

    }
}
