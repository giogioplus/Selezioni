﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.master" AutoEventWireup="true" CodeBehind="Archivio.aspx.cs" Inherits="SelezioniWebApp.Archivio" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head_lnk" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="menusx">
        <h4 class="menusx">
            Compilazione scheda Selezione</h4>
        <asp:Menu ID="mnu_Archivio" runat="server" DynamicMenuStyle-CssClass="menuvoce"
            CssClass="menuvoce" OnMenuItemClick="mnu_Archivio_MenuItemClick">
            <Items>
                 <asp:MenuItem Text="Ritorna" Value="Rit"></asp:MenuItem>
            </Items>
        </asp:Menu>
    </div>
    <div id="contenuto">
        <asp:Label ID="lblErr" runat="server" Text=""></asp:Label>
        <div id="Selezioni">
            <asp:Label ID="lblParametri" Text="Selezionare Parametri" runat="server" CssClass="etichetta"
                Font-Bold="True"></asp:Label>
            <div class="riga">
            </div>
            <table style="width: 1168px; margin-right: 76px" runat="server" id="tblParametri">
                <tr>
                    <td class="etichetta" style="width: 154px">
                        <asp:Label ID="lblTipoSelezione" runat="server" Text="Tipo selezione (*)" CssClass="etichetta"></asp:Label>
                    </td>
                    <td class="etichetta" style="width: 803px">
                        <asp:DropDownList ID="ddlTipoSelezione" runat="server" AutoPostBack="true" Width="250px"
                            OnSelectedIndexChanged="ddlTipoSelezione_SelectedIndexChanged" Style="margin-left: 0px;
                            margin-right: 0px" Height="25px">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="etichetta" style="width: 154px">
                        <asp:Label ID="lblAnno" runat="server" Text="Anno selezione" CssClass="etichetta"></asp:Label>
                    </td>
                    <td class="etichetta" style="width: 803px">
                        <asp:DropDownList ID="ddlAnno" runat="server" AutoPostBack="true" Width="148px" OnSelectedIndexChanged="ddlAnno_SelectedIndexChanged"
                            Style="margin-left: 0px; margin-right: 0px" Height="25px">
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
        <div id="Command">
            <br />
            <table id="tblCmd" runat="server" class="allineacellehtmlnobord">
                <tr>
                    <td class="style1">
                        <asp:Button ID="btnCerca" runat="server" CssClass="etichetta" Text="Carica selezioni"
                            OnClick="btnCerca_Click" />
                    </td>
                    <td class="style2">
                        <asp:Button ID="btnAnnulla" runat="server" CssClass="etichetta" Text="Annulla parametri di selezione"
                            OnClick="btnAnnulla_Click"  />
                    </td>
                </tr>
            </table>
        </div>
        <div id="dettaglio" runat="server">
            <asp:GridView ID="grdArchivio" runat="server" CellPadding="4" EmptyDataText="Non ci sono selezioni"
                ForeColor="#333333" GridLines="None" AutoGenerateColumns="False" Width="100%"
                OnRowDataBound="grdArchivio_RowDataBound" OnRowCommand="grdArchivio_RowCommand"
                Visible="false" AllowSorting="True"
			    OnSorting="grdArchivio_Sorting">
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#0066FF" Font-Bold="True" ForeColor="#333333" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <EditRowStyle BackColor="#999999" />
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:TemplateField ShowHeader="false">
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:ImageButton ID="btnSel" runat="server" CausesValidation="False" CommandName="Sel"
                                ImageUrl="~/Images/S_B_DPCH.gif" CommandArgument="<%# Container.DataItemIndex %>"
                                ToolTip="Seleziona Domanda" Enabled='<%# ConvertiStato(int.Parse(Eval("stato").ToString()))%>'
                                Visible ='<%# ConvertiStato(int.Parse(Eval("stato").ToString()))%>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Anno" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblAnno" runat="server" Text='<%# Bind("anno") %>' EnableViewState="True"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Categoria-Livello" HeaderStyle-HorizontalAlign="Left" SortExpression="inquadr_dip">
                        <ItemTemplate>
                            <asp:Label ID="lblInquadramento" runat="server" Text='<%# Bind("inquadr_dip") %>'
                                EnableViewState="True"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Afferenza" HeaderStyle-HorizontalAlign="Left" SortExpression="aff_org_dip">
                        <ItemTemplate>
                            <asp:Label ID="lblAfferenza" runat="server" Text='<%# Bind("aff_org_dip") %>' EnableViewState="True" ></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Matricola" HeaderStyle-HorizontalAlign="Left" SortExpression="matri_dip">
                        <ItemTemplate>
                            <asp:Label ID="lblMatri" runat="server" Text='<%# Bind("matri_dip") %>' EnableViewState="True"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cognome" HeaderStyle-HorizontalAlign="Left" SortExpression="cognome_dip">
                        <ItemTemplate>
                            <asp:Label ID="lblCognome" runat="server" Text='<%# Bind("cognome_dip") %>' EnableViewState="True"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Nome" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblNome" runat="server" Text='<%# Bind("nome_dip") %>' EnableViewState="True"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Stato domanda" HeaderStyle-HorizontalAlign="Left" SortExpression="stato">
                        <ItemTemplate>
                                  <asp:Label ID="lblStato" runat="server" Text='<%# Bind("descstato") %>'
                                EnableViewState="True"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Compilazione" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblCompilazione" runat="server" Text='<%# Bind("comp_id") %>' EnableViewState="True"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Selezione" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblSelezione" runat="server" Text='<%# Bind("selezione_id") %>' EnableViewState="True"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView  >
        </div>
    </div>
</asp:Content>
