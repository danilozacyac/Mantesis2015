using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using Mantesis2015.Dto;
using MantesisVerIusCommonObjects.DataAccess;
using MantesisVerIusCommonObjects.Dto;
using MantesisVerIusCommonObjects.Utilities;
using ScjnUtilities;
using UtilsMantesis;

namespace Mantesis2015.Model
{
    public class ReporteModel
    {
        /// <summary>
        /// Trae la información requerida de las tesis que se agregan al reporte de 
        /// Materias SGA de acuerdo a la materia respectiva
        /// </summary>
        /// <param name="idMantSGA"></param>
        /// <param name="tipoReporte"></param>
        /// <returns></returns>
        public List<TesisReg> GetTesis(long idMantSga)
        {
            List<TesisReg> tesisReg = new List<TesisReg>();

            List<Estructura> registros = this.GetNumIus(idMantSga);

            SqlConnection lConn = DbConnDac.GetConnectionIus();
            SqlCommand cmd;
            SqlDataReader reader = null;
            try
            {
                lConn.Open();
                foreach (Estructura regis in registros)
                {
                    string sqlCadena = "Select * from Tesis WHERE ius = " + regis.Ius4;

                    cmd = new SqlCommand(sqlCadena, lConn);
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        String volumen = Utils.GetInfoVolumenes(MantesisVerIusCommonObjects.Utilities.ValuesMant.Volumen);

                        tesisReg.Add(new TesisReg(
                            regis.Ius4,
                            reader["rubro"].ToString(),
                            reader["texto"].ToString(),
                            reader["Prec"].ToString(),
                            Utils.GetInfoDatosCompartidos(1, Convert.ToInt16(reader["epoca"])),
                            Utils.GetInfoDatosCompartidos(2, Convert.ToInt16(reader["sala"])),
                            Utils.GetInfoDatosCompartidos(3, Convert.ToInt16(reader["fuente"])),
                            volumen,
                            reader["tesis"].ToString(),
                            reader["pagina"].ToString(),
                            (Convert.ToInt16(reader["ta/tj"]) == 0) ? "Tesis Aislada" : "Jurisprudencia",
                            Utils.GetInfoDatosCompartidos(4, Convert.ToInt16(reader["materia1"])),
                            Utils.GetInfoDatosCompartidos(4, Convert.ToInt16(reader["materia2"])),
                            Utils.GetInfoDatosCompartidos(4, Convert.ToInt16(reader["materia3"])),
                            volumen,
                            "",
                            reader["RIndx"].ToString(),
                            Convert.ToInt32(reader["idSubVolumen"])));
                    }

                    reader.Close();
                }
            }
            catch (SqlException ex)
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
                lConn.Close();
            }

            return tesisReg;
        }

        /// <summary>
        /// Devuelve la información de todas las tesis del volumen seleccionado
        /// </summary>
        /// <returns></returns>
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
                                   "WHERE T.Volumen = " + MantesisVerIusCommonObjects.Utilities.ValuesMant.Volumen;

                cmd = new SqlCommand(sqlCadena, lConn);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    String volumen = Utils.GetInfoVolumenes(MantesisVerIusCommonObjects.Utilities.ValuesMant.Volumen);

                    tesisReg.Add(new TesisReg(
                        Convert.ToInt32(reader["IUS"]),
                        reader["rubro"].ToString(),
                        reader["texto"].ToString(),
                        reader["Prec"].ToString(),
                        Utils.GetInfoDatosCompartidos(1, Convert.ToInt16(reader["epoca"])),
                        Utils.GetInfoDatosCompartidos(2, Convert.ToInt16(reader["sala"])),
                        Utils.GetInfoDatosCompartidos(3, Convert.ToInt16(reader["fuente"])),
                        volumen,
                        reader["tesis"].ToString(),
                        reader["pagina"].ToString(),
                        (Convert.ToInt16(reader["ta/tj"]) == 0) ? "Tesis Aislada" : "Jurisprudencia",
                        Utils.GetInfoDatosCompartidos(4, Convert.ToInt16(reader["materia1"])),
                        Utils.GetInfoDatosCompartidos(4, Convert.ToInt16(reader["materia2"])),
                        Utils.GetInfoDatosCompartidos(4, Convert.ToInt16(reader["materia3"])),
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
                reader.Close();
                lConn.Close();
            }
            return tesisReg;
        }

        public int GetNoTesis()
        {
            int cTesis = 0;

            DbConnection lConn = DbConnDac.GetConnectionIus();

            string sqlCadena = "Select COUNT(*) cuantas from Tesis_MatSGA WHERE Volumen = " + MantesisVerIusCommonObjects.Utilities.ValuesMant.Volumen;

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

            string sqlCadena = "Select distinct M.Id,M.Consec FROM cMateriasSGA M INNER JOIN Tesis_MatSGA T ON M.ID = T.idMatSGA WHERE VOLUMEN = " + MantesisVerIusCommonObjects.Utilities.ValuesMant.Volumen + " Order By M.Consec ";

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

            DbConnection lConn = DbConnDac.GetConnectEpocas(MantesisVerIusCommonObjects.Utilities.ValuesMant.Epoca, MantesisVerIusCommonObjects.Utilities.ValuesMant.Volumen, 1);

            if (materia == 99) //Imprime todas las tesis
            {
                sqlCadena = "Select * from Tesis WHERE volumen = " + MantesisVerIusCommonObjects.Utilities.ValuesMant.Volumen;
            }
            else
            {
                sqlCadena = "Select * from Tesis WHERE volumen = " + MantesisVerIusCommonObjects.Utilities.ValuesMant.Volumen + "AND (materia1 = " + materia + " OR materia2 = " + materia + " OR materia3 = " + materia + " )";
            }

            try
            {
                DataTableReader reader = this.GetDatosTabla(sqlCadena, lConn);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        long ius = Convert.ToInt64(reader["ius4"]);
                        String volumen = Utils.GetInfoVolumenes(MantesisVerIusCommonObjects.Utilities.ValuesMant.Volumen);

                        tesisReg.Add(new TesisReg(
                            ius,
                            reader["rubro"].ToString(),
                            reader["texto"].ToString(),
                            reader["Precedentes"].ToString(),
                            Utils.GetInfoDatosCompartidos(1, Convert.ToInt16(reader["epoca"])),
                            Utils.GetInfoDatosCompartidos(2, Convert.ToInt16(reader["sala"])),
                            Utils.GetInfoDatosCompartidos(3, Convert.ToInt16(reader["fuente"])),
                            volumen,
                            reader["tesis"].ToString(),
                            reader["pagina"].ToString(),
                            (Convert.ToInt16(reader[((Convert.ToInt32(reader["epoca"]) != ConstMant.Decima) ? "ta/tj" : "tatj")]) == 0) ? "Tesis Aislada" : "Jurisprudencia",
                            Utils.GetInfoDatosCompartidos(4, Convert.ToInt16(reader["materia1"])),
                            Utils.GetInfoDatosCompartidos(4, Convert.ToInt16(reader["materia2"])),
                            Utils.GetInfoDatosCompartidos(4, Convert.ToInt16(reader["materia3"])),
                            volumen,
                            "",
                            reader["RubroStr"].ToString(),
                            Convert.ToInt32(reader["idSubVolumen"])));
                    }
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
                lConn.Close();
            }

            return tesisReg;
        }

        public string GetTomo(int nSubVolumen)
        {
            string localizacion = "";

            SqlConnection lConn = DbConnDac.GetConnectionMantesisSql();

            string sqlCadena = "SELECT * FROM subVolumen WHERE idVolumen = " + ValuesMant.Volumen +
                               " AND idSubVolumen = " + nSubVolumen;

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
                lConn.Close();
            }

            return localizacion;
        }

        private List<Estructura> GetNumIus(long idMantSga)
        {
            List<Estructura> numRegIus = new List<Estructura>();

            DbConnection lConn = DbConnDac.GetConnectionIus();

            string sqlCadena = "SELECT IUS,Fecha,sUsr " +
                               "FROM Tesis_MatSGA  " +
                               "WHERE Volumen = " + ValuesMant.Volumen + " AND idMatSGA = " + idMantSga;

            if (ValuesMant.TipoReporte == 3)
                sqlCadena += " AND usrSJF = " + AccesoUsuarioModel.Llave;

            try
            {
                DataTableReader reader = this.GetDatosTabla(sqlCadena, lConn);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        numRegIus.Add(new Estructura(Convert.ToInt32(reader["IUS"].ToString()), reader["fecha"].ToString(),
                            reader["sUsr"].ToString()));
                    }
                }
            }
            catch (SqlException ex)
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
                lConn.Close();
            }

            return numRegIus;
        }

        public List<TesisReg> GetTesisSinDetalle(long idMantSga)
        {
            List<TesisReg> tesisReg = new List<TesisReg>();

            List<Estructura> regIus = this.GetNumIus(idMantSga);

            DbConnection lConn = DbConnDac.GetConnectEpocas(ValuesMant.Epoca, ValuesMant.Volumen, 1);

            try
            {
                foreach (Estructura registro in regIus)
                {
                    string sqlCadena = "SELECT Rubro,tesis FROM Tesis WHERE ius4 = " + registro.Ius4;

                    DataTableReader reader = this.GetDatosTabla(sqlCadena, lConn);

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            //Tipo tesis contiene la fecha
                            tesisReg.Add(new TesisReg(
                                registro.Ius4,
                                reader["rubro"].ToString(),
                                reader["texto"].ToString(),
                                registro.Fecha,
                                registro.LlaveUsuario));
                        }
                    }
                }
            }
            catch (SqlException ex)
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
                lConn.Close();
            }

            return tesisReg;
        }

        public List<TesisReg> GetIusModif(List<long> motivos, List<TesisReg> listaTesis)
        {
            List<long> listaMotivos = motivos.Distinct().ToList();
            DbConnection lConn = DbConnDac.GetConnectTemp();

            List<TesisReg> listaFinal = new List<TesisReg>();

            string sqlCadena = "SELECT * FROM TesisModificadas WHERE TipoMotivo = 2 AND ";

            int cont = 0;
            foreach (long motivo in listaMotivos)
            {
                if (cont == 0)
                {
                    sqlCadena += " (Motivo = " + motivo;
                }
                else
                {
                    sqlCadena += " OR Motivo = " + motivo;
                }
                cont++;
            }

            foreach (TesisReg tesis in listaTesis)
            {
                string sqlCadenaFin = sqlCadena;
                sqlCadenaFin += ") AND ius4 = " + tesis.ius4;

                DataTableReader reader = this.GetDatosTabla(sqlCadenaFin, lConn);

                if (reader.Read())
                {
                    listaFinal.Add(tesis);
                }
            }

            return listaFinal;
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

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            return dtr;
        }
    }
}
