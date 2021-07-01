using Microsoft.VisualStudio.TestTools.UnitTesting;
using name_sorter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace name_sorter.Tests
{
    [TestClass()]
    public class PersonNameTests
    {
        [TestMethod()]
        public void One_Givenname_and_a_Surname_Parses_to_PersonName()
        {
            try
            {
                PersonName.Parse("Nooshin Sichani");
            }
 
            catch
            {
                Assert.Fail();
            }
        }
        [TestMethod()]
        public void Two_Givennames_and_a_Surname_Parses_to_PersonName()
        {
            try
            {
                PersonName.Parse("Nooshin Sichani");
            }

            catch
            {
                Assert.Fail();
            }
        }
        [TestMethod()]
        public void One_Char_Givennames_and_one_Char_Surname_Parses_to_PersonName()
        {
            try
            {
                PersonName.Parse("N S");
            }

            catch
            {
                Assert.Fail();
            }
        }
        [TestMethod()]
        public void Surname_with_No_Given_Name_Parse_Throw_Exception()
        {
            try
            {
                PersonName.Parse("Nooshin");
                Assert.Fail();
            }

            catch  {}
        }
        [TestMethod()]
        public void Surname_With_Four_Given_Name_Parse_Throw_Exception()
        {
            try
            {
                PersonName.Parse("Nooshin Fallahpour Sichani Yazdi");
                Assert.Fail();
            }

            catch
            {
            }
        }
        [TestMethod()]
        public void Same_Surname_Same_Givenname_Are_Equally_sorted()
        {
            Assert.AreEqual(((new PersonName("Nooshin", "Sichani")).CompareTo(new PersonName("Nooshin", "Sichani"))), 0);
        }
        [TestMethod()]
        public void Same_Surname_Sort_Based_On_Givennames()
        {
            Assert.AreEqual(((new PersonName("Nooshin", "Sichani")).CompareTo(new PersonName("Negin", "Sichani"))), 1);
        }
    }
}