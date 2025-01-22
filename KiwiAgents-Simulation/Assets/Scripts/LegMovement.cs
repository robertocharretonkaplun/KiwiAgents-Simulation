using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class LegMovement : MonoBehaviour
{
    public Transform raycastPoint;
    public Transform target;
    public Vector3 restingPos;
    public LayerMask mask;
    public Vector3 newposition;
    public Transform steppingPoint;
    public bool legGrounded;
    public GameObject player;
    public float offset;
    public float moveDistance;
    public static int currentMoveValue = 1;
    public int moveValue;
    public float speed = 10f;
    public LegMovement otherLeg;
    public bool hasMoved;
    public bool moving;
    public bool movingDown;

    public float movedDirection
    {
        get
        {
            return Controller.instance.agent.velocity.y;
        }
    }

    public bool movingBackwards = false;

    void Start()
    {
        restingPos = target.position;
        steppingPoint.position = new Vector3(restingPos.x + offset, restingPos.y, restingPos.z);
    }

    private void Update()
    {
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

        if ((Vector3.Distance(restingPos, newposition) > moveDistance || moving) && legGrounded)
        {
            Step(newposition);
        }

        uptadeIK();
    }

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

    public void Step(Vector3 position)
    {
        if (currentMoveValue == moveValue)
        {
            legGrounded = false;
            hasMoved = false;
            moving = true;

            target.position = Vector3.MoveTowards(target.position, position + Vector3.up, speed * Time.deltaTime);
            restingPos = Vector3.MoveTowards(target.position, position + Vector3.up, speed * Time.deltaTime);

            if (target.position == position + Vector3.up)
            {
                movingDown = true;
            }

            if (movingDown == true)
            {
                target.position = Vector3.MoveTowards(target.position, position, speed * Time.deltaTime);
                restingPos = Vector3.MoveTowards(target.position, position, speed * Time.deltaTime);
            }

            if (target.position == position)
            {
                legGrounded = true;
                hasMoved = true;
                moving = false;
                movingDown = false;

                if (currentMoveValue == moveValue && otherLeg.hasMoved == true)
                {
                    currentMoveValue = currentMoveValue * -1 + 3;
                }
            }
        }
    }

    public void uptadeIK()
    {
        target.position = restingPos;
    }
}