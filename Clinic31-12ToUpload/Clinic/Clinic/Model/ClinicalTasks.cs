using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Model
{
    [DataContract(Name="ClinicalTasks")]
    class ClinicalTasks:ViewModel
    {
        [DataMember(Name = "id")]
        public string Id
        {
            get;
            set;
        }

        private int patientID;
        [DataMember(Name = "patientID")]
        public int PatientID
        {
            get { return patientID; }
            set { SetValue(ref patientID, value); }
        }

        private string status;
        [DataMember(Name = "status")]
        public string Status
        {
            get { return status; }
            set { SetValue(ref status, value); }
        }


        private string description;
        [DataMember(Name = "description")]
        public string Description
        {
            get { return description; }
            set { SetValue(ref description, value); }
        }

        [DataMember(Name = "userID")]
        public int UserID
        {
            get;
            set;
        }


    }
}
