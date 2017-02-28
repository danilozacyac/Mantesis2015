using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Mantesis2015.Model;
using MantesisCommonObjects.Dto;
using MantesisCommonObjects.MantUtilities;
using MantesisCommonObjects.Model;
using ScjnUtilities;

namespace Mantesis2015.UserControls.Controller
{
    public class ListaTesisController
    {
        readonly ListaDeTesis listaTesisWindow;

        AddTesis selectedTesis;



        /// <summary>
        /// Listado de tesis que se muestran en el listado principal
        /// </summary>
        private List<AddTesis> listaTesis;

        /// <summary>
        /// Registro digital de la tesis seleccionada
        /// </summary>
        private long selectedIus;

        /// <summary>
        /// Número de fila de la tesis seleccionada
        /// </summary>
        private readonly int selectedRowIndex;

        public ListaTesisController(ListaDeTesis listaTesisWindow)
        {
            this.listaTesisWindow = listaTesisWindow;
        }


        public void GetTesisImportadas()
        {
            ListaTesisModel listaTesisModel = new ListaTesisModel();
            listaTesis = listaTesisModel.CargaTesisImportadas(0);

            

            listaTesisWindow.GTesis.DataContext = listaTesis;
            listaTesisWindow.GTesis.Focus();
        }


        public void TesisSeleccionada()
        {
            selectedTesis = listaTesisWindow.GTesis.SelectedItem as AddTesis;
            selectedIus = selectedTesis.Ius4;
        }

        /// <summary>
        /// Devuelve una lista con las tesis que cumplen con el criterio de filtrado
        /// </summary>
        /// <param name="criterioFiltrado">Materia o volumen por el cual se solicita el filtro</param>
        public void GetTesisFiltradas(int criterioFiltrado)
        {
            ListaTesisModel listaTesisModel = new ListaTesisModel();
            listaTesis = listaTesisModel.CargaTesisMantesisSql(criterioFiltrado);

            listaTesisWindow.GTesis.DataContext = listaTesis;
            listaTesisWindow.GTesis.Focus();
            //if (listaTesisWindow.XamDataGridTesis.Records.Count == 0)
            //{
            //    MessageBox.Show("No existen registros");
            //}
            //else
            //{
            //    listaTesisWindow.XamDataGridTesis.ActiveRecord = listaTesisWindow.XamDataGridTesis.Records[0];
            //    listaTesisWindow.TxtRegs.Content = RegLoc + listaTesis.Count;
            //}
        }

        /// <summary>
        /// Realiza la búsqueda de una tesis por su número de registro digital y lanza una nueva ventana mostrando la tesis
        /// </summary>
        /// <param name="txtNumIus"></param>
        public void GetTesisByVerIus(string txtNumIus)
        {
           // bool updatablePerm = ((List<int>)listaTesisWindow.VerIus.Tag).Contains(4);

            AddTesis tesisExiste = listaTesis.FirstOrDefault(x => x.Ius4 == Convert.ToInt64(txtNumIus));

            if (tesisExiste != null)
            {
                MessageBox.Show("La tesis solicitada ya fue importaba, búscala en la lista del lado derecho", "Atención", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                if (txtNumIus.Length < 8)
                {
                    NumIusModel numIusModel = new NumIusModel();

                    bool isTesisEliminated = numIusModel.GetCurrentTesisState(Convert.ToInt32(txtNumIus));
                    TesisDto tesis;

                    if (isTesisEliminated)//La tesis ya fue eliminada
                    {
                        tesis = numIusModel.BuscaTesisEliminadasPorRegistro(Convert.ToInt32(txtNumIus));

                        if (tesis != null)
                        {
                            tesis.IsEliminated = isTesisEliminated;

                            List<Volumen> volPerm = AccesoUsuarioModel.VolumenesPermitidos.Where(x => x.Volumenes == tesis.VolumenInt).ToList();

                            MessageBox.Show("Esta tesis fue eliminada");

                            UnaTesis unaTesis = new UnaTesis(tesis, false) { Tag = listaTesisWindow.VerIus.Tag };
                            unaTesis.ShowDialog();
                        }
                        else
                        {
                            tesis = numIusModel.BuscaTesis(Convert.ToInt32(txtNumIus));

                            List<Volumen> volPerm = AccesoUsuarioModel.VolumenesPermitidos.Where(x => x.Volumenes == tesis.VolumenInt).ToList();

                            if (tesis != null)
                            {
                                tesis.IsEliminated = isTesisEliminated;
                                MessageBox.Show("Esta tesis fue eliminada");

                                UnaTesis unaTesis = new UnaTesis(tesis, false) { Tag = listaTesisWindow.VerIus.Tag };
                                unaTesis.ShowDialog();
                            }
                            else
                            {
                                MessageBox.Show("Introduzca un número de registro valido");
                            }
                        }
                    }
                    else
                    {
                        tesis = numIusModel.BuscaTesis(Convert.ToInt32(txtNumIus));

                        

                        //List<Volumen> volPerm = AccesoUsuarioModel.VolumenesPermitidos.Where(x => x.Volumenes == tesis.VolumenInt).ToList();

                        if (tesis.Parte >= 100 && tesis.Parte <= 145)
                        {
                            ValuesMant.Epoca = 7;
                            ValuesMant.Volumen = tesis.VolumenInt;
                        }

                        if (tesis != null)
                        {
                            if (tesis.EpocaInt != 1 && (tesis.Parte < 100 || tesis.Parte > 145))
                            {
                                MessageBox.Show("El número de registro que se ingresó no pertenece a la Quinta Época ni a ninguno de los Apéndices");
                                return;
                            }

                            UnaTesis unaTesis = new UnaTesis(tesis, false) { Tag = listaTesisWindow.VerIus.Tag };
                            unaTesis.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Introduzca un número de registro valido");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Introduzca un número de registro valido");
                }
            }
            catch (NullReferenceException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ListaTesisController", "MantesisQuinta");
            }
        }


        /// <summary>
        /// Permite observar todos los detalles de la tesis, además de sus datos de publicación
        /// NO permite actulizar ningún dato de publicación
        /// </summary>
        /// <param name="materiasEstado">Indica si las materias SGA pueden ser actualizadas o no</param>
        /// <param name="isTesisUpdatable">Indica si la tesis puede ser actualizada o no</param>
        /// <param name="mainWindow">Ventana propietaria de la que esta por ser lanzada</param>
        public void MostrarTesis(byte materiasEstado, bool isTesisUpdatable,MainWindow mainWindow)
        {
            if (listaTesis != null && listaTesis.Count > 0)
            {
                ValuesMant.IusActualLstTesis = this.selectedIus;
                UnaTesisQuinta fUnaTesis = new UnaTesisQuinta(this.selectedIus, materiasEstado, listaTesis, this.selectedRowIndex, isTesisUpdatable) { Owner = mainWindow };
                fUnaTesis.ShowDialog();

                //MoveGridToIus(ValuesMant.IusActualLstTesis);
            }
        }

        public void ExportarOptions(string id)
        {
            //switch (id)
            //{
            //    case "RBtnPdf":
            //        MessageBoxResult result = MessageBox.Show("¿Desea generar el reporte con el detalle de las tesis? Si su respuesta es NO solo se generará el listado de tesis", "Atención:", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            //        if (result == MessageBoxResult.Yes)
            //            new ListaTesisPdf().GeneraPdfConDetalleTesis(Convert.ToInt32(listaTesisWindow.CbxMaterias.SelectedValue));
            //        else if (result == MessageBoxResult.No)
            //            new ListaTesisPdf().GeneraPdfListaTesis(listaTesis, listaTesisWindow.CbEpoca.Text, listaTesisWindow.CbxVolumen.Text);
            //        break;
            //    case "RBtnWord":
            //        MessageBoxResult result2 = MessageBox.Show("¿Desea generar el reporte con el detalle de las tesis? Si su respuesta es NO solo se generará el listado de tesis", "Atención:", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            //        if (result2 == MessageBoxResult.Yes)
            //            new ListaTesisWord().GeneraWordConDetalleTesis(Convert.ToInt32(listaTesisWindow.CbxMaterias.SelectedValue));
            //        else if (result2 == MessageBoxResult.No)
            //            new ListaTesisWord().GeneraWordListaTesis(listaTesis, listaTesisWindow.CbEpoca.Text, listaTesisWindow.CbxVolumen.Text);
            //        break;
            //    case "RBtnSga":
            //        TablaSga tabla = new TablaSga();
            //        tabla.GeneraReporte();
            //        break;
            //}
        }

    }
}
