<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="SelezioniWebApp.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_head_lnk" runat="server">
    <link href="App_Themes/SkinFile/cssSelezioni.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:Label ID="LblErr"  runat="server" Text="" CssClass="etichetta titolo"></asp:Label>
</asp:Content>
