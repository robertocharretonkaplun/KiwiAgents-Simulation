using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int seedCount = 0;

    public void AddSeed(int amount = 1)
    {
        seedCount += amount;
        Debug.Log("Semillas recogidas: " + seedCount);
    }

    public bool UseSeed()
    {
        if (seedCount > 0)
        {
            seedCount--;
            Debug.Log("Semilla usada. Restantes: " + seedCount);
            return true;
        }

        Debug.Log("No tienes semillas.");
        return false;
    }
}