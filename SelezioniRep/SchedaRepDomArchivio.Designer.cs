namespace SelezioniRep
{
    partial class SchedaRepDomArchivio
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchedaRepDomArchivio));
            Telerik.Reporting.TableGroup tableGroup1 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup2 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup3 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup4 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup5 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup6 = new Telerik.Reporting.TableGroup();
            this.lbl_punt = new Telerik.Reporting.TextBox();
            this.textBox9 = new Telerik.Reporting.TextBox();
            this.txt_descrinddet = new Telerik.Reporting.TextBox();
            this.txt_descrind = new Telerik.Reporting.TextBox();
            this.txt_categoriacod = new Telerik.Reporting.TextBox();
            this.pageHeaderSection1 = new Telerik.Reporting.PageHeaderSection();
            this.txt_intest = new Telerik.Reporting.TextBox();
            this.lbl_NomeCognome = new Telerik.Reporting.TextBox();
            this.txt_NomeCognome = new Telerik.Reporting.TextBox();
            this.lbl_Struttura = new Telerik.Reporting.TextBox();
            this.txt_Struttura = new Telerik.Reporting.TextBox();
            this.lbl_TipoScheda2 = new Telerik.Reporting.TextBox();
            this.lbl_TipoScheda3 = new Telerik.Reporting.TextBox();
            this.lbl_TipoScheda1 = new Telerik.Reporting.TextBox();
            this.pictureBox1 = new Telerik.Reporting.PictureBox();
            this.lbl_Stato = new Telerik.Reporting.TextBox();
            this.txt_Stato = new Telerik.Reporting.TextBox();
            this.detail = new Telerik.Reporting.DetailSection();
            this.tbl_riepilogo = new Telerik.Reporting.Table();
            this.txt_descrinddetpunt = new Telerik.Reporting.TextBox();
            this.txt_punt = new Telerik.Reporting.TextBox();
            this.lbl_descrinddet = new Telerik.Reporting.TextBox();
            this.lbl_descrind = new Telerik.Reporting.TextBox();
            this.lbl_categoriacod = new Telerik.Reporting.TextBox();
            this.subRep_Dettaglio = new Telerik.Reporting.SubReport();
            this.dettaglioRep1 = new SelezioniRep.DettaglioRepArchivio();
            this.pageFooterSection1 = new Telerik.Reporting.PageFooterSection();
            ((System.ComponentModel.ISupportInitialize)(this.dettaglioRep1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // lbl_punt
            // 
            this.lbl_punt.Name = "lbl_punt";
            this.lbl_punt.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.5297927856445312D), Telerik.Reporting.Drawing.Unit.Cm(0.846666693687439D));
            this.lbl_punt.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.lbl_punt.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.lbl_punt.Style.Font.Bold = true;
            this.lbl_punt.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.lbl_punt.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.lbl_punt.Value = "PUNTEGGIO PARZIALE MAX ATTRIBUIBILE";
            // 
            // textBox9
            // 
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2D), Telerik.Reporting.Drawing.Unit.Cm(0.846666693687439D));
            this.textBox9.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox9.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox9.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox9.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox9.StyleName = "";
            // 
            // txt_descrinddet
            // 
            this.txt_descrinddet.CanGrow = false;
            this.txt_descrinddet.Name = "txt_descrinddet";
            this.txt_descrinddet.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.8845815658569336D), Telerik.Reporting.Drawing.Unit.Cm(0.68791705369949341D));
            this.txt_descrinddet.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.txt_descrinddet.StyleName = "";
            this.txt_descrinddet.Value = "= SelezioniRep.DatiRepArchivio.EliminaLinkArchivio(Fields.descr_ind_det)";
            // 
            // txt_descrind
            // 
            this.txt_descrind.CanGrow = false;
            this.txt_descrind.Name = "txt_descrind";
            this.txt_descrind.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.1329169273376465D), Telerik.Reporting.Drawing.Unit.Cm(0.68791705369949341D));
            this.txt_descrind.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.txt_descrind.StyleName = "";
            this.txt_descrind.Value = "=Fields.descr_ind";
            // 
            // txt_categoriacod
            // 
            this.txt_categoriacod.Name = "txt_categoriacod";
            this.txt_categoriacod.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.1797913312911987D), Telerik.Reporting.Drawing.Unit.Cm(0.68791705369949341D));
            this.txt_categoriacod.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.txt_categoriacod.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txt_categoriacod.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Top;
            this.txt_categoriacod.StyleName = "";
            this.txt_categoriacod.Value = "=Fields.categoria_cod";
            // 
            // pageHeaderSection1
            // 
            this.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(3D);
            this.pageHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.txt_intest,
            this.lbl_NomeCognome,
            this.txt_NomeCognome,
            this.lbl_Struttura,
            this.txt_Struttura,
            this.lbl_TipoScheda2,
            this.lbl_TipoScheda3,
            this.lbl_TipoScheda1,
            this.pictureBox1,
            this.lbl_Stato,
            this.txt_Stato});
            this.pageHeaderSection1.Name = "pageHeaderSection1";
            this.pageHeaderSection1.PrintOnLastPage = true;
            // 
            // txt_intest
            // 
            this.txt_intest.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.0999999046325684D), Telerik.Reporting.Drawing.Unit.Cm(0.099999949336051941D));
            this.txt_intest.Name = "txt_intest";
            this.txt_intest.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(18.600000381469727D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.txt_intest.Style.Font.Bold = true;
            this.txt_intest.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.txt_intest.TextWrap = true;
            this.txt_intest.Value = "=\"DOMANDA DI PARTECIPAZIONE AI FINI DELLA PROGRESSIONE ECONOMICA ORIZZONTALE \" + " +
    "Fields.anno";
            // 
            // lbl_NomeCognome
            // 
            this.lbl_NomeCognome.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(1.6000000238418579D));
            this.lbl_NomeCognome.Name = "lbl_NomeCognome";
            this.lbl_NomeCognome.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(3.7996997833251953D), Telerik.Reporting.Drawing.Unit.Cm(0.59999990463256836D));
            this.lbl_NomeCognome.Value = "NOME E COGNOME:";
            // 
            // txt_NomeCognome
            // 
            this.txt_NomeCognome.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.8000001907348633D), Telerik.Reporting.Drawing.Unit.Cm(1.6000000238418579D));
            this.txt_NomeCognome.Name = "txt_NomeCognome";
            this.txt_NomeCognome.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(11.000000953674316D), Telerik.Reporting.Drawing.Unit.Cm(0.59999990463256836D));
            this.txt_NomeCognome.Value = "";
            // 
            // lbl_Struttura
            // 
            this.lbl_Struttura.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.899999618530273D), Telerik.Reporting.Drawing.Unit.Cm(1.6000000238418579D));
            this.lbl_Struttura.Name = "lbl_Struttura";
            this.lbl_Struttura.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.4000003337860107D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
            this.lbl_Struttura.Value = "STRUTTURA:";
            // 
            // txt_Struttura
            // 
            this.txt_Struttura.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(17.299999237060547D), Telerik.Reporting.Drawing.Unit.Cm(1.6000000238418579D));
            this.txt_Struttura.Name = "txt_Struttura";
            this.txt_Struttura.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(8.40000057220459D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.txt_Struttura.Value = "";
            // 
            // lbl_TipoScheda2
            // 
            this.lbl_TipoScheda2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(5.0999999046325684D), Telerik.Reporting.Drawing.Unit.Cm(2.2999999523162842D));
            this.lbl_TipoScheda2.Name = "lbl_TipoScheda2";
            this.lbl_TipoScheda2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.1000001430511475D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.lbl_TipoScheda2.Style.Font.Bold = true;
            this.lbl_TipoScheda2.Value = "RIEPILOGO";
            // 
            // lbl_TipoScheda3
            // 
            this.lbl_TipoScheda3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(7.5D), Telerik.Reporting.Drawing.Unit.Cm(2.2999999523162842D));
            this.lbl_TipoScheda3.Name = "lbl_TipoScheda3";
            this.lbl_TipoScheda3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.0999999046325684D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.lbl_TipoScheda3.Value = "INDICATORI E PUNTEGGI";
            // 
            // lbl_TipoScheda1
            // 
            this.lbl_TipoScheda1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(2.2999999523162842D));
            this.lbl_TipoScheda1.Name = "lbl_TipoScheda1";
            this.lbl_TipoScheda1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(4.8000001907348633D), Telerik.Reporting.Drawing.Unit.Cm(0.59999990463256836D));
            this.lbl_TipoScheda1.Value = "SCHEDA DI VALUTAZIONE";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.pictureBox1.MimeType = "image/jpeg";
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.0000004768371582D), Telerik.Reporting.Drawing.Unit.Cm(1.5D));
            this.pictureBox1.Sizing = Telerik.Reporting.Drawing.ImageSizeMode.ScaleProportional;
            this.pictureBox1.Value = ((object)(resources.GetObject("pictureBox1.Value")));
            // 
            // lbl_Stato
            // 
            this.lbl_Stato.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(14.899999618530273D), Telerik.Reporting.Drawing.Unit.Cm(2.2999999523162842D));
            this.lbl_Stato.Name = "lbl_Stato";
            this.lbl_Stato.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.4000003337860107D), Telerik.Reporting.Drawing.Unit.Cm(0.599999725818634D));
            this.lbl_Stato.Value = "STATO:";
            // 
            // txt_Stato
            // 
            this.txt_Stato.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(17.299999237060547D), Telerik.Reporting.Drawing.Unit.Cm(2.2999999523162842D));
            this.txt_Stato.Name = "txt_Stato";
            this.txt_Stato.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(8.40000057220459D), Telerik.Reporting.Drawing.Unit.Cm(0.60000008344650269D));
            this.txt_Stato.Value = "";
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(3.0999996662139893D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.tbl_riepilogo,
            this.subRep_Dettaglio});
            this.detail.Name = "detail";
            // 
            // tbl_riepilogo
            // 
            this.tbl_riepilogo.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(5.52979040145874D)));
            this.tbl_riepilogo.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(2D)));
            this.tbl_riepilogo.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.68791705369949341D)));
            this.tbl_riepilogo.Body.SetCellContent(0, 0, this.txt_descrinddetpunt);
            this.tbl_riepilogo.Body.SetCellContent(0, 1, this.txt_punt);
            tableGroup1.ReportItem = this.lbl_punt;
            tableGroup2.Name = "Group1";
            tableGroup2.ReportItem = this.textBox9;
            this.tbl_riepilogo.ColumnGroups.Add(tableGroup1);
            this.tbl_riepilogo.ColumnGroups.Add(tableGroup2);
            this.tbl_riepilogo.ColumnHeadersPrintOnEveryPage = true;
            this.tbl_riepilogo.Corner.SetCellContent(0, 2, this.lbl_descrinddet);
            this.tbl_riepilogo.Corner.SetCellContent(0, 1, this.lbl_descrind);
            this.tbl_riepilogo.Corner.SetCellContent(0, 0, this.lbl_categoriacod);
            this.tbl_riepilogo.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.txt_descrinddetpunt,
            this.txt_punt,
            this.lbl_punt,
            this.textBox9,
            this.lbl_descrinddet,
            this.lbl_descrind,
            this.lbl_categoriacod,
            this.txt_categoriacod,
            this.txt_descrind,
            this.txt_descrinddet});
            this.tbl_riepilogo.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.tbl_riepilogo.Name = "tbl_riepilogo";
            tableGroup6.Groupings.AddRange(new Telerik.Reporting.Data.Grouping[] {
            new Telerik.Reporting.Data.Grouping(null)});
            tableGroup6.Name = "DetailGroup";
            tableGroup5.ChildGroups.Add(tableGroup6);
            tableGroup5.Groupings.AddRange(new Telerik.Reporting.Data.Grouping[] {
            new Telerik.Reporting.Data.Grouping("=Fields.descr_ind_det")});
            tableGroup5.Name = "descr_ind_det";
            tableGroup5.ReportItem = this.txt_descrinddet;
            tableGroup4.ChildGroups.Add(tableGroup5);
            tableGroup4.Groupings.AddRange(new Telerik.Reporting.Data.Grouping[] {
            new Telerik.Reporting.Data.Grouping("=Fields.descr_ind")});
            tableGroup4.Name = "descr_ind";
            tableGroup4.ReportItem = this.txt_descrind;
            tableGroup3.ChildGroups.Add(tableGroup4);
            tableGroup3.Groupings.AddRange(new Telerik.Reporting.Data.Grouping[] {
            new Telerik.Reporting.Data.Grouping("=Fields.categoria_cod")});
            tableGroup3.Name = "categoria_cod";
            tableGroup3.ReportItem = this.txt_categoriacod;
            this.tbl_riepilogo.RowGroups.Add(tableGroup3);
            this.tbl_riepilogo.RowHeadersPrintOnEveryPage = false;
            this.tbl_riepilogo.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(25.727079391479492D), Telerik.Reporting.Drawing.Unit.Cm(1.5345838069915772D));
            this.tbl_riepilogo.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            // 
            // txt_descrinddetpunt
            // 
            this.txt_descrinddetpunt.CanGrow = false;
            this.txt_descrinddetpunt.Name = "txt_descrinddetpunt";
            this.txt_descrinddetpunt.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.5297927856445312D), Telerik.Reporting.Drawing.Unit.Cm(0.68791705369949341D));
            this.txt_descrinddetpunt.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.txt_descrinddetpunt.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.txt_descrinddetpunt.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.Solid;
            this.txt_descrinddetpunt.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_descrinddetpunt.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.txt_descrinddetpunt.Value = "=Fields.descr_ind_det_punt";
            // 
            // txt_punt
            // 
            this.txt_punt.CanGrow = false;
            this.txt_punt.Name = "txt_punt";
            this.txt_punt.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2D), Telerik.Reporting.Drawing.Unit.Cm(0.68791705369949341D));
            this.txt_punt.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            this.txt_punt.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.txt_punt.Style.BorderStyle.Left = Telerik.Reporting.Drawing.BorderType.None;
            this.txt_punt.Style.BorderStyle.Right = Telerik.Reporting.Drawing.BorderType.Solid;
            this.txt_punt.Style.BorderStyle.Top = Telerik.Reporting.Drawing.BorderType.Solid;
            this.txt_punt.StyleName = "";
            this.txt_punt.Value = "=Fields.punt";
            // 
            // lbl_descrinddet
            // 
            this.lbl_descrinddet.Name = "lbl_descrinddet";
            this.lbl_descrinddet.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.8845815658569336D), Telerik.Reporting.Drawing.Unit.Cm(0.84666675329208374D));
            this.lbl_descrinddet.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.lbl_descrinddet.Style.Font.Bold = true;
            this.lbl_descrinddet.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.lbl_descrinddet.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.lbl_descrinddet.StyleName = "";
            this.lbl_descrinddet.Value = "DETTAGLIO INDICATORI";
            // 
            // lbl_descrind
            // 
            this.lbl_descrind.Name = "lbl_descrind";
            this.lbl_descrind.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(7.1329169273376465D), Telerik.Reporting.Drawing.Unit.Cm(0.84666675329208374D));
            this.lbl_descrind.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.lbl_descrind.Style.Font.Bold = true;
            this.lbl_descrind.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.lbl_descrind.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.lbl_descrind.StyleName = "";
            this.lbl_descrind.Value = "INDICATORI";
            // 
            // lbl_categoriacod
            // 
            this.lbl_categoriacod.Name = "lbl_categoriacod";
            this.lbl_categoriacod.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.1797913312911987D), Telerik.Reporting.Drawing.Unit.Cm(0.846666693687439D));
            this.lbl_categoriacod.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.lbl_categoriacod.Style.Font.Bold = true;
            this.lbl_categoriacod.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.lbl_categoriacod.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.lbl_categoriacod.StyleName = "";
            this.lbl_categoriacod.Value = "CAT.";
            // 
            // subRep_Dettaglio
            // 
            this.subRep_Dettaglio.KeepTogether = true;
            this.subRep_Dettaglio.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(2.2999999523162842D));
            this.subRep_Dettaglio.Name = "subRep_Dettaglio";
            this.subRep_Dettaglio.ReportSource = this.dettaglioRep1;
            this.subRep_Dettaglio.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(25.729999542236328D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            // 
            // dettaglioRep1
            // 
            this.dettaglioRep1.Name = "DettaglioRep";
            // 
            // pageFooterSection1
            // 
            this.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(0.13229165971279144D);
            this.pageFooterSection1.Name = "pageFooterSection1";
            // 
            // SchedaRepDomArchivio
            // 
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.pageHeaderSection1,
            this.detail,
            this.pageFooterSection1});
            this.Name = "Selezioni";
            this.PageSettings.Landscape = true;
            this.PageSettings.Margins.Bottom = Telerik.Reporting.Drawing.Unit.Mm(10D);
            this.PageSettings.Margins.Left = Telerik.Reporting.Drawing.Unit.Mm(10D);
            this.PageSettings.Margins.Right = Telerik.Reporting.Drawing.Unit.Mm(10D);
            this.PageSettings.Margins.Top = Telerik.Reporting.Drawing.Unit.Mm(10D);
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.Style.BackgroundColor = System.Drawing.Color.White;
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(25.729999542236328D);
            ((System.ComponentModel.ISupportInitialize)(this.dettaglioRep1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.PageHeaderSection pageHeaderSection1;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.PageFooterSection pageFooterSection1;
        private Telerik.Reporting.TextBox txt_intest;
        private Telerik.Reporting.TextBox lbl_NomeCognome;
        private Telerik.Reporting.TextBox txt_NomeCognome;
        private Telerik.Reporting.TextBox lbl_Struttura;
        private Telerik.Reporting.TextBox txt_Struttura;
        private Telerik.Reporting.TextBox lbl_TipoScheda2;
        private Telerik.Reporting.TextBox lbl_TipoScheda3;
        private Telerik.Reporting.Table tbl_riepilogo;
        private Telerik.Reporting.TextBox txt_descrinddetpunt;
        private Telerik.Reporting.TextBox lbl_punt;
        private Telerik.Reporting.TextBox lbl_descrinddet;
        private Telerik.Reporting.TextBox lbl_descrind;
        private Telerik.Reporting.TextBox lbl_categoriacod;
        private Telerik.Reporting.TextBox txt_descrinddet;
        private Telerik.Reporting.TextBox txt_descrind;
        private Telerik.Reporting.TextBox txt_categoriacod;
        private Telerik.Reporting.TextBox txt_punt;
        private Telerik.Reporting.TextBox textBox9;
        private Telerik.Reporting.TextBox lbl_TipoScheda1;
        private Telerik.Reporting.PictureBox pictureBox1;
        private Telerik.Reporting.SubReport subRep_Dettaglio;
        private DettaglioRepArchivio dettaglioRep1;
        private Telerik.Reporting.TextBox lbl_Stato;
        private Telerik.Reporting.TextBox txt_Stato;
    }
}