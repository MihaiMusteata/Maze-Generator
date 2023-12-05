using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Maze : MonoBehaviour
{

     [SerializeField] private Item maze;
     [Header("Componenete")]
     [SerializeField] private Text mazeText;
     [SerializeField] private GameObject wallPrefab;
     [SerializeField] private TMP_InputField rowBox;
     [SerializeField] private TMP_InputField colBox;

     public void ReadInput()
     {
          maze.rows = int.Parse(rowBox.text);
          maze.cols = int.Parse(colBox.text);
          maze.rows += (maze.rows + 1) % 2;
          maze.cols += (maze.cols + 1) % 2;
          maze.Init();
     }

     void Generate()
     {
          System.Random rand = new System.Random();

          Stack<Vector2Int> pathStack = new Stack<Vector2Int>();
          Vector2Int start = new Vector2Int(1,1);

          pathStack.Push(start);
          maze.matrix[start.x, start.y] = maze.START;

          while (pathStack.Count > 0)
          {
               Vector2Int pos = pathStack.Peek();

               List<int> neighbors = new List<int>();

               for (int dir = 0; dir < 4; ++dir)
               {
                    int nx = pos.x + maze.dx[dir] * 2;
                    int ny = pos.y + maze.dy[dir] * 2;

                    if (nx > 0 && nx < maze.rows-1 && ny > 0 && ny < maze.cols-1 && maze.matrix[nx, ny] == maze.WALL)
                    {
                         neighbors.Add(dir);
                    }
               }

               if (neighbors.Count > 0)
               {
                    int randomNeighbor = neighbors[rand.Next(neighbors.Count)];
                    int nx = pos.x + maze.dx[randomNeighbor] * 2;
                    int ny = pos.y + maze.dy[randomNeighbor] * 2;

                    maze.matrix[nx, ny] = maze.PATH;
                    maze.matrix[pos.x + maze.dx[randomNeighbor], pos.y + maze.dy[randomNeighbor]] = maze.PATH;
                    pathStack.Push(new Vector2Int(nx, ny));
               }
               else
               {
                    pathStack.Pop();
               }
          }

          maze.matrix[(int)maze.finishCoordinate.x, (int)maze.finishCoordinate.y] = maze.FINISH;
     }

     void Display()
     {
          string mazeString = "";
          for (int i = 0; i < maze.rows; ++i)
          {
               for (int j = 0; j < maze.cols; ++j)
               {
                    mazeString +=  maze.matrix[i, j] + " ";
               }
               mazeString += "\n";
          }

          mazeText.text = mazeString;
     }

     void Build()
     {
          for (int i = 0; i < maze.rows; i++)
          {
               for (int j = 0; j < maze.cols; j++)
               {
                    if (maze.matrix[i, j] == maze.WALL)
                    {
                         // Pozitia în lume pentru instantierea peretelui
                         Vector3 position = new Vector3(i, 0, j);
                         Instantiate(wallPrefab, position, Quaternion.identity);
                    }
               }
          }
     }
     public void Clear()
     {
          GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

          foreach (GameObject wall in walls)
          {
               Destroy(wall);
          }
     }

     public void Regenerate()
     {
          // Reset 
          for (int i = 0; i < maze.rows; i++)
          {
               for (int j = 0; j < maze.cols; j++)
               {
                    maze.matrix[i, j] = maze.WALL;
               }
          }
          Clear();
          Generate();
          Build();
          Display();
     }

   


}
