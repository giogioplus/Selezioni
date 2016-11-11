<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.master" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="SelezioniWebApp.Upload" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head_lnk" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="menusx">
		<h4 class="menusx">
			UploadFile
		</h4>
		<asp:Menu ID="mnu_main" runat="server" OnMenuItemClick="mnu_main_MenuItemClick" CssClass="menuvoce">
			<Items>
                <asp:MenuItem Text="Elimina file" Value="Del"></asp:MenuItem>
				<asp:MenuItem Text="Carica file" Value="Upl"></asp:MenuItem>
				<asp:MenuItem Text="Ritorna" Value="Esc"></asp:MenuItem>
			</Items>
		</asp:Menu>
        </div>
        <div id="testata" runat="server">
        <br/><br/>
        <asp:Label ID="lbl_messaggi" runat="server" class="etichetta"  Text=""></asp:Label>
        <asp:Label ID="lbl_elencofile" runat="server" class="etichetta"  Text=""></asp:Label>
        <br/><br/>
        <asp:Label ID="lbl_deletefile" class="etichetta" Text="Inserisci il nome del file da eliminare" runat="server" ></asp:Label>
        <asp:TextBox class="etichetta" ID="txt_ins_deletefile" runat="server" MaxLength="100"></asp:TextBox>
		<br/><br/>
        <asp:Label ID="lbl_uploadfile"  Text="Seleziona il file da caricare" runat="server" class="etichetta"></asp:Label>
        <asp:FileUpload runat="server" ID="fuFile" />
        </div>
</asp:Content>
