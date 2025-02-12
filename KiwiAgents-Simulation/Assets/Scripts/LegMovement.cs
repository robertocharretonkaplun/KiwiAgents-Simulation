using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LegMovement : MonoBehaviour
{
    // GamObjects and layers
    public GameObject player;
    public LayerMask mask;

    // Transforms and positions
    public Transform raycastPoint;
    public Transform target;
    public Transform steppingPoint;
    public Vector3 restingPos;
    public Vector3 newposition;

    // Floats
    public float offset;
    public float moveDistance;
    public float speed = 10f;
    public int moveValue;

    // Booleans
    public bool legGrounded;
    public bool hasMoved;
    public bool moving;
    public bool movingDown;
    public bool movingBackwards = false;

    // Static variables
    public static int currentMoveValue = 1;

    // References
    public LegMovement otherLeg;

    /// <summary>
    /// Direction of the player's movement in the Y axis, from the controller's script.
    /// </summary>
    public float movedDirection
    {
        get
        {
            return Controller.instance.agent.velocity.y;
        }
    }

    /// <summary>
    /// Initializes the resting position and sets the stepping point.
    /// </summary>
    void Start()
    {
        restingPos = target.position;
        steppingPoint.position = new Vector3(restingPos.x + offset, restingPos.y, restingPos.z);
    }

    /// <summary>
    /// Updates the leg movement and IK logic each frame.
    /// </summary>
    private void Update()
    {
        // Handle movement direction changes
        if (movedDirection < 0 && !movingBackwards)
        {
            movingBackwards = true;
            offset = -0.5f;
            steppingPoint.localPosition += Vector3.right * offset;
        }
        else if (movedDirection > 0 && movingBackwards)
        {
            movingBackwards = false;
            offset = 0.5f;
            steppingPoint.localPosition += Vector3.right * offset;
        }

        newposition = calculatePoint(steppingPoint.position);

        // Check if the leg needs to step
        if ((Vector3.Distance(restingPos, newposition) > moveDistance || moving) && legGrounded)
        {
            Step(newposition);
        }

        uptadeIK();
    }

    /// <summary>
    /// Calculates the next valid stepping point using a raycast.
    /// </summary>
    public Vector3 calculatePoint(Vector3 position)
    {
        Vector3 dir = position - raycastPoint.position;
        RaycastHit hit;
        if (Physics.SphereCast(raycastPoint.position, 1f, dir, out hit, 5f, mask))
        {
            position = hit.point;
            legGrounded = true;
        }
        else
        {
            position = restingPos;
            legGrounded = false;
        }
        return position;
    }

    /// <summary>
    /// Calculates each movement of the leg to the specified position in a stepping motion.
    /// Handles upward movement, downward movement, and leg syncronization.
    /// </summary>
    public void Step(Vector3 position)
    {
        // Validates if this leg is allowed to move based on the currentMoveValue
        if (currentMoveValue == moveValue)
        {
            // Mark the leg as not grounded and start teh movement
            legGrounded = false;
            hasMoved = false;
            moving = true;

            // Move the leg upwards the stepping position and updates the resting position
            target.position = Vector3.MoveTowards(target.position, position + Vector3.up, speed * Time.deltaTime);
            restingPos = Vector3.MoveTowards(target.position, position + Vector3.up, speed * Time.deltaTime);

            // If the leg reaches the top of the step, transition to the downward motion
            if (target.position == position + Vector3.up)
            {
                movingDown = true;
            }

            if (movingDown == true)
            {
                // Move the leg downwards the final stepping position and updates the resting position
                target.position = Vector3.MoveTowards(target.position, position, speed * Time.deltaTime);
                restingPos = Vector3.MoveTowards(target.position, position, speed * Time.deltaTime);
            }

            // If the the leg reaches the final position
            if (target.position == position)
            {
                // Mark the leg as grounded again and stop the movement
                legGrounded = true;
                hasMoved = true;
                moving = false;
                movingDown = false;

                // If both legs have completed their movements, update the currentMoveValue
                // to allow the other leg to move next.
                // The code was intended for a spider, but this is not necessary for a bipedal agent.
                if (currentMoveValue == moveValue && otherLeg.hasMoved == true)
                {
                    currentMoveValue = currentMoveValue * -1 + 3;
                }
            }
        }
    }

    /// <summary>
    /// Keeps the target aligned with the resting position.
    /// </summary>
    public void uptadeIK()
    {
        target.position = restingPos;
    }
}