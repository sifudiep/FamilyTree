using System;
using System.Collections.Generic;

namespace FamilyTree
{
    public class Person
    {
        public string Name;
        public List<Person> Parents = new List<Person>();
        public Person Partner;
        public List<Person> Children = new List<Person>();
        public DateTime Birthdate;
    }
}