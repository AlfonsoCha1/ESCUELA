<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Administrador.aspx.cs" Inherits="WebApplication50.VIEWS.Administrador" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>Agregar Libros</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f9f9f9;
            background-image: url('https://static.vecteezy.com/system/resources/previews/022/971/761/non_2x/books-library-people-graphic-novel-illustration-colorful-illustration-vector.jpg'); /* Imagen de fondo */
            background-size: cover;
            background-position: center;
            background-attachment: fixed;
            color: #fff;
        }

        .container {
            max-width: 600px;
            margin: auto;
            background: rgba(255, 255, 255, 0.8); /* Fondo semi-transparente */
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        }

        h2 {
            text-align: center;
            margin-bottom: 20px;
            font-size: 28px;
            color: black;
        }

        .form-group {
            margin-bottom: 15px;
        }

        .form-group label {
            font-weight: bold;
            display: block;
            margin-bottom: 5px;
        }
        label{
            color: black;
        }

        .form-group input, .form-group textarea {
            width: 100%;
            padding: 10px;
            border: 1px solid #ddd;
            border-radius: 5px;
        }

        .form-group textarea {
            resize: none;
        }

        .btn {
            padding: 10px 20px;
            background-color: #28a745; /* Verde */
            color: white;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            text-align: center;
            margin: 10px 0;
        }

        .btn:hover {
            background-color: #218838; /* Verde oscuro */
        }

        .btn-secondary {
            background-color: #28a745; /* Verde */
        }

        .btn-inicio {
            background-color: coral;
        }

        .btn-secondary:hover {
            background-color: #218838;
        }

        .btn-warning {
            background-color: #ffc107;
        }

        .btn-warning:hover {
            background-color: #e0a800;
        }

        .message {
            margin-top: 20px;
            font-size: 14px;
            text-align: center;
        }

        table {
            width: 100%;
            margin-top: 20px;
            border-collapse: collapse;
            color: black; /* Cambia el color del texto a negro */
        }

        table th, table td {
            padding: 10px;
            border: 1px solid #ddd;
            text-align: left;
            color: black; /* Cambia el color del texto de los encabezados a negro */
        }

        table th {
            background-color: #f1f1f1;
            color: black; /* Cambia el color del texto de las celdas a negro */
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Agregar Libros</h2>
            <div class="form-group">
                <label for="txtTitulo">Titulo del Libro:</label>
                <asp:TextBox ID="txtTitulo" runat="server" placeholder="Titulo"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtAutor">Autor:</label>
                <asp:TextBox ID="txtAutor" runat="server" placeholder="Autor"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtEditorial">Editorial:</label>
                <asp:TextBox ID="txtEditorial" runat="server" placeholder="Editorial"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtPrecio">Precio:</label>
                <asp:TextBox ID="txtPrecio" runat="server" placeholder="Precio"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtCantidad_Disponible">Cantidad Disponible:</label>
                <asp:TextBox ID="txtCantidad_Disponible" runat="server" placeholder="Cantidad_Disponible"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtGenero">Genero:</label>
                <asp:TextBox ID="txtGenero" runat="server" TextMode="MultiLine" Rows="3" placeholder="Genero"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtAño_Publicacion">Año_Publicacion:</label>
                <asp:TextBox ID="txtAño_Publicacion" runat="server" placeholder="yyyy-MM-dd"></asp:TextBox>
            </div>
            <asp:Button ID="btnAgregarLibro" runat="server" Text="Agregar Libro" CssClass="btn" OnClick="btnAgregarLibro_Click" />
            <asp:Button ID="BtnMostrar" runat="server" Text="Mostrar Libros" CssClass="btn btn-secondary" OnClick="BtnMostrar_Click" />
            <asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label>
            <asp:Button ID="btnCerrarSesion" runat="server" Text="Cerrar Sesion" CssClass="btn btn-inicio" OnClick="btnCerrarSesion_Click" />

            <!-- GridView para mostrar y actualizar los productos -->
            <asp:GridView ID="GridDatos" runat="server" AutoGenerateColumns="false" CssClass="grid" OnRowCommand="GridDatos_RowCommand">
                <Columns>
                    <asp:BoundField DataField="ID_Libro" HeaderText="ID" SortExpression="Id_Libro" />
                    <asp:BoundField DataField="Titulo" HeaderText="Nombre" SortExpression="Titulo" />
                    <asp:BoundField DataField="Autor" HeaderText="Marca" SortExpression="Autor" />
                    <asp:BoundField DataField="Editorial" HeaderText="Categoría" SortExpression="Editorial" />
                    <asp:BoundField DataField="Precio" HeaderText="Precio" SortExpression="Precio" />
                    <asp:BoundField DataField="Cantidad_Disponible" HeaderText="Cantidad" SortExpression="CantidadDisponible" />
                    <asp:BoundField DataField="Genero" HeaderText="Descripción" SortExpression="Genero" />
                    <asp:BoundField DataField="Año_Publicacion" HeaderText="Fecha" SortExpression="Año_Publicacion" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" 
                                        CommandName="ActualizarLibro" 
                                        CommandArgument='<%# Eval("ID_Libro") %>' 
                                        CssClass="btn btn-warning" />
                            <asp:Button ID="btnEliminar" runat="server" Text="Eliminar" 
                                        CommandName="EliminarLibro" 
                                        CommandArgument='<%# Eval("ID_Libro") %>' 
                                        CssClass="btn btn-danger" 
                                        OnClientClick="return confirm('¿Estás seguro de que deseas eliminar este libro?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>

</body>
</html>
