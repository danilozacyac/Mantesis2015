using Mantesis2015.Dto;
using ScjnUtilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mantesis2015.Model
{
    public class AclaratoriaModel
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;

        public List<Aclaratoria> GetAclaratorias()
        {
            OleDbConnection connection = new OleDbConnection(connectionString);
            OleDbCommand cmd;
            OleDbDataReader reader;

            List<Aclaratoria> notasAclara = new List<Aclaratoria>();

            try
            {
                connection.Open();

                cmd = new OleDbCommand("SELECT * FROM NoPublicacion ORDER BY IdNota", connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        Aclaratoria nota = new Aclaratoria();

                        nota.IdNota = Convert.ToInt16(reader["IdNota"]);
                        nota.ParteInicia = Convert.ToInt32(reader["ParteInicia"]);
                        nota.ParteFin = Convert.ToInt32(reader["ParteFin"]);
                        nota.Materia = Convert.ToInt32(reader["Materia"]);
                        nota.Nota = reader["Nota"].ToString();

                        notasAclara.Add(nota);
                    }
                }
                reader.Close();
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AclaratoriaModel", "MantesisQuinta");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,AclaratoriaModel", "MantesisQuinta");
            }
            finally
            {
                //reader.Close();
                connection.Close();
            }

            return notasAclara;
        }


    }
}
