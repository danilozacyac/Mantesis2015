using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using ClasificacionInformes.Dto;
using MantesisVerIusCommonObjects.DataAccess;
using ScjnUtilities;

namespace ClasificacionInformes.Models
{
    public class TesisInformeModel
    {

        #region Operaciones CRUD

        /// <summary>
        /// Establece la clasificación de una tesis de acuerdo al apartado donde aparecerá en el disco
        /// </summary>
        /// <param name="tesisDto"></param>
        public void SetRelacion(TesisInforme tesisDto)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["salasConnection"].ConnectionString);
            SqlDataAdapter dataAdapter;

            DataSet dataSet = new DataSet();
            DataRow dr;

            string sqlCadena = "SELECT * FROM salasTesisClasif WHERE ius = 0";

            try
            {
                dataAdapter = new SqlDataAdapter();
                dataAdapter.SelectCommand = new SqlCommand(sqlCadena, connection);

                dataAdapter.Fill(dataSet, "Tesis");

                dr = dataSet.Tables["Tesis"].NewRow();
                dr["IUS"] = tesisDto.Ius;
                dr["idClasif"] = tesisDto.IdClasif;
                dr["Volumen"] = tesisDto.Volumen;
                dr["Tesis"] = tesisDto.Tesis;
                dr["Pagina"] = tesisDto.Pagina;
                dr["Sala"] = tesisDto.Sala;
                dr["Fuente"] = tesisDto.Fuente;
                dr["idProyecto"] = tesisDto.IdProyecto;
                dr["Rubro"] = tesisDto.Rubro;
                dr["Texto"] = tesisDto.Texto;
                dr["Precedentes"] = tesisDto.Precedentes;
                dr["Epoca"] = tesisDto.Epoca;
                dr["TATJ"] = tesisDto.TaTj;
                dr["OrdenImpresion"] = tesisDto.NumericOrder;

                dataSet.Tables["Tesis"].Rows.Add(dr);

                dataAdapter.InsertCommand = connection.CreateCommand();
                dataAdapter.InsertCommand.CommandText = "INSERT INTO salasTesisClasif  (ius,idClasif,volumen,tesis,pagina,sala,fuente,idproyecto,rubro,texto,precedentes,epoca,tatj,ordenImpresion)" +
                                                        " VALUES(@ius,@idClasif,@volumen,@tesis,@pagina,@sala,@fuente,@idproyecto,@rubro,@texto,@precedentes,@epoca,@tatj,@ordenImpresion)";

                dataAdapter.InsertCommand.Parameters.Add("@ius", SqlDbType.BigInt, 0, "ius");
                dataAdapter.InsertCommand.Parameters.Add("@idClasif", SqlDbType.Int, 0, "idClasif");
                dataAdapter.InsertCommand.Parameters.Add("@volumen", SqlDbType.Int, 0, "volumen");
                dataAdapter.InsertCommand.Parameters.Add("@tesis", SqlDbType.VarChar, 0, "tesis");
                dataAdapter.InsertCommand.Parameters.Add("@pagina", SqlDbType.NVarChar, 0, "pagina");
                dataAdapter.InsertCommand.Parameters.Add("@sala", SqlDbType.SmallInt, 0, "sala");
                dataAdapter.InsertCommand.Parameters.Add("@fuente", SqlDbType.TinyInt, 0, "fuente");
                dataAdapter.InsertCommand.Parameters.Add("@idproyecto", SqlDbType.Int, 0, "idproyecto");
                dataAdapter.InsertCommand.Parameters.Add("@rubro", SqlDbType.Text, 0, "rubro");
                dataAdapter.InsertCommand.Parameters.Add("@texto", SqlDbType.Text, 0, "texto");
                dataAdapter.InsertCommand.Parameters.Add("@precedentes", SqlDbType.Text, 0, "precedentes");
                dataAdapter.InsertCommand.Parameters.Add("@epoca", SqlDbType.SmallInt, 0, "epoca");
                dataAdapter.InsertCommand.Parameters.Add("@tatj", SqlDbType.TinyInt, 0, "tatj");
                dataAdapter.InsertCommand.Parameters.Add("@ordenImpresion", SqlDbType.BigInt, 0, "ordenImpresion");

                dataAdapter.Update(dataSet, "Tesis");

                dataSet.Dispose();
                dataAdapter.Dispose();

                BitacoraInformeModel.SetNewBitacoraEntry(tesisDto.Ius, 1, 0, tesisDto.IdClasif);
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
        }

        public void DeleteTesis(TesisInforme tesis)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["salasConnection"].ConnectionString);
            SqlCommand cmd;

            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();
                cmd.CommandText = "DELETE FROM salasTesisClasif WHERE IUS = " + tesis.Ius;
                cmd.ExecuteNonQuery();

                BitacoraInformeModel.SetNewBitacoraEntry(tesis.Ius, 2, tesis.IdClasif, 0);
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,DeleteTesis", "InformeSalas");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,DeleteTesis", "InformeSalas");
            }
            finally
            {
                connection.Close();
            }
        }

        public void UpdateRelacion(TesisInforme tesis)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["salasConnection"].ConnectionString);
            SqlCommand cmd;

            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();
                cmd.CommandText = "UPDATE salasTesisClasif SET ius = " + tesis.Ius + ",idClasif = " +
                                  tesis.IdClasif + " WHERE IUS = " + tesis.Ius;
                cmd.ExecuteNonQuery();

                BitacoraInformeModel.SetNewBitacoraEntry(tesis.Ius, 3, tesis.IdClasifAnterior, tesis.IdClasif);
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UpdateRelacion", "InformeSalas");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UpdateRelacion", "InformeSalas");
            }
            finally
            {
                connection.Close();
            }
        }

        public void UpdateRelacion(List<TesisInforme> listaTesis, int nuevaClasif)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["salasConnection"].ConnectionString);
            SqlCommand cmd;

            cmd = connection.CreateCommand();
            cmd.Connection = connection;

            try
            {
                connection.Open();

                foreach (TesisInforme tesis in listaTesis)
                {
                    cmd.CommandText = "UPDATE salasTesisClasif SET idClasif = " + nuevaClasif + " WHERE IUS = " + tesis.Ius;
                    cmd.ExecuteNonQuery();

                    BitacoraInformeModel.SetNewBitacoraEntry(tesis.Ius, 3, tesis.IdClasifAnterior, tesis.IdClasif);
                }
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UpdateRelacion", "InformeSalas");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,UpdateRelacion", "InformeSalas");
            }
            finally
            {
                connection.Close();
            }
        }



        #endregion


        /// <summary>
        /// Devuelve las tesis del mes que se esta trabajando que ya fueron clasificadas
        /// </summary>
        /// <param name="orden">Parámetro por el cual se ordenara la consulta</param>
        /// <param name="idClasif">Seccion de clasificacion</param>
        /// <returns></returns>
        public List<TesisInforme> GetTesisInformeSalas(int orden, int idClasif)
        {
            List<TesisInforme> listaTesis = new List<TesisInforme>();

            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["salasConnection"].ConnectionString);
            SqlCommand cmd;
            SqlDataReader dataReader;

            try
            {
                connection.Open();
                //string sqlCadena = "SELECT * FROM salasTesisClasif WHERE Volumen = " + ClasifVar.volumen + " AND idProyecto = " + ClasifVar.idProyecto + " and idClasif = " + idClasif + " ORDER BY ";
                string sqlCadena = "SELECT * FROM salasTesisClasif WHERE (" + ClasifVar.CurrentProject.CondicionVolumen + ") AND idProyecto = " + ClasifVar.IdProyecto + " and idClasif = " + idClasif + " ORDER BY ";

                switch (orden)
                {
                    case 0:
                        sqlCadena += " Volumen ASC,Pagina ASC";
                        break;
                    case 1:
                        sqlCadena += " IUS";
                        break;
                    case 2:
                        sqlCadena += " TATJ desc,OrdenImpresion";
                        break;
                    case 3:
                        sqlCadena += " Rubro";
                        break;
                    case 4:
                        sqlCadena += " OrdenImpresion";
                        break;
                }

                cmd = new SqlCommand(sqlCadena, connection);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    TesisInforme tesis = new TesisInforme();
                    tesis.Ius = Convert.ToInt32(dataReader["IUS"].ToString());
                    tesis.IdClasif = Convert.ToInt32(dataReader["IdClasif"].ToString());
                    tesis.Volumen = dataReader["Volumen"].ToString();
                    tesis.Tesis = dataReader["Tesis"].ToString();
                    tesis.Pagina = dataReader["Pagina"].ToString();
                    tesis.Sala = Convert.ToInt32(dataReader["Sala"].ToString());
                    tesis.IdProyecto = Convert.ToInt32(dataReader["IdProyecto"].ToString());
                    tesis.Rubro = dataReader["Rubro"].ToString();
                    tesis.Texto = dataReader["Texto"].ToString();
                    tesis.TaTj = Convert.ToInt16(dataReader["TATJ"]);
                    tesis.Precedentes = dataReader["Precedentes"].ToString();
                    tesis.Fuente = Convert.ToInt32(dataReader["Fuente"]);

                    listaTesis.Add(tesis);
                }
                dataReader.Close();
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

            return listaTesis;
        }

        /// <summary>
        /// Trae todas las tesis de la Sala seleccionada de acuerdo al volumen seleccionado en Mantesis,
        /// excepto aquella que hayan sido clasificadas previamente ordenadas de una forma específica
        /// </summary>
        /// <param name="sala"></param>
        /// <returns></returns>
        public List<TesisInforme> GetTesisSalasVolumen(int sala, int orden)
        {
            List<TesisInforme> listaTesis = new List<TesisInforme>();

            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["BaseIUS"].ConnectionString);
            SqlCommand cmd;
            SqlDataReader reader;

            try
            {
                connection.Open();

                string sqlCadena = "SELECT ius,rubro,texto,prec,tesis,pagina,epoca,[ta_tj],fuente,volumen " + 
                                   "FROM Tesis WHERE " + ClasifVar.CurrentProject.CondicionProyecto + "   ORDER By ";

                switch (orden)
                {
                    case 0:
                        sqlCadena += " ConsecIndx";
                        break;
                    case 1:
                        sqlCadena += " IUS";
                        break;
                    case 2:
                        sqlCadena += " [ta_tj] desc,Tesis";
                        break;
                    case 3:
                        sqlCadena += " Rubro";
                        break;
                }

                cmd = new SqlCommand(sqlCadena, connection);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    TesisInforme tesis = new TesisInforme();
                    tesis.Ius = Convert.ToInt32(reader["IUS"]);
                    tesis.Tesis = reader["Tesis"].ToString();
                    tesis.Pagina = (reader["pagina"] == null) ? " " : reader["PAGINA"].ToString();// as string? ?? " ";
                    tesis.Rubro = reader["Rubro"].ToString();
                    tesis.Texto = reader["Texto"].ToString();
                    tesis.Precedentes = reader["Prec"].ToString();
                    tesis.Epoca = reader["Epoca"].ToString();
                    tesis.TaTj = Convert.ToInt32(reader["TA_TJ"]);
                    tesis.Fuente = Convert.ToInt32(reader["Fuente"]);
                    tesis.NumericOrder = this.GetNumeroDeTesisOrden(tesis.Tesis, tesis.TaTj);
                    tesis.Sala = ClasifVar.CurrentProject.Sala;
                    tesis.Volumen = reader["Volumen"].ToString();

                    if (!GetTesisInformeSalasPorRegistro(tesis.Ius))
                        listaTesis.Add(tesis);
                }
                reader.Close();

                this.GetTesisInformeDiciembre(orden, listaTesis);
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

            return listaTesis;
        }




        //public List<TesisDTO> GetTesisSalasVolumen(int sala)
        //{
        //    List<TesisDTO> listaTesis = new List<TesisDTO>();

        //    OleDbConnection sqlConne = DbConnDac.GetConnectionDecimaAccess();
        //    OleDbCommand cmd;
        //    OleDbDataReader dataReader;

        //    try
        //    {
        //        sqlConne.Open();

        //        string sqlCadena = "SELECT * FROM Tesis WHERE Sala = @Sala and epoca = 100 and volumen >= 8804 and volumen <= 8816 ";

                
        //        cmd = new OleDbCommand(sqlCadena, sqlConne);
        //        cmd.Parameters.AddWithValue("@Sala", sala);
        //        dataReader = cmd.ExecuteReader();

        //        while (dataReader.Read())
        //        {
        //            TesisDTO tesis = new TesisDTO();
        //            tesis.Ius = Convert.ToInt32(dataReader["IUS4"]);
        //            tesis.Tesis = dataReader["Tesis"].ToString();
        //            tesis.Pagina = (dataReader["pagina"] == null) ? " " : dataReader["PAGINA"].ToString();// as string? ?? " ";
        //            tesis.Rubro = dataReader["Rubro"].ToString();
        //            tesis.Texto = dataReader["Texto"].ToString();
        //            tesis.Precedentes = dataReader["Precedentes"].ToString();
        //            tesis.Epoca = GeneralFunctions.To<int>(dataReader["epoca"], 0);
        //            tesis.Tatj = GeneralFunctions.To<int>(dataReader["TATJ"], 0);
        //            tesis.Fuente = GeneralFunctions.To<int>(dataReader["fuente"], 0);
        //            tesis.NumericOrder = this.GetNumeroDeTesisOrden(tesis.Tesis, tesis.Tatj);
        //            tesis.Sala = ClasifVar.CurrentProject.Sala;
        //            tesis.Volumen = GeneralFunctions.To<int>(dataReader["volumen"], 0);

        //            if (!GetTesisInformeSalasPorRegistro(tesis.Ius))
        //                listaTesis.Add(tesis);
        //        }
        //        dataReader.Close();

        //        this.GetTesisInformeDiciembre(sala, listaTesis);
        //    }
        //    catch (OleDbException sql)
        //    {
        //        MessageBox.Show("Error ({0}) : {1}" + sql.Source + sql.Message, "Error Interno --- " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        //    }
        //    finally
        //    {
        //        sqlConne.Close();
        //    }

        //    return listaTesis;
        //}


        /// <summary>
        /// Filtra aquellas tesis que ya fueron 
        /// clasificadas con anterioridad para que no sean mostradas en el listado del
        /// filtro de tesis denominado "sin clasificar"
        /// Realiza la busqueda de la tesis en cestion y nos dice si ya fue clasificada
        /// </summary>
        /// <returns></returns>
        private bool GetTesisInformeSalasPorRegistro(long ius)
        {
            bool existe = false;

            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["salasConnection"].ConnectionString);
            SqlCommand cmd;
            SqlDataReader reader;

            try
            {
                connection.Open();
                string sqlCadena = "SELECT * FROM salasTesisClasif WHERE idProyecto = " + ClasifVar.IdProyecto + " AND ius = " + ius + " ORDER By Pagina";
                cmd = new SqlCommand(sqlCadena, connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    existe = true;
                }
                reader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetTesisInformeSalasPorRegistro", "InformeSalas");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetTesisInformeSalasPorRegistro", "InformeSalas");
            }
            finally
            {
                connection.Close();
            }

            return existe;
        }


        private List<TesisInforme> GetTesisInformeDiciembre(int sala, List<TesisInforme> listaTesis)
        {
            OleDbConnection connection = DbConnDac.GetConnectionInformes();
            OleDbCommand cmd;
            OleDbDataReader dataReader;

            try
            {
                connection.Open();

                string sqlCadena = "SELECT * FROM Tesis WHERE Sala = @Sala";

                cmd = new OleDbCommand(sqlCadena, connection);
                cmd.Parameters.AddWithValue("@Sala", sala);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    TesisInforme tesis = new TesisInforme();
                    tesis.Ius = Convert.ToInt32(dataReader["IUS"]);
                    tesis.Tesis = dataReader["Tesis"].ToString();
                    tesis.Pagina = (dataReader["pagina"] == null) ? " " : dataReader["PAGINA"].ToString();// as string? ?? " ";
                    tesis.Rubro = dataReader["Rubro"].ToString();
                    tesis.Texto = dataReader["Texto"].ToString();
                    tesis.Precedentes = dataReader["Precedentes"].ToString();
                    tesis.Epoca = dataReader["Epoca"].ToString();
                    tesis.TaTj = Convert.ToInt32(dataReader["TA_TJ"]);
                    tesis.Fuente = Convert.ToInt32(dataReader["fuente"]);
                    //tesis.NumericOrder = this.GetNumeroDeTesisOrden(tesis.Tesis, tesis.Tatj);
                    tesis.Sala = ClasifVar.CurrentProject.Sala;
                    tesis.Volumen = dataReader["volumen"].ToString();

                    if (!GetTesisInformeSalasPorRegistro(tesis.Ius))
                        listaTesis.Add(tesis);
                }
                dataReader.Close();
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

            return listaTesis;
        }
















        #region TesisUtilidades

        private long GetNumeroDeTesisOrden(String tesis, int tatj)
        {
            long ordenTesis = 0;

            try
            {
                if (tatj == 0)
                {
                    tesis = tesis.Substring(tesis.IndexOf(' ')).Replace("(9a.)", "").Replace("(10a.)", "").Replace("(10a).", "").Trim();
                    String[] orden = tesis.Split('/');
                    ordenTesis = Convert.ToInt64(orden[1] + StringUtilities.RomanosADecimal(orden[0]));
                }
                else
                {
                    tesis = tesis.Substring(tesis.IndexOf('J') + 2);
                    tesis = tesis.Replace("(9a.)", "").Replace("(10a.)", "").Replace("(10a).", "").Trim();

                    String[] orden = tesis.Split('/');

                    ordenTesis = Convert.ToInt64(orden[1] + StringUtilities.SetCeros( orden[0]));
                }
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetNumeroDeTesisOrden", "InformeSalas");
            }

            return ordenTesis;
        }

        #endregion



        #region Reportes

        /// <summary>
        /// Devuelve las tesis que dentro del periodo comprendido por el proyecto no se han clasificado
        /// Se utiliza para la impresión, además de el panel de actualizar varias
        /// </summary>
        /// <returns></returns>
        public List<TesisInforme> GetTesisSinCalsificarImpresion()
        {
            List<TesisInforme> listaTesis = new List<TesisInforme>();

            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["salasConnection"].ConnectionString);
            SqlCommand cmd;
            SqlDataReader dataReader;

            try
            {
                connection.Open();

                string sqlCadena = "SELECT ius,rubro,texto,prec,tesis,pagina,epoca,[ta/tj],fuente,volumen FROM Tesis WHERE " + ClasifVar.CurrentProject.CondicionProyecto;//  ORDER By";

                cmd = new SqlCommand(sqlCadena, connection);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    TesisInforme tesis = new TesisInforme();
                    tesis.Ius = Convert.ToInt32(dataReader["IUS"]);
                    tesis.Tesis = dataReader["Tesis"].ToString();
                    tesis.Pagina = dataReader["Pagina"].ToString();
                    tesis.Rubro = dataReader["Rubro"].ToString();
                    tesis.Texto = dataReader["Texto"].ToString();
                    tesis.Precedentes = dataReader["Prec"].ToString();
                    tesis.Epoca = dataReader["epoca"].ToString();
                    tesis.TaTj = Convert.ToInt16(dataReader["TA/TJ"]);
                    tesis.Fuente = Convert.ToInt32(dataReader["Fuente"]);
                    tesis.NumericOrder = this.GetNumeroDeTesisOrden(tesis.Tesis, tesis.TaTj);
                    tesis.Volumen = dataReader["volumen"].ToString();

                    if (!GetTesisInformeSalasPorRegistro(tesis.Ius))
                        listaTesis.Add(tesis);
                }
                dataReader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetTesisSinCalsificarImpresion", "InformeSalas");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetTesisSinCalsificarImpresion", "InformeSalas");
            }
            finally
            {
                connection.Close();
            }

            return listaTesis;
        }

        /// <summary>
        /// Regresa las tesis de cada clasificacion para que sean impresas en el reporte,
        /// las tesis ya van ordenadas de acuerdo al criterio solicitado, por lo que no 
        /// necesario volver a ordenar
        /// </summary>
        /// <param name="idClasif"></param>
        /// <param name="tpoImpresion"> 0 - Acumulada, 1 - Mes trabajado</param>
        /// <returns></returns>
        public List<TesisInforme> GetTesisInformeImpresion(int idClasif, int tpoImpresion)
        {
            List<TesisInforme> listaTesis = new List<TesisInforme>();

            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["salasConnection"].ConnectionString);
            SqlCommand cmd;
            SqlDataReader dataReader;

            try
            {
                connection.Open();
                string sqlCadena = "";

                if (tpoImpresion == 1)
                {
                    sqlCadena = "SELECT * FROM salasTesisClasif WHERE idProyecto = " + ClasifVar.IdProyecto + " and idClasif = " +
                                idClasif + " AND Volumen = " + ClasifVar.Volumen + " ORDER BY Epoca, OrdenImpresion";
                }
                else if (tpoImpresion == 0)
                {
                    sqlCadena = "SELECT * FROM salasTesisClasif WHERE idProyecto = " + ClasifVar.IdProyecto + " and idClasif = " +
                                idClasif + " AND " + ClasifVar.CurrentProject.CondicionProyecto + " ORDER BY Epoca, OrdenImpresion";
                }

                cmd = new SqlCommand(sqlCadena, connection);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    TesisInforme tesis = new TesisInforme();
                    tesis.Ius = Convert.ToInt32(dataReader["IUS"].ToString());
                    tesis.IdClasif = Convert.ToInt32(dataReader["IdClasif"].ToString());
                    tesis.Volumen = dataReader["Volumen"].ToString();
                    tesis.Tesis = dataReader["Tesis"].ToString();
                    tesis.Pagina = dataReader["Pagina"].ToString();
                    tesis.Sala = Convert.ToInt32(dataReader["Sala"].ToString());
                    tesis.IdProyecto = Convert.ToInt32(dataReader["IdProyecto"].ToString());
                    tesis.Rubro = dataReader["Rubro"].ToString();
                    tesis.Texto = dataReader["Texto"].ToString();
                    tesis.Precedentes = dataReader["Precedentes"].ToString();
                    tesis.TaTj = Convert.ToInt16(dataReader["TATJ"]);
                    tesis.Epoca = dataReader["Epoca"].ToString();
                    tesis.NumericOrder = this.GetNumeroDeTesisOrden(tesis.Tesis, tesis.TaTj);

                    listaTesis.Add(tesis);
                }
                dataReader.Close();
            }
            catch (SqlException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetTesisInformeImpresion", "InformeSalas");
            }
            catch (Exception ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                ErrorUtilities.SetNewErrorMessage(ex, methodName + " Exception,GetTesisInformeImpresion", "InformeSalas");
            }
            finally
            {
                connection.Close();
            }

            return listaTesis;
        }


        #endregion


    }
}
