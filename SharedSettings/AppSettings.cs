using System;
using System.Collections;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace SharedSettings
{
    class AppSettings
    {

        public static void SettConfig(Settings settings)
        {
            FileInfo settingsFile;
            string commonAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string filename = String.Format(@"{0}.xml", "AppConfig");
            settingsFile = new FileInfo(Path.Combine(commonAppDataPath, "SF", filename));
            var serializer = new XmlSerializer(typeof(Settings));
            if (settingsFile.Directory != null && !settingsFile.Directory.Exists)
                settingsFile.Directory.Create();
            using (XmlWriter writer = new XmlTextWriter(File.Create(settingsFile.FullName), Encoding.UTF8))
            {
                serializer.Serialize(writer, settings);
            }
        }
        public static Settings getConfig(Settings settings)
        {
            FileInfo settingsFile;
            string commonAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            string filename = String.Format(@"{0}.xml", "AppConfig");
            settingsFile = new FileInfo(Path.Combine(commonAppDataPath, "SF", filename));
            // Check the path to the file
            if (settingsFile.Directory != null && settingsFile.Directory.Exists)
            {
                try
                {
                    var serializer = new XmlSerializer(typeof(Settings));
                    using (XmlReader reader = new XmlTextReader(File.OpenRead(settingsFile.FullName)))
                    {
                        settings = (Settings)serializer.Deserialize(reader);
                    }
                }
                catch (Exception)
                {
                }
            }
            return settings;
        }
    }
    public class Settings
    {
        public string sap_user
        { get; set; }

        public string sap_pass
        { get; set; }

        public string sap_host
        { get; set; }

        public string sap_sysnumber
        { get; set; }

        public string sap_name
        { get; set; }

        public string sap_client
        { get; set; }

        public string SAPRouter
        { get; set; }

        public string directorioExtra
        { get; set; }

        public string directorioLog
        { get; set; }

        public string directorioCtes
        { get; set; }

        public string directorioProv
        { get; set; }

        public bool esActivoDirProv
        { get; set; }

        public bool esActivoDirCtes
        { get; set; }

        public bool esActivoNomina
        { get; set; }

        public string directorioNomina
        { get; set; }

        public string directorioCtesC
        { get; set; }

        public string directorioProvC
        { get; set; }

        public string directorioNomC
        { get; set; }

        public string directorioCtesI
        { get; set; }

        public string directorioProvI
        { get; set; }

        public string directorioNomI
        { get; set; }

        public string prox_user
        { get; set; }

        public string prox_pass
        { get; set; }

        public string prox_host
        { get; set; }

        public int prox_puert
        { get; set; }

        public bool prox_act
        { get; set; }

        public bool esActivoProvC
        { get; set; }

        public bool esActivoCteC
        { get; set; }

        public bool esActivoNomC
        { get; set; }

        public bool esActivoProvI
        { get; set; }

        public bool esActivoCteI
        { get; set; }

        public bool esActivoNomI
        { get; set; }

        public bool esActivoWindows
        { get; set; }
        public Settings()
        {
            sap_client = "";
            sap_host = "";
            sap_name = "";
            sap_pass = "";
            sap_sysnumber = "";
            sap_user = "";
            SAPRouter = "";

            directorioCtes = "";
            directorioProv = "";
            directorioLog = "";
            esActivoDirCtes = false;
            esActivoDirProv = false;
            esActivoNomina = false;
            directorioNomina = "";

            directorioProvC = "";
            directorioProvI = "";
            directorioCtesC = "";
            directorioCtesI = "";
            directorioNomC = "";
            directorioNomI = "";
            esActivoProvC = false;
            esActivoCteC = false;
            esActivoNomC = false;
            esActivoProvI = false;
            esActivoCteI = false;
            esActivoNomI = false;

            prox_act = false;
            prox_host = "";
            prox_pass = "";
            prox_puert = 0;
            prox_user = "";

            esActivoWindows = false;
        }
    }
}
