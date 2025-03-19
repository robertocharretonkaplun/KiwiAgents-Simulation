using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int semillasRecolectadas = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RecolectarSemilla()
    {
        semillasRecolectadas = semillasRecolectadas + 2;
        Debug.Log("Semillas recolectadas: " + semillasRecolectadas);
    }
}
