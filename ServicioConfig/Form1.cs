using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharedSettings;
using TaskScheduler;

namespace ServicioConfig
{
    public partial class Form1 : Form
    {
        Settings settings = new Settings();
        public Form1()
        {
            InitializeComponent();
            CargarConf();
            HabilitadosGeneral();
            HabilitadosProxi();
            Prox_Ejec();
        }
        private void Guardar()
        {
            btnGuardar.Enabled = false;
            settings.sap_name = txtDescripcion.Text;
            settings.sap_host = txtServidor.Text;
            settings.sap_sysnumber = txtInstancia.Text;
            settings.sap_client = txtMandante.Text;
            settings.sap_user = txtUsuario.Text;
            settings.sap_pass = txtCveA.Text;
            settings.SAPRouter = txtRouter.Text;

            settings.esActivoDirCtes = chkCliente.Checked;
            settings.esActivoDirProv = chkProveedor.Checked;
            settings.esActivoNomina = chkNomina.Checked;
            settings.directorioCtes = txtCliente.Text;
            settings.directorioProv = txtProveedor.Text;
            settings.directorioNomina = txtNomina.Text;
            settings.directorioLog = txtLog.Text;

            settings.directorioCtesC = txtClienteC.Text;
            settings.directorioProvC = txtProveedorC.Text;
            settings.directorioNomC = txtNominaC.Text;
            settings.directorioCtesI = txtClienteI.Text;
            settings.directorioProvI = txtProveedorI.Text;
            settings.directorioNomI = txtNominaI.Text;
            settings.esActivoCteC = chkClienteC.Checked;
            settings.esActivoProvC = chkProveedorC.Checked;
            settings.esActivoNomC = chkNominaC.Checked;
            settings.esActivoCteI = chkClienteI.Checked;
            settings.esActivoProvI = chkProveedorI.Checked;
            settings.esActivoNomI = chkNominaI.Checked;

            settings.prox_act = chkProxy.Checked;
            settings.prox_host = txtIPProxy.Text;
            settings.prox_puert = Convert.ToInt32(txtPuerProxy.Text);
            settings.prox_user = txtUsuProxy.Text;
            settings.prox_pass = txtPassProxy.Text;

            settings.esActivoWindows = chkInicioW.Checked;

            if (String.IsNullOrEmpty(settings.directorioLog))
            {
                MessageBox.Show("Agrege un directorio en el Log", "¡Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnGuardar.Enabled = true;
                return;
            }
            if (String.IsNullOrEmpty(settings.prox_host) && chkProxy.Checked)
            {
                MessageBox.Show("Agrege la direccion del proxy", "¡Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnGuardar.Enabled = true;
                return;
            }
            if (IniciarWindows() == false)
            {
                btnGuardar.Enabled = true;
                return;
            }
            AppSettings.SettConfig(settings);//guarda la configuracion
            string mensajeconn = "";
            if (chkProxy.Checked)
            {
                mensajeconn = funcionProxy();
                if (mensajeconn != "")
                {
                    MessageBox.Show(mensajeconn, "¡Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnGuardar.Enabled = true;
                    return;
                }
            }
            mensajeconn = SAP_Connection.Save_sap_settings(settings);//valida la conexion en sap
            btnGuardar.Enabled = true;
            if (mensajeconn == "")
            {
                MessageBox.Show("¡¡Guardado Correctamente!!", "¡Correcto!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(mensajeconn, "¡Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void CargarConf()
        {
            settings = AppSettings.getConfig(settings);
            txtDescripcion.Text = settings.sap_name;
            txtServidor.Text = settings.sap_host;
            txtInstancia.Text = settings.sap_sysnumber;
            txtMandante.Text = settings.sap_client;
            txtUsuario.Text = settings.sap_user;
            txtCveA.Text = settings.sap_pass;
            txtRouter.Text = settings.SAPRouter;

            chkCliente.Checked = settings.esActivoDirCtes;
            chkProveedor.Checked = settings.esActivoDirProv;
            chkNomina.Checked = settings.esActivoNomina;
            txtCliente.Text = settings.directorioCtes;
            txtProveedor.Text = settings.directorioProv;
            txtNomina.Text = settings.directorioNomina;
            txtLog.Text = settings.directorioLog;

            txtClienteC.Text = settings.directorioCtesC;
            txtProveedorC.Text = settings.directorioProvC;
            txtNominaC.Text = settings.directorioNomC;
            txtClienteI.Text = settings.directorioCtesI;
            txtProveedorI.Text = settings.directorioProvI;
            txtNominaI.Text = settings.directorioNomI;
            chkClienteC.Checked = settings.esActivoCteC;
            chkProveedorC.Checked = settings.esActivoProvC;
            chkNominaC.Checked = settings.esActivoNomC;
            chkClienteI.Checked = settings.esActivoCteI;
            chkProveedorI.Checked = settings.esActivoProvI;
            chkNominaI.Checked = settings.esActivoNomI;

            chkProxy.Checked = settings.prox_act;
            txtIPProxy.Text = settings.prox_host;
            txtPuerProxy.Text = settings.prox_puert.ToString();
            txtUsuProxy.Text = settings.prox_user;
            txtPassProxy.Text = settings.prox_pass;

            chkInicioW.Checked = settings.esActivoWindows;
        }
        private string examinarRuta(string dir)
        {
            string res = dir;
            folderBrowserDialog1.SelectedPath = dir;
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                res = folderBrowserDialog1.SelectedPath;
            }

            return res;
        }
        private string Ruta(string dir)
        {
            string res = dir;

            openFileDialog1.FileName = Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("Config", "");
            openFileDialog1.InitialDirectory = (Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6));
            openFileDialog1.Filter = "Exe Files (.exe)|*.exe|All Files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    res = openFileDialog1.FileName;
                }
                catch (Exception e)
                {

                }
            }

            return res;
        }
        private string funcionProxy()
        {
            WebClient request = new WebClient();
            string msm = "";
            try
            {
                WebProxy myProxy = new WebProxy();
                string url = "http://" + settings.prox_host + ":" + settings.prox_puert;

                Uri newUri = new Uri(url);
                // Associate the newUri object to 'myProxy' object so that new myProxy settings can be set.
                myProxy.Address = newUri;
                // Create a NetworkCredential object and associate it with the 
                // Proxy property of request object.
                myProxy.Credentials = new NetworkCredential(settings.prox_user, settings.prox_pass);
                request.Proxy = myProxy;
                byte[] vs = request.UploadData("https://consultaqr.facturaelectronica.sat.gob.mx/ConsultaCFDIService.svc", new byte[] { });
                //request.DownloadString("https://consultaqr.facturaelectronica.sat.gob.mx/ConsultaCFDIService.svc");
            }
            catch (Exception)
            {
                msm = "Error de proxy: No pudo conectar a internet \r Verifique su configuracion.";
            }
            return msm;
        }
        private void SoloNum(ref KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {
                if (Char.IsControl(e.KeyChar)) //permitir teclas de control como retroceso 
                {
                    e.Handled = false;
                }
                else
                {
                    //el resto de teclas pulsadas se desactivan 
                    e.Handled = true;
                }
            }
        }
        private void HabilitadosGeneral()
        {
            if (chkInicioW.Checked == true)
            {
                Hab();
                chkEdit.Enabled = true;
            }
            else
            {
                Dehab();
                chkEdit.Checked = false;
                chkEdit.Enabled = false;
            }
            if ((chkEdit.Checked == true && label18.Visible == true) || (chkInicioW.Checked == true && label18.Visible == false))
            {
                Hab();
            }
            else
            {
                Dehab();
            }
        }
        private void HabilitadosProxi()
        {
            if (chkProxy.Checked == true)
            {
                txtIPProxy.Enabled = true;
                txtPuerProxy.Enabled = true;
                txtUsuProxy.Enabled = true;
                txtPassProxy.Enabled = true;
                label10.Enabled = true;
                label11.Enabled = true;
                label12.Enabled = true;
                label13.Enabled = true;
            }
            else
            {
                txtIPProxy.Enabled = false;
                txtPuerProxy.Enabled = false;
                txtUsuProxy.Enabled = false;
                txtPassProxy.Enabled = false;
                label10.Enabled = false;
                label11.Enabled = false;
                label12.Enabled = false;
                label13.Enabled = false;
            }
        }
        private bool IniciarWindows()
        {
            string nombreApp = (Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)).Replace("Config.exe", "");
            ScheduledTasks Tareas = new ScheduledTasks();
            string[] Existen = Tareas.GetTaskNames();
            short hora = Convert.ToInt16(TimePicker.Text.Substring(0, 2));
            short min = Convert.ToInt16(TimePicker.Text.Substring(3, 2));
            string pass = null;

            if (chkInicioW.Checked == true)
            {
                try
                {
                    //Referencia https://www.codeproject.com/Articles/2407/A-New-Task-Scheduler-Class-Library-for-NET

                    if (!Existen.Contains(nombreApp + ".job"))
                    {
                        TaskScheduler.Task tarea = Tareas.CreateTask(nombreApp);
                        tarea.ApplicationName = txtRutaRead.Text;
                        tarea.Comment = "Tarea para ejecutar el ReadFiles diariamente";
                        tarea.SetAccountInformation(Environment.UserName, pass);
                        tarea.Creator = Environment.UserName;
                        tarea.Priority = System.Diagnostics.ProcessPriorityClass.Normal;
                        tarea.Triggers.Add(new DailyTrigger(hora, min));
                        tarea.Flags = TaskFlags.RunOnlyIfLoggedOn;
                        tarea.Save();
                        tarea.Close();
                    }
                    else if (chkEdit.Checked)
                    {
                        TaskScheduler.Task tarea = Tareas.OpenTask(nombreApp);
                        tarea.Triggers.RemoveAt(0);
                        tarea.Triggers.Add(new DailyTrigger(hora, min));
                        tarea.Save();
                        tarea.Close();
                    }
                    Tareas.Dispose();
                    Prox_Ejec();
                    txtPassWind.Text = "";
                    return true;
                }
                catch (Exception e)
                {
                    Tareas.DeleteTask(nombreApp);
                    Tareas.Dispose();
                    MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Prox_Ejec();
                    return false;
                }
            }
            else
            {
                try
                {
                    Tareas.DeleteTask(nombreApp);
                    Tareas.Dispose();
                    Prox_Ejec();
                    return true;
                }
                catch (Exception e)
                {
                    Prox_Ejec();
                    return false;
                }
            }
        }
        private void Prox_Ejec()
        {
            string nombreApp = (Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)).Replace("Config.exe", "");
            ScheduledTasks Tareas = new ScheduledTasks();
            TaskScheduler.Task tarea = Tareas.OpenTask(nombreApp);
            if (chkInicioW.Checked && tarea != null)
            {
                string proxima = tarea.NextRunTime.ToString();
                label18.Visible = true;
                label18.Text = ("Proxima ejecucion: " + proxima);
                chkEdit.Visible = true;
                TimePicker.Value = Convert.ToDateTime(proxima);
                tarea.Close();

                Dehab();
            }
            else
            {
                label18.Visible = false;
                label18.Text = "";
                chkEdit.Visible = false;
                chkInicioW.Checked = false;
            }
            chkEdit.Checked = false;
            Tareas.Dispose();
        }
        private void Dehab()
        {
            txtUserWind.Enabled = false;
            txtPassWind.Enabled = false;
            txtRutaRead.Enabled = false;
            btnCambiar.Enabled = false;
            TimePicker.Enabled = false;
            txtUserWind.Text = "";
            txtPassWind.Text = "";
            txtRutaRead.Text = "";
            label14.Enabled = false;
            label15.Enabled = false;
            label16.Enabled = false;
            label17.Enabled = false;
        }
        private void Hab()
        {
            string path = (Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Remove(0, 6) + "\\" + Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)).Replace("Config", "");
            txtUserWind.Enabled = true;
            txtPassWind.Enabled = true;
            txtRutaRead.Enabled = true;
            btnCambiar.Enabled = true;
            TimePicker.Enabled = true;
            txtRutaRead.Text = path;
            txtUserWind.Text = Environment.UserDomainName + "\\" + Environment.UserName;
            label14.Enabled = true;
            label15.Enabled = true;
            label16.Enabled = true;
            label17.Enabled = true;
        }
        private void txtPuerProxy_KeyPress(object sender, KeyPressEventArgs e)
        {
            SoloNum(ref e);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            Guardar();
        }

        private void txtMandante_KeyPress(object sender, KeyPressEventArgs e)
        {
            SoloNum(ref e);
        }

        private void btnProveedor_Click(object sender, EventArgs e)
        {
            this.txtProveedor.Text = examinarRuta(txtProveedor.Text);
        }

        private void btnCliente_Click(object sender, EventArgs e)
        {
            this.txtCliente.Text = examinarRuta(txtCliente.Text);
        }

        private void btnNomina_Click(object sender, EventArgs e)
        {
            this.txtNomina.Text = examinarRuta(txtNomina.Text);
        }

        private void btnLog_Click(object sender, EventArgs e)
        {
            this.txtLog.Text = examinarRuta(txtLog.Text);
        }

        private void btnProveedorC_Click(object sender, EventArgs e)
        {
            this.txtProveedorC.Text = examinarRuta(txtProveedorC.Text);
        }

        private void btnClienteC_Click(object sender, EventArgs e)
        {
            this.txtClienteC.Text = examinarRuta(txtClienteC.Text);
        }

        private void btnNominaC_Click(object sender, EventArgs e)
        {
            this.txtNominaC.Text = examinarRuta(txtNominaC.Text);
        }

        private void btnProveedorI_Click(object sender, EventArgs e)
        {
            this.txtProveedorI.Text = examinarRuta(txtProveedorI.Text);
        }

        private void btnClienteI_Click(object sender, EventArgs e)
        {
            this.txtClienteI.Text = examinarRuta(txtClienteI.Text);
        }

        private void btnNominaI_Click(object sender, EventArgs e)
        {
            this.txtNominaI.Text = examinarRuta(txtNominaI.Text);
        }

        private void txtInstancia_KeyPress(object sender, KeyPressEventArgs e)
        {
            SoloNum(ref e);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkInicioW_CheckedChanged(object sender, EventArgs e)
        {
            HabilitadosGeneral();
        }

        private void btnCambiar_Click(object sender, EventArgs e)
        {
            this.txtRutaRead.Text = Ruta(txtRutaRead.Text);
        }

        private void chkProxy_CheckedChanged(object sender, EventArgs e)
        {
            HabilitadosProxi();
        }

        private void chkEdit_CheckedChanged(object sender, EventArgs e)
        {
            HabilitadosGeneral();
        }
    }
}
