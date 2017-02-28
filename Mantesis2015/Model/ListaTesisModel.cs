using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using MantesisCommonObjects.DataAccess;
using MantesisCommonObjects.Dto;
using MantesisCommonObjects.MantUtilities;
using ScjnUtilities;

namespace Mantesis2015.Model
{
    class ListaTesisModel
    {
        private List<AddTesis> listaTesis;

        //private DbConnection connectionEpocas;

        public ListaTesisModel()
        {
        }

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
                if (ValuesMant.Epoca == ConstMantesis.Apendice)
                {
                    if (filtro == 99 || filtro == -1)
                    {
                        sqlCadena = "SELECT ius,tesis,rubro,pagina,estado,ta_tj,volumen,epoca FROM Tesis WHERE Parte = " +
                                    ValuesMant.Parte + " ORDER BY pagina";
                    }
                    else
                    {
                        sqlCadena = "SELECT ius,tesis,rubro,pagina,estado,ta_tj,volumen,epoca FROM Tesis WHERE Parte = " +
                                    ValuesMant.Parte + " AND Volumen = " + ValuesMant.Volumen + " ORDER BY pagina";
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
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ListaTesisModel", "MantesisQuinta");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ListaTesisModel", "MantesisQuinta");
            }
            finally
            {
                reader.Close();
                connectionMantesisSql.Close();
            }

            return listaTesis;
        }



        public List<AddTesis> CargaTesisImportadas(int filtro)
        {
            OleDbConnection connection = new OleDbConnection(ConfigurationManager.ConnectionStrings["Base"].ConnectionString);
            OleDbCommand cmd;
           OleDbDataReader reader = null;

            listaTesis = new List<AddTesis>();
            long nId = 1;

            string sqlCadena = "";

            try
            {

                sqlCadena = "SELECT ius,tesis,rubro,pagina,estado,ta_tj,volumen,epoca FROM Tesis ORDER BY ConsecIndx";


                connection.Open();

                cmd = new OleDbCommand(sqlCadena, connection);
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

                connection.Close();

            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ListaTesisModel", "MantesisQuinta");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,ListaTesisModel", "MantesisQuinta");
            }
            finally
            {
                reader.Close();
                connection.Close();
            }

            return listaTesis;
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
            SqlConnection connIus= DbConnDac.GetConnectionIus();
            SqlDataReader readerReg;
            int nParte;
            int nEsValido = 1;

            connIus.Open();

            SqlCommand cmd = new SqlCommand("SELECT ius,parte FROM Tesis Where ius = @Ius", connIus);
            cmd.Parameters.AddWithValue("@Ius", ius);
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
    }
}