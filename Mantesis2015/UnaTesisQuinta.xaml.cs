using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Mantesis2015.Controllers;
using Mantesis2015.Dto;
using MantesisCommonObjects.Dto;
using Telerik.Windows.Controls;
using Telerik.Windows.Documents.Fixed;

namespace Mantesis2015
{
    /// <summary>
    /// Interaction logic for UnaTesisQuinta.xaml
    /// </summary>
    public partial class UnaTesisQuinta
    {
        private UnaTesisQuintaController controller;

        private long ius;
        private readonly byte accion;
        public List<AddTesis> ListaTesis;
        public int PosActual;
        public readonly bool IsTesisUpdatable;
        private readonly bool isVerIusAccess;
        private TesisQuinta tesisMostrada;

        public UnaTesisQuinta()
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
        public UnaTesisQuinta(long ius, byte accion, List<AddTesis> listaTesis, int posActual, bool isTesisUpdatable)
        {
            InitializeComponent();
            this.ius = ius;
            this.accion = accion;
            this.PosActual = posActual;
            this.ListaTesis = listaTesis;
            this.IsTesisUpdatable = isTesisUpdatable;
            this.isVerIusAccess = false;
            controller = new UnaTesisQuintaController(this);
            
        }

        /// <summary>
        /// Constructor que utiliza la funcionalidad de Ver IUS para lanzar la una nueva ventana con la tesis buscada
        /// </summary>
        /// <param name="tesisMostrada"></param>
        /// <param name="isTesisUpdatable"></param>
        public UnaTesisQuinta(TesisQuinta tesisMostrada, bool isTesisUpdatable)
            : this()
        {
            this.tesisMostrada = tesisMostrada;
            this.ius = tesisMostrada.Ius;
            this.IsTesisUpdatable = isTesisUpdatable;
            isVerIusAccess = true;
            controller = new UnaTesisQuintaController(this, tesisMostrada);
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            controller.LoadTesisWindow(this.ius);
            this.Header = (IsTesisUpdatable) ? "Actualizar Tesis" : "Visualizar Tesis";
        }

       

        private void RibbonButton_Click(object sender, RoutedEventArgs e)
        {
            RadRibbonButton ribbon = sender as RadRibbonButton;

            switch (ribbon.Name)
            {
                case "RBtnInicio": controller.TesisStart();
                    break;
                case "RBtnAnterior": controller.TesisPrevious();
                    break;
                case "RBtnSiguiente": controller.TesisNext();
                    break;
                case "RBtnFin": controller.TesisEnd();
                    break;
                case "RBtnClipTesis": controller.TesisToClipboard(1);
                    break;
                case "RBtnLigas": controller.LaunchLigasPreview();
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
                
            }
        }

        

        private void ExportarGroupClick(object sender, RoutedEventArgs e)
        {
            RadRibbonButton action = sender as RadRibbonButton;
            controller.ExportarOptions(action.Name);

        }

        private void RBtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            controller.GuardarCambios();
        }

        

        private void TextBoxCh(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            //controller.CambioCuadroTexto(sender, e);
        }

        private void RBtnVisualizar_Click(object sender, RoutedEventArgs e)
        {
            controller.MuestraSoporte();
        }

        private void RBtnAjuntarSop_Click(object sender, RoutedEventArgs e)
        {
            controller.AdjuntaSoportePrincipal();
        }

        private void BtnAclaratoria_Click(object sender, RoutedEventArgs e)
        {
            controller.LaunchAclaratoria();
        }

        

        

        
    }
}
