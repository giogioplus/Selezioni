<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.master" AutoEventWireup="true"
    CodeBehind="Compilazione.aspx.cs" Inherits="SelezioniWebApp.Compilazione"%>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_head_lnk" runat="server">
    <link href="App_Themes/SkinFile/cssSelezioni.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="menusx">
        <h4 class="menusx">
            Compilazione scheda Selezione</h4>
        <asp:Menu ID="mnu_CompilaSelezione" runat="server" DynamicMenuStyle-CssClass="menuvoce"
            CssClass="menuvoce" OnMenuItemClick="mnu_CompilaSelezione_MenuItemClick">
            <Items>
                <asp:MenuItem Text="Stampa la domanda" Value="Stm"></asp:MenuItem>
                <asp:MenuItem Text="Trasmetti la domanda all'ufficio" Value="Chd"></asp:MenuItem>
				<asp:MenuItem Text="Presentazione" Value="Faq"></asp:MenuItem>
                <asp:MenuItem Text="Richiesta Revisione Punteggi" Value="RicRev"></asp:MenuItem>
                <asp:MenuItem Text="Archivio" Value="Arc"></asp:MenuItem>
                <asp:MenuItem Text="Esci dalla procedura" Value="Rit"></asp:MenuItem>
            </Items>
        </asp:Menu>
    </div>
    <div id="contenuto">
        <div id="testata" runat="server">
            <asp:Label ID="lblErr" runat="server"></asp:Label>
            <table runat="server" id="tbl_testata">
                <tr>
                    <td style="width: 20%;">
                        <asp:Label runat="server" ID="label3" Text="Titolo:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_titolo"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%;">
                        <asp:Label runat="server" ID="label18" Text="Descrizione:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label CssClass="descrizione" runat="server" ID="lbl_descrizione"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%;">
                        <asp:Label runat="server" ID="label2" Text="Anno:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_anno"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%;">
                        <asp:Label runat="server" ID="label1" Text="Categoria"></asp:Label>
                    </td>
                    <td>
                        <asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_categoriacod"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%;">
                        <asp:Label runat="server" ID="label11" Text="Stato della domanda"></asp:Label>
                    </td>
                    <td>
                        <asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_descrstato"></asp:Label>
                    </td>
                </tr>
            </table>
            <table runat="server" id="tbl_date" 
                style="border-style: solid none none none; border-top-width: thin;"  width="100%">
                <tr>
                    <td style="width: 40%;">
                        <asp:Label ID="Label17" runat="server" Text="Termini compilazione domande: "></asp:Label>
                    </td>
                    <td style="vertical-align:bottom">
                        <asp:Label ID="lbl_inizval_datatermpres" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label14" runat="server" Text="Termine controllo e valutazione Ufficio Relazioni Sindacali: "></asp:Label>
                    </td>
                    <td style="vertical-align:bottom">
                        <asp:Label ID="lbl_datatermcontrolloamm" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label12" runat="server" Text="Termine valutazione Responsabile: "></asp:Label>
                    </td>
                    <td style="vertical-align:bottom">
                        <asp:Label ID="lbl_datatermvalresp" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label15" runat="server" Text="Termine controllo Dipendente: "></asp:Label>
                    </td>
                    <td style="vertical-align:bottom">
                        <asp:Label ID="lbl_datatermcontrollodip" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="Label16" runat="server" Text="Termine valutazione finale Ufficio Relazioni Sindacali: "></asp:Label>
                    </td>
                    <td style="vertical-align:bottom">
                        <asp:Label ID="lbl_datatermvalamm" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="rigabottombig" runat="server">
        </div>
        <div id="anagrafica" runat="server">
            <table runat="server" id="tbl_anagrafica">
                <tr>
                    <td style="width: 25%;">
                        <asp:Label runat="server" ID="label4" Text="Nome Cognome:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbl_NomeCognome"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%;">
                        <asp:Label runat="server" ID="label5" Text="Nascita:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbl_Nascita"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%;">
                        <asp:Label runat="server" ID="label6" Text="Residenza:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbl_Residenza"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%;">
                        <asp:Label runat="server" ID="label10" Text="Categoria:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbl_Categoria"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%;">
                        <asp:Label runat="server" ID="label7" Text="Afferenza:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbl_Afferenza"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%;">
                        <asp:Label runat="server" ID="label8" Text="Tel interno:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbl_TelInterno"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%;">
                        <asp:Label runat="server" ID="label9" Text="E-Mail:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbl_Email"></asp:Label>
                    </td>
                </tr>
				<tr>
                    <td style="width: 25%;">
                        <asp:Label runat="server" ID="label13" Text="Valutatore:"></asp:Label>
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lbl_Valutatore"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="menuseparatore">
        </div>
        <div id="dettaglio" runat="server">
            <asp:GridView ID="grdDomande" runat="server" CellPadding="4" EmptyDataText="" ForeColor="#333333"
                GridLines="None" AutoGenerateColumns="False" Width="100%" OnRowDataBound="grdDomande_RowDataBound"
                OnRowCommand="grdDomande_RowCommand">
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#0066FF" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:TemplateField ShowHeader="false">
                        <ItemStyle HorizontalAlign="Center" Width="30px" />
                        <ItemTemplate>
                            <asp:ImageButton ID="btnSel" runat="server" CausesValidation="False" CommandName="Sel"
                                ImageUrl="" CommandArgument="<%# Container.DataItemIndex %>"
                                ToolTip="Seleziona dettaglio indicatore" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="false">
                        <ItemStyle HorizontalAlign="Center" Width="30px"/>
                        <ItemTemplate>
                            <asp:ImageButton ID="btnDel" runat="server" CausesValidation="False" CommandName="Del"
                                ImageUrl="~/Images/S_B_DELE.gif" CommandArgument="<%# Container.DataItemIndex %>"
                                ToolTip="Cancella risposte" Visible="false" />
                        </ItemTemplate>
                    </asp:TemplateField>
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
                    <asp:TemplateField HeaderText=" " Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblCompId" runat="server" Text='<%# Bind("Compid") %>' EnableViewState="True"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Indicatore" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblDescr" runat="server" Text='<%# Bind("DescrInd") %>' EnableViewState="True"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dettaglio" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblDescrDet" runat="server" Text='<%# Bind("DescrDet") %>' EnableViewState="True"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Risposte compilate" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkFlRisposte" Checked='<%# Eval("FlRisposte") %>' Enabled="false"
                                runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Valutazione" HeaderStyle-HorizontalAlign="Left">
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:Label ID="lblPunteggio" runat="server" Text='<%# Bind("Punteggio") %>' EnableViewState="True"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
        </div>
    </div>
</asp:Content>
