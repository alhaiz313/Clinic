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
    /// <summary>
    /// A class used to store information about users in Windows Azure Mobile Service
    /// </summary>
    [DataContract(Name = "User")]
    public class User : ViewModel
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }


        [DataMember(Name = "isadmin")]
        public bool isAdmin { get; set; }

        [DataMember(Name = "userId")]
        public string UserId { get; set; }


        [DataMember(Name = "LiveSDKID")]
        public string LiveSDKID { get; set; }


        //[DataMember(Name = "isadmin")]
        //public bool IsAdmin { get; set; }


        [DataMember(Name = "created")]
        public DateTime CreatedDate { get; set; }

        [DataMember(Name = "imageUri")]
        public string ImageUri { get; set; }

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

        

        //private string city;

        //[DataMember(Name = "city")]
        //public string City
        //{
        //    get { return city; }
        //    set { SetValue(ref city, value); }
        //}

        //private string state;

        //[DataMember(Name = "state")]
        //public string State
        //{
        //    get { return state; }
        //    set { SetValue(ref state, value); }
        //}


        private string email;

        [DataMember(Name = "Email")]
        public string Email
        {
            get { return email; }
            set { SetValue(ref email, value); }
        }


        //private string phone;

        //[DataMember(Name = "phone")]
        //public string Phone
        //{
        //    get { return phone; }
        //    set { SetValue(ref phone, value); }
        //}

        private string type;

        [DataMember(Name = "type")]
        public string Type
        {
            get { return type; }
            set { SetValue(ref type, value); }
        }

        private static IMobileServiceTable<User> user = Connection.MobileService.GetTable<User>();
        public static async void InsertNewUser(User u)
        {
            try
            {
                await user.InsertAsync(u);
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Debug.WriteLine(e.Message);
            }

        }

        //public static List<User> users = new List<User>();
        public static async Task<List<User>> ReadUsersList()
        {
            List<User> users = new List<User>();
            users = await user.ToListAsync();
            return users;
        }

        public static async void DeleteUser(User u)
        {
            await user.DeleteAsync(u);
        }

        public static async Task<User> UpdateUser(User u)
        {
            await user.UpdateAsync(u);
            u = (await user.Where(uu => uu.UserId == u.UserId).ToListAsync()).First() ;
            return u;
        }

         public static async Task<User> getUser(string id)
        {
            User u = (await user.Where(uu => uu.UserId == id).ToListAsync()).First();
            return u;
        }
        //public enum UserType
        //{
        //    Undefined,
        //    Doctor,
        //    Nurse,
        //    Secretary,
        //    Manager
        //}

    }
}
