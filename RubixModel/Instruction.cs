using System;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CS_homecopyofcoursework.RubixModel
{

    public class Instruction
    {
        public List<string> Scramblinginstructions = new List<string>();
        private Stack<string> AntiInstructions = new Stack<string>();
        public List<string> Solvinginstructions = new List<string>();
        public int currentIndex;
        private string CurrentInstruction;
        public string errorMessageForInstruction;
        public string announcment;
        public bool isSolvingListComplete = false; 
        Random randomNumber = new Random();
        string[] possibleMoves = new string[] { };
        Dictionary<string, string> possibleMovesAndAntiMoves;

        public Instruction()
        {
            errorMessageForInstruction = "";
            CurrentInstruction = "";
            currentIndex = 0;
            possibleMoves = new string[] { "R", "R'", "L", "L'", "U", "U'", "D", "D'" };
                                           // "B", "B'" ,"F", "F'" };

            possibleMovesAndAntiMoves = new Dictionary<string, string> //a dictionary of moves and their antimoves 
            {
                { "R", "R'" },
                { "R'", "R" },
                { "L", "L'" },
                { "L'", "L" },
                { "U", "U'" },
                { "U'", "U" },
                { "D", "D'" },
                { "D'", "D" },
                //{ "B", "B'" },
                //{ "B'", "B" },
                //{ "F", "F'" },
                //{ "F'", "F" },
                //{ "F2", "F2" }, //any move 2 is the same it is 'immune' to the antimove 
                //{ "F'2", "F'2" }, //these antimoves are only used when the cube class is identifying the anti-move of the current 
                { "D2", "D2" },//instruction so it can be undone. 
                { "D'2", "D'2" },
                { "R2", "R2" },
                { "R'2", "R'2" },
                { "L2", "L2" },
                { "L'2", "L'2" },
                //{ "B2", "B2" },
                //{ "B'2", "B'2" },
                { "U2", "U2" },
                { "U'2", "U'2" },
            };

        }
        public string GetCurrentInstruction()
        {
            if (Solvinginstructions.Count > 0) //if theres actually any instructions in the queue right now
            {
                CurrentInstruction = Solvinginstructions.ElementAt(currentIndex);
                return CurrentInstruction;

            }
            return null;
        }
        public int GetCurrentIndex()
        {
            return currentIndex;
        }
        public void GetNextInstruction()
        {
            if (currentIndex + 1 < Solvinginstructions.Count)//prevent index out of range
            {
                if (currentIndex == (Solvinginstructions.Count / 2) -1 ) //if we are in the middle position of the queue of solving instructions
                {
                    announcment = " Half-way there! "; //to motivate user 
                }
                else
                {
                    announcment = "";
                }
                currentIndex++;
                CurrentInstruction = Solvinginstructions.ElementAt(currentIndex);
                errorMessageForInstruction = ""; //line that was added
            }
            
            else
            {
                isSolvingListComplete = true; //sets that the set of solving instructions has been completed. 
                announcment = "Congratulations! You have solved the cube! ";
            }

        }
        public void GetPreviousInstruction()
        {
            if (currentIndex - 1 >= 0)
            {
                currentIndex--;
                CurrentInstruction = Solvinginstructions.ElementAt(currentIndex);
                errorMessageForInstruction = ""; //line that was added
            }
            else
            {
                errorMessageForInstruction = "No more instructions to undo ";
            }

        }
        private void ConjoiningInstructions() //run length encoding technique
        {
            for (int i = 1; i < Solvinginstructions.Count(); i++)
            {
                if (Solvinginstructions[i - 1] == Solvinginstructions[i]) //if the next instruction equals the previous
                {
                    Solvinginstructions[i - 1] = Solvinginstructions[i - 1] + "2";
                    Solvinginstructions.RemoveAt(i);
                }
            }
        }
        public void CreateScrambleInstructions(string levelOfDifficulty, int number) //generates the appropiate scrambling algorithm depending on difficulty level 
        {
            switch (levelOfDifficulty)
            {
                case "easy":
                    EasyScramble();
                    break;
                case "medium":
                    MediumScramble();
                    break;
                case "hard":
                    HardScramble();
                    break;
                case "custom":
                    CustomScramble(number); //number of instructions to create
                    break;
            }
        }
        private void EasyScramble()
        {
            //the premise is to select a random amount of instructions and recursively produce a list of instructions 
            int maxNumberOfInstructions = randomNumber.Next(8, 13); //a random number of instructions varying from 8 to 13 for an easy scramble
            RecursiveScramble(maxNumberOfInstructions);
        }
        private void MediumScramble()
        {
            int maxNumberOfInstructions = randomNumber.Next(18, 23); //a random number of instructions varying from 18 to 23 for a medium scramble
            RecursiveScramble(maxNumberOfInstructions);
        }
        private void HardScramble()
        {
            int maxNumberOfInstructions = randomNumber.Next(28, 33); //a random number of instructions varying from 28 to 33 for a hard scramble
            RecursiveScramble(maxNumberOfInstructions);
        }
        private void CustomScramble(int number)
        {
            RecursiveScramble(number); //recursively creates a scrambling sequence using the number of instructions inputted 
        }
        private List<string> RecursiveScramble(int numberOfInstructions) 
        {
            int randomInstruction;
            if (numberOfInstructions == 0) //basecase- if ther are no more instructions to produce
            {
                return Scramblinginstructions; //return scrambling instructions 
            }
            else
            {
                randomInstruction = randomNumber.Next(0, possibleMoves.Length);
                Scramblinginstructions.Add(possibleMoves[randomInstruction]);
                return RecursiveScramble(numberOfInstructions - 1); //return the scrambling function take away 1 
            }
        }
        public string findingAntiMove(string move)
        {
            string antiMove = possibleMovesAndAntiMoves[move];
            return antiMove;
        }
        private void AntiMove(List<string> instructions)
        {
            foreach (string move in instructions)
            {
                AntiInstructions.Push(findingAntiMove(move));
            }
        }

        private void solvingInstructions()
        {
            int howManyAntiMoves = AntiInstructions.Count(); //if we dont do this here, the count decreases as the for loop continues
            for (int antiMove = 0; antiMove < howManyAntiMoves; antiMove++) //and it will end prematurely
            {
                Solvinginstructions.Add(AntiInstructions.Pop());
            }
        }
        public List<string> GetScramblingInstruction(string levelOfDifficulty, int number)
        {
            CreateScrambleInstructions(levelOfDifficulty, number);
            return Scramblinginstructions;
        }

        public List<string> GetSolvingInstruction()
        {
            AntiMove(Scramblinginstructions);
            solvingInstructions();
            ConjoiningInstructions();
            return Solvinginstructions;
        }

    }


}

