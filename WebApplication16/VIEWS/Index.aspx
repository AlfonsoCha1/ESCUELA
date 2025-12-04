<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="WebApplication16.VIEWS.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="content-type" content="text/html; charset=utf-8"/>
    <title>Calcular la edad</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
             <asp:Label Text="Elige tu fecha de nacimiento:" runat="server" />
            <asp:Calendar ID="calendar1" runat="server"></asp:Calendar>
            <br />
            <asp:Button ID="Calcular" Text="Calcular Edad" runat="server" OnClick="Calcular_Click" />
            <br />
            <asp:Label ID="Resultado" Text="" runat="server" />
        </div>
    </form>
</body>
</html>
