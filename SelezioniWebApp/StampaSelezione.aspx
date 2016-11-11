<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.master" AutoEventWireup="true"
    CodeBehind="StampaSelezione.aspx.cs" Inherits="SelezioniWebApp.StampaSelezione" %>

<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=6.0.12.215, Culture=neutral, PublicKeyToken=a9d7983dfcc261be"
    Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_head_lnk" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="menusx">
        <h4 class="menusx">
            Stampa selezione
        </h4>
        <asp:Menu ID="mnu_main" runat="server" OnMenuItemClick="mnu_main_MenuItemClick" CssClass="menuvoce">
            <Items>
                <asp:MenuItem Text="Scheda Domanda" Value="Dom"></asp:MenuItem>
                <asp:MenuItem Text="Scheda Valutazione" Value="Val"></asp:MenuItem>
                <asp:MenuItem Text="Ritorna" Value="Rit"></asp:MenuItem>
            </Items>
        </asp:Menu>
    </div>
    <div id="contenuto">
        <div id="generale" runat="server">
            <asp:Label ID="lblErr" runat="server" Text=""></asp:Label><br />
            <asp:Label ID="lblTitolo" runat="server" Font-Bold="True" Font-Size="Medium"></asp:Label>
            <br />
        </div>
        <div id="stampa">
            <telerik:ReportViewer ID="rpv_StampaSelezione" Height="707px" ParametersAreaVisible="False"
                ShowDocumentMapButton="False" ShowParametersButton="False" ShowZoomSelect="True"
                Width="997px" runat="server">
            </telerik:ReportViewer>
        </div>
    </div>
</asp:Content>
