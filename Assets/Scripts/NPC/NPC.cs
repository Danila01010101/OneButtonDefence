using UnityEngine;

public class NPC : MonoBehaviour
{
    public int Health;
    public float Speed;

    private Rigidbody rb;

    private void Start()
    {
        // Получаем компонент Rigidbody
        rb = GetComponent<Rigidbody>();
    }

    public virtual void Update()
    {
        
    }

    public virtual void Movement() 
    {
        // Направление движения NPC (например, вперед)
        Vector3 direction = -1*transform.forward;

        // Вычисляем новую позицию
        Vector3 newPosition = rb.position + direction * Speed * Time.fixedDeltaTime;

        // Перемещаем Rigidbody к новой позиции
        rb.MovePosition(newPosition);
    }

    public virtual void FixedUpdate()
    {
        Movement();
    }
}
