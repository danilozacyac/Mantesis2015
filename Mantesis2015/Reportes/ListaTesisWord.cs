using System;
using System.Collections.Generic;
using System.Linq;
using UtilsMantesis;
using Microsoft.Office.Interop.Word;
using System.Windows.Forms;
using ScjnUtilities;
using System.Diagnostics;
using System.IO;
using Mantesis2015.Model;
using MantesisVerIusCommonObjects.Dto;
using MantesisVerIusCommonObjects.Utilities;

namespace Mantesis2015.Reportes
{
    public class ListaTesisWord
    {
        readonly string filepath = Path.GetTempFileName() + ".docx";

        int fila = 1;

        Microsoft.Office.Interop.Word.Application oWord;
        Microsoft.Office.Interop.Word.Document oDoc;
        object oMissing = System.Reflection.Missing.Value;
        object oEndOfDoc = "\\endofdoc";

        Microsoft.Office.Interop.Word.Table oTable;

        public void GeneraWordListaTesis(List<AddTesis> listaTesis, string epoca, string volumen)
        {
            oWord = new Microsoft.Office.Interop.Word.Application();
            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc.PageSetup.Orientation = WdOrientation.wdOrientLandscape;

            //Insert a paragraph at the beginning of the document.
            //Microsoft.Office.Interop.Word.Paragraph oPara1;
            //oPara1 = oDoc.Content.Paragraphs.Add(ref oMissing);
            //oPara1.Range.Text = "Suprema Corte de Justicia de la Nación (" + DateTimeUtilities.ToMonthName(DateTime.Now.Month - 1) + " " + DateTime.Now.Year + ")";
            //oPara1.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            //oPara1.Range.Font.Bold = 1;
            //oPara1.Range.Font.Name = "Times New Roman";
            //oPara1.Format.SpaceAfter = 24;    //24 pt spacing after paragraph.
            //oPara1.Range.InsertParagraphAfter();

            Microsoft.Office.Interop.Word.Range wrdRng = oDoc.Bookmarks.get_Item(ref oEndOfDoc).Range;

            oTable = oDoc.Tables.Add(wrdRng, (listaTesis.Count + 1), 5, ref oMissing, ref oMissing);
            oTable.Range.ParagraphFormat.SpaceAfter = 6;
            oTable.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            oTable.Range.Font.Size = 9;
            oTable.Range.Font.Bold = 0;
            oTable.Borders.Enable = 1;

            oTable.Columns[1].SetWidth(40, WdRulerStyle.wdAdjustSameWidth);
            oTable.Columns[2].SetWidth(70, WdRulerStyle.wdAdjustSameWidth);
            oTable.Columns[3].SetWidth(90, WdRulerStyle.wdAdjustSameWidth);
            oTable.Columns[4].SetWidth(420, WdRulerStyle.wdAdjustSameWidth);
            oTable.Columns[5].SetWidth(40, WdRulerStyle.wdAdjustSameWidth);

            oTable.Cell(fila, 1).Range.Text = "#";
            oTable.Cell(fila, 2).Range.Text = "Núm. Registro";
            oTable.Cell(fila, 3).Range.Text = "Tesis";
            oTable.Cell(fila, 4).Range.Text = "Rubro";
            oTable.Cell(fila, 5).Range.Text = "Página";

            for (int x = 1; x < 6; x++)
            {
                oTable.Cell(fila, x).Range.Font.Size = 10;
                oTable.Cell(fila, x).Range.Font.Bold = 1;
            }

            fila++;

            try
            {
                oWord.Visible = true;

                foreach (AddTesis tesis in listaTesis)
                {
                    oTable.Cell(fila, 1).Range.Text = tesis.ID.ToString();
                    oTable.Cell(fila, 2).Range.Text = tesis.Ius4.ToString();
                    oTable.Cell(fila, 3).Range.Text = tesis.Tesis;
                    oTable.Cell(fila, 4).Range.Text = tesis.Rubro;
                    oTable.Cell(fila, 4).Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
                    oTable.Cell(fila, 5).Range.Text = tesis.Pagina;

                    fila++;
                }

                foreach (Section wordSection in oDoc.Sections)
                {
                    object pagealign = WdPageNumberAlignment.wdAlignPageNumberRight;
                    object firstpage = true;
                    wordSection.Footers[WdHeaderFooterIndex.wdHeaderFooterEvenPages].PageNumbers.Add(ref pagealign, ref firstpage);

                    Range footerRange = wordSection.Headers[WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                    footerRange.Paragraphs.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    footerRange.Font.ColorIndex = WdColorIndex.wdBlack;
                    footerRange.Font.Size = 12;
                    footerRange.Text = "Reporte de las tesis dadas de alta en el " + volumen;
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
                oDoc.Close();

                Process.Start(filepath);
            }

            MessageBox.Show("Total de Tesis impresas " + listaTesis.Count);
        }

        public void GeneraWordConDetalleTesis(int materia)
        {
            oWord = new Microsoft.Office.Interop.Word.Application();
            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            try
            {
                Microsoft.Office.Interop.Word.Paragraph par = oDoc.Content.Paragraphs.Add(ref oMissing);

                List<TesisReg> tesisImprime = new ReporteModel().GetTesisPorMateria(materia);

                foreach (TesisReg tesis in tesisImprime)
                {
                    par.Range.Font.Bold = 0;
                    par.Range.Font.Size = 10;
                    par.Range.Font.Name = "Arial";
                    //par.Range.Text = tesis.Epoca;
                    par.Range.Text = "Décima Época";
                    par.Range.InsertParagraphAfter();

                    par.Range.Text = "Registro: " + tesis.ius4;
                    par.Range.InsertParagraphAfter();

                    par.Range.Text = "Instancia: " + tesis.sala;
                    par.Range.InsertParagraphAfter();

                    par.Range.Text = tesis.ta_tj;
                    par.Range.InsertParagraphAfter();

                    par.Range.Text = "Fuente: " + tesis.fuente;
                    par.Range.InsertParagraphAfter();

                    par.Range.Text = tesis.Estado + ", " + new ReporteModel().GetTomo(tesis.SubTema);
                    par.Range.InsertParagraphAfter();

                    string materiaStr = "";

                    if (!tesis.materia1.Equals(""))
                    {
                        materiaStr += tesis.materia1;

                        if (!tesis.materia2.Equals("<sin materia>"))
                        {
                            materiaStr += ", " + tesis.materia2;

                            if (!tesis.materia3.Equals("<sin materia>"))
                            {
                                materiaStr += ", " + tesis.materia3;
                            }
                        }
                    }

                    par.Range.Text = "Materia(s): " + materiaStr;
                    par.Range.InsertParagraphAfter();

                    par.Range.Text = "Tesis: " + tesis.tesis;
                    par.Range.InsertParagraphAfter();

                    par.Range.Text = "Página: " + tesis.pagina;
                    par.Range.InsertParagraphAfter();
                    par.Range.InsertParagraphAfter();

                    par.Range.Font.Bold = 1;
                    par.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
                    par.Range.Text = tesis.RUBRO;
                    par.Range.InsertParagraphAfter();

                    par.Range.Font.Bold = 0;
                    par.Range.Text = tesis.TEXTO;
                    par.Range.InsertParagraphAfter();
                    par.Range.InsertParagraphAfter();

                    par.Range.Text = tesis.PRECEDENTES;
                    par.Range.InsertParagraphAfter();

                    oDoc.Words.Last.InsertBreak(Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak);
                }

                //Agregando esto para guardar hasta el inicio del catch
                SaveFileDialog cuadroDialogo = new SaveFileDialog();
                cuadroDialogo.DefaultExt = "docx";
                cuadroDialogo.Filter = "docx file(*.docx)|*.docx";
                cuadroDialogo.AddExtension = true;
                cuadroDialogo.RestoreDirectory = true;
                cuadroDialogo.Title = "Guardar";
                cuadroDialogo.InitialDirectory = @"D:\RESPALDO\SEMANARI\";
                if (cuadroDialogo.ShowDialog() == DialogResult.OK)
                {
                    oWord.ActiveDocument.SaveAs(cuadroDialogo.FileName);
                    oWord.ActiveDocument.Saved = true;
                    cuadroDialogo.Dispose();
                    cuadroDialogo = null;
                }
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
            }
        }

        public void GeneraWordConDetalleTesis(TesisDto tesis)
        {
            oWord = new Microsoft.Office.Interop.Word.Application();
            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);

            try
            {
                Microsoft.Office.Interop.Word.Paragraph par = oDoc.Content.Paragraphs.Add(ref oMissing);

                par.Range.Font.Bold = 0;
                par.Range.Font.Size = 10;
                par.Range.Font.Name = "Arial";
                par.Range.Text = tesis.Epoca;
                //par.Range.Text = "Décima Época";
                par.Range.InsertParagraphAfter();

                par.Range.Text = "Registro: " + tesis.Ius;
                par.Range.InsertParagraphAfter();

                par.Range.Text = "Instancia: " + Utils.GetInfoDatosCompartidos(2,tesis.Sala);
                par.Range.InsertParagraphAfter();

                par.Range.Text = tesis.TaTj == 1 ? "Jurisprudencia" : "Tesis Aislada";
                par.Range.InsertParagraphAfter();

                par.Range.Text = "Fuente: " + Utils.GetInfoDatosCompartidos(3,tesis.Fuente);
                par.Range.InsertParagraphAfter();

                //par.Range.Text = tesis.Estado + ", " + new ReporteModel().GetTomo(tesis.SubTema);
                //par.Range.InsertParagraphAfter();

                string materiaStr = "";

                if (!tesis.Materia1.Equals(""))
                {
                    materiaStr += Utils.GetInfoDatosCompartidos(4,tesis.Materia1);

                    if (!tesis.Materia2.Equals("<sin materia>"))
                    {
                        materiaStr += ", " + Utils.GetInfoDatosCompartidos(4, tesis.Materia2);

                        if (!tesis.Materia3.Equals("<sin materia>"))
                        {
                            materiaStr += ", " + Utils.GetInfoDatosCompartidos(4,tesis.Materia3);
                        }
                    }
                }

                par.Range.Text = "Materia(s): " + materiaStr;
                par.Range.InsertParagraphAfter();

                par.Range.Text = "Tesis: " + tesis.Tesis;
                par.Range.InsertParagraphAfter();

                par.Range.Text = "Página: " + tesis.Pagina;
                par.Range.InsertParagraphAfter();
                par.Range.InsertParagraphAfter();

                par.Range.Font.Bold = 1;
                par.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
                par.Range.Text = tesis.Rubro;
                par.Range.InsertParagraphAfter();

                par.Range.Font.Bold = 0;
                par.Range.Text = tesis.Texto;
                par.Range.InsertParagraphAfter();
                par.Range.InsertParagraphAfter();
                
                par.Range.Text = tesis.Precedentes;
                par.Range.InsertParagraphAfter();


                //Agregando esto para guardar hasta el inicio del catch
                SaveFileDialog cuadroDialogo = new SaveFileDialog();
                cuadroDialogo.DefaultExt = "docx";
                cuadroDialogo.Filter = "docx file(*.docx)|*.docx";
                cuadroDialogo.AddExtension = true;
                cuadroDialogo.RestoreDirectory = true;
                cuadroDialogo.Title = "Guardar";
                cuadroDialogo.InitialDirectory = @"D:\RESPALDO\SEMANARI\";
                if (cuadroDialogo.ShowDialog() == DialogResult.OK)
                {
                    oWord.ActiveDocument.SaveAs(cuadroDialogo.FileName);
                    oWord.ActiveDocument.Saved = true;
                    cuadroDialogo.Dispose();
                    cuadroDialogo = null;
                }
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
            }
        }
    }
}