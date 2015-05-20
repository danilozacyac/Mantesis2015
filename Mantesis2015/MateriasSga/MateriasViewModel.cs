using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using MantesisVerIusCommonObjects.Dto;
using ScjnUtilities;

namespace Mantesis2015.MateriasSga
{
    public class MateriasViewModel
    {
        static readonly string connectionString = ConfigurationManager.ConnectionStrings["BaseIUS"].ToString();


        public static List<MateriasModel> GetEstructuraNivel(int padre, bool isReadOnly)
        {
            List<MateriasModel> listaMaterias = new List<MateriasModel>();

            SqlConnection sqlConne = new SqlConnection(connectionString);// (SqlConnection)DbConnDac.GetConnectionIus();

            SqlCommand cmd;
            SqlDataReader dataReader;
            string miQry;

            try
            {
                sqlConne.Open();
                miQry = "Select * FROM cMateriasSGA WHERE padre = " + padre + " ORDER BY Consec";
                cmd = new SqlCommand(miQry, sqlConne);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    MateriasModel materia = new MateriasModel(dataReader["Descripcion"].ToString(),
                        GetEstructuraNivel(Convert.ToInt32(dataReader["ID"].ToString()),isReadOnly), Convert.ToInt32(dataReader["ID"].ToString()));

                    materia.IsReadOnly = isReadOnly;

                    listaMaterias.Add(materia);
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButton .OK, MessageBoxImage.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButton.OK, MessageBoxImage.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            finally
            {
                sqlConne.Close();
            }

            return listaMaterias;
        }

        public static List<int> GetMateriasRelacionadas(long ius, int volumen)
        {
            List<int> listaMaterias = new List<int>();

            SqlConnection sqlConne = new SqlConnection(connectionString);// (SqlConnection)DbConnDac.GetConnectionIus();

            SqlCommand cmd;
            SqlDataReader dataReader;
            string miQry;

            try
            {
                sqlConne.Open();
                miQry = "Select idMatSGA FROM Tesis_MatSGA WHERE ius = @ius AND Volumen = @Volumen";// +RequestData.Volumen;
                cmd = new SqlCommand(miQry, sqlConne);
                cmd.Parameters.AddWithValue("@ius", ius);
                cmd.Parameters.AddWithValue("@Volumen", volumen);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    listaMaterias.Add(Convert.ToInt32(dataReader["idMatSGA"].ToString()));
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButton.OK, MessageBoxImage.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButton.OK, MessageBoxImage.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            finally
            {
                sqlConne.Close();
            }

            return listaMaterias;
        }

        /// <summary>
        /// Devuelve la lista de materias del catálogo SGA asociadas a una tesis en particular, 
        /// se utiliza para el reporte generado después de que se realizan cambios en la tesis
        /// </summary>
        /// <param name="ius">Número de registro digital de la tesis</param>
        /// <returns></returns>
        public static List<string> GetMateriasRelacionadas(long ius)
        {
            List<string> listaMaterias = new List<string>();

            SqlConnection sqlConne = new SqlConnection(connectionString);

            SqlCommand cmd;
            SqlDataReader dataReader;
            string miQry;

            try
            {
                sqlConne.Open();
                miQry = "SELECT T.IdMatSGA,C.descripcion FROM cMateriasSGA C INNER JOIN Tesis_MatSGA T ON T.IdMatSGA = C.ID " +
                        " WHERE IUS = @ius";
                cmd = new SqlCommand(miQry, sqlConne);
                cmd.Parameters.AddWithValue("@ius", ius);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    string materia = dataReader["IdMatSGA"].ToString() + "    " + dataReader["Descripcion"].ToString();

                    listaMaterias.Add(materia);
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButton.OK, MessageBoxImage.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButton.OK, MessageBoxImage.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            finally
            {
                sqlConne.Close();
            }

            return listaMaterias;
        }

        public static void SetRelacionMateriasIus(long ius, List<int> materias, int volumen)
        {
            SqlConnection sqlConne = new SqlConnection(connectionString);// (SqlConnection)DbConnDac.GetConnectionIus();
            SqlCommand cmd;

            cmd = sqlConne.CreateCommand();
            cmd.Connection = sqlConne;

            try
            {
                sqlConne.Open();

                cmd.CommandText = "DELETE FROM Tesis_MatSGA WHERE ius = " + ius + " AND Volumen = " + volumen;
                cmd.ExecuteNonQuery();

                foreach (int materia in materias)
                {
                    int tomo = Convert.ToInt32(materia.ToString().Substring(0, 3));
                    cmd.CommandText = "INSERT INTO Tesis_MatSGA VALUES(" + ius + "," + materia + "," + tomo + ",'"
                        + DateTime.Now.ToString() + "','Mantesis',0," + AccesoUsuarioModel.Llave + ",'" + AccesoUsuarioModel.Nombre
                        + "'," + volumen + ")";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "INSERT INTO Bitacora_MateriasSGA VALUES(" + ius + "," + materia + "," + tomo + ",'Mantesis',0,"
                        + volumen + ",0," + AccesoUsuarioModel.Llave + ",'" + AccesoUsuarioModel.Nombre
                        + "',GETDATE())";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButton.OK, MessageBoxImage.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButton.OK, MessageBoxImage.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            finally
            {
                sqlConne.Close();
            }
        }


    }
}
