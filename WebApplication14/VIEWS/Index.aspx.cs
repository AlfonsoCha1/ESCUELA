using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication14.Controller;

namespace WebApplication14.VIEWS
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGridView();
            }
        }

        private void BindGridView()
        {
            // Crear una instancia de la clase Persona
            Persona persona = new Persona();
            List<int> numerosAleatorios = persona.GenerarNumerosAleatorios();

            // Tomar solo los primeros 10 números aleatorios para el GridView
            List<int> primerosDiezNumeros = numerosAleatorios.GetRange(0, 10);

            // Crear una tabla de datos para enlazar con el GridView
            DataTable dt = new DataTable();
            dt.Columns.Add("Número Aleatorio");

            // Llenar el DataTable con los números aleatorios
            foreach (int numero in primerosDiezNumeros)
            {
                DataRow row = dt.NewRow();
                row["Número Aleatorio"] = numero;
                dt.Rows.Add(row);
            }

            // Enlazar el DataTable al GridView
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
    }
}