using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using MantesisVerIusCommonObjects.Dto;

namespace Mantesis2015.MateriasSga
{
    public class MateriasViewModel
    {
        static readonly string connectionString = ConfigurationManager.ConnectionStrings["BaseIUS"].ToString();


        public static List<MateriasModel> GetEstructuraNivel(int padre, bool isReadOnly)
        {
            List<MateriasModel> listaMaterias = new List<MateriasModel>();

            SqlConnection sqlConne = new SqlConnection(connectionString);// (SqlConnection)DbConnDac.GetConnectionIus();

            SqlCommand cmd;
            SqlDataReader dataReader;
            string miQry;

            try
            {
                sqlConne.Open();
                miQry = "Select * FROM cMateriasSGA WHERE padre = " + padre + " ORDER BY Consec";
                cmd = new SqlCommand(miQry, sqlConne);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    MateriasModel materia = new MateriasModel(dataReader["Descripcion"].ToString(),
                        GetEstructuraNivel(Convert.ToInt32(dataReader["ID"].ToString()),isReadOnly), Convert.ToInt32(dataReader["ID"].ToString()));

                    materia.IsReadOnly = isReadOnly;

                    listaMaterias.Add(materia);
                }
            }
            catch (SqlException sex)
            {
                MessageBox.Show("Error ({0}) : {1}" + sex.Source + sex.Message, "Error Interno");
            }
            catch (Exception sex)
            {
                MessageBox.Show("Error ({0}) : {1}" + sex.Source + sex.Message, "Error Interno");
            }
            finally
            {
                sqlConne.Close();
            }

            return listaMaterias;
        }

        public static List<int> GetMateriasRelacionadas(long ius, int volumen)
        {
            List<int> listaMaterias = new List<int>();

            SqlConnection sqlConne = new SqlConnection(connectionString);// (SqlConnection)DbConnDac.GetConnectionIus();

            SqlCommand cmd;
            SqlDataReader dataReader;
            string miQry;

            try
            {
                sqlConne.Open();
                miQry = "Select idMatSGA FROM Tesis_MatSGA WHERE ius = @ius AND Volumen = @Volumen";// +RequestData.Volumen;
                cmd = new SqlCommand(miQry, sqlConne);
                cmd.Parameters.AddWithValue("@ius", ius);
                cmd.Parameters.AddWithValue("@Volumen", volumen);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    listaMaterias.Add(Convert.ToInt32(dataReader["idMatSGA"].ToString()));
                }
            }
            catch (SqlException sex)
            {
                MessageBox.Show("Error ({0}) : {1}" + sex.Source + sex.Message, "Error Interno");
            }
            catch (Exception sex)
            {
                MessageBox.Show("Error ({0}) : {1}" + sex.Source + sex.Message, "Error Interno");
            }
            finally
            {
                sqlConne.Close();
            }

            return listaMaterias;
        }

        public static void SetRelacionMateriasIus(long ius, List<int> materias, int volumen)
        {
            SqlConnection sqlConne = new SqlConnection(connectionString);// (SqlConnection)DbConnDac.GetConnectionIus();
            SqlCommand cmd;

            cmd = sqlConne.CreateCommand();
            cmd.Connection = sqlConne;

            try
            {
                sqlConne.Open();

                cmd.CommandText = "DELETE FROM Tesis_MatSGA WHERE ius = " + ius + " AND Volumen = " + volumen;
                cmd.ExecuteNonQuery();

                foreach (int materia in materias)
                {
                    int tomo = Convert.ToInt32(materia.ToString().Substring(0, 3));
                    cmd.CommandText = "INSERT INTO Tesis_MatSGA VALUES(" + ius + "," + materia + "," + tomo + ",'"
                        + DateTime.Now.ToString() + "','Mantesis',0," + AccesoUsuarioModel.Llave + ",'" + AccesoUsuarioModel.Nombre
                        + "'," + volumen + ")";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "INSERT INTO Bitacora_MateriasSGA VALUES(" + ius + "," + materia + "," + tomo + ",'Mantesis',0,"
                        + volumen + ",0," + AccesoUsuarioModel.Llave + ",'" + AccesoUsuarioModel.Nombre
                        + "',GETDATE())";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (SqlException sex)
            {
                MessageBox.Show("Error ({0}) : {1}" + sex.Source + sex.Message, "Error Interno");
            }
            catch (Exception sex)
            {
                MessageBox.Show("Error ({0}) : {1}" + sex.Source + sex.Message, "Error Interno");
            }
            finally
            {
                sqlConne.Close();
            }
        }


        /*
        private List<MateriasSga> GetEstructuraNivel(int padre)
        {
            List<MateriasSga> ListaMaterias = new List<MateriasSga>();

            SqlConnection sqlConne = (SqlConnection)DBConnDAC.GetConnectionIUS();

            SqlCommand cmd;
            SqlDataReader dataReader;
            string miQry;

            try
            {
                sqlConne.Open();
                miQry = "Select * FROM cMateriasSGA WHERE padre = " + padre + " ORDER BY Consec";
                cmd = new SqlCommand(miQry, sqlConne);
                dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    MateriasSga _Materia = new MateriasSga();
                    _Materia.Id = Convert.ToInt32(dataReader["id"].ToString());
                    _Materia.Nivel = Convert.ToInt32(dataReader["Nivel"].ToString());
                    _Materia.Padre = Convert.ToInt32(dataReader["Padre"].ToString());
                    _Materia.Descripcion = dataReader["Descripcion"].ToString();
                    _Materia.SeccionPadre = Convert.ToInt32(dataReader["seccionPadre"].ToString());
                    _Materia.Historica = Convert.ToInt32(dataReader["Historica"].ToString());
                    _Materia.Consec = Convert.ToInt32(dataReader["Consec"].ToString());
                    _Materia.Hoja = Convert.ToInt32(dataReader["Hoja"].ToString());
                    _Materia.NvlImpresion = Convert.ToInt32(dataReader["NvlImpresion"].ToString());
                    
                    ListaMaterias.Add(_Materia);
                }
            }
            catch (SqlException) { }
            finally
            {
                sqlConne.Close();
            }

            return ListaMaterias;
        }

        public List<TreeViewItem> getArbolClasificación()
        {
            List<TreeViewItem> _ArbolClasif = new List<TreeViewItem>();

            List<MateriasSga> _Temas = this.GetEstructuraNivel(0);

            foreach (MateriasSga tema in _Temas)
            {
                TreeViewItem item = new TreeViewItem();
                item.Header = tema.Descripcion;
                item.Tag = tema;

                item = generaHijoRecursivo(item, tema.Id);
                item.IsExpanded = true;
                _ArbolClasif.Add(item);
            }

            return _ArbolClasif;
        }

        private TreeViewItem generaHijoRecursivo(TreeViewItem item, int idPadre)
        {
            List<MateriasSga> _Childs = this.GetEstructuraNivel(idPadre);

            foreach (MateriasSga hijo in _Childs)
            {
                TreeViewItem child = new TreeViewItem();
                child.Header = hijo.Descripcion;
                child.Tag = hijo;
                child = generaHijoRecursivo(child, hijo.Id);
                child.IsExpanded = true;
                item.Items.Add(child);
            }
            return item;
        }*/
    }
}
