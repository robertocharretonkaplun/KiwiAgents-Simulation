using UnityEngine;

public class EnemyRevealOnSight : MonoBehaviour
{
    public GameObject modeloEnemigo; // El modelo oculto
    public GameObject exclamacionUI; // El ícono "!"
    public Transform player;
    public float rangoVision = 10f;
    public float anguloVision = 60f;
    public float tiempoAntesDeRevelar = 1.5f;

    private bool revelado = false;

    void Start()
    {
        if (modeloEnemigo != null) modeloEnemigo.SetActive(false);
        if (exclamacionUI != null) exclamacionUI.SetActive(false);

        if (player == null)
        {
            GameObject foundPlayer = GameObject.FindGameObjectWithTag("Player");
            if (foundPlayer != null)
                player = foundPlayer.transform;
        }
    }

    void Update()
    {
        if (revelado || player == null) return;

        Vector3 dirJugador = player.position - transform.position;
        float distancia = dirJugador.magnitude;

        if (distancia < rangoVision)
        {
            float angulo = Vector3.Angle(transform.forward, dirJugador);
            if (angulo < anguloVision * 0.5f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position + Vector3.up, dirJugador.normalized, out hit, rangoVision))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        StartCoroutine(RevelarEnemigo());
                    }
                }
            }
        }
    }

    System.Collections.IEnumerator RevelarEnemigo()
    {
        if (revelado) yield break;
        revelado = true;

        if (exclamacionUI != null) exclamacionUI.SetActive(true);

        yield return new WaitForSeconds(tiempoAntesDeRevelar);

        if (exclamacionUI != null) exclamacionUI.SetActive(false);
        if (modeloEnemigo != null) modeloEnemigo.SetActive(true);
    }
}
