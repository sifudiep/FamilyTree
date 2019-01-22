using System;
using System.Collections.Generic;
using System.Linq;

namespace FamilyTree
{
    internal class Program
    {
        /*
         *    #Partner
         *    ¤Parents
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
                    ShowRelations(input, FamilyTree);
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
            Console.WriteLine("By writing the NAME separated by the parents");
            Console.WriteLine("I.e : 'David%1992/#Linda%1993/¤Margaret%1970&Per%1965");
            Console.WriteLine(
                "In this example David is the new Person to be added,Linda is Davids Partner and she's born in 1993.");
            Console.WriteLine(
                "If David is the first known generation of the family, simply dont add the '/¤Margaret%1970&Per%1965' input");
            Console.WriteLine("to register David as the oldest known generation of the family.");
            Console.WriteLine("To find relations of a person simple type i.e '?David%1993' this will search and show ");
        }

        private static void Finished(List<Person> familyTree)
        {
            Console.Clear();
            Console.WriteLine("Finished!");
        }

        private static void AddToFamilyTree(string input, List<Person> familyTree)
        {
            var person = new Person();
            var people = input.Split('/');

            var personInfo = people[0].Split('%');
            person.Name = personInfo[0];
            person.Birthyear = Convert.ToInt32(personInfo[1]);

            AddParentsAndPartner(people[1], person, familyTree);
            AddParentsAndPartner(people[2], person, familyTree);

            familyTree.Add(person);
        }

        private static void AddParentsAndPartner(string people, Person person, List<Person> familyTree)
        {
            if (people[0] == '#')
            {
                var partnerInfo = people.Split('%');
                var partnerName = partnerInfo[0];
                var partnerBirthyear = int.Parse(partnerInfo[1]);
                person.Partner = Person.FindPerson(partnerName, partnerBirthyear, familyTree);
                person.Partner.Partner = person;
            }
            else if (people[0] == '¤')
            {
                var parents = people.Split('&');

                var firstParentInfo = parents[0].Split('%');
                var firstParentName = firstParentInfo[0].Remove(0, 1);
                var firstParentBirthyear = int.Parse(firstParentInfo[1]);
                person.Parents.Add(Person.FindPerson(firstParentName, firstParentBirthyear, familyTree));
                Person.FindPerson(firstParentName, firstParentBirthyear, familyTree).Children.Add(person);

                var secondParentInfo = parents[1].Split('%');
                var secondParentName = secondParentInfo[0];
                var secondParentBirthyear = int.Parse(secondParentInfo[1]);
                person.Parents.Add(Person.FindPerson(secondParentName, secondParentBirthyear, familyTree));
                Person.FindPerson(secondParentName, secondParentBirthyear, familyTree).Children.Add(person);
            }
        }

        private static bool ValidateInput(string input)
        {
            var people = input.Split('/');

            if (people[0].Length == 0)
                return false;

            if (people[1][0] != '#' && people[1][0] != '¤')
                return false;

            if (!people[1].Contains('%') || !people[2].Contains('%'))
                return false;

            return true;
        }

        private static void ShowRelations(string input, List<Person> familyTree)
        {
            Console.WriteLine("ShowingRelations!");
            Console.WriteLine("input: " + input);
            var personInfo = input.Split('%');
            Console.WriteLine("personInfo[0] : " + personInfo[0].Remove(0,1));
            Console.WriteLine("personInfo[1] : " + personInfo[1]);
            var person = Person.FindPerson(personInfo[0].Remove(0, 1), Convert.ToInt32(personInfo[1]), familyTree);

//            Console.WriteLine("Parents : " + person.Parents[0] + " and " + person.Parents[1]);
            Console.WriteLine("person.Parents.Count : " + person.Parents.Count);
            Console.WriteLine("Partner : " + person.Partner);
            for (var i = 0; i < person.Children.Count; i++)
                Console.WriteLine("Child " + (i + 1) + " : " + person.Children[i]);
            Console.WriteLine("---------------------");
        }
    }
}