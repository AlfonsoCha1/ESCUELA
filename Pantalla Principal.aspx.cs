using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using WebApplication50.Models;
using WebApplication50.Models.TiendaLibrosTableAdapters;


namespace WebApplication50.VIEWS
{
    public partial class Pantalla_Principal : System.Web.UI.Page
    {
        // Tabla estática para simular un carrito (en caso de usar un backend real, esto sería temporal)
        private static DataTable carrito = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarLibros(); // Cargar productos al inicio

                // Inicializar carrito si no existe en sesión
                if (Session["Carrito"] == null)
                {
                    Session["Carrito"] = new List<Carritoses>();
                }
            }
        }

        // Función para agregar productos al carrito
        protected void btnAgregarLibro_Click(object sender, EventArgs e)
        {
            try
            {
                Button btnAgregarLibro = (Button)sender;
                GridViewRow row = (GridViewRow)btnAgregarLibro.NamingContainer;
                int Login = row.RowIndex;

                // Asegúrate de que las claves existen
                if (gvLibros.DataKeys[Login] != null)
                {
                    int idLibro = Convert.ToInt32(gvLibros.DataKeys[Login].Values["ID_Libro"]);
                    decimal precioLibro = Convert.ToDecimal(gvLibros.DataKeys[Login].Values["Precio"]);
                    string tituloLibro = row.Cells[0].Text;

                    List<Carritoses> carrito = (List<Carritoses>)Session["Carrito"];
                    var libroExistente = carrito.FirstOrDefault(p => p.ID_Libro == idLibro);

                    if (libroExistente != null)
                    {
                        libroExistente.Cantidad_Disponible++;
                    }
                    else
                    {
                        carrito.Add(new Carritoses
                        {
                            ID_Libro = idLibro,
                            Titulo = tituloLibro,
                            Precio = precioLibro,
                            Cantidad_Disponible = 1
                        });
                    }

                    Session["Carrito"] = carrito;
                    ActualizarCarrito();
                }
                else
                {
                    lblCarrito.Text = "No se pudo encontrar la información del libro.";
                }
            }
            catch (Exception ex)
            {
                lblCarrito.Text = "Error al agregar algun libro: " + ex.Message;
            }
        }

        private void ActualizarCarrito()
        {
            try
            {
                List<Carritoses> carrito = (List<Carritoses>)Session["Carrito"];
                gvCarrito.DataSource = carrito.Select(item => new
                {
                    item.Titulo,
                    item.Cantidad_Disponible,
                    item.Precio,
                    Subtotal = item.Cantidad_Disponible * item.Precio
                }).ToList();
                gvCarrito.DataBind();

                lblCarrito.Text = carrito.Sum(item => item.Cantidad_Disponible).ToString();
            }
            catch (Exception ex)
            {
                lblCarrito.Text = "Error al actualizar carrito: " + ex.Message;
            }
        }

        // Cargar Libros desde la base de datos
        private void CargarLibros()
        {
            /*
            librosTableAdapter adapter = new librosTableAdapter();
            DataTable libros = adapter.GetData();

            gvLibros.DataSource = libros;
            gvLibros.DataBind();
            */
            try
            {
                librosTableAdapter adapter = new librosTableAdapter();
                DataTable libros = adapter.GetData();

                // Verifica las columnas devueltas por el adaptador (para depuración)
                foreach (DataColumn column in libros.Columns)
                {
                    System.Diagnostics.Debug.WriteLine(column.ColumnName);
                }

                gvLibros.DataSource = libros;
                gvLibros.DataBind();
            }
            catch (Exception ex)
            {
                lblCarrito.Text = "Error al cargar libros: " + ex.Message;
            }
        }

        // Mostrar el carrito de compras en el modal
        protected void btnConfirmarCompra_Click(object sender, EventArgs e)
        {
            try
            {
                List<Carritoses> carrito = (List<Carritoses>)Session["Carrito"];
                var carritoDisplay = carrito.Select(item => new
                {
                    item.Titulo,
                    item.Cantidad_Disponible,
                    item.Precio,
                    Subtotal = item.Cantidad_Disponible * item.Precio
                }).ToList();

                gvCarrito.DataSource = carritoDisplay;
                gvCarrito.DataBind();
            }
            catch (Exception ex)
            {
                lblCarrito.Text = "Error al mostrar carrito: " + ex.Message;
            }
        }

        // Procesar compra
        protected void btnRealizarCompra_Click(object sender, EventArgs e)
        {
            lblCarrito.Text = "¡Compra realizada con éxito!";
            Session["Carrito"] = new List<Carritoses>(); // Vaciar carrito
        }

        // Redirección al inicio de sesión
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        // Clase para representar productos en el carrito
        public class Carritoses
        {
            public int ID_Libro { get; set; }
            public string Titulo { get; set; }
            public decimal Precio { get; set; }
            public int Cantidad_Disponible { get; set; }
        }

        // ----------------------------- Lo de arriba de aquí funciona todo bien ---------------------------------------------------

        private bool ActualizarInventario(string tituloLibro, int cantidadVendida)
        {
            string connectionString = "server=127.0.0.1;user id=root;password=root;persistsecurityinfo=True;database=tiendas_de_libros";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Primero verificamos si hay suficiente stock
                string verificarQuery = "SELECT Cantidad_Disponible FROM libros WHERE Titulo = @Titulo";
                using (MySqlCommand verificarCmd = new MySqlCommand(verificarQuery, connection))
                {
                    verificarCmd.Parameters.AddWithValue("@Titulo", tituloLibro);
                    object resultado = verificarCmd.ExecuteScalar();

                    if (resultado == null || Convert.ToInt32(resultado) < cantidadVendida)
                    {
                        return false; // No hay suficiente stock
                    }
                }

                // Actualizamos el inventario
                string actualizarQuery = "UPDATE libros SET Cantidad_Disponible = Cantidad_Disponible - @Cantidad WHERE Titulo = @Titulo";
                using (MySqlCommand actualizarCmd = new MySqlCommand(actualizarQuery, connection))
                {
                    actualizarCmd.Parameters.AddWithValue("@Cantidad", cantidadVendida);
                    actualizarCmd.Parameters.AddWithValue("@Titulo", tituloLibro);
                    actualizarCmd.ExecuteNonQuery();
                }
            }

            return true; // Operación exitosa
        }

        // ----------------------------- Lo de arriba de aquí funciona todo bien ---------------------------------------------------
        private string GenerarPdfRecibo(List<Carritoses> carrito)
        {
            string filePath = Server.MapPath("~/Recibos/recibo_compra.pdf");

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Document doc = new Document();
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                // Título
                doc.Add(new Paragraph("Recibo de Compra"));
                doc.Add(new Paragraph("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")));
                doc.Add(new Paragraph("\n"));

                // Tabla
                PdfPTable table = new PdfPTable(4); // Nombre, Cantidad, Precio, Subtotal
                table.AddCell("Titulo");
                table.AddCell("Cantidad");
                table.AddCell("Precio");
                table.AddCell("Subtotal");

                decimal total = 0;

                foreach (var item in carrito)
                {
                    table.AddCell(item.Titulo);
                    table.AddCell(item.Cantidad_Disponible.ToString());
                    table.AddCell(item.Precio.ToString("C"));
                    decimal subtotal = item.Cantidad_Disponible * item.Precio;
                    table.AddCell(subtotal.ToString("C"));
                    total += subtotal;
                }

                doc.Add(table);

                // Total
                doc.Add(new Paragraph($"\nTotal: {total.ToString("C")}"));
                doc.Close();
            }

            return filePath;
        }

        public void GuardarRecibo()
        {
            // Define la ruta de la carpeta y el archivo
            string folderPath = @"C:\Users\alfon\source\repos\WebApplication50\Recibos";

            // Verifica si la carpeta existe; si no, la crea
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Define la ruta completa del archivo
            string filePath = Path.Combine(folderPath, "recibo_compra.pdf");

            // Código para generar y guardar el PDF
            File.WriteAllText(filePath, "Contenido del recibo"); // Ejemplo
        }

        private void EnviarCorreoConRecibo(string correoDestino, string archivoPdf)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("112175@alumnouninter.mx");
                mail.To.Add(correoDestino);
                mail.Subject = "Recibo de tu Compra";
                mail.Body = "Gracias por tu compra. Adjunto encontrarás tu recibo en formato PDF.";
                mail.Attachments.Add(new Attachment(archivoPdf));

                SmtpClient smtp = new SmtpClient("smtp.office365.com", 587);
                smtp.Credentials = new NetworkCredential("112175@alumnouninter.mx", "Qop09074");
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                lblCarrito.Text = "Error al enviar al correo: " + ex.Message;
            }
        }

        protected void btnRealizarCompra_Click2(object sender, EventArgs e)
        {
            GuardarRecibo();

            try
            {
                List<Carritoses> carrito = (List<Carritoses>)Session["Carrito"];
                if (carrito == null || !carrito.Any())
                {
                    lblCarrito.Text = "El carrito está vacío.";
                    return;
                }

                // Actualizar inventario
                foreach (var item in carrito)
                {
                    if (!ActualizarInventario(item.Titulo, item.Cantidad_Disponible))
                    {
                        lblCarrito.Text = $"No hay suficiente stock del libro: {item.Titulo}.";
                        return;
                    }
                }

                // Generar PDF
                string pdfPath = GenerarPdfRecibo(carrito);

                // Obtener el correo del usuario (adaptar esto a tu lógica)
                string correoUsuario = "112175@alumnouninter.mx";

                // Enviar el correo
                EnviarCorreoConRecibo(correoUsuario, pdfPath);

                // Vaciar carrito y mostrar mensaje
                Session["Carrito"] = new List<Carritoses>();
                ActualizarCarrito();
                lblCarrito.CssClass = "text-success";
                lblCarrito.Text = "Compra realizada con éxito. Asido enviado a tu correo.";
            }
            catch (Exception ex)
            {
                lblCarrito.Text = "Error al realizar la compra: " + ex.Message;
            }
        }
    }
}