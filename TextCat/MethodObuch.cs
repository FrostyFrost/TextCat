using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Windows.Forms;


namespace TextCat
{
   public class MethodObuch
    {



        //private static int N = 0;
        //public ClassMethodObuch() {N++;}



        public string MethodName;
        public string Language;
        public bool Type_obuch;
        public string Param1;
        public int Int1;                //используется как top_r  в  UNI2
        public int Int2;                //используется как top_e  в  UNI2
        public double Int3;                //используется как F1  в  UNI6
        public double Int4;                //используется как F2 в  UNI6
        public double Int5;                //используется как F3  в  UNI6

        public static int N = 0;        //переменная, показывающая текущий номер в массиве (при вызове конструктора увеличивается)

        public MethodObuch(string methodName, string language, bool type_obuch, string param1) //конструктор для метода обучения
        {
            this.MethodName = methodName;
            this.Language = language;
            this.Type_obuch = type_obuch;
            this.Param1 = param1;
            N++;
        }

        public MethodObuch(string methodName, string language, bool type_obuch) // перегруженный конструктор для метода обучения (без параметров)
        {
            this.MethodName = methodName;
            this.Language = language;
            this.Type_obuch = type_obuch;
            N++;

        }
        public MethodObuch(string methodName, string language, bool type_obuch,  int int1, int int2) //перегруженный конструктор для UNI1(2345?)
        {
            this.MethodName = methodName;
            this.Language = language;
            this.Type_obuch = type_obuch;
            this.Int1 = int1;
            this.Int2 = int2;
            N++;
        }
        public MethodObuch(string methodName, string language, bool type_obuch, int int1, int int2, double p3, double p4, double p5) //перегруженный конструктор для UNI6
        {
            this.MethodName = methodName;
            this.Language = language;
            this.Type_obuch = type_obuch;
            this.Int1 = int1;
            this.Int2 = int2;
            this.Int3 = p3;
            this.Int4 = p4;
            this.Int5 = p5;
            N++;
        }


       public class MethodsObuch
        {
            MethodObuch[] methodsArray = new MethodObuch[20];
            public int Get_N() { return N; }
            public MethodObuch this[int pos]
            
            {
                
                get
                {
                    if (pos >= 0 || pos < 20) return methodsArray[pos];
                    else throw new IndexOutOfRangeException("Вне диапазона - Обучение");
                }
                
                set { methodsArray[pos] = value; }

            }

        }

     

    }

}