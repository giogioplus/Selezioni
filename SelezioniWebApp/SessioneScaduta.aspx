<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.master" AutoEventWireup="true" CodeBehind="SessioneScaduta.aspx.cs" Inherits="SelezioniWebApp.SessioneScaduta" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head_lnk" runat="server">
    <link href="App_Themes/SkinFile/cssSelezioni.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="menusx">
        <h4 class="menusx">
            Selezione</h4>
        <asp:Menu ID="mnu_CompilaSelezione" runat="server" DynamicMenuStyle-CssClass="menuvoce"
            CssClass="menuvoce" OnMenuItemClick="mnu_CompilaSelezione_MenuItemClick">
            <Items>
                <asp:MenuItem Text="Esci dalla procedura" Value="Rit"></asp:MenuItem>
            </Items>
        </asp:Menu>
    </div>
    <div id="contenuto">
        <div id="testata" runat="server">
            <asp:Label ID="lblErr" Text="Sessione scaduta" runat="server"></asp:Label></div>
    </div>
</asp:Content>
