<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.master" AutoEventWireup="true"
    CodeBehind="Gestione.aspx.cs" Inherits="SelezioniWebApp.Gestione" %>
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_head_lnk" runat="server">
    <link href="App_Themes/SkinFile/cssSelezioni.css" rel="Stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<cc1:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
	</cc1:ToolkitScriptManager>
    <div id="menusx">
		<h4 class="menusx">
			Gestione Selezioni
		</h4>
		<asp:Menu ID="mnu_main" runat="server" OnMenuItemClick="mnu_main_MenuItemClick" CssClass="menuvoce">
			<Items>
				<asp:MenuItem Text="Crea nuova" Value="Add"></asp:MenuItem>
				<asp:MenuItem Text="Gestisci selezione" Value="Sel"></asp:MenuItem>
                <asp:MenuItem Text="Gestisci file" Value="Upl"></asp:MenuItem>
              	<asp:MenuItem Text="Esci" Value="Esc"></asp:MenuItem>
			</Items>
		</asp:Menu>
        
        
        	<div>
			<asp:Label ID="Label1" runat="server" Text="" CssClass="etichetta titolo"></asp:Label>
		</div>
		<div id="testata" runat="server">
			<div id="testata_lettura" runat="server" visible="true">
				<asp:Label ID="lbl_rdr_titolo" runat="server" Font-Bold="True" Font-Size="Medium"></asp:Label>
				<br/><br/>
				<asp:Label ID="lbl_stato" runat="server" Font-Bold="false" Font-Size="Small" Font-Italic="false"></asp:Label>
				<asp:Label ID="lbl_rdr_descr_stato" runat="server" Font-Bold="True" Font-Size="Small" Font-Italic="true"></asp:Label>
			</div>
			<div id="testata_creamod" runat="server" visible="false">
				<asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_titolo" Text="Titolo"></asp:Label>
				<asp:TextBox CssClass="etichetta" ID="txt_ins_titolo" runat="server" MaxLength="255"
					TextMode="MultiLine" Width="100%" ValidationGroup="1"></asp:TextBox>
				<asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_descrizione" Text="Descrizione"></asp:Label>
				<asp:TextBox CssClass="etichetta" ID="txt_ins_descrizione" runat="server" MaxLength="2000"
					TextMode="MultiLine" Width="100%"></asp:TextBox>
				<asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_data_iniz_val" Text="Data inizio validità"></asp:Label>
				<br />
				<asp:TextBox CssClass="etichetta" ID="txt_ins_data_iniz_val" runat="server" MaxLength="15"
					Columns="15"></asp:TextBox>
				<asp:ImageButton ID="imb_ins_data_iniz_val" ImageUrl="~/Images/Calendar_scheduleHS.png"
					runat="server" />
                    <cc1:MaskedEditExtender ID="mee_ins_data_iniz_val" TargetControlID="txt_ins_data_iniz_val"
                    runat="server" MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="true"
					ClipboardEnabled="true" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
					OnInvalidCssClass="MaskedEditError">
                    </cc1:MaskedEditExtender>
				
				<cc1:MaskedEditValidator ID="mev_ins_data_iniz_val" ControlExtender="mee_ins_data_iniz_val"
					ControlToValidate="txt_ins_data_iniz_val" IsValidEmpty="true" TooltipMessage="Inserire una data"
					InvalidValueMessage="Data non valida" InvalidValueBlurredMessage="Data non valida"
					ValidationExpression="" runat="server"></cc1:MaskedEditValidator>
				<cc1:CalendarExtender ID="cae_ins_data_iniz_val" TargetControlID="txt_ins_data_iniz_val"
					Format="dd/MM/yyyy" PopupButtonID="imb_ins_data_iniz_val" runat="server" CssClass="MyCalendar">
				</cc1:CalendarExtender>
				<br />
				<asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_data_fine_val" Text="Data fine validità"></asp:Label>
				<br />
				<asp:TextBox CssClass="etichetta" ID="txt_ins_data_fine_val" runat="server" MaxLength="15"
					Columns="15"></asp:TextBox>
				<asp:ImageButton ID="imb_ins_data_fine_val" ImageUrl="~/Images/Calendar_scheduleHS.png"
					runat="server" />
				<cc1:MaskedEditExtender ID="mee_ins_data_fine_val" TargetControlID="txt_ins_data_fine_val"
					runat="server" MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="true"
					ClipboardEnabled="true" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
					OnInvalidCssClass="MaskedEditError">
				</cc1:MaskedEditExtender>
				<cc1:MaskedEditValidator ID="mev_ins_data_fine_val" ControlExtender="mee_ins_data_fine_val"
					ControlToValidate="txt_ins_data_fine_val" IsValidEmpty="true" TooltipMessage="Inserire una data"
					InvalidValueMessage="Data non valida" InvalidValueBlurredMessage="Data non valida"
					ValidationExpression="" runat="server"></cc1:MaskedEditValidator>
				<cc1:CalendarExtender ID="cae_ins_data_fine_val" TargetControlID="txt_ins_data_fine_val"
					Format="dd/MM/yyyy" PopupButtonID="imb_ins_data_fine_val" runat="server" CssClass="MyCalendar">
				</cc1:CalendarExtender>
				<br />
				<asp:CheckBox ID="chk_ins_flg_sezioni" Text="Marcare se sono previste delle sezioni"
					runat="server" />
				<br />
				<br />
			</div>
		</div>
	</div>
    
    <div>
    <asp:Label ID="lblErr" runat="server" Text=""></asp:Label>
    </div>
</asp:Content>
