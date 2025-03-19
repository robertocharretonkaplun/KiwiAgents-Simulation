using UnityEngine;
using TMPro;
using System.Collections.Generic; 

public class KiwiFoodPickUp : MonoBehaviour
    //APuntes
    // Faltan errores por solucionar ya que si suelta el gusanito no puede revolver a recojer ese mismo, revisar logica y poder poner animacion para que se vea el pick up de gusanito
{
    [Header("Configuración de recolección")]
    public Collider pickUpTrigger; // Referencia al Collider de recolección

    [Header("UI")]
    public TextMeshProUGUI pickupText; // UI para mostrar el contador

    private List<GameObject> inventory = new List<GameObject>(); // Lista de objetos recogidos
    private GameObject objectToPickUp; // Referencia al objeto en la zona de recogida

    private void Start()
    {
        UpdateUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WormFood") && other != pickUpTrigger) // Verifica que no sea el mismo collider
        {
            objectToPickUp = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == objectToPickUp)
        {
            objectToPickUp = null;
        }
    }

    private void Update()
    {
        if (objectToPickUp != null && Input.GetKeyDown(KeyCode.P)) // "P" para recoger
        {
            PickUpItem(objectToPickUp);
        }

        if (inventory.Count > 0 && Input.GetKeyDown(KeyCode.O)) // "O" para soltar
        {
            DropItem();
        }
    }

    private void PickUpItem(GameObject item)
    {
        inventory.Add(item);
        item.SetActive(false); // Oculta el objeto
        objectToPickUp = null;
        UpdateUI();
        Debug.Log("Nadie me quiere, todos me odian, mejor me como un gusanito");
    }


    //Suelta el gusanito enfrente del jugador
    private void DropItem()
    {
        GameObject item = inventory[inventory.Count - 1];
        inventory.RemoveAt(inventory.Count - 1);
        item.SetActive(true);
        item.transform.position = transform.position + transform.forward * 2; // Lo coloca enfrente del jugador
        UpdateUI();
        Debug.Log("sabe feo, what color  == WAKALA ");
    }

    private void UpdateUI()
    {
        pickupText.text = "" + inventory.Count;
    }
}

