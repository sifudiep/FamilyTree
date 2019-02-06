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
         *    @Parents
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
            Console.WriteLine("By writing the 'NAME%BIRTHYEAR/#PARTNER%BIRTHYEAR/@PARENT%BIRTHYEAR&PARENT%BIRTHYEAR'");
            Console.WriteLine("Take note of the symbols such as '%, #, /, &, @'");
            Console.WriteLine("Write '%' after every persons name to show separate birthyear and name.");
            Console.WriteLine("Write '#' before the partners name");
            Console.WriteLine("Write '@' before the FIRST parents name, do NOT add it before the second parents name.");
            Console.WriteLine(
                "If David is the first known generation of the family, simply dont add the Parents(Partner is optional)");
            Console.WriteLine("Meaning the input should have this format 'NAME%BIRTHYEAR' ");
            Console.WriteLine("To find relations of a person, type in the following format : '?NAME%BIRTHYEAR'");
        }

        private static void Finished(List<Person> familyTree)
        {
            Console.Clear();
            Console.WriteLine("Finished!");
            Console.WriteLine("Type '!save' if you wish to save the familyTree to a txt file. ");
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
                AddParentsAndPartner(people[1], person, familyTree);
            }

            if (people.Length == 3)
            {
                AddParentsAndPartner(people[2], person, familyTree);
            }

            familyTree.Remove(Person.FindPerson(person.Name, person.Birthyear, familyTree, false));
            familyTree.Add(person);
        }

        private static void AddParentsAndPartner(string people, Person person, List<Person> familyTree)
        {
            if (people[0] == '#')
            {
                var partnerInfo = people.Split('%');
                var partnerName = partnerInfo[0].Remove(0,1);;
                var partnerBirthyear = int.Parse(partnerInfo[1]);
                
                person.Partner = Person.FindPerson(partnerName, partnerBirthyear, familyTree, true);
                person.Partner.Partner = person;
                familyTree.Add(person.Partner);
            }
            else if (people[0] == '@')
            {
                var parents = people.Split('&');

                var firstParentInfo = parents[0].Split('%');
                var firstParentName = firstParentInfo[0].Remove(0, 1);
                var firstParentBirthyear = int.Parse(firstParentInfo[1]);
                person.Parents.Add(Person.FindPerson(firstParentName, firstParentBirthyear, familyTree, true));

                var secondParentInfo = parents[1].Split('%');
                var secondParentName = secondParentInfo[0];
                var secondParentBirthyear = int.Parse(secondParentInfo[1]);
                person.Parents.Add(Person.FindPerson(secondParentName, secondParentBirthyear, familyTree, true));
               
                var firstParent = Person.FindPerson(firstParentName, firstParentBirthyear, familyTree, true);
                var secondParent = Person.FindPerson(secondParentName, secondParentBirthyear, familyTree, true);

                firstParent.Partner = secondParent;
                firstParent.Children.Add(person);
                familyTree.Add(firstParent);
                
                secondParent.Partner = firstParent;
                secondParent.Children.Add(person);
                familyTree.Add(secondParent);
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
                    Console.WriteLine(personName + "'s first parent : " + person.Parents[0].Name);
                    Console.WriteLine(personName + "'s second parent : " + person.Parents[1].Name);
                }
                
                if (person.Partner != new Person() && person.Partner != null)
                {
                    Console.WriteLine("Partner: " + person.Partner.Name);
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
                    Console.WriteLine("Child " + (i + 1) + " : " + person.Children[i].Name);

                Console.WriteLine("Generation: " + person.GetGeneration(1));
            }
        }
    }
}