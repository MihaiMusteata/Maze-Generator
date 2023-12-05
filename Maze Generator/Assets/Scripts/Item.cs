using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Create New Item")]
public class Item : ScriptableObject
{
     // Directii posibile pentru a naviga în labirint
     public int[] dx = { 0, 0, 1, -1 };
     public int[] dy = { 1, -1, 0, 0 };

     // Matricea labirintului
     public int[,] matrix;

     // Dimensiunile labirintului
     public int rows;
     public int cols;

     // Valori pentru traseu, perete, start si finish
     public int PATH = 0;
     public int WALL = 1;
     public int START = 8;
     public int FINISH = 8;

     // Coordonatele punctelor de inceput si sfarsit
     public Vector2 startCoordinate = new Vector2(1, 1);
     public Vector2 finishCoordinate;

     public void Init()
     {
          matrix = new int[rows, cols];
          for (int i = 0; i < rows; i++)
          {
               for (int j = 0; j < cols; j++)
               {
                    matrix[i, j] = 1;
               }
          }
          finishCoordinate = new Vector2(rows - 2, cols - 2);

     }
     public bool IsValid(int i, int j)
     {
          return (i >= 0 && i < rows && j >= 0 && j < cols);
     }
}

