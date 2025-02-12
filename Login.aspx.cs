using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication50.Models;
using WebApplication50.CONTROLLER;
using WebApplication50.VIEWS;
using System.Text;
using System.Security.Cryptography;

namespace WebApplication50.VIEWS
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string username = Username.Text.Trim();
            string password = Password.Text.Trim();

            if (ValidarCredenciales(username, password))
            {
                Response.Redirect("Administrador.aspx");
            }
            else
            {
                Response.Write("<script>alert('Usuario o contraseña incorrecta');</script>");
            }
        }
        private bool ValidarCredenciales(string username, string password)
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["tiendas_de_librosConnectionString"].ConnectionString;

                using (MySqlConnection conn = new MySqlConnection(connString))
                {
                    conn.Open();

                    // Encriptar la contraseña ingresada con SHA2
                    string hashedPassword = EncryptPassword(password); // Método que definiremos abajo

                    string query = "SELECT COUNT(1) FROM Usuarios WHERE nombre = @username AND contrasena = @password";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", hashedPassword);

                        // Retorna true si el usuario existe
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                Response.Write("<script>alert('Error al validar credenciales: " + ex.Message + "');</script>");
                return false;
            }
        }

        private string EncryptPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2")); // Convertir cada byte a hexadecimal
                }
                return sb.ToString();
            }
        }

    }
}