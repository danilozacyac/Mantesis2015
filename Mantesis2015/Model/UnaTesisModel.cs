using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using Mantesis2015.Dto;
using MantesisVerIusCommonObjects.DataAccess;
using MantesisVerIusCommonObjects.Dto;
using MantesisVerIusCommonObjects.Utilities;
using ScjnUtilities;

namespace Mantesis2015.Model
{
    class UnaTesisModel
    {
        private DbConnection connectionEpocas;
        private DbConnection connMantesis;

        private readonly List<TesisReferencia> lstTesisReferencia;

        public UnaTesisModel()
        {
            lstTesisReferencia = new List<TesisReferencia>();
        }

        public void DbConnectionOpen()
        {
            if (ValuesMant.Epoca == ConstMant.Apendice)
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
                    tesis.Epoca = Utils.GetInfoDatosCompartidos(1, Convert.ToInt32(reader["epoca"]));
                    tesis.Sala = Convert.ToInt16(reader["sala"]);
                    tesis.Fuente = Convert.ToInt16(reader["fuente"]);
                    tesis.Volumen = Utils.GetInfoVolumenes(Convert.ToInt32(reader["volumen"]));
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
                    tesis.MotivoModificar = Convert.ToInt32(reader["MotivoModificar"]);
                    tesis.RubroStr = reader["RubroStr"].ToString();
                    tesis.BaseOrigen = reader["BD"].ToString();
                    tesis.BdGrupo = Convert.ToInt16(reader["BdGrupo"]);

                    tesis.Genealogia = this.GetDescripGenealogia(tesis.IdGenealogia);
                    tesis.Observaciones = this.GetDescripOtrosTextos(tesis.Ius, 2);
                    tesis.Concordancia = this.GetDescripOtrosTextos(tesis.Ius, 3);
                    tesis.NotasRubro = reader["NotaPieR"].ToString();
                    tesis.NotasTexto = reader["NotaPieT"].ToString();
                    tesis.NotasPrecedentes = reader["NotaPieR"].ToString();
                    tesis.NotasGaceta = reader["NotaGaceta"].ToString();
                    tesis.NotaPublica = reader["NotaPublica"].ToString();
                    tesis.IsNotasModif = Convert.ToBoolean((reader["ModificadoNotas"].Equals(DBNull.Value)) ? 0 : Convert.ToInt16(reader["ModificadoNotas"]));
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, tesis.Ius);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, tesis.Ius);
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
            DbConnection connTemp;
            DbConnection connIus;
            DataTableReader readerReg;
            DataTableReader readerIus = null;
            string sqlCadena;

            connTemp = DbConnDac.GetConnectTemp();
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

        public string GetDescripTesis(DataTableReader reader, int npos)
        {
            DataTableReader readerReg;
            string sqlCadena = "";
            string sResult = "";

            switch (npos)
            {
                case 0:
                    sqlCadena = "SELECT txtDatosComp FROM DatosComp WHERE idtipo = 2 AND idSubTipo =" + reader["sala"].ToString();
                    readerReg = this.GetDatosRegistro(sqlCadena, connMantesis);
                    if (readerReg.Read())
                    {
                        sResult = readerReg["txtDatosComp"].ToString();
                    }
                    break;
                case 1:
                    sqlCadena = "SELECT txtDatosComp FROM DatosComp WHERE idtipo = 3 AND idSubTipo =" + reader["fuente"].ToString();
                    readerReg = this.GetDatosRegistro(sqlCadena, connMantesis);
                    if (readerReg.Read())
                    {
                        sResult = readerReg["txtDatosComp"].ToString();
                    }
                    break;
                case 2:
                    sqlCadena = "SELECT txtDatosComp FROM DatosComp WHERE idtipo = 4 AND idSubTipo =" + reader["materia1"].ToString();
                    readerReg = this.GetDatosRegistro(sqlCadena, connMantesis);
                    if (readerReg.Read())
                    {
                        sResult = readerReg["txtDatosComp"].ToString();
                    }
                    break;
                case 3:

                    sqlCadena = "SELECT txtDatosComp FROM DatosComp WHERE idtipo = 4 AND idSubTipo =" + reader["materia2"].ToString();
                    readerReg = this.GetDatosRegistro(sqlCadena, connMantesis);
                    if (readerReg.Read())
                    {
                        sResult = readerReg["txtDatosComp"].ToString();
                    }
                    break;
                case 4:

                    sqlCadena = "SELECT txtDatosComp FROM DatosComp WHERE idtipo = 4 AND idSubTipo =" + reader["materia3"].ToString();
                    readerReg = this.GetDatosRegistro(sqlCadena, connMantesis);
                    if (readerReg.Read())
                    {
                        sResult = readerReg["txtDatosComp"].ToString();
                    }
                    break;
                case 5:
                    sqlCadena = "SELECT txtGenealogia FROM Genealogia WHERE idGenealogia = " + reader["idGenealogia"].ToString();
                    readerReg = this.GetDatosRegistro(sqlCadena, connMantesis);
                    if (readerReg.Read())
                    {
                        sResult = readerReg["txtGenealogia"].ToString();
                    }
                    break;
                case 6:
                    sqlCadena = "SELECT textos FROM OtrosTextos WHERE ius = " + reader["ius4"].ToString() + " AND tiponota = 2";
                    readerReg = this.GetDatosRegistro(sqlCadena, connMantesis);
                    if (readerReg.Read())
                    {
                        sResult = readerReg["textos"].ToString();
                    }
                    break;
                case 7:
                    sqlCadena = "SELECT textos FROM OtrosTextos WHERE ius = " + reader["ius4"].ToString() + " AND tiponota = 3";
                    readerReg = this.GetDatosRegistro(sqlCadena, connMantesis);
                    if (readerReg.Read())
                    {
                        sResult = readerReg["textos"].ToString();
                    }
                    break;
                case 8:

                    sqlCadena = "SELECT txtDatosComp FROM DatosComp WHERE idtipo = 4 AND idSubTipo =" + reader["materia4"].ToString();
                    readerReg = this.GetDatosRegistro(sqlCadena, connMantesis);
                    if (readerReg.Read())
                    {
                        sResult = readerReg["txtDatosComp"].ToString();
                    }
                    break;
                case 9:

                    sqlCadena = "SELECT txtDatosComp FROM DatosComp WHERE idtipo = 4 AND idSubTipo =" + reader["materia5"].ToString();
                    readerReg = this.GetDatosRegistro(sqlCadena, connMantesis);
                    if (readerReg.Read())
                    {
                        sResult = readerReg["txtDatosComp"].ToString();
                    }
                    break;
            }

            return sResult;
        }

        /// <summary>
        /// Devuelve el texto de la genealogía asociada a cada tesis
        /// </summary>
        /// <param name="IdGenealogia"></param>
        /// <returns></returns>
        public string GetDescripGenealogia(long idGenealogia)
        {
            SqlConnection connectionMantesisSql = DbConnDac.GetConnectionMantesisSql();
            SqlCommand cmd;
            SqlDataReader reader;

            string genealogia = "";

            try
            {
                connectionMantesisSql.Open();

                string sqlCadena = "SELECT txtGenealogia FROM Genealogia WHERE idGenealogia = " + idGenealogia;

                cmd = new SqlCommand(sqlCadena, connectionMantesisSql);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    genealogia = reader["txtGenealogia"].ToString();
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
                reader = null;
                connectionMantesisSql.Close();
            }

            return genealogia;
        }

        /// <summary>
        /// Devuelve los textos relativos a las observaciones o a la concordancia respectiva a cada tesis
        /// </summary>
        /// <param name="ius"></param>
        /// <param name="tipoNota">2. Observaciones --- 3. Concordancia</param>
        /// <returns></returns>
        public string GetDescripOtrosTextos(int ius, int tipoNota)
        {
            SqlConnection connectionMantesisSql = DbConnDac.GetConnectionMantesisSql();
            SqlCommand cmd;
            SqlDataReader reader;

            string otrosTextos = "";

            try
            {
                connectionMantesisSql.Open();

                string sqlCadena = "SELECT textos FROM OtrosTextos WHERE ius = " + ius + " AND tiponota = " + tipoNota;

                cmd = new SqlCommand(sqlCadena, connectionMantesisSql);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();

                    otrosTextos = reader["textos"].ToString();
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
                reader = null;
                connectionMantesisSql.Close();
            }

            return otrosTextos;
        }

        public void SalvarRegistro(TesisDto tesisDto)
        {
            string sSql;
            OleDbConnection connectionEpsOle = new OleDbConnection();
            DbDataAdapter dataAdapter;

            tesisDto.IdGenealogia = SalvarGenealogia(tesisDto);

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM Tesis WHERE ius4 =" + tesisDto.Ius;

                connectionEpsOle = (OleDbConnection)connectionEpocas;
                dataAdapter = new OleDbDataAdapter();
                dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connectionEpsOle);

                dataAdapter.Fill(dataSet, "Tesis");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["Rubro"] = tesisDto.Rubro;
                dr["Texto"] = tesisDto.Texto;
                dr["Precedentes"] = tesisDto.Precedentes;
                dr["Sala"] = tesisDto.Sala;
                dr["Fuente"] = tesisDto.Fuente;

                if (tesisDto.Tesis == "")
                {
                    tesisDto.Tesis = " ";
                }
                dr["Tesis"] = tesisDto.Tesis;
                dr["Pagina"] = tesisDto.Pagina;
                dr["IdGenealogia"] = tesisDto.IdGenealogia;

                //dr["TA/TJ"] = tesisDto.TaTj; // Implica cambio en la localizacion de la tesis y su Tipo de Clasificacion

                dr["Materia1"] = tesisDto.Materia1;
                dr["Materia2"] = tesisDto.Materia2;
                dr["Materia3"] = tesisDto.Materia3;

                if (ValuesMant.Epoca >= ConstMant.Decima)
                {
                    dr["Materia4"] = tesisDto.Materia4;
                    dr["Materia5"] = tesisDto.Materia5;
                }

                if (ValuesMant.Epoca == 5 || ValuesMant.Epoca == 100)
                {
                    dr["NotaPieR"] = tesisDto.NotasRubro + " ";
                    dr["NotaPieT"] = tesisDto.NotasTexto + " ";
                    dr["NotaPieP"] = tesisDto.NotasPrecedentes + " ";
                    dr["NotaGaceta"] = tesisDto.NotasGaceta + " ";
                    dr["NotaPublica"] = tesisDto.NotaPublica + " ";
                    dr["ModificadoNotas"] = tesisDto.IsNotasModif == true ? 1 : 0;
                }

                dr["Estado"] = tesisDto.Estado;
                dr["MotivoModificar"] = tesisDto.MotivoModificar;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connectionEpsOle.CreateCommand();

                sSql = "UPDATE Tesis " +
                       "SET Tesis.Rubro = @Rubro, " +
                       " Tesis.Texto = @Texto, Tesis.Precedentes = @Precedentes, Tesis.Sala = @Sala, Tesis.Fuente = @Fuente," +
                       " Tesis.Tesis = @Tesis, Tesis.Pagina = @Pagina,Tesis.IdGenealogia = @IdGenealogia,";
                if (ValuesMant.Epoca >= ConstMant.Decima)
                {
                    sSql = sSql + " Tesis.Materia1 = @Materia1, Tesis.Materia2 = @Materia2, Tesis.Materia3 = @Materia3, Tesis.Materia4 = @Materia4, Tesis.Materia5 = @Materia5,";
                }
                else
                {
                    sSql = sSql + " Tesis.Materia1 = @Materia1, Tesis.Materia2 = @Materia2, Tesis.Materia3 = @Materia3, ";
                }

                sSql = sSql + " Tesis.Estado = @Estado, Tesis.MotivoModificar = @MotivoModificar ";

                //Solo Novena y Decima tienen estos campos 
                if (ValuesMant.Epoca == 5 || ValuesMant.Epoca == 100)
                {
                    sSql += ", NotaPieR = @NotaPieR, NotaPieT = @NotaPieT, NotaPieP = @NotaPieP, NotaGaceta = @NotaGaceta, ModificadoNotas = @ModificadoNotas, NotaPublica = @NotaPublica  ";
                }

                sSql += " WHERE Tesis.ius4 = @ius4";

                dataAdapter.UpdateCommand.CommandText = sSql;

                if (ValuesMant.Epoca >= ConstMant.Decima)
                {
                    AddParms((OleDbCommand)dataAdapter.UpdateCommand, "Rubro", "Texto", "Precedentes", "Sala", "Fuente", "Tesis", "Pagina", "IdGenealogia", "Materia1", "Materia2", "Materia3", "Materia4", "Materia5", "Estado", "MotivoModificar", "NotaPieR", "NotaPieT", "NotaPieP", "NotaGaceta", "ModificadoNotas", "NotaPublica", "ius4");
                }
                else if (ValuesMant.Epoca == ConstMant.Novena)
                {
                    AddParms((OleDbCommand)dataAdapter.UpdateCommand, "Rubro", "Texto", "Precedentes", "Sala", "Fuente", "Tesis", "Pagina", "IdGenealogia", "Materia1", "Materia2", "Materia3", "Estado", "MotivoModificar", "NotaPieR", "NotaPieT", "NotaPieP", "NotaGaceta", "ModificadoNotas", "NotaPublica", "ius4");
                }
                else
                {
                    AddParms((OleDbCommand)dataAdapter.UpdateCommand, "Rubro", "Texto", "Precedentes", "Sala", "Fuente", "Tesis", "Pagina", "IdGenealogia", "Materia1", "Materia2", "Materia3", "Estado", "MotivoModificar", "ius4");
                }

                dataAdapter.Update(dataSet, "Tesis");
                dataSet.Dispose();
                dataAdapter.Dispose();

                SalvarObservaciones(tesisDto, 2);
                SalvarObservaciones(tesisDto, 3);
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
                connectionEpsOle.Close();
            }
        }// fin SalvarRegistro

        /// <summary>
        /// Actualiza la información de la tesis en la base de datos Mantesis de SQL
        /// </summary>
        /// <param name="tesisDto">Tesis por actualizar</param>
        /// <param name="idAbs">Identificardor unico GUID</param>
        public void SalvarRegistroMantesisSql(TesisDto tesisDto, string idAbs)
        {
            string sSql;
            SqlConnection connectionMantesisSql = DbConnDac.GetConnectionMantesisSql();
            DbDataAdapter dataAdapter;

            tesisDto.IdGenealogia = SalvarGenealogiaMantesisSql(tesisDto);

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM Tesis WHERE IUS =" + tesisDto.Ius;

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connectionMantesisSql);

                dataAdapter.Fill(dataSet, "Tesis");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["Rubro"] = tesisDto.Rubro;
                dr["Texto"] = tesisDto.Texto;
                dr["Precedentes"] = tesisDto.Precedentes;

                if (String.IsNullOrEmpty(tesisDto.Notas))
                {
                    dr["Notas"] = DBNull.Value;
                }
                else
                {
                    dr["Notas"] = tesisDto.Notas;
                }

                dr["Sala"] = tesisDto.Sala;
                dr["Fuente"] = tesisDto.Fuente;

                if (tesisDto.Tesis == "")
                {
                    tesisDto.Tesis = " ";
                }
                dr["Tesis"] = tesisDto.Tesis;
                dr["Pagina"] = tesisDto.Pagina;
                dr["IdGenealogia"] = tesisDto.IdGenealogia;

                dr["TA_TJ"] = tesisDto.TaTj; // Implica cambio en la localizacion de la tesis y su Tipo de Clasificacion

                dr["Materia1"] = tesisDto.Materia1;
                dr["Materia2"] = tesisDto.Materia2;
                dr["Materia3"] = tesisDto.Materia3;
                dr["Materia4"] = tesisDto.Materia4;
                dr["Materia5"] = tesisDto.Materia5;

                dr["Estado"] = tesisDto.Estado;
                dr["MotivoModificar"] = tesisDto.MotivoModificar;
                dr["RubroStr"] = tesisDto.RubroStr;
                dr["NotaPieR"] = tesisDto.NotasRubro;
                dr["NotaPieT"] = tesisDto.NotasTexto;
                dr["NotaPieP"] = tesisDto.NotasPrecedentes;
                dr["NotaGaceta"] = tesisDto.NotasGaceta;
                dr["ModificadoNotas"] = tesisDto.IsNotasModif;
                dr["NotaPublica"] = tesisDto.NotaPublica;
                dr.EndEdit();

                dataAdapter.UpdateCommand = connectionMantesisSql.CreateCommand();

                sSql = "UPDATE Tesis " +
                       "SET Tesis.Rubro = @Rubro, " +
                       " Tesis.Texto = @Texto, Tesis.Precedentes = @Precedentes, Tesis.Notas = @Notas, Tesis.Sala = @Sala, Tesis.Fuente = @Fuente," +
                       " Tesis.Tesis = @Tesis, Tesis.Pagina = @Pagina,Tesis.IdGenealogia = @IdGenealogia,TA_TJ = @TA_TJ," +
                       " Tesis.Materia1 = @Materia1, Tesis.Materia2 = @Materia2, Tesis.Materia3 = @Materia3, Tesis.Materia4 = @Materia4, Tesis.Materia5 = @Materia5," +
                       " Tesis.FechaModifica = SYSDATETIME(), Tesis.Estado = @Estado, Tesis.MotivoModificar = @MotivoModificar, Tesis.RubroStr = @RubroStr, " +
                       " Tesis.NotaPieR = @NotaPieR, Tesis.NotaPieT = @NotaPieT, Tesis.NotaPieP = @NotaPieP, Tesis.NotaGaceta = @NotaGaceta, Tesis.ModificadoNotas = @ModificadoNotas, NotaPublica = @NotaPublica " +
                       " WHERE Tesis.IUS = @IUS";

                dataAdapter.UpdateCommand.CommandText = sSql;

                AddParms((SqlCommand)dataAdapter.UpdateCommand, "Rubro", "Texto", "Precedentes", "Notas", "Sala", "Fuente", "Tesis", "Pagina", "IdGenealogia", "TA_TJ",
                    "Materia1", "Materia2", "Materia3", "Materia4", "Materia5", "Estado", "MotivoModificar", "RubroStr", "NotaPieR", "NotaPieT", "NotaPieP", "NotaGaceta", "ModificadoNotas", "NotaPublica", "IUS");

                dataAdapter.Update(dataSet, "Tesis");
                dataSet.Dispose();
                dataAdapter.Dispose();

                SalvarObservacionesMantesisSql(tesisDto, 2);
                SalvarObservacionesMantesisSql(tesisDto, 3);

                SalvarBitacora(tesisDto, 2, idAbs);
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
                connectionMantesisSql.Close();
            }
        }// fin SalvarRegistroMantesisSql

        /// <summary>
        /// Actualiza la tabla Tesis en la base de datos IUS en el servidor CT9BD3 cuando se realiza algún pegote,
        /// además de ingresar el cambio dentro de la Bitácora de la tabla denominada Bitacora_Pegotes
        /// </summary>
        /// <param name="tesisDto"></param>
        public void ActualizaRegistroDSql(TesisDto tesisDto)
        {
            SqlConnection sqlConne = DbConnDac.GetConnectionIus();
            SqlDataAdapter dataAdapter;
            SqlCommand cmd;
            cmd = sqlConne.CreateCommand();
            cmd.Connection = sqlConne;

            try
            {
                sqlConne.Open();

                DataSet dataSet = new DataSet();
                DataRow dr;

                string sqlCadena = "SELECT * FROM Tesis WHERE ius =" + tesisDto.Ius;

                //connectionEpsSQL = (SqlConnection)connectionEpocas;
                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, sqlConne);

                dataAdapter.Fill(dataSet, "Tesis");

                dr = dataSet.Tables[0].Rows[0];
                dr.BeginEdit();
                dr["Rubro"] = tesisDto.Rubro;
                dr["Texto"] = tesisDto.Texto;
                dr["Prec"] = tesisDto.Precedentes;
                dr["Sala"] = tesisDto.Sala;
                dr["Fuente"] = tesisDto.Fuente;

                if (tesisDto.Tesis == "")
                {
                    tesisDto.Tesis = " ";
                }
                dr["Tesis"] = tesisDto.Tesis;
                dr["Pagina"] = tesisDto.Pagina;

                //dr["TA/TJ"] = tesisDto.TaTj; // Implica cambio en la localizacion de la tesis y su Tipo de Clasificacion

                dr["Materia1"] = tesisDto.Materia1;
                dr["Materia2"] = tesisDto.Materia2;
                dr["Materia3"] = tesisDto.Materia3;

                dr.EndEdit();

                dataAdapter.UpdateCommand = sqlConne.CreateCommand();

                string sSql = "UPDATE Tesis " +
                              "SET Tesis.Rubro = @Rubro, " +
                              " Tesis.Texto = @Texto, Tesis.Prec = @Prec, Tesis.Sala = @Sala, Tesis.Fuente = @Fuente," +
                              " Tesis.Tesis = @Tesis, Tesis.Pagina = @Pagina,";
                sSql = sSql + " Tesis.Materia1 = @Materia1, Tesis.Materia2 = @Materia2, Tesis.Materia3 = @Materia3 ";

                sSql = sSql + " WHERE Tesis.ius = @ius";

                dataAdapter.UpdateCommand.CommandText = sSql;

                AddParms(dataAdapter.UpdateCommand, "Rubro", "Texto", "Prec", "Sala", "Fuente", "Tesis", "Pagina", "Materia1", "Materia2", "Materia3", "ius");

                dataAdapter.Update(dataSet, "Tesis");
                dataSet.Dispose();
                dataAdapter.Dispose();

                cmd.CommandText = "INSERT INTO Bitacora_Pegotes(IUS,BDatos,Rubro,anio,mes,dia,fecha,estado,usrModifica) VALUES(" + tesisDto.Ius + ",'" + DbConnDac.BaseActual + "','" +
                                  tesisDto.Rubro + "'," + DateTime.Now.Year + "," + DateTime.Now.Month + "," + DateTime.Now.Day + ",'" +
                                  DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + "',1, '" + Environment.MachineName + "')";
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
            catch (IndexOutOfRangeException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message + @"\n No se actualizó la en la base de CT9BD3 porque no existe el registro todavía", methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                sqlConne.Close();
            }
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
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, nIus);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, nIus);
            }
            finally
            {
                reader.Close();
                connectionMantesisSql.Close();
            }

            return tesisExist;
        }

        public void InsertarRegistro(TesisDto tesisDto)
        {
            OleDbConnection connectionEpsOle = new OleDbConnection();
            SqlConnection connectionEpsSql = new SqlConnection();
            DbDataAdapter dataAdapter;
            long nIus;
            long nIdGenealogia;
            string sRubroStr;

            DataSet dataSet = new DataSet();
            DataRow dr;

            ValuesMant.IusActualLstTesis = -1;

            try
            {
                string sqlCadena = "SELECT * FROM Tesis WHERE ius4 =0";
                if (ConstMant.DBTipoConn == "ACCESS")
                {
                    connectionEpsOle = (OleDbConnection)connectionEpocas;
                    dataAdapter = new OleDbDataAdapter();
                    dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connectionEpsOle);
                }
                else
                {
                    connectionEpsSql = (SqlConnection)connectionEpocas;
                    dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connectionEpsSql);
                }

                dataAdapter.Fill(dataSet, "Tesis");

                nIus = RevUltimoRegIus();
                nIdGenealogia = RevUltimoRegGenealogia();

                sRubroStr = tesisDto.Rubro;
                sRubroStr = StringUtilities.QuitaCarCad(sRubroStr);
                sRubroStr = StringUtilities.ConvMay(sRubroStr);
                sRubroStr = MiscFunct.QuitaDblEspacio(sRubroStr);
                sRubroStr = sRubroStr.Trim();

                if (sRubroStr.Length > 250)
                {
                    sRubroStr = sRubroStr.Substring(0, 250);
                }

                dr = dataSet.Tables["Tesis"].NewRow();
                dr["Parte"] = 0;
                dr["Consec"] = 0;
                dr["IUS4"] = nIus;
                dr["Rubro"] = tesisDto.Rubro;
                dr["Texto"] = tesisDto.Texto;
                dr["Precedentes"] = tesisDto.Precedentes;
                dr["Epoca"] = ValuesMant.Epoca;
                dr["Sala"] = tesisDto.Sala;
                dr["Fuente"] = tesisDto.Fuente;
                dr["Volumen"] = ValuesMant.Volumen;
                dr["Tesis"] = (tesisDto.Tesis.Length == 0) ? " " : tesisDto.Tesis;
                dr["TC"] = " ";
                dr["Pagina"] = MiscFunct.Llena(tesisDto.Pagina, 5);
                dr["TATJ"] = tesisDto.TaTj;
                dr["Materia1"] = tesisDto.Materia1;
                dr["Materia2"] = tesisDto.Materia2;
                dr["Materia3"] = tesisDto.Materia3;
                dr["Materia4"] = tesisDto.Materia4;
                dr["Materia5"] = 0;

                dr["IdGenealogia"] = (tesisDto.Genealogia.Length > 0) ? nIdGenealogia : 0;
                dr["Estado"] = tesisDto.Estado;
                dr["MotivoModificar"] = tesisDto.MotivoModificar;

                dr["RubroStr"] = sRubroStr;

                dataSet.Tables["Tesis"].Rows.Add(dr);

                if (ConstMant.DBTipoConn.Equals("ACCESS"))
                {
                    //dataAdapter.UpdateCommand = connectionEpsOle.CreateCommand();
                    dataAdapter.InsertCommand = connectionEpsOle.CreateCommand();
                    dataAdapter.InsertCommand.CommandText =
                                                           "INSERT INTO Tesis(Parte,Consec,IUS4,Rubro,Texto,Precedentes,Epoca,Sala,Fuente," +
                                                           "Volumen,Tesis,TC,Pagina,TATJ,Materia1,Materia2,Materia3,Materia4,Materia5," +
                                                           "IdGenealogia,Estado,MotivoModificar,RubroStr)" +
                                                           " VALUES(@Parte,@Consec,@IUS4,@Rubro,@Texto,@Precedentes,@Epoca,@Sala,@Fuente," +
                                                           "@Volumen,@Tesis,@TC,@Pagina,@TATJ,@Materia1,@Materia2,@Materia3,@Materia4,@Materia5," +
                                                           "@IdGenealogia,@Estado,@MotivoModificar,@RubroStr)";

                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Parte", OleDbType.Numeric, 0, "Parte");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Consec", OleDbType.Numeric, 0, "Consec");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@IUS4", OleDbType.BigInt, 0, "IUS4");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Rubro", OleDbType.LongVarChar, 0, "Rubro");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Texto", OleDbType.LongVarChar, 0, "Texto");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Precedentes", OleDbType.LongVarChar, 0, "Precedentes");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Epoca", OleDbType.Numeric, 0, "Epoca");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Sala", OleDbType.Numeric, 0, "Sala");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Fuente", OleDbType.Numeric, 0, "Fuente");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Volumen", OleDbType.Numeric, 0, "Volumen");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Tesis", OleDbType.VarChar, 0, "Tesis");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@TC", OleDbType.VarChar, 0, "TC");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Pagina", OleDbType.VarChar, 0, "Pagina");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@TATJ", OleDbType.Numeric, 0, "TATJ");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Materia1", OleDbType.Numeric, 0, "Materia1");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Materia2", OleDbType.Numeric, 0, "Materia2");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Materia3", OleDbType.Numeric, 0, "Materia3");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Materia4", OleDbType.Numeric, 0, "Materia4");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Materia5", OleDbType.Numeric, 0, "Materia5");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@IdGenealogia", OleDbType.BigInt, 0, "IdGenealogia");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Estado", OleDbType.Numeric, 0, "Estado");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@MotivoModificar", OleDbType.BigInt, 0, "MotivoModificar");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@RubroStr", OleDbType.LongVarChar, 0, "RubroStr");
                }
                else
                {
                    //dataAdapter.UpdateCommand = connectionEpsSQL.CreateCommand();
                    dataAdapter.InsertCommand = connectionEpsSql.CreateCommand();
                    dataAdapter.InsertCommand.CommandText = "INSERT INTO Tesis(Parte,Consec,IUS4,Rubro,Texto,Precedentes,Epoca,Sala,Fuente," +
                                                            "Volumen,Tesis,TC,Pagina,TATJ,Materia1,Materia2,Materia3,Materia4,Materia5," +
                                                            "IdGenealogia,Estado,MotivoModificar,RubroStr)" +
                                                            " VALUES(@Parte,@Consec,@IUS4,@Rubro,@Texto,@Precedentes,@Epoca,@Sala,@Fuente," +
                                                            "@Volumen,@Tesis,@TC,@Pagina,@TATJ,@Materia1,@Materia2,@Materia3,@Materia4,@Materia5," +
                                                            "@IdGenealogia,@Estado,@MotivoModificar,@RubroStr)";

                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Parte", SqlDbType.Int, 0, "Parte");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Consec", SqlDbType.Int, 0, "Consec");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@IUS4", SqlDbType.BigInt, 0, "IUS4");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Rubro", SqlDbType.Text, 0, "Rubro");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Texto", SqlDbType.Text, 0, "Texto");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Precedentes", SqlDbType.Text, 0, "Precedentes");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Epoca", SqlDbType.Int, 0, "Epoca");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Sala", SqlDbType.Int, 0, "Sala");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Fuente", SqlDbType.Int, 0, "Fuente");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Volumen", SqlDbType.Int, 0, "Volumen");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Tesis", SqlDbType.VarChar, 0, "Tesis");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@TC", SqlDbType.VarChar, 0, "TC");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Pagina", SqlDbType.VarChar, 0, "Pagina");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@TATJ", SqlDbType.Int, 0, "TATJ");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Materia1", SqlDbType.Int, 0, "Materia1");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Materia2", SqlDbType.Int, 0, "Materia2");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Materia3", SqlDbType.Int, 0, "Materia3");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Materia4", SqlDbType.Int, 0, "Materia4");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Materia5", SqlDbType.Int, 0, "Materia5");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@IdGenealogia", SqlDbType.BigInt, 0, "IdGenealogia");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Estado", SqlDbType.Int, 0, "Estado");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@MotivoModificar", SqlDbType.BigInt, 0, "MotivoModificar");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@RubroStr", SqlDbType.VarChar, 0, "RubroStr");
                }

                dataAdapter.Update(dataSet, "Tesis");

                dataSet.Dispose();
                dataAdapter.Dispose();
                connectionEpsOle.Close();

                if (tesisDto.Genealogia.Length > 0)
                {
                    SalvarGenealogia(tesisDto);
                }

                if (tesisDto.Observaciones.Length > 0)
                {
                    SalvarObservaciones(tesisDto, 2);
                }

                if (tesisDto.Concordancia.Length > 0)
                {
                    SalvarObservaciones(tesisDto, 3);
                }

                ValuesMant.IusActualLstTesis = nIus;
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
            //            SalvarBitacora(tesisDto, 1);
        }// fin InsertarRegistro

        /// <summary>
        /// Ingresa una nueva tesis a la base de datos
        /// </summary>
        /// <param name="tesisDto">Tesis que será guardada</param>
        public void InsertarRegistroMantesisSql(TesisDto tesisDto)
        {
            SqlConnection connectionBitacoraSql = DbConnDac.GetConnectionMantesisSql();
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM Tesis WHERE ius = 0";

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connectionBitacoraSql);

                dataAdapter.Fill(dataSet, "Tesis");

                dr = dataSet.Tables["Tesis"].NewRow();
                dr["Parte"] = tesisDto.Parte;
                dr["Consec"] = tesisDto.Consec;
                dr["IUS"] = tesisDto.Ius;
                dr["Rubro"] = tesisDto.Rubro;
                dr["Texto"] = tesisDto.Texto;
                dr["Precedentes"] = tesisDto.Precedentes;
                dr["Notas"] = tesisDto.Notas;
                dr["Epoca"] = tesisDto.Epoca;
                dr["Sala"] = tesisDto.Sala;
                dr["Fuente"] = tesisDto.Fuente;
                dr["Volumen"] = tesisDto.Volumen;
                dr["Tesis"] = (tesisDto.Tesis.Length == 0) ? " " : tesisDto.Tesis;
                dr["Pagina"] = tesisDto.Pagina;
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
                dr["LocAbr"] = tesisDto.LocAbr;
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
                dr["idProg"] = tesisDto.IdProg;
                dr["ExistenTemas"] = tesisDto.ExistenTemas;
                dr["idClasif10"] = tesisDto.IdClasif10;
                dr["idSubVolumen"] = tesisDto.IdSubVolumen;
                dr["AnioPublica"] = tesisDto.AnioPublica;
                dr["MesPublica"] = tesisDto.MesPublica;
                dr["SemanaPublica"] = tesisDto.SemanaPublica;
                dr["FechaRegistro"] = tesisDto.FechaRegistro;
                dr["FechaModifica"] = tesisDto.FechaModifica;
                dr["Estado"] = tesisDto.Estado;
                dr["MotivoModificar"] = tesisDto.MotivoModificar;
                dr["RubroStr"] = tesisDto.RubroStr;
                dr["BD"] = tesisDto.BaseOrigen;
                dr["BDGrupo"] = tesisDto.BdGrupo;

                dataSet.Tables["Tesis"].Rows.Add(dr);

                dataAdapter.InsertCommand = connectionBitacoraSql.CreateCommand();
                dataAdapter.InsertCommand.CommandText = "INSERT INTO Tesis(Parte,Consec,IUS,Rubro,Texto,Precedentes,Notas,Epoca,Sala,Fuente," +
                                                        "Volumen,Tesis,Pagina,TA_TJ,Materia1,Materia2,Materia3,Materia4,Materia5," +
                                                        "IdGenealogia,VolOrden,ConsecIndx,IdTCC,InfAnexos,LocAbr,NumLetra,ConsecLetra,Instancia,ConsecInst," +
                                                        "TpoTesis,TpoAsunto1,TpoAsunto2,TpoAsunto3,TpoAsunto4,TpoAsunto5,TpoPonente1,TpoPonente2,TpoPonente3," +
                                                        "TpoPonente4,TpoPonente5,TpoPon1,TpoPon2,TpoPon3,TpoPon4,TpoPon5,OrdenTesis,OrdenInstancia,OrdenRubro," +
                                                        "IdProg,ExistenTemas,IdClasif10,IdSubVolumen,AnioPublica,MesPublica,SemanaPublica,FechaRegistro," +
                                                        "FechaModifica,Estado,MotivoModificar,RubroStr,BD,BDGrupo)" +
                                                        " VALUES(@Parte,@Consec,@IUS,@Rubro,@Texto,@Precedentes,@Notas,@Epoca,@Sala,@Fuente," +
                                                        "@Volumen,@Tesis,@Pagina,@TA_TJ,@Materia1,@Materia2,@Materia3,@Materia4,@Materia5," +
                                                        "@IdGenealogia,@VolOrden,@ConsecIndx,@IdTCC,@InfAnexos,@LocAbr,@NumLetra,@ConsecLetra,@Instancia,@ConsecInst," +
                                                        "@TpoTesis,@TpoAsunto1,@TpoAsunto2,@TpoAsunto3,@TpoAsunto4,@TpoAsunto5,@TpoPonente1,@TpoPonente2,@TpoPonente3," +
                                                        "@TpoPonente4,@TpoPonente5,@TpoPon1,@TpoPon2,@TpoPon3,@TpoPon4,@TpoPon5,@OrdenTesis,@OrdenInstancia,@OrdenRubro," +
                                                        "@IdProg,@ExistenTemas,@IdClasif10,@IdSubVolumen,@AnioPublica,@MesPublica,@SemanaPublica,@FechaRegistro," +
                                                        "@FechaModifica,@Estado,@MotivoModificar,@RubroStr,@BD,@BDGrupo)";

                dataAdapter.InsertCommand.Parameters.Add("@Parte", SqlDbType.Int, 0, "Parte");
                dataAdapter.InsertCommand.Parameters.Add("@Consec", SqlDbType.Int, 0, "Consec");
                dataAdapter.InsertCommand.Parameters.Add("@IUS", SqlDbType.BigInt, 0, "IUS");
                dataAdapter.InsertCommand.Parameters.Add("@Rubro", SqlDbType.Text, 0, "Rubro");
                dataAdapter.InsertCommand.Parameters.Add("@Texto", SqlDbType.Text, 0, "Texto");
                dataAdapter.InsertCommand.Parameters.Add("@Precedentes", SqlDbType.Text, 0, "Precedentes");
                dataAdapter.InsertCommand.Parameters.Add("@Notas", SqlDbType.Text, 0, "Notas");
                dataAdapter.InsertCommand.Parameters.Add("@Epoca", SqlDbType.Int, 0, "Epoca");
                dataAdapter.InsertCommand.Parameters.Add("@Sala", SqlDbType.Int, 0, "Sala");
                dataAdapter.InsertCommand.Parameters.Add("@Fuente", SqlDbType.Int, 0, "Fuente");
                dataAdapter.InsertCommand.Parameters.Add("@Volumen", SqlDbType.Int, 0, "Volumen");
                dataAdapter.InsertCommand.Parameters.Add("@Tesis", SqlDbType.VarChar, 0, "Tesis");
                dataAdapter.InsertCommand.Parameters.Add("@Pagina", SqlDbType.VarChar, 0, "Pagina");
                dataAdapter.InsertCommand.Parameters.Add("@TA_TJ", SqlDbType.Int, 0, "TA_TJ");
                dataAdapter.InsertCommand.Parameters.Add("@Materia1", SqlDbType.Int, 0, "Materia1");
                dataAdapter.InsertCommand.Parameters.Add("@Materia2", SqlDbType.Int, 0, "Materia2");
                dataAdapter.InsertCommand.Parameters.Add("@Materia3", SqlDbType.Int, 0, "Materia3");
                dataAdapter.InsertCommand.Parameters.Add("@Materia4", SqlDbType.Int, 0, "Materia4");
                dataAdapter.InsertCommand.Parameters.Add("@Materia5", SqlDbType.Int, 0, "Materia5");
                dataAdapter.InsertCommand.Parameters.Add("@IdGenealogia", SqlDbType.Int, 0, "IdGenealogia");
                dataAdapter.InsertCommand.Parameters.Add("@VolOrden", SqlDbType.Int, 0, "VolOrden");
                dataAdapter.InsertCommand.Parameters.Add("@ConsecIndx", SqlDbType.Int, 0, "ConsecIndx");
                dataAdapter.InsertCommand.Parameters.Add("@IdTCC", SqlDbType.VarChar, 0, "IdTCC");
                dataAdapter.InsertCommand.Parameters.Add("@InfAnexos", SqlDbType.Int, 0, "InfAnexos");
                dataAdapter.InsertCommand.Parameters.Add("@LocAbr", SqlDbType.NVarChar, 0, "LocAbr");
                dataAdapter.InsertCommand.Parameters.Add("@NumLetra", SqlDbType.Int, 0, "NumLetra");
                dataAdapter.InsertCommand.Parameters.Add("@ConsecLetra", SqlDbType.Int, 0, "ConsecLetra");
                dataAdapter.InsertCommand.Parameters.Add("@Instancia", SqlDbType.Int, 0, "Instancia");
                dataAdapter.InsertCommand.Parameters.Add("@ConsecInst", SqlDbType.Int, 0, "ConsecInst");
                dataAdapter.InsertCommand.Parameters.Add("@TpoTesis", SqlDbType.TinyInt, 0, "TpoTesis");
                dataAdapter.InsertCommand.Parameters.Add("@TpoAsunto1", SqlDbType.Int, 0, "TpoAsunto1");
                dataAdapter.InsertCommand.Parameters.Add("@TpoAsunto2", SqlDbType.Int, 0, "TpoAsunto2");
                dataAdapter.InsertCommand.Parameters.Add("@TpoAsunto3", SqlDbType.Int, 0, "TpoAsunto3");
                dataAdapter.InsertCommand.Parameters.Add("@TpoAsunto4", SqlDbType.Int, 0, "TpoAsunto4");
                dataAdapter.InsertCommand.Parameters.Add("@TpoAsunto5", SqlDbType.Int, 0, "TpoAsunto5");
                dataAdapter.InsertCommand.Parameters.Add("@TpoPonente1", SqlDbType.Int, 0, "TpoPonente1");
                dataAdapter.InsertCommand.Parameters.Add("@TpoPonente2", SqlDbType.Int, 0, "TpoPonente2");
                dataAdapter.InsertCommand.Parameters.Add("@TpoPonente3", SqlDbType.Int, 0, "TpoPonente3");
                dataAdapter.InsertCommand.Parameters.Add("@TpoPonente4", SqlDbType.Int, 0, "TpoPonente4");
                dataAdapter.InsertCommand.Parameters.Add("@TpoPonente5", SqlDbType.Int, 0, "TpoPonente5");
                dataAdapter.InsertCommand.Parameters.Add("@TpoPon1", SqlDbType.Int, 0, "TpoPon1");
                dataAdapter.InsertCommand.Parameters.Add("@TpoPon2", SqlDbType.Int, 0, "TpoPon2");
                dataAdapter.InsertCommand.Parameters.Add("@TpoPon3", SqlDbType.Int, 0, "TpoPon3");
                dataAdapter.InsertCommand.Parameters.Add("@TpoPon4", SqlDbType.Int, 0, "TpoPon4");
                dataAdapter.InsertCommand.Parameters.Add("@TpoPon5", SqlDbType.Int, 0, "TpoPon5");
                dataAdapter.InsertCommand.Parameters.Add("@OrdenTesis", SqlDbType.Int, 0, "OrdenTesis");
                dataAdapter.InsertCommand.Parameters.Add("@OrdenInstancia", SqlDbType.Int, 0, "OrdenInstancia");
                dataAdapter.InsertCommand.Parameters.Add("@OrdenRubro", SqlDbType.Int, 0, "OrdenRubro");
                dataAdapter.InsertCommand.Parameters.Add("@idProg", SqlDbType.NVarChar, 0, "idProg");
                dataAdapter.InsertCommand.Parameters.Add("@ExistenTemas", SqlDbType.TinyInt, 0, "ExistenTemas");
                dataAdapter.InsertCommand.Parameters.Add("@idClasif10", SqlDbType.SmallInt, 0, "idClasif10");
                dataAdapter.InsertCommand.Parameters.Add("@idSubVolumen", SqlDbType.SmallInt, 0, "idSubVolumen");
                dataAdapter.InsertCommand.Parameters.Add("@AnioPublica", SqlDbType.Int, 0, "AnioPublica");
                dataAdapter.InsertCommand.Parameters.Add("@MesPublica", SqlDbType.Int, 0, "MesPublica");
                dataAdapter.InsertCommand.Parameters.Add("@SemanaPublica", SqlDbType.Int, 0, "SemanaPublica");
                dataAdapter.InsertCommand.Parameters.Add("@FechaRegistro", SqlDbType.DateTime, 0, "FechaRegistro");
                dataAdapter.InsertCommand.Parameters.Add("@FechaModifica", SqlDbType.DateTime, 0, "FechaModifica");
                dataAdapter.InsertCommand.Parameters.Add("@Estado", SqlDbType.TinyInt, 0, "Estado");
                dataAdapter.InsertCommand.Parameters.Add("@MotivoModificar", SqlDbType.Int, 0, "MotivoModificar");
                dataAdapter.InsertCommand.Parameters.Add("@RubroStr", SqlDbType.NVarChar, 0, "RubroStr");
                dataAdapter.InsertCommand.Parameters.Add("@BD", SqlDbType.NVarChar, 0, "BD");
                dataAdapter.InsertCommand.Parameters.Add("@BDGrupo", SqlDbType.TinyInt, 0, "BDGrupo");

                dataAdapter.Update(dataSet, "Tesis");

                dataSet.Dispose();
                dataAdapter.Dispose();
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
        }

        public void InsertarRegistroInf(TesisDto tesisDto)
        {
            OleDbConnection connectionEpsOle = new OleDbConnection();
            SqlConnection connectionEpsSql = new SqlConnection();
            DbDataAdapter dataAdapter;
            long nIus;
            //long nIdGenealogia;
            string sRubroStr;

            DataSet dataSet = new DataSet();
            DataRow dr;

            ValuesMant.IusActualLstTesis = -1;

            try
            {
                string sqlCadena = "SELECT * FROM Tesis WHERE ius4 =0";
                if (ConstMant.DBTipoConn == "ACCESS")
                {
                    connectionEpsOle = (OleDbConnection)DbConnDac.GetConnectEpocas(ValuesMant.Epoca, ValuesMant.Volumen, 1);
                    dataAdapter = new OleDbDataAdapter();
                    dataAdapter.SelectCommand = new OleDbCommand(sqlCadena, connectionEpsOle);
                }
                else
                {
                    connectionEpsSql = (SqlConnection)connectionEpocas;
                    dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connectionEpsSql);
                }

                dataAdapter.Fill(dataSet, "Tesis");

                nIus = RevUltimoRegIus();
                //nIdGenealogia = RevUltimoRegGenealogia();

                sRubroStr = tesisDto.Rubro;
                sRubroStr = StringUtilities.QuitaCarCad(sRubroStr);
                sRubroStr = StringUtilities.ConvMay(sRubroStr);
                sRubroStr = MiscFunct.QuitaDblEspacio(sRubroStr);
                sRubroStr = sRubroStr.Trim();

                if (sRubroStr.Length > 250)
                {
                    sRubroStr = sRubroStr.Substring(0, 250);
                }

                dr = dataSet.Tables["Tesis"].NewRow();
                dr["Parte"] = 0;
                dr["Consec"] = 0;
                dr["IUS4"] = nIus;
                dr["Rubro"] = tesisDto.Rubro;
                dr["Texto"] = tesisDto.Texto;
                dr["Precedentes"] = tesisDto.Precedentes;
                dr["Epoca"] = ValuesMant.Epoca;
                dr["Sala"] = tesisDto.Sala;
                dr["Fuente"] = tesisDto.Fuente;
                dr["Volumen"] = ValuesMant.Volumen;
                dr["Tesis"] = (tesisDto.Tesis.Length == 0) ? " " : tesisDto.Tesis;
                dr["TC"] = " ";
                dr["Pagina"] = MiscFunct.Llena(tesisDto.Pagina, 5);
                dr["TATJ"] = tesisDto.TaTj;
                dr["Materia1"] = tesisDto.Materia1;
                dr["Materia2"] = tesisDto.Materia2;
                dr["Materia3"] = tesisDto.Materia3;
                dr["Materia4"] = tesisDto.Materia4;
                dr["Materia5"] = 0;

                dr["IdGenealogia"] = 0;
                dr["Estado"] = tesisDto.Estado;
                dr["MotivoModificar"] = tesisDto.MotivoModificar;

                dr["RubroStr"] = sRubroStr;

                dataSet.Tables["Tesis"].Rows.Add(dr);

                if (ConstMant.DBTipoConn == "ACCESS")
                {
                    //dataAdapter.UpdateCommand = connectionEpsOle.CreateCommand();
                    dataAdapter.InsertCommand = connectionEpsOle.CreateCommand();
                    dataAdapter.InsertCommand.CommandText =
                                                           "INSERT INTO Tesis(Parte,Consec,IUS4,Rubro,Texto,Precedentes,Epoca,Sala,Fuente," +
                                                           "Volumen,Tesis,TC,Pagina,TATJ,Materia1,Materia2,Materia3,Materia4,Materia5," +
                                                           "IdGenealogia,Estado,MotivoModificar,RubroStr)" +
                                                           " VALUES(@Parte,@Consec,@IUS4,@Rubro,@Texto,@Precedentes,@Epoca,@Sala,@Fuente," +
                                                           "@Volumen,@Tesis,@TC,@Pagina,@TATJ,@Materia1,@Materia2,@Materia3,@Materia4,@Materia5," +
                                                           "@IdGenealogia,@Estado,@MotivoModificar,@RubroStr)";

                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Parte", OleDbType.Numeric, 0, "Parte");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Consec", OleDbType.Numeric, 0, "Consec");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@IUS4", OleDbType.BigInt, 0, "IUS4");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Rubro", OleDbType.LongVarChar, 0, "Rubro");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Texto", OleDbType.LongVarChar, 0, "Texto");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Precedentes", OleDbType.LongVarChar, 0, "Precedentes");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Epoca", OleDbType.Numeric, 0, "Epoca");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Sala", OleDbType.Numeric, 0, "Sala");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Fuente", OleDbType.Numeric, 0, "Fuente");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Volumen", OleDbType.Numeric, 0, "Volumen");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Tesis", OleDbType.VarChar, 0, "Tesis");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@TC", OleDbType.VarChar, 0, "TC");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Pagina", OleDbType.VarChar, 0, "Pagina");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@TATJ", OleDbType.Numeric, 0, "TATJ");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Materia1", OleDbType.Numeric, 0, "Materia1");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Materia2", OleDbType.Numeric, 0, "Materia2");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Materia3", OleDbType.Numeric, 0, "Materia3");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Materia4", OleDbType.Numeric, 0, "Materia4");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Materia5", OleDbType.Numeric, 0, "Materia5");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@IdGenealogia", OleDbType.BigInt, 0, "IdGenealogia");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Estado", OleDbType.Numeric, 0, "Estado");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@MotivoModificar", OleDbType.BigInt, 0, "MotivoModificar");
                    ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@RubroStr", OleDbType.LongVarChar, 0, "RubroStr");
                }
                else
                {
                    //dataAdapter.UpdateCommand = connectionEpsSQL.CreateCommand();
                    dataAdapter.InsertCommand = connectionEpsSql.CreateCommand();
                    dataAdapter.InsertCommand.CommandText = "INSERT INTO Tesis(Parte,Consec,IUS4,Rubro,Texto,Precedentes,Epoca,Sala,Fuente," +
                                                            "Volumen,Tesis,TC,Pagina,TATJ,Materia1,Materia2,Materia3,Materia4,Materia5," +
                                                            "IdGenealogia,Estado,MotivoModificar,RubroStr)" +
                                                            " VALUES(@Parte,@Consec,@IUS4,@Rubro,@Texto,@Precedentes,@Epoca,@Sala,@Fuente," +
                                                            "@Volumen,@Tesis,@TC,@Pagina,@TATJ,@Materia1,@Materia2,@Materia3,@Materia4,@Materia5," +
                                                            "@IdGenealogia,@Estado,@MotivoModificar,@RubroStr)";

                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Parte", SqlDbType.Int, 0, "Parte");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Consec", SqlDbType.Int, 0, "Consec");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@IUS4", SqlDbType.BigInt, 0, "IUS4");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Rubro", SqlDbType.Text, 0, "Rubro");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Texto", SqlDbType.Text, 0, "Texto");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Precedentes", SqlDbType.Text, 0, "Precedentes");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Epoca", SqlDbType.Int, 0, "Epoca");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Sala", SqlDbType.Int, 0, "Sala");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Fuente", SqlDbType.Int, 0, "Fuente");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Volumen", SqlDbType.Int, 0, "Volumen");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Tesis", SqlDbType.VarChar, 0, "Tesis");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@TC", SqlDbType.VarChar, 0, "TC");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Pagina", SqlDbType.VarChar, 0, "Pagina");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@TATJ", SqlDbType.Int, 0, "TATJ");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Materia1", SqlDbType.Int, 0, "Materia1");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Materia2", SqlDbType.Int, 0, "Materia2");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Materia3", SqlDbType.Int, 0, "Materia3");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Materia4", SqlDbType.Int, 0, "Materia4");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Materia5", SqlDbType.Int, 0, "Materia5");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@IdGenealogia", SqlDbType.BigInt, 0, "IdGenealogia");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Estado", SqlDbType.Int, 0, "Estado");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@MotivoModificar", SqlDbType.BigInt, 0, "MotivoModificar");
                    ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@RubroStr", SqlDbType.VarChar, 0, "RubroStr");
                }

                dataAdapter.Update(dataSet, "Tesis");

                dataSet.Dispose();
                dataAdapter.Dispose();
                connectionEpsOle.Close();

                OleDbCommand cmd;
                cmd = connectionEpsOle.CreateCommand();
                cmd.Connection = connectionEpsOle;
                connectionEpsOle.Open();
                cmd.CommandText = "INSERT INTO AltaInformes VALUES(" + AccesoUsuarioModel.Llave + "," + nIus + "," +
                                  ValuesMant.Volumen + "," + DateTime.Now.Day + "," + DateTime.Now.Month + "," + DateTime.Now.Year + ")";
                cmd.ExecuteNonQuery();
                connectionEpsOle.Close();

                //if (tesisDto.Genealogia.Length > 0)
                //{
                //    SalvarGenealogia(tesisDto);
                //}

                if (tesisDto.Observaciones.Length > 0)
                {
                    SalvarObservaciones(tesisDto, 2);
                }

                if (tesisDto.Concordancia.Length > 0)
                {
                    SalvarObservaciones(tesisDto, 3);
                }

                ValuesMant.IusActualLstTesis = nIus;
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
            //SalvarBitacora(tesisDto, 1);
        }

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

                if (ConstMant.DBTipoConn == "ACCESS")
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
                        if (ConstMant.DBTipoConn == "ACCESS")
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

                        if (ConstMant.DBTipoConn == "ACCESS")
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

                        if (ConstMant.DBTipoConn == "ACCESS")
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
                            tesisDto.IdGenealogia = RevUltimoRegGenealogia();
                        }

                        dr = dataSet.Tables["Genealogia"].NewRow();
                        dr["IDGenealogia"] = tesisDto.IdGenealogia;
                        dr["txtGenealogia"] = tesisDto.Genealogia;

                        dataSet.Tables["Genealogia"].Rows.Add(dr);

                        if (ConstMant.DBTipoConn == "ACCESS")
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

                        if (ConstMant.DBTipoConn == "ACCESS")
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
            catch (DbException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, tesisDto.Ius);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, tesisDto.Ius);
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

        /// <summary>
        /// Actualiza la genealogia de la tesis en cuestión
        /// </summary>
        /// <param name="tesisDto">Tesis que se esta actualizando</param>
        /// <returns>Regresa el Id asignado a esta nota de genealogia perteneciente a esta tesis</returns>
        private long SalvarGenealogiaMantesisSql(TesisDto tesisDto)
        {
            DbDataAdapter dataAdapter = null;
            DataSet dataSet = new DataSet();
            DataRow dr;
            SqlConnection connectionMantesisSql = DbConnDac.GetConnectionMantesisSql();

            try
            {
                string sqlCadena = "SELECT * FROM Genealogia WHERE IDGenealogia =" + tesisDto.IdGenealogia;

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connectionMantesisSql);

                dataAdapter.Fill(dataSet, "Genealogia");

                if (dataSet.Tables[0].Rows.Count == 1)
                {
                    if (tesisDto.Genealogia.Trim().Length == 0)
                    {
                        //Si existe y la cadena esta vacia se Elimina
                        dataAdapter.DeleteCommand = connectionMantesisSql.CreateCommand();

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

                        dataAdapter.UpdateCommand = connectionMantesisSql.CreateCommand();

                        dataAdapter.UpdateCommand.CommandText =
                                                               "UPDATE Genealogia " +
                                                               "SET txtGenealogia = @txtGenealogia " +
                                                               " WHERE IDGenealogia = @IDGenealogia";

                        AddParms((SqlCommand)dataAdapter.UpdateCommand, "txtGenealogia", "IDGenealogia");

                        dataAdapter.Update(dataSet, "Genealogia");
                    }
                }
                else
                {
                    if (tesisDto.Genealogia.Trim().Length > 0)
                    {
                        if (tesisDto.IdGenealogia == 0)
                        {
                            tesisDto.IdGenealogia = RevUltimoRegGenealogiaMantesisSql();
                        }

                        dr = dataSet.Tables["Genealogia"].NewRow();
                        dr["IDGenealogia"] = tesisDto.IdGenealogia;
                        dr["txtGenealogia"] = tesisDto.Genealogia;

                        dataSet.Tables["Genealogia"].Rows.Add(dr);

                        dataAdapter.InsertCommand = connectionMantesisSql.CreateCommand();

                        dataAdapter.InsertCommand.CommandText =
                                                               "INSERT INTO Genealogia (IDGenealogia, txtGenealogia) " +
                                                               "VALUES (@IDGenealogia, @txtGenealogia)";

                        ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@IDGenealogia", SqlDbType.BigInt, 0, "IDGenealogia");
                        ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@txtGenealogia", SqlDbType.Text, 0, "txtGenealogia");

                        dataAdapter.Update(dataSet, "Genealogia");
                    }
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, tesisDto.Ius);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, tesisDto.Ius);
            }
            finally
            {
                dataSet.Dispose();
                dataAdapter.Dispose();
                connectionMantesisSql.Close();
            }
            return tesisDto.IdGenealogia;
        }

        private void SalvarObservaciones(TesisDto tesisDto, int nTipo)
        {
            DbDataAdapter dataAdapter = null;
            DataSet dataSet = new DataSet();
            DataRow dr;
            string sTexto;
            OleDbConnection connMantsOle = new OleDbConnection();
            SqlConnection connMantsSql = new SqlConnection();

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
                string sqlCadena = "SELECT * FROM OtrosTextos WHERE IUS =" + tesisDto.Ius + " AND TipoNota =" + nTipo;
                if (ConstMant.DBTipoConn.Equals("ACCESS"))
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

                dataAdapter.Fill(dataSet, "OtrosTextos");

                if (dataSet.Tables[0].Rows.Count == 1)
                {
                    if (sTexto.Trim().Length == 0)
                    {
                        //Si existe y la cadena esta vacia se Elimina
                        //dataAdapter.DeleteCommand = connMants.CreateCommand();
                        sqlCadena = "DELETE FROM OtrosTextos WHERE IUS = " + tesisDto.Ius + " AND TipoNota =" + nTipo;

                        if (ConstMant.DBTipoConn.Equals("ACCESS"))
                        {
                            //connMantsOle.Open();
                            connMantsOle.Open();
                            OleDbDataAdapter adapter = new OleDbDataAdapter();
                            adapter.DeleteCommand = new OleDbCommand(sqlCadena, connMantsOle);

                            adapter.DeleteCommand.ExecuteNonQuery();

                            connMantsOle.Close();
                        }
                        else
                        {
                            dataAdapter.DeleteCommand = connMantsSql.CreateCommand();
                        }
                        //sqlCadena = "DELETE FROM OtrosTextos WHERE IUS = " + tesisDto.IUS + " AND TipoNota =" + nTipo;
                        //dataAdapter.DeleteCommand.CommandText =
                        //    "DELETE FROM OtrosTextos WHERE IUS = " + tesisDto.IUS + " AND TipoNota =" +nTipo;
                        //dataAdapter.DeleteCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        //Si existe y la cadena tiene datos se Actualiza
                        dr = dataSet.Tables[0].Rows[0];
                        dr.BeginEdit();
                        dr["Textos"] = sTexto;
                        dr.EndEdit();

                        //dataAdapter.UpdateCommand = connMants.CreateCommand();
                        if (ConstMant.DBTipoConn.Equals("ACCESS"))
                        {
                            dataAdapter.UpdateCommand = connMantsOle.CreateCommand();
                        }
                        else
                        {
                            dataAdapter.UpdateCommand = connMantsSql.CreateCommand();
                        }

                        dataAdapter.UpdateCommand.CommandText =
                                                               "UPDATE OtrosTextos " +
                                                               "SET Textos = @Textos " +
                                                               " WHERE IUS = @IUS" +
                                                               " AND TipoNota = @TipoNota";

                        if (ConstMant.DBTipoConn.Equals("ACCESS"))
                        {
                            AddParms((OleDbCommand)dataAdapter.UpdateCommand, "Textos", "IUS", "TipoNota");
                        }
                        else
                        {
                            AddParms((SqlCommand)dataAdapter.UpdateCommand, "Textos", "IUS", "TipoNota");
                        }

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

                        //dataAdapter.InsertCommand = connMants.CreateCommand();
                        if (ConstMant.DBTipoConn.Equals("ACCESS"))
                        {
                            dataAdapter.InsertCommand = connMantsOle.CreateCommand();
                        }
                        else
                        {
                            dataAdapter.InsertCommand = connMantsSql.CreateCommand();
                        }

                        dataAdapter.InsertCommand.CommandText =
                                                               "INSERT INTO OtrosTextos (IUS, TipoNota, Textos) " +
                                                               "VALUES (@IUS, @TipoNota, @Textos)";

                        if (ConstMant.DBTipoConn.Equals("ACCESS"))
                        {
                            ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@IUS", OleDbType.Double, 0, "IUS");
                            ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@TipoNota", OleDbType.SmallInt, 0, "TipoNota");
                            ((OleDbDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Textos", OleDbType.LongVarChar, 0, "Textos");
                        }
                        else
                        {
                            ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@IUS", SqlDbType.BigInt, 0, "IUS");
                            ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@TipoNota", SqlDbType.SmallInt, 0, "TipoNota");
                            ((SqlDataAdapter)dataAdapter).InsertCommand.Parameters.Add("@Textos", SqlDbType.Text, 0, "Textos");
                        }
                        dataAdapter.Update(dataSet, "OtrosTextos");
                    }
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, tesisDto.Ius);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, tesisDto.Ius);
            }
            finally
            {
                dataSet.Dispose();
                dataAdapter.Dispose();
            }
        }

        /// <summary>
        /// Salva el texto introducido en el cuadro de texto respectivo
        /// </summary>
        /// <param name="tesisDto"></param>
        /// <param name="nTipo"> 2. Observaciones  ---  3. Concordancia</param>
        private void SalvarObservacionesMantesisSql(TesisDto tesisDto, int nTipo)
        {
            SqlDataAdapter dataAdapter = null;
            DataSet dataSet = new DataSet();
            DataRow dr;
            string sTexto;
            SqlConnection connMantsSql = DbConnDac.GetConnectionMantesisSql();

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
                string sqlCadena = "SELECT * FROM OtrosTextos WHERE IUS =" + tesisDto.Ius + " AND TipoNota =" + nTipo;

                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connMantsSql);

                dataAdapter.Fill(dataSet, "OtrosTextos");

                if (dataSet.Tables[0].Rows.Count == 1)
                {
                    if (sTexto.Trim().Length == 0)
                    {
                        //Si existe y la cadena esta vacia se Elimina
                        //dataAdapter.DeleteCommand = connMants.CreateCommand();
                        sqlCadena = "DELETE FROM OtrosTextos WHERE IUS = " + tesisDto.Ius + " AND TipoNota =" + nTipo;

                        dataAdapter.DeleteCommand = connMantsSql.CreateCommand();
                        dataAdapter.DeleteCommand.CommandText = sqlCadena;
                        dataAdapter.DeleteCommand.ExecuteNonQuery();
                    }
                    else
                    {
                        //Si existe y la cadena tiene datos se Actualiza
                        dr = dataSet.Tables[0].Rows[0];
                        dr.BeginEdit();
                        dr["Textos"] = sTexto;
                        dr.EndEdit();

                        dataAdapter.UpdateCommand = connMantsSql.CreateCommand();

                        dataAdapter.UpdateCommand.CommandText =
                                                               "UPDATE OtrosTextos " +
                                                               "SET Textos = @Textos " +
                                                               " WHERE IUS = @IUS" +
                                                               " AND TipoNota = @TipoNota";

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

                        dataAdapter.InsertCommand = connMantsSql.CreateCommand();

                        dataAdapter.InsertCommand.CommandText =
                                                               "INSERT INTO OtrosTextos (IUS, TipoNota, Textos) " +
                                                               "VALUES (@IUS, @TipoNota, @Textos)";

                        dataAdapter.InsertCommand.Parameters.Add("@IUS", SqlDbType.BigInt, 0, "IUS");
                        dataAdapter.InsertCommand.Parameters.Add("@TipoNota", SqlDbType.SmallInt, 0, "TipoNota");
                        dataAdapter.InsertCommand.Parameters.Add("@Textos", SqlDbType.Text, 0, "Textos");

                        dataAdapter.Update(dataSet, "OtrosTextos");
                    }
                }

                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, tesisDto.Ius);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, tesisDto.Ius);
            }
            finally
            {
                connMantsSql.Close();
            }
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

        private long RevUltimoRegGenealogia()
        {
            DataTableReader readerReg;
            int nGenealogia = 0;
            string sqlCadena = "";

            sqlCadena = "SELECT IDGenealogia FROM Genealogia ORDER BY IDGenealogia DESC";
            readerReg = this.GetDatosRegistro(sqlCadena, connMantesis);

            if (readerReg.Read())
            {
                nGenealogia = Convert.ToInt32(readerReg["IDGenealogia"].ToString());
                nGenealogia++;
            }
            else
            {
                nGenealogia = -1;
            }

            return nGenealogia;
        }

        /// <summary>
        /// Devuelve el identificador de genealogía que correspondería a la tesis que se esta guardando
        /// </summary>
        /// <returns></returns>
        private long RevUltimoRegGenealogiaMantesisSql()
        {
            int nGenealogia = 0;
            SqlConnection connectionMantesisSql = DbConnDac.GetConnectionMantesisSql();
            SqlCommand cmd;
            SqlDataReader reader;

            try
            {
                string sqlCadena = "SELECT IDGenealogia FROM Genealogia ORDER BY IDGenealogia DESC";
                cmd = new SqlCommand(sqlCadena, connectionMantesisSql);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    nGenealogia = Convert.ToInt32(reader["IDGenealogia"].ToString());
                    nGenealogia++;
                }
                else
                {
                    nGenealogia = -1;
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
                reader = null;
                connectionMantesisSql.Close();
            }
            return nGenealogia;
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
            catch (DbException ex)
            {
                //MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
            }
            catch (SystemException sex)
            {
                MessageBox.Show("Error ({0}) : {1}" + sex.Source + sex.Message, "Error Interno");
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

        /// <summary>
        /// Registra los cambios realizados a una tesis antes del Semanario electrónico
        /// en la Base mBitacora, tabla MantBitacora
        /// </summary>
        /// <param name="tesisDto"></param>
        /// <param name="nTipo">1. Inserta --- 2. Modificacion --- 4. Eliminada</param>
        /// <param name="idAbs">Identificador Unico GUID</param>
        public void SalvarBitacora(TesisDto tesisDto, int nTipo, string idAbs)
        {
            SqlConnection connectionSql = DbConnDac.GetConnectionmBitacora();
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM MantBitacora WHERE ID = 0";

                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connectionSql);
                dataAdapter.Fill(dataSet, "Bitacora");

                dr = dataSet.Tables["Bitacora"].NewRow();
                dr["Id"] = nTipo;
                dr["IdElemento"] = tesisDto.Ius;
                dr["IdUsr"] = AccesoUsuarioModel.Llave;
                dr["FechaStr"] = DateTime.Now.ToString("yyyy/MM/dd");
                dr["Fecha"] = DateTime.Now.ToString("yyyy/MM/dd");
                dr["IdMovimiento"] = tesisDto.MotivoModificar;
                dr["TipoDoc"] = 20;
                dr["TipoCat"] = 20;
                dr["Usuario"] = MiscFunct.GetUserCurrent();
                dr["Comp"] = MiscFunct.GetPcCurrent();
                dr["HoraStr"] = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
                dr["Observaciones"] = tesisDto.CamposModif;
                dr["IdAbs"] = idAbs;

                dataSet.Tables["Bitacora"].Rows.Add(dr);

                dataAdapter.InsertCommand = connectionSql.CreateCommand();
                dataAdapter.InsertCommand.CommandText =
                                                       "INSERT INTO MantBitacora(Id, IdElemento, IdUsr, FechaStr, Fecha, IdMovimiento, TipoDoc,TipoCat,Usuario," +
                                                       "Comp,HoraStr,Observaciones,IdAbs)" +
                                                       " VALUES(@Id,@IdElemento,@IdUsr,@FechaStr,@Fecha,@IdMovimiento,@TipoDoc,@TipoCat,@Usuario," +
                                                       "@Comp,@HoraStr,@Observaciones,@IdAbs)";

                dataAdapter.InsertCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");
                dataAdapter.InsertCommand.Parameters.Add("@IdElemento", SqlDbType.BigInt, 0, "IdElemento");
                dataAdapter.InsertCommand.Parameters.Add("@IdUsr", SqlDbType.Int, 0, "IdUsr");
                dataAdapter.InsertCommand.Parameters.Add("@FechaStr", SqlDbType.Date, 0, "FechaStr");
                dataAdapter.InsertCommand.Parameters.Add("@Fecha", SqlDbType.Date, 0, "Fecha");
                dataAdapter.InsertCommand.Parameters.Add("@IdMovimiento", SqlDbType.BigInt, 0, "IdMovimiento");
                dataAdapter.InsertCommand.Parameters.Add("@TipoDoc", SqlDbType.Int, 0, "TipoDoc");
                dataAdapter.InsertCommand.Parameters.Add("@TipoCat", SqlDbType.Int, 0, "TipoCat");
                dataAdapter.InsertCommand.Parameters.Add("@Usuario", SqlDbType.VarChar, 0, "Usuario");
                dataAdapter.InsertCommand.Parameters.Add("@Comp", SqlDbType.VarChar, 0, "Comp");
                dataAdapter.InsertCommand.Parameters.Add("@HoraStr", SqlDbType.DateTime, 0, "HoraStr");
                dataAdapter.InsertCommand.Parameters.Add("@Observaciones", SqlDbType.VarChar, 0, "Observaciones");
                dataAdapter.InsertCommand.Parameters.Add("@IdAbs", SqlDbType.UniqueIdentifier, 0, "IdAbs");

                dataAdapter.Update(dataSet, "Bitacora");

                dataSet.Dispose();
                dataAdapter.Dispose();
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
                connectionSql.Close();
            }
        }

        /// <summary>
        /// Registra la modificacion realizada a una tesis a partir del numero de registro
        /// </summary>
        /// <param name="pIus">Número de registro ius</param>
        /// <param name="motivoModif">Motivo de la modificacion</param>
        /// <param name="nTipo">1. Inserta --- 2. Modificacion --- 4. Eliminada</param>
        /// <param name="idAbs">Identificador Unico</param>
        public void SalvarBitacora(long pIus, int motivoModif, int nTipo, string idAbs)
        {
            SqlConnection connectionSql = DbConnDac.GetConnectionmBitacora();
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            DataSet dataSet = new DataSet();
            DataRow dr;

            try
            {
                string sqlCadena = "SELECT * FROM MantBitacora WHERE ID = 0";

                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connectionSql);
                dataAdapter.Fill(dataSet, "Bitacora");

                dr = dataSet.Tables["Bitacora"].NewRow();
                dr["Id"] = nTipo;
                dr["IdElemento"] = pIus;
                dr["IdUsr"] = AccesoUsuarioModel.Llave;
                dr["FechaStr"] = DateTime.Now.ToString("yyyy/MM/dd");
                dr["Fecha"] = DateTime.Now.ToString("yyyy/MM/dd");
                dr["IdMovimiento"] = motivoModif;
                dr["TipoDoc"] = 20;
                dr["TipoCat"] = 20;
                dr["Usuario"] = MiscFunct.GetUserCurrent();
                dr["Comp"] = MiscFunct.GetPcCurrent();
                dr["HoraStr"] = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
                dr["IdAbs"] = idAbs;

                dataSet.Tables["Bitacora"].Rows.Add(dr);

                dataAdapter.InsertCommand = connectionSql.CreateCommand();
                dataAdapter.InsertCommand.CommandText =
                                                       "INSERT INTO MantBitacora(Id, IdElemento, IdUsr, FechaStr, Fecha, IdMovimiento, TipoDoc,TipoCat,Usuario," +
                                                       "Comp,HoraStr,IdAbs)" +
                                                       " VALUES(@Id,@IdElemento,@IdUsr,@FechaStr,@Fecha,@IdMovimiento,@TipoDoc,@TipoCat,@Usuario," +
                                                       "@Comp,@HoraStr,@IdAbs)";

                dataAdapter.InsertCommand.Parameters.Add("@Id", SqlDbType.Int, 0, "Id");
                dataAdapter.InsertCommand.Parameters.Add("@IdElemento", SqlDbType.BigInt, 0, "IdElemento");
                dataAdapter.InsertCommand.Parameters.Add("@IdUsr", SqlDbType.Int, 0, "IdUsr");
                dataAdapter.InsertCommand.Parameters.Add("@FechaStr", SqlDbType.Date, 0, "FechaStr");
                dataAdapter.InsertCommand.Parameters.Add("@Fecha", SqlDbType.Date, 0, "Fecha");
                dataAdapter.InsertCommand.Parameters.Add("@IdMovimiento", SqlDbType.BigInt, 0, "IdMovimiento");
                dataAdapter.InsertCommand.Parameters.Add("@TipoDoc", SqlDbType.Int, 0, "TipoDoc");
                dataAdapter.InsertCommand.Parameters.Add("@TipoCat", SqlDbType.Int, 0, "TipoCat");
                dataAdapter.InsertCommand.Parameters.Add("@Usuario", SqlDbType.VarChar, 0, "Usuario");
                dataAdapter.InsertCommand.Parameters.Add("@Comp", SqlDbType.VarChar, 0, "Comp");
                dataAdapter.InsertCommand.Parameters.Add("@HoraStr", SqlDbType.DateTime, 0, "HoraStr");
                dataAdapter.InsertCommand.Parameters.Add("@IdAbs", SqlDbType.UniqueIdentifier, 0, "IdAbs");

                dataAdapter.Update(dataSet, "Bitacora");

                dataSet.Dispose();
                dataAdapter.Dispose();
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
                connectionSql.Close();
            }
        }

        /// <summary>
        /// Guarda el estado de las tesis antes de que fueran modificadas, para poder llevar un registro histórico
        /// </summary>
        /// <param name="tesisDto">Tesis sin modificar</param>
        /// <param name="uniqueId">Identificador unico</param>
        public void InsertaHistoricoTesis(TesisDto tesisDto, String uniqueId)
        {
            SqlConnection connectionBitacoraSql = DbConnDac.GetConnectionmBitacora();
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            string sqlCadena = "SELECT * FROM Tesis WHERE ius = 0";

            try
            {
                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connectionBitacoraSql);

                dataAdapter.Fill(dataSet, "Tesis");

                dr = dataSet.Tables["Tesis"].NewRow();
                dr["Parte"] = tesisDto.Parte;
                dr["Consec"] = tesisDto.Consec;
                dr["IUS"] = tesisDto.Ius;
                dr["Rubro"] = tesisDto.Rubro;
                dr["Texto"] = tesisDto.Texto;
                dr["Precedentes"] = tesisDto.Precedentes;
                dr["Notas"] = tesisDto.Notas;
                dr["Epoca"] = tesisDto.EpocaInt;
                dr["Sala"] = tesisDto.Sala;
                dr["Fuente"] = tesisDto.Fuente;
                dr["Volumen"] = tesisDto.VolumenInt;
                dr["Tesis"] = (tesisDto.Tesis.Length == 0) ? " " : tesisDto.Tesis;
                dr["Pagina"] = tesisDto.Pagina;
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
                dr["LocAbr"] = tesisDto.LocAbr;
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
                dr["idProg"] = tesisDto.IdProg;
                dr["ExistenTemas"] = tesisDto.ExistenTemas;
                dr["idClasif10"] = tesisDto.IdClasif10;
                dr["idSubVolumen"] = tesisDto.IdSubVolumen;
                dr["AnioPublica"] = tesisDto.AnioPublica;
                dr["MesPublica"] = tesisDto.MesPublica;
                dr["SemanaPublica"] = tesisDto.SemanaPublica;
                dr["FechaRegistro"] = tesisDto.FechaRegistro;
                dr["FechaModifica"] = DateTime.Now;
                dr["Estado"] = tesisDto.Estado;
                dr["MotivoModificar"] = tesisDto.MotivoModificar;
                dr["RubroStr"] = tesisDto.RubroStr;
                dr["BD"] = tesisDto.BaseOrigen;
                dr["BDGrupo"] = tesisDto.BdGrupo;
                dr["IdAbs"] = uniqueId;
                dr["NotaPieR"] = tesisDto.NotasRubro;
                dr["NotaPieT"] = tesisDto.NotasTexto;
                dr["NotaPieP"] = tesisDto.NotasPrecedentes;
                dr["NotaGaceta"] = tesisDto.NotasGaceta;
                dr["ModificadoNotas"] = (tesisDto.IsNotasModif) ? 1 : 0;
                dr["NotaPublica"] = tesisDto.NotaPublica;

                dataSet.Tables["Tesis"].Rows.Add(dr);

                dataAdapter.InsertCommand = connectionBitacoraSql.CreateCommand();
                dataAdapter.InsertCommand.CommandText = "INSERT INTO Tesis(Parte,Consec,IUS,Rubro,Texto,Precedentes,Notas,Epoca,Sala,Fuente," +
                                                        "Volumen,Tesis,Pagina,TA_TJ,Materia1,Materia2,Materia3,Materia4,Materia5," +
                                                        "IdGenealogia,VolOrden,ConsecIndx,IdTCC,InfAnexos,LocAbr,NumLetra,ConsecLetra,Instancia,ConsecInst," +
                                                        "TpoTesis,TpoAsunto1,TpoAsunto2,TpoAsunto3,TpoAsunto4,TpoAsunto5,TpoPonente1,TpoPonente2,TpoPonente3," +
                                                        "TpoPonente4,TpoPonente5,TpoPon1,TpoPon2,TpoPon3,TpoPon4,TpoPon5,OrdenTesis,OrdenInstancia,OrdenRubro," +
                                                        "IdProg,ExistenTemas,IdClasif10,IdSubVolumen,AnioPublica,MesPublica,SemanaPublica,FechaRegistro," +
                                                        "FechaModifica,Estado,MotivoModificar,RubroStr,BD,BDGrupo,IdAbs,NotaPieR,NotaPieT,NotaPieP,NotaGaceta,Modificadonotas,NotaPublica)" +
                                                        " VALUES(@Parte,@Consec,@IUS,@Rubro,@Texto,@Precedentes,@Notas,@Epoca,@Sala,@Fuente," +
                                                        "@Volumen,@Tesis,@Pagina,@TA_TJ,@Materia1,@Materia2,@Materia3,@Materia4,@Materia5," +
                                                        "@IdGenealogia,@VolOrden,@ConsecIndx,@IdTCC,@InfAnexos,@LocAbr,@NumLetra,@ConsecLetra,@Instancia,@ConsecInst," +
                                                        "@TpoTesis,@TpoAsunto1,@TpoAsunto2,@TpoAsunto3,@TpoAsunto4,@TpoAsunto5,@TpoPonente1,@TpoPonente2,@TpoPonente3," +
                                                        "@TpoPonente4,@TpoPonente5,@TpoPon1,@TpoPon2,@TpoPon3,@TpoPon4,@TpoPon5,@OrdenTesis,@OrdenInstancia,@OrdenRubro," +
                                                        "@IdProg,@ExistenTemas,@IdClasif10,@IdSubVolumen,@AnioPublica,@MesPublica,@SemanaPublica,@FechaRegistro," +
                                                        "@FechaModifica,@Estado,@MotivoModificar,@RubroStr,@BD,@BDGrupo,@IdAbs,@NotaPieR,@NotaPieT,@NotaPieP,@NotaGaceta,@Modificadonotas,@NotaPublica)";

                dataAdapter.InsertCommand.Parameters.Add("@Parte", SqlDbType.Int, 0, "Parte");
                dataAdapter.InsertCommand.Parameters.Add("@Consec", SqlDbType.Int, 0, "Consec");
                dataAdapter.InsertCommand.Parameters.Add("@IUS", SqlDbType.BigInt, 0, "IUS");
                dataAdapter.InsertCommand.Parameters.Add("@Rubro", SqlDbType.Text, 0, "Rubro");
                dataAdapter.InsertCommand.Parameters.Add("@Texto", SqlDbType.Text, 0, "Texto");
                dataAdapter.InsertCommand.Parameters.Add("@Precedentes", SqlDbType.Text, 0, "Precedentes");
                dataAdapter.InsertCommand.Parameters.Add("@Notas", SqlDbType.Text, 0, "Notas");
                dataAdapter.InsertCommand.Parameters.Add("@Epoca", SqlDbType.Int, 0, "Epoca");
                dataAdapter.InsertCommand.Parameters.Add("@Sala", SqlDbType.Int, 0, "Sala");
                dataAdapter.InsertCommand.Parameters.Add("@Fuente", SqlDbType.Int, 0, "Fuente");
                dataAdapter.InsertCommand.Parameters.Add("@Volumen", SqlDbType.Int, 0, "Volumen");
                dataAdapter.InsertCommand.Parameters.Add("@Tesis", SqlDbType.VarChar, 0, "Tesis");
                dataAdapter.InsertCommand.Parameters.Add("@Pagina", SqlDbType.VarChar, 0, "Pagina");
                dataAdapter.InsertCommand.Parameters.Add("@TA_TJ", SqlDbType.Int, 0, "TA_TJ");
                dataAdapter.InsertCommand.Parameters.Add("@Materia1", SqlDbType.Int, 0, "Materia1");
                dataAdapter.InsertCommand.Parameters.Add("@Materia2", SqlDbType.Int, 0, "Materia2");
                dataAdapter.InsertCommand.Parameters.Add("@Materia3", SqlDbType.Int, 0, "Materia3");
                dataAdapter.InsertCommand.Parameters.Add("@Materia4", SqlDbType.Int, 0, "Materia4");
                dataAdapter.InsertCommand.Parameters.Add("@Materia5", SqlDbType.Int, 0, "Materia5");
                dataAdapter.InsertCommand.Parameters.Add("@IdGenealogia", SqlDbType.Int, 0, "IdGenealogia");
                dataAdapter.InsertCommand.Parameters.Add("@VolOrden", SqlDbType.Int, 0, "VolOrden");
                dataAdapter.InsertCommand.Parameters.Add("@ConsecIndx", SqlDbType.Int, 0, "ConsecIndx");
                dataAdapter.InsertCommand.Parameters.Add("@IdTCC", SqlDbType.VarChar, 0, "IdTCC");
                dataAdapter.InsertCommand.Parameters.Add("@InfAnexos", SqlDbType.Int, 0, "InfAnexos");
                dataAdapter.InsertCommand.Parameters.Add("@LocAbr", SqlDbType.NVarChar, 0, "LocAbr");
                dataAdapter.InsertCommand.Parameters.Add("@NumLetra", SqlDbType.Int, 0, "NumLetra");
                dataAdapter.InsertCommand.Parameters.Add("@ConsecLetra", SqlDbType.Int, 0, "ConsecLetra");
                dataAdapter.InsertCommand.Parameters.Add("@Instancia", SqlDbType.Int, 0, "Instancia");
                dataAdapter.InsertCommand.Parameters.Add("@ConsecInst", SqlDbType.Int, 0, "ConsecInst");
                dataAdapter.InsertCommand.Parameters.Add("@TpoTesis", SqlDbType.TinyInt, 0, "TpoTesis");
                dataAdapter.InsertCommand.Parameters.Add("@TpoAsunto1", SqlDbType.Int, 0, "TpoAsunto1");
                dataAdapter.InsertCommand.Parameters.Add("@TpoAsunto2", SqlDbType.Int, 0, "TpoAsunto2");
                dataAdapter.InsertCommand.Parameters.Add("@TpoAsunto3", SqlDbType.Int, 0, "TpoAsunto3");
                dataAdapter.InsertCommand.Parameters.Add("@TpoAsunto4", SqlDbType.Int, 0, "TpoAsunto4");
                dataAdapter.InsertCommand.Parameters.Add("@TpoAsunto5", SqlDbType.Int, 0, "TpoAsunto5");
                dataAdapter.InsertCommand.Parameters.Add("@TpoPonente1", SqlDbType.Int, 0, "TpoPonente1");
                dataAdapter.InsertCommand.Parameters.Add("@TpoPonente2", SqlDbType.Int, 0, "TpoPonente2");
                dataAdapter.InsertCommand.Parameters.Add("@TpoPonente3", SqlDbType.Int, 0, "TpoPonente3");
                dataAdapter.InsertCommand.Parameters.Add("@TpoPonente4", SqlDbType.Int, 0, "TpoPonente4");
                dataAdapter.InsertCommand.Parameters.Add("@TpoPonente5", SqlDbType.Int, 0, "TpoPonente5");
                dataAdapter.InsertCommand.Parameters.Add("@TpoPon1", SqlDbType.Int, 0, "TpoPon1");
                dataAdapter.InsertCommand.Parameters.Add("@TpoPon2", SqlDbType.Int, 0, "TpoPon2");
                dataAdapter.InsertCommand.Parameters.Add("@TpoPon3", SqlDbType.Int, 0, "TpoPon3");
                dataAdapter.InsertCommand.Parameters.Add("@TpoPon4", SqlDbType.Int, 0, "TpoPon4");
                dataAdapter.InsertCommand.Parameters.Add("@TpoPon5", SqlDbType.Int, 0, "TpoPon5");
                dataAdapter.InsertCommand.Parameters.Add("@OrdenTesis", SqlDbType.Int, 0, "OrdenTesis");
                dataAdapter.InsertCommand.Parameters.Add("@OrdenInstancia", SqlDbType.Int, 0, "OrdenInstancia");
                dataAdapter.InsertCommand.Parameters.Add("@OrdenRubro", SqlDbType.Int, 0, "OrdenRubro");
                dataAdapter.InsertCommand.Parameters.Add("@idProg", SqlDbType.NVarChar, 0, "idProg");
                dataAdapter.InsertCommand.Parameters.Add("@ExistenTemas", SqlDbType.TinyInt, 0, "ExistenTemas");
                dataAdapter.InsertCommand.Parameters.Add("@idClasif10", SqlDbType.SmallInt, 0, "idClasif10");
                dataAdapter.InsertCommand.Parameters.Add("@idSubVolumen", SqlDbType.SmallInt, 0, "idSubVolumen");
                dataAdapter.InsertCommand.Parameters.Add("@AnioPublica", SqlDbType.Int, 0, "AnioPublica");
                dataAdapter.InsertCommand.Parameters.Add("@MesPublica", SqlDbType.Int, 0, "MesPublica");
                dataAdapter.InsertCommand.Parameters.Add("@SemanaPublica", SqlDbType.Int, 0, "SemanaPublica");
                dataAdapter.InsertCommand.Parameters.Add("@FechaRegistro", SqlDbType.DateTime, 0, "FechaRegistro");
                dataAdapter.InsertCommand.Parameters.Add("@FechaModifica", SqlDbType.DateTime, 0, "FechaModifica");
                dataAdapter.InsertCommand.Parameters.Add("@Estado", SqlDbType.TinyInt, 0, "Estado");
                dataAdapter.InsertCommand.Parameters.Add("@MotivoModificar", SqlDbType.Int, 0, "MotivoModificar");
                dataAdapter.InsertCommand.Parameters.Add("@RubroStr", SqlDbType.NVarChar, 0, "RubroStr");
                dataAdapter.InsertCommand.Parameters.Add("@BD", SqlDbType.NVarChar, 0, "BD");
                dataAdapter.InsertCommand.Parameters.Add("@BDGrupo", SqlDbType.TinyInt, 0, "BDGrupo");
                dataAdapter.InsertCommand.Parameters.Add("@IdAbs", SqlDbType.UniqueIdentifier, 0, "IdAbs");
                dataAdapter.InsertCommand.Parameters.Add("@NotaPieR", SqlDbType.NVarChar, 0, "NotaPieR");
                dataAdapter.InsertCommand.Parameters.Add("@NotaPieT", SqlDbType.NVarChar, 0, "NotaPieT");
                dataAdapter.InsertCommand.Parameters.Add("@NotaPieP", SqlDbType.NVarChar, 0, "NotaPieP");
                dataAdapter.InsertCommand.Parameters.Add("@NotaGaceta", SqlDbType.NVarChar, 0, "NotaGaceta");
                dataAdapter.InsertCommand.Parameters.Add("@ModificadoNotas", SqlDbType.Int, 0, "ModificadoNotas");
                dataAdapter.InsertCommand.Parameters.Add("@NotaPublica", SqlDbType.NVarChar, 0, "NotaPublica");

                dataAdapter.Update(dataSet, "Tesis");

                dataSet.Dispose();
                dataAdapter.Dispose();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, tesisDto.Ius);
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, tesisDto.Ius);
            }
            finally
            {
                connectionBitacoraSql.Close();
            }
        }
    }
}
