using System;
using System.Threading;

namespace MotorApp
{
    class Program
    {
        static int direccion = 0; // Variable para determinar la dirección en la que gira el abanico/motor
        static int frame = 0;
        static bool salir = false; // Variable para controlar si se debe salir del ciclo de animación
        static ManualResetEvent pausaAnimacion = new ManualResetEvent(true); // Objeto para pausar y reanudar la animación

        static void Main(string[] args)
        {
            Thread animationThread = new Thread(MostrarAnimacion); // Thread para manejar la animación

            int opcion;
            bool start = true;
            do
            {
                Console.Clear();
                MostrarMenu(); // Mostrar el menú inicial
                
                if(start){
                    animationThread.Start(); // Inicia el hilo de la animación después de configurar todo
                    start = false;
                }
                pausaAnimacion.Set(); //Reanudar la animación

                Console.Write("Opción: ");
                opcion = LeerOpcion();

                switch (opcion)
                {
                    case 1:
                        direccion = 0; // Establece la dirección en 0 cuando se pausa el motor
                        break;
                    case 2:
                        direccion = 1; // Establece la dirección en 1 cuando el motor gira a la derecha
                        break;
                    case 3:
                        direccion = -1; // Establece la dirección en -1 cuando el motor gira a la izquierda
                        break;
                    case 4:
                        Console.WriteLine("Saliendo del programa...");
                        Thread.Sleep(1000);
                        salir = true; // Configura la variable de salida en true
                        break;
                    default:
                        Console.WriteLine("Opción no válida.");
                        break;
                }
                pausaAnimacion.Reset(); //Pausar la animacion
            } while (!salir); // Repetir hasta que se elija la opción de salir

            // Espera a que el hilo de la animación termine antes de salir del programa
            animationThread.Join();

            Console.Clear();
        }

        static void MostrarMenu()
        {
            Console.WriteLine("1. Pausar el motor");
            Console.WriteLine("2. Motor gira a la derecha");
            Console.WriteLine("3. Motor gira a la izquierda");
            Console.WriteLine("4. Salir");
        }

        static int LeerOpcion()
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (int.TryParse(input, out int opcion))
                {
                    return opcion;
                }
                else
                {
                    Console.SetCursorPosition(0, 4); 
                    Console.Write(new string(' ', Console.WindowWidth)); // Escribir espacios en blanco en toda la fila
                    Console.SetCursorPosition(0, 4); 
                    Console.WriteLine("Por favor, ingrese un número entero válido.");
                    Console.Write("Opción: ");
                }
            }
        }

        static void MostrarAnimacion()
        {
            while (!salir) // Continuar mientras no se elija la opción de salir
            {
                pausaAnimacion.WaitOne(); // Esperar a que se levante la pausa

                if (frame == 4)
                {
                    frame = 0;
                }
                else if (frame == -1)
                {
                    frame = 3;
                }

                // Guardar la posición actual del cursor
                int cursorLeft = Console.CursorLeft;
                int cursorTop = Console.CursorTop;

                // Cambia la imagen del abanico según la dirección
                Console.SetCursorPosition(0, 5); // Establecer la posición de la animación
                Console.WriteLine(MotorAnimations.Abanico[frame]);

                // Restaurar la posición del cursor
                Console.SetCursorPosition(cursorLeft, cursorTop);

                Thread.Sleep(250); // Retraso de 0.25 segundos para cambiar la imagen cada 0.25 segundos
                frame += direccion;
            }
        }
    }
}
