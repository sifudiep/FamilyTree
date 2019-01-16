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
            Console.WriteLine("By writing the NAME seperated by the parents");
            Console.WriteLine("I.e : 'David/Linda/Margaret&Per");
            Console.WriteLine("In this example David is the new Person to be added,Linda is Davids Partner and Margaret and Per are Davids parents.");
            Console.WriteLine("If David is the first known generation of the family, simply dont add the slash(/) to parents");
            Console.WriteLine("to register David as the oldest known generation of the family.");
        }
        static void Finished(List<Person> familyTree)
        {
            Console.Clear();
            Console.WriteLine("Finished!");
        }

        static bool ValidateInput(string input)
        {
            int slashCounter = 0;
            int andCounter = 0;
            bool firstGeneration = false;
            for (int i = 0; i < input.Length; i++) {
                if (input[i] == '/') {
                    slashCounter++;
                } else if (input[i] == '&') {
                    andCounter++;
                }
            }

            if (slashCounter < 2 || slashCounter > 0)
            {
                firstGeneration = true;
                Console.WriteLine("First Generation!");
                return true;
            }

            if (slashCounter > 2)
            {
                Console.WriteLine("SlashCounter > 2");
                return false;
            }

            if (andCounter == 0) {
                Console.WriteLine("andCounter == 0");
                return false;
            }

            if (andCounter != 1) {
                Console.WriteLine("andCounter is not 1");
                return false;
            }

            var people = input.Split('/');
            if (people[0].Length == 0)
            {
                Console.WriteLine("people[0].Length == 0");
                return false;
            }

            if (people[1].Length == 0)
            {
                Console.WriteLine("people[1].Length== 0");    
            }
            
            if (!firstGeneration)
            {
                var parents = people[2].Split('&');
                if (parents[0].Length == 0 || parents[1].Length == 0)
                {
                    Console.WriteLine("parents[0].Length == 0 || parents[1].Length == 0");
                    return false;                     
                }
            }
            
            return true;
        }

        static void AddToFamilyTree(List<Person> familyTree, string input)
        {
            var people = input.Split('/');
            var person = people[0];
            var partner = people[1];
            var parents = people[2].Split('&');
            
            var newFamilyMember = new Person();
            newFamilyMember.Name = person;

            for (int i = 0; i < familyTree.Count; i++) {
                if (familyTree[i].Name == parents[0] || familyTree[i].Name == parents[1]) {
                    newFamilyMember.Parents.Add(familyTree[i]);
                }
            }

            for (int i = 0; i < familyTree.Count; i++)
            {
                if (familyTree[i].Name == parents[0] || familyTree[i].Name == parents[1])
                {
                    familyTree[i].Children.Add(newFamilyMember);
                }
            }
        }
    }
}
