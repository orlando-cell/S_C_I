using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NAudio.Wave;

namespace S_C_I
{
    public class Sensores
    {
        private string[] pisos = { "Piso 1", "Piso 2", "Piso 3" };
        private string[][] sectores = new string[][] //genera 2 sectores en 1 piso
        {
            new string[] { "Sector A", "Sector B" },
            new string[] { "Sector C", "Sector D" },
            new string[] { "Sector E", "Sector F" }
        };

        private int[,] temps = new int[3, 2];
        private int[,] humos = new int[3, 2];
        private string[,] estados = new string[3, 2];

        private Historial historial; //h. dentro de Clas sensores
        
        public Sensores(Historial historial)
        {
            this.historial = historial; //Guarda los eventos generados y lo muestra en bloc..
        }

        public void IniciarMonitoreo()
        {
            Random rand = new Random();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Monitoreo activo. Presione cualquier tecla para detener...");
            Thread.Sleep(2000);

            while (!Console.KeyAvailable)
            {
                bool HayAlarma = false;
                int PisoEvento = -1, SectorEvento = -1; 

                for (int p = 0; p < 3;  p++)
                {
                    for(int s = 0; s < 2; s++)
                    {
                        int prob = rand.Next(1, 101);
                        if(prob <= 98) //La probabilidad de incenido es del 2%.
                        {
                            estados[p, s] = "NORMAL";
                            temps[p, s] = rand.Next(15, 50);
                            humos[p, s] = rand.Next(0, 20);
                        }
                        else
                        {
                            estados[p, s] = "ALERTA";
                            temps[p, s] = rand.Next(50, 101);
                            humos[p, s] = rand.Next(20, 91);
                            HayAlarma = true;
                            PisoEvento = p;
                            SectorEvento = s;
                        }
                    }
                }
                MostrarTabla();

                if (!HayAlarma) //No
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("╔═══════════════════════════╦═══════════════════════════════╗");
                    Console.WriteLine("║  ALARMA: INACTIVA         ║  ROCIADORES: APAGADOS         ║");
                    Console.WriteLine("║  PUERTAS: CERRADAS        ║  ASCENSORES: OK               ║");
                    Console.WriteLine("╚═══════════════════════════╩═══════════════════════════════╝");
                    Console.Beep(400, 500);

                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
                    Console.WriteLine("║           🚨 ALARMA - PROTOCOLO INICIADO                  ║");
                    Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
                    Thread.Sleep(2000);
                    ProtocoloEmergencia(PisoEvento, SectorEvento);
                    return;
                }
                Thread.Sleep(2000);

                int TemMin = 15, TemMax = 49;
                int HumoMin = 0, HumoMax = 19;

                historial.AgregarEvento(
                    $"MONITOREO DETENIDO ▓ " +
                    $"Sin eventos de incendio ▓ " +
                    $"Temperaturas estables entre {TemMin}°C - {TemMax}°C ▓ " +
                    $"Humo estable entre {HumoMin}% - {HumoMax}%"
                    );

            }
            Console.ReadKey();
        }
        private void MostrarTabla()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("╔══════════════════╦══════════════╦══════════╦══════════════╗");
            Console.WriteLine("║  SENSOR          ║  TEMPERATURA ║   HUMO   ║    ESTADO    ║");
            Console.WriteLine("╠══════════════════╩══════════════╩══════════╩══════════════╣");
            string hora = DateTime.Now.ToString("HH:mm:ss");
            Console.WriteLine($"║  Hora: {hora,-10}  MONITOREO EN VIVO                      ║");
            Console.WriteLine("╠══════════════════╦══════════════╦══════════╦══════════════╣");

            for (int p = 2; p >= 0; p--)
            {
                for (int s = 0; s < 2; s++)
                {
                    Console.ForegroundColor = estados[p, s] == "NORMAL"
                        ? ConsoleColor.Green : ConsoleColor.Red;

                    string ind = estados[p, s] == "NORMAL" ? "🟢" : "🔴";//Verde/Rojo
                    string sensor = $"{ind} {pisos[p]} {sectores[p][s]}";
                    string tempCol = $"  {temps[p, s],3}°C     ";
                    string humoCol = $"  {humos[p, s],2}%    ";
                    string estadoCol = $"  {estados[p, s],-10}  ";

                    Console.WriteLine($"║{sensor,-11}║{tempCol,-13} ║{humoCol,-10}║{estadoCol,-10}║");
                }
                Console.ForegroundColor = ConsoleColor.Green;
                if (p > 0)
                    Console.WriteLine("╠══════════════════╬══════════════╬══════════╬══════════════╣");
            }
            Console.WriteLine("╚══════════════════╩══════════════╩══════════╩══════════════╝");
        }

        private void ProtocoloEmergencia(int pe, int se)
        {
            historial.AgregarEvento($"ALARMA ▓ {pisos[pe]} - {sectores[pe][se]} ▓ Temp: {temps[pe, se]}°C ▓ Humo: {humos[pe, se]}%");

            AudioFileReader audio = new AudioFileReader("alarma.mp3");
            WaveOutEvent reproductor = new WaveOutEvent(); //Reproduce el sonido
            reproductor.Init(audio); //Conecta el archivo al dispositivo
            reproductor.Play();

            // Sincronizado con el audio
            for (int ciclo = 0; ciclo < 8.6; ciclo++) //a mayor siclos mayor duración
            {

                //Parpadeo sincronizado con el sonido
                for (int golpe = 0; golpe < 5; golpe++) //De 3 a seis golpes
                {
                    Console.BackgroundColor = golpe % 2 == 0 ? ConsoleColor.Red : ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Clear();
                    Console.WriteLine("══════════════════════════════════════════════");
                    Console.WriteLine("          🚨 ALERTA DE INCENDIO 🚨            ");
                    Console.WriteLine("           EVACUACIÓN INMEDIATA               ");
                    Console.WriteLine("══════════════════════════════════════════════");
                    Thread.Sleep(58); //Duración de cada golpe 58 mili.
                }
                // Pausa entre grupos (el silencio de 1 segundo)
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Clear();
                Console.WriteLine("══════════════════════════════════════════════");
                Console.WriteLine("          🚨 ALERTA DE INCENDIO 🚨            ");
                Console.WriteLine("           EVACUACIÓN INMEDIATA               ");
                Console.WriteLine("══════════════════════════════════════════════");
                Thread.Sleep(1110); // pausa 
            }

            //Detener audio
            reproductor.Stop();
            reproductor.Dispose(); //libera dispositivo
            audio.Dispose();       //   ||   sonido


            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Clear();
            Console.WriteLine("══════════════════════════════════════════════");
            Console.WriteLine("        PROTOCOLO DE CONTROL INICIADO         ");
            Console.WriteLine("══════════════════════════════════════════════");
            Console.WriteLine($"  {estados[pe, se]} en {pisos[pe]} - {sectores[pe][se]}");
            Console.WriteLine($"  Temps: {temps[pe, se]}°C ▒ Humo: {humos[pe, se]}%");
            Console.WriteLine("══════════════════════════════════════════════");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  ➜  ROCIADORES:         ACTIVADOS");
            Console.WriteLine("  ➜  EXTRACTORES:        ENCENDIDOS");
            Console.WriteLine("  ➜  PUERTAS DE EMERGENCIA: ABIERTAS");
            Console.WriteLine("  ➜  ASCENSORES:         DESACTIVADOS");
            Console.WriteLine("  ➜  LUCES ESTROBOSCÓP:  ACTIVAS");
            Console.WriteLine("══════════════════════════════════════════════");
            Thread.Sleep(6000);

            int TemActual = temps[pe, se];
            int HumoActual = humos[pe, se];

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n  INICIANDO ROCIADORES Y EXTRACTORES...\n");
            Thread.Sleep(1500);

            //Agregamos controlando.mp3

            AudioFileReader audioControlando = new AudioFileReader("controlando.mp3"); //Abre
            WaveOutEvent reproductorControlando = new WaveOutEvent(); //Crea 
            reproductorControlando.Init(audioControlando);  //Conecta
            reproductorControlando.Play(); //Suena


                do
                {
                    int Bajadatem = TemActual > 70 ? 20 : TemActual > 40 ? 10 : 5;
                    int BajadaHum = HumoActual > 50 ? 15 : HumoActual > 20 ? 8 : 3;

                    TemActual -= Bajadatem; if (TemActual < 20) TemActual = 20;
                    HumoActual -= BajadaHum; if (HumoActual < 1) HumoActual = 1;

                    //Parpadeo suave cuando baja la temperatura
                    for(int i =0; i < 4; i++)
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.Clear();
                        Console.BackgroundColor = i % 2 == 0 ? ConsoleColor.DarkCyan : ConsoleColor.Black;
                        Console.WriteLine("══════════════════════════════════════════════");
                        Console.WriteLine("      💧 ROCIADORES ACTIVOS - CONTROLANDO     ");
                        Console.WriteLine("══════════════════════════════════════════════");
                        Console.WriteLine($"  Temp actual : {TemActual}°C");
                        Console.WriteLine($"  Humo actual : {HumoActual}%");
                        Console.WriteLine("══════════════════════════════════════════════");
                        Thread.Sleep(80); // parpadeo suave cada 80ms
                    }

                    Console.BackgroundColor = ConsoleColor.Black;
                    Thread.Sleep(1300);

                

                } while (TemActual > 20 || HumoActual > 1);


                //Detiene el audio al terminar
                reproductorControlando.Stop();
                reproductorControlando.Dispose();
                audioControlando.Dispose();


                AudioFileReader audioControlado = new AudioFileReader("controlado.mp3");
                WaveOutEvent reproductorControlado = new WaveOutEvent();
                reproductorControlado.Init(audioControlado);
                reproductorControlado.Play();

                //Espera 7 segundo antes de mostrar  controlado
                for (int i = 0; i < 6; i++)
                {

                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Clear();
                    Console.ForegroundColor = i % 2 == 0 ? ConsoleColor.Green : ConsoleColor.White;
                    Console.WriteLine("══════════════════════════════════════════════");
                    Console.WriteLine("         VERIFICANDO SISTEMA...               ");
                    Console.WriteLine("══════════════════════════════════════════════");
                    Thread.Sleep(1000); // 1 Segundos

                }
      
                historial.AgregarEvento($"CONTROLADO ▓ {pisos[pe]} - {sectores[pe][se]} ▒ Temp final: {TemActual}°C ▓ Humo final: {HumoActual}%");//Segundo evento
                //Muestra el incendio controlado
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("══════════════════════════════════════════════");
                Console.WriteLine("             INCENDIO CONTROLADO              ");
                Console.WriteLine("══════════════════════════════════════════════");
                Console.WriteLine($"  Ubicación : {pisos[pe]} - {sectores[pe][se]}");
                Console.WriteLine($"  tem Final : {TemActual}°C");
                Console.WriteLine($"  Humo Final : {HumoActual}%");
                Console.WriteLine("══════════════════════════════════════════════");

                //Detiene el audio
                reproductorControlado.Stop();
                reproductorControlado.Dispose();
                audioControlado.Dispose();

                Console.WriteLine($"\n Presione cualquier tecla para volver al Menú...");
                Console.ReadKey();
        }
    }
}
