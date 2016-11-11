<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.master" AutoEventWireup="true" CodeBehind="IstruzioniDettaglio.aspx.cs" Inherits="SelezioniWebApp.IstruzioniDettaglio" %>
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
                <asp:MenuItem Text="Ritorna" Value="Esc"></asp:MenuItem>
            </Items>
        </asp:Menu>
    </div>
     <div>
     <br />
     <br />
        <asp:Label ID="LblErrDefault" CssClass="etichetta titolo" runat="server" Text=""
            ForeColor="Red"></asp:Label>
        <br />
    </div>
    
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:DetailsView ID="dtv_dettaglio" runat="server" Height="70px" ForeColor="#333333"
        DefaultMode="readonly" AutoGenerateRows="False" AutoGenerateEditButton="True"
        AutoGenerateInsertButton="True" HeaderStyle-HorizontalAlign="Center" FieldHeaderStyle-HorizontalAlign="NotSet"
        OnItemCommand="dtv_dettaglio_ItemCommand" OnModeChanging="dtv_dettaglio_ModeChanging1"
        OnItemUpdating="dtv_dettaglio_ItemUpdating" OnItemInserting="dtv_dettaglio_ItemInserting"
        OnDataBound="dtv_dettaglio_DataBound" OnItemInserted="dtv_dettaglio_ItemInserted"
        Width="600px" >
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" VerticalAlign="Top" Width="600px" />
        <HeaderStyle Font-Bold="True" />
        <FieldHeaderStyle BackColor="#E9ECF1" Font-Bold="True"  Wrap="false"/>
        <Fields>
            <asp:TemplateField HeaderText="Identificativo file" >
                <ItemTemplate>
                    <asp:Label ID="lbl_id" runat="server" Text='<%# Bind("id") %>' Visible="true"></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:Label ID="lbl_upd_id" runat="server" Text='<%# Bind("id") %>' ></asp:Label>
                </EditItemTemplate>
                <%--<InsertItemTemplate>
                    <asp:Label ID="lbl_ins_id" runat="server" Visible="true"></asp:Label>
                </InsertItemTemplate>--%>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Codice Categoria">
                <ItemTemplate>
                    <asp:Label ID="lbl_categoria_cod" runat="server" Text='<%# Bind("categoria_cod")%>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="ddl_upd_categoria_cod" runat="server">
                    </asp:DropDownList>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:DropDownList ID="ddl_ins_categoria_cod" runat="server">
                    </asp:DropDownList>
                </InsertItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Anno">
                <ItemTemplate>
                    <asp:Label ID="lbl_anno" runat="server" Text='<%# Bind("anno")%>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txt_anno" runat="server" Text='<%# Bind("anno")%>'></asp:TextBox>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox ID="ins_anno" runat="server"></asp:TextBox>
                </InsertItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Descrizione">
                <ItemTemplate>
                    <asp:Label ID="lbl_descrizione" runat="server" Text='<%# Bind("descrizione")%>'  Wrap="true" TextMode="MultiLine" Width="500"></asp:Label>
                </ItemTemplate>
                 <EditItemTemplate>
                    <asp:TextBox ID="txt_descrizione" runat="server" Text='<%# Bind("descrizione")%>'
                        Wrap="true" TextMode="MultiLine" Width="500" Height="120"></asp:TextBox>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox ID="ins_descrizione" runat="server"  Wrap="true" TextMode="MultiLine" Width="500" Height="120"></asp:TextBox>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Titolo">
                <ItemTemplate>
                    <asp:Label ID="lbl_titolo" runat="server" Text='<%# Bind("titolo")%>'></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txt_titolo" runat="server" Text='<%# Bind("titolo")%>'></asp:TextBox>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox ID="ins_titolo" runat="server" ></asp:TextBox>
                </InsertItemTemplate>
 
            </asp:TemplateField>
             <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label ID="lblVis" runat="server" Text="File Allegato" Visible="true"></asp:Label>
                    <asp:Label ID="lblIns" runat="server" Text="Inserire Allegato" Visible="false"></asp:Label>
                    <%--<asp:Label ID="lblUpd" runat="server" Text="Sostituire Allegato" Visible="false"></asp:Label>--%>
                    <asp:Label ID="lblUpd" runat="server" Text="Allegato presente" Visible="false"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:ImageButton ID="ImageButton1" CommandName="Visualizza" ImageUrl="Images/S_B_UPLO.gif"
                        runat="server"></asp:ImageButton>
                         
                </ItemTemplate>
                <ItemTemplate>
                    <asp:Label ID="lbl_blb" runat="server" Text='<%# Bind("titolo_file")%>'></asp:Label>
                </ItemTemplate>
                <InsertItemTemplate>
                    <asp:FileUpload ID="blb_ins" runat="server" Width="500" />
                </InsertItemTemplate>
                <EditItemTemplate>
                    <asp:Label ID="lbl_titolo_file" runat="server" Text='<%# Bind("titolo_file")%>'></asp:Label>
                    <asp:FileUpload ID="blb_upd" runat="server" Width="500" ></asp:FileUpload>
                    <asp:CheckBox ID="blb_chb_del" runat="server" Text="Cancella" Visible="false"></asp:CheckBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="bytes_d" Visible="false">
            <ItemTemplate>
                    <asp:Label ID="lbl_bytes_d" runat="server" Text='<%# Bind("bytes")%>'></asp:Label>
                </ItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox ID="blb_ins_bytes_d" Text='<%# Bind("bytes") %>' runat="server"></asp:TextBox>
                </InsertItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="blb_upd_bytes_d" Text='<%# Bind("bytes") %>' runat="server"></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ext_d" Visible="false">
             <ItemTemplate>
                    <asp:Label ID="lbl_ext_d" runat="server" Text='<%# Bind("ext")%>'></asp:Label>
                </ItemTemplate>
                <InsertItemTemplate>
                    <asp:TextBox ID="blb_ins_ext_d" Text='<%# Bind("ext") %>' runat="server"></asp:TextBox>
                </InsertItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="blb_upd_ext_d" Text='<%# Bind("ext") %>' runat="server"></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
               </Fields>
    </asp:DetailsView>
  
    <div style="width: 435px; height: 306px;">
        <cc2:myModalDialog ID="MyMdlDlg" OkButtonText="Ok" runat="server" HeaderText="Attenzione"
            height="100" width="600" posOriz="60" posVert="200" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
</asp:Content>
