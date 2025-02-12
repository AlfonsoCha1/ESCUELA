<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Pantalla Principal.aspx.cs" Inherits="WebApplication50.VIEWS.Pantalla_Principal" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <title>Tienda De Smucky</title>
    <!-- Incluye Bootstrap -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <div class="container mt-5 p-4 bg-white rounded shadow">
            <!-- Cabecera -->
            <header class="d-flex justify-content-between align-items-center mb-4">
                <h1 class="h3 text-primary">Bienvenido a la Tienda de Smucky</h1>
                <div class="d-flex align-items-center">
                    <span class="me-2">Productos en el carrito:</span>
                    <asp:Label ID="lblCarrito" runat="server" Text="0" CssClass="badge bg-primary fs-5" />
                </div>
                <!-- Botones -->
                <div>
                    <asp:Button ID="btnEntrar" Text="Log in" runat="server" CssClass="btn btn-secondary me-2" OnClick="btnLogin_Click" />
                    <asp:Button ID="btnConfirmarCompra" runat="server" Text="Confirmar Compra" CssClass="btn btn-primary" OnClientClick="mostrarModal(); return false;" />
                </div>
            </header>

            <!-- GridView para mostrar los productos -->
            <section>
                <asp:GridView ID="gvLibros" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered table-hover" DataKeyNames="ID_Libro,Precio">
                    <Columns>
                        <asp:BoundField DataField="Titulo" HeaderText="Titulo" SortExpression="Titulo" />
                        <asp:BoundField DataField="Precio" HeaderText="Precio" SortExpression="Precio" DataFormatString="{0:C}" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnAgregarLibro" runat="server" Text="Agregar Libro" CssClass="btn btn-success btn-sm" 
                                    OnClick="btnAgregarLibro_Click" CommandArgument='<%# Container.DataItemIndex %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </section>

            <!-- Modal de confirmación -->
            <div id="modalConfirmacion" class="modal fade" tabindex="-1" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header bg-primary text-white">
                            <h5 class="modal-title">Confirmar Compra</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
                        </div>
                        <div class="modal-body">
                            <asp:GridView ID="gvCarrito" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered">
                                <Columns>
                                    <asp:BoundField DataField="Titulo" HeaderText="Titulo" />
                                    <asp:BoundField DataField="Cantidad_Disponible" HeaderText="Cantidad Disponible" />
                                    <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="{0:C}" />
                                    <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="{0:C}" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnRealizarCompra" runat="server" Text="Realizar Compra" CssClass="btn btn-primary" OnClick="btnRealizarCompra_Click2" />
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <!-- Scripts de Bootstrap -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script>
        function mostrarModal() {
            let modal = new bootstrap.Modal(document.getElementById('modalConfirmacion'));
            modal.show();
        }
    </script>
</body>
</html>
