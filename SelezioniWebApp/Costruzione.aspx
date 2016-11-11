<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.master" AutoEventWireup="true"
    CodeBehind="Costruzione.aspx.cs" Inherits="SelezioniWebApp.Costruzione" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_head_lnk" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="js/jquery-1.4.4.js"></script>
    <script type="text/javascript">
        var lastRowSelected;
        var originalColor;
        var lastRowSelectedD;
        var originalColorD;
        var lastRowSelectedDR;
        var originalColorDR;

        function GrdIndicatori_selectRow(row, IndId)
        {
            var hdn = document.getElementById("<%= hdnIndId.ClientID %>");
            var hdnR = document.getElementById("<%= hdnRigaIndicatoreSel.ClientID %>");
            if (hdnR != null)
            {
                if (hdnR.value != row.rowIndex)
                {
                    //                    $('#indicatoridet').ready(function ()
                    //                    {
                    //                        $('.visibile').hide();
                    //                    });


                    var hdnD = document.getElementById("<%= hdnIndDetId.ClientID %>");
                    var hdnDR = document.getElementById("<%= hdnRigaIndicatoreDetSel.ClientID %>");
                    if (hdnD != null)
                    {
                        hdnD.value = '0';
                        hdnDR.value = '';
                    }
                    var flgGetRowData = '1';
                }
                hdnR.value = row.rowIndex;
            }
            hdn.value = IndId;
            if (lastRowSelected != row)
            {
                if (lastRowSelected != null)
                {
                    lastRowSelected.style.backgroundColor = originalColor;
                    lastRowSelected.style.color = 'black'
                    lastRowSelected.style.fontWeight = 'normal';
                }
                originalColor = row.style.backgroundColor
                row.style.backgroundColor = '#E2DED6'
                row.style.color = '#333333'
                row.style.fontWeight = 'normal'
                lastRowSelected = row;
            }
            if (flgGetRowData == '1')
            {
                flgGetRowData = '0';
                GetRowData(IndId);
            }
        }

        function GrdIndicatoriDet_selectRow(row, IndDetid)
        {
            var hdn = document.getElementById("<%= hdnIndDetId.ClientID %>");
            var hdnR = document.getElementById("<%= hdnRigaIndicatoreDetSel.ClientID %>");
            if (hdnR != null)
            {
                hdnR.value = row.rowIndex;
            }
            hdn.value = IndDetid;
            if (lastRowSelectedD != row)
            {
                if (lastRowSelectedD != null)
                {
                    lastRowSelectedD.style.backgroundColor = originalColorD;
                    lastRowSelectedD.style.color = 'black'
                    lastRowSelectedD.style.fontWeight = 'normal';
                }
                originalColorD = row.style.backgroundColor
                row.style.backgroundColor = '#E2DED6'
                row.style.color = '#333333'
                row.style.fontWeight = 'normal'
                lastRowSelectedD = row;
            }
        }

        function GridView_mouseHover(row)
        {
            row.style.cursor = 'hand';
        }

        function selRiga()
        {
            var hdnR = document.getElementById("<%= hdnRigaIndicatoreSel.ClientID %>");
            var hdn = document.getElementById("<%= hdnIndId.ClientID %>");
            var tblS = document.getElementById("<%= grdIndicatori.ClientID %>");
            if (tblS != null && hdnR != null && hdnR.value != "")
            {
                GrdIndicatori_selectRow(tblS.rows[hdnR.value], hdn.value);
            }

            var hdnRD = document.getElementById("<%= hdnRigaIndicatoreDetSel.ClientID %>");
            var hdnD = document.getElementById("<%= hdnIndDetId.ClientID %>");
            var tblD = document.getElementById("<%= grdIndicatoriDet.ClientID %>");
            if (tblD != null && hdnRD != null && hdnRD.value != "")
            {
                GrdIndicatoriDet_selectRow(tblD.rows[hdnRD.value], hdnD.value);
            }
        }

        function GetRowData(index)
        {
            __doPostBack('<%=btnPopulate.UniqueID%>', index);
        }
        onload = selRiga;

    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="menusx">
        <h4 class="menusx">
            Costruzione della selezione</h4>
        <asp:Menu ID="mnu_CostruisciSelezione" runat="server" DynamicMenuStyle-CssClass="menuvoce"
            CssClass="menuvoce" OnMenuItemClick="mnu_CostruisciSelezione_MenuItemClick">
            <Items>
                <asp:MenuItem Text="Modifica testata" Value="ModT"></asp:MenuItem>
                <asp:MenuItem Text="Indicatori">
                    <asp:MenuItem Text="Inserisci indicatore" Value="InsInd"></asp:MenuItem>
                    <asp:MenuItem Text="Modifica indicatore" Value="ModInd"></asp:MenuItem>
                    <asp:MenuItem Text="Duplica indicatore" Value="DuplInd"></asp:MenuItem>
                    <asp:MenuItem Text="Cancella indicatore" Value="DelInd"></asp:MenuItem>
                </asp:MenuItem>
                <asp:MenuItem Text="Dettagli">
                    <asp:MenuItem Text="Inserisci dettaglio" Value="InsIndDet"></asp:MenuItem>
                    <asp:MenuItem Text="Modifica dettaglio" Value="ModIndDet"></asp:MenuItem>
                    <asp:MenuItem Text="Duplica dettaglio" Value="DuplIndDet"></asp:MenuItem>
                    <asp:MenuItem Text="Cancella dettaglio" Value="DelIndDet"></asp:MenuItem>
                </asp:MenuItem>
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
                <tr>
                    <td style="width: 20%;">
                        <asp:Label runat="server" ID="label4" Text="Descrizione"></asp:Label>
                    </td>
                    <td>
                        <asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_descrizione"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="rigabottombig">
        </div>
        <div class="menuseparatore">
        </div>
        <div id="indicatori" runat="server" title="Indicatori">
            <asp:Panel runat="server" ID="panel1" GroupingText="Indicatori">
                <asp:GridView ID="grdIndicatori" runat="server" CellPadding="4" EmptyDataText=""
                    ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" Width="100%"
                    OnRowDataBound="grdIndicatori_RowDataBound">
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#0066FF" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#999999" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField HeaderText=" " Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblSelezioneId" runat="server" Text='<%# Bind("SelezioneId") %>' EnableViewState="True"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" " Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblCategoriaCod" runat="server" Text='<%# Bind("CategoriaCod") %>'
                                    EnableViewState="True"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" " Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblIndId" runat="server" Text='<%# Bind("Indid") %>' EnableViewState="True"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Descrizione" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblDescr" runat="server" Text='<%# Bind("Descr") %>' EnableViewState="True"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Dipendente" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_dip_flg" runat="server" Enabled="false" EnableViewState="true"
                                    Checked='<%# ConvertiFlg(int.Parse(Eval("DipFlg").ToString())) %>'></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Responsabile" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_dip_flg" runat="server" Enabled="false" EnableViewState="true"
                                    Checked='<%# ConvertiFlg(int.Parse(Eval("RspFlg").ToString())) %>'></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amministrazione" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_dip_flg" runat="server" Enabled="false" EnableViewState="true"
                                    Checked='<%# ConvertiFlg(int.Parse(Eval("AmmFlg").ToString())) %>'></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ordine" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblOrd" runat="server" Text='<%# Bind("Ord") %>' EnableViewState="True"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <input id="hdnRigaIndicatoreSel" type="hidden" value="" runat="server" name="hdnRigaIndicatoreSel" />
                <input id="hdnIndId" type="hidden" value="0" runat="server" name="hdnIndId" />
            </asp:Panel>
        </div>
        <div class="rigabottombig">
        </div>
        <div class="menuseparatore">
        </div>
        <div id="indicatoridet" runat="server" title="Dettaglio Indicatori">
            <asp:Panel runat="server" ID="panel2" GroupingText="Dettaglio Indicatori">
                <asp:GridView ID="grdIndicatoriDet" runat="server" CellPadding="4" EmptyDataText=""
                    ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" Width="100%"
                    OnRowDataBound="grdIndicatoriDeti_RowDataBound">
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#0066FF" Font-Bold="True" ForeColor="#333333" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <EditRowStyle BackColor="#999999" />
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:TemplateField HeaderText=" " Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblSelezioneId" runat="server" Text='<%# Bind("SelezioneId") %>' EnableViewState="True"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" " Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblCategoriaCod" runat="server" Text='<%# Bind("CategoriaCod") %>'
                                    EnableViewState="True"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" " Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblIndId" runat="server" Text='<%# Bind("Indid") %>' EnableViewState="True"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" " Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblIndDetId" runat="server" Text='<%# Bind("IndDetid") %>' EnableViewState="True"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Descrizione" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblDescr" runat="server" Text='<%# Bind("DescrDet") %>' EnableViewState="True"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ordine" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="lblOrd" runat="server" Text='<%# Bind("Ord") %>' EnableViewState="True"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <input id="hdnRigaIndicatoreDetSel" type="hidden" value="" runat="server" name="hdnRigaIndicatoreDetSel" />
                <input id="hdnIndDetId" type="hidden" value="0" runat="server" name="hdnIndDetId" />
            </asp:Panel>
        </div>
        <div class="rigabottombig">
        </div>
        <div class="menuseparatore">
        </div>
        <div>
            <asp:Button runat="server" ID="btnPopulate" Style="display: none" OnClick="btnPopulate_Click" />
        </div>
    </div>
</asp:Content>
