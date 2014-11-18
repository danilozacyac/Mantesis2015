using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using Infragistics.Windows.Ribbon;
using Mantesis2015.Controllers;
using Mantesis2015.Model;
using MantesisVerIusCommonObjects.Dto;
using MateriasSgaControl;
using Microsoft.Windows.Controls.Ribbon;
using UtilsMantesis;

namespace Mantesis2015
{
    /// <summary>
    /// Lógica de interacción para UnaTesis.xaml
    /// </summary>
    public partial class UnaTesis : Window
    {

        private UnaTesisController controller;

        private UnaTesisModel unaTesisModel; 


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
            
        }

        private void RibbonButton_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton ribbon = sender as RibbonButton;

            switch (ribbon.Name)
            {
                case "RbtnInicio": controller.TesisStart();
                    break;
                case "RbtnPrevious": controller.TesisPrevious();
                    break;
                case "RbtnNext": controller.TesisNext();
                    break;
                case "RbtnFin": controller.TesisEnd();
                    break;
                case "RbtnClipboard": controller.TesisToClipboard(1);
                    break;
                case "RbtnLigas": controller.LaunchLigasPreview();
                    break;
                case "RbtnBitacora": controller.LaunchBitacora();
                    break;
                case "BtnCIus": controller.TesisToClipboard(2);
                    break;
                case "BtnCRubro": controller.TesisToClipboard(3);
                    break;
                case "BtnCTexto": controller.TesisToClipboard(4);
                    break;
                case "BtnCPrec": controller.TesisToClipboard(5);
                    break;
                case "RBtnSalir": this.Close();
                    break;
            }
        }

        private void ExportarGroupClick(object sender, RoutedEventArgs e)
        {
            RibbonButton action = sender as RibbonButton;
            controller.ExportarOptions(action.Name);

        }

        private void RBtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            controller.GuardarCambios();
        }

        private void BtnTesisElim_Click(object sender, RoutedEventArgs e)
        {
            controller.TesisEliminar(this.ius);
        }
    }
}
