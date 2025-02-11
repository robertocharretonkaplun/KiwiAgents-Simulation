using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using System;
public class UnitSelectionManager : MonoBehaviour
{
  public static UnitSelectionManager Instance { get; set; }

    public List <GameObject> allUnitsList = new List <GameObject> ();
    public List<GameObject> unitSelected = new List<GameObject>();

    public LayerMask clickable;
    public LayerMask ground;
    public GameObject groundMarker;
    private Camera cam;



    //-///////Singleton/////-
    private void Awake()
    {
        if (Instance !=null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;    
        }
    }
    //-////////////////////

    private void Start()
    {
        cam= Camera.main;
    }   

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);


            //SI es hit del click es un objeto seleccionable
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
            {
                SelectByClicking(hit.collider.gameObject);
            }
            else //Si no es objeto clickeable
            {
                DeselectAll();
            }
        }
    }


    private void DeselectAll()
    {

    }
    private void   SelectByClicking(GameObject unit)
    {
        DeselectAll();



        unitSelected.Add(unit);
        EnableUnitMovement(unit, true);


    }

    private void EnableUnitMovement(GameObject unit, bool shouldMove)
    {
        unit.GetComponent<KiwiMovement>().enabled = shouldMove;
    }
}
