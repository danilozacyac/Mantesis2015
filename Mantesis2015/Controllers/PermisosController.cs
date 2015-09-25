using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Mantesis2015.UserControls;
using MantesisVerIusCommonObjects.Dto;

namespace Mantesis2015.Controllers
{
    public class PermisosController
    {

        private readonly MainWindow main;
        private readonly ListaDeTesis listaTesisWindow;

        public PermisosController(MainWindow main)
        {
            this.main = main;
        }

        public PermisosController(ListaDeTesis listaTesisWindow)
        {
            this.listaTesisWindow = listaTesisWindow;
        }

        #region Permisos Ventana Principal

        public void LoadAdminPermission()
        {
            if (AccesoUsuarioModel.Permisos.Count > 0)
            {

                foreach (Secciones seccion in AccesoUsuarioModel.Permisos)
                {
                    List<int> permisosSeccion = MantesisAdminUtil.Utils.GetDecimalsInBinary(seccion.Permisos);

                    if (seccion.IdSeccion == 1)
                    {
                        main.GroupPermisosVolumen.IsEnabled = (permisosSeccion.Contains(1)) ? true : false;
                    }
                    else if (seccion.IdSeccion == 14)
                    {
                        main.GroupPermisosSecciones.IsEnabled = (permisosSeccion.Contains(1)) ? true : false;
                    }
                    
                    
                }
            }
            else
            {
                MessageBox.Show("No tienes autorización para utilizar ninguna de las características de este programa, ponte en contacto con tu administrador");

            }
        }
        
        public void LoadPermission()
        {

            if (AccesoUsuarioModel.Permisos.Count > 0)
            {

                foreach (Secciones seccion in AccesoUsuarioModel.Permisos)
                {
                    List<int> permisosSeccion = MantesisAdminUtil.Utils.GetDecimalsInBinary(seccion.Permisos);

                    if (seccion.IdSeccion == 1)
                    {
                        main.RBtnPermisos.IsEnabled = (permisosSeccion.Contains(1)) ? true : false;
                    }
                    else if (seccion.IdSeccion == 5)
                    {
                        main.BtnVisualizaTesis.IsEnabled = (permisosSeccion.Contains(1)) ? true : false;
                        main.BtnNuevaTesis.IsEnabled = (permisosSeccion.Contains(2)) ? true : false;
                        main.BtnActualizaTesis.IsEnabled = (permisosSeccion.Contains(4)) ? true : false;
                        main.BtnEliminaTesis.IsEnabled = (permisosSeccion.Contains(8)) ? true : false;

                    }
                    else if (seccion.IdSeccion == 9)
                    {
                        main.InformeTab.IsEnabled = (permisosSeccion.Contains(1)) ? true : false;
                    }
                    else if (seccion.IdSeccion == 14)
                    {
                        main.RBtnVolumenes.IsEnabled = (permisosSeccion.Contains(1)) ? true : false;
                    }
                    else if (seccion.IdSeccion == 25)
                    {
                        main.BtnStructMateriasSga.IsEnabled = (permisosSeccion.Contains(1)) ? true : false;
                        main.BtnEditStructSga.IsEnabled = (permisosSeccion.Contains(2)) ? true : false;
                        main.BtnPrintSgaPdf.IsEnabled = (permisosSeccion.Contains(16)) ? true : false;
                        main.BtnPrintSgaWord.IsEnabled = (permisosSeccion.Contains(16)) ? true : false;
                    }
                    else if (seccion.IdSeccion == 26)
                    {
                        main.GroupVerificadores.IsEnabled = (permisosSeccion.Contains(1)) ? true : false;
                    }
                }
            }
            else
            {
                MessageBox.Show("No tienes autorización para utilizar ninguna de las características de este programa, ponte en contacto con tu administrador");

            }
        }

        public void LoadListaTesisControlAuth()
        {

            if (AccesoUsuarioModel.Permisos.Count > 0)
            {

                foreach (Secciones seccion in AccesoUsuarioModel.Permisos)
                {
                    List<int> permisosSeccion = MantesisAdminUtil.Utils.GetDecimalsInBinary(seccion.Permisos);

                    
                    if (seccion.IdSeccion == 2)
                    {
                        listaTesisWindow.EpocaVolumen.IsEnabled = (permisosSeccion.Contains(1)) ? true : false;
                    }
                    else if (seccion.IdSeccion == 3)
                    {
                        listaTesisWindow.VerIus.Tag = permisosSeccion;
                        listaTesisWindow.VerIus.IsEnabled = (permisosSeccion.Contains(1)) ? true : false;
                    }
                    
                }
            }
            else
            {
                MessageBox.Show("No tienes autorización para utilizar ninguna de las características de este programa, ponte en contacto con tu administrador");

            }
        }

        #endregion

        #region Permisos UnaTesis

        private readonly UnaTesis unaTesis;

        public PermisosController(UnaTesis unaTesis)
        {
            this.unaTesis = unaTesis;
        }

        public void SetPermisosUnaTesis()
        {
            if (AccesoUsuarioModel.Permisos.Count > 0)
            {

                foreach (Secciones seccion in AccesoUsuarioModel.Permisos)
                {
                    List<int> permisosSeccion = MantesisAdminUtil.Utils.GetDecimalsInBinary(seccion.Permisos);

                    if (seccion.IdSeccion == 11)
                    {
                        unaTesis.RbtnLigas.IsEnabled = (permisosSeccion.Contains(1)) ? true : false;
                    }
                    if (seccion.IdSeccion == 12)
                    {
                        unaTesis.RbtnBitacora.IsEnabled = (permisosSeccion.Contains(1)) ? true : false;
                        unaTesis.RbtnBitacora.Tag = permisosSeccion;
                    }
                    else if (seccion.IdSeccion == 13)
                    {
                        unaTesis.RBtnSga.Tag = permisosSeccion;
                        unaTesis.RBtnSga.IsEnabled = (permisosSeccion.Contains(1)) ? true : false;
                    }
                    
                }
            }
            else
            {
                MessageBox.Show("No tienes autorización para utilizar ninguna de las características de este programa, ponte en contacto con tu administrador");

            }
        }


        #endregion
    }
}
