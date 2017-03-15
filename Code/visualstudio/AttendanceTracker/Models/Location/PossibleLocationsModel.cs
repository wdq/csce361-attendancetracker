using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AttendanceTracker.Models.Location
{
    public class PossibleLocationsModelLocation
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public PossibleLocationsModelLocation(AttendanceTracker.Building building, AttendanceTracker.Room room)
        {
            Id = room.Id.ToString();
            Name = building.Name + " " + room.Name + " (" + building.Code + "-" + room.Name + ")";
        }
    }

    public class PossibleLocationsModel
    {
        public List<PossibleLocationsModelLocation> Locations { get; set; }

        public PossibleLocationsModel()
        {
            List<PossibleLocationsModelLocation> locationsTemp = new List<PossibleLocationsModelLocation>();
            using (AttendanceTrackerDatabaseConnection context = new AttendanceTrackerDatabaseConnection())
            {
                foreach (var room in context.Rooms.ToList())
                {
                    var building = context.Buildings.FirstOrDefault(x => x.Id == room.BuildingId);
                    locationsTemp.Add(new PossibleLocationsModelLocation(building, room));
                }
            }
            Locations = locationsTemp.OrderBy(x => x.Name).ToList();
        }
    }
}