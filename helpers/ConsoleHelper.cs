using System;
using System.Collections.Generic;
using Project.Base;
using Project.Enums;

namespace Project.Helpers {

    class ConsoleHelper {

        // Prints a typed message to the console
        public static void Print(PrintType type, string message) {
            string prefix = type == PrintType.Default ? "" : ("[" + type + "] ");
            System.Console.WriteLine(prefix + message);
        }

        // Prints a message to the console
        public static void Print(string message) {
            Print(PrintType.Default, message);
        }

        // Prints all errors for the specified model
        public static void PrintErrors(Model model) {
            foreach (KeyValuePair<string, List<string>> attribute in model.GetErrors()) {
                foreach (string error in attribute.Value) {
                    Print(PrintType.Error, attribute.Key + " -> " + error);
                }
            }
        }

        // Parses an int value
        public static int ParseInt(string value, string paramName = null) {
            return int.Parse(value);
        }

        // Parses an double value
        public static double ParseDouble(string value, string paramName = null) {
            try {
                return double.Parse(value);
            } catch (FormatException) {
                throw new ArgumentException((paramName ?? value) + " moet een double zijn");
            }
        }

        // Parses an boolean value
        public static bool ParseBoolean(string value, string paramName = null) {
            switch(value) {
                case "ja":
                case "yes":
                case "true":
                    return true;
                case "nee":
                case "no":
                case "false":
                    return false;
                default:
                    throw new ArgumentException((paramName ?? value) + " moet een boolean zijn");
            }
        }

        // Parses a date value
        public static DateTime ParseDate(string value = null, string paramName = null) {
            return ParseDatetime(value, null, paramName, "datum");
        }

        // Parses a time value
        public static DateTime ParseTime(string value = null, string paramName = null) {
            return ParseDatetime(null, value, paramName, "tijd");
        }

        // Parses an DateTime value
        public static DateTime ParseDatetime(string date = null, string time = null, string paramName = null, string expected = "datum en tijd") {
            DateTime now = DateTime.Now;

            // Fill missing parts
            if(date == null) {
                date = now.ToString(Program.DATE_FORMAT);
            }

            if(time == null) {
                time = now.ToString(Program.TIME_FORMAT);
            }

            // Prepare values
            string dateTimeFormat = Program.DATE_FORMAT + " " + Program.TIME_FORMAT;
            string dateTime = date + " " + time;

            // Try to parse
            try {
                return DateTime.ParseExact(dateTime, dateTimeFormat, System.Globalization.CultureInfo.InvariantCulture);
            } catch(Exception) {
                throw new ArgumentException((paramName ?? dateTime) + " moet een " + expected + " zijn");
            }
        }

    }

}