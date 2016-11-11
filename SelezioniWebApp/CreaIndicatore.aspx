<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.master" AutoEventWireup="true"
    CodeBehind="CreaIndicatore.aspx.cs" Inherits="SelezioniWebApp.CreaIndicatore" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_head_lnk" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function validateAttrib(oSrc, args)
        {
            var elemDip = document.getElementById('<%= chk_DipFlg.ClientID %>');
            var elemRsp = document.getElementById('<%= chk_RspFlg.ClientID %>');
            var elemAmm = document.getElementById('<%= chk_AmmFlg.ClientID %>');
            if (!elemDip.checked && !elemRsp.checked && !elemAmm.checked)
            {
                args.IsValid = false;
            }
            else
            {
                args.IsValid = true;
            }
            return;
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="menusx">
        <h4 class="menusx">
            Definizione Indicatore</h4>
        <asp:Menu ID="mnu_CreaIndicatore" runat="server" DynamicMenuStyle-CssClass="menuvoce"
            CssClass="menuvoce" OnMenuItemClick="mnu_CreaIndicatore_MenuItemClick">
            <Items>
                <asp:MenuItem Text="Ritorna" Value="Rit"></asp:MenuItem>
            </Items>
        </asp:Menu>
    </div>
    <div id="contenuto">
        <div>
            <asp:Label ID="LblErr" runat="server" Text="" CssClass="etichetta titolo"></asp:Label>
        </div>
        <div id="testata" runat="server">
            <table runat="server" style="width: 70%;" id="tbl_testata">
                <tr>
                    <td style="width: 20%;">
                        <asp:Label runat="server" ID="label2" Text='Anno'></asp:Label>
                    </td>
                    <td>
                        <asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_anno"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%;">
                        <asp:Label runat="server" ID="label1" Text='Categoria'></asp:Label>
                    </td>
                    <td>
                        <asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_categoriacod"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%;">
                        <asp:Label runat="server" ID="label3" Text="Titolo"></asp:Label>
                    </td>
                    <td>
                        <asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_titolo"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="rigabottombig">
        </div>
        <div class="menuseparatore">
        </div>
        <div id="indicatore_creamod" runat="server" visible="true">
            <asp:Label CssClass="etichetta titolo" runat="server" ID="Label4" Text="Descrizione indicatore (*)"></asp:Label>
            <asp:TextBox CssClass="etichetta" ID="txt_Descr" runat="server" MaxLength="255" TextMode="MultiLine"
                Width="100%" ValidationGroup="1"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RFV_txt_Descr" runat="server" ErrorMessage="Descrizione indicatore OBBLIGATORIA"
                ControlToValidate="txt_Descr" Display="Dynamic" SetFocusOnError="True" Font-Italic="True"
                ForeColor="Red"></asp:RequiredFieldValidator>
            <br />
            <asp:Label CssClass="etichetta titolo" runat="server" ID="Label5" Text="Note compilazione indicatore"></asp:Label>
            <asp:TextBox CssClass="etichetta" ID="txt_NoteDip" runat="server" MaxLength="255"
                TextMode="MultiLine" Width="100%" ValidationGroup="1" ToolTip="Testo esplicativo che compare nel prospetto del dipendente"></asp:TextBox>
            <br />
            <asp:Label CssClass="etichetta titolo" runat="server" ID="Label6" Text="Note valutazione indicatore"></asp:Label>
            <asp:TextBox CssClass="etichetta" ID="txt_NoteVal" runat="server" MaxLength="255"
                TextMode="MultiLine" Width="100%" ValidationGroup="1" ToolTip="Testo esplicativo che compare nel prospetto di valutazione"></asp:TextBox>
            <br />
            <asp:Label CssClass="etichetta titolo" runat="server" ID="Label10" Text="Attribuzioni dell'indicatore (*):"></asp:Label>
            <br />
            <asp:Label CssClass="etichetta titolo" runat="server" ID="Label7" Text="Indicatore a cura del dipendente"></asp:Label>
            <br />
            <asp:CheckBox ID="chk_DipFlg" runat="server" ToolTip="Marcare se l'indicatore deve essere compilato al dipendente" />
            <asp:CustomValidator ID="CMV_chk_DipFlg" runat="server" ErrorMessage="Marcare almeno un'attribuzione dell'indicatore"
                Font-Italic="True" ClientValidationFunction="validateAttrib" ForeColor="Red"></asp:CustomValidator>
            <br />
            <asp:Label CssClass="etichetta titolo" runat="server" ID="Label8" Text="Indicatore a cura del responsabile"></asp:Label>
            <br />
            <asp:CheckBox ID="chk_RspFlg" runat="server" Checked="False" ToolTip="Marcare se l'indicatore deve essere compilato dal responsabile" />
            <asp:CustomValidator ID="CMV_chk_RspFlg" runat="server" ErrorMessage="Marcare almeno un'attribuzione dell'indicatore"
                Font-Italic="True" ClientValidationFunction="validateAttrib" ForeColor="Red"></asp:CustomValidator>
            <br />
            <asp:Label CssClass="etichetta titolo" runat="server" ID="Label9" Text="Indicatore a cura dell'amministrazione"></asp:Label>
            <br />
            <asp:CheckBox ID="chk_AmmFlg" runat="server" Checked="False" ToolTip="Marcare se l'indicatore deve essere compilato dall'amministrazione" />
            <asp:CustomValidator ID="CMV_chk_AmmFlg" runat="server" ErrorMessage="Marcare almeno un'attribuzione dell'indicatore"
                Font-Italic="True" ClientValidationFunction="validateAttrib" ForeColor="Red"></asp:CustomValidator>
            <br />
            <asp:Label CssClass="etichetta titolo" runat="server" ID="Label11" Text="Ordine di comparsa dell'indicatore (*)"></asp:Label>
            <asp:RequiredFieldValidator ID="RFV_txt_Ord" runat="server" ErrorMessage="Ordinamento indicatore OBBLIGATORIO"
                ControlToValidate="txt_Ord" Display="Dynamic" SetFocusOnError="True" Font-Italic="True"
                ForeColor="Red"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="REV_txt_Ord" ControlToValidate="txt_Ord" runat="server"
                ErrorMessage="Valore inserito non numerico" ValidationExpression="^\d+$" ForeColor="Red"></asp:RegularExpressionValidator><br />
            <asp:TextBox CssClass="etichetta" ID="txt_Ord" runat="server" MaxLength="5" Width="5%"
                ValidationGroup="1" ToolTip="Ordinamento di comparsa dell'indicatore"></asp:TextBox>
            <br />
            <div class="rigabottombig">
            </div>
            <div class="menuseparatore">
            </div>
            <asp:Button ID="btnConfermaInd" runat="server" Text="Conferma" CssClass="cerca" OnClick="btnConfermaInd_Click" />
            <asp:Button ID="btnAnnullaInd" runat="server" Text="Annulla" CssClass="cerca" OnClick="btnConfermaInd_Click"
                CausesValidation="False" />
        </div>
    </div>
</asp:Content>
