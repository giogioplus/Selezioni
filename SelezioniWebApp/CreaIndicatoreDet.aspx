<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.master" AutoEventWireup="true"
    CodeBehind="CreaIndicatoreDet.aspx.cs" Inherits="SelezioniWebApp.CreaIndicatoreDet"  %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_head_lnk" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var lastRowRigaSelected;
        var lastRowPuntSelected;
        var originalColor;

        function GridView_Riga_selectRow(row, RigaId, TipoRiga)
        {
            var hdnTipoRiga = document.getElementById("<%= hdnTipoRiga.ClientID %>");
            var hdnRiga = document.getElementById("<%= hdnRigaId.ClientID %>");
            var hdnR = document.getElementById("<%= hdnRigaIndicatoreDetRigaSel.ClientID %>");
            hdnTipoRiga.value = TipoRiga;
            hdnRiga.value = RigaId;
            hdnR.value = row.rowIndex;
            if (lastRowRigaSelected != row)
            {
                if (lastRowRigaSelected != null)
                {
                    lastRowRigaSelected.style.backgroundColor = originalColor;
                    lastRowRigaSelected.style.color = 'black'
                    lastRowRigaSelected.style.fontWeight = 'normal';
                }
                originalColor = row.style.backgroundColor
                row.style.backgroundColor = '#E2DED6'
                row.style.color = '#333333'
                row.style.fontWeight = 'normal'
                lastRowRigaSelected = row;
            }
        }

        function GridView_Punt_selectRow(row, PuntId)
        {
            var hdnPunt = document.getElementById("<%= hdnPuntId.ClientID %>");
            var hdnP = document.getElementById("<%= hdnRigaIndicatoreDetPuntSel.ClientID %>");
            hdnPunt.value = PuntId;
            hdnP.value = row.rowIndex;
            if (lastRowPuntSelected != row)
            {
                if (lastRowPuntSelected != null)
                {
                    lastRowPuntSelected.style.backgroundColor = originalColor;
                    lastRowPuntSelected.style.color = 'black'
                    lastRowPuntSelected.style.fontWeight = 'normal';
                }
                originalColor = row.style.backgroundColor
                row.style.backgroundColor = '#E2DED6'
                row.style.color = '#333333'
                row.style.fontWeight = 'normal'
                lastRowPuntSelected = row;
            }
        }

        function GridView_mouseHover(row)
        {
            row.style.cursor = 'hand';
        }

        function selRiga()
        {
            var hdnTipoRiga = document.getElementById("<%= hdnTipoRiga.ClientID %>");
            var hdnRiga = document.getElementById("<%= hdnRigaId.ClientID %>");
            var hdnR = document.getElementById("<%= hdnRigaIndicatoreDetRigaSel.ClientID %>");
            var tblQ = document.getElementById("<%= grdIndicatoriDetRiga.ClientID %>");
            if (hdnR.value != "")
            {
                GridView_Riga_selectRow(tblQ.rows[hdnR.value], hdnRiga.value, hdnTipoRiga.value);
            }

            var hdnPunt = document.getElementById("<%= hdnPuntId.ClientID %>");
            var hdnP = document.getElementById("<%= hdnRigaIndicatoreDetPuntSel.ClientID %>");
            var tblQP = document.getElementById("<%= grdIndicatoriDetPunt.ClientID %>");
            if (hdnP.value != "")
            {
                GridView_Punt_selectRow(tblQP.rows[hdnP.value], hdnPunt.value);
            }
        }

        function pulisciHdn()
        {
            var hdnTipoRiga = document.getElementById("<%= hdnTipoRiga.ClientID %>");
            var hdnRiga = document.getElementById("<%= hdnRigaId.ClientID %>");
            var hdnR = document.getElementById("<%= hdnRigaIndicatoreDetRigaSel.ClientID %>");
            hdnTipoRiga.value = "0";
            hdnRiga.value = "0";
            hdnR.value = "";

            var hdnPunt = document.getElementById("<%= hdnPuntId.ClientID %>");
            var hdnP = document.getElementById("<%= hdnRigaIndicatoreDetPuntSel.ClientID %>");
            hdnPunt.value = "0";
            hdnP.value = "";
        }

        onload = selRiga;
        onunload = pulisciHdn;
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="menusx">
        <h4 class="menusx">
            Definizione Indicatore</h4>
        <asp:Menu ID="mnu_CreaIndicatoreDet" runat="server" DynamicMenuStyle-CssClass="menuvoce"
            CssClass="menuvoce" OnMenuItemClick="mnu_CreaIndicatoreDet_MenuItemClick">
            <Items>
                <asp:MenuItem Text="Modifica Dettaglio" Value="ModDet"></asp:MenuItem>
                <asp:MenuItem Text="Righe" Selectable="false">
                    <asp:MenuItem Text="Inserisci riga" Value="InsRiga"></asp:MenuItem>
                    <asp:MenuItem Text="Modifica riga" Value="ModRiga"></asp:MenuItem>
                    <asp:MenuItem Text="Cancella riga" Value="DelRiga"></asp:MenuItem>
                </asp:MenuItem>
                <asp:MenuItem Text="Punteggi" Selectable="false">
                    <asp:MenuItem Text="Inserisci punteggio" Value="InsPunt"></asp:MenuItem>
                    <asp:MenuItem Text="Modifica punteggio" Value="ModPunt"></asp:MenuItem>
                    <asp:MenuItem Text="Cancella punteggio" Value="DelPunt"></asp:MenuItem>
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
                        <asp:Label runat="server" ID="label11" Text="Indicatore"></asp:Label>
                    </td>
                    <td>
                        <asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_descr"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="rigabottombig">
        </div>
        <div class="menuseparatore">
        </div>
        <div id="indicatoredet_creamod" runat="server" visible="true">
            <asp:Label CssClass="etichetta titolo" runat="server" ID="Label4" Text="Descrizione dettaglio (*)"></asp:Label>
            <asp:TextBox CssClass="etichetta" ID="txt_DescrDet" runat="server" MaxLength="255"
                TextMode="MultiLine" Width="100%" ValidationGroup="1"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RFV_txt_DescrDet" runat="server" ErrorMessage="Descrizione dettaglio OBBLIGATORIA"
                ControlToValidate="txt_DescrDet" Display="Dynamic" SetFocusOnError="True" Font-Italic="True"
                ForeColor="Red"></asp:RequiredFieldValidator>
            <asp:Label CssClass="etichetta titolo" runat="server" ID="Label5" Text="Note compilazione dettaglio"></asp:Label>
            <asp:TextBox CssClass="etichetta" ID="txt_NoteDetDip" runat="server" MaxLength="255"
                TextMode="MultiLine" Width="100%" ValidationGroup="1" ToolTip="Testo esplicativo che compare nel prospetto del dipendente"></asp:TextBox>
            <asp:Label CssClass="etichetta titolo" runat="server" ID="Label6" Text="Note valutazione dettaglio"></asp:Label>
            <asp:TextBox CssClass="etichetta" ID="txt_NoteDetVal" runat="server" MaxLength="255"
                TextMode="MultiLine" Width="100%" ValidationGroup="1" ToolTip="Testo esplicativo che compare nel prospetto di valutazione"></asp:TextBox>
            <asp:Label CssClass="etichetta titolo" runat="server" ID="Label7" Text="Ordine del dettaglio (*)"></asp:Label>
            <asp:TextBox CssClass="etichetta" ID="txt_Ord" runat="server" MaxLength="5" Width="5%"
                ValidationGroup="1" ToolTip="Ordinamento di comparsa del dettaglio nel modulo di compilazione o valutazione"></asp:TextBox>
            <asp:RegularExpressionValidator ID="REV_txt_Ord" ControlToValidate="txt_Ord" runat="server"
                ErrorMessage="Valore inserito non valido" ValidationExpression="^[1-9]+$" ForeColor="Red"></asp:RegularExpressionValidator>
            <asp:RequiredFieldValidator ID="RFV_txt_Ord" runat="server" ErrorMessage="Ordinamento OBBLIGATORIO"
                ControlToValidate="txt_Ord" Display="Dynamic" SetFocusOnError="True" Font-Italic="True"
                ForeColor="Red"></asp:RequiredFieldValidator>
            <asp:Label CssClass="etichetta titolo" runat="server" ID="Label8" Text="Numero massimo risposte (*)"></asp:Label>
            <asp:TextBox CssClass="etichetta" ID="txt_MaxRighe" runat="server" MaxLength="5"
                Width="5%" ValidationGroup="1" ToolTip="Numero massimo di risposte consentite / valutate"></asp:TextBox>
            <asp:RegularExpressionValidator ID="REV_txt_MaxRighe" ControlToValidate="txt_MaxRighe"
                runat="server" ErrorMessage="Valore inserito non valido" ValidationExpression="^[1-9]+$"
                ForeColor="Red"></asp:RegularExpressionValidator>
            <asp:RequiredFieldValidator ID="RFV_txt_MaxRighe" runat="server" ErrorMessage="Numero massimo di risposte OBBLIGATORIO"
                ControlToValidate="txt_MaxRighe" Display="Dynamic" SetFocusOnError="True" Font-Italic="True"
                ForeColor="Red"></asp:RequiredFieldValidator>
            <br />
            <asp:Button ID="btnConfermaIndDet" runat="server" Text="Conferma" CssClass="cerca"
                OnClick="btnConfermaIndDet_Click" />
            <asp:Button ID="btnAnnullaIndDet" runat="server" Text="Annulla" CssClass="cerca"
                OnClick="btnConfermaIndDet_Click" CausesValidation="False" />
        </div>
        <div class="rigabottombig">
        </div>
        <div class="menuseparatore">
        </div>
        <div id="indicatoredet_riga_punt_creamod">
            <table runat="server" style="width: 100%;" id="tbl_riga_punt" frame="border" border="10">
                <tr>
                    <td style="width: 53%;">
                        <div id="indicatoredet_riga_grd" runat="server">
                            <asp:GridView ID="grdIndicatoriDetRiga" runat="server" CellPadding="4" EmptyDataText=""
                                ForeColor="#333333" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%"
                                OnRowDataBound="grdIndicatoriDetRiga_RowDataBound">
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
                                    <asp:TemplateField HeaderText="Tipo riga">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTipoRiga" runat="server" Text='<%# Bind("DescrTipoRiga") %>' EnableViewState="True"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Riga" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRigaId" runat="server" Text='<%# Bind("RigaId") %>' EnableViewState="True"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ordinamento righe">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrdRiga" runat="server" Text='<%# Bind("OrdRiga") %>' EnableViewState="True"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tipo controllo">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTipoCtrl" runat="server" Text='<%# Bind("DescrTipoCtrl") %>' EnableViewState="True"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Descrizione" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescrRiga" runat="server" Text='<%# Bind("DescrRiga") %>' EnableViewState="True"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <input id="hdnRigaIndicatoreDetRigaSel" type="hidden" value="" runat="server" name="hdnRigaIndicatoreDetRigaSel" />
                            <input id="hdnTipoRiga" type="hidden" value="0" runat="server" name="hdnTipoRiga" />
                            <input id="hdnRigaId" type="hidden" value="0" runat="server" name="hdnRigaId" />
                        </div>
                    </td>
                    <td style="width: 47%;">
                        <div id="indicatoredet_punt_grd" runat="server">
                            <asp:GridView ID="grdIndicatoriDetPunt" runat="server" CellPadding="4" EmptyDataText=""
                                ForeColor="#333333" GridLines="Horizontal" AutoGenerateColumns="False" Width="100%"
                                OnRowDataBound="grdIndicatoriDetPunt_RowDataBound">
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
                                    <asp:TemplateField HeaderText="ID Punteggio">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPuntId" runat="server" Text='<%# Bind("PuntId") %>' EnableViewState="True"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Descrizione" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescrPunt" runat="server" Text='<%# Bind("DescrPunt") %>' EnableViewState="True"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Punteggio">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPunt" runat="server" Text='<%# Bind("Punt") %>' EnableViewState="True"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <input id="hdnRigaIndicatoreDetPuntSel" type="hidden" value="" runat="server" name="hdnRigaIndicatoreDetPuntSel" />
                            <input id="hdnPuntId" type="hidden" value="0" runat="server" name="hdnPuntId" />
                        </div>
                    </td>
                </tr>
            </table>
            <br />
            <div id="indicatoredet_riga_creamod" runat="server">
                <asp:Label CssClass="etichetta titolo" runat="server" ID="Label12" Text="Tipologia della riga (*)"></asp:Label>
                <asp:DropDownList ID="ddl_TipoRiga" runat="server" OnSelectedIndexChanged="ddl_TipoRiga_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:Label CssClass="etichetta titolo" runat="server" ID="Label13" Text="Tipologia del controllo da inserire"></asp:Label>
                <asp:DropDownList ID="ddl_TipoCtrl" runat="server" OnSelectedIndexChanged="ddl_TipoCtrl_SelectedIndexChanged">
                </asp:DropDownList>
                <br />
                <asp:Label CssClass="etichetta titolo" runat="server" ID="Label9" Text="Descrizione riga (*)"></asp:Label>
                <asp:TextBox CssClass="etichetta" ID="txt_DescrRiga" runat="server" MaxLength="255"
                    TextMode="MultiLine" Width="100%" ValidationGroup="1"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RFV_txt_DescrRiga" runat="server" ErrorMessage="Descrizione riga OBBLIGATORIA"
                    ControlToValidate="txt_DescrRiga" Display="Dynamic" SetFocusOnError="True" Font-Italic="True"
                    ForeColor="Red"></asp:RequiredFieldValidator>
                <asp:Label CssClass="etichetta titolo" runat="server" ID="Label10" Text="Ordine di comparsa della riga (*)"></asp:Label>
                <asp:TextBox CssClass="etichetta" ID="txt_OrdRiga" runat="server" MaxLength="5" Width="5%"
                    ValidationGroup="1" ToolTip="Ordinamento di comparsa della riga"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RFV_txt_OrdRiga" runat="server" ErrorMessage="Ordinamento riga OBBLIGATORIO"
                    ControlToValidate="txt_OrdRiga" Display="Dynamic" SetFocusOnError="True" Font-Italic="True"
                    ForeColor="Red"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="REV_txt_OrdRiga" ControlToValidate="txt_OrdRiga"
                    runat="server" ErrorMessage="Valore inserito non numerico" ValidationExpression="^\d+$"
                    ForeColor="Red"></asp:RegularExpressionValidator>
                <br />
                <br />
                <asp:Button ID="btnConfermaIndDetRiga" runat="server" Text="Conferma" CssClass="cerca"
                    OnClick="btnConfermaIndDetRiga_Click" />
                <asp:Button ID="btnAnnullaIndDetRiga" runat="server" Text="Annulla" CssClass="cerca"
                    OnClick="btnConfermaIndDetRiga_Click" CausesValidation="False" />
            </div>
            <div id="indicatoredet_punt_creamod" runat="server">
                <asp:Label CssClass="etichetta titolo" runat="server" ID="Label14" Text="Descrizione punteggio (*)"></asp:Label>
                <asp:TextBox CssClass="etichetta" ID="txt_DescrPunt" runat="server" MaxLength="255"
                    TextMode="MultiLine" Width="100%" ValidationGroup="1"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RFV_txt_DescrPunt" runat="server" ErrorMessage="Descrizione punteggio OBBLIGATORIO"
                    ControlToValidate="txt_DescrPunt" Display="Dynamic" SetFocusOnError="True" Font-Italic="True"
                    ForeColor="Red"></asp:RequiredFieldValidator>
                <asp:Label CssClass="etichetta titolo" runat="server" ID="Label15" Text="Punteggio"></asp:Label>
                <asp:TextBox CssClass="etichetta" ID="txt_Punt" runat="server" MaxLength="10" Width="10%"
                    ValidationGroup="1" ToolTip="Punteggio"></asp:TextBox>
                <asp:RegularExpressionValidator ID="REP_txt_Punt" ControlToValidate="txt_Punt" runat="server"
                    ErrorMessage="Valore inserito non valido" ValidationExpression="(^\d{0,3}(\,)\d{0,2})|(^\d{0,3})"
                    ForeColor="Red"></asp:RegularExpressionValidator>
                <br />
                <br />
                <asp:Button ID="btnConfermaIndDetPunt" runat="server" Text="Conferma" CssClass="cerca"
                    OnClick="btnConfermaIndDetPunt_Click" />
                <asp:Button ID="btnAnnullaIndDetPunt" runat="server" Text="Annulla" CssClass="cerca"
                    OnClick="btnConfermaIndDetPunt_Click" CausesValidation="False" />
            </div>
        </div>
        </div>
</asp:Content>
