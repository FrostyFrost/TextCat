using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using System.Data.SqlClient;

namespace TextCat
{
    class ClassConnection : IDisposable 
    {
            public OleDbConnection connection = new OleDbConnection();

            private static int viborkaNumber;

            public static int ViborkaNumber
            {
                get { return ClassConnection.viborkaNumber; }
                set { ClassConnection.viborkaNumber = value; }
            }
            public BindingSource bindingSource = new BindingSource();
            public DataSet dataSet = new DataSet();
            private bool disposed = false;
           // public string ConnectionString;

            public void connectionOpen()
            {
                
                connection.ConnectionString = @"Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=TextCat;Data Source=(local)";
                try
                    {
                        connection.Open();
                       // MessageBox.Show ("Connection OK");
                    }
                catch   
                    {
                        MessageBox.Show ("Connection error");
                    }
            
             }
            //public void sqlQuery()
            //{ 
      
            //string connect = @"Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=TextCat;Data Source=(local)";
            //DataSet ds = new DataSet();
            //            SqlConnection dataBaseConnection = new SqlConnection(connect);
            //            SqlDataAdapter dataAdapter = new SqlDataAdapter("SELECT * FROM User", dataBaseConnection);
            //            dataAdapter.Fill(ds, "User");
            //}
               

       
            public void connectionClose()
            {
                connection.Close();

            }

            public int SendCommandINT(string strSQL)
            {
                OleDbCommand cmd = new OleDbCommand(strSQL , connection );
                return (int)cmd.ExecuteScalar();
            }

            public bool SendCommand(string strSQL)
            {
                OleDbCommand cmd = new OleDbCommand(strSQL, connection);
                OleDbDataAdapter adapter = new OleDbDataAdapter (cmd);


                if (cmd.ExecuteNonQuery() != -1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public  DataSet  SendCommandDS (string strSQL)
            {
                //DataSet dataSet = new DataSet();
                OleDbCommand cmd = new OleDbCommand(strSQL, connection);
                //OleDbCommand cmd2 = new OleDbCommand("SELECT *  FROM [TextCat].[dbo].[BrS]" +
                //                                        "select * from [TextCat].[dbo].q_SA63", connection);
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                adapter.Fill(dataSet);

                return dataSet;

            }

            public void GetViborkaNumber()      // Получить номер выборки
            {
                int i = 0;
                OleDbCommand cmdGetVibNum = connection.CreateCommand();
                cmdGetVibNum.CommandType = CommandType.StoredProcedure;
                cmdGetVibNum.CommandText = "dbo.Viborka_Check";

                cmdGetVibNum.Parameters.Add("@retvalue", OleDbType.Integer);
                cmdGetVibNum.Parameters[0].Direction = ParameterDirection.ReturnValue;


                try
                {
                    cmdGetVibNum.ExecuteNonQuery();
                }
                catch (OleDbException e)
                {
                    i++;
                    if (i <= 2) { cmdGetVibNum.ExecuteNonQuery(); }
                    else { MessageBox.Show(e.ToString()); }
                }
             
                ViborkaNumber = (int)cmdGetVibNum.Parameters[0].Value;
            }


            public void MDTCreate(string VibNum, bool IsFast)       // Создаем МДТ
            { 


                            OleDbCommand cmdProc = connection.CreateCommand(); 
           
                            cmdProc.CommandType = CommandType.StoredProcedure;
                            cmdProc.CommandText = "MDT_GLOBAL_Create";
                            cmdProc.Parameters.Add("@fast_", OleDbType.Boolean);
                            cmdProc.Parameters.Add("@viborka_", OleDbType.Integer);
                            cmdProc.Parameters[0].Value = IsFast;  //если стоит галочка "Быстрое", то передаем 1
                            cmdProc.Parameters[1].Value = VibNum;  //номер выборки

                            OleDbDataReader reader = cmdProc.ExecuteReader();    //Выполнили процедуру

                            //Получаем номер выборки
                            GetViborkaNumber();
                        
                            MessageBox.Show("Готово. \nНомер выборки " + ViborkaNumber.ToString());
            }



            public int  CallProfileObuch(string methodName, string language, bool type_obuch)  // Обучение, кроме UNI2
            {
                int i=0;
                OleDbCommand cmdObuch = connection.CreateCommand();

                cmdObuch.CommandType = CommandType.StoredProcedure;
                cmdObuch.CommandText = "profile_obuch_proc";

                cmdObuch.Parameters.Add("@return_value", OleDbType.Integer);
                cmdObuch.Parameters[0].Direction = ParameterDirection.ReturnValue;
            

                cmdObuch.Parameters.Add("@method", OleDbType.VarChar , 4);
                cmdObuch.Parameters[1].Direction = ParameterDirection.Input;
                cmdObuch.Parameters[1].Value = methodName;

                cmdObuch.Parameters.Add("@lang", OleDbType.Char);
                cmdObuch.Parameters[2].Direction = ParameterDirection.Input;
                switch (language)
                {   case "Русская": cmdObuch.Parameters[2].Value = "r"; break;
                    case "Английская": cmdObuch.Parameters[2].Value = "e"; break;
                    case "Смешанная": cmdObuch.Parameters[2].Value = "a"; break;
                };

                cmdObuch.Parameters.Add("@fast", OleDbType.Boolean);
                cmdObuch.Parameters[3].Direction = ParameterDirection.Input;
                cmdObuch.Parameters[3].Value = type_obuch;

                cmdObuch.Parameters.Add("@viborka", OleDbType.Integer);
                cmdObuch.Parameters[4].Direction = ParameterDirection.Input;
                cmdObuch.Parameters[4].Value = ViborkaNumber ;

                cmdObuch.Parameters.Add("@top_r", OleDbType.Integer);
                cmdObuch.Parameters[5].Direction = ParameterDirection.Input;
                cmdObuch.Parameters[5].Value = 0;

                cmdObuch.Parameters.Add("@top_e", OleDbType.Integer);
                cmdObuch.Parameters[6].Direction = ParameterDirection.Input;
                cmdObuch.Parameters[6].Value = 0;

                cmdObuch.Parameters.Add("@F1", OleDbType.Double);
                cmdObuch.Parameters[7].Direction = ParameterDirection.Input;
                cmdObuch.Parameters[7].Value = 1;

                cmdObuch.Parameters.Add("@F2", OleDbType.Double);
                cmdObuch.Parameters[8].Direction = ParameterDirection.Input;
                cmdObuch.Parameters[8].Value = 1;

                cmdObuch.Parameters.Add("@F3", OleDbType.Double);
                cmdObuch.Parameters[9].Direction = ParameterDirection.Input;
                cmdObuch.Parameters[9].Value = 1;
                try
                {
                    cmdObuch.ExecuteNonQuery();
                }
                catch (OleDbException e)
                {
                    i++;
                    if (i <= 2) { cmdObuch.ExecuteNonQuery();  }
                    else { MessageBox.Show(e.ToString()); }
                }
                //DONE: Сделать проверку завершенности
                return (int)cmdObuch.Parameters[0].Value;
         
            }

            public int CallProfileObuch(string methodName, string language, bool type_obuch, int top_r, int top_e)  // Обучение UNI2,3 (пар-ры top_r, top_e)
            {
                int i = 0;
                OleDbCommand cmdObuch = connection.CreateCommand();

                cmdObuch.CommandType = CommandType.StoredProcedure;
                cmdObuch.CommandText = "profile_obuch_proc";

                cmdObuch.Parameters.Add("@return_value", OleDbType.Integer);
                cmdObuch.Parameters[0].Direction = ParameterDirection.ReturnValue;


                cmdObuch.Parameters.Add("@method", OleDbType.VarChar, 4);
                cmdObuch.Parameters[1].Direction = ParameterDirection.Input;
                cmdObuch.Parameters[1].Value = methodName;

                cmdObuch.Parameters.Add("@lang_", OleDbType.Char);
                cmdObuch.Parameters[2].Direction = ParameterDirection.Input;
                cmdObuch.Parameters[2].Value = "a"; // для UNI только смешанный
               

                cmdObuch.Parameters.Add("@fast", OleDbType.Boolean);
                cmdObuch.Parameters[3].Direction = ParameterDirection.Input;
                cmdObuch.Parameters[3].Value = type_obuch;

                cmdObuch.Parameters.Add("@viborka", OleDbType.Integer);
                cmdObuch.Parameters[4].Direction = ParameterDirection.Input;
                cmdObuch.Parameters[4].Value = ViborkaNumber;

                cmdObuch.Parameters.Add("@top_r", OleDbType.Integer);
                cmdObuch.Parameters[5].Direction = ParameterDirection.Input;
                cmdObuch.Parameters[5].Value = top_r;

                cmdObuch.Parameters.Add("@top_e", OleDbType.Integer);
                cmdObuch.Parameters[6].Direction = ParameterDirection.Input;
                cmdObuch.Parameters[6].Value = top_e;

                cmdObuch.Parameters.Add("@F1", OleDbType.Double);
                cmdObuch.Parameters[7].Direction = ParameterDirection.Input;
                cmdObuch.Parameters[7].Value = 1;

                cmdObuch.Parameters.Add("@F2", OleDbType.Double);
                cmdObuch.Parameters[8].Direction = ParameterDirection.Input;
                cmdObuch.Parameters[8].Value = 1;

                cmdObuch.Parameters.Add("@F3", OleDbType.Double);
                cmdObuch.Parameters[9].Direction = ParameterDirection.Input;
                cmdObuch.Parameters[9].Value = 1;

                try
                {
                    cmdObuch.ExecuteNonQuery();
                }
                catch (OleDbException e)
                {
                    i++;
                    if (i <= 2) { cmdObuch.ExecuteNonQuery(); }
                    else { MessageBox.Show(e.ToString()); }
                }
                //DONE: Сделать проверку завершенности
                return (int)cmdObuch.Parameters[0].Value;

            }


            public int CallProfileObuch(string methodName, string language, bool type_obuch, int top_r, int top_e, double F1, double F2, double F3)  // Обучение UNI6 (пар-ры top_r, top_e, F1, F2, F3)
            {
                int i = 0;
                OleDbCommand cmdObuch = connection.CreateCommand();

                cmdObuch.CommandType = CommandType.StoredProcedure;
                cmdObuch.CommandText = "profile_obuch_proc";

                cmdObuch.Parameters.Add("@return_value", OleDbType.Integer);
                cmdObuch.Parameters[0].Direction = ParameterDirection.ReturnValue;


                cmdObuch.Parameters.Add("@method", OleDbType.VarChar, 4);
                cmdObuch.Parameters[1].Direction = ParameterDirection.Input;
                cmdObuch.Parameters[1].Value = methodName;

                cmdObuch.Parameters.Add("@lang_", OleDbType.Char);
                cmdObuch.Parameters[2].Direction = ParameterDirection.Input;
                cmdObuch.Parameters[2].Value = "a"; // для UNI только смешанный


                cmdObuch.Parameters.Add("@fast", OleDbType.Boolean);
                cmdObuch.Parameters[3].Direction = ParameterDirection.Input;
                cmdObuch.Parameters[3].Value = type_obuch;

                cmdObuch.Parameters.Add("@viborka", OleDbType.Integer);
                cmdObuch.Parameters[4].Direction = ParameterDirection.Input;
                cmdObuch.Parameters[4].Value = ViborkaNumber;

                cmdObuch.Parameters.Add("@top_r", OleDbType.Integer);
                cmdObuch.Parameters[5].Direction = ParameterDirection.Input;
                cmdObuch.Parameters[5].Value = top_r;

                cmdObuch.Parameters.Add("@top_e", OleDbType.Integer);
                cmdObuch.Parameters[6].Direction = ParameterDirection.Input;
                cmdObuch.Parameters[6].Value = top_e;

                cmdObuch.Parameters.Add("@F1", OleDbType.Double);
                cmdObuch.Parameters[7].Direction = ParameterDirection.Input;
                cmdObuch.Parameters[7].Value = F1;

                cmdObuch.Parameters.Add("@F2", OleDbType.Double);
                cmdObuch.Parameters[8].Direction = ParameterDirection.Input;
                cmdObuch.Parameters[8].Value = F2;

                cmdObuch.Parameters.Add("@F3", OleDbType.Double);
                cmdObuch.Parameters[9].Direction = ParameterDirection.Input;
                cmdObuch.Parameters[9].Value = F3;
                try
                {
                    cmdObuch.ExecuteNonQuery();
                }
                catch (OleDbException e)
                {
                    i++;
                    if (i <= 2) { cmdObuch.ExecuteNonQuery(); }
                    else { MessageBox.Show(e.ToString()); }
                }
                //DONE: Сделать проверку завершенности
                return (int)cmdObuch.Parameters[0].Value;

            }


            // Классификация - для профилей, без КБС  
            public int CallProfilePartsCycle(string methodName, string language, int length, bool fast)
            {
                int i = 0;
                OleDbCommand cmdClassif = connection.CreateCommand();
                cmdClassif.CommandType = CommandType.StoredProcedure;
                cmdClassif.CommandText = "profile_cycle_proc";

                cmdClassif.Parameters.Add("@return_value", OleDbType.Integer);
                cmdClassif.Parameters[0].Direction = ParameterDirection.ReturnValue;

                cmdClassif.Parameters.Add("@method_", OleDbType.VarChar, 4);
                cmdClassif.Parameters[1].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[1].Value = methodName;

                cmdClassif.Parameters.Add("@lang_", OleDbType.Char);
                cmdClassif.Parameters[2].Direction = ParameterDirection.Input;
                switch (language)
                {
                    case "Русская": cmdClassif.Parameters[2].Value = "r"; break;
                    case "Английская": cmdClassif.Parameters[2].Value = "e"; break;
                    case "Смешанная": cmdClassif.Parameters[2].Value = "a"; break;
                };

                cmdClassif.Parameters.Add("@M_", OleDbType.Integer);
                cmdClassif.Parameters[3].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[3].Value = length;

                cmdClassif.Parameters.Add("@kbs", OleDbType.Integer);
                cmdClassif.Parameters[4].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[4].Value = 0;

                cmdClassif.Parameters.Add("@viborka_", OleDbType.Integer);
                cmdClassif.Parameters[5].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[5].Value = ViborkaNumber;

                cmdClassif.Parameters.Add("@fast", OleDbType.Boolean);
                cmdClassif.Parameters[6].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[6].Value = fast;

                cmdClassif.Parameters.Add("@method_t_", OleDbType.VarChar, 4);
                cmdClassif.Parameters[7].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[7].Value = "";

                cmdClassif.Parameters.Add("@method_a_", OleDbType.VarChar, 4);
                cmdClassif.Parameters[8].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[8].Value = "";

                cmdClassif.Parameters.Add("@method_k_", OleDbType.VarChar, 4);
                cmdClassif.Parameters[9].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[9].Value = "";

                cmdClassif.Parameters.Add("@t_", OleDbType.Double);
                cmdClassif.Parameters[10].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[10].Value = 0;

                cmdClassif.Parameters.Add("@a_", OleDbType.Double);
                cmdClassif.Parameters[11].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[11].Value = 0;

                cmdClassif.Parameters.Add("@k_", OleDbType.Double);
                cmdClassif.Parameters[12].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[12].Value = 0;
                //TODO: Центроид - двоичные или символьные данные могут быть усечены

                try
                {
                    cmdClassif.ExecuteNonQuery();
                }
                catch (OleDbException e)
                {
                    i++;
                    if (i <= 2) { cmdClassif.ExecuteNonQuery(); }
                    else { MessageBox.Show(e.ToString()); }
                }

                return (int)cmdClassif.Parameters[0].Value;
            }

            // Классификация - для раздельной классификации 
            public int CallProfilePartsCycle(string methodName, string language, int length, bool fast, string method_T, string method_A, string method_K, double t_c, double t_a, double t_k) 
            {
                int i = 0;
                OleDbCommand cmdClassif = connection.CreateCommand();
                cmdClassif.CommandType = CommandType.StoredProcedure;
                cmdClassif.CommandText = "profile_cycle_proc";

                cmdClassif.Parameters.Add("@return_value", OleDbType.Integer);
                cmdClassif.Parameters[0].Direction = ParameterDirection.ReturnValue;

                cmdClassif.Parameters.Add("@method_", OleDbType.VarChar, 4);
                cmdClassif.Parameters[1].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[1].Value = methodName;

                cmdClassif.Parameters.Add("@lang_", OleDbType.Char);
                cmdClassif.Parameters[2].Direction = ParameterDirection.Input;
                switch (language)
                {
                    case "Русская": cmdClassif.Parameters[2].Value = "r"; break;
                    case "Английская": cmdClassif.Parameters[2].Value = "e"; break;
                    case "Смешанная": cmdClassif.Parameters[2].Value = "a"; break;
                };

                cmdClassif.Parameters.Add("@M_", OleDbType.Integer);
                cmdClassif.Parameters[3].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[3].Value = length;

                cmdClassif.Parameters.Add("@kbs", OleDbType.Integer);
                cmdClassif.Parameters[4].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[4].Value = 0;

                cmdClassif.Parameters.Add("@viborka_", OleDbType.Integer);
                cmdClassif.Parameters[5].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[5].Value = ViborkaNumber;

                cmdClassif.Parameters.Add("@fast", OleDbType.Boolean);
                cmdClassif.Parameters[6].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[6].Value = fast;

                cmdClassif.Parameters.Add("@method_t_", OleDbType.VarChar, 4);
                cmdClassif.Parameters[7].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[7].Value = method_T;

                cmdClassif.Parameters.Add("@method_a_", OleDbType.VarChar, 4);
                cmdClassif.Parameters[8].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[8].Value = method_A;

                cmdClassif.Parameters.Add("@method_k_", OleDbType.VarChar, 4);
                cmdClassif.Parameters[9].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[9].Value = method_K;

                cmdClassif.Parameters.Add("@t_", OleDbType.Double);
                cmdClassif.Parameters[10].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[10].Value = t_c;

                cmdClassif.Parameters.Add("@a_", OleDbType.Double);
                cmdClassif.Parameters[11].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[11].Value = t_a;

                cmdClassif.Parameters.Add("@k_", OleDbType.Double);
                cmdClassif.Parameters[12].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[12].Value = t_k;
                //TODO: Центроид - двоичные или символьные данные могут быть усечены
                
                try
                {
                    cmdClassif.ExecuteNonQuery();
                }
                catch (OleDbException e)
                {
                    i++;
                    if (i <= 2) { cmdClassif.ExecuteNonQuery(); }
                    else { MessageBox.Show(e.ToString()); }
                }

                return (int)cmdClassif.Parameters[0].Value;
            }

            // Классификация - для КБС  
            public int CallProfilePartsCycle(string methodName, string language, int length, int kbs, bool fast) 
            {
                int i = 0;

                OleDbCommand cmdClassif = connection.CreateCommand();
                cmdClassif.CommandType = CommandType.StoredProcedure;
                cmdClassif.CommandText = "profile_cycle_proc";

                cmdClassif.Parameters.Add("@return_value", OleDbType.Integer);
                cmdClassif.Parameters[0].Direction = ParameterDirection.ReturnValue;

                cmdClassif.Parameters.Add("@method_", OleDbType.VarChar, 4);
                cmdClassif.Parameters[1].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[1].Value = methodName;

                cmdClassif.Parameters.Add("@lang_", OleDbType.Char);
                cmdClassif.Parameters[2].Direction = ParameterDirection.Input;
                switch (language)
                {
                    case "Русская": cmdClassif.Parameters[2].Value = "r"; break;
                    case "Английская": cmdClassif.Parameters[2].Value = "e"; break;
                    case "Смешанная": cmdClassif.Parameters[2].Value = "a"; break;
                };

                cmdClassif.Parameters.Add("@M_", OleDbType.Integer);
                cmdClassif.Parameters[3].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[3].Value = length;

                cmdClassif.Parameters.Add("@kbs", OleDbType.Integer);
                cmdClassif.Parameters[4].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[4].Value = kbs;

                cmdClassif.Parameters.Add("@viborka_", OleDbType.Integer);
                cmdClassif.Parameters[5].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[5].Value = ViborkaNumber;

                cmdClassif.Parameters.Add("@fast", OleDbType.Boolean );
                cmdClassif.Parameters[6].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[6].Value = fast;

                cmdClassif.Parameters.Add("@method_t_", OleDbType.VarChar, 4);
                cmdClassif.Parameters[7].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[7].Value = "";

                cmdClassif.Parameters.Add("@method_a_", OleDbType.VarChar, 4);
                cmdClassif.Parameters[8].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[8].Value = "";

                cmdClassif.Parameters.Add("@method_k_", OleDbType.VarChar, 4);
                cmdClassif.Parameters[9].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[9].Value = "";

                cmdClassif.Parameters.Add("@t_", OleDbType.Double);
                cmdClassif.Parameters[10].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[10].Value = 0;

                cmdClassif.Parameters.Add("@a_", OleDbType.Double);
                cmdClassif.Parameters[11].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[11].Value = 0;

                cmdClassif.Parameters.Add("@k_", OleDbType.Double);
                cmdClassif.Parameters[12].Direction = ParameterDirection.Input;
                cmdClassif.Parameters[12].Value =0 ;
                try
                {
                    cmdClassif.ExecuteNonQuery();
                }
                catch (OleDbException e)
                {
                    i++;
                    if (i <= 2) { cmdClassif.ExecuteNonQuery(); } 
                    else { MessageBox.Show(e.ToString());}                  
                }
                return (int)cmdClassif.Parameters[0].Value;
            }

            public int CallKRPCheck(string method1, string method2, string method3, string method4, string method5 ) // Классификация коллективом решающих правил
            {
                OleDbCommand cmdKRP = connection.CreateCommand();
                cmdKRP.CommandType = CommandType.StoredProcedure;
                cmdKRP.CommandText = "KRP_Check";

                cmdKRP.Parameters.Add("@return_value", OleDbType.Integer);
                cmdKRP.Parameters[0].Direction = ParameterDirection.ReturnValue;

                cmdKRP.Parameters.Add("@method1", OleDbType.VarChar, 4);
                cmdKRP.Parameters[1].Direction = ParameterDirection.Input;
                cmdKRP.Parameters[1].Value = method1;

                cmdKRP.Parameters.Add("@method2", OleDbType.VarChar, 4);
                cmdKRP.Parameters[2].Direction = ParameterDirection.Input;
                cmdKRP.Parameters[2].Value = method2;

                cmdKRP.Parameters.Add("@method3", OleDbType.VarChar, 4);
                cmdKRP.Parameters[3].Direction = ParameterDirection.Input;
                cmdKRP.Parameters[3].Value = method3;

                cmdKRP.Parameters.Add("@method4", OleDbType.VarChar, 4);
                cmdKRP.Parameters[4].Direction = ParameterDirection.Input;
                cmdKRP.Parameters[4].Value = method4;

                cmdKRP.Parameters.Add("@method5", OleDbType.VarChar, 4);
                cmdKRP.Parameters[5].Direction = ParameterDirection.Input;
                cmdKRP.Parameters[5].Value = method5;

                cmdKRP.ExecuteNonQuery();

                return (int)cmdKRP.Parameters[0].Value;
            }

            public void Dispose()
            {
                Dispose(true);
                // This object will be cleaned up by the Dispose method.
                // Therefore, you should call GC.SupressFinalize to
                // take this object off the finalization queue
                // and prevent finalization code for this object
                // from executing a second time.
                GC.SuppressFinalize(this);
            }


            protected virtual void Dispose(bool disposing)
            {
                // Check to see if Dispose has already been called.
                if (!this.disposed)
                {
                    // If disposing equals true, dispose all managed
                    // and unmanaged resources.
                    if (disposing)
                    {
                        // Dispose managed resources.
                        connection.Dispose();
                        bindingSource.Dispose();
                        dataSet.Dispose();
                    }

                    // Call the appropriate methods to clean up
                    // unmanaged resources here.
                    // If disposing is false,
                    // only the following code is executed.
                   


                    // Note disposing has been done.
                    disposed = true;

                }
            }


            public void ConnectionClose()
            { 
                connection.Close();
            }

       }




























 }

