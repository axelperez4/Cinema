using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaTraining
{
    class Program
    {
        static void Main(string[] args)
        {
            //Un comentario

            //Declaración de variables
            string miString = "hola";
            var miString2 = 2;
            int numero = 2;
            DateTime fecha = DateTime.Now;
            bool bollean = true;
            List<Persona> personas = new List<Persona>();

            //Objetos
            Persona persona1 = new Persona("Rodrigo");
            Persona persona2 = new Persona();
            Console.Out.WriteLine(persona1.Nombre);
            Console.Out.WriteLine(persona2.Nombre);

            //Ciclos
            for (int i = 0; i < personas.Count; i++)
            {
                var nombre = personas[i].Nombre;
                Console.Out.WriteLine(i);
            }

            foreach (var persona in personas)
            {
                var nombre = persona.Nombre;
            }

            while (determinarSiSi())
            {

            }

            //Condicional
            if (
                determinarSiSi()  //Metodo devuelve booleano
                && 10 == 10  //And = &&      Igualdad ==  
                || 7 != 10   //OR = ||       Diferente !=
                )
            {
            }
            else if (false)
            {
            }
            else
            {
            }

            //Operaciones algebraicas
            int a = 1;
            int b = 5;
            int suma = a + b;
            int multiplicacion = a * b;
            int division = b / a;
            int resta = a - b;
            decimal divisionDecimal = b / a;

            int total = Sumar(a, b);
            Console.Out.WriteLine(total);


            //Metodos de extension
            string texto = "hola    ";
            var textoSinEspacios = texto.Trim(); //"hola"
            texto.ToUpper(); //HOLA
            texto.ToLower(); //hola
            texto.StartsWith("h"); //true




            var ingreso = Console.In.ReadLine();

        }

        //Metodo
        //Metodo devuelve booleano
        static public bool determinarSiSi()
        {
            return false;
        }

        //Metodo que no devuelve nada
        static public void NoDevulevoNada()
        {

        }

        //Metodo suma
        static public int Sumar(int a, int b)
        {
            var total = a + b;

            return total;
        }
    }

    public class Persona
    {
        //Constructor
        public Persona(string nombre)
        {
            Nombre = nombre;
        }

        //Constructor 2
        public Persona()
        {
        }

        public string Nombre { get; set; }
    }
}
