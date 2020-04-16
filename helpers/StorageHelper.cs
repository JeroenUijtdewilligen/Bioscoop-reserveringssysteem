using System.IO;
using Newtonsoft.Json;
using Project.Base;

namespace Project.Helpers {

    class StorageHelper {

        // Fill an objects properties from a json file
        public static void LoadFile(string category, string fileName, object obj) {
            StorageFile file = new StorageFile(category, fileName);

            // Read file and fill object
            string json = File.ReadAllText(file.location);
            JsonConvert.PopulateObject(json, obj);
        }

        // Saves the object properties to a file
        public static void SaveFile(string category, string fileName, object obj) {
            StorageFile file = new StorageFile(category, fileName);
            string json = JsonConvert.SerializeObject(obj);
            
            // Create file
            if(!File.Exists(file.location)) {
                Directory.CreateDirectory(file.directory);
                File.Create(file.location).Close();
            }

            // Write contents
            File.WriteAllText(file.location, json);
        }

    }

}