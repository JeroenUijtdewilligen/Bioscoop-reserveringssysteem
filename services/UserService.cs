using System;
using System.Collections.Generic;
using System.Linq;
using Project.Base;
using Project.Data;
using Project.Enums;
using Project.Helpers;
using Project.Models;
using Project.Records;

namespace Project.Services {

    class UserService : Service {

        // Current user (logged in user)
        private User currentUser;

        public override string GetHandle() {
            return "users";
        }

        public override void Load() {
            Database database = Program.GetInstance().GetDatabase();

            // Creating default user if we need to
            if (database.users.Count == 0) {
                User admin = new User("Admin user", "admin", EncryptionHelper.CreateHash("admin"), true);

                if (!SaveUser(admin)) {
                    ConsoleHelper.Print(PrintType.Error, "Failed to create default user");
                    return;
                }

                SetCurrentUser(admin);
                ConsoleHelper.Print(PrintType.Warning, "Created default admin user, please configure it.");
            }
        }

        // Returns all users
        public List<User> GetUsers() {
            Database database = Program.GetInstance().GetDatabase();
            List<User> models = new List<User>();

            foreach(UserRecord record in database.users) {
                models.Add(new User(record));
            }

            return models;
        }

        // Returns a user by its id
        public User GetUserById(int id) {
            Database database = Program.GetInstance().GetDatabase();

            try {
                return new User(database.users.Where(i => i.id == id).First());
            } catch (InvalidOperationException) {
                return null;
            }
        }

        // Returns a user by its username
        public User GetUserByUsername(string username) {
            Database database = Program.GetInstance().GetDatabase();

            try {
                return new User(database.users.Where(i => i.username.Equals(username)).First());
            } catch(InvalidOperationException) {
                return null;
            }
        }

        // Saves the specified user
        public bool SaveUser(User user) {
            Database database = Program.GetInstance().GetDatabase();
            bool isNew = user.id == -1;

            // Validate and add if valid
            if(!user.Validate()) {
                return false;
            }

            // Set id if its a new user
            if (isNew) {
                user.id = database.GetNewId("users");
            }

            // Find existing record
            UserRecord record = database.users.SingleOrDefault(i => i.id == user.id);
            
            // Add if no record exists
            if(record == null) {
                record = new UserRecord();
                database.users.Add(record);
            }

            // Update record
            record.id = user.id;
            record.fullName = user.fullName;
            record.username = user.username;
            record.password = user.password;
            record.admin = user.admin;

            return true;
        }

        // Deletes the specified user
        public bool DeleteUser(User user) {
            Database database = Program.GetInstance().GetDatabase();
            UserRecord record = database.users.SingleOrDefault(i => i.id == user.id);

            // Return false if room doesn't exist
            if (record == null) {
                return false;
            }

            // Remove record
            database.users.Remove(record);
            // TODO: Remove related reservations

            // Try to save
            database.TryToSave();

            return true;
        }

        // Returns current user
        public User GetCurrentUser() {
            return currentUser;
        }

        // Sets the current user
        public void SetCurrentUser(User currentUser) {
            this.currentUser = currentUser;
        }

    }

}
