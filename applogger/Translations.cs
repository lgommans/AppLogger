using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace applogger
{
    class Translations
    {
        public static Dictionary<string, string> getTranslations(string language) {
            switch (language)
            {
                case "nl":
                    return nl();
                case "en":
                    return en();
                default:
                    return null;
            }
        }

        private static Dictionary<string, string> en()
        {
            Dictionary<string, string> translations = new Dictionary<string, string>();

            translations.Add("program name", "AppLogger");
            translations.Add("hour symbol", "h");
            translations.Add("btn start logging", "Start logging");
            translations.Add("status: idle", "Status: idle");
            translations.Add("status: starting...", "Status: starting...");
            translations.Add("btn stop logging", "Stop logging");
            translations.Add("status: logging to ", "Status: logging to ");
            translations.Add("journal", "journal");
            translations.Add("404, opening folder", "File not found; opening folder instead.");
            translations.Add("failed to open", "Failed to open");
            translations.Add("log saved at ", "Log saved at ");
            translations.Add("column info", "Columns: start time, duration, window title");
            translations.Add("incomplete time; app still open", "This duration is incomplete because the window is currently still open.");
            translations.Add("error writing log", "Could not write to log file. Is it currently open?");
            translations.Add("active", "active");
            translations.Add("click to open", "Click to open");

            return translations;
        }

        private static Dictionary<string, string> nl()
        {
            Dictionary<string, string> translations = new Dictionary<string, string>();

            translations.Add("program name", "AppLogger");
            translations.Add("hour symbol", "u");
            translations.Add("btn start logging", "Start loggen");
            translations.Add("status: idle", "Status: inactief");
            translations.Add("status: starting...", "Status: starten...");
            translations.Add("btn stop logging", "Stop loggen");
            translations.Add("status: logging to ", "Status: loggen naar ");
            translations.Add("journal", "journaal");
            translations.Add("404, opening folder", "Bestand niet gevonden; map geopend in plaats daarvan.");
            translations.Add("failed to open", "Openen mislukt");
            translations.Add("log saved at ", "Log opgeslagen om ");
            translations.Add("column info", "Kolommen: starttijd, duur, venstertitel");
            translations.Add("incomplete time; app still open", "Deze tijdsduur is incompleet omdat het venster op dit moment nog open staat.");
            translations.Add("error writing log", "Kon logbestand niet wegschrijven. Is het momenteel geopend?");
            translations.Add("active", "actief");
            translations.Add("click to open", "Klik om te openen");

            return translations;
        }
    }
}
