﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using MantesisVerIusCommonObjects.DataAccess;
using MantesisVerIusCommonObjects.Utilities;
using ScjnUtilities;
using UtilsMantesis;

namespace Mantesis2015.Model
{
    class ListaTesisModel
    {
        private List<AddTesis> listaTesis;
        //private DbConnection connectionEpocas;

        public ListaTesisModel()
        {
        }

        ///// <summary>
        ///// Obtiene la parte a la cual pertenecen las tesis del apéndice y Volumen seleccionados
        ///// </summary>
        ///// <param name="nMat">Materia Seleccionada</param>
        ///// <param name="nApendic">Apéndice seleccionado</param>
        ///// <returns></returns>
        //private int GetParte(int nMat, int nApendic)
        //{
        //    switch (nApendic)
        //    {
        //        case 1: //1997-1995
        //            switch (nMat)
        //            {
        //                case 1:
        //                    return 124; //constitucional
        //                case 2:
        //                    return 125; //penal
        //                case 3:
        //                    return 126; //Administrativa
        //                case 4:
        //                    return 127; //Civil
        //                case 5:
        //                    return 128; //Laboral
        //                case 6:
        //                    return 129; //Común
        //            }
        //            break;
        //        case 2: //1954-1988
        //            switch (nMat)
        //            {
        //                case 1:
        //                    return 132; //constitucional
        //                case 2:
        //                    return 133; //penal
        //                case 3:
        //                    return 134; //Administrativa
        //                case 4:
        //                    return 135; //Civil
        //                case 5:
        //                    return 136; //Laboral
        //                case 6:
        //                    return 137; //Común
        //            }
        //            break;
        //        case 3: //1917-2000
        //            switch (nMat)
        //            {
        //                case 1:
        //                    return 116; //constitucional
        //                case 2:
        //                    return 117; //penal
        //                case 3:
        //                    return 118; //Administrativa
        //                case 4:
        //                    return 119; //Civil
        //                case 5:
        //                    return 120; //Laboral
        //                case 6:
        //                    return 121; //Común
        //                case 51:
        //                    return 122; //Conflictos Competenciales
        //                case 10:
        //                    return 123; //Electoral
        //            }
        //            break;
        //        case 4: //Act. 2001
        //            switch (nMat)
        //            {
        //                case 1:
        //                    return 108; //constitucional
        //                case 2:
        //                    return 109; //penal
        //                case 3:
        //                    return 110; //Administrativa
        //                case 4:
        //                    return 111; //Civil
        //                case 5:
        //                    return 112; //Laboral
        //                case 6:
        //                    return 113; //Común
        //                case 10:
        //                    return 115; //Electoral
        //            }
        //            break;
        //        case 5: // Act. 2002
        //            switch (nMat)
        //            {
        //                case 1:
        //                    return 100; //constitucional
        //                case 2:
        //                    return 101; //penal
        //                case 3:
        //                    return 102; //Administrativa
        //                case 4:
        //                    return 103; //Civil
        //                case 5:
        //                    return 104; //Laboral
        //                case 6:
        //                    return 105; //Común
        //                case 51:
        //                    return 106; //Conflictos Competenciales
        //                case 10:
        //                    return 107; //Electoral
        //            }
        //            break;
        //        case 6: //1917-2011
        //            switch (nMat)
        //            {
        //                case 1:
        //                    return 140; //constitucional
        //                case 2:
        //                    return 141; //penal
        //                case 3:
        //                    return 142; //Administrativa
        //                case 4:
        //                    return 143; //Civil
        //                case 5:
        //                    return 144; //Laboral
        //                case 51:
        //                    return 146; //Conflictos Competenciales
        //                case 10:
        //                    return 147; //Electoral
        //                case 50:
        //                    return 145; //Procesal constitucional
        //            }
        //            break;
        //        default:
        //            return 0;
        //    }
        //    return 0;
        //}

        //public List<AddTesis> CargaTesis(int filtro)
        //{
        //    listaTesis = new List<AddTesis>();
        //    long nId = 1;

        //    string sqlCadena = "";

        //    if (ValuesMant.Epoca == ConstMant.Apendice)
        //    {
        //        connectionEpocas = DbConnDac.GetConnectEpocas(ValuesMant.Epoca, ValuesMant.ApendicNom, 1);
        //        if (ValuesMant.Volumen == 0)
        //        {
        //            sqlCadena = "SELECT ius4,tesis,rubro,pagina,estado,[ta/tj],Volumen,Epoca FROM Tesis ORDER BY pagina";
        //        }
        //        else
        //        {
        //            sqlCadena = "SELECT ius4,tesis,rubro,pagina,estado,[ta/tj],Volumen,Epoca FROM Tesis WHERE Parte =" + GetParte(ValuesMant.Volumen, ValuesMant.ApendicNom) + " ORDER BY pagina";
        //        }
        //    }
        //    else
        //    {
        //        if (ValuesMant.Epoca == ConstMant.Quinta)
        //        {
        //            connectionEpocas = DbConnDac.GetConnectEpocas(ValuesMant.Epoca, ValuesMant.Volumen, 1);
        //        }
        //        else
        //        {
        //            connectionEpocas = DbConnDac.GetConnectEpocas(ValuesMant.Epoca, 0, 1);
        //        }

        //        if (filtro == 99)
        //        {
        //            sqlCadena = "SELECT ius4,tesis,rubro,pagina,estado," +
        //                        ((ValuesMant.Epoca == ConstMant.Decima) ? "tatj" : "[ta/tj]") + ",Volumen,Epoca FROM Tesis WHERE Volumen = " +
        //                        ValuesMant.Volumen + " ORDER BY Epoca,Volumen,pagina";
        //        }
        //        else
        //        {
        //            sqlCadena = "SELECT ius4,tesis,rubro,pagina,estado," +
        //                        ((ValuesMant.Epoca == ConstMant.Decima) ? "tatj" : "[ta/tj]") + ",Volumen,Epoca FROM Tesis WHERE Volumen = " +
        //                        ValuesMant.Volumen + " AND (Materia1 = " + filtro + " OR Materia2 = " + filtro + " OR Materia3 = " +
        //                        filtro + " ) ORDER BY Epoca,Volumen,pagina";
        //        }
        //    }

        //    DataTableReader reader = this.GetDatosTabla(sqlCadena, connectionEpocas);

        //    while (reader.Read())
        //    {
        //        listaTesis.Add(MiscFunct.GetNewTesis(reader, nId++));
        //    }

        //    connectionEpocas.Close();

        //    listaTesis.Sort(delegate(AddTesis p1, AddTesis p2)
        //    {
        //        return p1.PaginaNum.CompareTo(p2.PaginaNum);
        //    });

        //    return listaTesis;
        //}

        public List<AddTesis> CargaTesisMantesisSql(int filtro)
        {
            SqlConnection connectionMantesisSql = DbConnDac.GetConnectionMantesisSql();
            SqlCommand cmd;
            SqlDataReader reader = null;

            listaTesis = new List<AddTesis>();
            long nId = 1;

            string sqlCadena = "";

            try
            {
                if (ValuesMant.Epoca == ConstMant.Apendice)
                {
                    if (ValuesMant.Volumen == 0)
                    {
                        sqlCadena = "SELECT ius,tesis,rubro,pagina,estado,ta_tj,volumen,epoca FROM Tesis ORDER BY pagina";
                    }
                    else
                    {
                        //sqlCadena = "SELECT ius,tesis,rubro,pagina,estado,ta_tj,volumen,epoca FROM Tesis WHERE Parte =" + 
                        //  GetParte(ValuesMant.Volumen, ValuesMant.ApendicNom) + " ORDER BY pagina";
                        sqlCadena = "SELECT ius,tesis,rubro,pagina,estado,ta_tj,volumen,epoca FROM Tesis WHERE Parte = " +
                            ValuesMant.Parte + " ORDER BY pagina";
                    }
                }
                else if (filtro == 99)
                {
                    if (ValuesMant.Epoca == 6)
                    {
                        sqlCadena = "SELECT ius,tesis,rubro,pagina,estado,ta_tj,volumen,epoca,materia1,materia2,materia3,materia4,materia5 FROM Tesis WHERE Volumen = " +
                                   ValuesMant.Volumen + " ORDER BY epoca,volumen,pagina";
                    }
                    else
                    {
                        sqlCadena = "SELECT ius,tesis,rubro,pagina,estado,ta_tj,volumen,epoca,materia1,materia2,materia3,materia4,materia5 FROM Tesis WHERE Volumen = " +
                                   ValuesMant.Volumen + " AND Epoca = " + ValuesMant.Epoca + " ORDER BY epoca,volumen,pagina";
                    }
                }
                else
                {
                    sqlCadena = "SELECT ius,tesis,rubro,pagina,estado,ta_tj,volumen,epoca FROM Tesis WHERE Volumen = " +
                                ValuesMant.Volumen + " AND (Materia1 = " + filtro + " OR Materia2 = " + filtro + " OR Materia3 = " +
                                filtro + " ) ORDER BY epoca,volumen,pagina";
                }

                connectionMantesisSql.Open();

                cmd = new SqlCommand(sqlCadena, connectionMantesisSql);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    listaTesis.Add(new AddTesis(
                        nId,
                        Convert.ToInt32(reader["ius"]),
                        reader["tesis"].ToString(),
                        reader["rubro"].ToString(),
                        reader["pagina"].ToString(),
                        MiscFunct.GetEstadoTesis(Convert.ToInt16(reader["estado"].ToString())),
                        Convert.ToInt16(reader["ta_tj"]),
                        Convert.ToInt32(reader["pagina"]),
                        Convert.ToInt32(reader["volumen"])));
                    nId++;
                }

                connectionMantesisSql.Close();

                listaTesis.Sort(delegate(AddTesis p1, AddTesis p2)
                {
                    return p1.PaginaNum.CompareTo(p2.PaginaNum);
                });

                connectionMantesisSql.Close();
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
                connectionMantesisSql.Close();
            }

            return listaTesis;
        }


        ///// <summary>
        ///// Elimina las tesis en las bases de datos de Access
        ///// </summary>
        ///// <param name="ius">Número de registro ius de la tesis a eliminar</param>
        ///// <returns></returns>
        //public int BajaRegistroIus(long ius)
        //{
        //    DbConnection connEpocas;
        //    DataTableReader readerReg;
        //    string sSql = "";
        //    int nEstado = 0;

        //    if (MantesisVerIusCommonObjects.Utilities.ValuesMant.Epoca == ConstMant.Apendice)
        //    {
        //        connEpocas = DbConnDac.GetConnectEpocas(MantesisVerIusCommonObjects.Utilities.ValuesMant.Epoca, MantesisVerIusCommonObjects.Utilities.ValuesMant.ApendicNom, 1);
        //    }
        //    else
        //    {
        //        connEpocas = DbConnDac.GetConnectEpocas(MantesisVerIusCommonObjects.Utilities.ValuesMant.Epoca, 0, 1);
        //    }
        //    connEpocas.Open();

        //    sSql = "SELECT ius4,estado FROM Tesis WHERE IUS4 =" + ius;
        //    readerReg = this.GetDatosRegistro(sSql, connEpocas);

        //    UnaTesisModel unaTesis = new UnaTesisModel();

        //    if (readerReg.Read())
        //    {
        //        nEstado = Convert.ToInt16(readerReg["Estado"].ToString());

        //        string idAbs = Guid.NewGuid().ToString();

        //        if (nEstado >= 4)
        //        {
        //            unaTesis.SalvarBitacora(ius, MantesisVerIusCommonObjects.Utilities.ValuesMant.MotivoBaja, 4, idAbs);
        //            readerReg.Close();
        //            connEpocas.Close();
        //            return nEstado;
        //        }

        //        if (nEstado < 4)
        //        {
        //            sSql = "UPDATE Tesis SET Estado = 6 WHERE ius4 =" + ius;
        //            DbCommand command = connEpocas.CreateCommand();
        //            command.CommandText = sSql;
        //            command.ExecuteNonQuery();

        //            unaTesis.SalvarBitacora(ius, MantesisVerIusCommonObjects.Utilities.ValuesMant.MotivoBaja, 4, idAbs);

        //            SalvarTemporal(ius, MantesisVerIusCommonObjects.Utilities.ValuesMant.MotivoBaja, MantesisVerIusCommonObjects.Utilities.ValuesMant.RegIusporBaja);
        //        }
        //    }

        //    readerReg.Close();
        //    connEpocas.Close();

        //    return nEstado;
        //}

        ///// <summary>
        ///// Elimina las tesis en las bases de datos de Access
        ///// </summary>
        ///// <param name="ius">Número de registro ius de la tesis a eliminar</param>
        ///// <returns></returns>
        //public int BajaRegistroIusMantesisSql(long ius)
        //{
        //    SqlConnection connectionMantesisSql = DbConnDac.GetConnectionMantesisSql();

        //    SqlDataReader reader = null;
        //    string sSql = "";
        //    int nEstado = 0;

        //    try
        //    {
        //        TesisDto tesisEliminar = new UnaTesisModel().CargaDatosTesisMantesisSql(ius);
        //        connectionMantesisSql.Open();

        //        sSql = "SELECT * FROM Tesis WHERE IUS4 =" + ius;
        //        //reader = this.GetDatosRegistro(sSql, connectionMantesisSql);

        //        UnaTesisModel unaTesis = new UnaTesisModel();

        //        if (reader.Read())
        //        {
        //            nEstado = Convert.ToInt16(reader["Estado"].ToString());

        //            string idAbs = Guid.NewGuid().ToString();

        //            if (nEstado >= 4)
        //            {
        //                unaTesis.SalvarBitacora(ius, MantesisVerIusCommonObjects.Utilities.ValuesMant.MotivoBaja, 4, idAbs);
        //                reader.Close();
        //                connectionMantesisSql.Close();
        //                return nEstado;
        //            }

        //            if (nEstado < 4)
        //            {
        //                sSql = "UPDATE Tesis SET Estado = 6 WHERE ius4 =" + ius;
        //                DbCommand command = connectionMantesisSql.CreateCommand();
        //                command.CommandText = sSql;
        //                command.ExecuteNonQuery();

        //                unaTesis.SalvarBitacora(ius, MantesisVerIusCommonObjects.Utilities.ValuesMant.MotivoBaja, 4, idAbs);

        //                SalvarTemporal(ius, MantesisVerIusCommonObjects.Utilities.ValuesMant.MotivoBaja, MantesisVerIusCommonObjects.Utilities.ValuesMant.RegIusporBaja);
        //            }
        //        }

        //        reader.Close();
        //        connectionMantesisSql.Close();
        //    }
        //    catch (SqlException sql)
        //    {
        //        MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
        //    }
        //    finally
        //    {
        //        reader.Close();
        //        connectionMantesisSql.Close();
        //    }

        //    return nEstado;
        //}

        ///// <summary>
        ///// Verifica el estado de la tesis en las bases de datos de Access
        ///// </summary>
        ///// <param name="nIus">Número de registro IUS de la tesis a eliminar</param>
        ///// <returns>Si regresa un 4 es que la tesis ya fue eliminada </returns>
        //public int BajaRegistroCheck(long nIus)
        //{
        //    DbConnection connEpocas;
        //    DataTableReader readerReg;
        //    string sSql = "";
        //    int nEstado = 0;

        //    if (MantesisVerIusCommonObjects.Utilities.ValuesMant.Epoca == ConstMant.Apendice)
        //    {
        //        connEpocas = DbConnDac.GetConnectEpocas(MantesisVerIusCommonObjects.Utilities.ValuesMant.Epoca, MantesisVerIusCommonObjects.Utilities.ValuesMant.ApendicNom, 1);
        //    }
        //    else
        //    {
        //        connEpocas = DbConnDac.GetConnectEpocas(MantesisVerIusCommonObjects.Utilities.ValuesMant.Epoca, 0, 1);
        //    }
        //    connEpocas.Open();

        //    sSql = "SELECT ius4,estado FROM Tesis WHERE IUS4 =" + nIus;
        //    readerReg = this.GetDatosRegistro(sSql, connEpocas);

        //    if (readerReg.Read())
        //    {
        //        nEstado = Convert.ToInt16(readerReg["Estado"].ToString());

        //        if (nEstado >= 4)
        //        {
        //            readerReg.Close();
        //            connEpocas.Close();
        //            return nEstado;
        //        }
        //    }

        //    readerReg.Close();
        //    connEpocas.Close();

        //    return nEstado;
        //}

        ///// <summary>
        ///// Verifica el estado de la tesis en la base de datos de Mantesis SQL
        ///// </summary>
        ///// <param name="nIus">Número de registro IUS de la tesis a eliminar</param>
        ///// <returns>Si regresa un 4 es que la tesis ya fue eliminada </returns>
        //public int BajaRegistroCheckMantesisSql(long nIus)
        //{
        //    SqlConnection connectionMantesisSql = DbConnDac.GetConnectionMantesisSql();
        //    SqlCommand cmd;
        //    SqlDataReader reader = null;

        //    string sSql = "";
        //    int nEstado = 0;

        //    try
        //    {

        //        connectionMantesisSql.Open();

        //        sSql = "SELECT IUS,estado FROM Tesis WHERE IUS = " + nIus;
        //        cmd = new SqlCommand(sSql, connectionMantesisSql);
        //        reader = cmd.ExecuteReader();

        //        if (reader.HasRows)
        //        {

        //            if (reader.Read())
        //            {
        //                nEstado = Convert.ToInt16(reader["Estado"].ToString());

        //                if (nEstado >= 4)
        //                {
        //                    reader.Close();
        //                    connectionMantesisSql.Close();
        //                    return nEstado;
        //                }
        //            }
        //            reader.Close();

        //        }
        //        else
        //        {
        //            sSql = "SELECT IUS FROM Tmp_TesisSup WHERE IUS = " + nIus;
        //            cmd = new SqlCommand(sSql, connectionMantesisSql);
        //            reader = cmd.ExecuteReader();

        //            if (reader.HasRows)
        //            {
        //                nEstado = 4;
        //                reader.Close();
        //                connectionMantesisSql.Close();
        //                return nEstado;

        //            }
        //        }

        //    }
        //    catch (SqlException sql)
        //    {
        //        MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, "Error Interno");
        //    }
        //    finally
        //    {
        //        reader.Close();
        //        connectionMantesisSql.Close();
        //    }

        //    return nEstado;
        //}


        private void SalvarTemporal(long ius, int motivoModif, long nRegistro)
        {
            DbConnection connTemp;

            connTemp = DbConnDac.GetConnectTemp();

            try
            {

                connTemp.Open();

                string sqlCadena = "INSERT INTO TesisModificadas(IUS4,TipoMotivo,Motivo,Fecha,Usuario,Hora,Registro)" +
                                   " VALUES(" + ius + ",4," + motivoModif + ",'" + DateTime.Now.ToString("yyyyMMdd") +
                                   "'," + AccesoUsuarioModel.Llave + ",'" + DateTime.Now.ToString("hh:mm:ss") + "'," + nRegistro + ")";

                DbCommand command = connTemp.CreateCommand();
                command.CommandText = sqlCadena;
                command.ExecuteNonQuery();
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
                connTemp.Close();
            }
        }

        //private void SalvarBitacora(long pIus, int motivoModif, int nTipo)
        //{
        //    DbConnection connmBitacora;
        //    try
        //    {
        //        connmBitacora = DbConnDac.GetConnectionmBitacora();
        //        connmBitacora.Open();
        //        string sGuid;

        //        sGuid = Guid.NewGuid().ToString();

        //        string sqlCadena = "INSERT INTO MantBitacora (Id, IdElemento, IdUsr,FechaStr,Fecha,IdMovimiento," +
        //                           "TipoDoc,TipoCat,Usuario,Comp,HoraStr,Observaciones,IdAbs)" +
        //                           " VALUES (" + nTipo + "," + pIus + "," + AccesoUsuarioModel.Llave + ",'" + DateTime.Now.ToString("yyyy/MM/dd") +
        //                           "','" + DateTime.Now.ToString("yyyy/MM/dd") + "'," + motivoModif + ",20,20,'" + MiscFunct.GetUserCurrent() + "','" + MiscFunct.GetPcCurrent() + "','" +
        //                           DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + "',4,'" + sGuid + "')";

        //        DbCommand command = connmBitacora.CreateCommand();
        //        command.CommandText = sqlCadena;
        //        command.ExecuteNonQuery();

        //        connmBitacora.Close();
        //    }
        //    catch (DbException exDb)
        //    {
        //        Console.WriteLine("DbException.GetType: {0}", exDb.GetType());
        //        Console.WriteLine("DbException.Source: {0}", exDb.Source);
        //        Console.WriteLine("DbException.ErrorCode: {0}", exDb.ErrorCode);
        //        Console.WriteLine("DbException.Message: {0}", exDb.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Exception.Message: {0}", ex.Message);
        //    }
        //}


        /// <summary>
        /// Verifica el estatus del número de IUS en cuestión, puede ser válido, eliminado o no existe
        /// </summary>
        /// <param name="ius"></param>
        /// <returns></returns>
        public int VerificaIus(long ius)
        {
            SqlConnection connIus;
            SqlDataReader readerReg;
            int nParte;
            int nEsValido = 1;
            string sqlCadena;

            connIus = DbConnDac.GetConnectionIus();
            connIus.Open();

            sqlCadena = "SELECT ius,parte FROM Tesis Where ius =" + ius;
            SqlCommand cmd = new SqlCommand(sqlCadena, connIus);
            readerReg = cmd.ExecuteReader();

            if (readerReg.Read())
            {
                nParte = Convert.ToInt16(readerReg["parte"].ToString());
                if (nParte == 99)
                {
                    // Esta eliminado
                    nEsValido = 4;
                }
                else
                {
                    // Es Valido
                    nEsValido = 0;
                }
            }
            else
            {
                // No existe
                nEsValido = 5;
            }

            return nEsValido;
        }

        //private DataTableReader GetDatosRegistro(string sqlString, DbConnection lConn)
        //{
        //    DataTableReader dtr = null;
        //    try
        //    {
        //        DataAdapter query = DbConnDac.GetDataAdapter(sqlString, lConn);
        //        dtr = DbConnDac.GetDatosReader(query);
        //    }
        //    catch (DbException ex)
        //    {
        //        string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //        MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
        //    }
        //    catch (Exception ex)
        //    {
        //        string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

        //        MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
        //    }

        //    return dtr;
        //}
    }
}
