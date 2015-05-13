using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UtilsMantesis;
using Microsoft.Office.Interop.Word;
using System.Windows.Forms;
using ScjnUtilities;
using System.Diagnostics;
using Mantesis2015.Model;
using System.ComponentModel;

namespace Mantesis2015.Reportes
{
    public class TablaSga
    {
        public static Dictionary<long, string> DicDescripciones = new Dictionary<long, string>();
        public static Dictionary<long, int> DicNiveles = new Dictionary<long, int>();
        public static Dictionary<long, int> DicPadres = new Dictionary<long, int>();

        readonly string filepath = Path.GetTempFileName() + ".docx";

        int fila = 1;

        private List<TesisReg> tesisImprime = new List<TesisReg>();
        Microsoft.Office.Interop.Word.Application oWord;
        Microsoft.Office.Interop.Word.Document oDoc;
        object oMissing = System.Reflection.Missing.Value;
        object oEndOfDoc = "\\endofdoc";

        Microsoft.Office.Interop.Word.Table oTable;

        public void GeneraReporte()
        {
            DoBackgroundWork();
        }

        private void GeneraWord()
        {
            oWord = new Microsoft.Office.Interop.Word.Application();
            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc.PageSetup.Orientation = WdOrientation.wdOrientLandscape;

            //Insert a paragraph at the beginning of the document.
            Microsoft.Office.Interop.Word.Paragraph oPara1;
            oPara1 = oDoc.Content.Paragraphs.Add(ref oMissing);
            oPara1.Range.Text = "SISTEMATIZACIÓN DE TESIS JURISPRUDENCIALES Y AISLADAS PUBLICADAS EN EL SEMANARIO JUDICIAL DE LA FEDERACIÓN Y SU GACETA (" + DateTimeUtilities.ToMonthName(DateTime.Now.Month - 1) + " " + DateTime.Now.Year + ")";
            oPara1.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            oPara1.Range.Font.Bold = 1;
            oPara1.Range.Font.Name = "Times New Roman";
            oPara1.Format.SpaceAfter = 24;    //24 pt spacing after paragraph.
            oPara1.Range.InsertParagraphAfter();

            Microsoft.Office.Interop.Word.Range wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;

            oTable = oDoc.Tables.Add(wrdRng, (new ReporteModel().GetNoTesis() + 1), 8, ref oMissing, ref oMissing);
            oTable.Range.ParagraphFormat.SpaceAfter = 6;
            oTable.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            oTable.Range.Font.Size = 9;
            oTable.Range.Font.Bold = 0;
            oTable.Borders.Enable = 1;

            oTable.Columns[1].SetWidth(40, WdRulerStyle.wdAdjustSameWidth);
            oTable.Columns[2].SetWidth(70, WdRulerStyle.wdAdjustSameWidth);
            oTable.Columns[3].SetWidth(90, WdRulerStyle.wdAdjustSameWidth);
            oTable.Columns[4].SetWidth(86, WdRulerStyle.wdAdjustSameWidth);
            oTable.Columns[5].SetWidth(80, WdRulerStyle.wdAdjustSameWidth);
            oTable.Columns[6].SetWidth(43, WdRulerStyle.wdAdjustSameWidth);
            oTable.Columns[7].SetWidth(210, WdRulerStyle.wdAdjustSameWidth);
            oTable.Columns[8].SetWidth(60, WdRulerStyle.wdAdjustSameWidth);

            oTable.Cell(fila, 1).Range.Text = "IUS";
            oTable.Cell(fila, 2).Range.Text = "Clasificación por materia";
            oTable.Cell(fila, 3).Range.Text = "Clasificación por submateria";
            oTable.Cell(fila, 4).Range.Text = "Sección";
            oTable.Cell(fila, 5).Range.Text = "Subsección";
            oTable.Cell(fila, 6).Range.Text = "Época";
            oTable.Cell(fila, 7).Range.Text = "Rubro";
            oTable.Cell(fila, 8).Range.Text = "Tesis";

            for (int x = 1; x < 9; x++)
            {
                oTable.Cell(fila, x).Range.Font.Size = 10;
                oTable.Cell(fila, x).Range.Font.Bold = 1;
            }

            fila++;

            try
            {
                ImprimeDocumento();

                foreach (Section wordSection in oDoc.Sections)
                {
                    object pagealign = WdPageNumberAlignment.wdAlignPageNumberRight;
                    object firstpage = true;
                    wordSection.Footers[WdHeaderFooterIndex.wdHeaderFooterPrimary].PageNumbers.Add(ref pagealign, ref firstpage);
                }

                oWord.ActiveDocument.SaveAs(filepath);
                oWord.ActiveDocument.Saved = true;
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            finally
            {
                oWord.Visible = true;
                DicNiveles.Clear();
                DicDescripciones.Clear();
                DicPadres.Clear();
                oDoc.Close();

                Process.Start(filepath);
            }
        }

        private void ImprimeDocumento()
        {
            oWord.Visible = true;
            List<long> idsMatSga = new ReporteModel().GetidMatSgaMes();

            int numTesis = 0;

            tesisImprime = new ReporteModel().GetTesis();

            foreach (long idMat in idsMatSga)
            {
                if (!DicDescripciones.ContainsKey(idMat))
                {
                    new EstructuraModel().FillDictionary(Convert.ToChar(idMat.ToString().Substring(0, 1)));
                }

                List<TesisReg> listaImprimir = (from n in tesisImprime
                                                where n.IdMateriaSga == idMat
                                                orderby n.txtGenealogia
                                                select n).ToList();

                foreach (TesisReg tesis in listaImprimir)
                {
                    //if (tesis.tesis.StartsWith("PC."))
                    //{
                        int nivelImprime = DicNiveles[idMat];
                        int nivelPadre = DicPadres[idMat];

                        if (nivelImprime == 4)
                        {
                            oTable.Cell(fila, 5).Range.Text = DicDescripciones[idMat]; //Subsección
                            oTable.Cell(fila, 4).Range.Text = DicDescripciones[nivelPadre];//Sección

                            int nivelAbuelo = DicPadres[nivelPadre];

                            nivelImprime = DicNiveles[nivelAbuelo];

                            if (nivelImprime != 2)
                            {
                                oTable.Cell(fila, 3).Range.Text = DicDescripciones[nivelAbuelo];
                            }//SubMateria}
                            else
                            {
                                int nivelBisAbuelo = DicPadres[nivelAbuelo];
                                if (DicNiveles[nivelBisAbuelo] != 0)
                                    oTable.Cell(fila, 3).Range.Text = DicDescripciones[nivelBisAbuelo];
                                else
                                    oTable.Cell(fila, 3).Range.Text = "";
                            }
                            oTable.Cell(fila, 2).Range.Text = GetMateria(idMat);
                        }
                        else if (nivelImprime == 3)
                        {
                            oTable.Cell(fila, 5).Range.Text = ""; //Subsección
                            oTable.Cell(fila, 4).Range.Text = DicDescripciones[idMat];//Sección

                            nivelImprime = DicNiveles[nivelPadre];

                            if (nivelImprime != 2)
                            {
                                oTable.Cell(fila, 3).Range.Text = DicDescripciones[nivelPadre];
                            }//SubMateria}
                            else
                            {
                                int nivelAbuelo = DicPadres[nivelPadre];
                                if (DicNiveles[nivelAbuelo] != 0)
                                    oTable.Cell(fila, 3).Range.Text = DicDescripciones[nivelAbuelo];
                                else
                                    oTable.Cell(fila, 3).Range.Text = "";
                            }
                            oTable.Cell(fila, 2).Range.Text = GetMateria(idMat);
                        }
                        else if (nivelImprime == 2)
                        {
                            oTable.Cell(fila, 5).Range.Text = ""; //Subsección
                            oTable.Cell(fila, 4).Range.Text = "";//Sección
                            oTable.Cell(fila, 3).Range.Text = (DicNiveles[nivelPadre] == 1) ? DicDescripciones[nivelPadre] : "";// new EstructuraModel().getSubMateriaNivelDos(idTema);//SubMateria
                            oTable.Cell(fila, 2).Range.Text = GetMateria(idMat);
                        }

                        oTable.Cell(fila, 1).Range.Text = tesis.ius4.ToString();
                        oTable.Cell(fila, 6).Range.Text = tesis.epoca;
                        oTable.Cell(fila, 7).Range.Text = tesis.RUBRO;
                        oTable.Cell(fila, 7).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
                        oTable.Cell(fila, 8).Range.Text = tesis.tesis;


                        fila++;
                        numTesis++;
                    //}
                }
            }
            MessageBox.Show("Total de Tesis impresas " + numTesis);
        }

        private string GetMateria(long id)
        {
            int start = Convert.ToInt32(id.ToString().Substring(0, 1));

            switch (start)
            {
                case 1:
                    return "I. Constitucional";
                case 2:
                    return "II. Procesal Constitucional";
                case 4:
                    return "III. Penal";
                case 5:
                    return "IV. Administrativa";
                case 6:
                    return "V. Civil";
                case 7:
                    return "VI. Laboral";
                case 8:
                    return "VII. Conflictos competenciales";
                case 9:
                    return "VIII. Electoral";
                default:
                    return "Error";
            }
        }

        /// <summary>
        /// Creates a BackgroundWorker class to do work
        /// on a background thread.
        /// </summary>
        private void DoBackgroundWork()
        {
            BackgroundWorker worker = new BackgroundWorker();

            // Tell the worker to report progress.
            worker.WorkerReportsProgress = true;

            worker.ProgressChanged += ProgressChanged;
            worker.DoWork += DoWork;
            worker.RunWorkerCompleted += WorkerCompleted;
            worker.RunWorkerAsync();
        }

        /// <summary>
        /// The work for the BackgroundWorker to perform.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        void DoWork(object sender, DoWorkEventArgs e)
        {
            this.GeneraWord();
        }

        /// <summary>
        /// Occurs when the BackgroundWorker reports a progress.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //pbLoad.Value = e.ProgressPercentage;
        }

        /// <summary>
        /// Occurs when the BackgroundWorker has completed its work.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void WorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //_backgroundButton.IsEnabled = true;
            //pbLoad.Visibility = Visibility.Collapsed;
        }
    }
}