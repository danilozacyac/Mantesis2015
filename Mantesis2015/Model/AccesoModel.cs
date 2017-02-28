using System;
using System.Data.SqlClient;
using System.Linq;
using AuthManager.Models;
using MantesisCommonObjects.DataAccess;
using MantesisCommonObjects.Dto;
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

                sSql = "SELECT * FROM Usuarios WHERE usuario = @Usuario AND Contraseña = @Pwd";
                cmd = new SqlCommand(sSql, connectionMantesisSql);
                cmd.Parameters.AddWithValue("@Usuario", sUsuario);
                cmd.Parameters.AddWithValue("@Pwd", sPwd);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    AccesoUsuarioModel.Usuario = reader["usuario"].ToString();
                    AccesoUsuarioModel.Pwd = reader["contraseña"].ToString();
                    AccesoUsuarioModel.Llave = Convert.ToInt16(reader["Llave"].ToString());
                    AccesoUsuarioModel.Grupo = Convert.ToInt16(reader["Grupo"].ToString());
                    AccesoUsuarioModel.Nombre = reader["nombre"].ToString();
                    AccesoUsuarioModel.Permisos = new PermisosSeccionModel().GetSeccionesByUsuario(AccesoUsuarioModel.Llave);
                    AccesoUsuarioModel.VolumenesPermitidos = new VolumenesViewModel().GetVolumenesPorUser(AccesoUsuarioModel.Llave);
                    bExisteUsuario = true;
                }
                else
                {
                    AccesoUsuarioModel.Llave = -1;
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AccesoModel", "MantesisQuinta");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AccesoModel", "MantesisQuinta");
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
                    AccesoUsuarioModel.Llave = Convert.ToInt16(reader["Llave"]);
                    AccesoUsuarioModel.Grupo = Convert.ToInt16(reader["Grupo"]);
                    AccesoUsuarioModel.Nombre = reader["nombre"].ToString();
                    AccesoUsuarioModel.Permisos = new PermisosSeccionModel().GetSeccionesByUsuario(AccesoUsuarioModel.Llave);
                    AccesoUsuarioModel.VolumenesPermitidos = new VolumenesViewModel().GetVolumenesPorUser(AccesoUsuarioModel.Llave);
                    bExisteUsuario = true;
                }
                else
                {
                    AccesoUsuarioModel.Llave = -1;
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AccesoModel", "MantesisQuinta");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AccesoModel", "MantesisQuinta");
            }
            finally
            {
                connection.Close();
            }

            return bExisteUsuario;
        }
    }
}
