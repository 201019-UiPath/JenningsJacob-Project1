using GGsDB.Models;
using System.Collections.Generic;

namespace GGsDB.Repos
{
    public interface ILocationRepo
    {
        void AddLocation(Location location);
        void UpdateLocation(Location location);
        Location GetLocationById(int id);
        List<Location> GetAllLocations();
        void DeleteLocation(Location location);
    }
}