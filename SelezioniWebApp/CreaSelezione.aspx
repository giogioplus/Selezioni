<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.master" AutoEventWireup="true"
    CodeBehind="CreaSelezione.aspx.cs" Inherits="SelezioniWebApp.CreaSelezione" %>

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
            Gestione selezioni</h4>
        <asp:Menu ID="mnu_CreaSelezione" runat="server" DynamicMenuStyle-CssClass="menuvoce"
            CssClass="menuvoce" OnMenuItemClick="mnu_CreaSelezione_MenuItemClick">
            <Items>
                <%--<asp:MenuItem Text="Modifica testata" Value="ModT"></asp:MenuItem>
				<asp:MenuItem Text="Elimina testata" Value="DelT"></asp:MenuItem>--%>
                <asp:MenuItem Text="Ritorna" Value="Back"></asp:MenuItem>
            </Items>
        </asp:Menu>
    </div>
    <div id="contenuto">
        <div id="testata" runat="server">
            <div id="testata_lettura" runat="server" visible="true">
                <asp:Label ID="lbl_rdr_titolo" runat="server" Font-Bold="True" Font-Size="Medium"></asp:Label>
                <br />
                <br />
                <asp:Label ID="lbl_stato" runat="server" Font-Bold="false" Font-Size="Small" Font-Italic="false"></asp:Label>
                <asp:Label ID="lbl_rdr_descr_stato" runat="server" Font-Bold="True" Font-Size="Small"
                    Font-Italic="true"></asp:Label>
            </div>
            <div id="testata_creamod" runat="server" visible="true">
                <asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_titolo" Text="Titolo (*)"></asp:Label>
                <asp:RequiredFieldValidator ID="RFV_txt_ins_titolo" runat="server" ErrorMessage="Titolo obbligatorio"
                    ControlToValidate="txt_ins_titolo" ForeColor="Red" Enabled="true"></asp:RequiredFieldValidator>
                <asp:TextBox CssClass="etichetta" ID="txt_ins_titolo" runat="server" MaxLength="255"
                    TextMode="MultiLine" Width="100%" ValidationGroup="1"></asp:TextBox>
                <asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_categoria_cod" Text="Categoria (*)"></asp:Label><br />
                <asp:DropDownList ID="ddl_ins_categoria_cod" runat="server" OnSelectedIndexChanged="ddl_ins_categoria_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RFV_ddl_ins_categoria_cod" runat="server" ErrorMessage="Categoria obbligatoria"
                    ControlToValidate="ddl_ins_categoria_cod" ForeColor="Red" Enabled="true"></asp:RequiredFieldValidator>
                <br />
                <asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_cod_selezione" Text="Codice selezione (*)"></asp:Label><br />
                <asp:DropDownList ID="ddl_ins_selezione_cod" runat="server" OnSelectedIndexChanged="ddl_ins_selezione_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:RequiredFieldValidator ID="RFV_ddl_ins_selezione_cod" runat="server" ErrorMessage="Codice selezione obbligatorio"
                    ControlToValidate="ddl_ins_selezione_cod" ForeColor="Red" Enabled="true"></asp:RequiredFieldValidator>
                <br />
                <asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_anno" Text="Anno (*)"></asp:Label>
                <asp:RequiredFieldValidator ID="RFV_txt_ins_anno" runat="server" ErrorMessage="Anno obbligatorio"
                    ControlToValidate="txt_ins_anno" Enabled="true" ForeColor="Red"></asp:RequiredFieldValidator><br />
                <asp:TextBox CssClass="etichetta" ID="txt_ins_anno" runat="server" Width="15%" ValidationGroup="1"></asp:TextBox><br />
                <asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_descrizione" Text="Descrizione"></asp:Label>
                <asp:TextBox CssClass="etichetta" ID="txt_ins_descrizione" runat="server" MaxLength="2000"
                    TextMode="MultiLine" Width="100%"></asp:TextBox>
                <asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_data_iniz_val" Text="Data inizio validità (*)"></asp:Label>
                <br />
                <asp:TextBox CssClass="etichetta" ID="txt_ins_data_iniz_val" runat="server" MaxLength="15"
                    Columns="15"></asp:TextBox>
                <asp:ImageButton ID="imb_ins_data_iniz_val" ImageUrl="~/Images/Calendar_scheduleHS.png"
                    runat="server" />
                <asp:RequiredFieldValidator ID="RFV_txt_ins_data_iniz_val" runat="server" ErrorMessage="Data inizio validità obbligatoria"
                    ControlToValidate="txt_ins_data_iniz_val" ForeColor="Red" Enabled="true"></asp:RequiredFieldValidator>
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
                <asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_data_term_pres" Text="Data fine compilazione"></asp:Label>
                <br />
                <asp:TextBox CssClass="etichetta" ID="txt_ins_data_term_pres" runat="server" MaxLength="15"
                    Columns="15"></asp:TextBox>
                <asp:ImageButton ID="imb_ins_data_fine_pres" ImageUrl="~/Images/Calendar_scheduleHS.png"
                    runat="server" />
                <cc1:MaskedEditExtender ID="mee_ins_data_term_pres" TargetControlID="txt_ins_data_term_pres"
                    runat="server" MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="true"
                    ClipboardEnabled="true" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                    OnInvalidCssClass="MaskedEditError">
                </cc1:MaskedEditExtender>
                <cc1:MaskedEditValidator ID="mev_ins_data_term_pres" ControlExtender="mee_ins_data_term_pres"
                    ControlToValidate="txt_ins_data_term_pres" IsValidEmpty="true" TooltipMessage="Inserire una data"
                    InvalidValueMessage="Data non valida" InvalidValueBlurredMessage="Data non valida"
                    ValidationExpression="" runat="server">
                </cc1:MaskedEditValidator>
                <cc1:CalendarExtender ID="cae_ins_data_term_pres" TargetControlID="txt_ins_data_term_pres"
                    Format="dd/MM/yyyy" PopupButtonID="imb_ins_data_term_pres" runat="server" CssClass="MyCalendar">
                </cc1:CalendarExtender>
                <br />
                <asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_data_term_controllo_amm"
                    Text="Data fine controllo ufficio"></asp:Label>
                <br />
                <asp:TextBox CssClass="etichetta" ID="txt_ins_data_term_controllo_amm" runat="server"
                    MaxLength="15" Columns="15"></asp:TextBox>
                <asp:ImageButton ID="imb_ins_data_term_controllo_amm" ImageUrl="~/Images/Calendar_scheduleHS.png"
                    runat="server" />
                <cc1:MaskedEditExtender ID="mee_ins_data_term_controllo_amm" TargetControlID="txt_ins_data_term_controllo_amm"
                    runat="server" MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="true"
                    ClipboardEnabled="true" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                    OnInvalidCssClass="MaskedEditError">
                </cc1:MaskedEditExtender>
                <cc1:MaskedEditValidator ID="mev_ins_data_term_controllo_amm" ControlExtender="mee_ins_data_term_controllo_amm"
                    ControlToValidate="txt_ins_data_term_controllo_amm" IsValidEmpty="true" TooltipMessage="Inserire una data"
                    InvalidValueMessage="Data non valida" InvalidValueBlurredMessage="Data non valida"
                    ValidationExpression="" runat="server">
                </cc1:MaskedEditValidator>
                <cc1:CalendarExtender ID="cae_ins_data_term_controllo_amm" TargetControlID="txt_ins_data_term_controllo_amm"
                    Format="dd/MM/yyyy" PopupButtonID="imb_ins_data_term_controllo_amm" runat="server"
                    CssClass="MyCalendar">
                </cc1:CalendarExtender>
                <br />
                <asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_data_term_val_resp"
                    Text="Data fine valutazione responsabili"></asp:Label>
                <br />
                <asp:TextBox CssClass="etichetta" ID="txt_ins_data_term_val_resp" runat="server"
                    MaxLength="15" Columns="15"></asp:TextBox>
                <asp:ImageButton ID="imb_ins_data_term_val_resp" ImageUrl="~/Images/Calendar_scheduleHS.png"
                    runat="server" />
                <cc1:MaskedEditExtender ID="mee_ins_data_term_val_resp" TargetControlID="txt_ins_data_term_val_resp"
                    runat="server" MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="true"
                    ClipboardEnabled="true" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                    OnInvalidCssClass="MaskedEditError">
                </cc1:MaskedEditExtender>
                <cc1:MaskedEditValidator ID="mev_ins_data_term_val_resp" ControlExtender="mee_ins_data_term_val_resp"
                    ControlToValidate="txt_ins_data_term_val_resp" IsValidEmpty="true" TooltipMessage="Inserire una data"
                    InvalidValueMessage="Data non valida" InvalidValueBlurredMessage="Data non valida"
                    ValidationExpression="" runat="server">
                </cc1:MaskedEditValidator>
                <cc1:CalendarExtender ID="cae_ins_data_term_val_resp" TargetControlID="txt_ins_data_term_val_resp"
                    Format="dd/MM/yyyy" PopupButtonID="imb_ins_data_term_val_resp" runat="server"
                    CssClass="MyCalendar">
                </cc1:CalendarExtender>
                <br />
                <asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_data_term_controllo_dip"
                    Text="Data fine controllo dipendente"></asp:Label>
                <br />
                <asp:TextBox CssClass="etichetta" ID="txt_ins_data_term_controllo_dip" runat="server"
                    MaxLength="15" Columns="15"></asp:TextBox>
                <asp:ImageButton ID="imb_ins_data_term_controllo_dip" ImageUrl="~/Images/Calendar_scheduleHS.png"
                    runat="server" />
                <cc1:MaskedEditExtender ID="mee_ins_data_term_controllo_dip" TargetControlID="txt_ins_data_term_controllo_dip"
                    runat="server" MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="true"
                    ClipboardEnabled="true" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                    OnInvalidCssClass="MaskedEditError">
                </cc1:MaskedEditExtender>
                <cc1:MaskedEditValidator ID="mev_ins_data_term_controllo_dip" ControlExtender="mee_ins_data_term_controllo_dip"
                    ControlToValidate="txt_ins_data_term_controllo_dip" IsValidEmpty="true" TooltipMessage="Inserire una data"
                    InvalidValueMessage="Data non valida" InvalidValueBlurredMessage="Data non valida"
                    ValidationExpression="" runat="server">
                </cc1:MaskedEditValidator>
                <cc1:CalendarExtender ID="cae_ins_data_term_controllo_dip" TargetControlID="txt_ins_data_term_controllo_dip"
                    Format="dd/MM/yyyy" PopupButtonID="imb_ins_data_term_controllo_dip" runat="server"
                    CssClass="MyCalendar">
                </cc1:CalendarExtender>
                <br />
                <asp:Label CssClass="etichetta titolo" runat="server" ID="lbl_data_term_val_amm"
                    Text="Data fine valutazione ufficio amministrativo"></asp:Label>
                <br />
                <asp:TextBox CssClass="etichetta" ID="txt_ins_data_term_val_amm" runat="server" MaxLength="15"
                    Columns="15"></asp:TextBox>
                <asp:ImageButton ID="imb_ins_data_term_val_amm" ImageUrl="~/Images/Calendar_scheduleHS.png"
                    runat="server" />
                <cc1:MaskedEditExtender ID="mee_ins_data_term_val_amm" TargetControlID="txt_ins_data_term_val_amm"
                    runat="server" MaskType="Date" Mask="99/99/9999" ClearMaskOnLostFocus="true"
                    ClipboardEnabled="true" MessageValidatorTip="true" OnFocusCssClass="MaskedEditFocus"
                    OnInvalidCssClass="MaskedEditError">
                </cc1:MaskedEditExtender>
                <cc1:MaskedEditValidator ID="mev_ins_data_term_val_amm" ControlExtender="mee_ins_data_term_val_amm"
                    ControlToValidate="txt_ins_data_term_val_amm" IsValidEmpty="true" TooltipMessage="Inserire una data"
                    InvalidValueMessage="Data non valida" InvalidValueBlurredMessage="Data non valida"
                    ValidationExpression="" runat="server">
                </cc1:MaskedEditValidator>
                <cc1:CalendarExtender ID="cae_ins_data_term_val_amm" TargetControlID="txt_ins_data_term_val_amm"
                    Format="dd/MM/yyyy" PopupButtonID="imb_ins_data_term_val_amm" runat="server"
                    CssClass="MyCalendar">
                </cc1:CalendarExtender>
                <br />
                <asp:Button ID="btnConfermaSel" runat="server" Text="Inserisci" CssClass="cerca"
                    OnClick="btnConfermaSel_Click" />
                <asp:Button ID="btnAnnullaSel" runat="server" Text="Annulla" CssClass="cerca" OnClick="btnConfermaSel_Click"
                    CausesValidation="False" />
            </div>
        </div>
        <asp:Label ID="LblErr" runat="server" Text=""></asp:Label>
    </div>
</asp:Content>
