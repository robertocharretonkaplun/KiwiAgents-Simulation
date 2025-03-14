using Unity.Cinemachine;
using UnityEngine;

//Bruwu

public class LClickFreeCamera : MonoBehaviour
{
    [Header("Cinemachine")]
   public CinemachineCamera freeCamera; 
   public CinemachineCamera mainCamera;

   void Update()
   {
    if (Input.GetMouseButtonDown(1))
    {
        freeCamera.Priority = 1;
        mainCamera.Priority = 2;
    }
    else if (Input.GetMouseButtonUp(1)) //Ese mi Juan es Vida no chambea, 0 procedural de su parte -3-
    {
        freeCamera.Priority = 2;
        mainCamera.Priority = 1;
    }
   }

   //ya chambea gomi plis u.u
}
