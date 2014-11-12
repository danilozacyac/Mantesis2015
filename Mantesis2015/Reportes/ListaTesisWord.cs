using System;
using System.Collections.Generic;
using System.Linq;
using UtilsMantesis;
using Microsoft.Office.Interop.Word;
using System.Windows.Forms;
using ScjnUtilities;
using System.Diagnostics;
using System.IO;

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

        public void GeneraPdfListaTesis(List<AddTesis> listaTesis, string epoca, string volumen)
        {
            oWord = new Microsoft.Office.Interop.Word.Application();
            oDoc = oWord.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);
            oDoc.PageSetup.Orientation = WdOrientation.wdOrientLandscape;

            //Insert a paragraph at the beginning of the document.
            Microsoft.Office.Interop.Word.Paragraph oPara1;
            oPara1 = oDoc.Content.Paragraphs.Add(ref oMissing);
            oPara1.Range.Text = "Suprema Corte de Justicia de la Nación (" + DateTimeUtilities.ToMonthName(DateTime.Now.Month - 1) + " " + DateTime.Now.Year + ")";
            oPara1.Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
            oPara1.Range.Font.Bold = 1;
            oPara1.Range.Font.Name = "Times New Roman";
            oPara1.Format.SpaceAfter = 24;    //24 pt spacing after paragraph.
            oPara1.Range.InsertParagraphAfter();

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
                    footerRange.Font.ColorIndex = WdColorIndex.wdDarkRed;
                    footerRange.Font.Size = 10;
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

    }
}