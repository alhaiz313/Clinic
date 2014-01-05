using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Model
{
     [DataContract(Name = "PatientImages")]
    class PatientImages:ViewModel
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


        [JsonProperty(PropertyName = "containerName")]
        public string ContainerName { get; set; }


        [JsonProperty(PropertyName = "resourceName")]
        public string ResourceName { get; set; }


        [JsonProperty(PropertyName = "sasQueryString")]
        public string SasQueryString { get; set; }


        [JsonProperty(PropertyName = "imageUri")]
        public string ImageUri { get; set; }
       
    
    }
}
