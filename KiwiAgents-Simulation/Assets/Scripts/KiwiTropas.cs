using UnityEngine;

public class KiwiTropas : MonoBehaviour
{
    public Transform player;      // Arrastra aquí el transform del jugador
    public float speed = 5f;      // Velocidad de movimiento
    public float stoppingDistance = 2f; // Distancia mínima para dejar de moverse

    void Update()
    {
        if (player == null)
            return;

        // Calcula la distancia al jugador
        float distance = Vector3.Distance(transform.position, player.position);

        // Si está más lejos que la distancia mínima, se mueve hacia el jugador
        if (distance > stoppingDistance)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // (Opcional) Girar hacia el jugador
            transform.LookAt(new Vector3(player.position.x, transform.position.y, player.position.z));
        }
    }
}