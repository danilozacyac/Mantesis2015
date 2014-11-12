using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using MantesisVerIusCommonObjects.Dto;
using MantesisVerIusCommonObjects.Model;
using MantesisVerIusCommonObjects.Utilities;
using ScjnUtilities;
using UtilsMantesis;

namespace Mantesis2015.Controllers
{
    public class NumIusController
    {
        private readonly MainWindow main;

        



        public void GetTesisByVerIus(string txtNumIus)
        {
            try
            {
                if (txtNumIus.Length < 8)
                {
                    NumIusModel numIUsModel = new NumIusModel();

                    bool isTesisEliminated = numIUsModel.GetCurrentTesisState(Convert.ToInt32(txtNumIus));
                    TesisDto tesis;

                    if (isTesisEliminated)//La tesis ya fue eliminada
                    {
                        tesis = numIUsModel.BuscaTesisEliminadasPorRegistro(Convert.ToInt32(txtNumIus));

                        if (tesis != null)
                        {
                            tesis.IsEliminated = isTesisEliminated;

                            MessageBox.Show("Esta tesis fue eliminada");

                            UnaTesis unaTesis = new UnaTesis(tesis, true);
                            //fUnaTesis.Tag = permisos;
                            unaTesis.ShowDialog();
                            //this.Show();

                        }
                        else
                        {
                            tesis = numIUsModel.BuscaTesis(Convert.ToInt32(txtNumIus));
                            if (tesis != null)
                            {
                                tesis.IsEliminated = isTesisEliminated;
                                MessageBox.Show("Esta tesis fue eliminada");

                                UnaTesis unaTesis = new UnaTesis(tesis, true);
                                //fUnaTesis.Tag = permisos;
                                unaTesis.ShowDialog();
                                //this.Show();
                            }
                            else
                            {
                                MessageBox.Show("Introduzca un número de registro valido");
                            }
                        }
                    }
                    else
                    {
                        tesis = numIUsModel.BuscaTesis(Convert.ToInt32(txtNumIus));

                        //var volumenAuth = (from n in AccesoUsuarioModel.VolumenesPermitidos
                        //                   where n.Volumen == tesis.VolumenInt
                        //                   select n).ToList();

                        //if (volumenAuth.Count() == 0 && userCanModify == true)
                        //    userCanModify = false;
                        //else if (volumenAuth.Count() > 0 && userCanModify == false)
                        //    userCanModify = true;

                        if (tesis.Parte >= 100 && tesis.Parte <= 145)
                        {
                            ValuesMant.Epoca = 7;
                            ValuesMant.Volumen = tesis.VolumenInt;
                        }


                        if (tesis != null)
                        {
                            //this.Hide();

                            UnaTesis unaTesis = new UnaTesis(tesis, true);
                            //fUnaTesis.Tag = permisos;
                            unaTesis.ShowDialog();
                            //this.Show();
                        }
                        else
                        {
                            MessageBox.Show("Introduzca un número de registro valido");
                        }
                    }

                }
                else
                {
                    MessageBox.Show("Introduzca un número de registro valido");
                }
            }
            catch (NullReferenceException ex)
            {
                string methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;

                MessageBox.Show("Error ({0}) : {1}" + ex.Source + ex.Message);//, methodName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ErrorUtilities.SetNewErrorMessage(ex, methodName, 0);
            }
        }

    }
}
