using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using MantesisCommonObjects.DataAccess;
using MantesisCommonObjects.Dto;
using MantesisCommonObjects.MantUtilities;
using ScjnUtilities;

namespace Mantesis2015.Model
{
    public class ReporteModel
    {
        

        public List<TesisReg> GetTesis()
        {
            List<TesisReg> tesisReg = new List<TesisReg>();

            SqlConnection lConn = DbConnDac.GetConnectionIus();
            SqlCommand cmd;
            SqlDataReader reader = null;

            try
            {
                lConn.Open();
                string sqlCadena = "SELECT IdMatSGA,T.* FROM Tesis T INNER JOIN Tesis_MatSGA M ON T.IUS = M.IUS " +
                                   "WHERE T.Volumen = @Volumen";

                cmd = new SqlCommand(sqlCadena, lConn);
                cmd.Parameters.AddWithValue("@Volumen", ValuesMant.Volumen);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    String volumen = MantUtils.GetInfoVolumenes(ValuesMant.Volumen);

                    tesisReg.Add(new TesisReg(
                        Convert.ToInt32(reader["IUS"]),
                        reader["rubro"].ToString(),
                        reader["texto"].ToString(),
                        reader["Prec"].ToString(),
                        MantUtils.GetInfoDatosCompartidos(1, Convert.ToInt16(reader["epoca"])),
                        MantUtils.GetInfoDatosCompartidos(2, Convert.ToInt16(reader["sala"])),
                        MantUtils.GetInfoDatosCompartidos(3, Convert.ToInt16(reader["fuente"])),
                        volumen,
                        reader["tesis"].ToString(),
                        reader["pagina"].ToString(),
                        (Convert.ToInt16(reader["ta_tj"]) == 0) ? "Tesis Aislada" : "Jurisprudencia",
                        MantUtils.GetInfoDatosCompartidos(4, Convert.ToInt16(reader["materia1"])),
                        MantUtils.GetInfoDatosCompartidos(4, Convert.ToInt16(reader["materia2"])),
                        MantUtils.GetInfoDatosCompartidos(4, Convert.ToInt16(reader["materia3"])),
                        volumen,
                        "",
                        reader["RIndx"].ToString(),
                        Convert.ToInt32(reader["idSubVolumen"]),
                        Convert.ToInt64(reader["IdMatSGA"])));
                }

                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReporteModel", "MantesisQuinta");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReporteModel", "MantesisQuinta");
            }
            finally
            {
                reader.Close();
                lConn.Close();
            }
            return tesisReg;
        }

        public int GetNoTesis()
        {
            int cTesis = 0;

            DbConnection lConn = DbConnDac.GetConnectionIus();

            string sqlCadena = "Select COUNT(*) cuantas from Tesis_MatSGA WHERE Volumen = " + ValuesMant.Volumen;

            DataTableReader reader = this.GetDatosTabla(sqlCadena, lConn);

            if (reader.HasRows)
            {
                reader.Read();
                cTesis = Convert.ToInt32(reader["cuantas"].ToString());
            }

            return cTesis;
        }

        public List<long> GetidMatSgaMes()
        {
            List<long> ids = new List<long>();

            DbConnection lConn = DbConnDac.GetConnectionIus();

            string sqlCadena = String.Format("Select distinct M.Id,M.Consec FROM cMateriasSGA M INNER JOIN Tesis_MatSGA T ON M.ID = T.idMatSGA WHERE VOLUMEN = {0} Order By M.Consec ", ValuesMant.Volumen);

            DataTableReader reader = this.GetDatosTabla(sqlCadena, lConn);

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ids.Add(Convert.ToInt32(reader["id"].ToString()));
                }
            }

            return ids;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="materia"></param>
        /// <returns></returns>
        public List<TesisReg> GetTesisPorMateria(int materia)
        {
            List<TesisReg> tesisReg = new List<TesisReg>();
            string sqlCadena = "";

            DbConnection lConn = DbConnDac.GetConnectEpocas(ValuesMant.Epoca, ValuesMant.Volumen, 1);

            if (materia == 99) //Imprime todas las tesis
            {
                sqlCadena = "Select * from Tesis WHERE volumen = " + ValuesMant.Volumen;
            }
            else
            {
                sqlCadena = String.Format("Select * from Tesis WHERE volumen = {0}AND (materia1 = {1} OR materia2 = {1} OR materia3 = {1} )", ValuesMant.Volumen, materia);
            }

            try
            {
                DataTableReader reader = this.GetDatosTabla(sqlCadena, lConn);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        long ius = Convert.ToInt64(reader["ius4"]);
                        String volumen = MantUtils.GetInfoVolumenes(ValuesMant.Volumen);

                        tesisReg.Add(new TesisReg(
                            ius,
                            reader["rubro"].ToString(),
                            reader["texto"].ToString(),
                            reader["Precedentes"].ToString(),
                            MantUtils.GetInfoDatosCompartidos(1, Convert.ToInt16(reader["epoca"])),
                            MantUtils.GetInfoDatosCompartidos(2, Convert.ToInt16(reader["sala"])),
                            MantUtils.GetInfoDatosCompartidos(3, Convert.ToInt16(reader["fuente"])),
                            volumen,
                            reader["tesis"].ToString(),
                            reader["pagina"].ToString(),
                            (Convert.ToInt16(reader[((Convert.ToInt32(reader["epoca"]) != ConstMantesis.Decima) ? "ta/tj" : "tatj")]) == 0) ? "Tesis Aislada" : "Jurisprudencia",
                            MantUtils.GetInfoDatosCompartidos(4, Convert.ToInt16(reader["materia1"])),
                            MantUtils.GetInfoDatosCompartidos(4, Convert.ToInt16(reader["materia2"])),
                            MantUtils.GetInfoDatosCompartidos(4, Convert.ToInt16(reader["materia3"])),
                            volumen,
                            "",
                            reader["RubroStr"].ToString(),
                            Convert.ToInt32(reader["idSubVolumen"])));
                    }
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReporteModel", "MantesisQuinta");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReporteModel", "MantesisQuinta");
            }
            finally
            {
                lConn.Close();
            }

            return tesisReg;
        }

        public string GetTomo(int nSubVolumen)
        {
            string localizacion = "";

            SqlConnection lConn = DbConnDac.GetConnectionMantesisSql();

            string sqlCadena = String.Format("SELECT * FROM subVolumen WHERE idVolumen = {0} AND idSubVolumen = {1}", ValuesMant.Volumen, nSubVolumen);

            try
            {
                DataTableReader reader = this.GetDatosTabla(sqlCadena, lConn);

                if (reader.HasRows)
                {
                    reader.Read();
                    localizacion = reader["txtVolumen"].ToString();
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReporteModel", "MantesisQuinta");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReporteModel", "MantesisQuinta");
            }
            finally
            {
                lConn.Close();
            }

            return localizacion;
        }




        private DataTableReader GetDatosTabla(string sqlString, DbConnection lConn)
        {
            DataTableReader dtr = null;
            try
            {
                //lConn.Open();
                DataAdapter query = DbConnDac.GetDataAdapter(sqlString, lConn);
                dtr = DbConnDac.GetDatosReader(query);
            }
            catch (DbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReporteModel", "MantesisQuinta");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ReporteModel", "MantesisQuinta");
            }
            return dtr;
        }
    }
}
