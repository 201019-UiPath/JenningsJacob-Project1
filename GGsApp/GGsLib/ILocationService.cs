﻿using GGsDB.Models;
using System.Collections.Generic;

namespace GGsLib
{
    public interface ILocationService
    {
        void AddLocation(Location location);
        void DeleteLocation(Location location);
        List<Location> GetAllLocations();
        Location GetLocationById(int id);
        void UpdateLocation(Location location);
    }
}