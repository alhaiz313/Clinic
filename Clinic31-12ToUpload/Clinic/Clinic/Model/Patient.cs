using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Model
{
    [DataContract(Name = "Patient")]
    public class Patient:ViewModel
    {
       // private string patientID;

        [DataMember(Name = "id")]
        public string PatientID
        {
            get;
            set;
        }

        private string fname;

        [DataMember(Name = "fname")]
        public string FName
        {
            get { return fname; }
            set { SetValue(ref fname, value); }
        }

        private string lname;

        [DataMember(Name = "lname")]
        public string LName
        {
            get { return lname; }
            set { SetValue(ref lname, value); }
        }

        private DateTime dob;
        [DataMember(Name="dob")]
        public DateTime Dob
        {
            get { return dob; }
            set { SetValue(ref dob, value); }
        }

        private string gender;
        [DataMember(Name = "gender")]
        public string Gender
        {
            get { return gender; }
            set { SetValue(ref gender, value); }
        }

        private string maritalStatus;

        [DataMember(Name = "maritalStatus")]
        public string MaritalStatus
        {
            get { return maritalStatus; }
            set { SetValue(ref maritalStatus, value); }
        }

        private string socialID;

        [DataMember(Name = "socialID")]
        public string SocialID
        {
            get { return socialID; }
            set { SetValue(ref socialID, value); }
        }

        private string email;

        [DataMember(Name = "email")]
        public string Email
        {
            get { return email; }
            set { SetValue(ref email, value); }
        }

        private string phone;

        [DataMember(Name = "phone")]
        public string Phone
        {
            get { return phone; }
            set { SetValue(ref phone, value); }
        }


        [JsonProperty(PropertyName = "containerName")]
        public string ContainerName { get; set; }


        [JsonProperty(PropertyName = "resourceName")]
        public string ResourceName { get; set; }


        [JsonProperty(PropertyName = "sasQueryString")]
        public string SasQueryString { get; set; }


        [JsonProperty(PropertyName = "imageUri")]
        public string ImageUri { get; set; }
       
        private static IMobileServiceTable<Patient> patient = Connection.MobileService.GetTable<Patient>();
        public static async void InsertNewPatient(Patient p)
        {
            try
            {
                await patient.InsertAsync(p);
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Debug.WriteLine(e.Message);
            }

        }

       
        public static async Task<List<Patient>> ReadPatientsList()
        {
            List<Patient> patients;
            patients = await patient.ToListAsync();
            return patients;
        }

        public static async Task<Patient> getPatient(string id) 
        {
            List<Patient> patients = await patient.ToListAsync();
            Patient p = patients.Where(pp => pp.PatientID.Equals(id)).ToList().First();
            return p;
        }

        public static async void DeletePatient(Patient p)
        {
            await patient.DeleteAsync(p);
        }

        public static async void UpdatePatient(Patient p)
        {
            await patient.UpdateAsync(p);
        }


    }
}
