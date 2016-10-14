using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Timers;
using System.Threading;

namespace TextCat
{

    

    
    public partial class MainForm : Form
    {
        ClassConnection conn = new ClassConnection();   //объект класса ClassConnection
        public  MethodObuch.MethodsObuch METHS_OB = new MethodObuch.MethodsObuch();     //массив методов на обучение 
        public MethodClassif.MethodsClassif METHS_CL = new MethodClassif.MethodsClassif(); // Массив методов классифкации
        private int viborkaNumber;   // перменная - номер выборки
        private string[]  Unis;   // сюда запихиваются все UNI, которые требуют указания top_r, top_e
       

        public int ViborkaNumber
        {
            get { return viborkaNumber; }
            set { viborkaNumber = value; }
        }
        

        //public int ViborkaNumber
        //{
        //    get {return viborkaNumber;}
        //    set {viborkaNumber = value;}
        //}

        public MainForm()
        {
            InitializeComponent();
            this.progressBar1.Value = 10;

            conn.connectionOpen();
            this.progressBar1.Value = 60;
            //Получение номера выборки
            conn.GetViborkaNumber();
            this.labelViborka.Text = "Выборка " + ClassConnection.ViborkaNumber.ToString();
            this.progressBar1.Value = 100;
            this.progressBar1.Visible = false;
            Unis = new string[5] { "Uni2", "Uni3", "Uni5", "Uni6", "Uni7" };

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'textCatDataSet.Methods' table. You can move, or remove it, as needed.
            this.methodsTableAdapter.Fill(this.textCatDataSet.Methods);
            // TODO: данная строка кода позволяет загрузить данные в таблицу "textCatDataSet.Methods". При необходимости она может быть перемещена или удалена.
            this.methodsTableAdapter.Fill(this.textCatDataSet.Methods);
            //// TODO: This line of code loads data into the 'textCatDataSet.BrS' table. You can move, or remove it, as needed.
            //this.brSTableAdapter.Fill(this.textCatDataSet.BrS);
            //// TODO: This line of code loads data into the 'textCatDataSet.Classes' table. You can move, or remove it, as needed.
            //this.classesTableAdapter.Fill(this.textCatDataSet.Classes);
            //// TODO: This line of code loads data into the 'textCatDataSet.centr' table. You can move, or remove it, as needed.
            //this.centrTableAdapter.Fill(this.textCatDataSet.centr);

        }



        private void button2_Click(object sender, EventArgs e)
        {
            // Тест - получение значений из базы запросом         
            //MessageBox.Show(conn.SendCommandINT("select count(*) from Classes").ToString()); // получили значение 15
            //MessageBox.Show(conn.SendCommand("insert into classes (num,class) select '16', 'rr'").ToString()); // добавили строку, ответ true


            // Тест - загрузка данных в гридвью
            //conn.SendCommandDS("select * from Classes");

            //dataGridView1.AutoGenerateColumns = true;
            //conn.bindingSource.DataSource = conn.dataSet.Tables[0];
            //dataGridView1.DataSource = conn.bindingSource;
            ////conn.dataSet.Clear(); // очищаем датагрид
             

           // conn.SendCommandDS("select * from BrS");
          //  dataGridView2.AutoGenerateColumns = true;
          //  conn.bindingSource.DataSource = conn.dataSet.Tables[1];
          //  dataGridView2.DataSource = conn.bindingSource;
          
            //Замена данных в комбобоксе
           // comboBox1.DisplayMember= textCatDataSet.BrS.Columns[0].ToString();
     

            
            
        }

        private void buttonAddToListObuch_Click(object sender, EventArgs e)
        {
            //добавляем метод+язык+тип в массив
            //массив методов на обучение 
             //METHS_OB = new MethodObuch.MethodsObuch();
            // TODO: Проверка ну существование класса с теми же параметрами

            for (int i = 0; i < this.checkedListBoxObuch.CheckedItems.Count; i++)
            {
                
//                if (this.checkedListBoxObuch.CheckedItems[i].ToString() == "Uni2" | this.checkedListBoxObuch.CheckedItems[i].ToString() == "Uni3" | this.checkedListBoxObuch.CheckedItems[i].ToString() == "Uni5" | this.checkedListBoxObuch.CheckedItems[i].ToString() == "Uni6") 
                if (System.Array.IndexOf(Unis, this.checkedListBoxObuch.CheckedItems[i].ToString()) >= 0  ) 
                {
                    METHS_OB[MethodObuch.N] = new MethodObuch(this.checkedListBoxObuch.CheckedItems[i].ToString(), "Смешанная", this.radioButtonFast.Checked, Convert.ToInt32(textBoxTopR.Text), Convert.ToInt32(this.textBoxTopE.Text), Convert.ToDouble(this.textBoxF1.Text), Convert.ToDouble(this.textBoxF2.Text), Convert.ToDouble(this.textBoxF3.Text));
                    // отображаем на листбокс параметры выбранных методов
                    this.listBoxMethodsObuch.Items.Add(METHS_OB[MethodObuch.N - 1].MethodName + " " + "Смешанная" + " " + (METHS_OB[MethodObuch.N - 1].Type_obuch == true ? "Быстрое" : "Полное") + " " + METHS_OB[MethodObuch.N - 1].Int1 + "-" + METHS_OB[MethodObuch.N - 1].Int2);
                }
                else if (this.checkedListBoxObuch.CheckedItems[i].ToString() == "Uni1") 
                {
                    METHS_OB[MethodObuch.N] = new MethodObuch(this.checkedListBoxObuch.CheckedItems[i].ToString(), "Смешанная", this.radioButtonFast.Checked);
                    // отображаем на листбокс параметры выбранных методов
                    this.listBoxMethodsObuch.Items.Add(METHS_OB[MethodObuch.N - 1].MethodName + " " + "Смешанная" + " " + (METHS_OB[MethodObuch.N - 1].Type_obuch == true ? "Быстрое" : "Полное"));
                }
                else 
                {
                    METHS_OB[MethodObuch.N] = new MethodObuch(this.checkedListBoxObuch.CheckedItems[i].ToString(), this.comboBoxLang.SelectedItem.ToString(), this.radioButtonFast.Checked);
                    // отображаем на листбокс параметры выбранных методов
                    this.listBoxMethodsObuch.Items.Add(METHS_OB[MethodObuch.N - 1].MethodName + " " + METHS_OB[MethodObuch.N - 1].Language + " " + (METHS_OB[MethodObuch.N - 1].Type_obuch == true ? "Быстрое" : "Полное"));
                }
            }
            
         
 

        }





        private void buttonMDTRun_Click(object sender, EventArgs e)
        {
            
            if ((this.comboBoxMDT.SelectedItem != null) & ((this.radioButtonMDTFast.Checked != false) | (this.radioButtonMDTFull.Checked != false)) )
            {

                object[] ParArr = { this.comboBoxMDT.SelectedItem.ToString(), this.radioButtonMDTFast.Checked,  this };

                Thread t = new Thread(new ParameterizedThreadStart(Func));
                t.Start(ParArr);

                //Thread t = new Thread(this.conn.MDTCreate).Start(this.comboBoxMDT.SelectedItem.ToString(), this.radioButtonMDTFast.Checked);
                // conn.MDTCreate(this.comboBoxMDT.SelectedItem.ToString(), this.radioButtonMDTFast.Checked);
               
            }
            else
            { 
                MessageBox.Show("Укажите параметры"); 
            }
        }


        private void Func(object Params)
        {
            //frmProgress frm = new frmProgress();
            //frm.Owner = this;
            //frm.ShowDialog();

            conn.MDTCreate(((object[])(Params))[0].ToString(), (bool)((object[])(Params))[1]);
            string vibnum = ViborkaNumber.ToString(); 

            var form = ((MainForm)((object[])Params)[2]);
            form.Invoke(new Action( () => 
            {
                form.conn.GetViborkaNumber();
                form.labelViborka.Text = ("Выборка "  + ((object[])(Params))[0].ToString());
            }));
        } 




        private void buttonRunObuch_Click(object sender, EventArgs e)
        {
           
            this.progressBar1.Visible = true;
            this.progressBar1.Value = 0;
            
            // по порядку считать строки
            string strReady = "";
            for (int i = 0; i < this.listBoxMethodsObuch.Items.Count; i++)
            {
                // для каждой строки вызвать процедуру 
                if (METHS_OB[i].MethodName != null | METHS_OB[i].MethodName != "" )
                {
//                    if (METHS_OB[i].MethodName == "Uni2" | METHS_OB[i].MethodName == "Uni3" | METHS_OB[i].MethodName == "Uni5" | METHS_OB[i].MethodName == "Uni6") 
                    if (System.Array.IndexOf(Unis, METHS_OB[i].MethodName) >= 0) 
                    {
                            this.progressBar1.Value = this.progressBar1.Value + (100 / this.listBoxMethodsObuch.Items.Count);
                            strReady = strReady + (conn.CallProfileObuch(METHS_OB[i].MethodName, "Смешанная", METHS_OB[i].Type_obuch, METHS_OB[i].Int1, METHS_OB[i].Int2, METHS_OB[i].Int3, METHS_OB[i].Int4, METHS_OB[i].Int5) > 0 ? METHS_OB[i].MethodName.ToString() + " Готово\n" : METHS_OB[i].MethodName.ToString() + " Ошибка\n");
                         }
                    else if (METHS_OB[i].MethodName == "Uni1") 
                         {
                            this.progressBar1.Value = this.progressBar1.Value + (100 / this.listBoxMethodsObuch.Items.Count);
                            strReady = strReady + (conn.CallProfileObuch(METHS_OB[i].MethodName, "Смешанная", METHS_OB[i].Type_obuch) > 0 ? METHS_OB[i].MethodName.ToString() + " Готово\n" : METHS_OB[i].MethodName.ToString() + " Ошибка\n");
                         }
                     else 
                         {
                            this.progressBar1.Value = this.progressBar1.Value + (100 / this.listBoxMethodsObuch.Items.Count);
                            strReady = strReady + (conn.CallProfileObuch(METHS_OB[i].MethodName, METHS_OB[i].Language, METHS_OB[i].Type_obuch) > 0 ? METHS_OB[i].MethodName.ToString() + " Готово\n" : METHS_OB[i].MethodName.ToString() + " Ошибка\n");
                         }
                }




                //if ((METHS_OB[i].MethodName != null | METHS_OB[i].MethodName != "" ) & METHS_OB[i].MethodName != "Uni2")
                //{
                //    this.progressBar1.Value = this.progressBar1.Value + (100 / this.listBoxMethodsObuch.Items.Count);
                //    strReady = strReady + (conn.CallProfileObuch(METHS_OB[i].MethodName, METHS_OB[i].Language, METHS_OB[i].Type_obuch) > 0 ? METHS_OB[i].MethodName.ToString() + " Готово\n" : METHS_OB[i].MethodName.ToString() + " Ошибка\n");
                //}
                //else if (METHS_OB[i].MethodName == "Uni2")
                //{
                //    this.progressBar1.Value = this.progressBar1.Value + (100 / this.listBoxMethodsObuch.Items.Count);
                //    strReady = strReady + (conn.CallProfileObuch(METHS_OB[i].MethodName, "a", METHS_OB[i].Type_obuch, METHS_OB[i].Int1,METHS_OB[i].Int2 ) > 0 ? METHS_OB[i].MethodName.ToString() + " Готово\n" : METHS_OB[i].MethodName.ToString() + " Ошибка\n");

                //}
            }

            this.progressBar1.Visible = false;
            MessageBox.Show(strReady);
            //for (int i = 0; i <= 3; i++) { MessageBox.Show(METHS_OB[i].language + METHS_OB[i].methodName); } //показываем параметры методов на обучение
        }

        private void buttonClearObuch_Click(object sender, EventArgs e)    //Очистка списка на обучение
        {
            for (int i = 0; i < this.listBoxMethodsObuch.Items.Count; i++)
            {
                MethodObuch.N = 0;
                this.listBoxMethodsObuch.Items.Clear();
            }

            // очищаем листбокс обучения
            
            while (checkedListBoxObuch.CheckedIndices.Count > 0)
            {
                checkedListBoxObuch.SetItemChecked(checkedListBoxObuch.CheckedIndices[0], false);
            }

        }


        private void buttonAddToListClassif_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.checkedListBoxClassif.CheckedItems.Count; i++)
            {
                if (this.checkedListBoxClassif.CheckedItems[i].ToString() == "KBS") // Для КБС
                    {
                        METHS_CL[MethodClassif.N] = new MethodClassif(this.checkedListBoxClassif.CheckedItems[i].ToString(), this.comboBoxLangClassif.SelectedItem.ToString(), Convert.ToInt32(this.textBoxKBSm.Text), Convert.ToInt32(this.textBoxKBSsosedi.Text), this.radioButtonClassifFast.Checked);
                        // отображаем на листбокс параметры выбранных методов 
                        this.listBoxMethodsClassif.Items.Add(METHS_CL[MethodClassif.N - 1].MethodName + " " + METHS_CL[MethodClassif.N - 1].Language + " " + (METHS_CL[MethodClassif.N - 1].Length) + " " + (METHS_CL[MethodClassif.N - 1].Kbs));
                    }

                else if (System.Array.IndexOf(Unis,this.checkedListBoxClassif.CheckedItems[i].ToString()) >= 0) //Для UNI
                    {
                        METHS_CL[MethodClassif.N] = new MethodClassif(this.checkedListBoxClassif.CheckedItems[i].ToString(),"Смешанная", Convert.ToInt32(this.comboBoxProfileLength.SelectedItem), this.radioButtonClassifFast.Checked);
                        this.listBoxMethodsClassif.Items.Add(METHS_CL[MethodClassif.N - 1].MethodName + " " + "Смешанная" + " " + (METHS_CL[MethodClassif.N - 1].Length));
                    }

                else if (this.checkedListBoxClassif.CheckedItems[i].ToString() == "Part") //Для Раздельной
                {
                    METHS_CL[MethodClassif.N] = new MethodClassif(this.checkedListBoxClassif.CheckedItems[i].ToString(), this.comboBoxLangClassif.SelectedItem.ToString(), Convert.ToInt32(this.comboBoxProfileLength.SelectedItem), this.radioButtonClassifFast.Checked, this.comboBoxTitle.Text.ToString(), this.comboBoxAbstract.Text.ToString(), this.comboBoxKeywords.Text.ToString(), Convert.ToDouble(this.textBox_T_C.Text.ToString()), Convert.ToDouble(this.textBox_A_C.Text.ToString()), Convert.ToDouble(this.textBox_K_C.Text.ToString()));
                    this.listBoxMethodsClassif.Items.Add(METHS_CL[MethodClassif.N - 1].MethodName + " " + METHS_CL[MethodClassif.N - 1].Language + " " + (METHS_CL[MethodClassif.N - 1].Length) + " " + this.comboBoxTitle.Text.ToString() + "-" + this.comboBoxAbstract.Text.ToString() + "-" + this.comboBoxKeywords.Text.ToString());
                }
                
                
                else
                    {
                        METHS_CL[MethodClassif.N] = new MethodClassif(this.checkedListBoxClassif.CheckedItems[i].ToString(), this.comboBoxLangClassif.SelectedItem.ToString(), Convert.ToInt32(this.comboBoxProfileLength.SelectedItem), this.radioButtonClassifFast.Checked);
                        this.listBoxMethodsClassif.Items.Add(METHS_CL[MethodClassif.N - 1].MethodName + " " + METHS_CL[MethodClassif.N - 1].Language + " " + (METHS_CL[MethodClassif.N - 1].Length));
                    }       
           }
        }

        private void buttonRunClassif_Click(object sender, EventArgs e) // Классифицировать
        {
            this.progressBar1.Visible = true;
            this.progressBar1.Value = 0;
            // по порядку считать строки
            //string strReady = "";
            for (int i = 0; i < this.listBoxMethodsClassif.Items.Count; i++)
            {
                // для каждой строки вызвать процедуру 
                if (METHS_CL[i].MethodName != null | METHS_CL[i].MethodName != "")
                {
                    this.progressBar1.Value = this.progressBar1.Value + (100 / this.listBoxMethodsClassif.Items.Count);
                    //проставляем ошибки в листбокс
                    
                    if (METHS_CL[i].MethodName != "KBS" & METHS_CL[i].MethodName != "Part")
                    { this.listBoxMethodsClassif.Items[i] += " Ош" +this.labelViborka.Text.Replace("Выборка ", "")  + ": " + (conn.CallProfilePartsCycle(METHS_CL[i].MethodName, METHS_CL[i].Language, METHS_CL[i].Length, METHS_CL[i].Type_classif) / 1.05).ToString();}
                    else if (METHS_CL[i].MethodName == "Part")
                    { this.listBoxMethodsClassif.Items[i] += " Ош" + this.labelViborka.Text.Replace("Выборка ", "") + ": " + (conn.CallProfilePartsCycle(METHS_CL[i].MethodName, METHS_CL[i].Language, METHS_CL[i].Length, METHS_CL[i].Type_classif, METHS_CL[i].Method_T, METHS_CL[i].Method_A, METHS_CL[i].Method_K, METHS_CL[i].t_c, METHS_CL[i].a_c, METHS_CL[i].k_c) / 1.05).ToString(); } 
                    else // для КБС
                    { this.listBoxMethodsClassif.Items[i] += " Ош" + this.labelViborka.Text.Replace("Выборка ", "") + ": " + (conn.CallProfilePartsCycle(METHS_CL[i].MethodName, METHS_CL[i].Language, METHS_CL[i].Length, METHS_CL[i].Kbs, METHS_CL[i].Type_classif) / 1.05).ToString(); }
                    
                    //strReady = strReady + (conn.CallProfilePartsCycle (METHS_CL[i].MethodName, METHS_CL[i].Language, METHS_CL[i].Length) > 0 ? METHS_CL[i].MethodName.ToString() + " Готово\n" +  : METHS_CL[i].MethodName.ToString() + " Ошибка\n");
                    
                    //Вставляем tblRes в гридвью
                    //conn.SendCommandDS("select * from tblRes");
                    //dataGridViewResultsClassif.AutoGenerateColumns = true;
                    //conn.bindingSource.DataSource = conn.dataSet.Tables[0];
                    //dataGridViewResultsClassif.DataSource = conn.bindingSource;
                }

            }

            this.progressBar1.Visible = false;
            //MessageBox.Show(strReady);
        }

        private void buttonRunKRP_Click(object sender, EventArgs e)
        {
            this.progressBar1.Visible = true;
            this.progressBar1.Value = 0;

            if (this.checkedListBoxKRP.CheckedItems.Count == 3)
            {
                MessageBox.Show((conn.CallKRPCheck(this.checkedListBoxKRP.CheckedItems[0].ToString(), this.checkedListBoxKRP.CheckedItems[1].ToString(), this.checkedListBoxKRP.CheckedItems[2].ToString(), "", "") / 1.05).ToString());
            }
            else if (this.checkedListBoxKRP.CheckedItems.Count == 5)
            {
                MessageBox.Show("КРП, ошибка: " + (conn.CallKRPCheck(this.checkedListBoxKRP.CheckedItems[0].ToString(), this.checkedListBoxKRP.CheckedItems[1].ToString(), this.checkedListBoxKRP.CheckedItems[2].ToString(), this.checkedListBoxKRP.CheckedItems[3].ToString(), this.checkedListBoxKRP.CheckedItems[4].ToString()) / 1.05).ToString());
            }
            else
            {
                MessageBox.Show("Выберите 3 или 5 классификаторов");
            }

            this.progressBar1.Value = 100;
            this.progressBar1.Visible = false;
        }

        private void buttonClearClassif_Click(object sender, EventArgs e) // Очистить список классификации
        {
            for (int i = 0; i < this.listBoxMethodsClassif.Items.Count; i++)
            {
                MethodClassif.N = 0;
                this.listBoxMethodsClassif.Items.Clear();
            }

            // очищаем листбокс классификации

            while (checkedListBoxClassif.CheckedIndices.Count > 0)
            {
                checkedListBoxClassif.SetItemChecked(checkedListBoxClassif.CheckedIndices[0], false);
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.ConnectionClose();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void checkedListBoxObuch_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxTitle_Click(object sender, EventArgs e)
        {
            this.checkedListBoxClassif.SetItemChecked (13 ,true );
        }

        private void tabPageClassification_Click(object sender, EventArgs e)
        {

        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            conn.dataSet.Clear();
            string top = this.textBoxTop.Text;
            string cl = this.textBoxCl.Text;
            conn.SendCommandDS("select  mdt_" + cl + ".word, count_words,  isnull(z1.r, 0) as RO , isnull(z2.r, 0) as MI_I, isnull(z3.r, 0) as S1, isnull(z4.r, 0) as S2,  (isnull(z1.rr, 0) + isnull(z2.rr, 0) + isnull(z3.rr, 0) + isnull(z4.rr, 0))as sum_profiles from mdt_" + cl + " " +
                                "left join (select top " + top + " word,  ISNULL(RO_" + cl + ".max_ro, 0) as r, count(ISNULL(RO_" + cl + ".max_ro, 0)) as rr from RO_" + cl + " group by word, max_ro order by max_ro desc ) as z1 on mdt_" + cl + ".word = z1.word " +
                                "left join (select top " + top + " word,  ISNULL(MI_I_" + cl + ".max_MI_I, 0) as r, count(ISNULL(MI_I_" + cl + ".max_MI_I, 0)) as rr  from MI_I_" + cl + " group by word, max_MI_I order by max_MI_I desc ) as z2 on mdt_" + cl + ".word = z2.word " +
                                "left join (select top " + top + " word,  ISNULL(S1_" + cl + ".max_S1, 0) as r, count(ISNULL(S1_" + cl + ".max_s1, 0)) as rr  from S1_" + cl + " group by word, max_s1 order by max_s1 desc ) as z3 on mdt_" + cl + ".word = z3.word " +
                                "left join (select top " + top + " word,  ISNULL(S2_" + cl + ".max_S2, 0) as r, count(ISNULL(S2_" + cl + ".max_s2, 0)) as rr  from S2_" + cl + " group by word, max_s2 order by max_s2 desc ) as z4 on mdt_" + cl + ".word = z4.word " + 

                                "order by sum_profiles desc");

            dataGridView_Profiles.AutoGenerateColumns = true;
            conn.bindingSource.DataSource = conn.dataSet.Tables[0];
            dataGridView_Profiles.DataSource = conn.bindingSource;

            //DataGridViewCellStyle style = new DataGridViewCellStyle();
            //style.BackColor = Color.White;
            //style.ForeColor = Color.Blue;

            
            //this.dataGridView_Profiles.DefaultCellStyle = style;

            for (int i = 0; i < this.dataGridView_Profiles.RowCount; i++)
            {
                switch (this.dataGridView_Profiles[6, i].FormattedValue.ToString())
                {
                    case "4": dataGridView_Profiles[0, i].Style.BackColor = Color.Green;
                        break;
                    case "3": dataGridView_Profiles[0, i].Style.BackColor = Color.Yellow;
                        break;
                    case "2": dataGridView_Profiles[0, i].Style.BackColor = Color.Orange;
                        break;
                    case "1": dataGridView_Profiles[0, i].Style.BackColor = Color.Red;
                        break;

                }
            }
                  
         }

        private void dataGridView_Profiles_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            for (int i = 0; i < this.dataGridView_Profiles.RowCount; i++)
            {
                switch (this.dataGridView_Profiles[6, i].FormattedValue.ToString())
                {
                    case "4": dataGridView_Profiles[0, i].Style.BackColor = Color.Green;
                        break;
                    case "3": dataGridView_Profiles[0, i].Style.BackColor = Color.Yellow;
                        break;
                    case "2": dataGridView_Profiles[0, i].Style.BackColor = Color.Orange;
                        break;
                    case "1": dataGridView_Profiles[0, i].Style.BackColor = Color.Red;
                        break;

                }
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }
        



     
         
        

    }

}