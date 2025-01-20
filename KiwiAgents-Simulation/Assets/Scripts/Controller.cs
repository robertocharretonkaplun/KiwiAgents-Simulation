using UnityEngine;

public class Controller : MonoBehaviour
{
    private Rigidbody rb;
    public float speed;
    public float angspeed;
    private float movx;
    public float movy;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    public static Controller instance;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        movx = Input.GetAxis("Horizontal"); 
        movy = Input.GetAxis("Vertical");

        if(movy != 0)
        {
            Vector3 movement = transform.right * movy * speed * Time.deltaTime;
            rb.linearVelocity = movement;
        }

        transform.Rotate(0, movx * angspeed * Time.deltaTime, 0);
    }
}
