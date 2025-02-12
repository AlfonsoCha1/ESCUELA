using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication50.Models;
using WebApplication50.CONTROLLER;
using WebApplication50.Models.TiendaLibrosTableAdapters;

namespace WebApplication50.VIEWS
{
    public partial class Administrador : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                {
                   
                }
            }

            // Agregando Lirbros
            protected void btnAgregarLibro_Click(object sender, EventArgs e)
            {
                string titulo = txtTitulo.Text; 
                string autor = txtAutor.Text;
                string editorial = txtEditorial.Text;
                decimal precio = decimal.Parse(txtPrecio.Text);
                int cantidad_disponible = int.Parse(txtCantidad_Disponible.Text);
                string genero = txtGenero.Text;
                int año_publicacion = int.Parse(txtAño_Publicacion.Text);

                // Insertar el libro en la mysql
                librosTableAdapter libros = new librosTableAdapter();
                libros.InsertQuery(titulo, autor,  precio, editorial,  genero, cantidad_disponible, año_publicacion);

                // Mostrar un mensaje de éxito
                lblMessage.Text = "El libro fue  agregado exitosamente.";
                lblMessage.ForeColor = System.Drawing.Color.Green;

                // Limpiar los campos del formulario
                ClearFields();
            }

            // Botón mostrar productos
            protected void BtnMostrar_Click(object sender, EventArgs e)
            {
                CargarLibros();
            }

            // Método para cargar productos en el GridView
            private void CargarLibros()
            {
                 Libros librosControler = new Libros();
                var libros = librosControler.ObtenerLibros();

                if (libros.Count > 0)
                {
                    GridDatos.DataSource = libros;
                    GridDatos.DataBind();
                }
                else
                {
                    lblMessage.Text = "No se encontraron productos.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }



            // Manejar el evento de actualizar producto
            protected void CargarLibroParaEdicion(int idLibro)
            {
                Libros librosControler = new Libros();
                var libros = librosControler.ObtenerLibros();
                var libro = libros.Find(p => p.ID_Libro == idLibro);

                if (libro != null)
                {
                txtTitulo.Text = libro.Titulo;
                txtAutor.Text = libro.Autor;
                txtEditorial.Text = libro.Editorial;
                    txtPrecio.Text = libro.Precio.ToString();
                txtCantidad_Disponible.Text = libro.Cantidad_Disponible.ToString();
                txtGenero.Text = libro.Genero;
                txtAño_Publicacion.Text = libro.Año_Publicacion.ToString("yyyy");

                    // Guardamos el ID del producto para usarlo en la actualización
                    ViewState["ID_Libro"] = libro.ID_Libro;
                }
            }


            protected void btnActualizar_Click(object sender, EventArgs e)
            {
                // Obtener los valores desde los controles de la página
                int idLibro = Convert.ToInt32(ViewState["ID_Libro"]);  // Suponiendo que el ID se guarda en ViewState
                string titulo = txtTitulo.Text;
                string autor = txtAutor.Text;
                string editorial = txtEditorial.Text;
                decimal precio = decimal.Parse(txtPrecio.Text);
                int cantidaddisponible = int.Parse(txtCantidad_Disponible.Text);
                string genero = txtGenero.Text;
                int año_publicacion = int.Parse(txtAño_Publicacion.Text);

                // Crear una instancia de la clase Productos
                Libros libro = new Libros();

                // Llamar al método para actualizar el producto
                bool exito = libro.ActualizarLibro(titulo, autor, precio, editorial,   genero, cantidaddisponible, año_publicacion, idLibro);

                if (exito)
                {
                    lblMessage.Text = "Actualizado exitosamente.";
                    lblMessage.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblMessage.Text = "Error al actualizar del los libros.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                }
            }


        // Funciona Actializar pero no Eliminar
        protected void GridDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ActualizarLibro")
            {
                int idLibro = Convert.ToInt32(e.CommandArgument); // Obtener el ID del producto desde el CommandArgument

                string titulo = txtTitulo.Text;
                string autor = txtAutor.Text;
                string editorial = txtEditorial.Text;
                decimal precio = decimal.Parse(txtPrecio.Text);
                int cantidad_disponible = int.Parse(txtCantidad_Disponible.Text);
                string genero = txtGenero.Text;
                int año_publicacion = int.Parse(txtAño_Publicacion.Text);

                
                librosTableAdapter libroAdapter = new librosTableAdapter();

  
                libroAdapter.UpdateLibro(titulo, autor, precio, editorial,   genero, cantidad_disponible, año_publicacion, idLibro);

             
                lblMessage.Text = "Producto actualizado exitosamente.";
                lblMessage.ForeColor = System.Drawing.Color.Green;

                // Limpiar los campos
                ClearFields();

                // Volver a cargar los productos para reflejar los cambios
                CargarLibros();
            }


            if (e.CommandName == "EliminarLibro")
            {
                // Obtener el ID del producto que se quiere eliminar
                int idLibro = Convert.ToInt32(e.CommandArgument);

                // Crear una instancia de la clase Libros
                Libros librosControler = new Libros();

                // Llamar al método de eliminación en el controlador
                bool eliminado = librosControler.EliminarLibro(idLibro);

                // Verificar si la eliminación fue exitosa
                if (eliminado)
                {
                    // Mostrar un mensaje de éxito
                    lblMessage.Text = "Eliminado con éxito.";
                    lblMessage.CssClass = "mensaje exitoso";
                }
                else
                {
                    // Mostrar un mensaje de error
                    lblMessage.Text = "Error al eliminar el libro.";
                    lblMessage.CssClass = "error de mensaje ";
                }

                // Actualizar la lista de productos
                CargarLibros(); // Método que actualiza el GridView con los productos actuales
            }
        }

            // Limpiar los campos del formulario
            private void ClearFields()
            {
                txtTitulo.Text = "";
                txtAutor.Text = "";
                txtEditorial.Text = "";
                txtPrecio.Text = "";
                txtCantidad_Disponible.Text = "";
                txtGenero.Text = "";
                txtAño_Publicacion.Text = "";
            }

        protected void btnCerrarSesion_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Views/Pantalla Principal.aspx");
        }
    }
}