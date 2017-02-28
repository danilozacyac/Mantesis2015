using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using Mantesis2015.Dto;
using Mantesis2015.Model;
using Mantesis2015.Reportes;
using MantesisCommonObjects.Dto;
using MantesisCommonObjects.MantUtilities;
using MantesisCommonObjects.Singleton;
using Microsoft.Win32;
using ScjnUtilities;
using Telerik.Windows.Documents.Fixed;

namespace Mantesis2015.Controllers
{
    class UnaTesisQuintaController
    {
         private readonly UnaTesisQuinta unaTesis;

        private UnaTesisModel unaTesisModel;

        private TesisQuinta tesisMostrada = null;
        private TesisQuinta tesisEdoAnterior = null;

        private bool isModifRubro = false;
        private bool isModifTexto = false;
        private bool isModifPrec = false;
        private bool isModifNotas = false;

        private bool isModifNotaRubro = false;
        private bool isModifNotaTexto = false;
        private bool isModifNotaPrec = false;
        private bool isModifNotasGaceta = false;
        private bool isModifNotaPublica = false;

        private bool isObsRubroChanged = false;
        private bool isObsTextoChanged = false;
        private bool isObsPreceChanged = false;

        private bool isSoportevisible = false;

        private string binaryMotivos;


        public UnaTesisQuintaController(UnaTesisQuinta unaTesis)
        {
            this.unaTesis = unaTesis;
            //permisosCon = new PermisosController(unaTesis);
        }

        public UnaTesisQuintaController(UnaTesisQuinta unaTesis, TesisQuinta tesisMostrada)
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

            tesisMostrada.PropertyChanged += TesisDto_PropertyChanged;
            
        }

        /// <summary>
        /// Carga la información de la tesis en cada uno de los campos del formulario
        /// </summary>
        public void LoadTesis(long ius)
        {
            unaTesisModel = new UnaTesisModel();

            if (tesisMostrada == null)
            {
                tesisMostrada = unaTesisModel.GetTesis(ius);
                tesisEdoAnterior = unaTesisModel.GetTesis(ius);
            }
            else
            {
                ius = tesisMostrada.Ius;
                tesisEdoAnterior = unaTesisModel.GetTesis(tesisMostrada.Ius);
            }

            tesisMostrada.IsEnable = unaTesis.IsTesisUpdatable;
            tesisMostrada.IsReadOnly = !unaTesis.IsTesisUpdatable;

            unaTesis.DataContext = tesisMostrada;

            if (unaTesis.ListaTesis != null && unaTesis.ListaTesis.Count > 1)
                unaTesis.LblContador.Content = String.Format("     {0} / {1}", (unaTesis.PosActual + 1), unaTesis.ListaTesis.Count);
            else
            {
                unaTesis.LblContador.Content = "    1 / 1";
                unaTesis.Navega.IsEnabled = false;
            }

            
            
        }

        public void LoadComboBoxes()
        {
            unaTesis.CbxInstancia.ItemsSource = DatosCompartidosSingleton.Instancias;
            unaTesis.CbxFuente.ItemsSource = DatosCompartidosSingleton.Fuentes;

            unaTesis.CbxMat1.ItemsSource =MantUtils.GetMateriasForComboBox();
            unaTesis.CbxMat2.ItemsSource =MantUtils.GetMateriasForComboBox();
            unaTesis.CbxMat3.ItemsSource =MantUtils.GetMateriasForComboBox();
            unaTesis.CbxMat4.ItemsSource =MantUtils.GetMateriasForComboBox();
            unaTesis.CbxMat5.ItemsSource =MantUtils.GetMateriasForComboBox();


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

            binaryMotivos = NumericUtilities.ToBinaryInvert(tesisMostrada.MotivoModificar);
            binaryArray = binaryMotivos.ToCharArray();
        }

        private char[] binaryArray;
        private string sCamposModif = "";
        public void TesisDto_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Rubro":
                    //txtRubro.Text = tesisMostrada.Rubro;
                    sCamposModif += "R";
                    isModifRubro = true;
                    unaTesis.GTesisObs.Visibility = Visibility.Visible;
                    if (binaryArray.Length >= 2)
                    {
                        if (binaryArray[2 - 1] == '0')
                            tesisMostrada.MotivoModificar += 2;
                    }
                    else
                    {
                        tesisMostrada.MotivoModificar += 2;
                    }
                    break;
                case "Texto":
                    //txtTexto.Text = tesisMostrada.Texto;
                    sCamposModif += "T";
                    isModifTexto = true;
                    unaTesis.GTesisObs.Visibility = Visibility.Visible;
                    if (binaryArray.Length >= 9)
                    {
                        if (binaryArray[9 - 1] == '0')
                            tesisMostrada.MotivoModificar += 256;
                    }
                    else
                    {
                        tesisMostrada.MotivoModificar += 256;
                        
                    }
                    break;
                case "Precedentes":
                    //txtPrec.Text = tesisMostrada.Precedentes;
                    sCamposModif += "P";
                    isModifPrec = true;
                    unaTesis.GTesisObs.Visibility = Visibility.Visible;
                    if (binaryArray.Length >= 7)
                    {
                        if (binaryArray[7 - 1] == '0')
                            tesisMostrada.MotivoModificar += 64;
                    }
                    else
                    {
                        tesisMostrada.MotivoModificar += 64;
                    }
                    break;
                case "NotasRubro":
                   // TxtNotaR.Text = tesisMostrada.NotasRubro;
                    sCamposModif += "nRub";
                    isModifNotaRubro = true;
                    if (binaryArray.Length >= 26)
                    {
                        if (binaryArray[26 - 1] == '0')
                            tesisMostrada.MotivoModificar += 268435456;
                    }
                    else
                    {
                        tesisMostrada.MotivoModificar += 268435456;
                    }
                    break;
                case "NotasTexto":
                   // TxtNotaT.Text = tesisMostrada.NotasTexto;
                    sCamposModif += "nTex";
                    isModifNotaTexto = true;
                    if (binaryArray.Length >= 27)
                    {
                        if (binaryArray[27 - 1] == '0')
                            tesisMostrada.MotivoModificar += 134217728;
                    }
                    else
                    {
                        tesisMostrada.MotivoModificar += 134217728;
                    }
                    break;
                case "NotasPrecedente":
                    //TxtNotaP.Text = tesisMostrada.NotasPrecedentes;
                    sCamposModif += "nPrec";
                    isModifNotaPrec = true;
                    if (binaryArray.Length >= 28)
                    {
                        if (binaryArray[28 - 1] == '0')
                            tesisMostrada.MotivoModificar += 134217728;
                    }
                    else
                    {
                        tesisMostrada.MotivoModificar += 134217728;
                    }
                    break;
                case "NotasGaceta":
                    //TxtNotaGaceta.Text = tesisMostrada.NotasGaceta;
                    sCamposModif += "nGac";
                    isModifNotasGaceta = true;
                    if (binaryArray.Length >= 29)
                    {
                        if (binaryArray[29 - 1] == '0')
                            tesisMostrada.MotivoModificar += 536870912;
                    }
                    else
                    {
                        tesisMostrada.MotivoModificar += 536870912;
                    }

                    tesisMostrada.IsNotasModif = true;
                    break;
                case "NotasPublica":
                    //TxtNotaPublica.Text = tesisMostrada.NotaPublica;
                    sCamposModif += "nPub";
                    isModifNotaPublica = true;
                    if (binaryArray.Length >= 30)
                    {
                        if (binaryArray[30 - 1] == '0')
                            tesisMostrada.MotivoModificar += 1073741824;
                    }
                    else
                    {
                        tesisMostrada.MotivoModificar += 1073741824;
                    }

                    tesisMostrada.IsNotasModif = true;
                    break;
                case "ObsRubro":
                    isObsRubroChanged = true;
                    break;
                case "ObsTexto":
                    isObsTextoChanged = true;
                    break;
                case "ObsPrecedentes":
                    isObsPreceChanged = true;
                    break;
                case "SoportePrincipal":
                    if (String.IsNullOrEmpty(tesisMostrada.SoportePrincipal))
                    {
                        unaTesis.RBtnAjuntarSop.IsEnabled = true;
                        unaTesis.RBtnVisualizar.IsEnabled = false;
                    }
                    else
                    {
                        unaTesis.RBtnAjuntarSop.IsEnabled = false;
                        unaTesis.RBtnVisualizar.IsEnabled = true;
                    }
                    break;
            }
        }

        public void LaunchAclaratoria()
        {
            SelectNotas nota = new SelectNotas(ref tesisMostrada) { Owner = unaTesis };
            nota.ShowDialog();
        }

        public void GuardarCambios()
        {
            if (isModifRubro && !isObsRubroChanged)
            {
                MessageBox.Show("Al modificar el título y subtítulo es necesario ingresar las observaciones pertinentes en el campo de observaciones");
                return;
            }
            

            //MModificacion modif = new MModificacion(this.tesisMostrada.MotivoModificar);
            //bool? dResult = modif.ShowDialog();

            //if (dResult == true)
            //{
                this.tesisMostrada.Sala = Convert.ToInt16(unaTesis.CbxInstancia.SelectedValue);
                this.tesisMostrada.Fuente = Convert.ToInt16(unaTesis.CbxFuente.SelectedValue);
                this.tesisMostrada.RubroStr = StringUtilities.ConvMay(StringUtilities.QuitaCarCad(unaTesis.TxtRubro.Text));

                this.tesisMostrada.TaTj = (unaTesis.RbtAislada.IsChecked == true) ? 0 : 1;

                this.tesisMostrada.Materia1 = Convert.ToInt16(unaTesis.CbxMat1.SelectedValue);
                this.tesisMostrada.Materia2 = Convert.ToInt16(unaTesis.CbxMat2.SelectedValue);
                this.tesisMostrada.Materia3 = Convert.ToInt16(unaTesis.CbxMat3.SelectedValue);
                this.tesisMostrada.Materia4 = Convert.ToInt16(unaTesis.CbxMat4.SelectedValue);
                this.tesisMostrada.Materia5 = Convert.ToInt16(unaTesis.CbxMat5.SelectedValue);

                this.tesisMostrada.Estado = 2;
                this.tesisMostrada.MotivoModificar = NumericUtilities.BinaryToDecimal(ValuesMant.BinaryVal);
                this.tesisMostrada.CamposModif = sCamposModif;

                string idAbs = Guid.NewGuid().ToString();


                bool guardadoExitoso = unaTesisModel.ActualizarTesisQuinta(this.tesisMostrada);
                //guardadoExitoso = unaTesisModel.SalvarRegistroMantesisSql(this.tesisMostrada, idAbs); // Salva los cambios en Mantesis SQL                   // Salva los cambios en las bases de Access

                
                unaTesis.Close();
           // }
        }


        #region RibbonButtons

        public void TesisStart()
        {
            tesisMostrada = null;
            unaTesis.PosActual = 0;
            AddTesis tesis = unaTesis.ListaTesis[unaTesis.PosActual];
            this.LoadTesisWindow(tesis.Ius4);

            unaTesis.LblContador.Content = String.Format("     {0} / {1}", (unaTesis.PosActual + 1), unaTesis.ListaTesis.Count); 
        }

        public void TesisPrevious()
        {
            tesisMostrada = null;
            if (unaTesis.PosActual > 0)
            {
                unaTesis.PosActual--;
                AddTesis tesis = unaTesis.ListaTesis[unaTesis.PosActual];
                this.LoadTesisWindow(tesis.Ius4);

                unaTesis.LblContador.Content = String.Format("     {0} / {1}", (unaTesis.PosActual + 1), unaTesis.ListaTesis.Count); 
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

                unaTesis.LblContador.Content = String.Format("     {0} / {1}", (unaTesis.PosActual + 1), unaTesis.ListaTesis.Count); 
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
            unaTesis.LblContador.Content = String.Format("     {0} / {1}", (unaTesis.PosActual + 1), unaTesis.ListaTesis.Count); 
        }

        public void TesisToClipboard(int queMando)
        {
            switch (queMando)
            {
                case 1: // Toda la tesis
                    Clipboard.SetText(
                                      unaTesis.TxtEpoca.Text + Environment.NewLine + "Registro: " + tesisMostrada.Ius + Environment.NewLine +
                                      "Instancia: " + unaTesis.CbxInstancia.Text + Environment.NewLine +
                                      ((unaTesis.RbtJurisp.IsChecked == true) ? "Jurisprudencia" : "Tesis Aislada") + Environment.NewLine +
                                      "Fuente: " + unaTesis.CbxFuente.Text + Environment.NewLine +
                                      unaTesis.TxtVolumen.Text + Environment.NewLine +
                                      "Materia(s): " + unaTesis.CbxMat1.Text + ((!unaTesis.CbxMat2.Text.Equals("<sin materia>"))
                                                                                ? (", " + unaTesis.CbxMat2.Text + ((!unaTesis.CbxMat3.Text.Equals("<sin materia>")) ? ", " + unaTesis.CbxMat3.Text : "")) : "") + Environment.NewLine +
                                      "Tesis: " + unaTesis.TxtTesis.Text + Environment.NewLine + "Página: " + unaTesis.TxtPag.Text + Environment.NewLine +
                                      Environment.NewLine +
                                      ((!unaTesis.TxtGenealogia.Text.Equals(String.Empty)) ? "Genealogía: " + unaTesis.TxtGenealogia.Text + Environment.NewLine : String.Empty) +
                                      tesisMostrada.Rubro + Environment.NewLine + tesisMostrada.Texto + Environment.NewLine +
                                      tesisMostrada.Precedentes + Environment.NewLine + Environment.NewLine + Environment.NewLine +
                                      ((!unaTesis.TxtObservaciones.Text.Equals(String.Empty)) ? "Notas: " + Environment.NewLine + unaTesis.TxtObservaciones.Text + Environment.NewLine + Environment.NewLine : "") +
                                      ((!unaTesis.TxtConcordancia.Text.Equals(String.Empty)) ? "Notas: " + Environment.NewLine + unaTesis.TxtConcordancia.Text + Environment.NewLine + Environment.NewLine : "") +
                                      "Nota de publicación:" + Environment.NewLine + unaTesis.TxtNotaPublica.Text);
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

        public void AdjuntaSoportePrincipal()
        {
            string filePath = this.GetFilePath();

            if (!String.IsNullOrEmpty(filePath))
            {
                string newFilePath = String.Format(ConfigurationManager.AppSettings["Soportes"] + "{0}{1}", tesisMostrada.Ius, ".pdf");

                File.Copy(filePath, newFilePath, true);

                tesisMostrada.SoportePrincipal = newFilePath;
            }
        }

        private string GetFilePath()
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                InitialDirectory = String.Format(@"C:\Users\{0}\Documents", Environment.UserName),
                Title = "Selecciona el archivo de soporte"
            };
            dialog.ShowDialog();

            string ruta = dialog.FileName;


            return ruta;
        }

        public void MuestraSoporte()
        {
            if (unaTesis.GViewer.IsVisible)
            {
                unaTesis.RBtnVisualizar.Content = "Mostrar";
                unaTesis.GViewer.Visibility = Visibility.Collapsed;
                
            }
            else
            {
                unaTesis.RBtnVisualizar.Content = "Ocultar";

                if (unaTesis.PdfViewer.DocumentSource == null)
                {
                    unaTesis.PdfViewer.DocumentSource = new PdfDocumentSource(new Uri(tesisMostrada.SoportePrincipal));
                    unaTesis.PdfViewer.Mode = Telerik.Windows.Documents.Fixed.UI.FixedDocumentViewerMode.Pan;
                }

                unaTesis.GViewer.Visibility = Visibility.Visible;
            }
        }
    }
}