using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MantesisVerIusCommonObjects.Dto;
using ScjnUtilities;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Mantesis2015.Reportes
{
    public class EstadoTesis
    {
        private readonly TesisDto estadoAnterior;
        private readonly TesisDto estadoActual;

        private iTextSharp.text.Document myDocument;
        private readonly Paragraph blankParagraph = new Paragraph("    ");

        public EstadoTesis(TesisDto estadoAnterior, TesisDto estadoActual)
        {
            this.estadoAnterior = estadoAnterior;
            this.estadoActual = estadoActual;
        }

        /// <summary>
        /// Genera un pdf que muestra los diferentes estados de una tesis durante su actualización,
        /// es decir, muestra el estado anterior de la misma y el estado posterior a la modificación 
        /// de la información
        /// </summary>
        public void GeneraPdf()
        {
            try
            {
                this.VerificacionFilesPath();

                myDocument = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);

                string documento = ConfigurationManager.AppSettings["CertificationPath"].ToString()
                    + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + estadoActual.Ius + ".pdf";

                PdfWriter writer = PdfWriter.GetInstance(myDocument, new FileStream(documento, FileMode.Create));
                HeaderFooter pdfPage = new HeaderFooter();
                writer.PageEvent = pdfPage;

                myDocument.Open();

                Paragraph para = new Paragraph("Estado Anterior", Fuentes.BoldFont(Fuentes.Black, "Arial", 16));
                para.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(para);

                this.PrintInfoOnPdf(estadoAnterior);

                myDocument.NewPage();

                para = new Paragraph("Estado después de modificación", Fuentes.BoldFont(Fuentes.Black, "Arial", 16));
                para.Alignment = Element.ALIGN_CENTER;
                myDocument.Add(para);

                this.PrintInfoOnPdf(estadoActual);

                myDocument.Close();
                System.Diagnostics.Process.Start(documento);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            
        }

        private void PrintInfoOnPdf(TesisDto tesis)
        {
            this.SetInfo(String.Empty, tesis.Epoca);
            this.SetInfo(String.Empty, tesis.Ius.ToString());
            this.SetInfo("Instancia: ", "");
            this.SetInfo(String.Empty,(tesis.TaTj == 1) ? "Jurisprudencia" : "Tesis Aislada");
            this.SetInfo("Fuente: ", "");
            this.SetInfo(String.Empty, tesis.Volumen);
            this.SetInfo("Materia(s): ", "");
            this.SetInfo("Tesis: ",  tesis.Tesis);
            this.SetInfo("Página: ", tesis.Pagina);
            myDocument.Add(blankParagraph);

            this.SetInfo(String.Empty, tesis.Rubro);
            myDocument.Add(blankParagraph);

            this.SetInfo(String.Empty, tesis.Texto);
            myDocument.Add(blankParagraph);

            this.SetInfo(String.Empty, tesis.Precedentes);
            myDocument.Add(blankParagraph);

            this.SetInfo(String.Empty, tesis.NotaPublica);
            myDocument.Add(blankParagraph);

            this.SetInfo("Nota Gaceta: ", String.Empty);
            this.SetInfo(String.Empty, tesis.NotasGaceta);

            this.SetInfo("Nota al pie de rubro: ", String.Empty);
            this.SetInfo(String.Empty, tesis.NotasRubro);

            this.SetInfo("Nota al pie de texto: ", String.Empty);
            this.SetInfo(String.Empty, tesis.NotasTexto);

            this.SetInfo("Nota al pie de precedente: ", String.Empty);
            this.SetInfo(String.Empty, tesis.NotasPrecedentes);

            this.SetInfo("Genealogía: ", String.Empty);
            this.SetInfo(String.Empty, tesis.Genealogia);

            this.SetInfo("Observaciones:", String.Empty);
            this.SetInfo(String.Empty, tesis.Observaciones);

            this.SetInfo("Concordancia: ", String.Empty);
            this.SetInfo(String.Empty, tesis.NotasPrecedentes);

            this.SetInfo("Clasificación de Materias SGA:", String.Empty);

            foreach (string clasif in tesis.MateriasSga)
            {
                this.SetInfo(String.Empty, clasif);
            }
            
        }


        private void SetInfo(string header, string texto)
        {
            Paragraph para = new Paragraph(header + texto);
            para.Alignment = Element.ALIGN_JUSTIFIED;
            myDocument.Add(para);

        }


        /// <summary>
        /// Verifica que exista la carpeta donde se guardarán los archivos de certificación,
        /// de no existir se crea
        /// </summary>
        private void VerificacionFilesPath()
        {
            string filePath = ConfigurationManager.AppSettings["CertificationPath"].ToString();

            if (!File.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            
        }

    }
}
