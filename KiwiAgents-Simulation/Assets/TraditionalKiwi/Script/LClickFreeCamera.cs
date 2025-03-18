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
    if (Input.GetKeyDown((KeyCode.C)))
    {
        freeCamera.Priority = 1;
        mainCamera.Priority = 2;
    }
    else if (Input.GetKeyUp((KeyCode.C))) //Ese mi I AM A GAME DESIGNER no chambea, 0 procedural de su parte -3-
    {
        freeCamera.Priority = 2;
        mainCamera.Priority = 1;
    }
   }

   //ya chambea gomi plis u.u
}
