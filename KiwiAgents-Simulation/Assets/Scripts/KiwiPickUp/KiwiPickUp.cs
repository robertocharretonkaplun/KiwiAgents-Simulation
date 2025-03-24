using UnityEngine;
using TMPro;

public class KiwiFoodPickUp : MonoBehaviour
{
    [Header("Configuración de recolección")]
    public Collider pickUpTrigger; // Referencia al Collider de recolección
    public Transform wormHoldPosition; // Posición donde se mostrará el gusano al recogerlo
    public Vector3 wormRotation; // Rotación ajustable desde el inspector

    [Header("UI")]
    public TextMeshProUGUI pickupText; // UI para mostrar el contador

    private GameObject objectToPickUp; // Gusano disponible para recoger
    private GameObject heldWorm; // Gusano actualmente sostenido

    private void Start()
    {
        UpdateUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WormFood") && other != pickUpTrigger && heldWorm == null)
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
        if (objectToPickUp != null && Input.GetKeyDown(KeyCode.E)) // "E" para recoger
        {
            PickUpItem();
        }

        if (heldWorm != null && Input.GetKeyDown(KeyCode.Q)) // "Q" para soltar
        {
            DropItem();
        }
    }

    private void PickUpItem()
    {
        heldWorm = objectToPickUp;
        heldWorm.SetActive(false); // Oculta el objeto en la escena
        heldWorm.transform.SetParent(wormHoldPosition); 
        heldWorm.transform.localPosition = Vector3.zero; 
        heldWorm.transform.localRotation = Quaternion.Euler(wormRotation); // Aplica la rotación desde el Inspector
        heldWorm.SetActive(true); 
        objectToPickUp = null;
        UpdateUI();
    }

    private void DropItem()
    {
        heldWorm.transform.SetParent(null); 
        heldWorm.transform.position = transform.position + transform.forward * 2; // Lo coloca enfrente
        heldWorm.SetActive(true);
        heldWorm = null;
        UpdateUI();
    }

    private void UpdateUI()
    {
        pickupText.text = heldWorm != null ? "1" : "0";
    }
}
