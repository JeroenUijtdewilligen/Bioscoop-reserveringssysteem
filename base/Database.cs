using System;
using System.Collections.Generic;
using Project.Helpers;

namespace Project.Base {

    abstract class Database {

        // List to keep track of the used ids
        // You can get a new unique id using GetNewId()
        public Dictionary<string, int> newIds { get; set; } = new Dictionary<string, int>();

        // Returns the name of the database file
        public abstract string GetFileName();

        // Loads the database file into this object
        public bool Load() {
            try {
                StorageHelper.LoadFile("data", this.GetFileName(), this);
                return true;
            } catch(Exception) {
                return false;
            }
        }

        // Saves this object into the database file
        public bool Save() {
            try {
                StorageHelper.SaveFile("data", this.GetFileName(), this);
                return true;
            } catch(Exception) {
                return false;
            }
        }

        // Returns a new unique id for the specified category
        public int GetNewId(string category) {
            if(category == null) {
                category = "__global__";
            }

            // Grab id
            int newId;
            if(!newIds.TryGetValue(category, out newId)) {
                newId = 1;
            }

            // Increase and return
            newIds[category] = newId + 1;
            return newId;
        }

    }

}