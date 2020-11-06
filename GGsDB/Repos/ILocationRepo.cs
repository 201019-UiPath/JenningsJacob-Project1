using System.Collections.Generic;
using GGsDB.Models;

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