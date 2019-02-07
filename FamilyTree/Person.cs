using System;
using System.Collections.Generic;

namespace FamilyTree
{
    public class Person
    {
        public int Birthyear;
        public List<Person> Children = new List<Person>();
        public string Name;
        public List<Person> Parents = new List<Person>();
        public int Generation;
        public Person Partner;

        /// <summary>
        ///     Finds a person with the name and birthyear parameter inside the personList.
        ///     If person is not found the function creates a new person with the name and birthyear params.
        /// </summary>
        /// <param name="name">Name of the wanted person</param>
        /// <param name="birthyear">Birthyear of the wanted person</param>
        /// <param name="familyTree">Familytree of the person</param>
        /// <returns></returns>
        public static Person FindPerson(string name, int birthyear, List<Person> familyTree, bool createPerson)
        {
            for (var i = 0; i < familyTree.Count; i++)
            {
                if (familyTree[i].Name == name && familyTree[i].Birthyear == birthyear)
                    return familyTree[i];
            }
            // Creates a new person if the person is not found.
            if (createPerson)
            {
                var person = new Person();
                person.Name = name;
                person.Birthyear = birthyear;
                return person;    
            }
            
            return new Person();
        }

        public int GetGeneration(int generationCounter)
        {
            if (Parents.Count > 0)
            {
                generationCounter++;
                generationCounter = Parents[0].GetGeneration(generationCounter);
                
            }
            
            return generationCounter;
        }
    }
}