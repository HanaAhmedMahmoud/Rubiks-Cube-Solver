using System;
using Microsoft.AspNetCore.Components.Forms;

namespace CS_homecopyofcoursework.RubixModel
{
    public class Face
    {
        public string[,] face = new string[3, 3];  // Create empty face

        public string colour = "";

        // Store the orientation needed to be able to display the face correctly
        public int currentOrientation;


        // Create a class constructor for the Face class
        public Face(string colour, int current)
        {
            this.colour = colour;
            this.currentOrientation = current;

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 3; col++)
                {
                    face[row, col] = colour;
                }
            }
        }

        public void changeOne(int x, int y)
        {
            face[x, y] = "red";
        }

        public string getCubieColour(int x, int y)
        {
            return face[x, y];
        }

        public void matrixRotationAntiClockwise()
        {
            string[,] rotated = new string[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    rotated[j, 2 - i] = face[i, j];
                }
            }
            face = rotated;

        }
        public void matrixHalfRotation()
        {
            string[,] rotated = new string[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    rotated[2 - i, 2 - j] = face[i, j];
                }
            }
            face = rotated;
        }
        public void matrixRotationClockwise()
        {
            string[,] rotated = new string[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    rotated[2 - j, i] = face[i, j];
                }
            }
            face = rotated;

        }


        public string[] getLeftColumn(int currentOrientation)
        {
            string[] swapColumn = new string[3];
            switch (currentOrientation)
            {
                case 0:
                    for (int i = 0; i < 3; i++)
                    {
                        swapColumn[i] = face[i, 0];
                    }
                    return swapColumn;
                case 1:
                    for (int i = 0; i < 3; i++)
                    {
                        swapColumn[i] = face[0, 2 - i];
                    }
                    return swapColumn;
                case 2:
                    for (int i = 0; i < 3; i++)
                    {
                        swapColumn[i] = face[2 - i, 2];
                    }
                    return swapColumn;
                case 3:
                    for (int i = 0; i < 3; i++)
                    {
                        swapColumn[i] = face[2, 2 - i];
                    }
                    return swapColumn;
                default:
                    return swapColumn;
            }
        }

        public string[] getColumn(int columnToGet, bool reverse) //takes in the column to get and if it needs reversing 
        {
            string[] swapColumn = new string[3]; //sets up a temporary column
            if (columnToGet == 0 && reverse == false) 
            {
                for (int i = 0; i < 3; i++)
                {
                    swapColumn[i] = face[i, 0]; //gets the lefthand column top to bottom 
                }
                return swapColumn; 
            }
            else if (columnToGet == 0 && reverse == true)
            {
                for (int i = 0; i < 3; i++)
                {
                    swapColumn[i] = face[2-i, 0]; //gets the lefthand column bottom to top (reversed) 
                }
                return swapColumn;
            }
            else if (columnToGet == 2 && reverse == false)
            {
                for (int i = 0; i < 3; i++)
                {
                    swapColumn[i] = face[i, 2]; //gets the right hand column top to bottom
                }
                return swapColumn;
            }
            else //colomn 2 and reverse order 
            {
                for (int i = 0; i < 3; i++)
                {
                    swapColumn[i] = face[2-i, 2]; //gets the right hand column bottom to top (reverse) 
                }
                return swapColumn;
            }
        }

        public string[] getRow(int rowToGet, bool reverse) //takes in the row to get and if it needs reversing 
        {
            string[] swapColumn = new string[3];
            if (rowToGet == 0 && reverse == false)
            {
                for (int i = 0; i < 3; i++)
                {
                    swapColumn[i] = face[0, i]; //top row left to right 
                }
                return swapColumn; 
            }
            else if (rowToGet == 0 && reverse == true)
            {
                for (int i = 0; i < 3; i++)
                {
                    swapColumn[i] = face[0, 2-i]; //top row right to left (reverse order) 
                }
                return swapColumn;
            }
            else if (rowToGet == 2 && reverse == false)
            {
                for (int i = 0; i < 3; i++)
                {
                    swapColumn[i] = face[2, i]; ///bottom row left to right
                }
                return swapColumn;
            }
            else //row 2 and reverse = true 
            {
                for (int i = 0; i < 3; i++)
                {
                    swapColumn[i] = face[2, 2-i]; //bottom row right to left (reverse order) 
                }
                return swapColumn;
            }
        }

        public string[] getRightColumn(int currentOrientation)
        {
            string[] swapColumn = new string[3];
            switch (currentOrientation)
            {
                case 0:
                    for (int i = 0; i < 3; i++)
                    {
                        swapColumn[i] = face[i, 2];
                    }
                    return swapColumn;
                case 1:
                    for (int i = 0; i < 3; i++)
                    {
                        swapColumn[i] = face[2, 2 - i];
                    }
                    return swapColumn;
                case 2:
                    for (int i = 0; i < 3; i++)
                    {
                        swapColumn[i] = face[i, 0];
                    }
                    return swapColumn;
                case 3:
                    for (int i = 0; i < 3; i++)
                    {
                        swapColumn[i] = face[0, i];
                    }
                    return swapColumn;

            }
            return swapColumn;

        }

        public string[] getTopRow(int currentOrientation)
        {
            string[] swapRow = new string[3];
            switch (currentOrientation)
            {
                case 0:
                    for (int i = 0; i < 3; i++)
                    {
                        swapRow[i] = face[0, i];
                    }
                    break;
                case 1:
                    for (int i = 0; i < 3; i++)
                    {
                        swapRow[i] = face[i, 2];
                    }
                    break;
                case 2:
                    for (int i = 0; i < 3; i++)
                    {
                        swapRow[i] = face[2, 2 - i];
                    }
                    break;
                case 3:
                    for (int i = 0; i < 3; i++)
                    {
                        swapRow[i] = face[2 - i, 0];
                    }
                    break;
            }
            return swapRow;

        }

        public string[] getBottomRow(int currentOrientation)
        {
            string[] swapRow = new string[3];
            switch (currentOrientation)
            {
                case 0:
                    for (int i = 0; i < 3; i++)
                    {
                        swapRow[i] = face[2, i];
                    }
                    break;
                case 1:
                    for (int i = 0; i < 3; i++)
                    {
                        swapRow[i] = face[i, 0];
                    }
                    break;
                case 2:
                    for (int i = 0; i < 3; i++)
                    {
                        swapRow[i] = face[2, 2 - i];
                    }
                    break;
                case 3:
                    for (int i = 0; i < 3; i++)
                    {
                        swapRow[i] = face[i, 2];
                    }
                    break;
            }
            return swapRow;
        }

        public void UpdateNewColumn(string[] col , int targetCol) //takes in a column and where it needs to be placed
        {
            for (int i = 0; i < 3; i++)
            {
              face[i, targetCol] = col[i]; //replaces the targer column in the face with column inputted
           }
           
        }

        public void UpdateNewRow(string[] row, int targetRow) //takes in a row and where it needs to be placed
        {
                for (int i = 0; i < 3; i++)
                {
                    face[targetRow, i] = row[i]; // replaces the targer row in the face with row inputted
                }
            
        }

        public void UpdateColumn(string[] col, int currentOrientation, int targetCol)
        {
            int c = 0;
            switch (currentOrientation)
            {
                case 0:
                    for (int i = 0; i < 3; i++)
                    {
                        face[i, targetCol] = col[i];
                    }
                    break;
                case 1:
                    for (int i = 0; i < 3; i++)
                    {
                        face[targetCol, 2 - i] = col[i];
                    }
                    break;
                case 2:
                    for (int i = 2; i >= 0; i--)
                    {
                        face[i, 2 - targetCol] = col[c];
                        c++;
                    }
                    break;
                case 3:
                    for (int i = 2; i >= 0; i--)
                    {
                        face[2 - i, targetCol] = col[c];
                        c++;
                    }
                    break;
            }
        }


        public void UpdateRow(string[] row, int currentOrientation, int targetRow)
        {

            int c = 0;
            switch (currentOrientation)
            {
                case 0:
                    for (int i = 0; i < 3; i++)
                    {
                        face[targetRow, i] = row[i];
                    }
                    break;
                case 1:
                    for (int i = 0; i < 3; i++)
                    {
                        face[2 - i, targetRow] = row[i];
                    }
                    break;
                case 2:
                    for (int i = 2; i >= 0; i--)
                    {
                        face[2 - targetRow, i] = row[c];
                        c++;
                    }
                    break;
                case 3:
                    for (int i = 2; i >= 0; i--)
                    {
                        face[i, 2 - targetRow] = row[c];
                        c++;
                    }
                    break;
            }

        }

        public string getMiddleCubie()
        {
            return face[1, 1];
        }

        public string getTopEdge()
        {
            return face[0, 1];
        }
        public string getRightEdge()
        {
            return face[1, 2];
        }
        public string getLeftEdge()
        {
            return face[1, 0];
        }
        public string getBottomEdge()
        {
            return face[2, 1];
        }

        public void UpdateFaceOrientation(int orientation) //takes in the orientaiton it wants to update to 
        {
            UpdateCubies(currentOrientation, orientation);
            currentOrientation = orientation; //updates the currentOrientation of the face 
        }
        public string ReturnCurrentFaceAndOrientation(int currentface, int currentorientation)
        {
            string CurrentFaceAndOrientation = Convert.ToString(currentface) + Convert.ToString(currentorientation); //converts the current face and orientation integers to strings
            //and concatenates them, storing it in currentFaceAndOrientation 
            return CurrentFaceAndOrientation; 
        }

        public void UpdateCubies(int currentOrientation, int targetOrientation)
        {
            int numRotations = (4 + targetOrientation - currentOrientation) % 4; // calculate the number of 90 degree rotations needed

            for (int r = 0; r < numRotations; r++)
            {
                matrixRotationClockwise(); // rotate the face 90 degrees clockwise
                // update the cubies' positions based on the new orientation               
            }
        }

        public string[,] FlipFaceDisplay() //flips the orientation of the back face (it is stored in orienation 0 but displayed in 2) 
        {
            string[,] rotated = new string[3, 3]; //uses the matrix half rotation code but returns the rotated face instead of being a subroutine 
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    rotated[2 - i, 2 - j] = face[i, j];
                }
            }

            return rotated;

        }


    }

}





