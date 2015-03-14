using Civ;
using Global;
using System;
using System.Threading;

namespace CivConsole
{
    static class MainClass
    {

		/// <summary>
		/// Acciones en civilización.
		/// </summary>
		static System.Collections.Generic.Dictionary<string, Action<Civilizacion, string[]>> AccionesCiv = new System.Collections.Generic.Dictionary<string, Action<Civilizacion, string[]>>();
		/// <summary>
		/// Acciones en edificio.
		/// </summary>
		static System.Collections.Generic.Dictionary<string, Action<Ciudad, string[]>> AccionesCiudad = new System.Collections.Generic.Dictionary<string, Action<Ciudad, string[]>>();
		/// <summary>
		/// Acciones globales.
		/// </summary>
		static System.Collections.Generic.Dictionary<string, Action<string[]>> Acciones = new System.Collections.Generic.Dictionary<string, Action<string[]>>();
		static DateTime timer = DateTime.Now;
		const float MultiplicadorVelocidad = 360;
        static Civilizacion MyCiv;
		static object Obj;

        public static void Main(string[] args)
        {
			// Inicializar la consola
			Acciones.Add("ch", CambiarObj);
			Acciones.Add("", MostrarObjeto);

			AccionesCiv.Add("ciencias", MostrarCiencias);
			AccionesCiv.Add("inv", MostrarAvanzando);
			AccionesCiv.Add("ciudades", MostrarCiudades);

			AccionesCiudad.Add("edificios", MostrarEdificios);

			// Inicializar el juego.
            g_.CargaData();
            g_.InicializarJuego();


            MyCiv = g_.State.Civs[0];
			MyCiv.OnNuevoMensaje += MuestraMensajes;
			MyCiv.getCiudades[0].AlimentoAlmacén = 10;
			Obj = MyCiv;

			Thread emu = new Thread(new ThreadStart(Ticker));

			emu.Start();

            string input;
            while (true)
            {
				// input = Consola.Iterando();
                input = Console.ReadLine().ToLower();
				string[] inp = input.Split(' ');
				// globales
				if (inp != null) foreach (var x in Acciones)
					{
						if (x.Key == inp[0]) x.Value.Invoke(inp);
					}				
				if (Obj is Civilizacion)
					ObjCiv(inp);
				if (Obj is Ciudad)
					ObjCiudad(inp);
			}            
        }

		/// <summary>
		/// Cuando el objeto es una civilización.
		/// </summary>
		/// <param name="input"></param>
		static void ObjCiv(string[] inp)
		{		
			
			Civilizacion C = (Civilizacion)Obj;
			if (inp != null) foreach (var x in AccionesCiv)
				{
					if (x.Key == inp[0]) x.Value.Invoke(C, inp);
				}
		}

		/// <summary>
		/// Cuando el objeto es una ciudad.
		/// </summary>
		/// <param name="input"></param>
		static void ObjCiudad(string[] inp)
		{

			Ciudad C = (Ciudad)Obj;
			if (inp != null) foreach (var x in AccionesCiudad)
				{
					if (x.Key == inp[0]) x.Value.Invoke(C, inp);
				}
		}


		static void MostrarCiencias (Civilizacion C, string[] inp)
		{
			Console.WriteLine("Investigadas: ");
			foreach (var x in C.Avances)
			{
				Console.WriteLine(x.Nombre);
			}
		}

		static void MostrarAvanzando(Civilizacion C, string[] inp)
		{
			Console.WriteLine("Investigando: ");
			foreach (var x in C.Investigando)
			{
				Console.WriteLine(string.Format("{0}: {1:p}", x.Ciencia.Nombre, x.ObtPct()));
			}
		}

		static void MostrarCiudades (Civilizacion C, string[] inp)
		{
			Console.WriteLine("Ciudades: ");
			foreach (var x in C.getCiudades)
			{
				Console.WriteLine(x.Nombre);
			}
		}

		static void MostrarEdificios(Ciudad C, string[] inp)
		{
			Console.WriteLine("Edificios: ");
			foreach (var x in C.Edificios)
			{
				Console.WriteLine(x.Nombre);
			}
		}
		

		static void CambiarObj (string[] inp)
		{
			if (inp[1] == "ciudad")
				Obj = MyCiv.getCiudades.Find(x => x.Nombre.ToLower() == inp[2]);
		}

		static void MostrarObjeto (string[] inp)
		{
			Console.WriteLine(string.Format("{0}: {1}", Obj.GetType().ToString(), Obj.ToString()));
		}

		public static void Ticker()
		{
			while (true)
			{
				TimeSpan tiempo = DateTime.Now - timer;
				timer = DateTime.Now;
				float t = (float)tiempo.TotalHours * MultiplicadorVelocidad;

				// Console.WriteLine(t);
				Global.g_.Tick(t);

				if (Global.g_.State.Civs.Count == 0)
					throw new Exception("Ya se acabó el juego :3");
			}
		}
		static void MuestraMensajes()
		{
			while (MyCiv.ExisteMensaje)
			{
				IU.Mensaje m = MyCiv.SiguitenteMensaje();
				if (m != null)
				{
					Console.WriteLine(m.ToString());
					//MessageBox.Show(m.ToString());
					//listMensajes.Items.Add(m.ToString());
				}
			}
		}
    }

	
}
