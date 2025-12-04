using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;

namespace WebApplication14.Controller
{
    public class Persona
    {
        public List<int> GenerarNumerosAleatorios()
        {
            List<int> numerosAleatorios = new List<int>();
            Random random = new Random();

            for (int i = 0; i < 100; i++)
            {
                numerosAleatorios.Add(random.Next(1, 100)); // Genera un número entre 1 y 100 (incluido)
            }

            return numerosAleatorios;
        }
    }
}