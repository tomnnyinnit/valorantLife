using UnityEngine;

public class omenMovement : MonoBehaviour

{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    Rigidbody2D rb;
    private Vector2 moveDir;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        NewDirection();

    }

    // Update is called once per frame
    void Update()
    {
        Move(2.5f);

    }
    void Move(float speed)
    {
        rb.linearVelocity = moveDir * speed;
    }
    void NewDirection()
    {
        moveDir = Random.insideUnitCircle.normalized;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {


        if (collision.gameObject.CompareTag("Wall"))
        {
            moveDir = Vector2.Reflect(moveDir, collision.contacts[0].normal);
        }

        if (collision.gameObject.CompareTag("Jett"))
        {
            moveDir = Vector2.Reflect(moveDir, collision.contacts[0].normal);
        }
        if (collision.gameObject.CompareTag("Orb"))
        {
            moveDir = Vector2.Reflect(moveDir, collision.contacts[0].normal);
        }


        if (collision.gameObject.CompareTag("Sage"))
        {
            moveDir = Vector2.Reflect(moveDir, collision.contacts[0].normal);
        }
    }
}
