using SAP.Middleware.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioConfig
{
    class SAP_Connection
    {
        public static string Save_sap_settings(SharedSettings.Settings settings)
        {
            SAP_CNX conexion = new SAP_CNX();
            string mensajeconn = "";
            try
            {
                //Establecemos conexion con SAP
                RfcConfigParameters rfc = GetParameters(conexion, settings);

                RfcDestination rfcDest = null;
                rfcDest = RfcDestinationManager.GetDestination(rfc);

                //Creamos repositorio para la función
                RfcRepository repo = rfcDest.Repository;
                IRfcFunction save_settings = repo.CreateFunction("Z_MAIL_SYNC_SETTINGS");


                //save_settings.SetValue("BUKRS", settings.company_code);
                save_settings.SetValue("FOLDER_FILE", "");

                string DELETE_ATTACH = "";
                //if (settings.delete_files_at_attach)
                DELETE_ATTACH = "";

                save_settings.SetValue("DELETE_ATTACH", DELETE_ATTACH);

                //Ejecutamos la consulta
                save_settings.Invoke(rfcDest);

                IRfcStructure bapiret = save_settings.GetStructure("RETURN");

                string type = bapiret.GetString("TYPE");
                if (type == "E")
                {
                    return mensajeconn;
                }
                else { return mensajeconn; }
            }
            catch (RfcCommunicationException e)
            {
                //throw e;
                return e.Message;
            }
            catch (RfcLogonException e)
            {
                // user could not logon...
                return e.Message;
                //throw e;
            }
            catch (RfcAbapRuntimeException e)
            {
                // serious problem on ABAP system side...
                //throw e;
                return e.Message;
            }
            catch (RfcAbapBaseException e)
            {
                // The function module returned an ABAP exception, an ABAP message
                // or an ABAP class-based exception...
                //throw e;
                return e.Message;
            }
            catch (Exception e)
            {
                //throw e;
                return e.Message;
            }
        }
        public static RfcConfigParameters GetParameters(SAP_CNX cnx, SharedSettings.Settings settings)
        {
            RfcConfigParameters parms = new RfcConfigParameters();

            parms.Add(RfcConfigParameters.Name, settings.sap_name);
            parms.Add(RfcConfigParameters.AppServerHost, settings.sap_host);
            parms.Add(RfcConfigParameters.SystemNumber, settings.sap_sysnumber);
            parms.Add(RfcConfigParameters.User, settings.sap_user);
            parms.Add(RfcConfigParameters.Password, settings.sap_pass);
            parms.Add(RfcConfigParameters.Client, settings.sap_client);
            parms.Add(RfcConfigParameters.Language, "EN");
            parms.Add(RfcConfigParameters.SAPRouter, settings.SAPRouter);
            parms.Add(RfcConfigParameters.PoolSize, "5");
            parms.Add(RfcConfigParameters.PeakConnectionsLimit, "10");
            parms.Add(RfcConfigParameters.PoolIdleTimeout, "600");

            return parms;
        }
    }
    public class SAP_CNX
    {
        private string _sap_cnx = string.Empty;
        private string _sap_ip = string.Empty;
        private string _sap_synumber = string.Empty;
        private string _sap_user = string.Empty;
        private string _sap_pass = string.Empty;
        private string _sap_client = string.Empty;
        private string _sap_router = string.Empty;

        public string sap_cnx
        {
            get { return _sap_cnx; }
            set { _sap_cnx = value; }
        }
        public string sap_ip
        {
            get { return _sap_ip; }
            set { _sap_ip = value; }
        }
        public string sap_synumber
        {
            get { return _sap_synumber; }
            set { _sap_synumber = value; }
        }
        public string sap_user
        {
            get { return _sap_user; }
            set { _sap_user = value; }
        }
        public string sap_pass
        {
            get { return _sap_pass; }
            set { _sap_pass = value; }
        }
        public string sap_client
        {
            get { return _sap_client; }
            set { _sap_client = value; }
        }
        public string sap_router
        {
            get { return _sap_router; }
            set { _sap_router = value; }
        }
    }
}
