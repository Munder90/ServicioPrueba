using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServicioPrueba
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var worker = new Thread(Iniciar);
            Log.CrearLog();
            worker.Name = "Prueba";
            worker.IsBackground = false;
            worker.Start();

        }

        void Iniciar()
        {
            Log l = new Log();
            int milisegundos = 15000;
            try
            {

                while (true)
                {
                    Thread.Sleep(milisegundos);
                    Log.MostrarMensajeConsola("TODO VA BIEN " + System.DateTime.Now);
                }
            }
            catch (Exception ex)
            {
                Log.MostrarMensajeConsola("error" + ex.ToString(), false);
            }
            finally
            {
                Thread.Sleep(milisegundos);
            }
        }

        protected override void OnStop()
        {
        }
    }
}
