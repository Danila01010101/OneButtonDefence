using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class NPC : MonoBehaviour
{
    public int Health;
    public float Speed;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void Update()
    {
        
    }

    public virtual void Movement() 
    {
        Vector3 direction = -1*transform.forward;
        Vector3 newPosition = rb.position + direction * Speed * Time.fixedDeltaTime;
        rb.rotation.SetLookRotation(newPosition - transform.position);
        rb.MovePosition(newPosition);
    }

    public virtual void FixedUpdate()
    {
        Movement();
    }
}
