<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.master" AutoEventWireup="true" CodeBehind="Istruzioni.aspx.cs" Inherits="SelezioniWebApp.Istruzioni" %>
<%@ Register Assembly="MyControls" Namespace="MyControls" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head_lnk" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <div id="menusx">
        <h4 class="menusx">
            Gestione File
        </h4>
        <asp:Menu ID="mnu_main" runat="server" OnMenuItemClick="mnu_main_MenuItemClick" CssClass="menuvoce">
            <Items>
                <asp:MenuItem Text="Carica nuovo file" Value="New"></asp:MenuItem>
                <asp:MenuItem Text="Ritorna" Value="Esc"></asp:MenuItem>
            </Items>
        </asp:Menu>
    </div>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <script type="text/javascript" src="js/jquery-1.4.4.js"></script>
    <script type="text/javascript">
        function displaymessage(messaggio) {
            alert(messaggio);
        }
    </script>
    <div>
        
        <h2 title="Gestione File" >
            Gestione file
        </h2>
        <%--<div class="rigabottombig">
        </div>--%>
    </div>
    <div>
        <asp:Label ID="LblErrDefault" CssClass="etichetta titolo" runat="server" Text=""
            ForeColor="Red"></asp:Label>
        <br />
    </div>
    <div>
        <asp:GridView ID="dtv_dettaglio" runat="server" AutoGenerateColumns="False" AllowSorting="True"
            OnSorting="dtv_dettaglio_Sorting" EmptyDataText="Non ci sono file Istruzioni" OnRowCommand="dtv_dettaglio_RowCommand" >
            <Columns>
                <asp:TemplateField ShowHeader="false">
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <asp:HyperLink ID="hyp_id_sel" runat="server" NavigateUrl='<%#RichiamaDettaglio(int.Parse(Eval("id").ToString())) %>'>
                            <asp:Image ID="lbl_Select" runat="server" ImageUrl="~/Images/S_B_DPCH.gif" ToolTip="Seleziona/Aggiorna">
                            </asp:Image>
                        </asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="false">
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemTemplate>
                        <%--<asp:ImageButton ID="btnCancRec" runat="server" CausesValidation="False" CommandName="Canc"
                            ImageUrl="~/Images/S_B_DELE.gif" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"id") %>'
                            ToolTip="Cancella" />--%>
                            <asp:ImageButton ID="btnCancRec" runat="server" CausesValidation="False" CommandName="Canc"
                            ImageUrl="~/Images/S_B_DELE.gif" CommandArgument='<%#DataBinder.Eval(Container.DataItem,"id") %>'
                            ToolTip="Cancella" />
                            
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Identificativo file" Visible="true">
                    <ItemTemplate>
                        <asp:Label ID="lbl_id" runat="server" Text='<%# Bind("id")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Anno" SortExpression="anno">
                    <ItemTemplate>
                        <asp:HyperLink ID="hyp_id" runat="server" NavigateUrl='<%#RichiamaDettaglio(int.Parse(Eval("id").ToString())) %>'>
                            <asp:Label ID="lbl_anno" runat="server" class="etichetta" Text='<%# Bind("anno")%>'
                                Width="150"></asp:Label>
                        </asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Categoria" SortExpression="categoria">
                    <ItemTemplate>
                        <asp:Label ID="lbl_categoria_cod" runat="server" class="etichetta" Text='<%# Bind("categoria_cod")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Titolo" SortExpression="titolo">
                    <ItemTemplate>
                        <asp:Label ID="lbl_titolo" runat="server" class="etichetta" Text='<%# Bind("titolo")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tipo File">
                    <ItemTemplate>
                        <asp:Label ID="lbl_istruzioni" runat="server" class="etichetta" Visible="false" Text='<%# Bind("istruzioni")%>'></asp:Label>
                        <asp:Label ID="lbl_ext" runat="server" class="etichetta" Text='<%# Bind("ext")%>'></asp:Label>
                        <asp:Label ID="lbl_bytes" runat="server" class="etichetta" Visible="false" Text='<%# Bind("bytes")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Descrizione" SortExpression="descrizione">
                    <ItemTemplate>
                        <asp:Label ID="lbl_descrizione" runat="server" class="etichetta" Text='<%# Bind("descrizione")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div style="width: 435px; height: 306px;">
        <cc2:myModalDialog ID="MyMdlDlg" OkButtonText="Ok" runat="server" HeaderText="Attenzione"
            height="100" width="600" posOriz="60" posVert="200" />
    </div>
    <div id="MessaggioConf" style="width: 435px; height: 306px;" runat="server">
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>