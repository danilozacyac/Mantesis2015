using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Mantesis2015.Controllers;
using MantesisCommonObjects.Dto;
using Telerik.Windows.Controls;

namespace Mantesis2015
{
    /// <summary>
    /// Lógica de interacción para UnaTesis.xaml
    /// </summary>
    public partial class UnaTesis : Window
    {

        private UnaTesisController controller;

        private long ius;
        private readonly byte accion;
        public List<AddTesis> ListaTesis;
        public int PosActual;
        public readonly bool IsTesisUpdatable;
        private readonly bool isVerIusAccess;
        private TesisDto tesisMostrada;

        public UnaTesis()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ius">Número de registro IUS de la tesis a mostrar</param>
        /// <param name="accion">Como se abrira el registro.   1  Visualiza  ----  2  Actualiza  ---- 3 Nueva tesis</param>
        /// <param name="listaTesis">Lista de Tesis mostrada en la ventana principal</param>
        /// <param name="posActual">Posición de la tesis dentro del listado mostrado</param>
        /// <param name="isTesisUpdatable"></param>
        public UnaTesis(long ius, byte accion, List<AddTesis> listaTesis, int posActual, bool isTesisUpdatable)
        {
            InitializeComponent();
            this.ius = ius;
            this.accion = accion;
            this.PosActual = posActual;
            this.ListaTesis = listaTesis;
            this.IsTesisUpdatable = isTesisUpdatable;
            this.isVerIusAccess = false;
            controller = new UnaTesisController(this);
            
        }

        /// <summary>
        /// Constructor que utiliza la funcionalidad de Ver IUS para lanzar la una nueva ventana con la tesis buscada
        /// </summary>
        /// <param name="tesisMostrada"></param>
        /// <param name="isTesisUpdatable"></param>
        public UnaTesis(TesisDto tesisMostrada, bool isTesisUpdatable)
            : this()
        {
            this.tesisMostrada = tesisMostrada;
            this.ius = tesisMostrada.Ius;
            this.IsTesisUpdatable = isTesisUpdatable;
            isVerIusAccess = true;
            controller = new UnaTesisController(this,tesisMostrada);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            controller.LoadTesisWindow(this.ius);
            this.Title = (IsTesisUpdatable) ? "Actualizar Tesis" : "Visualizar Tesis";

            
        }

        private void RibbonButton_Click(object sender, RoutedEventArgs e)
        {
            RadRibbonButton ribbon = sender as RadRibbonButton;

            switch (ribbon.Name)
            {
                
                case "RBtnClipTesis": controller.TesisToClipboard(1);
                    break;
                case "RBtnLigas": controller.LaunchLigasPreview();
                    break;
                case "RBtnBitacora": controller.LaunchBitacora();
                    break;
                case "RBtnClipIus": controller.TesisToClipboard(2);
                    break;
                case "RBtnClipRubro": controller.TesisToClipboard(3);
                    break;
                case "RBtnClipTexto": controller.TesisToClipboard(4);
                    break;
                case "RBtnClipPrece": controller.TesisToClipboard(5);
                    break;
                case "RBtnSalir": this.Close();
                    break;
                case "RBtnMateriasSga": controller.LaunchSga();
                    break;
            }
        }

        

        private void ExportarGroupClick(object sender, RoutedEventArgs e)
        {
            RadRibbonButton action = sender as RadRibbonButton;
            controller.ExportarOptions(action.Name);

        }

        private void RBtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            //controller.GuardarCambios();
        }

        private void BtnTesisElim_Click(object sender, RoutedEventArgs e)
        {
            controller.TesisEliminar(this.ius);
        }

        private void TextBoxCh(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            //controller.CambioCuadroTexto(sender, e);
        }

        private void BtnImportar_Click(object sender, RoutedEventArgs e)
        {
            controller.ImportarCambios();
        }

        

        
    }
}
