using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using ClasificacionInformes.Dto;
using ScjnUtilities;

namespace ClasificacionInformes.Models
{
    public class ClasificacionModel
    {

        public List<ClasificacionDto> GetClasificacion(int idPadre, int seleccionado, int tatj)
        {
            List<ClasificacionDto> temas = new List<ClasificacionDto>();

            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["salasConnection"].ConnectionString);
            SqlCommand cmd;
            SqlDataReader dataReader;

            string sqlCadena = "SELECT * FROM salasClasificaciones WHERE padre = " + idPadre;

            if (tatj == 1)
            {
                sqlCadena += " AND (tpoTesis = 1 OR tpoTesis =2) ";
            }
            else if (tatj == 0)
            {
                sqlCadena += " AND (tpoTesis = 0 OR tpoTesis =2) ";
            }

            sqlCadena += " ORDER BY idClasif";

            try
            {
                connection.Open();

                cmd = new SqlCommand(sqlCadena, connection);
                dataReader = cmd.ExecuteReader();

                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        ClasificacionDto tema = new ClasificacionDto();
                        tema.IdClasif = Convert.ToInt32(dataReader["idClasif"]);
                        tema.IsSelected = (tema.IdClasif == seleccionado) ? true : false;
                        tema.Descripcion = dataReader["Descripcion"].ToString();
                        tema.Nivel = Convert.ToInt32(dataReader["nivel"]);
                        tema.Padre = Convert.ToInt32(dataReader["padre"]);
                        tema.Estado = Convert.ToBoolean(dataReader["estado"]);

                        temas.Add(tema);
                    }
                }
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


            return temas;
        }


    }
}
