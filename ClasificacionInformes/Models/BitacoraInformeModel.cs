using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MantesisVerIusCommonObjects.Dto;
using ScjnUtilities;

namespace ClasificacionInformes.Models
{
    public class BitacoraInformeModel
    {


        public static void SetNewBitacoraEntry(int ius,int movimiento,int clasifAnterior,int clasifActual)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["salasConnection"].ConnectionString);
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            string sqlCadena = "SELECT * FROM salasBitacora WHERE ius = 0";

            try
            {
                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "salasBitacora");

                dr = dataSet.Tables["salasBitacora"].NewRow();
                dr["IUS"] = ius;
                dr["Movimiento"] = movimiento;
                dr["idClasifAntes"] = clasifAnterior;
                dr["idClasifDesp"] = clasifActual;
                dr["FechaStr"] = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
                dr["usuario"] = AccesoUsuarioModel.Usuario;
                dr["UsuarioPC"] = Environment.MachineName;
                dr["idProyecto"] = ClasifVar.IdProyecto;

                dataSet.Tables["salasBitacora"].Rows.Add(dr);

                dataAdapter.InsertCommand = connection.CreateCommand();
                dataAdapter.InsertCommand.CommandText = "INSERT INTO salasBitacora(ius,Movimiento,idClasifAntes,idClasifDesp,fechaStr,usuario,usuariopc,idProyecto)" +
                                  "VALUES(@ius,@Movimiento,@idClasifAntes,@idClasifDesp,@fechaStr,@usuario,@usuariopc,@idProyecto)";

                dataAdapter.InsertCommand.Parameters.Add("@ius", SqlDbType.BigInt, 0, "ius");
                dataAdapter.InsertCommand.Parameters.Add("@Movimiento", SqlDbType.Int, 0, "Movimiento");
                dataAdapter.InsertCommand.Parameters.Add("@idClasifAntes", SqlDbType.Int, 0, "idClasifAntes");
                dataAdapter.InsertCommand.Parameters.Add("@idClasifDesp", SqlDbType.Int, 0, "idClasifDesp");
                dataAdapter.InsertCommand.Parameters.Add("@fechaStr", SqlDbType.DateTime, 0, "fechaStr");
                dataAdapter.InsertCommand.Parameters.Add("@usuario", SqlDbType.VarChar, 0, "usuario");
                dataAdapter.InsertCommand.Parameters.Add("@usuariopc", SqlDbType.VarChar, 0, "usuariopc");
                dataAdapter.InsertCommand.Parameters.Add("@idProyecto", SqlDbType.Int, 0, "idProyecto");

                dataAdapter.Update(dataSet, "salasBitacora");

                dataSet.Dispose();
                dataAdapter.Dispose();

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetClasificacion", "InformeSalas");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetClasificacion", "InformeSalas");
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
