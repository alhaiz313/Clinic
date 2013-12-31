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

   
     [DataContract(Name = "WorkHours")]
    public class WorkHours:ViewModel
    {
         [DataMember(Name = "id")]
         public string Id { get; set; }

         private string employeeId;
         [DataMember(Name = "EmployeeId")]
         public string EmployeeId {
             get { return employeeId; } 
             set{SetValue(ref employeeId, value);}
         }

         private DateTime from;
         [DataMember(Name="From")]
         public DateTime From
         {
             get { return from; }
             set { SetValue(ref from, value); }
         }

         private DateTime to;
         [DataMember(Name = "To")]
         public DateTime To
         {
             get { return to; }
             set { SetValue(ref to, value); }
         }

         private bool onCallOnly;
         [DataMember(Name = "OnCallOnly")]
         public bool OnCallOnly
         {
             get { return onCallOnly; }
             set { SetValue(ref onCallOnly, value); }
         }

         private TimeSpan breakTimeFrom;
         [DataMember(Name = "BreakTimeFrom")]
         public TimeSpan BreakTimeFrom
         {
             get { return breakTimeFrom; }
             set { SetValue(ref breakTimeFrom, value); }
         }

         private TimeSpan breakTimeTo;
         [DataMember(Name = "BreakTimeTo")]
         public TimeSpan BreakTimeTo
         {
             get { return breakTimeTo; }
             set { SetValue(ref breakTimeTo, value);
             }
         }


         private DateTime timeOffFrom;
         [DataMember(Name = "TimeOffFrom")]
         public DateTime TimeOffFrom
         {
             get { return timeOffFrom; }
             set
             {
                 SetValue(ref timeOffFrom, value);
             }
         }

         private DateTime timeOffTo;
         [DataMember(Name = "TimeOffTo")]
         public DateTime TimeOffTo
         {
             get { return timeOffTo; }
             set
             {
                 SetValue(ref timeOffTo, value);
             }
         }

         private string timeOffReason;
          [DataMember(Name = "TimeOffReason")]
         public string TimeOffReason
         {
             get { return timeOffReason; }
             set
             {
                 SetValue(ref timeOffReason, value);
             }
         }

          //private int total;
          //[DataMember(Name = "Total")]
          //public int Total
          //{
          //    get { return total; }
          //    set
          //    {
          //        SetValue(ref total, value);
          //    }
          //}

          private static IMobileServiceTable<WorkHours> workHours = Connection.MobileService.GetTable<WorkHours>();

          public async static Task<List<WorkHours>> ReadListAsync()
          {

              List<WorkHours> whList = await workHours.ToListAsync();
              return whList;
          }


          public static async Task InsertNewWorkHours(WorkHours whs)
          {

              
              try
              {
                  await workHours.InsertAsync(whs);
              }
              catch (MobileServiceInvalidOperationException e)
              {
                  MessageDialog errormsg = new MessageDialog(e.Response.ReasonPhrase+ "Only the admin is authorized to organize the work hours.");
                    var ignoreAsyncOpResult = errormsg.ShowAsync();
              }

          }

          public static async Task UpdateWorkHours(WorkHours u)
          {
              await workHours.UpdateAsync(u);
              //u = (await user.Where(uu => uu.UserId == u.UserId).ToListAsync()).First();
              //return u;
          }

          public static async Task DeleteWorkHours(WorkHours wh)
          {
              await workHours.DeleteAsync(wh);
          }

    }

}
