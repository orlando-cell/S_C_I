using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace S_C_I
{
    public class PanelCentral
    {
        private string EstadoPanel = "NORMAL";
        private int Monitoreos = 0 ;
        private Historial historial; //Guarda eventos
        private Sensores sensores; //Activa protocolos

        public PanelCentral()
        {
            historial  = new Historial();
            sensores = new Sensores(historial);

        }
        public void Iniciar()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8; //Permite agregar caracteres especiales
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;

            int opcion;
            do
            {//
                
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("╔══════════════════════════════════════════════╗");
                Console.WriteLine("║       SISTEMA CONTRA INCENDIO - SCI          ║");
                Console.WriteLine("╠══════════════════════════════════════════════╣");
                Console.WriteLine("║              PANEL CENTRAL                   ║");
                Console.WriteLine("╠══════════════════════════════════════════════╣");
                string fechaHora = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                Console.WriteLine($"║  Fecha y Hora: {fechaHora,-30}║");
                Console.WriteLine($"║  Estado Panel: {EstadoPanel,-30}║");
                Console.WriteLine($"║  Monitoreso Realizados: {Monitoreos, -21}║"); //Agregamos nuevos monitoreos
                Console.WriteLine("╠══════════════════════════════════════════════╣");
                Console.WriteLine("║                                              ║");
                Console.WriteLine("║       1. Monitoreo de Sensores               ║");
                Console.WriteLine("║       2. Historial de Eventos                ║");
                Console.WriteLine("║       3. Generar Informe                     ║");
                Console.WriteLine("║       0. Salir                               ║");
                Console.WriteLine("║                                              ║");
                Console.WriteLine("╚══════════════════════════════════════════════╝");

                Console.Write("\nSeleccione una opción: ");

                if (!int.TryParse(Console.ReadLine(), out opcion))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ingrese un número válido.");
                    Thread.Sleep(800);
                    opcion = -1;
                    continue;

                }
                switch (opcion)
                {
                    case 1: Monitoreos++; sensores.IniciarMonitoreo(); break;
                    case 2: historial.MostrarHistorial(); break;
                    case 3: historial.GenerarInforme(); break;
                    case 0: Salir(); break;
                    default: Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Opción no válida.");
                        Thread.Sleep(800);
                        break;
                }
            } while (opcion != 0);

        }
        private void Salir()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("══════════════════════════════════════════════");
            Console.WriteLine("          Saliendo del sistema...             ");
            Console.WriteLine("══════════════════════════════════════════════");
            Thread.Sleep(1000);
        }
    }
}
