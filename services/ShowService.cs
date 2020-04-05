﻿using System;
using System.Collections.Generic;
using System.Linq;
using Project.Base;
using Project.Data;
using Project.Enums;
using Project.Helpers;
using Project.Models;
using Project.Records;

namespace Project.Services {

    class ShowService : Service {



        public override string GetHandle() {
            return "shows";
        }

        // Returns all shows
        public List<Show> GetShows() {
            Database database = Program.GetInstance().GetDatabase();
            List<Show> models = new List<Show>();
            foreach (ShowRecord record in database.shows) {
                models.Add(new Show(record));
            }

            return models;
        }

        // Returns a show by its id
        public Show GetShowById(int id) {
            Database database = Program.GetInstance().GetDatabase();

            try {
                return new Show(database.shows.Where(i => i.id == id).First());
            }
            catch (InvalidOperationException) {
                return null;
            }
        }
            // Saves the specified room
            public bool SaveShow(Show show) {
                Program app = Program.GetInstance();
                Database database = app.GetDatabase();
                bool isNew = show.id == -1;

                // Set id if its a new room
                if (isNew) {
                    show.id = database.GetNewId("shows");
                }

                // Find existing record
                ShowRecord record = database.shows.SingleOrDefault(i => i.id == show.id);

                // Add if no record exists
                if (record == null) {
                    record = new ShowRecord();
                    database.shows.Add(record);
                }

                // Update record
                record.id = show.id;
                record.dateTimeShow = show.dateTimeShow;
                record.roomId = show.roomId;
                record.movieId = show.movieId;

                // Try to save
                database.TryToSave();

                return true;
            }

            // Deletes the specified room
            public bool DeleteShow(Show show) {
                Program app = Program.GetInstance();
                Database database = app.GetDatabase();
                ShowRecord record = database.shows.SingleOrDefault(i => i.id == show.id);
                bool isNew = show.id == -1;
                // Return false if room doesn't exist
                if (record == null) {
                    return false;
                }

                // Remove record and chairs
                database.shows.Remove(record);
                // Try to save
                database.TryToSave();

                return true;
            }

       }

   }


