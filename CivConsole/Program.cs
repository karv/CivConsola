using System;
using Civ;
using Global;
using System.Threading;

namespace CivConsole
{
	class MainClass
	{

		static Civilizacion MyCiv;
		static DateTime LastUpdate;

		public static void Main(string[] args)
		{
			g_.CargaData();

			g_.InicializarJuego();
#if DEBUG
			CivLibrary.Debug.Debug.CrearArchivoObjetosAbiertos();
#endif


#if HACERARBOL
			System.Environment.Exit(0);
#endif


			MyCiv = g_.State.Civs[0];
			/*
			Armada a = new Armada();
			a.AgregaUnidad(new Unidad(U));
			MyCiv.Armadas.Add(a);
			Armada b = new Armada();
			b.AgregaUnidad(new Unidad(U));
			g_.State.Civs[1].Armadas.Add(b);

			a.Pelea(b, 0.01f);
            */

			Thread Tk = new Thread(new ThreadStart(Ticker));
			string input;
			LastUpdate = DateTime.Now;
			Tk.Start();
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

		static void Ticker()
		{
			DateTime N = DateTime.Now;
			TimeSpan t = N - LastUpdate;
			LastUpdate = N;

			g_.Tick((float)t.TotalHours);
		}
	}
}
