<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.master" AutoEventWireup="true" CodeBehind="RichiestaRevisione.aspx.cs" Inherits="SelezioniWebApp.RichiestaRevisione" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head_lnk" runat="server">
  <style type="text/css">
        .style1
        {
            width: 425px;
        }
        .style2
        {
            width: 328px;
        }
        input
        {
        	width:300px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
<div id="menusx">
        <h4 class="menusx">
            Richiesta Revisione Punteggi
        </h4>
        <asp:Menu ID="mnu_RichiestaRevisione" runat="server" DynamicMenuStyle-CssClass="menuvoce"
            OnMenuItemClick="mnu_RichiestaRevisione_MenuItemClick" CssClass="menuvoce" RenderingMode="Table">
            <Items>
                <asp:MenuItem Text="Ritorna" Value="Rit"></asp:MenuItem>
            </Items>
        </asp:Menu>
    </div>
    <div id="contenuto" class="etichetta"">
        <asp:Label ID="lblErr" runat="server" Text=""></asp:Label>
        <asp:Label ID="lblAlert"  runat="server" Visible="false"><h1> Attenzione non è più possibile inviare email per la revisione dei punteggi.<br /> Contattare l'ufficio Ufficio Organizzazione e relazioni sindacali </h1> </asp:Label>
        <table style="width: 76%; height: 146px; margin-top: 32px;" >
            <tr>
                <td class="style2">
                    <asp:Label ID="lblEmailUffAmm" visible="false" runat="server">rel.sindacali@amm.units.it</asp:Label>
                </td>
               
            </tr>
           <tr>
                <td class="style2">
                    <asp:Label ID="lblUsername"  runat="server">Username:</asp:Label>
                </td>
                <td class="style1">
                    <asp:TextBox ID="txtUsername" runat="server" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2" >
                    <asp:Label ID="lblTitolo" runat="server">PEO:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtTitolo" runat="server" Enabled ="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    <asp:Label ID="lblCompId" runat="server" >Compilazione ID:</asp:Label>
                </td>
                <td>
                    <asp:TextBox ID="txtCompId" runat="server" Text=""  Enabled ="false"  ></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style2">
                    <asp:Label ID="lblAreaRevisione" runat="server">Richiesta revisione punteggi:</asp:Label>
                </td>
                <td>
                    <asp:TextBox id="txtAreaRevisione" TextMode="multiline" Columns="50" Rows="20" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="style2">
                    <asp:Label ID="lblInvioEmail" runat="server" Visible="false" >Email inviata il:</asp:Label>
                </td>
                <td>
                    <asp:TextBox id="txtInvioEmail" Text="" Visible="false" Enabled ="false" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="style2">
                    <asp:Button ID="btnInviaEmail" runat="server" Text="Invia Email" OnClick="InviaEmailRevisione"></asp:Button>
                </td>
                <td>
                    <asp:Button ID="btnAnnulla" runat="server" Text="Annulla Testo" OnClick="Annulla"></asp:Button>
                </td>
            </tr>
              
            <tr>
                <td class="style2" >
                    &nbsp;
                </td>
                <td >
                    &nbsp;
                    <asp:Label ID="ErrLabel" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style2" >
                    <asp:CheckBox ID="chkPersist" runat="server" Text="Persist Cookie" Visible="false" />
                </td>
                <td >
                    &nbsp;
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:Label ID="RicRev"  runat="server" Text="" CssClass="etichetta titolo"></asp:Label>
</asp:Content>
