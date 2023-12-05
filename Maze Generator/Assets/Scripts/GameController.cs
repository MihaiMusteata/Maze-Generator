using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class GameController : MonoBehaviour
{
     private StarterAssetsInputs starterAssetsInputs;
     private ObjectInstanceController menuController;
     [SerializeField] private GameObject menu;


     void Start()
     {
          starterAssetsInputs = GetComponent<StarterAssetsInputs>();
          menuController = menu.GetComponent<ObjectInstanceController>();
     }

     void Update()
     {
          if (starterAssetsInputs.pause)
          {
               Debug.Log("Pause presed");
               Debug.Log("Menu : " + menu);
               Debug.Log("Menu active: " + menuController.isActive);
               if (!menuController.isActive)
               {
                    menu.SetActive(true);
               }
               else
               {
                    menu.SetActive(false);
               }
               starterAssetsInputs.pause = false;
          }

     }
}
