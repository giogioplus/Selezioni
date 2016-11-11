<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.master" AutoEventWireup="true" CodeBehind="logout.aspx.cs" Inherits="SelezioniWebApp.logout" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head_lnk" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="contenuto">
        <div id="testata" runat="server">
            <asp:Label ID="lblErr" Text="Precedentemente sei uscito dall'applicativo PEO senza chiudere il BROWSER utilizzato. Per uscire correttamente dalla procedura chiudi il browser e rientra nell'applicativo PEO" runat="server"></asp:Label></div>
    </div>
</asp:Content>
