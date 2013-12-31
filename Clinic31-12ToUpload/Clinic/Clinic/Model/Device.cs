// Copyright (c) Microsoft Corporation. All rights reserved

using Microsoft.WindowsAzure.MobileServices;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Clinic.Model
{
    /// <summary>
    /// Used to store the channel URI for each user and installation in Windows Azure Mobile Service
    /// </summary>
    [DataContract(Name = "device")]
    public class Device
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "userId")]
        public string UserId { get; set; }

        [DataMember(Name = "installationId")]
        public string InstallationId { get; set; }

        [DataMember(Name = "channelUri")]
        public string ChannelUri { get; set; }

        private static IMobileServiceTable<Device> device = Connection.MobileService.GetTable<Device>();
        public static async void InsertNewDevice(Device d)
        {
            try
            {
                await device.InsertAsync(d);
            }
            catch (MobileServiceInvalidOperationException e)
            {
                Debug.WriteLine(e.Message);
            }

        }

        //public static List<User> users = new List<User>();
        public static async void ReadDevicesList(List<Device> devices)
        {

            devices = await device.ToListAsync();

        }

        public static async void DeleteDevice(Device d)
        {
            await device.DeleteAsync(d);
        }

        public static async void UpdateDevice(Device d)
        {
            await device.UpdateAsync(d);
        }

    }
}
