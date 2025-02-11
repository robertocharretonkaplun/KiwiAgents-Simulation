using UnityEngine;
using UnityEngine.AI;

public class Kiwi  : MonoBehaviour
{
    public Camera mainCamera;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Botón derecho del mouse
        {
            MoveToClick();
        }
    }

    void MoveToClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            agent.SetDestination(hit.point);
        }
    }
}