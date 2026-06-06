using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S_C_I
{
   public class Historial
    {

        private List<string> eventos = new List<string>(); //Guarda todos los eventos
        
        public void AgregarEvento(string evento)
        {
            eventos.Add($"[{DateTime.Now:dd/MM/yyyy HH:mm:ss}] {evento}");
        }
        public void MostrarHistorial()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║            Historial de Eventos              ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");

            if (eventos.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("║     Sin eventos registrados.                 ║");
                Console.WriteLine("╚══════════════════════════════════════════════╝");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                foreach (string evento in eventos)
                {
                    string truncado = evento.Length > 44 ? evento.Substring(0, 41) + "..." : evento;
                    Console.WriteLine($"║  {truncado,-44}║");
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("╚══════════════════════════════════════════════╝");

                //Guarda en bloc de notas
                string nombreArchivo = "HISTORIAL_SCI.txt";
                System.IO.File.WriteAllLines(nombreArchivo, eventos);
                System.Diagnostics.Process.Start("notepad.exe", nombreArchivo); //Abre bloc
            }

            Console.WriteLine("\nPresione cualquier tecla para regresar...");
            Console.ReadKey();  
            
        }
        public void GenerarInforme()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║       INFORME DE EVENTOS RECIENTES           ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");

            if (eventos.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("║     Sin eventos registrados.                 ║");
                Console.WriteLine("╚══════════════════════════════════════════════╝");
            }
            else
            {
                
                List<string> informe = new List<string>(); //Nueva lista
                informe.Add("═══════════════════════════════════════════");
                informe.Add("     SISTEMA CONTRA INCENDIO - SCI        ");
                informe.Add("     INFORME DE EVENTOS RECIENTES         ");
                informe.Add($"     Generado: {DateTime.Now:dd/MM/yyyy HH:mm:ss}");
                informe.Add("═══════════════════════════════════════════");
                informe.Add("");

                int contAlarmas = 0;
                int contControlado = 0;

                foreach (string evento in eventos)
                {
                    informe.Add(evento);
                    if (evento.Contains("ALARMA")) contAlarmas++;
                    if (evento.Contains("CONTROLADO")) contControlado++;
                }

                informe.Add("");
                informe.Add("═══════════════════════════════════════════");
                informe.Add("           RESUMEN FINAL                   ");
                informe.Add("═══════════════════════════════════════════");
                string FechaCompleta = DateTime.Now.ToString("dddd dd 'de' MMMM 'del' yyyy 'a las' HH:mm:ss");
                informe.Add($" Total de alarmas detectadas  : {contAlarmas}");
                informe.Add($" Total de incendios controlados : {contControlado}");
                informe.Add($" Total de eventos registrados : {eventos.Count}");
                informe.Add("");
                informe.Add($" Informe generado el : {FechaCompleta}");
                informe.Add("═══════════════════════════════════════════");

                string nombreArchivo = "INFORME_SCI.txt";
                System.IO.File.WriteAllLines(nombreArchivo, informe);
                System.Diagnostics.Process.Start("notepad.exe", nombreArchivo);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("║  Informe generado correctamente.             ║");
                Console.WriteLine("║  Archivo: INFORME_SCI.txt                    ║");
                Console.WriteLine("╚══════════════════════════════════════════════╝");
            }

            Console.WriteLine("\nPresione cualquier tecla para volver...");
            Console.ReadKey();
        }
    }
}
