
using System;
 
using System.Collections.Generic;
 
using System.Linq;
 
using System.Text;
 
using System.IO;
 
using System.Net;
 
using System.Runtime;
 
using System.Text.RegularExpressions;
 
 
 
namespace ConsoleApplication1
 
{
 
    class Program
 
    {
 
        public static void getURL()
 
        {
 
            TextWriter textWriter = new StreamWriter("HTMLPages.txt", false);
 
            for (int i = 0; i <= 6200; i=i+100)
 
            {
 
                textWriter.WriteLine("http://www.747.ru/office/sale/cenain:/cenaout:/pl1in:/pl1out:/limitstart:"+ i +"/limit:100/");
 
                Console.WriteLine("http://www.747.ru/office/sale/cenain:/cenaout:/pl1in:/pl1out:/limitstart:" + i + "/limit:100/");
 
            }
 
            textWriter.Close();
 
        }
 
        public static void getHTMLCode()
 
        {
 
            string[] urls = new string[70];
 
            TextReader textReader = new StreamReader("HTMLPages.txt", false);
 
            for (int i = 0; i < 63; i++)
 
            {
 
                urls[i] = textReader.ReadLine();
 
                string url = urls[i];
 
                StringBuilder sb = new StringBuilder();
 
                byte[] buf = new byte[8192];
 
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
 
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
 
                Stream resStream = response.GetResponseStream();
 
                int count = 0;
 
                do
 
                {
 
                    count = resStream.Read(buf, 0, buf.Length);
 
                    if (count != 0)
 
                    {
 
                        sb.Append(Encoding.Default.GetString(buf, 0, count));
 
                    }
 
                }
 
                while (count > 0);
 
                resStream.Close();
 
                TextWriter textWriter = new StreamWriter("Contest.txt", true);
 
                textWriter.WriteLine(sb);
 
                textWriter.Close();
 
            }
 
        }
 
 
 
        public static void getDetailedUrls()
 
        {
 
            TextReader textReader = new StreamReader("Contest.txt", false);
 
            TextWriter textWriter = new StreamWriter("DetailedURLS.txt", false);
 
            string line = "";
 
            do
 
            {
 
                line = textReader.ReadLine();
 
                if (line.Contains("<td><a href='"))
 
                {
 
                    line = line.Trim();
 
                    line = line.Remove(0, 13);
 
                    line = line.Remove(line.Length - 82, 82);
 
                    textWriter.WriteLine(line);
 
                }
 
            } while (line != null);
 
            textWriter.Close();
 
            textReader.Close();
 
        }
 
 
 
        public static void getUnfilteredInformation()
 
        {
 
            string[] urls = new string[6352];
 
            TextReader textReader1 = new StreamReader("DetailedURLS.txt", false);
 
            for (int i = 0; i < 6352; i++)
 
            {
 
                urls[i] = textReader1.ReadLine();
 
                string url = urls[i];
 
                StringBuilder sb = new StringBuilder();
 
                byte[] buf = new byte[8192];
 
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
 
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
 
                Stream resStream = response.GetResponseStream();
 
                int count = 0;
 
                do
 
                {
 
                    count = resStream.Read(buf, 0, buf.Length);
 
                    if (count != 0)
 
                    {
 
                        sb.Append(Encoding.Default.GetString(buf, 0, count));
 
                    }
 
                }
 
                while (count > 0);
 
                resStream.Close();
 
                TextWriter textWriter1 = new StreamWriter("UnfilteredInformation.txt", true);
 
                textWriter1.WriteLine(sb);
 
                textWriter1.Close();
 
            }
 
        }
 
 
 
        public static void getInformation()
 
        {
 
            TextReader textReader = new StreamReader("UnfilteredInformation.txt", false);
 
            TextWriter textWriter = new StreamWriter("Information.txt", false);
 
            //textWriter.Write("");
 
            string[] line = new string[1000];
 
            int index = 0;
 
            int flag = 0;
 
            do
 
            {
 
                line[index] = textReader.ReadLine();
 
                if (line[index] != null)
 
                {
 
 
 
                    if (line[index].Contains("Метро"))
 
                    {
 
                        Console.Write("There is metro");
 
                        line[index + 1] = line[index + 1].Trim();
 
                        line[index + 1] = line[index + 1].Remove(0, 4);
 
                        line[index + 1] = line[index + 1].Remove(line[index + 1].Length - 5, 5);
 
                        textWriter.WriteLine(line[index + 1] + "\t");
 
                    }
 
                    if (line[index].Contains("Улица"))
 
                    {
 
                        Console.Write("There is street");
 
                        line[index + 1] = line[index + 1].Trim();
 
                        line[index + 1] = line[index + 1].Remove(0, 4);
 
                        line[index + 1] = line[index + 1].Remove(line[index + 1].Length - 5, 5);
 
                        textWriter.WriteLine(line[index + 1] + "\t");
 
                    }
 
                    if (line[index].Contains("Этаж"))
 
                    {
 
                        Console.Write("There is floor");
 
                        line[index + 1] = line[index + 1].Trim();
 
                        line[index + 1] = line[index + 1].Remove(0, 4);
 
                        line[index + 1] = line[index + 1].Remove(line[index + 1].Length - 5, 5);
 
                        textWriter.WriteLine(line[index + 1] + "\t");
 
                    }
 
                    if (line[index].Contains("Расст. от метро/вид трансп"))
 
                    {
 
                        Console.Write("There is metrostation");
 
                        line[index + 1] = line[index + 1].Trim();
 
                        line[index + 1] = line[index + 1].Remove(0, 4);
 
                        line[index + 1] = line[index + 1].Remove(line[index + 1].Length - 5, 5);
 
                        textWriter.WriteLine(line[index + 1] + "\t");
 
                    }
 
                    if (line[index].Contains("Площадь"))
 
                    {
 
                        Console.Write("There is ploshad'");
 
                        line[index + 1] = line[index + 1].Trim();
 
                        line[index + 1] = line[index + 1].Remove(0, 4);
 
                        line[index + 1] = line[index + 1].Remove(line[index + 1].Length - 5, 5);
 
                        textWriter.WriteLine(line[index + 1] + "\t");
 
                    }
 
                    if (line[index].Contains("Этажность"))
 
                    {
 
                        Console.Write("There is numfloor");
 
                        string[] splited = new string[100];
 
                        line[index] = line[index].Trim();
 
                        for (int i = 0; i < 100; i++)
 
                        {
 
                            splited = line[index].Split(';', ':');
 
                            if (splited[i] == "Этажность")
 
                            {
 
                                line[index] = splited[i + 1];
 
                            }
 
                        }
 
                        textWriter.WriteLine(line[index] + "\t");
 
                    }
 
                    if (line[index].Contains("Цена"))
 
                    {
 
                        Console.Write("There is price");
 
                        line[index + 1] = line[index + 1].Trim();
 
                        line[index + 1] = line[index + 1].Remove(0, 4);
 
                        line[index + 1] = line[index + 1].Remove(line[index + 1].Length - 5, 5);
 
                        textWriter.WriteLine(line[index + 1] + "\n");
 
                    }
 
                }
 
                if (line[index] == null)
 
                {
 
                    flag = 1;
 
                }
 
                index++;
 
            } while (flag == 0);
 
            textReader.Close();
 
            textWriter.Close();
 
        }
 
        static void Main(string[] args)
 
        {
 
            do
 
            {
 
                //getURL();
 
                //getHTMLCode();
 
                //getDetailedUrls();
 
                //getUnfilteredInformation();
 
                getInformation();
 
            }
 
            while (Console.ReadKey().Key != ConsoleKey.Escape);
 
        }
 
    }
 
}