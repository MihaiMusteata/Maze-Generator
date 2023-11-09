using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeGenerator : MonoBehaviour
{
     // Dimensiunile labirintului
     [Header("Dimensiuni labirint")]
     [SerializeField] private int rows = 10;
     [SerializeField] public int cols = 10;
     [Header("Componenete")]
     [SerializeField] private Text mazeText;
     [SerializeField] private GameObject wallPrefab; 


     // Direc?ii posibile pentru a naviga în labirint
     private int[] dx = { 0, 0, 1, -1 };
     private int[] dy = { 1, -1, 0, 0 };


     // Valori pentru traseu, perete, start ?i finish
     private int PATH = 0;
     private int WALL = 1;
     private int START = 8;
     private int FINISH = 8;

     // Matrice pentru a reprezenta labirintul
     private int[,] maze;

     private void Awake()
     {
          rows += (rows + 1) % 2;
          cols += (cols + 1) % 2;
          
          maze = new int[rows, cols];
          for(int i = 0; i < rows; i++)
          {
               for(int j = 0; j < cols; j++)
               {
                    maze[i, j] = WALL;
               }
          }    
     }
     
     private void Start()
     { 
          GenerateMaze();
          DisplayMaze();
          BuildMaze();
     }

     void GenerateMaze()
     {
          System.Random rand = new System.Random();

          Stack<Vector2Int> pathStack = new Stack<Vector2Int>();
          Vector2Int start = new Vector2Int(1,1);

          pathStack.Push(start);
          maze[start.x, start.y] = START;

          while (pathStack.Count > 0)
          {
               Vector2Int pos = pathStack.Peek();

               List<int> neighbors = new List<int>();

               for (int dir = 0; dir < 4; ++dir)
               {
                    int nx = pos.x + dx[dir] * 2;
                    int ny = pos.y + dy[dir] * 2;

                    if (nx > 0 && nx < rows-1 && ny > 0 && ny < cols-1 && maze[nx, ny] == WALL)
                    {
                         neighbors.Add(dir);
                    }
               }

               if (neighbors.Count > 0)
               {
                    int randomNeighbor = neighbors[rand.Next(neighbors.Count)];
                    int nx = pos.x + dx[randomNeighbor] * 2;
                    int ny = pos.y + dy[randomNeighbor] * 2;

                    maze[nx, ny] = PATH;
                    maze[pos.x + dx[randomNeighbor], pos.y + dy[randomNeighbor]] = PATH;
                    pathStack.Push(new Vector2Int(nx, ny));
               }
               else
               {
                    pathStack.Pop();
               }
          }

          // Setarea zonei de finish
          int finishX = rows - 2;
          int finishY = cols - 2;
          maze[finishX, finishY] = FINISH;

     }

     void DisplayMaze()
     {
          string mazeString = "";
          for (int i = 0; i < rows; ++i)
          {
               for (int j = 0; j < cols; ++j)
               {
                    mazeString += maze[i, j] + " ";
               }
               mazeString += "\n";
          }

          mazeText.text = mazeString;
     }

     void BuildMaze()
     {
          for (int i = 0; i < rows; i++)
          {
               for (int j = 0; j < cols; j++)
               {
                    if (maze[i, j] == WALL)
                    {
                         // Pozi?ia în lume pentru instan?ierea peretelui
                         Vector3 position = new Vector3(i, 0, j);
                         Instantiate(wallPrefab, position, Quaternion.identity);
                    }
               }
          }
     }
     public void ClearMaze()
     {
          GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

          foreach (GameObject wall in walls)
          {
               Destroy(wall);
          }
     }

     public void RegenerateMaze()
     {
          // Reseta?i labirintul cu pere?i peste tot
          for (int i = 0; i < rows; i++)
          {
               for (int j = 0; j < cols; j++)
               {
                    maze[i, j] = WALL;
               }
          }
          ClearMaze();
          GenerateMaze();
          BuildMaze();
     }


}
