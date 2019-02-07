using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FamilyTree
{
    internal class Program
    {
        /*
         *    #Partner
         *    @Children
         *    %Birthyear
         *    ?Search
         */
        private static void Main(string[] args)
        {
            var FamilyTree = new List<Person>();
            var notFinished = true;
            while (notFinished)
            {
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Type '!help' for application instructions");
                Console.WriteLine("Type '!finished' to finish the family tree");
                var input = Console.ReadLine();
                if (input == "!help")
                {
                    Instructions();
                }
                else if (input == "!finished")
                {
                    Finished(FamilyTree);
                }
                else if (input == "!generation")
                {
                    ShowGenerations(FamilyTree);
                }
                else if (input[0] == '?')
                {
                    var trimmedInput = input.Remove(0,1);
                    var inputRegEx = new Regex(@"^[0-9]");
                    var matches = inputRegEx.Match(trimmedInput);
                    Console.WriteLine(matches);
                    ShowRelations(trimmedInput, FamilyTree);
                } 
                else
                {
                    if (ValidateInput(input))
                    {
                        AddToFamilyTree(input, FamilyTree);
                        Console.WriteLine("Added To FamilyTree!");
                    }
                    else
                        Console.WriteLine(
                            "Input string was not typed in the right format, type !help for the correct input format.");
                }
            }
        }

        private static void Instructions()
        {
            Console.Clear();
            Console.WriteLine("Add a person to the family tree!");
            Console.WriteLine("By writing the 'NAME%BIRTHYEAR/#PARTNER%BIRTHYEAR/@CHILD%BIRTHYEAR&CHILD%BIRTHYEAR'");
            Console.WriteLine("Take note of the symbols such as '%, #, /, &, @'");
            Console.WriteLine("Write '%' after every persons name to separate birthyear and name.");
            Console.WriteLine("Write '#' before the partners name");
            Console.WriteLine("Write '@' before the FIRST child's name, do NOT add it before the other children's name.");
            Console.WriteLine("To find relations of a person, type in the following format : '?NAME%BIRTHYEAR'");
        }

        private static void Finished(List<Person> familyTree)
        {
            Console.Clear();
            Console.WriteLine("Finished!");
            Console.WriteLine("Type '!save' if you wish to save the familyTree to a txt file. ");
            var input = Console.ReadLine();
            if (input == "!save")
            {
                
            }
        }

        private static void AddToFamilyTree(string input, List<Person> familyTree)
        {
            var person = new Person();
            var people = input.Split('/');

            var personInfo = people[0].Split('%');
            person.Name = personInfo[0];
            person.Birthyear = Convert.ToInt32(personInfo[1]);

            if (people.Length > 1)
            {
                AddChildrenAndPartner(people[1], person, familyTree);
            }

            if (people.Length == 3)
            {
                AddChildrenAndPartner(people[2], person, familyTree);
            }
            
            Person duplicate = new Person(); 
            for (int i = 0; i < familyTree.Count; i++)
            {
                if (familyTree[i].Name == person.Name && familyTree[i].Birthyear == person.Birthyear)
                {
                    duplicate = familyTree[i];
                }
            }
            
            if (duplicate.Children.Count > person.Children.Count)
            {
                foreach (var child in person.Children)
                {
                    person.Children.Remove(child);
                }
                foreach (var child in duplicate.Children)
                {
                    person.Children.Add(child);
                }
            }

            if (duplicate.Parents.Count > person.Parents.Count)
            {
                foreach (var parent in person.Parents)
                {
                    person.Parents.Remove(parent);
                }
                
                foreach (var parent in duplicate.Parents)
                {
                    person.Parents.Add(parent);
                }
            }

            familyTree.Remove(duplicate);
            familyTree.Add(person);
        }

        private static void AddChildrenAndPartner(string people, Person person, List<Person> familyTree)
        {
            if (people[0] == '#')
            {
                var partnerInfo = people.Split('%');
                var partnerName = partnerInfo[0].Remove(0,1);;
                var partnerBirthyear = int.Parse(partnerInfo[1]);
                var partner = Person.FindPerson(partnerName, partnerBirthyear, familyTree, true);
                person.Partner = partner;
                partner.Partner = person;
                
                familyTree.Add(partner);
            }
            else if (people[0] == '@')
            {
                var children = people.Split('&');

                for (int i = 0; i < children.Length; i++)
                {
                    var childInfo = children[i].Split('%');
                    var childName = "";
                    if (i == 0)
                    {
                        childName = childInfo[0].Remove(0, 1);
                    }
                    else
                    {
                        childName = childInfo[0];
                    }
                    var childBirthyear = int.Parse(childInfo[1]);

                    var child = Person.FindPerson(childName, childBirthyear, familyTree, true);
                    
                    person.Children.Add(child);
                    person.Partner.Children.Add(child);
                    child.Parents.Add(person);
                    child.Parents.Add(person.Partner);
                    familyTree.Add(child);
                }
            }
        }

        private static bool ValidateInput(string input)
        {
            var people = input.Split('/');

            if (people[0].Length == 0)
                return false;

            if (people.Length > 1)
            {
                if (people[1][0] != '#' && people[1][0] != '@')
                    return false;
                if (people[1].Length < 3)
                    return false;
            }

            if (people.Length == 3)
            {
                if (people[2][0] != '#' && people[2][0] != '@')
                    return false;
                if (people[2].Length < 3)
                    return false;
            }

            return true;
        }

        private static void ShowRelations(string input, List<Person> familyTree)
        {
            if (input.Contains('/'))
            {
                Console.WriteLine("Wrong input format");
            }
            else
            {
                var personInfo = input.Split('%');
                var personName = personInfo[0];
                var personBirthyear = Convert.ToInt32(personInfo[1]);
            
                var person = Person.FindPerson(personName, personBirthyear, familyTree, false);
                if (person.Name == null)
                {
                    Console.WriteLine("Could not find the person.");
                }
                else
                {
                    Console.WriteLine("ShowingRelations!");
                    if (person.Parents.Count == 0)
                    {
                        Console.WriteLine(personName + " parents are unknown.");
                    }
                    else
                    {
                        Console.WriteLine(personName + "'s first parent : " + person.Parents[0].Name + "%" + person.Parents[0].Birthyear);
                        Console.WriteLine(personName + "'s second parent : " + person.Parents[1].Name + "%" + person.Parents[1].Birthyear);
                    }
                
                    if (person.Partner != new Person() && person.Partner != null)
                    {
                        Console.WriteLine("Partner: " + person.Partner.Name + "%" + person.Partner.Birthyear);
                    }
                    else
                    {
                        Console.WriteLine(personName + " does not have a partner.");
                    }

                    if (person.Children.Count == 0)
                    {
                        Console.WriteLine(personName + " does not have children.");
                    }
                    for (var i = 0; i < person.Children.Count; i++)
                        Console.WriteLine("Child " + (i + 1) + " : " + person.Children[i].Name + "%" + person.Children[i].Birthyear);

                    Console.WriteLine("Generation: " + person.GetGeneration(1));
                }
            }
        }

        private static void ShowGenerations(List<Person> familyTree)
        {
            int latestGeneration = 1;
            for (int i = 0; i < familyTree.Count; i++)
            {
                if (familyTree[i].GetGeneration(1) > latestGeneration)
                {
                    latestGeneration = familyTree[i].GetGeneration(1);
                }
            }

            for (int i = 1; i < latestGeneration; i++)
            {
                string generationText = "Generation " + i + " : ";
                for (int j = 0; j < familyTree.Count; j++)
                {
                    if (familyTree[j].Parents.Count == 0)
                    {
//                        Console.WriteLine("no parents 4 : " + familyTree[j].Name);
                        familyTree[j].Generation = familyTree[j].Partner.GetGeneration(1);
                        if (familyTree[j].Generation == i)
                        {
                            generationText += familyTree[j].Name + " ";
                        }
                    }
                    else
                    {
//                        Console.WriteLine("parents 4 : " + familyTree[j].Name);
                        familyTree[j].Generation = familyTree[j].GetGeneration(1);
                        if (familyTree[j].Generation == i)
                        {
                            generationText += familyTree[j].Name + " ";
                        }
                    }
                }

                Console.WriteLine(generationText);
            }
        }
    }
}