using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using Mantesis2015.Dto;
using MantesisCommonObjects.DataAccess;
using MantesisCommonObjects.Dto;
using MantesisCommonObjects.MantUtilities;
using ScjnUtilities;

namespace Mantesis2015.Model
{
    class UnaTesisModel
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["Base"].ConnectionString;

        private DbConnection connectionEpocas;
        private DbConnection connMantesis;

        private readonly List<TesisReferencia> lstTesisReferencia;

        public UnaTesisModel()
        {
            lstTesisReferencia = new List<TesisReferencia>();
        }

        public void DbConnectionOpen()
        {
            if (ValuesMant.Epoca == ConstMantesis.Apendice)
            {
                //connectionEpocas = MantesisDAC.GetConnectionApendice(ValuesMant.Epoca,ValuesMant.ApendicNom);
                connectionEpocas = DbConnDac.GetConnectEpocas(ValuesMant.Epoca, ValuesMant.ApendicNom, 1);
                connectionEpocas.Open();
            }
            else
            {
                //connectionEpocas = MantesisDAC.GetConnectionEpocas(ValuesMant.Epoca);
                connectionEpocas = DbConnDac.GetConnectEpocas(ValuesMant.Epoca, ValuesMant.Volumen, 1);
                connectionEpocas.Open();
            }
            //connMantesis = MantesisDAC.GetConnectionMant();
            connMantesis = DbConnDac.GetConnectMant();
            connMantesis.Open();
        }

        public void DbConnectionClose()
        {
            connectionEpocas.Close();
            connMantesis.Close();
        }

        /// <summary>
        /// Obtiene los datos completos de la tesis que se quiere visualizar
        /// </summary>
        /// <param name="nIus"></param>
        /// <returns></returns>
        public TesisDto CargaDatosTesisMantesisSql(long nIus)
        {
            SqlConnection connectionMantesisSql = DbConnDac.GetConnectionMantesisSql();
            SqlCommand cmd;
            SqlDataReader reader = null;

            TesisDto tesis = null;

            try
            {
                connectionMantesisSql.Open();

                string sqlCadena = "SELECT * FROM Tesis WHERE ius = " + nIus;

                cmd = new SqlCommand(sqlCadena, connectionMantesisSql);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    tesis = new TesisDto();

                    tesis.Parte = Convert.ToInt16(reader["Parte"]);
                    tesis.Consec = Convert.ToInt32(reader["Consec"]);
                    tesis.Ius = Convert.ToInt32(reader["ius"]);
                    tesis.Rubro = reader["rubro"].ToString();
                    tesis.Texto = reader["texto"].ToString();
                    tesis.Precedentes = reader["precedentes"].ToString();
                    tesis.Notas = (reader["Notas"].Equals(DBNull.Value)) ? String.Empty : reader["Notas"].ToString();
                    tesis.EpocaInt = Convert.ToInt32(reader["epoca"]);
                    tesis.Epoca =MantUtils.GetInfoDatosCompartidos(1, Convert.ToInt32(reader["epoca"]));
                    tesis.Sala = Convert.ToInt16(reader["sala"]);
                    tesis.Fuente = Convert.ToInt16(reader["fuente"]);
                    tesis.Volumen =MantUtils.GetInfoVolumenes(Convert.ToInt32(reader["volumen"]));
                    tesis.VolumenInt = Convert.ToInt32(reader["Volumen"]);
                    tesis.Tesis = reader["tesis"].ToString();
                    tesis.Pagina = reader["pagina"].ToString();
                    tesis.TaTj = Convert.ToInt16(reader["ta_tj"]);
                    tesis.Materia1 = Convert.ToInt16(reader["Materia1"]);
                    tesis.Materia2 = Convert.ToInt16(reader["Materia2"]);
                    tesis.Materia3 = Convert.ToInt16(reader["Materia3"]);
                    tesis.Materia4 = Convert.ToInt16(reader["Materia4"]);
                    tesis.Materia5 = Convert.ToInt16(reader["Materia5"]);
                    tesis.IdGenealogia = Convert.ToInt16(reader["idGenealogia"]);
                    tesis.VolOrden = Convert.ToInt16(reader["VolOrden"]);
                    tesis.ConsecIndx = Convert.ToInt32(reader["Consecindx"]);
                    tesis.IdTcc = Convert.ToInt16(reader["IdTCC"]);
                    tesis.InfAnexos = Convert.ToInt16(reader["InfAnexos"]);
                    tesis.LocAbr = reader["LocAbr"].ToString();
                    tesis.NumLetra = Convert.ToInt16(reader["NumLetra"]);
                    tesis.ConsecLetra = Convert.ToInt32(reader["ConsecLetra"]);
                    tesis.Instancia = Convert.ToInt32(reader["Instancia"]);
                    tesis.ConsecInst = Convert.ToInt32(reader["ConsecInst"]);
                    tesis.TpoTesis = Convert.ToInt16(reader["TpoTesis"]);
                    tesis.TpoAsunto1 = Convert.ToInt16(reader["TpoAsunto1"]);
                    tesis.TpoAsunto2 = Convert.ToInt16(reader["TpoAsunto2"]);
                    tesis.TpoAsunto3 = Convert.ToInt16(reader["TpoAsunto3"]);
                    tesis.TpoAsunto4 = Convert.ToInt16(reader["TpoAsunto4"]);
                    tesis.TpoAsunto5 = Convert.ToInt16(reader["TpoAsunto5"]);
                    tesis.TpoPonente1 = Convert.ToInt16(reader["TpoPonente1"]);
                    tesis.TpoPonente2 = Convert.ToInt16(reader["TpoPonente2"]);
                    tesis.TpoPonente3 = Convert.ToInt16(reader["TpoPonente3"]);
                    tesis.TpoPonente4 = Convert.ToInt16(reader["TpoPonente4"]);
                    tesis.TpoPonente5 = Convert.ToInt16(reader["TpoPonente5"]);
                    tesis.TpoPon1 = Convert.ToInt16(reader["TpoPon1"]);
                    tesis.TpoPon2 = Convert.ToInt16(reader["TpoPon2"]);
                    tesis.TpoPon3 = Convert.ToInt16(reader["TpoPon3"]);
                    tesis.TpoPon4 = Convert.ToInt16(reader["TpoPon4"]);
                    tesis.TpoPon5 = Convert.ToInt16(reader["TpoPon5"]);
                    tesis.OrdenTesis = Convert.ToInt32(reader["OrdenTesis"]);
                    tesis.OrdenInstancia = Convert.ToInt32(reader["OrdenInstancia"]);
                    tesis.OrdenRubro = Convert.ToInt32(reader["OrdenRubro"]);
                    tesis.IdProg = (reader["idProg"].Equals(DBNull.Value)) ? String.Empty : reader["idProg"].ToString();
                    tesis.ExistenTemas = Convert.ToInt16(reader["ExistenTemas"]);
                    tesis.IdClasif10 = Convert.ToInt16(reader["IdClasif10"]);
                    tesis.IdSubVolumen = Convert.ToInt32(reader["idSubVolumen"]);
                    tesis.AnioPublica = Convert.ToInt16(reader["AnioPublica"]);
                    tesis.MesPublica = Convert.ToInt16(reader["MesPublica"]);
                    tesis.SemanaPublica = (reader["SemanaPublica"].Equals(DBNull.Value)) ? 0 : Convert.ToInt16(reader["SemanaPublica"]);
                    tesis.FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"]);

                    if (reader["FechaModifica"].Equals(DBNull.Value))
                        tesis.FechaModifica = null;
                    else
                        tesis.FechaModifica = Convert.ToDateTime(reader["FechaModifica"]);

                    tesis.Estado = Convert.ToInt16(reader["Estado"]);
                    tesis.MotivoModificar = 0;
                    tesis.RubroStr = reader["RubroStr"].ToString();
                    tesis.BaseOrigen = reader["BD"].ToString();
                    tesis.BdGrupo = Convert.ToInt16(reader["BdGrupo"]);

                    tesis.Genealogia = this.GetGenealogia(tesis.IdGenealogia);
                    tesis.Observaciones = this.GetOtrosTextos(tesis.Ius, 2);
                    tesis.Concordancia = this.GetOtrosTextos(tesis.Ius, 3);
                    tesis.NotasRubro = reader["NotaPieR"].ToString();
                    tesis.NotasTexto = reader["NotaPieT"].ToString();
                    tesis.NotasPrecedentes = reader["NotaPieR"].ToString();
                    tesis.NotasGaceta = reader["NotaGaceta"].ToString();
                    tesis.NotaPublica = reader["NotaPublica"].ToString();
                    tesis.IsNotasModif = Convert.ToBoolean((reader["ModificadoNotas"].Equals(DBNull.Value)) ? 0 : Convert.ToInt16(reader["ModificadoNotas"]));
                    //tesis.MateriasSga = new ClasificacionSgaModel().GetMateriasRelacionadas(tesis.Ius);
                }
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UnaTesisModel", "MantesisQuinta");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UnaTesisModel", "MantesisQuinta");
            }
            finally
            {
                reader.Close();
                connectionMantesisSql.Close();
            }

            return tesis;
        }

        public DataTableReader RevisaTesisReferencia(long pIus)
        {
            long nIusAnterior = 0;
            long nIus4 = 0;
            string sRegistro;
            DbConnection connTemp = DbConnDac.GetConnectTemp();
            DbConnection connIus;
            DataTableReader readerReg;
            DataTableReader readerIus = null;
            string sqlCadena;
            connTemp.Open();

            if (lstTesisReferencia.Count == 0)
            {
                sqlCadena = "SELECT * FROM TesisModificadas WHERE TipoMotivo =4 AND Registro <> ''";
                readerReg = this.GetDatosRegistro(sqlCadena, connTemp);

                while (readerReg.Read())
                {
                    sRegistro = readerReg["Registro"].ToString().Trim();
                    nIusAnterior = Convert.ToInt32(sRegistro);
                    nIus4 = Convert.ToInt32(readerReg["IUS4"].ToString());

                    if (nIus4 != nIusAnterior)
                    {
                        lstTesisReferencia.Add(new TesisReferencia(nIus4, Convert.ToInt16(readerReg["Motivo"].ToString()), nIusAnterior));
                    }
                }
                readerReg.Close();
            }

            foreach (var item in lstTesisReferencia)
            {
                if (item.Registro == pIus)
                {
                    nIusAnterior = item.Ius;
                    ValuesMant.MotivoBaja = item.Motivo;

                    connIus = DbConnDac.GetConnectionIus();
                    connIus.Open();

                    sqlCadena = "SELECT * FROM Tesis WHERE IUS=" + nIusAnterior;
                    readerIus = this.GetDatosRegistro(sqlCadena, connIus);
                    break;
                }
            }

            connTemp.Close();

            return readerIus;
        }

        /// <summary>
        /// Verifica si el número de registro que se quiere actualizar existe dentro de la base de datos del 
        /// servidor CT9BD3
        /// </summary>
        /// <param name="nIus">Número de registro IUS que se quiere actualizar</param>
        /// <returns></returns>
        public bool DoesRegIusExist(long nIus)
        {
            SqlConnection connectionMantesisSql = DbConnDac.GetConnectionIus();
            SqlCommand cmd;
            SqlDataReader reader = null;

            bool tesisExist = false;

            try
            {
                connectionMantesisSql.Open();

                string sqlCadena = "SELECT * FROM Tesis WHERE ius = " + nIus;

                cmd = new SqlCommand(sqlCadena, connectionMantesisSql);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    tesisExist = true;
                }
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UnaTesisModel", "MantesisQuinta");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UnaTesisModel", "MantesisQuinta");
            }
            finally
            {
                reader.Close();
                connectionMantesisSql.Close();
            }

            return tesisExist;
        }

        public long RevUltimoRegIus()
        {
            DataTableReader readerReg;
            string sqlCadena = "";
            long nIus = 0;

            sqlCadena = "SELECT * FROM ContIUS4";
            readerReg = this.GetDatosRegistro(sqlCadena, connMantesis);

            if (readerReg.Read())
            {
                nIus = Convert.ToInt32(readerReg["ContIUS"].ToString());

                sqlCadena = "UPDATE ContIUS4 SET ContIUS =" + (nIus + 1);
                DbCommand command = connMantesis.CreateCommand();
                command.CommandText = sqlCadena;
                command.ExecuteNonQuery(); // Devuelve un entero con el número de registros involucrados
            }
            else
            {
                nIus = -1;
            }

            return nIus;
        }

        public DataTableReader GetDatosRegistro(string sqlString, DbConnection lConn)
        {
            DataTableReader dtr = null;
            try
            {
                //Create Command
                DataAdapter query = DbConnDac.GetDataAdapter(sqlString, lConn);
                dtr = DbConnDac.GetDatosReader(query);
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UnaTesisModel", "MantesisQuinta");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UnaTesisModel", "MantesisQuinta");
            }

            return dtr;
        }

        private void AddParms(OleDbCommand cmd, params string[] cols)
        {
            // Add each parameter. Note that each colum in
            // table "Customers" is of type VARCHAR !
            foreach (String column in cols)
            {
                cmd.Parameters.Add("@" + column, OleDbType.Char, 0, column);
            }
        }

        private void AddParms(SqlCommand cmd, params string[] cols)
        {
            // Add each parameter. Note that each colum in
            // table "Customers" is of type VARCHAR !
            foreach (String column in cols)
            {
                cmd.Parameters.Add("@" + column, SqlDbType.Char, 0, column);
            }
        }

        #region TesisQuinta

        /// <summary>
        /// Obtiene la información completa de la tesis seleccionada para su visualización o modificación.
        /// Estas tesis ya fueron importadas previamente
        /// </summary>
        /// <param name="nIus"></param>
        /// <returns></returns>
        public TesisQuinta GetTesis(long nIus)
        {
            OleDbConnection connection = new OleDbConnection(connectionString);
            OleDbCommand cmd;
            OleDbDataReader reader;

            TesisQuinta tesis = null;

            try
            {
                connection.Open();

                cmd = new OleDbCommand("SELECT * FROM Tesis WHERE ius = @Ius", connection);
                cmd.Parameters.AddWithValue("@Ius", nIus);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    tesis = new TesisQuinta();

                    tesis.Parte = Convert.ToInt16(reader["Parte"]);
                    tesis.Consec = Convert.ToInt32(reader["Consec"]);
                    tesis.Ius = Convert.ToInt32(reader["ius"]);
                    tesis.Rubro = reader["rubro"].ToString();
                    tesis.Texto = reader["texto"].ToString();
                    tesis.Precedentes = reader["precedentes"].ToString();
                    tesis.Notas = (reader["Notas"].Equals(DBNull.Value)) ? String.Empty : reader["Notas"].ToString();
                    tesis.EpocaInt = Convert.ToInt32(reader["epoca"]);
                    tesis.Epoca = MantUtils.GetInfoDatosCompartidos(1, Convert.ToInt32(reader["epoca"]));
                    tesis.Sala = Convert.ToInt16(reader["sala"]);
                    tesis.Fuente = Convert.ToInt16(reader["fuente"]);
                    tesis.Volumen =MantUtils.GetInfoVolumenes(Convert.ToInt32(reader["volumen"]));
                    tesis.VolumenInt = Convert.ToInt32(reader["Volumen"]);
                    tesis.Tesis = reader["tesis"].ToString();
                    tesis.Pagina = reader["pagina"].ToString();
                    tesis.TaTj = Convert.ToInt16(reader["ta_tj"]);
                    tesis.Materia1 = Convert.ToInt16(reader["Materia1"]);
                    tesis.Materia2 = Convert.ToInt16(reader["Materia2"]);
                    tesis.Materia3 = Convert.ToInt16(reader["Materia3"]);
                    tesis.Materia4 = Convert.ToInt16(reader["Materia4"]);
                    tesis.Materia5 = Convert.ToInt16(reader["Materia5"]);
                    tesis.IdGenealogia = Convert.ToInt16(reader["idGenealogia"]);
                    tesis.VolOrden = Convert.ToInt16(reader["VolOrden"]);
                    tesis.ConsecIndx = Convert.ToInt32(reader["Consecindx"]);
                    tesis.IdTcc = Convert.ToInt16(reader["IdTCC"]);
                    tesis.InfAnexos = Convert.ToInt16(reader["InfAnexos"]);
                    tesis.LocAbr = reader["LocAbr"].ToString();
                    tesis.NumLetra = Convert.ToInt16(reader["NumLetra"]);
                    tesis.ConsecLetra = Convert.ToInt32(reader["ConsecLetra"]);
                    tesis.Instancia = Convert.ToInt32(reader["Instancia"]);
                    tesis.ConsecInst = Convert.ToInt32(reader["ConsecInst"]);
                    tesis.TpoTesis = Convert.ToInt16(reader["TpoTesis"]);
                    tesis.TpoAsunto1 = Convert.ToInt16(reader["TpoAsunto1"]);
                    tesis.TpoAsunto2 = Convert.ToInt16(reader["TpoAsunto2"]);
                    tesis.TpoAsunto3 = Convert.ToInt16(reader["TpoAsunto3"]);
                    tesis.TpoAsunto4 = Convert.ToInt16(reader["TpoAsunto4"]);
                    tesis.TpoAsunto5 = Convert.ToInt16(reader["TpoAsunto5"]);
                    tesis.TpoPonente1 = Convert.ToInt16(reader["TpoPonente1"]);
                    tesis.TpoPonente2 = Convert.ToInt16(reader["TpoPonente2"]);
                    tesis.TpoPonente3 = Convert.ToInt16(reader["TpoPonente3"]);
                    tesis.TpoPonente4 = Convert.ToInt16(reader["TpoPonente4"]);
                    tesis.TpoPonente5 = Convert.ToInt16(reader["TpoPonente5"]);
                    tesis.TpoPon1 = Convert.ToInt16(reader["TpoPon1"]);
                    tesis.TpoPon2 = Convert.ToInt16(reader["TpoPon2"]);
                    tesis.TpoPon3 = Convert.ToInt16(reader["TpoPon3"]);
                    tesis.TpoPon4 = Convert.ToInt16(reader["TpoPon4"]);
                    tesis.TpoPon5 = Convert.ToInt16(reader["TpoPon5"]);
                    tesis.OrdenTesis = Convert.ToInt32(reader["OrdenTesis"]);
                    tesis.OrdenInstancia = Convert.ToInt32(reader["OrdenInstancia"]);
                    tesis.OrdenRubro = Convert.ToInt32(reader["OrdenRubro"]);
                    tesis.IdProg = (reader["idProg"].Equals(DBNull.Value)) ? String.Empty : reader["idProg"].ToString();
                    tesis.ExistenTemas = Convert.ToInt16(reader["ExistenTemas"]);
                    tesis.IdClasif10 = Convert.ToInt16(reader["IdClasif10"]);
                    tesis.IdSubVolumen = Convert.ToInt32(reader["idSubVolumen"]);
                    tesis.AnioPublica = Convert.ToInt16(reader["AnioPublica"]);
                    tesis.MesPublica = Convert.ToInt16(reader["MesPublica"]);
                    tesis.SemanaPublica = (reader["SemanaPublica"].Equals(DBNull.Value)) ? 0 : Convert.ToInt16(reader["SemanaPublica"]);
                    tesis.FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"]);

                    if (reader["FechaModifica"].Equals(DBNull.Value))
                        tesis.FechaModifica = null;
                    else
                        tesis.FechaModifica = Convert.ToDateTime(reader["FechaModifica"]);

                    tesis.Estado = Convert.ToInt16(reader["Estado"]);
                    tesis.MotivoModificar = 0;
                    tesis.RubroStr = reader["RubroStr"].ToString();
                    tesis.BaseOrigen = reader["BD"].ToString();
                    tesis.BdGrupo = Convert.ToInt16(reader["BdGrupo"]);

                    tesis.Genealogia = this.GetGenealogia(tesis.IdGenealogia);
                    tesis.Observaciones = this.GetOtrosTextos(tesis.Ius, 2);
                    tesis.Concordancia = this.GetOtrosTextos(tesis.Ius, 3);
                    tesis.NotasRubro = reader["NotaPieR"].ToString();
                    tesis.NotasTexto = reader["NotaPieT"].ToString();
                    tesis.NotasPrecedentes = reader["NotaPieR"].ToString();
                    tesis.NotasGaceta = reader["NotaGaceta"].ToString();
                    tesis.NotaPublica = reader["NotaPublica"].ToString();
                    tesis.IsNotasModif = Convert.ToBoolean((reader["ModificadoNotas"].Equals(DBNull.Value)) ? 0 : Convert.ToInt16(reader["ModificadoNotas"]));
                    tesis.NotaAclaratoria = Convert.ToInt32(reader["NoPublicacion"]);
                }
                reader.Close();
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UnaTesisModel", "MantesisQuinta");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UnaTesisModel", "MantesisQuinta");
            }
            finally
            {
                //reader.Close();
                connection.Close();
            }

            return tesis;
        }

        /// <summary>
        /// Devuelve el texto de la genealogía asociada a cada tesis
        /// </summary>
        /// <param name="IdGenealogia"></param>
        /// <returns></returns>
        public string GetGenealogia(long idGenealogia)
        {
            OleDbConnection connection = new OleDbConnection(connectionString);
            OleDbCommand cmd;
            OleDbDataReader reader;

            string genealogia = "";

            try
            {
                connection.Open();

                cmd = new OleDbCommand("SELECT txtGenealogia FROM Genealogia WHERE idGenealogia = @Id", connection);
                cmd.Parameters.AddWithValue("@Id", idGenealogia);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    genealogia = reader["txtGenealogia"].ToString();
                }

                reader.Close();
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UnaTesisModel", "MantesisQuinta");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UnaTesisModel", "MantesisQuinta");
            }
            finally
            {
                reader = null;
                connection.Close();
            }

            return genealogia;
        }

        /// <summary>
        /// Devuelve los textos relativos a las observaciones o a la concordancia respectiva a cada tesis
        /// </summary>
        /// <param name="ius"></param>
        /// <param name="tipoNota">2. Observaciones --- 3. Concordancia</param>
        /// <returns></returns>
        public string GetOtrosTextos(int ius, int tipoNota)
        {
            OleDbConnection connection = new OleDbConnection(connectionString);
            OleDbCommand cmd;
            OleDbDataReader reader;

            string otrosTextos = "";

            try
            {
                connection.Open();

                cmd = new OleDbCommand("SELECT textos FROM OtrosTextos WHERE ius = @Ius AND tiponota = @Tipo", connection);
                cmd.Parameters.AddWithValue("@Ius", ius);
                cmd.Parameters.AddWithValue("@Tipo", tipoNota);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    otrosTextos = reader["textos"].ToString();
                }

                reader.Close();
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UnaTesisModel", "MantesisQuinta");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UnaTesisModel", "MantesisQuinta");
            }
            finally
            {
                reader = null;
                connection.Close();
            }

            return otrosTextos;
        }

        public bool ImportarRegistro(TesisDto tesisDto)
        {
            bool complete = false;

            OleDbConnection connection = new OleDbConnection();
            OleDbDataAdapter dataAdapter;
            long nIus;
            long nIdGenealogia;
            string sRubroStr;

            DataSet dataSet = new DataSet();
            DataRow dr;

            ValuesMant.IusActualLstTesis = -1;

            try
            {
                const string sqlCadena = "SELECT * FROM Tesis WHERE IUS = 0";

                connection = new OleDbConnection(ConfigurationManager.ConnectionStrings["Base"].ConnectionString);
                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Tesis");

                //nIus = RevUltimoRegIus();
                //nIdGenealogia = RevUltimoRegGenealogia();

                sRubroStr = tesisDto.Rubro;
                sRubroStr = StringUtilities.QuitaCarCad(sRubroStr);
                sRubroStr = StringUtilities.ConvMay(sRubroStr);
                sRubroStr = StringUtilities.ReplaceDoubleSpaces(sRubroStr);
                sRubroStr = sRubroStr.Trim();

                if (sRubroStr.Length > 250)
                {
                    sRubroStr = sRubroStr.Substring(0, 250);
                }

                dr = dataSet.Tables["Tesis"].NewRow();
                dr["Parte"] = tesisDto.Parte;
                dr["Consec"] = 0;
                dr["IUS"] = tesisDto.Ius;
                dr["Rubro"] = tesisDto.Rubro;
                dr["Texto"] = tesisDto.Texto;
                dr["Precedentes"] = tesisDto.Precedentes;
                dr["Notas"] = tesisDto.Notas;
                dr["Epoca"] = ValuesMant.Epoca;
                dr["Sala"] = tesisDto.Sala;
                dr["Fuente"] = 0;
                dr["Volumen"] = ValuesMant.Volumen;
                dr["Tesis"] = (tesisDto.Tesis.Length == 0) ? " " : tesisDto.Tesis;
                dr["Pagina"] = MiscFunct.Llena(tesisDto.Pagina, 5);
                dr["TA_TJ"] = tesisDto.TaTj;
                dr["Materia1"] = tesisDto.Materia1;
                dr["Materia2"] = tesisDto.Materia2;
                dr["Materia3"] = tesisDto.Materia3;
                dr["Materia4"] = tesisDto.Materia4;
                dr["Materia5"] = tesisDto.Materia5;
                dr["IdGenealogia"] = tesisDto.IdGenealogia;
                dr["VolOrden"] = tesisDto.VolOrden;
                dr["ConsecIndx"] = tesisDto.ConsecIndx;
                dr["IdTCC"] = tesisDto.IdTcc;
                dr["InfAnexos"] = tesisDto.InfAnexos;
                dr["LocAbr"] = String.Empty;
                dr["NumLetra"] = tesisDto.NumLetra;
                dr["ConsecLetra"] = tesisDto.ConsecLetra;
                dr["Instancia"] = tesisDto.Instancia;
                dr["ConsecInst"] = tesisDto.ConsecInst;
                dr["TpoTesis"] = tesisDto.TpoTesis;
                dr["TpoAsunto1"] = tesisDto.TpoAsunto1;
                dr["TpoAsunto2"] = tesisDto.TpoAsunto2;
                dr["TpoAsunto3"] = tesisDto.TpoAsunto3;
                dr["TpoAsunto4"] = tesisDto.TpoAsunto4;
                dr["TpoAsunto5"] = tesisDto.TpoAsunto5;
                dr["TpoPonente1"] = tesisDto.TpoPonente1;
                dr["TpoPonente2"] = tesisDto.TpoPonente2;
                dr["TpoPonente3"] = tesisDto.TpoPonente3;
                dr["TpoPonente4"] = tesisDto.TpoPonente4;
                dr["TpoPonente5"] = tesisDto.TpoPonente5;
                dr["TpoPon1"] = tesisDto.TpoPon1;
                dr["TpoPon2"] = tesisDto.TpoPon2;
                dr["TpoPon3"] = tesisDto.TpoPon3;
                dr["TpoPon4"] = tesisDto.TpoPon4;
                dr["TpoPon5"] = tesisDto.TpoPon5;
                dr["OrdenTesis"] = tesisDto.OrdenTesis;
                dr["OrdenInstancia"] = tesisDto.OrdenInstancia;
                dr["OrdenRubro"] = tesisDto.OrdenRubro;
                dr["IdProg"] = tesisDto.IdProg;
                dr["ExistenTemas"] = tesisDto.ExistenTemas;
                dr["IdClasif10"] = tesisDto.IdClasif10;
                dr["IdSubVolumen"] = tesisDto.IdSubVolumen;
                dr["AnioPublica"] = tesisDto.AnioPublica;
                dr["MesPublica"] = tesisDto.MesPublica;
                dr["SemanaPublica"] = tesisDto.SemanaPublica;

                if (tesisDto.FechaRegistro == null)
                    dr["FechaRegistro"] = DBNull.Value;
                else
                    dr["FechaRegistro"] = tesisDto.FechaRegistro;

                if (tesisDto.FechaModifica == null)
                    dr["FechaModifica"] = DBNull.Value;
                else
                    dr["FechaModifica"] = tesisDto.FechaModifica;

                dr["Estado"] = tesisDto.Estado;
                dr["MotivoModificar"] = tesisDto.MotivoModificar;
                dr["RubroStr"] = sRubroStr;
                dr["BD"] = tesisDto.BaseOrigen;
                dr["BdGrupo"] = tesisDto.BdGrupo;
                dr["NotaPieR"] = tesisDto.NotasRubro;
                dr["NotaPieT"] = tesisDto.NotasTexto;
                dr["NotaPieP"] = tesisDto.NotasPrecedentes;
                dr["NotaGaceta"] = tesisDto.NotasGaceta;
                dr["NotaPublica"] = tesisDto.NotaPublica;
                dr["FechaPublicado"] = DBNull.Value;
                dr["FechaConocimiento"] = DBNull.Value;
                dr["ModificadoNotas"] = tesisDto.IsNotasModif;
                dr["ObsRubro"] = String.Empty;
                dr["ObsTexto"] = String.Empty;
                dr["ObsPrecedentes"] = String.Empty;

                dataSet.Tables["Tesis"].Rows.Add(dr);

                //dataAdapter.UpdateCommand = connectionEpsOle.CreateCommand();
                dataAdapter.InsertCommand = connection.CreateCommand();
                dataAdapter.InsertCommand.CommandText = "INSERT INTO Tesis(Parte,Consec,IUS,Rubro,Texto,Precedentes,Notas,Epoca,Sala,Fuente," +
                                                        "Volumen,Tesis,Pagina,TA_TJ,Materia1,Materia2,Materia3,Materia4,Materia5," +
                                                        "IdGenealogia,VolOrden,ConsecIndx,IdTCC,InfAnexos,LocAbr,NumLetra,ConsecLetra,Instancia,ConsecInst," +
                                                        "TpoTesis,TpoAsunto1,TpoAsunto2,TpoAsunto3,TpoAsunto4,TpoAsunto5,TpoPonente1,TpoPonente2,TpoPonente3,TpoPonente4,TpoPonente5," +
                                                        "TpoPon1,TpoPon2,TpoPon3,TpoPon4,TpoPon5,OrdenTesis,OrdenInstancia,OrdenRubro,IdProg,ExistenTemas,IdClasif10," +
                                                        "IdSubVolumen,AnioPublica,MesPublica,SemanaPublica,FechaRegistro,FechaModifica,Estado,MotivoModificar,RubroStr,BD,BdGrupo," +
                                                        "NotaPieR,NotaPieT,NotaPieP,NotaGaceta,NotaPublica,FechaPublicado,FechaConocimiento,ModificadoNotas,ObsRubro,ObsTexto,ObsPrecedentes) " +
                                                        " VALUES(@Parte,@Consec,@IUS,@Rubro,@Texto,@Precedentes,@Notas,@Epoca,@Sala,@Fuente," +
                                                        "@Volumen,@Tesis,@Pagina,@TA_TJ,@Materia1,@Materia2,@Materia3,@Materia4,@Materia5," +
                                                        "@IdGenealogia,@VolOrden,@ConsecIndx,@IdTCC,@InfAnexos,@LocAbr,@NumLetra,@ConsecLetra,@Instancia,@ConsecInst," +
                                                        "@TpoTesis,@TpoAsunto1,@TpoAsunto2,@TpoAsunto3,@TpoAsunto4,@TpoAsunto5,@TpoPonente1,@TpoPonente2,@TpoPonente3,@TpoPonente4,@TpoPonente5," +
                                                        "@TpoPon1,@TpoPon2,@TpoPon3,@TpoPon4,@TpoPon5,@OrdenTesis,@OrdenInstancia,@OrdenRubro,@IdProg,@ExistenTemas,@IdClasif10," +
                                                        "@IdSubVolumen,@AnioPublica,@MesPublica,@SemanaPublica,@FechaRegistro,@FechaModifica,@Estado,@MotivoModificar,@RubroStr,@BD,@BdGrupo," +
                                                        "@NotaPieR,@NotaPieT,@NotaPieP,@NotaGaceta,@NotaPublica,@FechaPublicado,@FechaConocimiento,@ModificadoNotas,@ObsRubro,@ObsTexto,@ObsPrecedentes)";

                dataAdapter.InsertCommand.Parameters.Add("@Parte", OleDbType.Numeric, 0, "Parte");//0;
                dataAdapter.InsertCommand.Parameters.Add("@Consec", OleDbType.Numeric, 0, "Consec");//0;
                dataAdapter.InsertCommand.Parameters.Add("@IUS", OleDbType.Numeric, 0, "IUS");//nIus;
                dataAdapter.InsertCommand.Parameters.Add("@Rubro", OleDbType.VarChar, 0, "Rubro");//tesisDto.Rubro;
                dataAdapter.InsertCommand.Parameters.Add("@Texto", OleDbType.VarChar, 0, "Texto");//tesisDto.Texto;
                dataAdapter.InsertCommand.Parameters.Add("@Precedentes", OleDbType.VarChar, 0, "Precedentes");//tesisDto.Precedentes;
                dataAdapter.InsertCommand.Parameters.Add("@Notas", OleDbType.VarChar, 0, "Notas");//tesisDto.Notas;
                dataAdapter.InsertCommand.Parameters.Add("@Epoca", OleDbType.Numeric, 0, "Epoca");//ValuesMant.Epoca;
                dataAdapter.InsertCommand.Parameters.Add("@Sala", OleDbType.Numeric, 0, "Sala");//tesisDto.Sala;
                dataAdapter.InsertCommand.Parameters.Add("@Fuente", OleDbType.Numeric, 0, "Fuente");//0;
                dataAdapter.InsertCommand.Parameters.Add("@Volumen", OleDbType.Numeric, 0, "Volumen");//ValuesMant.Volumen;
                dataAdapter.InsertCommand.Parameters.Add("@Tesis", OleDbType.VarChar, 0, "Tesis");//(tesisDto.Tesis.Length == 0) ? " " : tesisDto.Tesis;
                dataAdapter.InsertCommand.Parameters.Add("@Pagina", OleDbType.VarChar, 0, "Pagina");//MiscFunct.Llena(tesisDto.Pagina, 5);
                dataAdapter.InsertCommand.Parameters.Add("@TA_TJ", OleDbType.Numeric, 0, "TA_TJ");//tesisDto.TaTj;
                dataAdapter.InsertCommand.Parameters.Add("@Materia1", OleDbType.Numeric, 0, "Materia1");//tesisDto.Materia1;
                dataAdapter.InsertCommand.Parameters.Add("@Materia2", OleDbType.Numeric, 0, "Materia2");//tesisDto.Materia2;
                dataAdapter.InsertCommand.Parameters.Add("@Materia3", OleDbType.Numeric, 0, "Materia3");//tesisDto.Materia3;
                dataAdapter.InsertCommand.Parameters.Add("@Materia4", OleDbType.Numeric, 0, "Materia4");//tesisDto.Materia4;
                dataAdapter.InsertCommand.Parameters.Add("@Materia5", OleDbType.Numeric, 0, "Materia5");//tesisDto.Materia5;
                dataAdapter.InsertCommand.Parameters.Add("@IdGenealogia", OleDbType.Numeric, 0, "IdGenealogia");//(tesisDto.Genealogia.Length > 0) ? nIdGenealogia : 0;
                dataAdapter.InsertCommand.Parameters.Add("@VolOrden", OleDbType.Numeric, 0, "VolOrden");//tesisDto.VolOrden;
                dataAdapter.InsertCommand.Parameters.Add("@ConsecIndx", OleDbType.Numeric, 0, "ConsecIndx");//tesisDto.ConsecIndx;
                dataAdapter.InsertCommand.Parameters.Add("@IdTCC", OleDbType.Numeric, 0, "IdTCC");//tesisDto.IdTcc;
                dataAdapter.InsertCommand.Parameters.Add("@InfAnexos", OleDbType.Numeric, 0, "InfAnexos");//tesisDto.InfAnexos;
                dataAdapter.InsertCommand.Parameters.Add("@LocAbr", OleDbType.VarChar, 0, "LocAbr");//String.Empty;
                dataAdapter.InsertCommand.Parameters.Add("@NumLetra", OleDbType.Numeric, 0, "NumLetra");//tesisDto.NumLetra;
                dataAdapter.InsertCommand.Parameters.Add("@ConsecLetra", OleDbType.Numeric, 0, "ConsecLetra");//tesisDto.ConsecLetra;
                dataAdapter.InsertCommand.Parameters.Add("@Instancia", OleDbType.Numeric, 0, "Instancia");//tesisDto.Instancia;
                dataAdapter.InsertCommand.Parameters.Add("@ConsecInst", OleDbType.Numeric, 0, "ConsecInst");//tesisDto.ConsecInst;
                dataAdapter.InsertCommand.Parameters.Add("@TpoTesis", OleDbType.Numeric, 0, "TpoTesis");//tesisDto.TpoTesis;
                dataAdapter.InsertCommand.Parameters.Add("@TpoAsunto1", OleDbType.Numeric, 0, "TpoAsunto1");//tesisDto.TpoAsunto1;
                dataAdapter.InsertCommand.Parameters.Add("@TpoAsunto2", OleDbType.Numeric, 0, "TpoAsunto2");//tesisDto.TpoAsunto2;
                dataAdapter.InsertCommand.Parameters.Add("@TpoAsunto3", OleDbType.Numeric, 0, "TpoAsunto3");//tesisDto.TpoAsunto3;
                dataAdapter.InsertCommand.Parameters.Add("@TpoAsunto4", OleDbType.Numeric, 0, "TpoAsunto4");//tesisDto.TpoAsunto4;
                dataAdapter.InsertCommand.Parameters.Add("@TpoAsunto5", OleDbType.Numeric, 0, "TpoAsunto5");//tesisDto.TpoAsunto5;
                dataAdapter.InsertCommand.Parameters.Add("@TpoPonente1", OleDbType.Numeric, 0, "TpoPonente1");//tesisDto.TpoPonente1;
                dataAdapter.InsertCommand.Parameters.Add("@TpoPonente2", OleDbType.Numeric, 0, "TpoPonente2");//tesisDto.TpoPonente2;
                dataAdapter.InsertCommand.Parameters.Add("@TpoPonente3", OleDbType.Numeric, 0, "TpoPonente3");//tesisDto.TpoPonente3;
                dataAdapter.InsertCommand.Parameters.Add("@TpoPonente4", OleDbType.Numeric, 0, "TpoPonente4");//tesisDto.TpoPonente4;
                dataAdapter.InsertCommand.Parameters.Add("@TpoPonente5", OleDbType.Numeric, 0, "TpoPonente5");//tesisDto.TpoPonente5;
                dataAdapter.InsertCommand.Parameters.Add("@TpoPon1", OleDbType.Numeric, 0, "TpoPon1");//tesisDto.TpoPon1;
                dataAdapter.InsertCommand.Parameters.Add("@TpoPon2", OleDbType.Numeric, 0, "TpoPon2");//tesisDto.TpoPon2;
                dataAdapter.InsertCommand.Parameters.Add("@TpoPon3", OleDbType.Numeric, 0, "TpoPon3");//tesisDto.TpoPon3;
                dataAdapter.InsertCommand.Parameters.Add("@TpoPon4", OleDbType.Numeric, 0, "TpoPon4");//tesisDto.TpoPon4;
                dataAdapter.InsertCommand.Parameters.Add("@TpoPon5", OleDbType.Numeric, 0, "TpoPon5");//tesisDto.TpoPon5;
                dataAdapter.InsertCommand.Parameters.Add("@OrdenTesis", OleDbType.Numeric, 0, "OrdenTesis");//tesisDto.OrdenTesis;
                dataAdapter.InsertCommand.Parameters.Add("@OrdenInstancia", OleDbType.Numeric, 0, "OrdenInstancia");//tesisDto.OrdenInstancia;
                dataAdapter.InsertCommand.Parameters.Add("@OrdenRubro", OleDbType.Numeric, 0, "OrdenRubro");//tesisDto.OrdenRubro;
                dataAdapter.InsertCommand.Parameters.Add("@IdProg", OleDbType.VarChar, 0, "IdProg");//tesisDto.IdProg;
                dataAdapter.InsertCommand.Parameters.Add("@ExistenTemas", OleDbType.Numeric, 0, "ExistenTemas");//tesisDto.ExistenTemas;
                dataAdapter.InsertCommand.Parameters.Add("@IdClasif10", OleDbType.Numeric, 0, "IdClasif10");//tesisDto.IdClasif10;
                dataAdapter.InsertCommand.Parameters.Add("@IdSubVolumen", OleDbType.Numeric, 0, "IdSubVolumen");//tesisDto.IdSubVolumen;
                dataAdapter.InsertCommand.Parameters.Add("@AnioPublica", OleDbType.Numeric, 0, "AnioPublica");//tesisDto.AnioPublica;
                dataAdapter.InsertCommand.Parameters.Add("@MesPublica", OleDbType.Numeric, 0, "MesPublica");//tesisDto.MesPublica;
                dataAdapter.InsertCommand.Parameters.Add("@SemanaPublica", OleDbType.Numeric, 0, "SemanaPublica");//tesisDto.SemanaPublica;
                dataAdapter.InsertCommand.Parameters.Add("@FechaRegistro", OleDbType.Date, 0, "FechaRegistro");//tesisDto.FechaRegistro;
                dataAdapter.InsertCommand.Parameters.Add("@FechaModifica", OleDbType.Date, 0, "FechaModifica");//tesisDto.FechaModifica;
                dataAdapter.InsertCommand.Parameters.Add("@Estado", OleDbType.Numeric, 0, "Estado");//tesisDto.Estado;
                dataAdapter.InsertCommand.Parameters.Add("@MotivoModificar", OleDbType.Numeric, 0, "MotivoModificar");//tesisDto.MotivoModificar;
                dataAdapter.InsertCommand.Parameters.Add("@RubroStr", OleDbType.VarChar, 0, "RubroStr");//sRubroStr;
                dataAdapter.InsertCommand.Parameters.Add("@BD", OleDbType.VarChar, 0, "BD");//tesisDto.BaseOrigen;
                dataAdapter.InsertCommand.Parameters.Add("@BdGrupo", OleDbType.Numeric, 0, "BdGrupo");//tesisDto.BdGrupo;
                dataAdapter.InsertCommand.Parameters.Add("@NotaPieR", OleDbType.VarChar, 0, "NotaPieR");//tesisDto.NotasRubro;
                dataAdapter.InsertCommand.Parameters.Add("@NotaPieT", OleDbType.VarChar, 0, "NotaPieT");//tesisDto.NotasTexto;
                dataAdapter.InsertCommand.Parameters.Add("@NotaPieP", OleDbType.VarChar, 0, "NotaPieP");//tesisDto.NotasPrecedentes;
                dataAdapter.InsertCommand.Parameters.Add("@NotaGaceta", OleDbType.VarChar, 0, "NotaGaceta");//tesisDto.NotasGaceta;
                dataAdapter.InsertCommand.Parameters.Add("@NotaPublica", OleDbType.VarChar, 0, "NotaPublica");//tesisDto.NotaPublica;
                dataAdapter.InsertCommand.Parameters.Add("@FechaPublicado", OleDbType.Date, 0, "FechaPublicado");//tesisDto.FechaPublicado;
                dataAdapter.InsertCommand.Parameters.Add("@FechaConocimiento", OleDbType.Date, 0, "FechaConocimiento");//tesisDto.FechaConocimietno;
                dataAdapter.InsertCommand.Parameters.Add("@ModificadoNotas", OleDbType.Numeric, 0, "ModificadoNotas");//tesisDto.FechaConocimietno;
                dataAdapter.InsertCommand.Parameters.Add("@ObsRubro", OleDbType.VarChar, 0, "ObsRubro");//tesisDto.ObsRubro;
                dataAdapter.InsertCommand.Parameters.Add("@ObsTexto", OleDbType.VarChar, 0, "ObsTexto");//tesisDto.ObsTexto;
                dataAdapter.InsertCommand.Parameters.Add("@ObsPrecedentes", OleDbType.VarChar, 0, "ObsPrecedentes");//tesisDto.ObsPrecedentes;

                dataAdapter.Update(dataSet, "Tesis");

                dataSet.Dispose();
                dataAdapter.Dispose();
                connection.Close();

                if (tesisDto.IdGenealogia > 0)
                {
                    ImportarGenealogia(tesisDto);
                }

                if (!String.IsNullOrEmpty(tesisDto.Observaciones))
                    SalvarObservaciones(tesisDto, 2);

                if (!String.IsNullOrEmpty(tesisDto.Concordancia))
                    SalvarObservaciones(tesisDto, 3);

                complete = true;
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UnaTesisModel", "MantesisQuinta");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UnaTesisModel", "MantesisQuinta");
            }

            return complete;
            //            SalvarBitacora(tesisDto, 1);
        }// fin InsertarRegistro

        private long ImportarGenealogia(TesisDto tesisDto)
        {
            OleDbDataAdapter dataAdapter = null;
            DataSet dataSet = new DataSet();
            DataRow dr;
            OleDbConnection connection = new OleDbConnection(ConfigurationManager.ConnectionStrings["Base"].ConnectionString);

            try
            {
                const string sqlCadena = "SELECT * FROM Genealogia WHERE IDGenealogia = 0";

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Genealogia");

                dr = dataSet.Tables["Genealogia"].NewRow();
                dr["IDGenealogia"] = tesisDto.IdGenealogia;
                dr["txtGenealogia"] = tesisDto.Genealogia;

                dataSet.Tables["Genealogia"].Rows.Add(dr);

                dataAdapter.InsertCommand = connection.CreateCommand();

                dataAdapter.InsertCommand.CommandText = "INSERT INTO Genealogia (IDGenealogia, txtGenealogia) " +
                                                        "VALUES (@IDGenealogia, @txtGenealogia)";

                dataAdapter.InsertCommand.Parameters.Add("@IDGenealogia", OleDbType.Double, 0, "IDGenealogia");
                dataAdapter.InsertCommand.Parameters.Add("@txtGenealogia", OleDbType.LongVarChar, 0, "txtGenealogia");

                dataAdapter.Update(dataSet, "Genealogia");
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UnaTesisModel", "MantesisQuinta");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UnaTesisModel", "MantesisQuinta");
            }
            finally
            {
                dataSet.Dispose();
                dataAdapter.Dispose();
                connection.Close();
            }

            return tesisDto.IdGenealogia;
        }

        public bool ActualizarTesisQuinta(TesisQuinta tesisDto)
        {
            bool registroExitoso = false;

            string sSql;
            OleDbConnection connection = new OleDbConnection(connectionString);
            DbDataAdapter dataAdapter;

            //tesisDto.IdGenealogia = SalvarGenealogia(tesisDto);

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = String.Format("SELECT * FROM Tesis WHERE ius = {0}", tesisDto.Ius);

                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Tesis");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["Parte"] = tesisDto.Parte;
                dr["Consec"] = 0;
                dr["IUS"] = tesisDto.Ius;
                dr["Rubro"] = tesisDto.Rubro;
                dr["Texto"] = tesisDto.Texto;
                dr["Precedentes"] = tesisDto.Precedentes;
                dr["Notas"] = tesisDto.Notas;
                dr["Epoca"] = tesisDto.EpocaInt;
                dr["Sala"] = tesisDto.Sala;
                dr["Fuente"] = tesisDto.Fuente;
                dr["Volumen"] = ValuesMant.Volumen;
                dr["Tesis"] = (tesisDto.Tesis.Length == 0) ? " " : tesisDto.Tesis;
                dr["Pagina"] = MiscFunct.Llena(tesisDto.Pagina, 5);
                dr["TA_TJ"] = tesisDto.TaTj;
                dr["Materia1"] = tesisDto.Materia1;
                dr["Materia2"] = tesisDto.Materia2;
                dr["Materia3"] = tesisDto.Materia3;
                dr["Materia4"] = tesisDto.Materia4;
                dr["Materia5"] = tesisDto.Materia5;
                dr["IdGenealogia"] = tesisDto.IdGenealogia;
                dr["VolOrden"] = tesisDto.VolOrden;
                dr["ConsecIndx"] = tesisDto.ConsecIndx;
                dr["IdTCC"] = tesisDto.IdTcc;
                dr["InfAnexos"] = tesisDto.InfAnexos;
                dr["LocAbr"] = String.Empty;
                dr["NumLetra"] = tesisDto.NumLetra;
                dr["ConsecLetra"] = tesisDto.ConsecLetra;
                dr["Instancia"] = tesisDto.Instancia;
                dr["ConsecInst"] = tesisDto.ConsecInst;
                dr["TpoTesis"] = tesisDto.TpoTesis;
                dr["TpoAsunto1"] = tesisDto.TpoAsunto1;
                dr["TpoAsunto2"] = tesisDto.TpoAsunto2;
                dr["TpoAsunto3"] = tesisDto.TpoAsunto3;
                dr["TpoAsunto4"] = tesisDto.TpoAsunto4;
                dr["TpoAsunto5"] = tesisDto.TpoAsunto5;
                dr["TpoPonente1"] = tesisDto.TpoPonente1;
                dr["TpoPonente2"] = tesisDto.TpoPonente2;
                dr["TpoPonente3"] = tesisDto.TpoPonente3;
                dr["TpoPonente4"] = tesisDto.TpoPonente4;
                dr["TpoPonente5"] = tesisDto.TpoPonente5;
                dr["TpoPon1"] = tesisDto.TpoPon1;
                dr["TpoPon2"] = tesisDto.TpoPon2;
                dr["TpoPon3"] = tesisDto.TpoPon3;
                dr["TpoPon4"] = tesisDto.TpoPon4;
                dr["TpoPon5"] = tesisDto.TpoPon5;
                dr["OrdenTesis"] = tesisDto.OrdenTesis;
                dr["OrdenInstancia"] = tesisDto.OrdenInstancia;
                dr["OrdenRubro"] = tesisDto.OrdenRubro;
                dr["IdProg"] = tesisDto.IdProg;
                dr["ExistenTemas"] = tesisDto.ExistenTemas;
                dr["IdClasif10"] = tesisDto.IdClasif10;
                dr["IdSubVolumen"] = tesisDto.IdSubVolumen;
                dr["AnioPublica"] = tesisDto.AnioPublica;
                dr["MesPublica"] = tesisDto.MesPublica;
                dr["SemanaPublica"] = tesisDto.SemanaPublica;

                if (tesisDto.FechaRegistro == null)
                    dr["FechaRegistro"] = DBNull.Value;
                else
                    dr["FechaRegistro"] = tesisDto.FechaRegistro;

                if (tesisDto.FechaModifica == null)
                    dr["FechaModifica"] = DBNull.Value;
                else
                    dr["FechaModifica"] = tesisDto.FechaModifica;

                dr["Estado"] = tesisDto.Estado;
                dr["MotivoModificar"] = tesisDto.MotivoModificar;
                dr["RubroStr"] = StringUtilities.PrepareToAlphabeticalOrder(tesisDto.Rubro);
                dr["BD"] = tesisDto.BaseOrigen;
                dr["BdGrupo"] = tesisDto.BdGrupo;
                dr["NotaPieR"] = tesisDto.NotasRubro;
                dr["NotaPieT"] = tesisDto.NotasTexto;
                dr["NotaPieP"] = tesisDto.NotasPrecedentes;
                dr["NotaGaceta"] = tesisDto.NotasGaceta;
                dr["NotaPublica"] = tesisDto.NotaPublica;
                dr["ModificadoNotas"] = tesisDto.IsNotasModif;
                dr["ObsRubro"] = tesisDto.ObsRubro;
                dr["ObsTexto"] = tesisDto.ObsTexto;
                dr["ObsPrecedentes"] = tesisDto.ObsPrecedentes;
                dr["NoPublica"] = tesisDto.NotaAclaratoria;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connection.CreateCommand();

                sSql = "UPDATE Tesis SET Parte = @Parte,Consec = @Consec, Rubro = @Rubro,Texto = @Texto, Precedentes = @Precedentes,Notas = @Notas, Epoca = @Epoca," +
                       "Sala = @Sala, Fuente = @Fuente,Volumen = @Volumen,Tesis = @Tesis,Pagina = @Pagina,TA_TJ = @TA_TJ,Materia1 = @Materia1,Materia2 = @Materia2, " +
                       "Materia3 = @Materia3, Materia4 = @Materia4, Materia5 = @Materia5,IdGenealogia = @IdGenealogia,VolOrden = @VolOrden,ConsecIndx = @ConsecIndx," +
                       "IdTCC = @IdTCC,InfAnexos = @InfAnexos,LocAbr = @LocAbr,NumLetra = @NumLetra,ConsecLetra = @ConsecLetra,Instancia = @Instancia,ConsecInst = @ConsecInst," +
                       "TpoTesis = @TpoTesis,TpoAsunto1 = @TpoAsunto1,TpoAsunto2 = @TpoAsunto2,TpoAsunto3 = @TpoAsunto3,TpoAsunto4 = @TpoAsunto4,TpoAsunto5 = @TpoAsunto5," +
                       "TpoPonente1 = @TpoPonente1,TpoPonente2 = @TpoPonente2,TpoPonente3 = @TpoPonente3,TpoPonente4 = @TpoPonente4,TpoPonente5 = @TpoPonente5," +
                       "TpoPon1 = @TpoPon1,TpoPon2 = @TpoPon2,TpoPon3 = @TpoPon3,TpoPon4 = @TpoPon4,TpoPon5=@TpoPon5,OrdenTesis = @OrdenTesis,OrdenInstancia = @OrdenInstancia," +
                       "OrdenRubro = @OrdenRubro,IdProg = @IdProg,ExistenTemas = @ExistenTemas,IdClasif10 = @IdClasif10,IdSubVolumen = @IdSubVolumen,AnioPublica = @AnioPublica," +
                       "MesPublica = @MesPublica,SemanaPublica = @SemanaPublica,FechaRegistro = @FechaRegistro,FechaModifica = @FechaModifica,Estado = @Estado," +
                       "MotivoModificar = @MotivoModificar,RubroStr = @RubroStr,BD = @BD,BdGrupo = @BdGrupo,NotaPieR = @NotaPieR, NotaPieT = @NotaPieT, " +
                       "NotaPieP = @NotaPieP, NotaGaceta = @NotaGaceta,  NotaPublica = @NotaPublica, ModificadoNotas = @ModificadoNotas,ObsRubro = @ObsRubro,ObsTexto = @ObsTexto, ObsPrecedentes = @ObsPrecedentes, NoPublica = @NoPublica  " +
                       " WHERE IUS = @IUS";

                dataAdapter.UpdateCommand.CommandText = sSql;

                AddParms((OleDbCommand)dataAdapter.UpdateCommand, "Parte", "Consec", "Rubro", "Texto", "Precedentes", "Notas", "Epoca", "Sala", "Fuente", "Volumen", "Tesis", "Pagina", "TA_TJ",
                    "Materia1", "Materia2", "Materia3", "Materia4", "Materia5", "IdGenealogia", "VolOrden", "ConsecIndx", "IdTCC", "InfAnexos", "LocAbr", "NumLetra", "ConsecLetra", "Instancia",
                    "ConsecInst", "TpoTesis", "TpoAsunto1", "TpoAsunto2", "TpoAsunto3", "TpoAsunto4", "TpoAsunto5", "TpoPonente1", "TpoPonente2", "TpoPonente3", "TpoPonente4", "TpoPonente5",
                    "TpoPon1", "TpoPon2", "TpoPon3", "TpoPon4", "TpoPon5", "OrdenTesis", "OrdenInstancia", "OrdenRubro", "IdProg", "ExistenTemas", "IdClasif10", "IdSubVolumen", "AnioPublica",
                    "MesPublica", "SemanaPublica", "FechaRegistro", "FechaModifica", "Estado", "MotivoModificar", "RubroStr", "BD", "BdGrupo", "NotaPieR", "NotaPieT", "NotaPieP", "NotaGaceta",
                    "NotaPublica", "ModificadoNotas", "ObsRubro", "ObsTexto", "ObsPrecedentes","NoPublica", "IUS");

                dataAdapter.Update(dataSet, "Tesis");
                dataSet.Dispose();
                dataAdapter.Dispose();

                SalvarObservaciones(tesisDto, 2);
                SalvarObservaciones(tesisDto, 3);
                registroExitoso = true;
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UnaTesisModel", "MantesisQuinta");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UnaTesisModel", "MantesisQuinta");
            }
            finally
            {
                connection.Close();
            }

            return registroExitoso;
        }// fin SalvarRegistro

        private long SalvarGenealogia(TesisDto tesisDto)
        {
            DbDataAdapter dataAdapter = null;
            DataSet dataSet = new DataSet();
            DataRow dr;
            OleDbConnection connMantsOle = new OleDbConnection();
            SqlConnection connMantsSql = new SqlConnection();

            try
            {
                string sqlCadena = "SELECT * FROM Genealogia WHERE IDGenealogia =" + tesisDto.IdGenealogia;

                if (ConstMantesis.DBTipoConn == "ACCESS")
                {
                    connMantsOle = (OleDbConnection)connMantesis;
                    dataAdapter = new OleDbDataAdapter();
                    dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connMantsOle);
                }
                else
                {
                    connMantsSql = (SqlConnection)connMantesis;
                    dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connMantsSql);
                }

                dataAdapter.Fill(dataSet, "Genealogia");

                if (dataSet.Tables[0].Rows.Count == 1)
                {
                    if (tesisDto.Genealogia.Trim().Length == 0)
                    {
                        //Si existe y la cadena esta vacia se Elimina
                        if (ConstMantesis.DBTipoConn == "ACCESS")
                        {
                            dataAdapter.DeleteCommand = connMantsOle.CreateCommand();
                        }
                        else
                        {
                            dataAdapter.DeleteCommand = connMantsSql.CreateCommand();
                        }
                        dataAdapter.DeleteCommand.CommandText =
                                                               "DELETE FROM Genealogia WHERE IDGenealogia = " + tesisDto.IdGenealogia;

                        dataAdapter.DeleteCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        //Si existe y la cadena tiene datos se Actualiza
                        dr = dataSet.Tables[0].Rows[0];

                        dr.BeginEdit();
                        dr["txtGenealogia"] = tesisDto.Genealogia;
                        dr.EndEdit();

                        if (ConstMantesis.DBTipoConn == "ACCESS")
                        {
                            dataAdapter.UpdateCommand = connMantsOle.CreateCommand();
                        }
                        else
                        {
                            dataAdapter.UpdateCommand = connMantsSql.CreateCommand();
                        }

                        dataAdapter.UpdateCommand.CommandText =
                                                               "UPDATE Genealogia " +
                                                               "SET txtGenealogia = @txtGenealogia " +
                                                               " WHERE IDGenealogia = @IDGenealogia";

                        if (ConstMantesis.DBTipoConn == "ACCESS")
                        {
                            AddParms((OleDbCommand)dataAdapter.UpdateCommand, "txtGenealogia", "IDGenealogia");
                        }
                        else
                        {
                            AddParms((SqlCommand)dataAdapter.UpdateCommand, "txtGenealogia", "IDGenealogia");
                        }

                        dataAdapter.Update(dataSet, "Genealogia");
                    }
                }
                else
                {
                    if (tesisDto.Genealogia.Trim().Length > 0)
                    {
                        if (tesisDto.IdGenealogia == 0)
                        {
                            // tesisDto.IdGenealogia = RevUltimoRegGenealogia();
                        }

                        dr = dataSet.Tables["Genealogia"].NewRow();
                        dr["IDGenealogia"] = tesisDto.IdGenealogia;
                        dr["txtGenealogia"] = tesisDto.Genealogia;

                        dataSet.Tables["Genealogia"].Rows.Add(dr);

                        if (ConstMantesis.DBTipoConn == "ACCESS")
                        {
                            dataAdapter.InsertCommand = connMantsOle.CreateCommand();
                        }
                        else
                        {
                            dataAdapter.InsertCommand = connMantsSql.CreateCommand();
                        }

                        dataAdapter.InsertCommand.CommandText =
                                                               "INSERT INTO Genealogia (IDGenealogia, txtGenealogia) " +
                                                               "VALUES (@IDGenealogia, @txtGenealogia)";

                        if (ConstMantesis.DBTipoConn == "ACCESS")
                        {
                            ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@IDGenealogia", OleDbType.Double, 0, "IDGenealogia");
                            ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@txtGenealogia", OleDbType.LongVarChar, 0, "txtGenealogia");
                        }
                        else
                        {
                            ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@IDGenealogia", SqlDbType.BigInt, 0, "IDGenealogia");
                            ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@txtGenealogia", SqlDbType.Text, 0, "txtGenealogia");
                        }
                        dataAdapter.Update(dataSet, "Genealogia");
                    }
                }
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UnaTesisModel", "MantesisQuinta");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UnaTesisModel", "MantesisQuinta");
            }
            finally
            {
                dataSet.Dispose();
                dataAdapter.Dispose();
                connMantsOle.Close();
                connMantsSql.Close();
            }

            return tesisDto.IdGenealogia;
        }

        private void SalvarObservaciones(TesisDto tesisDto, int nTipo)
        {
            OleDbDataAdapter dataAdapter = null;
            DataSet dataSet = new DataSet();
            DataRow dr;
            string sTexto;
            OleDbConnection connection = new OleDbConnection(ConfigurationManager.ConnectionStrings["Base"].ConnectionString);

            if (nTipo == 2)
            {
                sTexto = tesisDto.Observaciones;
            }
            else
            {
                sTexto = tesisDto.Concordancia;
            }

            try
            {
                string sqlCadena = "SELECT * FROM OtrosTextos WHERE IUS = @Ius AND TipoNota = @Tipo";// +nTipo;
                
                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connection);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@Ius", tesisDto.Ius);
                dataAdapter.SelectCommand.Parameters.AddWithValue("@Tipo", nTipo);
                dataAdapter.Fill(dataSet, "OtrosTextos");

                if (dataSet.Tables[0].Rows.Count == 1)
                {
                    if (sTexto.Trim().Length == 0)
                    {
                        //Si existe y la cadena esta vacia se Elimina
                        //dataAdapter.DeleteCommand = connMants.CreateCommand();
                        sqlCadena = "DELETE FROM OtrosTextos WHERE IUS = @Ius AND TipoNota = @Tipo";

                        connection.Open();
                        OleDbDataAdapter adapter = new OleDbDataAdapter() { DeleteCommand = new OleDbCommand(sqlCadena, connection) };
                        adapter.DeleteCommand.Parameters.AddWithValue("@Ius", tesisDto.Ius);
                        adapter.DeleteCommand.Parameters.AddWithValue("@Tipo", nTipo);
                        adapter.DeleteCommand.ExecuteNonQuery();
                        connection.Close();
                    }
                    else
                    {
                        //Si existe y la cadena tiene datos se Actualiza
                        dr = dataSet.Tables[0].Rows[0];
                        dr.BeginEdit();
                        dr["Textos"] = sTexto;
                        dr.EndEdit();

                        dataAdapter.UpdateCommand = connection.CreateCommand();
                        dataAdapter.UpdateCommand.CommandText = "UPDATE OtrosTextos SET Textos = @Textos WHERE IUS = @IUS AND TipoNota = @TipoNota";

                        AddParms(dataAdapter.UpdateCommand, "Textos", "IUS", "TipoNota");
                        dataAdapter.Update(dataSet, "OtrosTextos");
                    }
                }
                else
                {
                    if (sTexto.Trim().Length > 0)
                    {
                        dr = dataSet.Tables["OtrosTextos"].NewRow();
                        dr["IUS"] = tesisDto.Ius;
                        dr["TipoNota"] = nTipo;
                        dr["Textos"] = sTexto;

                        dataSet.Tables["OtrosTextos"].Rows.Add(dr);

                        dataAdapter.InsertCommand = connection.CreateCommand();
                        dataAdapter.InsertCommand.CommandText = "INSERT INTO OtrosTextos (IUS, TipoNota, Textos) VALUES (@IUS, @TipoNota, @Textos)";

                        dataAdapter.InsertCommand.Parameters.Add("@IUS", OleDbType.Double, 0, "IUS");
                        dataAdapter.InsertCommand.Parameters.Add("@TipoNota", OleDbType.SmallInt, 0, "TipoNota");
                        dataAdapter.InsertCommand.Parameters.Add("@Textos", OleDbType.LongVarChar, 0, "Textos");

                        dataAdapter.Update(dataSet, "OtrosTextos");
                    }
                }
            }
            catch (OleDbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UnaTesisModel", "MantesisQuinta");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UnaTesisModel", "MantesisQuinta");
            }
            finally
            {
                dataSet.Dispose();
                dataAdapter.Dispose();
            }
        }
        
        #endregion
    }
}