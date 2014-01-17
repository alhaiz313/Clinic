// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

using Clinic.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Clinic.Common
{
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTime date = (DateTime)value;
            return date.ToString("MMMM dd, yyyy");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            string strValue = value as string;
            DateTime resultDateTime;
            if (DateTime.TryParse(strValue, out resultDateTime))
            {
                return resultDateTime;
            }
            throw new Exception(
                  "Unable to convert string to date time");
        }



    }


    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            TimeSpan time = (TimeSpan)value;
            return time.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();

        }



    }

    public class IndexConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int index = (int)value;
            return (index > -1) ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }



    public class GenderConverter : IValueConverter
    {
        string male = "Assets/1385591353_male.png";
        string female = "Assets/1385591356_female.png";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string Gender = (string)value;
            return (Gender == "Male") ? male : female;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }


    public class HistoryConverter : IValueConverter
    {
        string str = "History info available";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            PatientHistory pHistory = (PatientHistory)value;
            return (!String.IsNullOrEmpty(pHistory.PatientId)) ? str : String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    //AppointmentConverter
    public class AppointmentConverter : IValueConverter
    {
        // string str = "Appointment";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Appointment app = (Appointment)value;
            return (!String.IsNullOrEmpty(app.PatientID )) ? "Appointment" : String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    //HistoryVisibility
    public class HistoryVisibility : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            PatientHistory pHistory = (PatientHistory)value;
            return (!String.IsNullOrEmpty(pHistory.PatientId )) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    //AppConverter

    public class AppConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Appointment app = (Appointment)value;
            return (!String.IsNullOrEmpty(app.PatientID))  ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class OutLookEventConverter : IValueConverter
    {
        //string str = "History info available";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            OutLookEvent myOEvent = (OutLookEvent)value;

            return (myOEvent != null) ? "Outlook Event: " + myOEvent.Name : String.Empty;//"Available Info": String.Empty;
            // "Outlook Event: "+ myOEvent.Name : String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }


    }

    // 

    public class OutLookEventConverterWeekly : IValueConverter
    {
        //string str = "History info available";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            OutLookEvent myOEvent = (OutLookEvent)value;
            return (myOEvent != null && !myOEvent.isAllDay) ? "Outlook Event: " + myOEvent.Name : String.Empty;//"Available Info": String.Empty;
            // "Outlook Event: "+ myOEvent.Name : String.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }


    }


    public class StaffItem1Converter : IValueConverter
    {
        //string str = "History info available";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            User u = (User)value;

            return (u != null ? u.FName : null);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }


    }


    public class TotalConverter : IValueConverter
    {
        //string str = "History info available";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int total = (int)value;

            return (total != 0 ? total.ToString() : String.Empty);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }


    }

    public class ShiftConverter : IValueConverter
    {
        //string str = "History info available";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            WorkHours wh = (WorkHours)value;

            return (wh.From.TimeOfDay.ToString().Equals(wh.To.TimeOfDay.ToString()) ? null : (wh.From.TimeOfDay.Hours == 7 ? "Day Shift" : wh.From.TimeOfDay.Hours == 15 ? "Swing Shift" : "Night Shift"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }


    }

    //WorkHoursConverter

    public class WorkHoursConverter : IValueConverter
    {
        //string str = "History info available";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            WorkHours wh = (WorkHours)value;

            return (wh.From.TimeOfDay.ToString().Equals(wh.To.TimeOfDay.ToString()) ? null : wh.From.TimeOfDay.ToString() + "-" + wh.To.TimeOfDay.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }


    }

    public class BreakConverter : IValueConverter
    {
        //string str = "History info available";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            WorkHours wh = (WorkHours)value;

            return ((wh.BreakTimeFrom.ToString().Equals(wh.BreakTimeTo.ToString())) ? null : wh.BreakTimeFrom.ToString() + "-" + wh.BreakTimeTo.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }


    }

    //phone-icon-th.png
    public class OnCallConverter : IValueConverter
    {
        //string str = "History info available";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            WorkHours wh = (WorkHours)value;

            return (wh.OnCallOnly ? "Assets/phone-icon-th.png" : "");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }


    }


    public class ColorConverter : IValueConverter
    {
        //string str = "History info available";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            WorkHours wh = (WorkHours)value;

            return (String.IsNullOrEmpty(wh.TimeOffReason) ? "" : wh.TimeOffReason.Equals("Time Off") ? "Red" : wh.TimeOffReason.Equals("Vacation") ? "Green" : wh.TimeOffReason.Equals("Business Related") ? "Violet" : wh.TimeOffReason.Equals("Family Emergency") ? "Pink" : wh.TimeOffReason.Equals("Absent") ? "Blue" : wh.TimeOffReason.Equals("Sick Leave") ? "Yellow" : "Red");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }


    }

    public class WidthConverter : IValueConverter
    {
        //string str = "History info available";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            WorkHours wh = (WorkHours)value;

            return (wh.TimeOffFrom.Equals(wh.TimeOffTo) ? 0 : 100);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }


    }

    public class TimeOffConverter : IValueConverter
    {
        //string str = "History info available";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            WorkHours wh = (WorkHours)value;

            return (String.IsNullOrEmpty(wh.TimeOffReason)  ? wh.TimeOffReason : String.Empty);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }


    }

    public class EventConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            NameValueItem nvi = (NameValueItem)value;


            return (nvi.myOutLookEvent == null && nvi.workHours != null ? 0 : (nvi.myOutLookEvent != null && nvi.workHours != null) ? 30 : 60);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }



    }

    public class EventConverter2 : IValueConverter
    {
        //string str = "History info available";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            NameValueItem nvi = (NameValueItem)value;

            // return ( nvi.workHours != null ? Visibility.Visible : Visibility.Collapsed);
            return (nvi.workHours != null && nvi.myOutLookEvent == null ? 60 : (nvi.workHours != null && nvi.myOutLookEvent != null) ? 30 : 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }


    }

    public class Underscore : IValueConverter
    {
        //string str = "History info available";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string timeStr = (string)value;

            // return ( nvi.workHours != null ? Visibility.Visible : Visibility.Collapsed);
            return (String.IsNullOrEmpty(timeStr) ? "" : "         _");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }


    }

    public class Underscore2 : IValueConverter
    {
        //public static Appointment app = new Appointment();
        //string str = "History info available";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string app = (string)value;

            // return ( nvi.workHours != null ? Visibility.Visible : Visibility.Collapsed);
            return (String.IsNullOrEmpty(app) ? "_" : app);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();

        }


    }


    public class PopupAppVisibility : IValueConverter
    {
        //public static Appointment app = new Appointment();
        //string str = "History info available";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Appointment app = (Appointment)value;


            return ((app != null) ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();

        }


    }

    public class PopupInvVisibility : IValueConverter
    {
        //public static Appointment app = new Appointment();
        //string str = "History info available";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Invitation app = (Invitation)value;


            return ((app != null) ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();

        }


    }

    public class Underscore4 : IValueConverter
    {
        //public static Appointment app = new Appointment();
        //string str = "History info available";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string app = (string)value;
            Patient pa = null;
            if (!String.IsNullOrEmpty(app))
            {
                pa = Scheduling.getPatient(app);
            }

            return (pa == null) ? "_" : ("_" + pa.LName + ", " + pa.FName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();

        }


    }

    public class Underscore5 : IValueConverter
    {
        //public static Appointment app = new Appointment();
        //string str = "History info available";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string doc = (string)value;
            User doctor = null;
            if (!String.IsNullOrEmpty(doc))
            {
                doctor = Scheduling.getUser(doc);
            }
            return (doctor == null) ? "_" : ("_" + doctor.LName + ", " + doctor.FName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();

        }


    }

    public class Underscore3 : IValueConverter
    {
        //string str = "History info available";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int width = (int)value;

            // return ( nvi.workHours != null ? Visibility.Visible : Visibility.Collapsed);
            return (width == 135 ? "_" : "");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }


    }


    //InvitationButton

    public class InvitationButton : IValueConverter
    {
        //string str = "History info available";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string inv = (string)value;

            return (String.IsNullOrEmpty(inv) ? Visibility.Collapsed : Visibility.Visible);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();

        }

    }
    //InvitationWidth

    public class InvitationWidth : IValueConverter
    {
        //string str = "History info available";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string inv = (string)value;

            return (String.IsNullOrEmpty(inv) ? 0 : 20);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();

        }

    }

    //AppWidth


    public class AppWidth : IValueConverter
    {
        //string str = "History info available";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string inv = (string)value;

            return (String.IsNullOrEmpty(inv) ? 135 : 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();

        }

    }


    public class DateTimeToDateTimeOffsetConverter : IValueConverter
    {





        public object Convert(object value, Type targetType, object parameter, string language)
        {


            try
            {


                DateTime date = (DateTime)value;


                return new DateTimeOffset(date);


            }


            catch (Exception ex)
            {


                return DateTimeOffset.MinValue;


            }


        }





        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {


            try
            {


                DateTimeOffset dto = (DateTimeOffset)value;


                return dto.DateTime;


            }


            catch (Exception ex)
            {


                return DateTime.MinValue;


            }


        }


    }

    //PatientNameConverter
    public class PatientNameConverter : IValueConverter
    {
        //string str = "History info available";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Patient pa = (Patient)value;

            return (pa.LName + ", " + pa.FName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();

        }

    }

    public class AppIdToDoc : IValueConverter
    {
        //string str = "History info available";
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string appId = (string)value;
            Appointment app = null;
            if (!String.IsNullOrEmpty(appId))
            {
                app = NewPatient.getAppointment(appId);
            }
            User u = NewPatient.getUser(app.UserID);
            return (u.LName + ", " + u.FName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();

        }

    }

    public class AppIdToPatient : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string appId = (string)value;
            Appointment app = null;
            if (!String.IsNullOrEmpty(appId))
            {
                app = NewPatient.getAppointment(appId);
            }
            Patient u = NewPatient.getPatient(app.PatientID);
            return (u.LName + ", " + u.FName);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();

        }

    }

    public class AppIdToDate : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string appId = (string)value;
            Appointment app = null;
            if (!String.IsNullOrEmpty(appId))
            {
                app = NewPatient.getAppointment(appId);
            }

            try
            {
                DateTime date = (DateTime)app.Date;
                return new DateTimeOffset(date);
            }
            catch (Exception ex)
            {
                return DateTimeOffset.MinValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();

        }
    }

    public class AppIdToFromTime : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string appId = (string)value;
            Appointment app = null;
            if (!String.IsNullOrEmpty(appId))
            {
                app = NewPatient.getAppointment(appId);
            }
            TimeSpan ts = app.TimeFrom;
            return (ts);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();

        }

    }

    public class AppIdToToTime : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string appId = (string)value;
            Appointment app = null;
            if (!String.IsNullOrEmpty(appId))
            {
                app = NewPatient.getAppointment(appId);
            }
            TimeSpan ts = app.TimeTo;
            return (ts);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();

        }

    }


    public class AppIdToComplaint : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string appId = (string)value;
            Appointment app = null;
            if (!String.IsNullOrEmpty(appId))
            {
                app = NewPatient.getAppointment(appId);
            }
            string s = app.Complaint;
            return (s);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();

        }

    }
    //PatientEncounterConverter
    public class PatientEncounterConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            PatientEncounter pe = (PatientEncounter)value;
            //if(pe.AppointmentId!=null)
            return ((pe.AppointmentId != null) ? "Patient Encounter" : "");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();

        }

    }

    //NotCheckedConverter
    public class NotCheckedConverter : IValueConverter
    {
        public NotCheckedConverter()
        {
            VisibleValue = true;
        }

        public bool VisibleValue { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || value.GetType() != typeof(System.Boolean))
            {
                return Visibility.Collapsed;
            }
            bool interpreted = System.Convert.ToBoolean(value);
            return interpreted == VisibleValue ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            //if (value == null || value.GetType() != typeof(Visibility))
            //{
            //    return !VisibleValue;
            //}

            //Visibility visibility = (Visibility)value;

            //return visibility == Visibility.Visible ? VisibleValue : !VisibleValue;
            throw new NotImplementedException();
        }
    }

}
