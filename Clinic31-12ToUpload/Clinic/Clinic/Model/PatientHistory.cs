using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Clinic.Model
{
    [DataContract(Name = "PatientHistory")]
    public class PatientHistory:ViewModel
    {

        [DataMember(Name = "id")]
        public string PatientInfoId
        {
            get;
            set;
        }

        private int patientId;
        [DataMember(Name = "PatientId")]
        public int PatientId
        {
            get { return patientId; }
            set { SetValue(ref patientId, value); }
        }

        private DateTime infoDate;

        [DataMember(Name = "infoDate")]
        public DateTime InfoDate
        {
            get { return infoDate; }
            set { SetValue(ref infoDate, value); }
        }

        private string cholestrol;

        [DataMember(Name = "cholestrol")]
        public string Cholestrol
        {
            get { return cholestrol; }
            set { SetValue(ref cholestrol, value); }
        }

        private string bloodPressure;

        [DataMember(Name = "bloodPressure")]
        public string BloodPressure
        {
            get { return bloodPressure; }
            set { SetValue(ref bloodPressure, value); }
        }

      

        private string pulse;

        [DataMember(Name = "pulse")]
        public string Pulse
        {
            get { return pulse; }
            set { SetValue(ref pulse, value); }
        }

        private string rr;

        [DataMember(Name = "rr")]
        public string Rr
        {
            get { return rr; }
            set { SetValue(ref rr, value); }
        }

        private string temperature;

        [DataMember(Name = "temperature")]
        public string Temperature
        {
            get { return temperature; }
            set { SetValue(ref temperature, value); }
        }

        private string weight;

        [DataMember(Name = "weight")]
        public string Weight
        {
            get { return weight; }
            set { SetValue(ref weight, value); }
        }

        private string height;

        [DataMember(Name = "height")]
        public string Height
        {
            get { return height; }
            set { SetValue(ref height, value); }
        }


        //allergies
        private string info1;

        [DataMember(Name = "info1")]
        public string Info1
        {
            get { return info1; }
            set { SetValue(ref info1, value); }
        }


        //Immunization
        private string info2;

        [DataMember(Name = "info2")]
        public string Info2
        {
            get { return info2; }
            set { SetValue(ref info2, value); }
        }

        //progress
        private string info3;

        [DataMember(Name = "info3")]
        public string Info3
        {
            get { return info3; }
            set { SetValue(ref info3, value); }
        }
        
    }
}
