namespace SelezioniRep
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Data;
    using System.Web;

    /// <summary>
    /// Summary description for DettaglioRep.
    /// </summary>
    public partial class DettaglioRepArchivio : Telerik.Reporting.Report
    {
        public DettaglioRepArchivio()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //


        }
        public void DettaglioRepArchivio_NeedDataSource(object sender, EventArgs e)
        {
            try
            {
                //Costanti: dimensioni oggetti report

                double nIncrY = .2;   //Incremento di ordinata di un item rispetto al precedente
                double nIncr_TA_Y = 2;   //Incremento di ordinata di un item rispetto al precedente se questo era Araea di Testo
                double nIncrX = .5;   //Incremento di ascissa di un item rispetto al precedente

                //Dimensioni utilizzate
                // Indicatori
                double IndPosX = 0.1;
                double IndW = 24; //larghezza indicatore
                double IndH = 1; //altezza indicatore
                double IndS = 12;
                string IndTA = "Left";
                bool IndBold = true;
                bool IndItalic = false;
                bool IndUnderline = false;

                // Dettagli indicatori
                double IndDetPosX = 0.6;
                double IndDetW = 24; //larghezza dettaglio indicatore
                double IndDetH = 1; //altezza dettaglio indicatore
                double IndDetS = 10;
                string IndDetTA = "Left";
                bool IndDetBold = true;
                bool IndDetItalic = false;
                bool IndDetUnderline = false;

                // Note
                double NotePosX = 0.6;
                double NoteW = 24; //larghezza dettaglio indicatore
                double NoteH = 1; //altezza dettaglio indicatore
                double NoteS = 10;
                string NoteTA = "Left";
                bool NoteBold = false;
                bool NoteItalic = true;
                bool NoteUnderline = false;

                // Righe dettaglio
                double RigaPosX = 0.6;
                double RigaW = 18; //larghezza dettaglio indicatore
                double RigaH = 1; //altezza dettaglio indicatore
                double Riga_D_W = 5; //larghezza dettaglio indicatore tipo data
                double Riga_TA_H = 10; //altezza text area
                double RigaS = 9;
                string RigaTAL = "Left";
                string RigaTAR = "Right";
                bool RigaBold = false;
                bool RigaItalic = false;
                bool RigaUnderline = false;
                double Riga_C_W = 10; //larghezza checkbox risposta alla domanda multipla
                double Riga_C_H = 0.5; //altezza checkbox risposta alla domanda multipla

                // Linea orizzontale
                double HLPosX = 0.21;
                double HLW = 25; //larghezza
                double HLH = 0.2; //altezza 

                DataTable dt = new DataTable();
                dt = DatiRepArchivio.ds.Tables[1];
                int ind_id_old = 0;
                int ind_det_id_old = 0;
                int ind_prog = 0;
                //int ord_old = 0; //>>ga13092016<<
                string ord_old = "0"; //>>ga13092016<<
                int num_det_old = 0;
                bool PrecTA = false;
                detail.PageBreak = Telerik.Reporting.PageBreak.Before;
                foreach (DataRow item in dt.Rows)
                {
                    if (int.Parse(item["ind_id"].ToString()) != ind_id_old)
                    {
                        Line_Construct(HLPosX, nBottom + 5 * nIncrY, HLW, HLH, "HL", 5);
                        ind_prog += 1;
                        /* >>ga092016<< inizio */
                        string value = ind_prog.ToString() + ") " + item["descr_ind"].ToString();

                        int initialString = value.IndexOf("&lt;");
                        if (initialString != -1)
                        {
                            int finalString = value.IndexOf("&gt;");
                            if (finalString != -1)
                            {
                                int LengthString = finalString - initialString + 1;
                                value.Remove(initialString, LengthString);
                            }
                        }
                        value = HttpUtility.HtmlDecode(value);

                        /*  >>ga092016<< fine */
                        TextBox_Construct(IndPosX, nBottom + 7 * nIncrY, IndW, IndH, IndBold, IndItalic, IndUnderline, IndS, IndTA, value, null);
                        string note = item["note_ind"].ToString();
                        TextBox_Construct(NotePosX, nBottom + nIncrY, NoteW, NoteH, NoteBold, NoteItalic, NoteUnderline, NoteS, NoteTA, note, null);
                        ind_id_old = int.Parse(item["ind_id"].ToString());
                        ind_det_id_old = 0;
                        PrecTA = false;
                    }
                    if (int.Parse(item["ind_det_id"].ToString()) != ind_det_id_old)
                    {
                        string value = item["descr_ind_det"].ToString();
                        /*  >>ga092016<< inizio */
                        int initialString = value.IndexOf("&lt;");
                        if (initialString != -1)
                        {
                            int finalString = value.IndexOf("/a&gt;");
                            if (finalString != -1)
                            {
                                finalString = finalString + 5;
                                int LengthString = finalString - initialString + 1;
                                value = value.Remove(initialString, LengthString);
                            }
                        }
                        value = HttpUtility.HtmlDecode(value);

                        /*  >>ga092016<< fine */
                        if (dt.Columns.Contains("punt"))
                        {
                            if (int.Parse(item["tipo_ctrl"].ToString()) != 6)
                                value = value + "   -  PUNTEGGIO ASSEGNATO " + item["punt"].ToString(); //>>ga21082012<<
                        }

                        TextBox_Construct(IndDetPosX, nBottom + 5 * nIncrY, IndDetW, IndDetH, IndDetBold, IndDetItalic, IndDetUnderline, IndDetS, IndDetTA, value, null);
                        string note = item["note_ind_det"].ToString();
                        TextBox_Construct(NotePosX, nBottom + nIncrY, NoteW, NoteH, NoteBold, NoteItalic, NoteUnderline, NoteS, NoteTA, note, null);

                        ind_det_id_old = int.Parse(item["ind_det_id"].ToString());
                        ord_old = "0";
                        num_det_old = 0;
                        PrecTA = false;
                        Telerik.Reporting.Panel pnl = new Telerik.Reporting.Panel();
                        Shape_Construct(HLPosX, nBottom, HLW, HLH, "HL");
                    }
                    string valueItem = HttpUtility.HtmlDecode(item["descr_ind_det_riga"].ToString()) + ": ";
                    if (int.Parse(item["risposta_flg"].ToString()) == 1)
                        valueItem += item["risp"].ToString();
                    double RigaHCtrl = RigaH;
                    double RigaWCtrl = RigaW;
                    string RigaTACtrl = RigaTAL;
                    double IncrYCtrl = nIncrY;
                    if (int.Parse(item["tipo_ctrl"].ToString()) == 2) //Data
                    {
                        RigaWCtrl = Riga_D_W;
                    }
                    else if (int.Parse(item["tipo_ctrl"].ToString()) == 3 || int.Parse(item["tipo_ctrl"].ToString()) == 4)
                    {
                        RigaHCtrl = Riga_C_H;
                        RigaWCtrl = Riga_C_W;
                    }
                    if (PrecTA == true)
                    {
                        IncrYCtrl = nIncr_TA_Y;
                    }

                    if (int.Parse(item["num_det"].ToString()) != num_det_old && num_det_old != 0)
                    {
                        Shape_Construct(HLPosX, nBottom + IncrYCtrl - .2, HLW, HLH, "HL");
                    }
                    //if (int.Parse(item["ord"].ToString()) != ord_old) >>ga13092016<<
                    if (item["ord"].ToString() != ord_old) //>>ga13092016<<
                    {
                        if (int.Parse(item["tipo_ctrl"].ToString()) != 3 && int.Parse(item["tipo_ctrl"].ToString()) != 4)
                            TextBox_Construct(RigaPosX, nBottom + IncrYCtrl, RigaWCtrl, RigaHCtrl, RigaBold, RigaItalic, RigaUnderline, RigaS, RigaTACtrl, valueItem, null);
                        else
                            CheckBox_Construct(RigaPosX, nBottom + IncrYCtrl, RigaWCtrl, RigaHCtrl, RigaTACtrl, valueItem, int.Parse(item["risposta_flg"].ToString()), null);
                        if (int.Parse(item["tipo_ctrl"].ToString()) == 5)
                            PrecTA = true;
                        else
                            PrecTA = false;
                    }
                    else
                    {
                        if (nIncrX + nRight + RigaWCtrl > 25)
                            RigaWCtrl = 25 - (nIncrX + nRight);
                        if (int.Parse(item["tipo_ctrl"].ToString()) != 3 && int.Parse(item["tipo_ctrl"].ToString()) != 4)
                            TextBox_Construct(nIncrX + nRight, nTop, RigaWCtrl, RigaHCtrl, RigaBold, RigaItalic, RigaUnderline, RigaS, RigaTACtrl, valueItem, null);
                        else
                            CheckBox_Construct(nIncrX + nRight, nTop, RigaWCtrl, RigaHCtrl, RigaTACtrl, valueItem, int.Parse(item["risposta_flg"].ToString()), null);
                    }
                    //ord_old = int.Parse(item["ord"].ToString()); >>ga13092016<<
                    ord_old = item["ord"].ToString(); //>>ga13092016<<
                    num_det_old = int.Parse(item["num_det"].ToString());
                }
                Shape_Construct(HLPosX, nBottom + 2 * nIncrY, HLW, HLH, "HL");
            }
            catch (Exception ex)
            {
            }
        }
        private void TextBox_Construct(double pPosX, double pPosY, double pIndW, double pIndH, bool pBold, bool pItalic, bool pUnderline, double pSize, string pTextAlign, string pValue, string pBorder)
        {
            Telerik.Reporting.TextBox tb = new Telerik.Reporting.TextBox();
            tb.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(pPosX, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(pPosY, Telerik.Reporting.Drawing.UnitType.Cm));
            tb.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(pIndW, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(pIndH, Telerik.Reporting.Drawing.UnitType.Cm));
            tb.CanShrink = true;
            tb.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            if (pBold == true)
                tb.Style.Font.Bold = true;
            if (pItalic == true)
                tb.Style.Font.Italic = true;
            if (pUnderline == true)
                tb.Style.Font.Underline = true;
            tb.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(pSize, Telerik.Reporting.Drawing.UnitType.Point);
            if (pTextAlign == "Left")
                tb.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            else if (pTextAlign == "Center")
                tb.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            else
                tb.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            tb.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Top;
            tb.Value = pValue;
            tb.CanGrow = true;
            if (pBorder == "Left")
            {
                tb.Style.BorderStyle.Left = BorderType.Solid;
                tb.Style.BorderStyle.Top = BorderType.Solid;
                tb.Style.BorderStyle.Bottom = BorderType.Solid;
            }
            else if (pBorder == "Right")
            {
                tb.Style.BorderStyle.Right = BorderType.Solid;
                tb.Style.BorderStyle.Top = BorderType.Solid;
                tb.Style.BorderStyle.Bottom = BorderType.Solid;
            }
            detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] { tb });
            nBottom = tb.Bottom.Value;
            nTop = tb.Top.Value;
            nRight = tb.Right.Value;
        }
        private void Shape_Construct(double pPosX, double pPosY, double pIndW, double pIndH, string pType)
        {
            Telerik.Reporting.Shape shp = new Shape();
            shp.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(pPosX, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(pPosY, Telerik.Reporting.Drawing.UnitType.Cm));
            if (pType == "HL")
            {
                shp.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            }
            else if (pType == "VL")
            {
                shp.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.NS);
            }
            shp.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(pIndW, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(pIndH, Telerik.Reporting.Drawing.UnitType.Cm));
            detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] { shp });
            if (pType == "HL")
                nBottomLine = shp.Bottom.Value;
        }
        private void CheckBox_Construct(double pPosX, double pPosY, double pIndW, double pIndH, string pTextAlign, string pValue, int pChecked, string pBorder)
        {
            Telerik.Reporting.CheckBox cb = new Telerik.Reporting.CheckBox();
            cb.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(pPosX, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(pPosY, Telerik.Reporting.Drawing.UnitType.Cm));
            cb.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(pIndW, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(pIndH, Telerik.Reporting.Drawing.UnitType.Cm));
            cb.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            cb.Text = pValue;
            if (pChecked == 1)
            {
                cb.Value = true;
            }
            else
            {
                cb.Value = false;
            }
            detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] { cb });
            nBottom = cb.Bottom.Value;
            nTop = cb.Top.Value;
            nRight = cb.Right.Value;
        }
        public double nBottom
        {
            get;
            set;
        }
        public double nTop
        {
            get;
            set;
        }
        public double nRight
        {
            get;
            set;
        }
        public double nBottomLine
        {
            get;
            set;
        }
        private void Line_Construct(double pPosX, double pPosY, double pIndW, double pIndH, string pType, int LineWidth)
        {
            Telerik.Reporting.Shape shp = new Shape();
            shp.Location = new Telerik.Reporting.Drawing.PointU(new Telerik.Reporting.Drawing.Unit(pPosX, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(pPosY, Telerik.Reporting.Drawing.UnitType.Cm));
            if (pType == "HL")
            {
                shp.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.EW);
            }
            else if (pType == "VL")
            {
                shp.ShapeType = new Telerik.Reporting.Drawing.Shapes.LineShape(Telerik.Reporting.Drawing.Shapes.LineDirection.NS);
            }
            shp.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(pIndW, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(pIndH, Telerik.Reporting.Drawing.UnitType.Cm));
            shp.Style.Color = System.Drawing.Color.DimGray;
            shp.Style.LineWidth = Telerik.Reporting.Drawing.Unit.Point(LineWidth);
            detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] { shp });
            if (pType == "HL")
                nBottomLine = shp.Bottom.Value;
        }
    }
}