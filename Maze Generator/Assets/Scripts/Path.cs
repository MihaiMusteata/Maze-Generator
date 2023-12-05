using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.FilePathAttribute;

public class Path : MonoBehaviour
{
     [Header("Componenete")]
     [SerializeField] private GameObject pointsPrefab;
     [SerializeField] private GameObject pathPrefab;
     [SerializeField] private GameObject floorPrefab;
     [SerializeField] private Text mazeText;
     [SerializeField] private Item maze;

     [Header("Player")]
     [SerializeField] private GameObject player;
     private MovementInput playerController;

     Stack<Vector2Int> pathCoord = new Stack<Vector2Int>();
     private int[,] lee_matrix = null;

     private void Awake()
     {
          playerController = player.GetComponent<MovementInput>();
     }


     bool isValid(int x, int y)
     {
          if (x <= 0 || x >= maze.rows || y <= 0 || y >= maze.cols && lee_matrix[x, y] == 0)
               return false;
          return true;
     }


     void Find(int i, int j)
     {
          pathCoord.Push(new Vector2Int(i, j));

          int inou, jnou;
          for (int k = 0; k < 4; ++k)
          {
               inou = i + maze.dx[k];
               jnou = j + maze.dy[k];
               if (isValid(inou, jnou) && lee_matrix[inou, jnou] == lee_matrix[i, j] - 1)
               {
                    Find(inou, jnou);
               }
          }
     }

     public void Build(bool showSolution)
     {
          ClearPath();
          for (int i = 0; i < maze.rows; ++i)
          {
               for (int j = 0; j < maze.cols; ++j)
               {
                    if (!pathCoord.Contains(new Vector2Int(i, j)))
                    {
                         Instantiate(floorPrefab, new Vector3(i, 0, j), Quaternion.identity);
                    }
               }
          }
          foreach (var coord in pathCoord)
          {
               if (coord == maze.finishCoordinate || coord == maze.startCoordinate)
               {
                    continue;
               }

               if (showSolution)
               {
                    Instantiate(pathPrefab, new Vector3(coord.x, 0, coord.y), Quaternion.identity);
               }
               else
               {
                    Instantiate(floorPrefab, new Vector3(coord.x, 0, coord.y), Quaternion.identity);
               }
          }

          Instantiate(pointsPrefab, new Vector3(maze.finishCoordinate.x, 0, maze.finishCoordinate.y), Quaternion.identity);
          Instantiate(pointsPrefab, new Vector3(maze.startCoordinate.x, 0, maze.startCoordinate.y), Quaternion.identity);

          Vector3 location = new Vector3(maze.startCoordinate.x, 1, maze.startCoordinate.y);
          playerController.Teleport(location);
     }

     void Lee(int is_, int js)
     {
          lee_matrix = new int[maze.rows, maze.cols];
          maze.matrix[(int)maze.finishCoordinate.x, (int)maze.finishCoordinate.y] = 0;
          Queue<Vector2Int> Q = new Queue<Vector2Int>();
          Q.Enqueue(new Vector2Int(is_, js));
          lee_matrix[is_, js] = 1;

          while (Q.Count > 0)
          {
               Vector2Int current = Q.Dequeue();
               int i = current.x;
               int j = current.y;

               for (int k = 0; k < 4; ++k)
               {
                    int inou = i + maze.dx[k];
                    int jnou = j + maze.dy[k];

                    if (maze.IsValid(inou, jnou) && maze.matrix[inou, jnou] == 0 && lee_matrix[inou, jnou] == 0)
                    {
                         lee_matrix[inou, jnou] = lee_matrix[i, j] + 1;
                         Q.Enqueue(new Vector2Int(inou, jnou));
                    }
               }
          }

     }
     public void Display()
     {
          //display lee_matrix
          string mazeString = "";
          for (int i = 0; i < maze.rows; ++i)
          {
               for (int j = 0; j < maze.cols; ++j)
               {
                    mazeString += lee_matrix[i, j] + "\t\t";
               }
               mazeString += "\n";
          }
          mazeText.text = mazeString;
     }

     public void ClearPath()
     {
          GameObject[] paths = GameObject.FindGameObjectsWithTag("Path");
          foreach (var path in paths)
          {
               Destroy(path);
          }
          GameObject[] points = GameObject.FindGameObjectsWithTag("Points");
          foreach (var point in points)
          {
               Destroy(point);
          }
          GameObject[] floors = GameObject.FindGameObjectsWithTag("Floor");
          foreach (var floor in floors)
          {
               Destroy(floor);
          }
     }
     public void ClearGame()
     {
          ClearPath();
          mazeText.text = "";
          lee_matrix = null;
          pathCoord.Clear();
     }

     public void Regenerate(bool showSolution)
     {
          ClearGame();
          Lee((int)maze.startCoordinate.x, (int)maze.startCoordinate.y);
          Find((int)maze.finishCoordinate.x, (int)maze.finishCoordinate.y);
          Build(showSolution);

          Display();
     }


}
