using UnityEngine;

public class AgentStateIndicator : MonoBehaviour
{
    public GameObject agent; // Referencia al agente
    public Color movingColor = Color.green;
    public Color idleColor = Color.yellow;
    public Color jumpingColor = Color.red;

    private Renderer rend;
    private Rigidbody agentRb;
    private Vector3 lastPosition;
    private bool isGrounded;

    void Start()
    {
        rend = GetComponent<Renderer>();
        agentRb = agent.GetComponent<Rigidbody>();
        lastPosition = agent.transform.position;
    }

    void Update()
    {
        // Verificar si el agente está en movimiento
        bool isMoving = Vector3.Distance(lastPosition, agent.transform.position) > 0.001f;
        lastPosition = agent.transform.position;

        // Verificar si el agente está en el aire (saltando)
        isGrounded = Physics.Raycast(agent.transform.position, Vector3.down, 0.1f);

        if (!isGrounded)
        {
            rend.material.color = jumpingColor; // Rojo cuando salta
        }
        else if (isMoving)
        {
            rend.material.color = movingColor; // Verde cuando se mueve
        }
        else
        {
            rend.material.color = idleColor; // Amarillo cuando está quieto
        }
    }
}
