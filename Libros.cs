using System;
using System.Collections.Generic;
using System.Data;  
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using iTextSharp.text;
using iTextSharp.text.pdf;
using WebApplication50.Models.TiendaLibrosTableAdapters;
using WebApplication50.CONTROLLER;

namespace WebApplication50.CONTROLLER
{
    public class Libros
    {

        public List<Libros> ObtenerLibros()
        {
            try
            {
                librosTableAdapter librosAdapter;
                using (librosAdapter = new librosTableAdapter())
                {
                    var dt = librosAdapter.GetData();  // Obtiene todos los Libros

                    if (dt.Rows.Count > 0)
                    {
                        List<Libros> listaLibros = new List<Libros>();

                        foreach (DataRow row in dt.Rows)
                        {
                            Libros libro = new Libros
                            {
                                ID_Libro = Convert.ToInt32(row["ID_Libro"]),  // Asegúrate de que este campo existe en tu base de datos
                                Titulo = row["Titulo"].ToString(),
                                Autor = row["Autor"].ToString(),
                                Editorial = row["Editorial"].ToString(),
                                Precio = Convert.ToDecimal(row["Precio"]),
                                Cantidad_Disponible = Convert.ToInt32(row["Cantidad_Disponible"]),
                                Genero = row["Genero"].ToString(),
                                Año_Publicacion = Convert.ToInt32(row["Año_Publicacion"])
                            };
                            listaLibros.Add(libro);
                        }


                        return listaLibros;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return new List<Libros>();
        }

        // eliminar  Libro por su ID
        public bool EliminarLibro(int idLibro)
        {
            try
            {
                using (librosTableAdapter librosAdapter = new librosTableAdapter())
                {
                    librosAdapter.DeleteLibro(idLibro);
                    return true; 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar el Libro: " + ex.Message);
                return false; // n error
            }
        }

        public bool ActualizarLibro(string titulo, string autor, decimal precio, string editorial,    string genero, int cantidadDisponible, int añoPublicacion, int idLibro)
        {
            try
            {
                using (librosTableAdapter librosAdapter = new librosTableAdapter())
                {
                    librosAdapter.UpdateLibro(titulo, autor, precio, editorial, genero,  cantidadDisponible, añoPublicacion, idLibro);
                    return true; 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar el Libro: " + ex.Message);
                return false; // error
            }
        }

        public Libros ObtenerLibroPorID(int ID_Libro)
        {
            try
            {
                using (librosTableAdapter librosAdapter = new librosTableAdapter())
                {
                    var dt = librosAdapter.GetDataByID(ID_Libro); 

                    if (dt.Rows.Count == 1)
                    {
                        DataRow row = dt.Rows[0]; //  primera  fila
                        Libros libro = new Libros
                        {
                            ID_Libro = Convert.ToInt32(row["id_libro"]),
                            Titulo = row["titulo"].ToString(),
                            Autor = row["autor"].ToString(),
                            Editorial = row["editorial"].ToString(),
                            Precio = Convert.ToDecimal(row["precio"]),
                            Cantidad_Disponible = Convert.ToInt32(row["cantidad_disponible"]),
                            Genero = row["genero"].ToString(),
                            Año_Publicacion = Convert.ToInt32(row["año_publicacion"])
                        };

                        return libro; 
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener el Libro: " + ex.Message);
            }
            return null; // no se encontró el libro 
        }



        // Constructor que recibe parámetros
        public Libros(string titulo, string autor, string editorial, decimal precio, int cantidad_disponible, string genero, int año_publicacion)
        {
            Titulo = titulo;
            Autor = autor;
            Editorial = editorial;
            Precio = precio;
            Cantidad_Disponible = cantidad_disponible;
            Genero = genero;
            Año_Publicacion = año_publicacion;
        }

        // Constructor sin parámetros
        public Libros()
        {
        }

        // Propiedades
        public int ID_Libro { get; set; }  // Asegúrate de tener una propiedad para el ID
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Editorial { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad_Disponible { get; set; }
        public string Genero { get; set; }
        public int Año_Publicacion { get; set; }


        public void RealizarCompra(int ID_Libro, int cantidad)
        {
          
            Libros libro = ObtenerLibroPorID(ID_Libro);
            if (libro == null)
            {
                Console.WriteLine("Libro no encontrado.");
                return;
            }

            decimal totalCompra = libro.Precio * cantidad;

          
            string nombreUsuarioActual = (string)System.Web.HttpContext.Current.Session["UsuarioActual"]; // Asegúrate de configurar esta sesión

            if (string.IsNullOrEmpty(nombreUsuarioActual))
            {
                Console.WriteLine("Usuario no logueado.");
                return;
            }

            // Obtener el correo electrónico del usuario
            string correoElectronico = ObtenerCorreoDeUsuario(nombreUsuarioActual);

            if (string.IsNullOrEmpty(correoElectronico))
            {
                Console.WriteLine("Correo electrónico del usuario no encontrado.");
                return;
            }

            // Generar el PDF del recibo de compra
            string archivoPdf = GenerarPdfRecibo(libro, cantidad, totalCompra);

            // Enviar el PDF por correo electrónico
            EnviarCorreo(correoElectronico, archivoPdf);

            Console.WriteLine("Compra realizada con éxito.");
        }


        // Nuevo método para obtener el correo electrónico del usuario
        /*
        private string ObtenerCorreoDeUsuario()
        {
            try
            {
                string correo = string.Empty;
                using (MySqlConnection conn = new MySqlConnection("server=127.0.0.1;user id=root;password=root;persistsecurityinfo=True;database=tiendas_de_libros"))
                {
                    conn.Open();
                    string query = "SELECT correo_electronico FROM usuarios WHERE nombre_usuario = 'cvillada'";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        correo = reader.GetString("correo_electronico");
                    }
                }
                return correo;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener el correo del usuario: " + ex.Message);
                return null;
            }
        }
        */

        private string ObtenerCorreoDeUsuario(string nombreUsuario)
        {
            try
            {
                string correo = string.Empty;
                using (MySqlConnection conn = new MySqlConnection("server=127.0.0.1;user id=root;password=root;persistsecurityinfo=True;database=tiendas_de_libros"))
                {
                    conn.Open();
                    string query = "SELECT correo FROM usuarios WHERE nombre = @nombreUsuario";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@nombreUsuario", nombreUsuario); // Uso de parámetro para evitar inyección SQL

                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        correo = reader.GetString("correo");
                    }
                }
                return correo;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener el correo del usuario: " + ex.Message);
                return null;
            }
        }

        // Nuevo método para generar el PDF del recibo
        private string GenerarPdfRecibo(Libros libro, int cantidad_disponible, decimal totalCompra)
        {
            string filePath = @"C:\path\to\save\recibo_compra.pdf"; // Cambia esta ruta según tu entorno

            Document document = new Document();
            PdfWriter.GetInstance(document, new System.IO.FileStream(filePath, System.IO.FileMode.Create));
            document.Open();

            // Contenido del PDF
            document.Add(new Paragraph("Recibo de Compra"));
            document.Add(new Paragraph($"Titulo: {libro.Titulo}"));
            document.Add(new Paragraph($"cantidad_disponible: {cantidad_disponible}"));
            document.Add(new Paragraph($"Precio: {libro.Precio:C}"));
            document.Add(new Paragraph($"Total: {totalCompra:C}"));
            document.Add(new Paragraph($"Fecha de compra: {DateTime.Now}"));

            document.Close();
            return filePath;
        }

        // Nuevo método para enviar el correo con el PDF adjunto
        private void EnviarCorreo(string correoDestino, string archivoAdjunto)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient("smtp.office365.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("112175@alumnouninter.com.mx", "Qop09074"),
                    EnableSsl = true
                };

                MailMessage mailMessage = new MailMessage
                {
                    From = new MailAddress("112175@alumnouninter.com.mx"),
                    Subject = "Recibo de Compra",
                    Body = "Gracias por tu compra. Adjunto te enviamos tu recibo.",
                    IsBodyHtml = true
                };
                mailMessage.To.Add(correoDestino);

                // Adjuntar el archivo PDF
                Attachment attachment = new Attachment(archivoAdjunto);
                mailMessage.Attachments.Add(attachment);

                smtpClient.Send(mailMessage);
                Console.WriteLine("Correo enviado con éxito.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al enviar el correo: " + ex.Message);
            }
        }
    }
}