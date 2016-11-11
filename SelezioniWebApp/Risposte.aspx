<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.master" AutoEventWireup="true"
    CodeBehind="Risposte.aspx.cs" Inherits="SelezioniWebApp.Risposte" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_head_lnk" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        var isShift = false;

        var seperator = "/";

        function DateFormat(txt, keyCode) {
            if (keyCode == 16)
                isShift = true;
            if (((keyCode >= 48 && keyCode <= 57) || keyCode == 8 ||
                 keyCode <= 37 || keyCode <= 39 ||
                 (keyCode >= 96 && keyCode <= 105)) && isShift == false) {
                if ((txt.value.length == 2 || txt.value.length == 5) && keyCode != 8) {
                    txt.value += seperator;
                }
                return true;
            }
            else {
                return false;
            }
        }

        function ValidateDate(txt, keyCode) {
            if (keyCode == 16)
                isShift = false;
            var val = txt.value;

            if (val.length == 10) {
                var splits = val.split("/");
                var dt = new Date(splits[1] + "/" + splits[0] + "/" + splits[2]);
                if (!(dt.getDate() == splits[0] && dt.getMonth() + 1 == splits[1]
                    && dt.getFullYear() == splits[2])) {
                    //            lblmesg.style.color="red";
                    //            lblmesg.innerHTML = "Data invalida";
                    alert("Data non valida");
                    txt.value = "";
                    txt.focus();
                    txt.select();
                    return false;
                }
                else {
                    if (!RangeValidation(dt)) {
                        txt.value = "";
                        txt.focus();
                        txt.select();
                        return false;
                    }
                    else {
                        //                lblmesg.innerHTML = "";
                        return true;
                    }
                }
            }
            else if (val.length < 10 && val.length > 0) {
                //        lblmesg.style.color="red";
                //        lblmesg.innerHTML = 
                //         "Data invalida (formato richiesto dd/mm/yy).";
                alert("Data non valida (formato richiesto dd/mm/yyyy)");
                txt.value = "";
                txt.focus();
                txt.select();
                return false;
            }
        }

        function RangeValidation(dt) {
            return true;
            //            var startrange = new Date(Date.parse("01/01/2007"));
            //            var endrange = new Date(Date.parse("12/31/2011"));
            //            if (dt < startrange || dt > endrange)
            //            {
            //                //        lblmesg.style.color="red";
            //                //        lblmesg.innerHTML = "La data deve essere compresa tra 01/01/1900 e 31/12/2099";
            //                alert("La data deve essere compresa tra 01/01/2007 e 31/12/2011");
            //                return false;
            //            }
            //            else
            //            {
            //                //        lblmesg.innerHTML = "";
            //                return true;
            //            }
        }

        function ValidateNumber(txt) {
            var testnum = txt.value.replace(",", ".");
            if (isNaN(testnum) && txt.value.length > 0) {
                alert("Dato non numerico");
                txt.value = "";
                txt.focus();
                txt.select();
                return false;
            }
            else {
                if (testnum.replace(".", "").length > 5) {
                    alert("Il numero deve avere al massimo 3 interi e due decimali");
                    txt.value = "";
                    txt.focus();
                    txt.select();
                    return false;
                }
                var num_array = testnum.split(".");
                if (num_array.length > 2) {
                    alert("Dato non numerico");
                    txt.value = "";
                    txt.focus();
                    txt.select();
                    return false;
                }
                else if (num_array[0].length > 3) {
                    alert("Il numero deve avere al massimo 3 interi");
                    txt.value = "";
                    txt.focus();
                    txt.select();
                    return false;
                }
                else if (num_array.length == 2 && num_array[1].length > 2) {
                    alert("Il numero deve avere al massimo due decimali");
                    txt.value = "";
                    txt.focus();
                    txt.select();
                    return false;
                }
                else
                    return true;
            }
        }


    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="contenuto" runat="server">
        <div id="intestazione" runat="server">
            <div class="marginesxPunteggi" runat="server">
                <asp:GridView ID="grdPunteggi" runat="server" CellPadding="4" ForeColor="#333333"
                    GridLines="None" AutoGenerateColumns="False" Width="80%" Visible="true" BorderStyle="Double">
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <Columns>
                        <asp:TemplateField HeaderText="Punteggi">
                            <ItemTemplate>
                                <asp:Label ID="lblDescr" runat="server" Text='<%# Bind("DescrPunt") %>' EnableViewState="True"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:Label ID="lblPunt" runat="server" Text='<%# Bind("Punt") %>' EnableViewState="True"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
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
                    <tr>
                        <td style="width: 20%;">
                            <asp:Label runat="server" ID="label5" Text="Indicatore"></asp:Label>
                        </td>
                        <td>
                            <asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_descrind"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label CssClass="etichetta nota" runat="server" ID="lbl_notaind"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 20%;">
                            <asp:Label runat="server" ID="label4" Text="Dettaglio"></asp:Label>
                        </td>
                        <td>
                            <asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_descrinddet"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label CssClass="etichetta nota" runat="server" ID="lbl_notainddet"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:Panel ID="pnl_dettaglio" DefaultButton="btn_hide" runat="server">
           <%-- <div id="Div1" class="rigabottombig" runat="server">
            </div>--%>
            <table runat="server" id="tbl_riga" width="100%" class="nomargini">
                <tr>
                    <td class="rigabottombig" >&nbsp;
                    </td>
                </tr>
            </table>
            <asp:ImageButton ID="btn_hide" runat="server" ImageUrl="~/Images/Blank.png" Height="1px"
                Width="1px" />
            <div id="comandi" runat="server" class="margini">
                <asp:Button ID="btnSav" runat="server" Text="Conferma" OnClick="btnSav_Click" Width="100px"
                    Enabled="false" />
                <asp:Button ID="btnRit" runat="server" Text="Ritorna" OnClick="btnRit_Click" Width="100px" />
            </div>
            <div id="dettaglio" runat="server">
            </div>
        </asp:Panel>
        <div class="marginesx">
        </div>
    </div>
</asp:Content>
