using System;
using System.Configuration;
using System.Linq;
using System.Windows;
using Mantesis2015.Model;
using MantesisVerIusCommonObjects.Dto;
using MantesisVerIusCommonObjects.Singletons;
using MantesisVerIusCommonObjects.Utilities;
using UtilsMantesis;

namespace Mantesis2015.Controllers
{
    public class UnaTesisController
    {

        private readonly UnaTesis unaTesis;

        private UnaTesisModel unaTesisModel; 

        private TesisDto tesisMostrada = null;
        private TesisDto tesisEdoAnterior = null;


        public UnaTesisController(UnaTesis unaTesis)
        {
            this.unaTesis = unaTesis;
        }

        public void LoadTesisWindow(long ius)
        {
            LoadComboBoxes();
            LoadTesis(ius);
            LoadNoBindingValues();
        }

        /// <summary>
        /// Carga la información de la tesis en cada uno de los campos del formulario
        /// </summary>
        public void LoadTesis(long ius)
        {
            unaTesisModel = new UnaTesisModel();

            tesisMostrada = unaTesisModel.CargaDatosTesisMantesisSql(ius);
            tesisEdoAnterior = unaTesisModel.CargaDatosTesisMantesisSql(ius);

            unaTesis.DataContext = tesisMostrada;
        }

        public void LoadComboBoxes()
        {
            unaTesis.CbxInstancia.ItemsSource = DatosCompartidosSingleton.Instancias;
            unaTesis.CbxFuente.ItemsSource = DatosCompartidosSingleton.Fuentes;

            unaTesis.CbxMat1.ItemsSource = Utils.GetMateriasForComboBox();
            unaTesis.CbxMat2.ItemsSource = Utils.GetMateriasForComboBox();
            unaTesis.CbxMat3.ItemsSource = Utils.GetMateriasForComboBox();
            unaTesis.CbxMat4.ItemsSource = Utils.GetMateriasForComboBox();
            unaTesis.CbxMat5.ItemsSource = Utils.GetMateriasForComboBox();
        }

        public void LoadNoBindingValues()
        {
            unaTesis.RbtAislada.FontWeight = FontWeights.Normal;
            unaTesis.RbtJurisp.FontWeight = FontWeights.Normal;

            unaTesis.CbxInstancia.SelectedValue = tesisMostrada.Sala;
            unaTesis.CbxFuente.SelectedValue = tesisMostrada.Fuente;

            unaTesis.CbxMat1.SelectedValue = tesisMostrada.Materia1;
            unaTesis.CbxMat2.SelectedValue = tesisMostrada.Materia2;
            unaTesis.CbxMat3.SelectedValue = tesisMostrada.Materia3;
            unaTesis.CbxMat4.SelectedValue = tesisMostrada.Materia4;
            unaTesis.CbxMat5.SelectedValue = tesisMostrada.Materia5;

            if (tesisMostrada.TaTj == 0)
            {
                unaTesis.RbtAislada.IsChecked = true;
                unaTesis.RbtAislada.FontWeight = FontWeights.Bold;
            }
            else
            {
                unaTesis.RbtJurisp.IsChecked = true;
                unaTesis.RbtJurisp.FontWeight = FontWeights.Bold;
            }
        }








        #region RibbonButtons

        public void TesisStart()
        {
            tesisMostrada = null;
            unaTesis.PosActual = 0;
            AddTesis tesis = unaTesis.ListaTesis[unaTesis.PosActual];
            //unaTesisModel.DbConnectionOpen();
            this.LoadTesisWindow(tesis.Ius4);
            //unaTesisModel.DbConnectionClose();

            //lblContador.Text = (lnPosActual + 1) + "/" + nTamTesis;
        }

        public void TesisPrevious()
        {
            tesisMostrada = null;
            if (unaTesis.PosActual > 0)
            {
                unaTesis.PosActual--;
                AddTesis tesis = unaTesis.ListaTesis[unaTesis.PosActual];

                //unaTesisModel.DbConnectionOpen();
                this.LoadTesisWindow(tesis.Ius4);
                //unaTesisModel.DbConnectionClose();

                //lblContador.Text = (unaTesis.PosActual + 1) + "/" + nTamTesis;
            }
        }

        public void TesisNext()
        {
            tesisMostrada = null;
            if (unaTesis.PosActual < unaTesis.ListaTesis.Count - 1)
            {
                unaTesis.PosActual++;
                AddTesis tesis = unaTesis.ListaTesis[unaTesis.PosActual];

                //unaTesisModel.DbConnectionOpen();
                this.LoadTesisWindow(tesis.Ius4);
                //unaTesisModel.DbConnectionClose();

                //lblContador.Text = (unaTesis.PosActual + 1) + "/" + nTamTesis;
            }
        }

        public void TesisEnd()
        {
            tesisMostrada = null;

            unaTesis.PosActual = unaTesis.ListaTesis.Count - 1;
            AddTesis tesis = unaTesis.ListaTesis[unaTesis.PosActual];

            //unaTesisModel.DbConnectionOpen();
            this.LoadTesisWindow(tesis.Ius4);
            //unaTesisModel.DbConnectionClose();
            //lblContador.Text = (unaTesis.PosActual + 1) + "/" + nTamTesis;
        }


        public void TesisToClipboard()
        {
            Clipboard.SetText(
                             unaTesis.TxtEpoca.Text + "\r\n" + "Registro: " + tesisMostrada.Ius + "\r\n" +
                             "Instancia: " + unaTesis.CbxInstancia.Text + "\r\n" +
                             ((unaTesis.RbtJurisp.IsChecked == true) ? "Jurisprudencia" : "Tesis Aislada") + "\r\n" +
                             "Fuente: " + unaTesis.CbxFuente.Text + "\r\n" +
                             unaTesis.TxtVolumen.Text + "\r\n" +
                             "Materia(s): " + unaTesis.CbxMat1.Text + ((!unaTesis.CbxMat2.Text.Equals("<sin materia>"))
                                                              ? (", " + unaTesis.CbxMat2.Text + ((!unaTesis.CbxMat3.Text.Equals("<sin materia>")) ? ", " + unaTesis.CbxMat3.Text : "")) : "") + "\r\n" +
                             "Tesis: " + unaTesis.TxtTesis.Text + "\r\n" + "Página: " + unaTesis.TxtPag.Text + "\r\n" +
                             "\r\n" +
                             ((!unaTesis.TxtGenealogia.Text.Equals(String.Empty)) ? "Genealogía: " + unaTesis.TxtGenealogia.Text + "\r\n" : String.Empty) +
                             tesisMostrada.Rubro + "\r\n" + tesisMostrada.Texto + "\r\n" +
                             tesisMostrada.Precedentes + "\r\n" + "\r\n" + "\r\n" +
                             ((!unaTesis.TxtObservaciones.Text.Equals(String.Empty)) ? "Notas: " + "\r\n" + unaTesis.TxtObservaciones.Text + "\r\n" + "\r\n" : "") +
                             ((!unaTesis.TxtConcordancia.Text.Equals(String.Empty)) ? "Notas: " + "\r\n" + unaTesis.TxtConcordancia.Text + "\r\n" + "\r\n" : "") +
                             "Nota de publicación:" + "\r\n" + unaTesis.TxtNotaPublica.Text
                             );

            MessageBox.Show("Tesis enviada al portapapeles");
        }

        public void LaunchLigasPreview()
        {
            String url = ConfigurationManager.AppSettings.Get("UrlLigas");
            System.Diagnostics.Process.Start("IExplore.exe", url + tesisMostrada.Ius);
        }

        public void LaunchBitacora()
        {
            BitacoraWin bitacora = new BitacoraWin(tesisMostrada.Ius);
            bitacora.ShowDialog();
        }

        #endregion
    }
}
