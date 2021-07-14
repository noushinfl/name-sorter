# name-sorter
This repository contains a solution implemented in C# for GlobalX Coding Assessment name-sorter.

# Description
Input a text file of names and sort them first by last name and then by any given name a person may have.
A person must have at least one given name and up to 3 given names.

# How did I apply SOLID Concept in my solution

Single Responsibility
  Each class has a specific responsibility. 
  PersonNameCreator class specifically to parse a string into a PersonName object.
  In my previous solution, this was a static method in PersonName class
  
Open/Close
  My Solution is open to extention, so if in future I want to read and sort another object type like BookName , all I need to do is to create a BookName class and a  BookNameCreator class to parse strings into BookNames, I don't have to touch existing PersonName class
  
Liskov substitution principle
  I needed a ToString() method to output PersonNames into console and file output,instead of relying on the fact that Object class has ToString method and all Classes inherits it I decided to create a new IOutputable interface with ToOutputString() method, and each Class that wants to use my framework for sorting has to implement this ToOutputSting method. In this case I won't be surprised with invalid strings in the output.

Interface segregation principle 
  I used IComprable interface to make sure that my object can be sorted correctly by List.Sort() method. I also have IOutputable interface to make sure that I have a valid stirng output representation of the object.i.e. I sperated the responsibility into smaller intefaces.


Dependency inversion principle
  My ListSort class does not know anything about PersonName , PersonName only has to implement IComparable method so it can be sorted via ListSorter.




