
/// <summary>
///The goal is to provide a c# solution for a Name Sorter.
///The solution has to adhere to SOLID principles.
///Author : Nooshin Sichani
///Date   : 01/07/2021
/// <summary>
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace name_sorter
{
    public interface IOutputable
    {
        String ToOutputString();
    }

    interface ICreator<T>
    {
        T Create(String s);
    }
    interface IListCreator<IComparable>
    {
        List<IComparable> CreateListFromFile(String file);
    }
    public interface ISorter<IComparable>
    {
        void Sort(List<IComparable> list);
    }
    public interface IOutputter<IOutputable>
    {
        public void WriteToConsole(List<IOutputable> list);
        public void WriteToFile(StreamWriter file, List<IOutputable> list);
    }
    class InvalidPersonNameException : Exception
    {
        public InvalidPersonNameException() { }

        public InvalidPersonNameException(string name)
            : base(String.Format("This string cannot be converted to a Person Name: {0}", name))
        {
        }
    }
    /// <summary>
    /// Concrete Class to represent a name that can be assigned to a person
    /// It implements IComparable it means that it is sortable
    /// It implements IOutputable it means that it can be written to output
    /// </summary>
    public class PersonName : IComparable,IOutputable
    {
        private String _surName = " ";
        private String _fullName = " ";
        private String _givenName = " ";
        public PersonName(String surname, String givenName)
        {
            _surName = surname;
            _givenName = givenName;
            _fullName = String.Concat(_givenName, " ", _surName);
        }

        public String ToOutputString()
        {
            return this._fullName;
        }
        /// <summary>
        /// Compares two PersonName objects based on surname first and then givennames
        /// </summary>
        public int CompareTo(Object obj)
        {
            if (obj == null) return 1;
            PersonName otherName = (PersonName)obj;
            int surNameCompare;
            if ((surNameCompare = String.Compare(this._surName, otherName._surName)) != 0)
                return surNameCompare;
            return String.Compare(this._givenName, otherName._givenName);
        }

    }
    /// <summary>
    /// This class is responsible for validating and converting a String into PersonName
    /// </summary>
    public class PersonNameCreator :ICreator<PersonName>
    {
        private const int MIN_GIVEN_NAMES = 1;
        private const int MAX_GIVEN_NAMES = 3;

        public PersonName Create(String unformattedName)
        {
             if (unformattedName == null
                || unformattedName.Length < 2
                || unformattedName.LastIndexOf(" ") <= 0)
                throw new InvalidPersonNameException(unformattedName);

            var surname = unformattedName.Substring(unformattedName.LastIndexOf(" ") + 1);
            var givenname = unformattedName.Substring(0, unformattedName.LastIndexOf(" "));

            if ((givenname.Replace(" ", "").Length == 0)
                || (givenname.Split().Length < MIN_GIVEN_NAMES)
                || (givenname.Split().Length > MAX_GIVEN_NAMES))
                throw new InvalidPersonNameException(unformattedName);

            return new PersonName(surname, givenname);
        }
    }
    /// <summary>
    /// This class is responsible for creating a List of PersonNames from an Input File
    /// </summary>
    class PersonNameListCreator : IListCreator<PersonName>
    {
        public List<PersonName> CreateListFromFile(string file)
        {
           var query = File.ReadAllLines(file)
                                .Select(line => (new PersonNameCreator()).Create(line));
            return query.ToList();
        }
    }
    /// <summary>
    /// This class is responsible for sorting a List of objects comparable Objects
    /// </summary>
    public class ListSorter : ISorter<IComparable>
    {
        public void Sort(List<IComparable> listToSort)
        {
            listToSort.Sort();
        }
    }
    /// <summary>
    /// This class is responsible to output a List of objects that are Outputabble
    /// </summary>
    public class ListOutputter : IOutputter<IOutputable>
    {
        public void WriteToConsole(List<IOutputable> list)
        {
            foreach (IOutputable item in list)
            {
                Console.WriteLine(item.ToString());
            }
        }
        public void WriteToFile(StreamWriter writer, List<IOutputable> list)
        {
            foreach (IOutputable item in list)
            {
                writer.WriteLine(item.ToString());
            }
            writer.Close();
        }
    }
    /// <summary>
    /// This class is responsible for orchastrating activities required to build, sort and output a list of
    /// objects.
    /// It has an implemented method Sort that accepts a file and a content Type
    /// Based on content Type it will build the list, sort and output.
    /// I can be extended to sort any other object types and source data from other data sources.
    /// 
    /// </summary>
    public class ObjectSorter
    {
        public void Sort(String file, String fileContentType) 
        {
            if (String.Equals(fileContentType, "NAMES"))
            {
                try
                {
                    var unsortedList = (new PersonNameListCreator()).CreateListFromFile(file);
                    List<IComparable> sortList = unsortedList.Cast<IComparable>().ToList();
                    List<IOutputable> outputList = unsortedList.Cast<IOutputable>().ToList();
                    (new ListSorter()).Sort(sortList);
                    var outputer = new ListOutputter();
                    outputer.WriteToConsole(outputList);
                    StreamWriter writer = new StreamWriter("sorted-names-list.txt");
                    outputer.WriteToFile(writer, outputList);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
                throw new NotImplementedException("Sort of this file type contenct not implemented yet");
        
        }
    }
    /// <summary>
    /// Execute ObjectSorter for a file containing Person names
    /// </summary>
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Missing File Name in the arguments");
                return;
            }
            String file = args[0];
            if (File.Exists(file))
            {

                try
                {
                    new ObjectSorter().Sort(file, "NAMES");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
            else
            {
                Console.WriteLine("File does not exist in the current directory!");
                Console.WriteLine("Current Direcotry is :" + Path.GetFullPath("."));
            }
        }
    }
    }
