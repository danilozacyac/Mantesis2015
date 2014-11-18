using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Mantesis2015.Model;
using MantesisAdminUtil.Dto;

namespace Mantesis2015.Controllers
{
    public class PermisosController
    {

        MainWindow main;

        public PermisosController(MainWindow main)
        {
            this.main = main;
        }

        #region Permisos Ventana Principal

        public void LoadPermission()
        {
            if (AccesoUsuarioModel.Permisos.Count > 0)
            {

                foreach (Secciones seccion in AccesoUsuarioModel.Permisos)
                {
                    List<int> permisosSeccion = MantesisAdminUtil.Utils.GetDecimalsInBinary(seccion.Permisos);

                    if (seccion.IdSeccion == 2)
                    {
                        main.EpocaVolumen.IsEnabled = (permisosSeccion.Contains(1)) ? true : false;
                    }
                    else if (seccion.IdSeccion == 3)
                    {
                        main.VerIus.Tag = permisosSeccion;
                        main.VerIus.IsEnabled = (permisosSeccion.Contains(1)) ? true : false;
                    }
                    else if (seccion.IdSeccion == 5)
                    {
                        main.BtnVisualizaTesis.IsEnabled = (permisosSeccion.Contains(1)) ? true : false;
                        main.BtnNuevaTesis.IsEnabled = (permisosSeccion.Contains(2)) ? true : false;
                        main.BtnActualizaTesis.IsEnabled = (permisosSeccion.Contains(4)) ? true : false;
                        main.BtnEliminaTesis.IsEnabled = (permisosSeccion.Contains(8)) ? true : false;

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
