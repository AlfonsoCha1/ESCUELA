<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WebApplication50.VIEWS.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login - Tienda de Libros</title>

     <style>
       body {
           background-image: url('https://www.elindependiente.com/wp-content/uploads/2021/02/Dos-alternativas-para-comprar-libros-online-sin-recurrir-a-Amazon.jpg'); /* Reemplaza con una imagen de personas comprando libros en una tienda */
            background-size: cover;
            background-attachment: fixed;
            display: flex;
            align-items: center;
            justify-content: center;
            height: 100vh;
            margin: 0;
            font-family: 'Georgia', serif; /* Fuente más clásica para la tienda de libros */
            color: white;
        }
        .overlay {
            position: absolute;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.7); /* Fondo oscuro para mejorar la visibilidad del contenido */
        }
        .login-container {
            position: relative;
            z-index: 1;
            background-color: rgba(20, 20, 20, 0.8);
            padding: 40px;
            border-radius: 10px;
            text-align: center;
            width: 350px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.4);
            backdrop-filter: blur(5px);
            animation: fadeIn 1s ease-out;
        }
        @keyframes fadeIn {
            from { opacity: 0; transform: scale(0.9); }
            to { opacity: 1; transform: scale(1); }
        }
        .login-container h2 {
            font-size: 28px;
            font-weight: bold;
            color: #f4a261; /* Un color cálido que puede recordar al papel envejecido */
        }
        .input-field {
            background-color: rgba(255, 255, 255, 0.2);
            border: none;
            padding: 10px;
            border-radius: 5px;
            color: white;
            width: 100%;
            margin: 10px 0;
        }
        .login-button {
            background-color: #2a9d8f; /* Verde claro para transmitir tranquilidad */
            color: white;
            padding: 12px;
            border: none;
            border-radius: 5px;
            width: 100%;
            margin-top: 20px;
            cursor: pointer;
            font-size: 16px;
            font-weight: bold;
            transition: background-color 0.3s;
        }
        .login-button:hover {
            background-color: #264653; /* Un tono más oscuro para el hover */
        }
        .login-options, .register-link {
            color: #a0e3ff;
            text-decoration: none;
            transition: color 0.3s;
        }
        .register-link:hover, .login-options a:hover {
            color: #ffdd57; /* Color amarillo para acentuar los enlaces */
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="overlay"></div>
        <div class="login-container">
            <h2>Tienda de Libros</h2>
            <asp:TextBox ID="Username" runat="server" CssClass="input-field" placeholder="Nombre de usuario"></asp:TextBox>
            <asp:TextBox ID="Password" runat="server" CssClass="input-field" placeholder="Contraseña" TextMode="Password"></asp:TextBox>
            <div class="login-options">
                <asp:CheckBox ID="RememberMe" runat="server" Text="Recordarme" ForeColor="white" />
                <a href="#" style="color: #a0e3ff;">¿Olvidaste tu contraseña?</a>
            </div>
            <asp:Button ID="LoginButton" runat="server" Text="Iniciar sesión" CssClass="login-button" OnClick="LoginButton_Click" />
            <div style="margin-top: 10px;">
                <span>¿No tienes cuenta?</span>
                <a href="Registrar.aspx" class="register-link">Regístrate</a>
            </div>
        </div>
    </form>

</body>
</html>
