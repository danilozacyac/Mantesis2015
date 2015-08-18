using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using InformesSalas.Dto;
using ScjnUtilities;

namespace InformesSalas.Models
{
    public class ProyectoModel
    {
        public List<ProyectoDto> GetProyectos()
        {
            List<ProyectoDto> proyectos = new List<ProyectoDto>();

            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["salasConnection"].ConnectionString);
            SqlCommand cmd;
            SqlDataReader dataReader;

            string sqlCadena = "SELECT SP.*,SC.Condicion,(SELECT Condicion FROM salasCondiciones WHERE idProyecto = SP.anio) VolumenCondicion " +
                " FROM salasProyectos SP INNER JOIN salasCondiciones SC ON SP.idProyecto = SC.idproyecto";

            try
            {
                connection.Open();
                cmd = new SqlCommand(sqlCadena, connection);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    proyectos.Add(
                                  new ProyectoDto(
                                                  Convert.ToInt32(dataReader["idProyecto"].ToString()),
                                                  dataReader["descripcion"].ToString(), Convert.ToInt16(dataReader["Sala"]),
                                                  Convert.ToInt32(dataReader["VolumenInicio"]),
                                                  Convert.ToInt32(dataReader["VolumenFinal"]),
                                                  dataReader["Condicion"].ToString(),
                                                  dataReader["VolumenCondicion"].ToString()
                                                  )
                                 );
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetProyectos", "InformeSalas");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetProyectos", "InformeSalas");
            }
            finally
            {
                connection.Close();
            }

            return proyectos;
        }
    }
}
