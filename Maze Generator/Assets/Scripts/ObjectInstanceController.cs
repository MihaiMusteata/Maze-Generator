using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInstanceController : MonoBehaviour
{
     public bool isActive = false;
     
     public void OnEnable()
     {
          isActive = true;
     }

     public void OnDisable()
     {
          isActive = false;
     }
}
