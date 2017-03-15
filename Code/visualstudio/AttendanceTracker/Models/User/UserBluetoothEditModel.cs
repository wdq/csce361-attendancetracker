using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceTracker.Models.User
{
    public class UserBluetoothEditModel
    {
        public string AddOrEdit { get; set; }
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public UserBluetoothEditModel()
        {
            
        }

        public UserBluetoothEditModel(UserBluetooth userBluetooth, string addOrEdit)
        {
            Id = userBluetooth.Id;
            UserId = userBluetooth.UserId;
            Name = userBluetooth.Name;
            Address = userBluetooth.Address;
            AddOrEdit = addOrEdit;
        }

        public static UserBluetoothEditModel UserBluetoothEdit(string userId, string bluetoothId)
        {
            string addOrEdit;
            if (string.IsNullOrEmpty(bluetoothId))
            {
                UserBluetoothEditModel model = new UserBluetoothEditModel(new UserBluetooth(), "Add");
                model.UserId = new Guid(userId);
                return model;
            }
            else
            {
                UserBluetooth bluetooth = new UserBluetooth();
                using (AttendanceTrackerDatabaseConnection context = new AttendanceTrackerDatabaseConnection())
                {
                    bluetooth = context.UserBlueteeth.FirstOrDefault(x => x.Id == new Guid(bluetoothId));       
                }
                UserBluetoothEditModel model = new UserBluetoothEditModel(bluetooth, "Edit");
                return model;
            }
        }

        public static UserBluetooth UserBluetoothEditPost(UserBluetoothEditModel model)
        {
            UserBluetooth bluetooth = new UserBluetooth();

            using (AttendanceTrackerDatabaseConnection context = new AttendanceTrackerDatabaseConnection())
            {
                bluetooth = context.UserBlueteeth.FirstOrDefault(x => x.Id == model.Id);

                if (bluetooth != null)
                {
                    bluetooth.Name = model.Name;
                    bluetooth.Address = model.Address;
                }
                else
                {
                    bluetooth = new UserBluetooth();

                    bluetooth.Id = Guid.NewGuid();
                    bluetooth.Name = model.Name;
                    bluetooth.UserId = model.UserId;
                    bluetooth.Address = model.Address;

                    context.UserBlueteeth.Add(bluetooth);
                }
                context.SaveChanges();
            }
            return bluetooth;
        }
    }
}