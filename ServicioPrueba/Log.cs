using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharedSettings;

namespace ServicioPrueba
{
    class Log
    {
        static Settings settings = new Settings();
        static string direlog;
        public static void CrearLog()
        {
            settings = AppSettings.getConfig(settings);
            string dirFile = settings.directorioLog;
            DateTime thisDay = DateTime.Now;
            string nombre = "Recuperador" + thisDay.ToString();
            nombre = nombre.Replace(':', '-');
            nombre = nombre.Replace('\\', '-');
            nombre = nombre.Replace('/', '-');
            dirFile = dirFile + "\\" + nombre + ".txt";
            try
            {
                System.IO.File.WriteAllText(@dirFile, "Log de Servicio:");
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@dirFile, true))
                {
                    file.WriteLine(" ");
                    file.Close();
                }

                direlog = @dirFile;
            }
            catch (Exception)
            {
                //MostrarMensajeConsola("No fue posible crear el archivo de LOG de la carga de archivos");
                //MostrarMensajeConsola("Debe indicar un directorio donde pueda ser creado");
                //MostrarMensajeConsola("Pulse enter para salir");
                //Console.ReadLine();
                //Environment.Exit(0);
            }
        }

        public static void MostrarMensajeConsola(string mensaje, bool tipo = true)
        {
            try
            {
                int ancho = 250;
                //ancho = Console.BufferWidth;
                string inicioMensaje = "[] ", complementoMensaje = " []";
                if (!tipo)
                { inicioMensaje = "<< "; complementoMensaje = " >>"; }
                if (mensaje.Length >= ancho - (inicioMensaje.Length + complementoMensaje.Length))
                {
                    DividirMensaje(mensaje, ancho, complementoMensaje, inicioMensaje);
                }
                else
                {
                    MostrarMensajeFinal(inicioMensaje, mensaje, complementoMensaje, ancho);
                }
            }
            catch (Exception)
            {
                Console.WriteLine(mensaje);
            }
        }
        private static string[] DividirMensaje(string mensaje, int ancho, string complementoMensaje, string inicioMensaje)
        {
            string[] array = new string[2];
            int recortar = ancho - complementoMensaje.Length;
            if (mensaje.Contains('\n'))
            {
                string[] comp = mensaje.Split('\n');
                foreach (string mess in comp)
                {
                    MostrarMensajeFinal(inicioMensaje, mess, complementoMensaje, ancho);
                }
            }
            else
            {
                if (recortar > 0 && mensaje.Length > recortar)
                {
                    array[0] = mensaje.Substring(0, recortar);
                    array[1] = mensaje.Substring(recortar, (mensaje.Length - recortar));
                    MostrarMensajeFinal(inicioMensaje, array[0], complementoMensaje, ancho);
                    //Console.WriteLine(inicioMensaje + array[0] + complementoMensaje);
                    if (array[1].Length >= ancho - (inicioMensaje.Length + complementoMensaje.Length))
                    {
                        DividirMensaje(array[1], ancho, complementoMensaje, inicioMensaje);
                    }
                    else
                    {
                        MostrarMensajeFinal(inicioMensaje, array[1], complementoMensaje, ancho);
                    }
                }
                else
                {
                    array[0] = mensaje;
                    MostrarMensajeFinal(inicioMensaje, array[0], complementoMensaje, ancho);
                }
            }
            return array;
        }
        private static void MostrarMensajeFinal(string inicioMensaje, string mensaje, string complementoMensaje, int ancho)
        {
            string mensajeFinal = inicioMensaje + mensaje;
            string relleno = "";
            int tam = mensajeFinal.Length;
            int rellenar = ancho - (tam + complementoMensaje.Length);
            for (int i = 0; i < rellenar; i++)
            {
                relleno += " ";
            }
            string msgShow = inicioMensaje + mensaje + relleno + complementoMensaje;
            //Console.WriteLine(msgShow);
            //logs.Add(msgShow);
            string[] text = { msgShow };
            File.AppendAllLines(direlog, text);
        }
    }
}
