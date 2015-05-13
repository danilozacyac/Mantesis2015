using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using MantesisAdminUtil.Model;
using MantesisVerIusCommonObjects.DataAccess;
using MantesisVerIusCommonObjects.Dto;
using PVolumenesControl.Models;
using ScjnUtilities;

namespace Mantesis2015.Model
{
    public class AccesoModel
    {
        public bool ObtenerUsuarioContraseña(string sUsuario, string sPwd)
        {
            bool bExisteUsuario = false;
            string sSql;

            SqlCommand cmd;
            SqlDataReader reader;
            SqlConnection connectionMantesisSql = DbConnDac.GetConnectionMantesisSql();

            try
            {
                connectionMantesisSql.Open();

                sSql = "SELECT * FROM Usuarios WHERE usuario = '" + sUsuario + "' AND Contraseña ='" + sPwd + "'";
                cmd = new SqlCommand(sSql, connectionMantesisSql);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    AccesoUsuarioModel.Usuario = reader["usuario"].ToString();
                    AccesoUsuarioModel.Pwd = reader["contraseña"].ToString();
                    AccesoUsuarioModel.Llave = Convert.ToInt16(reader["Llave"].ToString());
                    AccesoUsuarioModel.Grupo = Convert.ToInt16(reader["Grupo"].ToString());
                    AccesoUsuarioModel.Nombre = reader["nombre"].ToString();
                    AccesoUsuarioModel.Permisos = new PermisosModel().GetSeccionesByUsuario(AccesoUsuarioModel.Llave);
                    AccesoUsuarioModel.VolumenesPermitidos = new VolumenesViewModel().GetVolumenesPorUser(AccesoUsuarioModel.Llave);
                    bExisteUsuario = true;
                }
                else
                {
                    AccesoUsuarioModel.Llave = -1;
                }
            }
            catch (DbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            finally
            {
                connectionMantesisSql.Close();
            }

            return bExisteUsuario;
        }

        public bool ObtenerUsuarioContraseña()
        {
            bool bExisteUsuario = false;
            string sSql;

            SqlConnection connection = DbConnDac.GetConnectionMantesisSql();

            SqlCommand cmd;
            SqlDataReader reader;


            try
            {
                connection.Open();

                sSql = "SELECT * FROM Usuarios WHERE usuario = @usuario";// AND Pass = @Pass";
                cmd = new SqlCommand(sSql, connection);
                cmd.Parameters.AddWithValue("@usuario", Environment.UserName.ToUpper());
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    AccesoUsuarioModel.Usuario = reader["usuario"].ToString();
                    AccesoUsuarioModel.Pwd = reader["contraseña"].ToString();
                    AccesoUsuarioModel.Llave = Convert.ToInt16(reader["Llave"].ToString());
                    AccesoUsuarioModel.Grupo = Convert.ToInt16(reader["Grupo"].ToString());
                    AccesoUsuarioModel.Nombre = reader["nombre"].ToString();
                    AccesoUsuarioModel.Permisos = new PermisosModel().GetSeccionesByUsuario(AccesoUsuarioModel.Llave);
                    AccesoUsuarioModel.VolumenesPermitidos = new VolumenesViewModel().GetVolumenesPorUser(AccesoUsuarioModel.Llave);
                    bExisteUsuario = true;
                }
                else
                {
                    AccesoUsuarioModel.Llave = -1;
                }
            }
            catch (DbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            finally
            {
                connection.Close();
            }

            return bExisteUsuario;
        }
    }
}
