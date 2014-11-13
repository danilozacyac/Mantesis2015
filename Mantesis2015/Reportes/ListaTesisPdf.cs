using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Mantesis2015.Model;
using MantesisVerIusCommonObjects.Dto;
using MantesisVerIusCommonObjects.Utilities;
using ScjnUtilities;
using UtilsMantesis;

namespace Mantesis2015.Reportes
{
    public class ListaTesisPdf
    {
        private iTextSharp.text.Document myDocument;

        /// <summary>
        /// Genera un PDF con el detalle de cada una de las tesis que se encuentran en el listado de 
        /// la ventana principal
        /// </summary>
        /// <param name="materia"></param>
        /// <returns></returns>
        public int GeneraPdfConDetalleTesis(int materia)
        {
            myDocument = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);
            string documento = Path.GetTempFileName() + ".pdf";
            PdfWriter writer = PdfWriter.GetInstance(myDocument, new FileStream(documento, FileMode.Create));

            int numTesis = 0;

            myDocument.Open();

            try
            {
                List<TesisReg> tesisImprime = new ReporteModel().GetTesisPorMateria(materia);

                if (tesisImprime.Count > 0)
                {
                    numTesis += tesisImprime.Count;

                    tesisImprime.Sort(delegate(TesisReg p1, TesisReg p2)
                    {
                        return StringUtilities.QuitaCarOrden(p1.pagina).CompareTo(StringUtilities.QuitaCarOrden(p2.pagina));
                    });

                    foreach (TesisReg tesis in tesisImprime)
                    {
                        Paragraph para = new Paragraph(tesis.epoca, Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 12));
                        myDocument.Add(para);
                        para = new Paragraph("Registro: " + tesis.ius4, Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 12));
                        para.Alignment = Element.ALIGN_LEFT;
                        myDocument.Add(para);

                        para = new Paragraph("Instancia: " + tesis.sala, Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 12));
                        myDocument.Add(para);
                        para = new Paragraph(tesis.ta_tj, Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 12));
                        para.Alignment = Element.ALIGN_LEFT;
                        myDocument.Add(para);

                        para = new Paragraph("Fuente: " + tesis.fuente, Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 12));
                        myDocument.Add(para);
                        para = new Paragraph(tesis.Estado + ", " + new ReporteModel().GetTomo(tesis.SubTema), Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 12));
                        para.Alignment = Element.ALIGN_LEFT;
                        myDocument.Add(para);

                        string materiaTxt = tesis.materia1 + ", " + tesis.materia2 + ", " + tesis.materia3;

                        para = new Paragraph("Materia(s): " + materiaTxt, Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 12));
                        myDocument.Add(para);
                        para = new Paragraph("Tesis: " + tesis.tesis, Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 12));
                        para.Alignment = Element.ALIGN_LEFT;
                        myDocument.Add(para);

                        para = new Paragraph("Página: " + tesis.pagina, Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 12));
                        myDocument.Add(para);
                        para = new Paragraph(" ", Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 12));
                        para.Alignment = Element.ALIGN_LEFT;
                        myDocument.Add(para);

                        para = new Paragraph(tesis.RUBRO, Fuentes.BoldFont(Fuentes.Black, Fuentes.ArialFont, 12));
                        para.Alignment = Element.ALIGN_JUSTIFIED;
                        myDocument.Add(para);

                        para = new Paragraph(tesis.TEXTO, Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 12));
                        para.Alignment = Element.ALIGN_JUSTIFIED;
                        myDocument.Add(para);
                        para = new Paragraph(" ", Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 12));
                        para.Alignment = Element.ALIGN_CENTER;
                        myDocument.Add(para);

                        para = new Paragraph(tesis.PRECEDENTES, Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 12));
                        para.Alignment = Element.ALIGN_JUSTIFIED;
                        myDocument.Add(para);
                        para = new Paragraph(" ");
                        myDocument.Add(para);
                        para = new Paragraph(" ");
                        myDocument.Add(para);

                        myDocument.NewPage();
                    }
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
                myDocument.Close();
                System.Diagnostics.Process.Start(documento);
            }
            return numTesis;
        }

        public void GeneraPdfConDetalleTesis(TesisDto tesis)
        {
            myDocument = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);
            string documento = Path.GetTempFileName() + ".pdf";
            PdfWriter writer = PdfWriter.GetInstance(myDocument, new FileStream(documento, FileMode.Create));

            myDocument.Open();

            try
            {
                Paragraph para = new Paragraph(tesis.Epoca, Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 12));
                myDocument.Add(para);
                para = new Paragraph("Registro: " + tesis.Ius, Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 12));
                para.Alignment = Element.ALIGN_LEFT;
                myDocument.Add(para);

                para = new Paragraph("Instancia: " + Utils.GetInfoDatosCompartidos(2,tesis.Sala), Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 12));
                myDocument.Add(para);
                para = new Paragraph((tesis.TaTj == 1) ? "Jurisprudencia" : "Tesis Aislada", Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 12));
                para.Alignment = Element.ALIGN_LEFT;
                myDocument.Add(para);

                para = new Paragraph("Fuente: " + Utils.GetInfoDatosCompartidos(3,tesis.Fuente), Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 12));
                myDocument.Add(para);
                //para = new Paragraph(tesis.Estado + ", " + new ReporteModel().GetTomo(tesis.SubTema), Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 12));
                //para.Alignment = Element.ALIGN_LEFT;
                //myDocument.Add(para);

                string materiaTxt = Utils.GetInfoDatosCompartidos(4,tesis.Materia1) + ", " + Utils.GetInfoDatosCompartidos(4,tesis.Materia2)
                    + ", " + Utils.GetInfoDatosCompartidos(4,tesis.Materia3);

                para = new Paragraph("Materia(s): " + materiaTxt, Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 12));
                myDocument.Add(para);
                para = new Paragraph("Tesis: " + tesis.Tesis, Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 12));
                para.Alignment = Element.ALIGN_LEFT;
                myDocument.Add(para);

                para = new Paragraph("Página: " + tesis.Pagina, Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 12));
                myDocument.Add(para);
                para = new Paragraph(" ", Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 12));
                para.Alignment = Element.ALIGN_LEFT;
                myDocument.Add(para);

                para = new Paragraph(tesis.Rubro, Fuentes.BoldFont(Fuentes.Black, Fuentes.ArialFont, 12));
                para.Alignment = Element.ALIGN_JUSTIFIED;
                myDocument.Add(para);

                para = new Paragraph(tesis.Texto, Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 12));
                para.Alignment = Element.ALIGN_JUSTIFIED;
                myDocument.Add(para);
                para = new Paragraph(" ", Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 12));
                para.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(para);

                para = new Paragraph(tesis.Precedentes, Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 12));
                para.Alignment = Element.ALIGN_JUSTIFIED;
                myDocument.Add(para);
                para = new Paragraph(" ");
                myDocument.Add(para);
                para = new Paragraph(" ");
                myDocument.Add(para);

                myDocument.NewPage();
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            finally
            {
                myDocument.Close();
                System.Diagnostics.Process.Start(documento);
            }
        }

        /// <summary>
        /// Genera un PDF con el listado de tesis que se visualiza en la ventana principal
        /// </summary>
        /// <param name="listaTesis"></param>
        /// <param name="epoca"></param>
        /// <param name="volumen"></param>
        public void GeneraPdfListaTesis(List<AddTesis> listaTesis, string epoca, string volumen)
        {
            myDocument = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);
            string documento = Path.GetTempFileName() + ".pdf";

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(myDocument, new FileStream(documento, FileMode.Create));

                Paragraph para;

                myDocument.Open();

                if (listaTesis.Count > 0)
                {
                    myDocument.NewPage();

                    para = new Paragraph("Suprema Corte de Justicia de la Nación", Fuentes.BoldFont(Fuentes.Black, Fuentes.ArialFont, 18));
                    para.Alignment = Element.ALIGN_CENTER;
                    myDocument.Add(para);
                    para = new Paragraph("Coordinación General de Compilación y", Fuentes.BoldFont(Fuentes.Black, Fuentes.ArialFont, 14));
                    para.Add(Environment.NewLine);
                    para.Add("Sistematización de Tesis");
                    para.Alignment = Element.ALIGN_CENTER;
                    myDocument.Add(para);
                    para = new Paragraph("Reporte de las tesis dadas de alta", Fuentes.BoldFont(Fuentes.Black, Fuentes.ArialFont, 14));
                    para.Alignment = Element.ALIGN_CENTER;
                    myDocument.Add(para);

                    iTextSharp.text.Paragraph white = new iTextSharp.text.Paragraph(" ");
                    myDocument.Add(white);

                    para = new Paragraph(epoca + ". " + volumen, Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 10));
                    para.Alignment = Element.ALIGN_LEFT;
                    myDocument.Add(para);

                    white = new iTextSharp.text.Paragraph(" ");
                    myDocument.Add(white);

                    PdfPTable table = new PdfPTable(5);
                    //table.TotalWidth = 400;
                    table.WidthPercentage = 100;

                    float[] widths = new float[] { .3f, 1f, 1f, 3.2f, .5f };
                    table.SetWidths(widths);

                    table.SpacingBefore = 20f;
                    table.SpacingAfter = 30f;

                    string[] encabezado = { "#", "Registro", "Tesis", "Rubro", "Pág." };
                    PdfPCell cell;

                    foreach (string cabeza in encabezado)
                    {
                        cell = new PdfPCell(new Phrase(cabeza, Fuentes.BoldFont(Fuentes.Black, Fuentes.ArialFont, 12)));
                        cell.Colspan = 0;
                        cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                        table.AddCell(cell);
                    }
                    int consec = 1;
                    foreach (AddTesis tesis in listaTesis)
                    {
                        string[] descs = { consec.ToString(), tesis.Ius4.ToString(), tesis.Tesis, tesis.Rubro, tesis.Pagina };

                        foreach (string desc in descs)
                        {
                            cell = new PdfPCell(new Phrase(desc, Fuentes.NormalFont(Fuentes.Black, Fuentes.ArialFont, 12)));
                            cell.Colspan = 0;
                            cell.HorizontalAlignment = (desc.Length > 20) ? 3 : 1; //0=Left, 1=Centre, 2=Right
                            table.AddCell(cell);
                        }
                        consec++;
                    }

                    myDocument.Add(table);
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
                myDocument.Close();
                System.Diagnostics.Process.Start(documento);
            }
        }
    }
}