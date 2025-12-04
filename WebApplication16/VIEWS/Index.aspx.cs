using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication16.VIEWS;

namespace WebApplication16.VIEWS
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Calcular_Click(object sender, EventArgs e)
        {
            // Verifica que se haya seleccionado una fecha
            if (calendar1.SelectedDate != DateTime.MinValue)
            {
                DateTime fechaNacimiento = calendar1.SelectedDate;
                DateTime fechaActual = DateTime.Now;

                // Calcular la edad
                int edad = fechaActual.Year - fechaNacimiento.Year;

                // Ajuste si la persona aún no ha cumplido años este año
                if (fechaActual < fechaNacimiento.AddYears(edad))
                {
                    edad--;
                }

                // Mostrar el resultado
                Resultado.Text = $"La persona tiene {edad} años.";
            }
            else
            {
                Resultado.Text = "Por favor selecciona tu fecha de nacimiento.";
            }
        }
    }
}