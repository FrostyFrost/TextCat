using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;



namespace TextCat
{
    public class MethodClassif
    {
        public string MethodName;
        public string Language;
        public int  Length;
        public int Kbs;
       // public bool type_obuch;
        public string Param1;
        public static int N = 0;        //переменная, показывающая текущий номер в массиве (при вызове конструктора увеличивается)
        public bool Type_classif;       //Fast-Full
        public string Method_T;
        public string Method_A;
        public string Method_K;
        public double  t_c;
        public double a_c;
        public double k_c;


        //конструктор класса без параметров
        public MethodClassif(string methodName, string language, bool type_classif) 
        {
            this.MethodName = methodName;
            this.Language = language;
            this.Type_classif = type_classif;
            N++;
        }

        //конструктор класса для профилей - с длиной профиля или m - порогом отсечения
        public MethodClassif(string methodName, string language, int length, bool type_classif) 
        {
            this.MethodName = methodName;
            this.Language = language;
            this.Length = length;
            this.Type_classif = type_classif;
            N++;
        }


        //конструктор класса для КБС
        public MethodClassif(string methodName, string language, int length, int kbs, bool type_classif) 
        {
            this.MethodName = methodName;
            this.Language = language;
            this.Length = length;
            this.Kbs = kbs;
            this.Type_classif = type_classif;
            N++;
        }

        //конструктор класса для раздельной классификации
        public MethodClassif(string methodName, string language, int length, bool type_classif, string method_t, string method_a, string method_k, double t_c, double a_c, double k_c)
        {
            this.MethodName = methodName;
            this.Language = language;
            this.Length = length;
            this.Type_classif = type_classif;
            this.Method_T = method_t;
            this.Method_A = method_a;
            this.Method_K = method_k;
            this.t_c = t_c;
            this.a_c = a_c;
            this.k_c = k_c;

            N++;
        }

        public class MethodsClassif
        { 
            MethodClassif[] methodsClassifArray = new MethodClassif[20];
            public int Get_N() { return N; }
            public MethodClassif this[int pos]
            {
                get
                {
                    if (pos >= 0 || pos < 20) return methodsClassifArray[pos];
                    else throw new IndexOutOfRangeException("Вне диапазона - Классификация");
                }
                set  {   methodsClassifArray[pos] = value; }
            }


        }

    }
}
