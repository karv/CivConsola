using System;
using Civ;
using Global;

namespace CivConsole
{
    class MainClass
    {

        static Civilizacion MyCiv;

        public static void Main(string[] args)
        {
            g_.CargaData();
            g_.InicializarJuego();


            MyCiv = g_.State.Civs[0];

            string input;
            while (true)
            {
                input = Console.ReadLine().ToLower();
                switch (input)
                {
                    case "ciencias":
                        Console.WriteLine("Ciencias conocidas:");
                        foreach (var x in MyCiv.Avances)
                        {
                            Console.WriteLine(x);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
