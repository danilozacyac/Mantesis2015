using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Mantesis2015.Model;
using Mantesis2015.Reportes;
using MantesisCommonObjects.Dto;
using MantesisCommonObjects.MantUtilities;
using MantesisCommonObjects.Singleton;
using ScjnUtilities;

namespace Mantesis2015.Controllers
{
    public class UnaTesisController
    {
        private readonly UnaTesis unaTesis;

        private UnaTesisModel unaTesisModel;

        private TesisDto tesisMostrada = null;

        

        private string binaryMotivos;


        public UnaTesisController(UnaTesis unaTesis)
        {
            this.unaTesis = unaTesis;
        }

        public UnaTesisController(UnaTesis unaTesis, TesisDto tesisMostrada)
        {
            this.unaTesis = unaTesis;
            this.tesisMostrada = tesisMostrada;
            //permisosCon = new PermisosController(unaTesis);
        }

        public void LoadTesisWindow(long ius)
        {

            LoadComboBoxes();
            LoadTesis(ius);
            LoadNoBindingValues();
            //permisosCon.SetPermisosUnaTesis();

            //tesisMostrada.PropertyChanged += TesisDto_PropertyChanged;
            
        }

        /// <summary>
        /// Carga la información de la tesis en cada uno de los campos del formulario
        /// </summary>
        public void LoadTesis(long ius)
        {
            unaTesisModel = new UnaTesisModel();

            if (tesisMostrada == null)
            {
                tesisMostrada = unaTesisModel.CargaDatosTesisMantesisSql(ius);
            }
            else
            {
                ius = tesisMostrada.Ius;
            }

            tesisMostrada.IsEnable = unaTesis.IsTesisUpdatable;
            tesisMostrada.IsReadOnly = !unaTesis.IsTesisUpdatable;

            unaTesis.DataContext = tesisMostrada;

           

            
            
        }

        public void LoadComboBoxes()
        {
            unaTesis.CbxInstancia.ItemsSource = DatosCompartidosSingleton.Instancias;
            unaTesis.CbxFuente.ItemsSource = DatosCompartidosSingleton.Fuentes;

            unaTesis.CbxMat1.ItemsSource = MantUtils.GetMateriasForComboBox();
            unaTesis.CbxMat2.ItemsSource = MantUtils.GetMateriasForComboBox();
            unaTesis.CbxMat3.ItemsSource = MantUtils.GetMateriasForComboBox();
            unaTesis.CbxMat4.ItemsSource = MantUtils.GetMateriasForComboBox();
            unaTesis.CbxMat5.ItemsSource = MantUtils.GetMateriasForComboBox();


        }

        public void LoadNoBindingValues()
        {
            unaTesis.RbtAislada.FontWeight = System.Windows.FontWeights.Normal;
            unaTesis.RbtJurisp.FontWeight = System.Windows.FontWeights.Normal;

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
                unaTesis.RbtAislada.FontWeight = System.Windows.FontWeights.Bold;
            }
            else
            {
                unaTesis.RbtJurisp.IsChecked = true;
                unaTesis.RbtJurisp.FontWeight = System.Windows.FontWeights.Bold;
            }

            binaryMotivos = NumericUtilities.ToBinaryInvert(tesisMostrada.MotivoModificar);
            binaryArray = binaryMotivos.ToCharArray();
        }

        private char[] binaryArray;

        

        public void ImportarCambios()
        {
            
                this.tesisMostrada.Sala = Convert.ToInt16(unaTesis.CbxInstancia.SelectedValue);
                this.tesisMostrada.Fuente = Convert.ToInt16(unaTesis.CbxFuente.SelectedValue);
                this.tesisMostrada.RubroStr = StringUtilities.ConvMay(StringUtilities.QuitaCarCad(unaTesis.TxtRubro.Text));

                this.tesisMostrada.TaTj = (unaTesis.RbtAislada.IsChecked == true) ? 0 : 1;

                this.tesisMostrada.Materia1 = Convert.ToInt16(unaTesis.CbxMat1.SelectedValue);
                this.tesisMostrada.Materia2 = Convert.ToInt16(unaTesis.CbxMat2.SelectedValue);
                this.tesisMostrada.Materia3 = Convert.ToInt16(unaTesis.CbxMat3.SelectedValue);
                this.tesisMostrada.Materia4 = Convert.ToInt16(unaTesis.CbxMat4.SelectedValue);
                this.tesisMostrada.Materia5 = Convert.ToInt16(unaTesis.CbxMat5.SelectedValue);

                bool guardadoExitoso = unaTesisModel.ImportarRegistro(this.tesisMostrada);                   // Salva los cambios en las bases de Access

                if (guardadoExitoso)
                    unaTesis.Close();
                else
                {
                    MessageBox.Show("No se pudo importar la tesis, favor de volver a intentar");
                }
            
        }

        public void TesisEliminar(long ius)
        {
            ValuesMant.MotivoBaja = 0;

            DataTableReader reader = unaTesisModel.RevisaTesisReferencia(ius);

            if (reader != null)
            {
                if (reader.Read())
                {
                    unaTesis.txtRefIUS.Text = unaTesis.txtRefIUS.Text + reader["IUS"].ToString();

                    unaTesis.txtReferencia.Text = reader["Rubro"].ToString() + (Convert.ToChar(13).ToString() + Convert.ToChar(10).ToString()) + (Convert.ToChar(13).ToString() + Convert.ToChar(10).ToString());
                    unaTesis.txtReferencia.Text = unaTesis.txtReferencia.Text + reader["Texto"].ToString() + (Convert.ToChar(13).ToString() + Convert.ToChar(10).ToString());
                    unaTesis.txtReferencia.Text = unaTesis.txtReferencia.Text + reader["Prec"].ToString();

                    unaTesis.txtRefLocAbr.Text = reader["LocAbr"].ToString();

                    switch (ValuesMant.MotivoBaja)
                    {
                        case 1:
                            unaTesis.txtRefMotivo.Text = "La tesis aislada ha integrado jurisprudencia.";
                            break;
                        case 2:
                            unaTesis.txtRefMotivo.Text = "La tesis se encuentra repetida de manera idéntica en otro reistro.";
                            break;
                        case 4:
                            unaTesis.txtRefMotivo.Text = "La tesis se encuentra repetida con diferentes datos de publicación y diferentes precedentes.";
                            break;
                        case 8:
                            unaTesis.txtRefMotivo.Text = "La tesis se encuentra repetida con diferentes datos de publicación e ideénticos precedentes.";
                            break;
                        case 16:
                            unaTesis.txtRefMotivo.Text = "La tesis se ha vuelto a publicar con correcciones de la instancia judicial que emitió.";
                            break;
                        case 32:
                            unaTesis.txtRefMotivo.Text = "La tesis no corresponde a la publicación que señala, y no se pudo determinar el volumen correcto.";
                            break;
                        case 64:
                            unaTesis.txtRefMotivo.Text = "La tesis se ha vuelto a publicar con correcciones u observaciones de una instancia superior emisora.";
                            break;
                    }
                }
            }
            else
            {
                MessageBox.Show("  No existe información relacionada");
            }
        }

        #region RibbonButtons


        public void TesisToClipboard(int queMando)
        {
            switch (queMando)
            {
                case 1: // Toda la tesis
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
                                      "Nota de publicación:" + "\r\n" + unaTesis.TxtNotaPublica.Text);
                    MessageBox.Show("Tesis enviada al portapapeles");
                    break;
                case 2: // NumIus
                    Clipboard.SetText(tesisMostrada.Ius.ToString());
                    break;
                case 3: // Rubro
                    Clipboard.SetText(tesisMostrada.Rubro);
                    break;
                case 4: // Texto
                    Clipboard.SetText(tesisMostrada.Texto);
                    break;
                case 5: // Precedentes
                    Clipboard.SetText(tesisMostrada.Precedentes);
                    break;
                case 6: // Nota Rubro
                    Clipboard.SetText(tesisMostrada.NotasRubro);
                    break;
                case 7: // Nota Texto
                    Clipboard.SetText(tesisMostrada.NotasTexto);
                    break;
                case 8: // Nota Precedentes
                    Clipboard.SetText(tesisMostrada.NotasPrecedentes);
                    break;
            }
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

        public void LaunchSga()
        {
            //MateriasSgaWin sga = new MateriasSgaWin(tesisMostrada.Ius, tesisMostrada.VolumenInt, unaTesis.IsTesisUpdatable);
            //RelacionaMateriaSga sga = new RelacionaMateriaSga(tesisMostrada.Ius, tesisMostrada.VolumenInt, unaTesis.IsTesisUpdatable);
            //sga.ShowDialog();

            //if (sga.DialogResult == true)
            //{
            //    tesisMostrada.MotivoModificar += 33554432;
            //    tesisMostrada.MateriasSga = new ClasificacionSgaModel().GetMateriasRelacionadas(tesisMostrada.Ius);
            //}
            

        }

        #endregion

        #region Exportar
        
        public void ExportarOptions(string name)
        {
            switch (name)
            {
                case "RBtnPdf":
                    new ListaTesisPdf().GeneraPdfConDetalleTesis(tesisMostrada);
                    break;
                case "RBtnWord":
                    
                    new ListaTesisWord().GeneraWordConDetalleTesis(tesisMostrada);
                    break;
            }
        }
        
        #endregion

        
    }
}