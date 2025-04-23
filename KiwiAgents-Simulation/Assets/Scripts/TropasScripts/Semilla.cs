using UnityEngine;
using UnityEngine.UI;

public class Semilla : MonoBehaviour
{
    public int seedAmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            inventory.AddSeed(seedAmount);
            Destroy(gameObject); // Desaparece la semilla recogida
        }
    }
}