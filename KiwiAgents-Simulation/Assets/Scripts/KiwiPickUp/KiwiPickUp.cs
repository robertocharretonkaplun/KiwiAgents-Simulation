using UnityEngine;
using TMPro;

public class KiwiFoodPickUp : MonoBehaviour
{
    [Header("Configuración de recolección")]
    public Collider pickUpTrigger; // Collider que define el área de recolección
    public Transform wormHoldPosition; // Lugar donde se mostrará el gusano

    [Header("UI")]
    public TextMeshProUGUI pickupText; // UI para mostrar el contador

    private GameObject objectToPickUp; // Gusano disponible para recoger
    private GameObject heldWorm; // Gusano actualmente sostenido
    private Rigidbody heldWormRb; // Referencia al Rigidbody del gusano
    private WormPatrol wormPatrolScript; // Referencia al script de patrulla

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
        else if (heldWorm != null && Input.GetKeyDown(KeyCode.Q)) // "Q" para soltar
        {
            DropItem();
        }
    }

    private void PickUpItem()
    {
        heldWorm = objectToPickUp;
        heldWormRb = heldWorm.GetComponent<Rigidbody>(); // Obtiene el Rigidbody
        wormPatrolScript = heldWorm.GetComponent<WormPatrol>(); // Obtiene el script de patrulla

        if (heldWormRb != null)
        {
            heldWormRb.isKinematic = true; // Desactiva las físicas para que no se mueva
            heldWormRb.useGravity = false;
        }

        if (wormPatrolScript != null)
        {
            wormPatrolScript.enabled = false; // Desactiva la patrulla
        }

        heldWorm.transform.SetParent(wormHoldPosition);
        heldWorm.transform.localPosition = Vector3.zero;
        heldWorm.SetActive(true);
        objectToPickUp = null;
        UpdateUI();
    }

    private void DropItem()
    {
        heldWorm.transform.SetParent(null);
        heldWorm.transform.position = transform.position + transform.forward * 2; // Lo coloca enfrente

        if (heldWormRb != null)
        {
            heldWormRb.isKinematic = false; // Reactiva las físicas
            heldWormRb.useGravity = true;
        }

        if (wormPatrolScript != null)
        {
            wormPatrolScript.enabled = true; // Reactiva la patrulla
        }

        heldWorm = null;
        heldWormRb = null;
        wormPatrolScript = null;
        UpdateUI();
    }

    private void UpdateUI()
    {
        pickupText.text = heldWorm != null ? "1" : "0";
    }
}
