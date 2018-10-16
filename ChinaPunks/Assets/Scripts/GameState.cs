using System.Collections;
using System.Collections.Generic;

public class GameState {
    private int row;
    private int col;
    private int numberOfUnits;

    private Unit[] units;
    /* Temorarily set as an array of string
     * Plan to have a "Unit" class declared to form an array of "Unit"
     */
    private int acting;   //The character (either a player or an enemy) who is turn to act

    public int[,] board;  //Storing the index of "Units" stored in units[]

    public GameState(int newRow, int newCol)
    {
        row = newRow;
        col = newCol;
        numberOfUnits = row * col;
        units = new Unit[numberOfUnits];
        /* For now, I just assume we will have an empty-Unit to stand for an empty cell, 
         * so we need to fill up this array with Units in this way
         * Actually we can also use -1 in board[,] to notify it's empty
         * This will make more safety check when indexing units[] using board[,], 
         * but will also decrease the size of units[]
         */
        acting = 0;       //Assuming players always act firstly (?)
        board = new int[row,col];
    }

    public int Row()
    {
        return row;
    }

    public int Col()
    {
        return col;
    }

    public void RemoveCell(int r,int c)
    {
        units[board[r,c]] = null;  //Will be the empty-Unit (temporarily using null)
    }

    public void Next()
    {
        for (int i = 0; i < numberOfUnits; i++)
        {
            acting++;
            if (acting == numberOfUnits)
                acting = 0;
//            if (units[acting].type == "Moveable")  //Some tags (may not be unity's tag) refering it's a moveable "Unit"
//                return;
        }
    }

    public Unit GetCurrent()  //Will be changed to returning "Unit"
    {
        return units[acting];
    }
}
