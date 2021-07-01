
/// <summary>
///The goal is to provide a c# solution for a Name Sorter.
///Author : Nooshin Sichani
///Date   : 01/07/2021
/// <summary>
using System;
using System.IO;
using System.Linq;
namespace name_sorter
{
    class InvalidPersonNameException : Exception
    {
        public InvalidPersonNameException() { }

        public InvalidPersonNameException(string name)
            : base(String.Format("This string cannot be converted to a Person Name: {0}", name))
        {
        }
    }
    /// <summary>
    /// This class represents an object that can be allocated as a name to a person
    /// It contains required methods to convert an string into a person name and method
    /// to compare two names alphabetically.
    /// </summary>
    public class PersonName : IComparable
    {
        private String _surName = " ";
        private String _fullName = " ";
        private String _givenName = " ";
        private const int MIN_GIVEN_NAMES = 1;
        private const int MAX_GIVEN_NAMES = 3;

        public String SurName
        {
            get { return _surName; }
        }

        public String GivenName
        {
            get { return _givenName; }
        }
        public String FullName
        {
            get { return _fullName; }
        }
        /// <summary>
        /// Convert a String into a PersonName object split string to givenname and  surname with space delimiter
        /// it throws an exception if string cannot be formatted to a PersonName
        /// </summary>
        public static PersonName Parse(String unformattedName)
        {
            if (unformattedName == null 
                || unformattedName.Length < 2 
                || unformattedName.LastIndexOf(" ") <= 0)
                throw new InvalidPersonNameException(unformattedName);

            var surname = unformattedName.Substring(unformattedName.LastIndexOf(" ") + 1);
            var givenname = unformattedName.Substring(0, unformattedName.LastIndexOf(" "));

            if (givenname.Replace(" ", "").Length == 0)
                throw new InvalidPersonNameException(unformattedName);

            if (givenname.Split().Length < MIN_GIVEN_NAMES)
                throw new InvalidPersonNameException(unformattedName);

            if (givenname.Split().Length > MAX_GIVEN_NAMES)
                throw new InvalidPersonNameException(unformattedName);

            return new PersonName(surname, givenname);

        }
        public PersonName(String surname,String givenName)
        {
            _surName = surname;
            _givenName = givenName;
            _fullName = String.Concat(_givenName, " ", _surName);
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
    /// Reads file into an array of PersonNames.
    /// Perform sort on the list , print list on screen and create an extract
    /// named sorted-names-list.txt in the current directory.
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
                    // Read Strings from file and parse them into array of PersonName
                    PersonName[] names =
                        File.ReadAllLines(file)
                            .Select(line => PersonName.Parse(line)).ToArray();
                    Array.Sort(names);
                    StreamWriter writer = new StreamWriter("sorted-names-list.txt");
                    foreach (PersonName name in names)
                    {
                        Console.WriteLine(name.FullName);
                        writer.WriteLine(name.FullName);
                    }
                    writer.Close();
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
