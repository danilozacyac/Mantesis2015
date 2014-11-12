using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using Mantesis2015.Dto;
using Mantesis2015.Reportes;
using MantesisVerIusCommonObjects.DataAccess;
using MantesisVerIusCommonObjects.Utilities;
using ScjnUtilities;

namespace Mantesis2015.Model
{
    public class EstructuraModel
    {
        public bool RevisaTesisUsuario(long pIus, int pLlave)
        {
            bool bValida;

            DbConnection lConn = DbConnDac.GetConnectionIus();

            string sqlCadena = "SELECT ius,UsrSJF FROM Tesis_MatSGA WHERE IUS = " + pIus + " AND UsrSJF =" + pLlave;

            DataTableReader reader = this.GetDatosTabla(sqlCadena, lConn);

            if (reader.HasRows)
            {
                bValida = true;
            }
            else
            {
                bValida = false;
            }

            return bValida;
        }

        /// <summary>
        /// Reporte detallado
        /// </summary>
        /// <param name="padre"></param>
        /// <returns></returns>
        public List<Estructura> GetEstructura(long padre)
        {
            List<Estructura> estructura = new List<Estructura>();

            DbConnection lConn = DbConnDac.GetConnectionIus();

            string sqlCadena = "SELECT * FROM cMateriasSGA WHERE padre = " + padre + " ORDER BY Consec";
            try
            {
                DataTableReader reader = this.GetDatosTabla(sqlCadena, lConn);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        estructura.Add(new Estructura(Convert.ToInt64(reader["id"].ToString()), Convert.ToInt64(reader["nivel"].ToString()),
                            Convert.ToInt64(reader["padre"].ToString()), reader["descripcion"].ToString(), Convert.ToInt16(reader["historica"].ToString()),
                            Convert.ToInt16(reader["hoja"].ToString())));
                    }
                }

                reader.Close();
                lConn.Close();
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

            return estructura;
        }

        public void FillDictionary(char empieza)
        {
            DbConnection lConn = DbConnDac.GetConnectionIus();

            string sqlCadena = "select convert(varchar,id) id,descripcion,padre,nvlImpresion from cMateriasSGA WHERE id LIKE '" + empieza + "%'";

            try
            {
                DataTableReader reader = this.GetDatosTabla(sqlCadena, lConn);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        TablaSga.DicDescripciones.Add(Convert.ToInt32(reader["id"].ToString()), reader["Descripcion"].ToString());
                        TablaSga.DicPadres.Add(Convert.ToInt32(reader["id"].ToString()), Convert.ToInt32(reader["padre"].ToString()));
                        TablaSga.DicNiveles.Add(Convert.ToInt32(reader["id"].ToString()), Convert.ToInt32(reader["nvlImpresion"].ToString()));
                    }
                }

                reader.Close();
                lConn.Close();
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
        }

        /// <summary>
        /// Reporte SGA en tabla
        /// </summary>
        /// <param name="padre"></param>
        /// <returns></returns>
        public string GetDescripcionClasif(long idMateria)
        {
            string estructura = "";

            DbConnection lConn = DbConnDac.GetConnectionIus();

            try
            {
                string sqlCadena = "SELECT descripcion FROM cMateriasSGA WHERE id = " + idMateria;

                DataTableReader reader = this.GetDatosTabla(sqlCadena, lConn);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        estructura = reader["descripcion"].ToString();
                    }
                }

                reader.Close();
                lConn.Close();
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

            return estructura;
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

        public List<Estructura> GetRelaciones(long ius4)
        {
            List<Estructura> estructura = new List<Estructura>();

            DbConnection lConn = DbConnDac.GetConnectionIus();

            string sqlCadena = "SELECT C.Id,C.Descripcion,C.Nivel,C.Padre from cMateriasSGA C " +
                               "INNER JOIN Tesis_MatSGA T ON T.idMatSGA = C.id WHERE T.IUS = " + ius4;

            try
            {
                DataTableReader reader = this.GetDatosTabla(sqlCadena, lConn);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        List<Estructura> miHoja = CargaRelacion(Convert.ToInt64(reader["id"].ToString()), new List<Estructura>());
                        string descrip = "";

                        foreach (Estructura hoja in miHoja)
                        {
                            descrip += hoja.Descripcion + "; ";
                        }

                        estructura.Add(new Estructura(Convert.ToInt64(reader["id"].ToString()), descrip));
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
            return estructura;
        }

        public void InsertRelacion(long ius4, long iden, long tomo)
        {
            DateTime stamp = DateTime.Now;

            DbConnection lConn = DbConnDac.GetConnectionIus();

            DbCommand cmd;

            cmd = lConn.CreateCommand();
            cmd.Connection = lConn;

            try
            {
                lConn.Open();

                cmd.CommandText = "INSERT INTO Tesis_MatSGA VALUES(" + ius4 + "," + iden + "," + tomo + ",'" + stamp.ToString() + "','Mantesis',1," + AccesoUsuarioModel.Llave + ",'" + AccesoUsuarioModel.Nombre + "'," + MantesisVerIusCommonObjects.Utilities.ValuesMant.Volumen + ")";
                cmd.ExecuteNonQuery();

                this.SalvarBitacora(ius4, 1);
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
        }

        public void DeleteRelacion(long ius4, long idenMat)
        {
            DbConnection lConn = DbConnDac.GetConnectionIus();

            DbCommand cmd;

            cmd = lConn.CreateCommand();
            cmd.Connection = lConn;

            try
            {
                lConn.Open();

                cmd.CommandText = "DELETE FROM Tesis_MatSGA WHERE IUS = " + ius4 + " AND IdMatSGA = " + idenMat;
                cmd.ExecuteNonQuery();

                this.SalvarBitacora(ius4, 4);
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
        }

        private void SalvarBitacora(long ius4, int nTipo)
        {
            DbConnection connmBitacora = new SqlConnection();
            try
            {
                connmBitacora = DbConnDac.GetConnectionmBitacora();
                connmBitacora.Open();
                string sGuid;

                sGuid = Guid.NewGuid().ToString();

                string sqlCadena = "INSERT INTO MantBitacora (Id, IdElemento, IdUsr,FechaStr,Fecha,IdMovimiento,TipoDoc,TipoCat,Usuario,Comp,HoraStr,Observaciones,IdAbs)" +
                                   " VALUES (" + nTipo + "," + ius4 + "," + AccesoUsuarioModel.Llave + ",'" + DateTime.Now.ToString("yyyy/MM/dd") + "','" + DateTime.Now.ToString("yyyy/MM/dd") + "',0,20,20,'" +
                                   MiscFunct.GetUserCurrent() + "','" + MiscFunct.GetPcCurrent() + "','" +
                                   DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss") + "','4','" + sGuid + "')";

                DbCommand command = connmBitacora.CreateCommand();
                command.CommandText = sqlCadena;
                command.ExecuteNonQuery();

                connmBitacora.Close();
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
                connmBitacora.Close();
            }
        }

        public List<Estructura> CargaRelacion(long idMatSga, List<Estructura> consulta)
        {
            DbConnection sqlConne = DbConnDac.GetConnectionIus();

            string sqlCadena = "SELECT * FROM cMateriasSGA Where ID =  " + idMatSga;

            try
            {
                DataTableReader reader = this.GetDatosTabla(sqlCadena, sqlConne);

                long padre = 0;

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        padre = Convert.ToInt64(reader["padre"].ToString());
                        consulta.Add(new Estructura(Convert.ToInt64(reader["id"].ToString()), Convert.ToInt64(reader["nivel"].ToString()),
                            padre, reader["descripcion"].ToString(), Convert.ToInt16(reader["historica"].ToString()),
                            Convert.ToInt32(reader["hoja"].ToString())));
                    }
                }

                if (padre != 0)
                {
                    CargaRelacion(padre, consulta);
                }
                consulta.Sort(delegate(Estructura p1, Estructura p2)
                {
                    return p1.Nivel.CompareTo(p2.Nivel);
                });
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
                sqlConne.Close();
            }

            return consulta;
        }
    }
}
