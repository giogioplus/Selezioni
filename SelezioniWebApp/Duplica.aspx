<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.master" AutoEventWireup="true"
    CodeBehind="Duplica.aspx.cs" Inherits="SelezioniWebApp.Duplica" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_head_lnk" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
  <style type="text/css">
        .etichetta
        {
            margin-left: 0px;
        }
        .style3
        {
            width: 513px;
        }
        .style4
        {
            width: 149px;
        }
        .style5
        {
            width: 424px;
            height: 30px;
        }
        .style6
        {
            width: 226px;
            height: 30px;
        }
        .style7
        {
            height: 30px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="menusx">
        <h4 class="menusx">
            Duplica selezione</h4>
        <asp:Menu ID="mnu_DuplicaSelezione" runat="server" DynamicMenuStyle-CssClass="menuvoce"
            CssClass="menuvoce" OnMenuItemClick="mnu_DuplicaSelezione_MenuItemClick">
            <Items>
                <asp:MenuItem Text="Ritorna" Value="Back"></asp:MenuItem>
            </Items>
        </asp:Menu>
    </div>
    <div id="contenuto">
        <div id="Autore" runat="server">
            <table id="Table1" runat="server" class="allineacellehtmlnobord">
            <tr>
                    <td class="style4">
                        <asp:Label ID="lbl_titolo" runat="server" Text="Selezione" CssClass="etichetta titolo"></asp:Label>
                    </td>
                    <td class="style3">
                        <asp:Label ID="lbl_titolo_t" runat="server" Text="" CssClass="etichetta titolo"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style4">
                        <asp:Label ID="lbl_categoria_cod" runat="server" Text="Categoria" CssClass="etichetta titolo"></asp:Label>
                    </td>
                    <td class="style3">
                        <asp:Label ID="lbl_categoria_cod_t" runat="server" Text="" CssClass="etichetta titolo"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style4">
                        <asp:Label ID="lbl_anno" runat="server" Text="Anno" CssClass="etichetta titolo"></asp:Label>
                    </td>
                    <td class="style3">
                        <asp:Label ID="lbl_anno_t" runat="server" Text="" CssClass="etichetta titolo"></asp:Label>
                    </td>
                </tr>
            </table>
            </div>
            <div class="rigabottombig">
            </div>
        <div id="testata_duplica" runat="server" visible="true">
        <br />
                <asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_anno_d" Text="Duplica per anno"></asp:Label>
                <asp:CheckBox
                    ID="ckb_anno" runat="server" />
                    <br />
                    <br />
                <asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_categoria" Text="Duplica per Categoria"></asp:Label><br />
                <asp:DropDownList ID="ddl_ins_categoria_cod" runat="server"  OnSelectedIndexChanged="ddl_ins_categoria_SelectedIndexChanged">
                </asp:DropDownList>
                <br />
                <br />
                <asp:Button ID="btnConfermaSel" runat="server" Text="Duplica" CssClass="cerca"
                    OnClick="btnConfermaSel_Click" />
                <asp:Button ID="btnAnnullaSel" runat="server" Text="Annulla" CssClass="cerca" OnClick="btnConfermaSel_Click" />
                
        </div>
    </div>
</asp:Content>
