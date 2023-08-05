using System;
using System.Diagnostics;
using System.Drawing;
using static System.Formats.Asn1.AsnWriter;

namespace CS_homecopyofcoursework.RubixModel
{
    public class Cube
    {
        private string? message;

        public List<Face> faceList = new();

        public int currentFace;

        public string CubieInput { get; set; }

        public string cubeShow { get; set; }

        public string levelOfDifficulty { get; set; }

        public int NumberOfMoves { get; set; }

        private string errorMessageFortheSameColourEntered;
        private string errorMessageForCorner;
        private string errorMessageForEdge;
        private string errorMessageForInput;
        private string errorMessageForNumberOfMovesInput;
        private string announcment;
        private string errorMessageForInstruction;

        public int CurrentOrientation;

        Dictionary<string, int[]> DisplayAfterSeeingAdjacentFaces;
        Dictionary<string, int[]> UpdateAllFacesCurrentOrientation;
        Dictionary<int, string> whichFaceToShow;
        private Dictionary<string, Action> movesToPerform = new Dictionary<string, Action>();
        private string[,] faceToShow = new string[3, 3];

        public Instruction instruction = new Instruction(); // Add this line to create a new instance of the Instruction class
        List<string> solvingInstructions;

        //validation
        public string ErrorMessageForNumberOfColours
        {
            get { return errorMessageFortheSameColourEntered; }
            set { errorMessageFortheSameColourEntered = value; }
        }
        public string ErrorMessageForCorner
        {
            get { return errorMessageForCorner; }
            set { errorMessageForCorner = value; }
        }
        public string ErrorMessageForEdge
        {
            get { return errorMessageForEdge; }
            set { errorMessageForEdge = value; }
        }
        public string ErrorMessageForInput
        {
            get { return errorMessageForInput; }
            set { errorMessageForInput = value; }
        }
        public string Announcments
        {
            get { return announcment; }
            set { announcment = value; }
        }
        public string ErrorMessageForNumberOfMovesInput
        {
            get { return errorMessageForNumberOfMovesInput; }
            set { errorMessageForNumberOfMovesInput = value; }
        }
        public string ErrorMessageForInstruction
        {
            get { return errorMessageForInstruction; }
            set { errorMessageForInstruction = value; }
        }
        //Instantiation of the cube class
        public Cube()
        {
            // For testing
            message = $"Initialized at {DateTime.Now}";
            currentFace = 0;
            CurrentOrientation = 0;
            errorMessageFortheSameColourEntered = "";
            errorMessageForCorner = "";
            errorMessageForEdge = "";
            announcment = "";

            // Creates blank Faces (waiting for user input) and adds to list
            faceList.Add(new Face("white", 0));
            faceList.Add(new Face("white", 0));
            faceList.Add(new Face("white", 0));
            faceList.Add(new Face("white", 0));
            faceList.Add(new Face("white", 0));
            faceList.Add(new Face("white", 2)); 

            //Setup face to show
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    faceToShow[r, c] = faceList[currentFace].face[r, c];
                }
            }


            //a dictionary which says which face and orientation to show after each turn 
            DisplayAfterSeeingAdjacentFaces = new Dictionary<string, int[]>
            {
                { "00", new int[] { 1, 0, 2, 0, 4, 0, 3, 0 } },//key then see left, right, up, down
                { "10", new int[] { 5, 2, 0, 0, 4, 0, 3, 0 } },
                { "20", new int[] { 0, 0, 5, 2, 4, 0, 3, 0 } },
                { "30", new int[] { 1, 0, 2, 0, 0, 0, 5, 2 } },
                { "40", new int[] { 1, 0, 2, 0, 5, 0, 0, 0 } },
                { "50", new int[] { 1, 0, 2, 0, 3, 0, 4, 0 } },
                { "52", new int[] { 2, 0, 1, 0, 4, 0, 3, 0 } }
            };

            whichFaceToShow = new Dictionary<int, string>
            {
                { 0, "show-front" },
                { 1, "show-left" },
                { 2, "show-right" },
                { 3, "show-bottom" },
                { 4, "show-top" },
                { 5, "show-back" }
            };

            UpdateAllFacesCurrentOrientation = new Dictionary<string, int[]>
            {
                { "00", new int[] { 0, 0, 1, 0, 2, 0, 3, 0, 4, 0, 5, 2 } }, //how the user is viewing- helps the rotations! 
                { "10", new int[] { 0, 0, 1, 0, 2, 0, 3, 3, 4, 1, 5, 2 } },
                { "20", new int[] { 0, 0, 1, 0, 2, 0, 3, 1, 4, 3, 5, 2 } },
                { "30", new int[] { 0, 0, 1, 1, 2, 3, 3, 0, 4, 2, 5, 0 } },
                { "40", new int[] { 0, 0, 1, 3, 2, 1, 3, 2, 4, 0, 5, 0 } },
                { "52", new int[] { 0, 0, 1, 0, 2, 0, 3, 2, 4, 2, 5, 2 } }
            }; //these are the only way the user can view each face, update each relevent face based on 

            movesToPerform = new Dictionary<string, Action>
            {
                {"R", RotateRightClockwise},
                {"R'", RotateRightAntiClockwise},
                {"L", RotateLeftClockwise},
                {"L'", RotateLeftAntiClockwise},
                {"U", RotateUpClockwise},
                {"U'", RotateUpAntiClockwise},
                {"D", RotateDownClockwise},
                {"D'", RotateDownAntiClockwise},
                {"B", RotateBackClockwise},
                {"B'", RotateBackAntiClockwise},
                {"F", RotateFrontClockwise},
                {"F'", RotateFrontClockwise},
                {"F2", RotateFrontHalf},
                {"B2", RotateBackHalf},
                {"L2", RotateLeftHalf},
                {"R2", RotateRightHalf},
                {"U2", RotateUpHalf},
                {"D2", RotateDownHalf},
                {"F'2", RotateFrontHalf},
                {"B'2", RotateBackHalf},
                {"L'2", RotateLeftHalf},
                {"R'2", RotateRightHalf},
                {"U'2", RotateUpHalf},
                {"D'2", RotateDownHalf},
            };
        }

        public string[,] DisplayFace()
        {
            //test code
            for (int r = 0; r < 3; r++)
            {
                for (int c = 0; c < 3; c++)
                {
                    faceToShow[r, c] = faceList[currentFace].face[r, c];
                }
            }
            //Return to display
            return faceToShow;

        }

        //Here I am going to create the different rotations for the cube based on its current orientation and current face 
        public void RotateLeftClockwise()
        {
            //Face placeholders for updated cubies
            string[] face0 = faceList[0].getColumn(0,false);
            string[] face1 = faceList[4].getColumn(0,false);
            string[] face2 = faceList[5].getColumn(0,false);
            string[] face3 = faceList[3].getColumn(0,false);
            faceList[0].UpdateNewColumn(face3, 0);
            faceList[4].UpdateNewColumn(face0, 0);
            faceList[5].UpdateNewColumn(face1, 0);
            faceList[3].UpdateNewColumn(face2, 0);
            faceList[2].matrixRotationClockwise();
        }
        public void RotateLeftAntiClockwise()
        {
            string[] face0 = faceList[0].getColumn(0,false);
            string[] face1 = faceList[4].getColumn(0,false);
            string[] face2 = faceList[5].getColumn(0,false);
            string[] face3 = faceList[3].getColumn(0,false); 
            faceList[0].UpdateNewColumn(face1, 0);
            faceList[4].UpdateNewColumn(face2, 0);
            faceList[5].UpdateNewColumn(face3, 0);
            faceList[3].UpdateNewColumn(face0, 0);
            faceList[2].matrixRotationAntiClockwise();
        }

        public void RotateRightClockwise()
        {
            string[] face0 = faceList[0].getColumn(2,false);
            string[] face1 = faceList[4].getColumn(2,false);
            string[] face2 = faceList[5].getColumn(2,false);
            string[] face3 = faceList[3].getColumn(2,false);
            faceList[0].UpdateNewColumn(face3, 2);
            faceList[4].UpdateNewColumn(face0, 2);
            faceList[5].UpdateNewColumn(face1, 2);
            faceList[3].UpdateNewColumn(face2, 2); 
            faceList[2].matrixRotationClockwise();
        }
        public void RotateRightAntiClockwise()
        {
            string[] face0 = faceList[0].getColumn(2,false);
            string[] face1 = faceList[4].getColumn(2,false);
            string[] face2 = faceList[5].getColumn(2,false);
            string[] face3 = faceList[3].getColumn(2,false); 
            faceList[0].UpdateNewColumn(face1, 2);
            faceList[4].UpdateNewColumn(face2, 2);
            faceList[5].UpdateNewColumn(face3, 2);
            faceList[3].UpdateNewColumn(face0, 2);
            faceList[2].matrixRotationAntiClockwise();
        }
        public void RotateFrontClockwise()
        {
            string[] face0 = faceList[1].getColumn(2, true); //face 1 column 2 backwards 
            string[] face1 = faceList[2].getColumn(0, true);//face 2 column 0 backwards
            string[] face2 = faceList[3].getRow(0, false); //get row 0 of face 3 
            string[] face3 = faceList[4].getRow(2, false);//get row 2 of the 4th face
            faceList[1].UpdateNewColumn(face2, 2);
            faceList[2].UpdateNewColumn(face3, 0);
            faceList[3].UpdateNewRow(face1, 0);
            faceList[4].UpdateNewRow(face0, 2);
            faceList[0].matrixRotationClockwise(); 
        }
        public void RotateFrontAntiClockwise()
        {
            string[] face0 = faceList[1].getColumn(2, false); //get face 1 column 2 
            string[] face1 = faceList[2].getColumn(0, false);//get face 2 column 0
            string[] face2 = faceList[3].getRow(0, true); //get row 0 of face 3 (backwards)  
            string[] face3 = faceList[4].getRow(2, true);//get row 2 of the 4th face (backwards) 
            faceList[1].UpdateNewColumn(face3, 2);
            faceList[2].UpdateNewColumn(face2, 0);
            faceList[3].UpdateNewRow(face0, 0);
            faceList[4].UpdateNewRow(face1, 2);
            faceList[0].matrixRotationAntiClockwise();
        }
        public void RotateBackClockwise()
        {
            string[] face0 = faceList[1].getColumn(0, true); //face 1 column 0 backwards 
            string[] face1 = faceList[2].getColumn(2, true);//face 2 column 2 backwards
            string[] face2 = faceList[3].getRow(2, false); //get row 2 of face 3 
            string[] face3 = faceList[4].getRow(0, false);//get row 0 of the 4th face
            faceList[1].UpdateNewColumn(face2, 0);
            faceList[2].UpdateNewColumn(face3, 2);
            faceList[3].UpdateNewRow(face1, 2);
            faceList[4].UpdateNewRow(face0, 0);
            faceList[5].matrixRotationClockwise();
        }
        public void RotateBackAntiClockwise()
        {
            string[] face0 = faceList[1].getColumn(0, false); //get face 1 column 0
            string[] face1 = faceList[2].getColumn(2, false);//get face 2 column 2
            string[] face2 = faceList[3].getRow(2, true); //get row 2 of face 3 (backwards)
            string[] face3 = faceList[4].getRow(0, true);//get row 0 of the 4th face (backwards)
            faceList[1].UpdateNewColumn(face3, 0);
            faceList[2].UpdateNewColumn(face2, 2);
            faceList[3].UpdateNewRow(face0, 2);
            faceList[4].UpdateNewRow(face1, 0);
            faceList[5].matrixRotationAntiClockwise();
        }
        public void RotateUpClockwise()
        {
            string[] face0 = faceList[0].getRow(0,false);//face 0 row 0 
            string[] face1 = faceList[1].getRow(0,true); //face 1 row 0 backwards
            string[] face2 = faceList[2].getRow(0,false); //face 2 row 0 
            string[] face3 = faceList[5].getRow(2,true); //face 5 row 2 backwards
            faceList[0].UpdateNewRow(face2, 0);
            faceList[1].UpdateNewRow(face0, 0);
            faceList[2].UpdateNewRow(face3, 0);
            faceList[5].UpdateNewRow(face1, 2);
            faceList[4].matrixRotationClockwise();

        }
        public void RotateUpAntiClockwise()
        {
            string[] face0 = faceList[0].getRow(0,false); //face 0 row 0 
            string[] face1 = faceList[1].getRow(0,false); //face 1 row 0 
            string[] face2 = faceList[2].getRow(0,true); //face 2 row 0 (backwards) 
            string[] face3 = faceList[5].getRow(2,true); //face 5 row 2 (backwards)
            faceList[0].UpdateNewRow(face1, 0);
            faceList[1].UpdateNewRow(face3, 0);
            faceList[2].UpdateNewRow(face0, 0);
            faceList[5].UpdateNewRow(face2, 2);
            faceList[4].matrixRotationAntiClockwise();
        }
        public void RotateDownClockwise()
        {
            string[] face0 = faceList[0].getRow(2,false);//face 0 row 2
            string[] face1 = faceList[1].getRow(2,true);//face 1 row 2 (backwards)
            string[] face2 = faceList[2].getRow(2,false);//face 2 row 2 
            string[] face3 = faceList[5].getRow(0,true);//face 5 row 0 (backwards)
            faceList[0].UpdateNewRow(face2,  2);
            faceList[1].UpdateNewRow(face0,  2);
            faceList[2].UpdateNewRow(face3,  2);
            faceList[5].UpdateNewRow(face1,  0);
            faceList[3].matrixRotationClockwise();
        }
        public void RotateDownAntiClockwise()
        {
            string[] face0 = faceList[0].getRow(2,false); //face 0 row 2
            string[] face1 = faceList[1].getRow(2,false); //face 1 row 2 
            string[] face2 = faceList[2].getRow(2,true); //face 2 row 2 (backwards) 
            string[] face3 = faceList[5].getRow(0,true);//face 5 row 0 (backwards)
            faceList[0].UpdateNewRow(face1,  2);
            faceList[1].UpdateNewRow(face3,  2);
            faceList[2].UpdateNewRow(face0,  2);
            faceList[5].UpdateNewRow(face2,  0);
            faceList[3].matrixRotationAntiClockwise();
        }
        public void RotateRightHalf()
        {
            RotateRightClockwise();
            RotateRightClockwise();
        }

        public void RotateLeftHalf()
        {
            RotateLeftClockwise();
            RotateLeftClockwise();
        }

        public void RotateUpHalf()
        {
            RotateUpClockwise();
            RotateUpClockwise();
        }
        public void RotateDownHalf()
        {
            RotateDownClockwise();
            RotateDownClockwise();
        }
        public void RotateBackHalf()
        {
            RotateBackClockwise();
            RotateBackClockwise();
        }

        public void RotateFrontHalf()
        {
            RotateFrontClockwise();
            RotateFrontClockwise();
        }


        public int[] DisplayAdjacentFace(string key)
        {
            return DisplayAfterSeeingAdjacentFaces[key];
        }

        public int[] FindingCurrentOrientation(string key)
        {
            return UpdateAllFacesCurrentOrientation[key];
        }
        public string WhichFaceToShow(int key)
        {
            return whichFaceToShow[key];
        }
        public Action WhichMoveToPerform(string key)
        {
            return movesToPerform[key];
        }

        public void UpdateAllFacesAndTheirOrientation()
        {
            string whatFaceAreWeCurrentlyDisplaying = faceList[currentFace].ReturnCurrentFaceAndOrientation(currentFace, CurrentOrientation);
            int[] allOtherFacesOrientation = FindingCurrentOrientation(whatFaceAreWeCurrentlyDisplaying);
            // Loop through each face and update its orientation
            for (int i = 0; i < allOtherFacesOrientation.Length; i += 2)
            {
                int FaceIndex = allOtherFacesOrientation[i];
                int FaceOrientation = allOtherFacesOrientation[i + 1];
                // Update the orientation of the face we are at through functions in the face class 
                faceList[FaceIndex].UpdateFaceOrientation(FaceOrientation);
            }
        }

        public string faceToDisplay()
        {
            string whichFaceToDisplay = WhichFaceToShow(currentFace);
            return whichFaceToDisplay;
        }

        public void SeeFrontFace()
        {
            currentFace = 0;
            CurrentOrientation = 0;
            UpdateAllFacesAndTheirOrientation();
        }
        public void SeeBackFace()
        {
            currentFace = 5;
            CurrentOrientation = 2;
            UpdateAllFacesAndTheirOrientation();
        }
        public void SeeLeftFace()
        {
            currentFace = 1;
            CurrentOrientation = 0;
            UpdateAllFacesAndTheirOrientation();
        }
        public void SeeRightFace()
        {
            currentFace = 2;
            CurrentOrientation = 0;
            UpdateAllFacesAndTheirOrientation();
        }
        public void SeeTopFace()
        {
            currentFace = 4;
            CurrentOrientation = 0;
            UpdateAllFacesAndTheirOrientation();
        }
        public void SeeBottomFace()
        {
            currentFace = 3;
            CurrentOrientation = 0;
            UpdateAllFacesAndTheirOrientation();
        }


        //validation techniques start here 
        //Subroutine to recieve user input to create a face
        public void CreateFace(string[,] faceInput, int currentFace)
        {
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {

                    faceList[currentFace].face[row, col] = faceInput[row, col];
                }
            }
        }

        public bool IsCubeSolvable()
        {
            bool Solvable = true;

            if (ValidateCornerCubies() == false)
            {
                Solvable = false;
            }
            if (ValidateEdgeCubies() == false)
            {
                Solvable = false;
            }
            if (Solvable == true)
            {
                announcment = "Solvable Cube Entered";
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool ValidateSameColourMoreThanOnce(List<string> faceInput) //checks if the user has entered the same colour face more than once 
        {
            int numberofSameColours = 0;
            string sameColourErrorMessage = "";
            List<string> FaceColoursInLowerCase = ConvertToLower(faceInput); //converts all values to lower case
            for (int colours = 1; colours < FaceColoursInLowerCase.Count; colours++) //loops through all the values in the list 
            {
                for (int i = colours - 1; i >= 0; i--)
                {
                    if (FaceColoursInLowerCase[colours] == FaceColoursInLowerCase[i]) //compares the current colour with each previous value to see if there are any comparisons
                    {
                        numberofSameColours++; //adds one if the colours are the same
                        sameColourErrorMessage += $" You entered {FaceColoursInLowerCase[colours]} more than once. \n"; //to create a useful error message
                    }
                }

            }
            if (numberofSameColours == 0) //returns true if all the colours are unique
            {
                errorMessageFortheSameColourEntered = "";
                return true;
            }
            else
            {
                errorMessageFortheSameColourEntered = "Invalid: " + sameColourErrorMessage;
                return false;
            }
        }

        public void SubmitFace()
        {
            List<string> FaceInputInListFormat = new List<string>(CubieInput.Split(','));
            if (ValidNumberOfFacesInputted(FaceInputInListFormat) && ValidFaceColourInputted(FaceInputInListFormat) && ValidateSameColourMoreThanOnce(FaceInputInListFormat))
            {
                PutUserInputIntoCube(FaceInputInListFormat);
                CubieInput = "";  // reset the input string
                errorMessageForInput = "";  // clear the error message
            }
        }

        public bool ValidNumberOfFacesInputted(List<string> faceInput)
        {
            if (faceInput.Count != 6)
            {
                errorMessageForInput = "Invalid input. Please may you enter 6 valid face colours seperated by commas and try again. ";
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool ValidFaceColourInputted(List<string> faceInput)
        {
            List<string> FaceColoursInLowerCase = ConvertToLower(faceInput); //converts all values to lower case
            List<string> ValidCubieColours = new List<string>() { "red", "green", "yellow", "blue", "white", "orange" };
            IEnumerable<string> unionOfColours = FaceColoursInLowerCase.Union(ValidCubieColours); //finds all the common values between the user input and set colours
            if (unionOfColours.Count() == ValidCubieColours.Count) //if the number of colours in the union is equal to the number of colours in the valid set
            {
                return true;  //it is valid
            }
            else
            {
                errorMessageForInput = "Invalid colour(s). Please re-enter one of the following colours: red, green, blue, yellow, orange, or white and try again";
                return false;
            }
        }
        private void PutUserInputIntoCube(List<string> faceInput)
        {
            List<string> FaceColoursInLowerCase = ConvertToLower(faceInput); //converts all values to lower case
            for (int face = 0; face < faceList.Count; face++) //goes through each face and updates the colours depending on user input
            {
                faceList[face].colour = FaceColoursInLowerCase[face];
                for (int row = 0; row < 3; row++) //loops through each element in the array and updates the colour to the colour inputted
                {
                    for (int col = 0; col < 3; col++)
                    {
                        faceList[face].face[row, col] = FaceColoursInLowerCase[face];
                    }
                }
            }
            errorMessageForInput = "";

        }
        private List<string> ConvertToLower(List<string> cubieInput)
        {
            List<string> CubieInputLowerCase = new List<string>();
            for (int i = 0; i < cubieInput.Count; i++)
            {
                CubieInputLowerCase.Add(cubieInput[i].ToLower());
            }
            return CubieInputLowerCase;
        }

        public string[,] GetFace()
        {
            return faceList[currentFace].face;
        }

        public bool ValidateCornerCubies()
        {
            List<string[]> currentCornerCubieColours = new List<string[]>
            {
                new string[] { faceList[0].face[0, 0], faceList[1].face[0, 2], faceList[4].face[2, 0] }.OrderBy(x => x).ToArray(), // top left front in alphabetical order
                new string[] { faceList[0].face[0, 2], faceList[2].face[0, 0], faceList[4].face[2, 2] }.OrderBy(x => x).ToArray(), // top right front
                new string[] { faceList[0].face[2, 0], faceList[1].face[2, 2], faceList[3].face[0, 0] }.OrderBy(x => x).ToArray(), // bottom left front
                new string[] { faceList[0].face[2, 2], faceList[2].face[2, 0], faceList[3].face[0, 2] }.OrderBy(x => x).ToArray(), // bottom right front
                new string[] { faceList[5].face[0, 0], faceList[4].face[0, 2], faceList[2].face[0, 2] }.OrderBy(x => x).ToArray(), // top left back
                new string[] { faceList[5].face[0, 2], faceList[4].face[0, 0], faceList[1].face[0, 0] }.OrderBy(x => x).ToArray(), // top right back
                new string[] { faceList[5].face[2, 0], faceList[3].face[2, 2], faceList[2].face[2, 2] }.OrderBy(x => x).ToArray(), // bottom left back
                new string[] { faceList[5].face[2, 2], faceList[3].face[2, 0], faceList[1].face[2, 0] }.OrderBy(x => x).ToArray() // bottom right back
            };

            // Sort the valid corner colours array in alphabetical order
            int numberOfValidCorners = 0;
            string invalidCornerCubieColours = "";
            bool allCornersValid = true;
            List<string[]> validCornerCubieColours = new List<string[]>() {
                new string[] {"green", "orange", "white"},
                new string[] {"green", "orange", "yellow"},
                new string[] {"green", "red", "yellow"},
                new string[] {"green", "red", "white"},
                new string[] {"blue", "orange", "white"},
                new string[] {"blue", "orange", "yellow"},
                new string[] {"blue", "red", "yellow"},
                new string[] {"blue", "red", "white"}
         }.Select(a => a.OrderBy(x => x).ToArray()).ToList(); // Sort each array in alphabetical order :)

            // Extract the colours of each corner cubie from the user input

            for (int i = 0; i < validCornerCubieColours.Count; i++)
            {
                bool isValid = false;

                for (int j = 0; j < currentCornerCubieColours.Count; j++)
                {
                    if (currentCornerCubieColours[j].SequenceEqual(validCornerCubieColours[i])) //comparing each element in the valid list to the inputted list
                    {
                        isValid = true;
                        numberOfValidCorners++;
                        break; // Exit the inner loop once a match is found
                    }
                }

                if (!isValid)
                {
                    allCornersValid = false;
                    invalidCornerCubieColours += $" You entered an invalid corner consisting of the colours {currentCornerCubieColours[i][0]}, {currentCornerCubieColours[i][1]}, {currentCornerCubieColours[i][2]} . \n";
                }

                if (numberOfValidCorners == 8)
                {
                    break;

                }
            }

            if (numberOfValidCorners == 8) //return true if there are 8 valid corners 
            {
                errorMessageForCorner = "";
                return true;
            }
            else
            {
                errorMessageForCorner = "Invalid input: " + invalidCornerCubieColours;
                return false;
            }

        }

        public bool ValidateEdgeCubies()
        {

            // Extract the colours of each edge cubie
            List<string[]> currentEdgeCubieColours = new List<string[]>
            {
                new string[] { faceList[0].face[0, 1], faceList[4].face[2, 1] }.OrderBy(x => x).ToArray(), // front top alphabetically (like corners) 
                new string[] { faceList[0].face[1, 2], faceList[2].face[1, 0] }.OrderBy(x => x).ToArray(), // front right
                new string[] { faceList[0].face[2, 1], faceList[3].face[0, 1] }.OrderBy(x => x).ToArray(), // front bottom
                new string[] { faceList[0].face[1, 0], faceList[1].face[1, 2] }.OrderBy(x => x).ToArray(), // front left
                new string[] { faceList[1].face[0, 1], faceList[4].face[2, 1] }.OrderBy(x => x).ToArray(), // left top
                new string[] { faceList[1].face[2, 1], faceList[3].face[1, 0] }.OrderBy(x => x).ToArray(), // left bottom
                new string[] { faceList[2].face[0, 1], faceList[4].face[1, 2] }.OrderBy(x => x).ToArray(), // right top
                new string[] { faceList[2].face[2, 1], faceList[3].face[1, 2] }.OrderBy(x => x).ToArray(), // right bottom
                new string[] { faceList[5].face[0, 1], faceList[4].face[0, 1] }.OrderBy(x => x).ToArray(), // back top
                new string[] { faceList[5].face[1, 2], faceList[1].face[1, 0] }.OrderBy(x => x).ToArray(), // back right
                new string[] { faceList[5].face[1, 0], faceList[2].face[1, 2] }.OrderBy(x => x).ToArray(), // back left
                new string[] { faceList[5].face[2, 1], faceList[3].face[2, 1] }.OrderBy(x => x).ToArray() // back bottom
            };

            List<string[]> validEdgeCubieColours = new List<string[]>() {
    new string[] {"blue", "orange"},
    new string[] {"blue", "yellow"},
    new string[] {"green", "orange"},
    new string[] {"green", "red"},
    new string[] {"green", "yellow"},
    new string[] {"red", "blue"},
   new string[] {"red", "yellow"},
    new string[] {"white", "blue"},
    new string[] {"white", "green"},
    new string[] {"white", "orange"},
    new string[] {"white", "red"},
    new string[] {"yellow", "orange"}
}.Select(a => a.OrderBy(x => x).ToArray()).ToList(); // Sort each array in alphabetical order :)


            string invalidEdgeCubieColours = "";
            int numberOfValidEdges = 0;
            bool allEdgesValid = true;
            for (int i = 0; i < currentEdgeCubieColours.Count; i++)// Check each edge cubie colour combination against the valid list, and keep count of the number of valid edges
            {
                bool isValid = false;

                for (int j = 0; j < validEdgeCubieColours.Count; j++)
                {
                    if (validEdgeCubieColours[j].SequenceEqual(currentEdgeCubieColours[i]))
                    {
                        isValid = true;
                        numberOfValidEdges++;
                        break; // Exit the inner loop once a match is found
                    }
                }

                if (!isValid)
                {
                    allEdgesValid = false;
                    invalidEdgeCubieColours += $" You entered an invalid edge consisting of the colours {currentEdgeCubieColours[i][0]}, {currentEdgeCubieColours[i][1]} . \n";
                }

                if (numberOfValidEdges == 12)
                {
                    break;
                }
            }


            if (numberOfValidEdges == 12) //return true if there are 8 valid corners 
            {
                errorMessageForEdge = "";
                return true;
            }
            else
            {
                errorMessageForCorner = "Invalid input: " + invalidEdgeCubieColours;
                return false;
            }

        }

        public bool isCubeSolved()
        {
            int numberOfSameColourCubies = 0;
            string colour = "";
            bool isSovled = true;
            for (int faceIndex = 0; faceIndex < 6; faceIndex++) //triple for loop can lead to quite a high time complexity o(n^3) but it is a small list so should be fine! 
            {
                colour = faceList[faceIndex].getMiddleCubie(); //gets the colour of what the face is meant to be. 
                for (int r = 0; r < 3; r++)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        if (faceList[faceIndex].getCubieColour(r, c) == colour)
                        {
                            numberOfSameColourCubies++;
                        }
                    }
                }
                if (numberOfSameColourCubies == 9)
                {
                    numberOfSameColourCubies = 0;
                }
                else
                {
                    isSovled = false;
                    break; //breaks the loop to prevent wasting runtime! 
                }
            }
            if (isSovled == true)
            {
                announcment = "Cube is (already) solved! ";
                return true;
            }
            else
            {
                return false;
            }
        }



        public void ApplyScrambleToCube() //apply scrambling instructions to the cube
        {
            List<string> scramblingInstructions = instruction.GetScramblingInstruction(levelOfDifficulty, NumberOfMoves);
            for (int move = 0; move < scramblingInstructions.Count(); move++)
            {
                Action moveToBePerformed = WhichMoveToPerform(scramblingInstructions[move]);
                moveToBePerformed(); //to actually call the method 
            }

            solvingInstructions = instruction.GetSolvingInstruction();
        }

        public void ApplySolverToCube()
        {
            for (int move = 0; move < solvingInstructions.Count(); move++)
            {
                Action moveToBePerformed = WhichMoveToPerform(solvingInstructions[move]);
                moveToBePerformed(); //to actually call the method 
            }
        }

        public void ApplyCurrentInstructionToCube() //everytime you click the current instruction is applied to the cube
        {
            if (instruction.isSolvingListComplete == false) //as long as the list of solving instructions has not been completed 
            {
                Action moveToBePerformed = WhichMoveToPerform(solvingInstructions[instruction.GetCurrentIndex()]);
                moveToBePerformed(); //to actually call the method 
                announcment = instruction.announcment;
                errorMessageForInstruction = instruction.errorMessageForInstruction;
            }
            else
            {
                announcment = instruction.announcment;
                errorMessageForInstruction = instruction.errorMessageForInstruction;
            }
            
        }

        public void UndoCurrentInstructionToCube()
        {
            string currentInstruction = solvingInstructions[instruction.GetCurrentIndex()]; //gets the current instruction 
            Action moveToBePerformed = WhichMoveToPerform(instruction.findingAntiMove(currentInstruction)); //gets the antimove of the current instruction 
            moveToBePerformed(); //applies the antimove
            announcment = instruction.announcment;
            errorMessageForInstruction = instruction.errorMessageForInstruction;
        }

        public void validatingNumberInput()
        {
            try
            {
                int number = NumberOfMoves;
                if (number >= 100) //if number is greater or equal to 100 
                {
                    errorMessageForNumberOfMovesInput = " Invalid: number is too big, try again ";
                }
                else if (number == 0) //if no input is given
                {
                    errorMessageForNumberOfMovesInput = " Invalid: please enter an integer like 23 and try again  ";
                }
                else //succesful 
                {
                    errorMessageForNumberOfMovesInput = "";
                    announcment = " Custom scramble submitted successfully";
                }
            }
            catch (FormatException) //catches any expception from invalid data type being entered 
            {
                errorMessageForNumberOfMovesInput = " Invalid: please enter an integer like 23 and try again  ";
            }

        }
    }


}



