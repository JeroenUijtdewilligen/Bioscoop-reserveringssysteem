using System;
using Project_Helpers;

namespace Project_Base {

    abstract class BaseDatabase {

        public abstract string GetFileName();

        public bool Load() {
            try {
                StorageHelper.LoadFile("data", this.GetFileName(), this);
                return true;
            } catch(Exception) {
                return false;
            }
        }

        public bool Save() {
            try {
                StorageHelper.SaveFile("data", this.GetFileName(), this);
                return true;
            } catch(Exception) {
                return false;
            }
        }

    }

}