using System;
using System.Xml.Serialization;
using System.IO;
using System.Linq;
using System.Diagnostics;


namespace Lab2
{
    class Program


    { /// <summary>
      /// Вызывается метод Input и возвращенная им информация записывается в массив info
      /// <param name="info">массив содержащий информацию из файла input.txt</param>
      /// Вызывается метод DisplayAllInfo и FindInfo(в него передается информация считанная из консоли)
      /// </summary>
        static void Main(string[] args)
        {
            TextWriterTraceListener tr1 = new TextWriterTraceListener(System.Console.Out);
            Trace.Listeners.Add(tr1);
            Trace.Flush();


            PhoneBook[] info = Input();

            DisplayAllInfo(info);

            Console.WriteLine("Enter your request:");
            FindInfo(Console.ReadLine(), info);
            
            Console.ReadLine();
        }
        /// <summary>
        /// Метод считывает информацию из файла input.txt и записывает ее в массив info
        /// <param name="info">массив содержащий информацию из файла input.txt</param>
        /// <param name="str">вспомогательный массив</param>
        /// <param name="n">количество записей в начальном файле</param>
        /// </summary>
        ///  <returns>Массив info</returns>
        static PhoneBook[] Input()
        {
            Int32.TryParse(File.ReadLines(@"D:\input.txt").Skip(0).First(), out int n);
            PhoneBook[] info = new PhoneBook[n];
            string[] str;
            for (int i = 0; i < n; i++)
            {
                str = File.ReadLines(@"D:\input.txt").Skip(i + 1).First().Split(',');
                if (str.Length == 3)
                    info[i] = new Person(str[0], str[1], str[2]);
                else if (str.Length == 4)
                    info[i] = new Friend(str[0], str[1], str[2], str[3]);
                else if (str.Length == 5)
                    info[i] = new Organization(str[0], str[1], str[2], str[3], str[4]);
            }
           
            XmlSerializer formatter = new XmlSerializer(typeof(PhoneBook[]));

            using (FileStream fs = new FileStream("phonebook.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, info);
            }
            return info;
          
        }
        /// <summary>
        /// Метод выводит все значения массива info
        /// <param name="info">массив содержащий информацию из файла input.txt</param>
        /// <param name="a">вспомогательная переменная</param>
        /// </summary>
        static void DisplayAllInfo(PhoneBook[] info)
        {
            foreach (PhoneBook a in info)
            {          
                a.Display();
            }
        }
        /// <summary>
        /// Метод выводит заданную запись из массива info
        /// <param name="info">массив содержащий информацию из файла input.txt</param>
        /// <param name="str">строка, по которой производится поиск</param>
        /// </summary>
        static void FindInfo(string str, PhoneBook[] info)
        {
            foreach (PhoneBook a in info) 
            {
                if (a.SameName(str))
                    a.Display();

            }
        }
    }
    [XmlInclude(typeof(Person))]
    [XmlInclude(typeof(Friend))]
    [XmlInclude(typeof(Organization))]
    [Serializable]
    public abstract class PhoneBook
    {
        /// <summary>
        /// Абстрактный метод Display 
        /// выводит запись в нужном формате
        /// </summary>
        
        public abstract void Display();
        public string name;
        public string address;
        public string phonenumber;
        /// <summary>
        /// Метод Метод находит одинаковые названия\имена
        /// <param name="Name">заданное имя</param>
        /// </summary>
        /// <returns>Возвращает true в случае совпадения, в противном случае false</returns>
        public bool SameName(string Name)
        {          
            Trace.WriteLineIf(Name == name,"Requested info has been found");
            return Name == name;
        }
    }
    [Serializable]
    // производный класс 
    public class Person : PhoneBook
    {
        public Person()
        { }
        /// <summary>
        /// Конструктор
        /// </summary>
        public Person(string name, string address, string phonenumber)
        {
            this.name = name;
            this.address=address;
            this.phonenumber=phonenumber;           
        }

        public override void Display()
        {
            Console.WriteLine("Last Name: {0}, Address: {1}, Phone Number: {2}", name, address, phonenumber);
            Trace.WriteLine("Person info displayed");
        }
    }
    [Serializable]
    public class Friend : PhoneBook
    {
        public Friend()
        { }
        string birthdate;
        /// <summary>
        /// Конструктор
        /// </summary>
        public Friend(string name, string address, string phonenumber, string birthdate)
        {
            this.name = name;
            this.address = address;
            this.phonenumber = phonenumber;
            this.birthdate = birthdate;
        }

        public override void Display()
        {
            Console.WriteLine("Last Name: {0}, Address: {1}, Phone Number: {2}, Birth Date: {3}", 
                name, address, phonenumber, birthdate);
            Trace.WriteLine("Friend info displayed");

        }
    }
    [Serializable]
    public class Organization : PhoneBook
    {
        public Organization()
        { }
        string fax;
        string representative;
        /// <summary>
        /// Конструктор
        /// </summary>
        public Organization(string name, string address, string phonenumber, string fax, string representative)
        {
            this.name = name;
            this.address = address;
            this.phonenumber = phonenumber;
            this.fax = fax;
            this.representative = representative;
        }
        public override void Display()
        {
            Console.WriteLine("Last Name: {0}, Address: {1}, Phone Number: {2}, Fax: {3}, Company Representative: {4}",
                name, address, phonenumber, fax,representative);
            Trace.WriteLine("Organization info displayed");
        }
    }
}
