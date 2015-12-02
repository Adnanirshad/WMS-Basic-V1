<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WMS.AppForms.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <section class="col-md-5">
        <div class="col-md-6">
            <asp:Label ID="Label1" runat="server" Text="P.No"></asp:Label>
        </div>
        <div class="col-md-6">
            <asp:TextBox ID="tbEmpNo" runat="server"></asp:TextBox>
        </div>
        <div class="col-md-6">
            <asp:Label ID="Label4" runat="server" Text="Date"></asp:Label>
        </div>
        <div class="col-md-6">
            <asp:TextBox ID="tbDate" runat="server"></asp:TextBox>
        </div>
        <div class="col-md-6">
            <asp:Label ID="Label2" runat="server" Text="Time In"></asp:Label>
        </div>
        <div class="col-md-6">
             <asp:TextBox ID="tbTimeIn" runat="server"></asp:TextBox>
        </div>
        <div class="col-md-6">
            <asp:Label ID="Label5" runat="server" Text="Time Out"></asp:Label>
        </div>
        <div class="col-md-6">
             <asp:TextBox ID="tbTimeOut" runat="server"></asp:TextBox>
        </div>
    </section>
    <section class="col-md-7">

    </section>
   
    <asp:Button ID="Button1" runat="server" Text="Button" />
    </form>
</body>
</html>
