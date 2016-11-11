<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.master" AutoEventWireup="true"
    CodeBehind="SelSelezione.aspx.cs" Inherits="SelezioniWebApp.SelSelezione" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_head_lnk" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var lastRowSelected;
        var originalColor;

        function GridView_selectRow(row, SelezioneId) {
            var hdn = document.getElementById("<%= hdnSelezioneId.ClientID %>");
            var hdnR = document.getElementById("<%= hdnRigaSelezioneSel.ClientID %>");
            hdn.value = SelezioneId;
            //            hdnR.value = row.rowIndex;
            if (hdnR != null) {
                if (hdnR.value != row.rowIndex) {
                    var flgGetRowData = '1';
                }
                hdnR.value = row.rowIndex;
            }
            if (lastRowSelected != row) {
                if (lastRowSelected != null) {
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
            if (flgGetRowData == '1') {
                flgGetRowData = '0';
                GetRowData(SelezioneId);
            }
        }

        function GridView_mouseHover(row) {
            row.style.cursor = 'hand';
        }

        function selRiga() {
            var hdnR = document.getElementById("<%= hdnRigaSelezioneSel.ClientID %>");
            var hdn = document.getElementById("<%= hdnSelezioneId.ClientID %>");
            var tblQ = document.getElementById("<%= grdSelezioni.ClientID %>");
            if (hdnR.value != "") {
                GridView_selectRow(tblQ.rows[hdnR.value], hdn.value);
            }
        }

        function pulisciHdn() {
            var hdnR = document.getElementById("<%= hdnRigaSelezioneSel.ClientID %>");
            var hdn = document.getElementById("<%= hdnSelezioneId.ClientID %>");
            hdn.value = "0";
            hdnR.value = "";
        }

        function GetRowData(index) {
            __doPostBack('<%=btnAttivaDisattivaMenu.UniqueID%>', index);
        }

        onload = selRiga;
        onunload = pulisciHdn;
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="menusx">
        <h4 class="menusx">
            Gestisci selezione
        </h4>
        <asp:Menu ID="mnu_main" runat="server" OnMenuItemClick="mnu_main_MenuItemClick" CssClass="menuvoce">
            <Items>
                <asp:MenuItem Text="Modifica selezione" Value="Mod"></asp:MenuItem>
                <asp:MenuItem Text="Duplica" Value="Dup"></asp:MenuItem>
                <asp:MenuItem Text="Cancella" Value="Del"></asp:MenuItem>
                <asp:MenuItem Text="Autorizza" Value="Aut" ToolTip="Scegli la popolazione a cui è rivolta la selezione"
                    Enabled="false"></asp:MenuItem>
                <asp:MenuItem Text="Stampa" Value="Stm"></asp:MenuItem>
                <asp:MenuItem Text="Pubblica/In bozza" Value="Pub" ToolTip="In bozza = selezione in elaborazione / Pubblicato = selezione attiva per la compilazione">
                </asp:MenuItem>
                <asp:MenuItem Text="Chiudi/Pubblica" Value="Chd" ToolTip="Chiudi = Selezione non più attiva / Pubblica = selezione riattivata">
                </asp:MenuItem>
                <%--<asp:MenuItem Text="Estrai Elenco" Value="Exc" ToolTip="Estrai file excel con punteggi">
                </asp:MenuItem>--%>
                <asp:MenuItem Text="Archivia selezione" Value="Arc"></asp:MenuItem>
                <asp:MenuItem Text="Ritorna" Value="Rit"></asp:MenuItem>
            </Items>
        </asp:Menu>
    </div>
    <div id="contenuto">
        <asp:Label ID="lblErr" runat="server" Text=""></asp:Label>
        <div id="Filtro">
            <br />
            <asp:Label ID="lbl_filtro_stato" runat="server" Text="Scegli lo stato"></asp:Label>
            <asp:DropDownList ID="ddl_filtro_stato" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_filtro_stato_SelectedIndexChanged">
            </asp:DropDownList>
            <asp:Label ID="lbl_filtro_anno" runat="server" Text="Scegli l'anno"></asp:Label>
            <asp:DropDownList ID="ddl_filtro_anno" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_filtro_anno_SelectedIndexChanged">
            </asp:DropDownList>
            <asp:Label ID="lbl_filtro_categoria" runat="server" Text="Scegli la categoria"></asp:Label>
            <asp:DropDownList ID="ddl_filtro_categoria" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_filtro_categoria_SelectedIndexChanged">
            </asp:DropDownList>
            <asp:Button ID="btn_filtro" runat="server" Text="Esegui filtri" OnClick="btnfiltro_Click" />
            <br />
            <br />
            <br />
        </div>
        <div id="divgrdSelezioni" runat="server" visible="true">
            <asp:GridView ID="grdSelezioni" runat="server" EmptyDataText="" ForeColor="#333333"
                GridLines="None" AutoGenerateColumns="False" Width="100%" OnRowDataBound="grdSelezioni_RowDataBound">
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#0066FF" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:TemplateField HeaderText=" ">
                        <ItemTemplate>
                            <asp:Label ID="lblSelezioneId" runat="server" Text='<%# Bind("SelezioneId") %>' EnableViewState="True"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Categoria" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_categoria" runat="server" Text='<%# Bind("CategoriaCod") %>' EnableViewState="True"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Anno" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_anno" runat="server" Text='<%# Bind("Anno") %>' EnableViewState="True"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Titolo" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_titolo" runat="server" Text='<%# Bind("Titolo") %>' EnableViewState="True"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Data inizio validità" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_data_iniz_val" runat="server" Text='<%# ConvertiData(DateTime.Parse(Eval("DataInizVal").ToString()))%>'
                                EnableViewState="True"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Data fine validità" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_data_fine_val" runat="server" Text='<%# ConvertiData(DateTime.Parse(Eval("DataFineVal").ToString()))%>'
                                EnableViewState="True"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Stato" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lbl_descr_stato" runat="server" Text='<%# Bind("DescrStato")%>' EnableViewState="True"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:Label ID="lblselezioneId" Text="" runat="server" />
            <input id="hdnRigaSelezioneSel" type="hidden" value="" runat="server" name="hdnRigaSelezioneSel" />
            <input id="hdnSelezioneId" type="hidden" value="0" runat="server" name="hdnSelezioneId" />
            <div>
                <asp:Button runat="server" ID="btnAttivaDisattivaMenu" Style="display: none" OnClick="btnAttivaDisattivaMenu_Click" />
            </div>
        </div>
    </div>
</asp:Content>
