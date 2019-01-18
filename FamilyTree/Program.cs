using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTree
{
    class Program
    {
        /*
         *    #Partner
         *    ¤Parents
         *    %Birthyear
         */
        static void Main(string[] args)
        {
            var FamilyTree = new List<Person>();
            var notFinished = true;
            while (notFinished)
            {
                Console.WriteLine("--------------------------------------------");
                Console.WriteLine("Type '!help' for application instructions");
                Console.WriteLine("Type '!finished' to finish the family tree");
                var input = Console.ReadLine();
                if (input == "!help") {
                    Instructions();
                } else if (input == "!finished") {
                    Finished(FamilyTree);
                } else {
                    if (ValidateInput(input)) {
                        Console.WriteLine("ValidateInput was a success!");
                    }
                    else
                    {
                        Console.WriteLine("Input string was not typed in the right format, type !help for the correct input format.");
                    }
                }

            }
        }
        
        static void Instructions()
        {
            Console.Clear();
            Console.WriteLine("Add a person to the family tree!");
            Console.WriteLine("By writing the NAME separated by the parents");
            Console.WriteLine("I.e : 'David/#Linda%1993/¤Margaret%1970&Per%1965");
            Console.WriteLine("In this example David is the new Person to be added,Linda is Davids Partner and she's born in 1993.");
            Console.WriteLine("If David is the first known generation of the family, simply dont add the '/¤Margaret%1970&Per%1965' input");
            Console.WriteLine("to register David as the oldest known generation of the family.");
        }
        static void Finished(List<Person> familyTree)
        {
            Console.Clear();
            Console.WriteLine("Finished!");
        }

        static Person AddToFamilyTree(string input, List<Person> familyTree)
        {
            var person = new Person();
            var people = input.Split('/');

            AddParentsAndPartner(people[1], person, familyTree);
            AddParentsAndPartner(people[2], person, familyTree);
            
            return person;
        }

        static void AddParentsAndPartner(string people, Person person, List<Person> familyTree )
        {
            if (people[0] == '#')
            {
                var partnerInfo = people.Split('%');
                var partnerName = partnerInfo[0];
                var partnerBirthyear = Int32.Parse(partnerInfo[1]);
                person.Partner = Person.FindPerson(partnerName, partnerBirthyear, familyTree);
                person.Partner.Partner = person;
            }
            else if (people[0] == '¤')
            {
                var parents = people.Split('&');
                    
                var firstParentInfo = parents[0].Split('%');
                var firstParentName = firstParentInfo[0].Remove(0,1);
                var firstParentBirthyear = Int32.Parse(firstParentInfo[1]);
                person.Parents.Add(Person.FindPerson(firstParentName, firstParentBirthyear, familyTree));
                Person.FindPerson(firstParentName, firstParentBirthyear, familyTree).Children.Add(person);

                var secondParentInfo = parents[1].Split('%');
                var secondParentName = secondParentInfo[0];
                var secondParentBirthyear = Int32.Parse(secondParentInfo[1]);
                person.Parents.Add(Person.FindPerson(secondParentName, secondParentBirthyear, familyTree));
                Person.FindPerson(secondParentName, secondParentBirthyear, familyTree).Children.Add(person);
            }
        }
        
        static bool ValidateInput(string input)
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
    }
}
