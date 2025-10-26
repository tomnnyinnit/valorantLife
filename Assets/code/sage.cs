using System.Linq;
using UnityEngine;

public class sage : MonoBehaviour
{
    [Header("movement")]
    public float moveSpeed = 5f;

    private Vector2 moveDir;
    private Rigidbody2D rb;
    private bool isDead = false;
    public jett jett;

    [Header("Sprites")]
    public Sprite idle;
    public Sprite ability;
    public Sprite dead;
    public Sprite angry;
    public SpriteRenderer sr;
    public GameObject slow;

    private enum SageState { Wander, Dead, Orb, LastSage }
    private SageState currentState = SageState.Wander;
    private float stateTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        NewDirection();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        stateTimer -= Time.deltaTime;

        switch (currentState)
        {
            case SageState.Wander:
                Move(moveSpeed);
                sr.sprite = idle;
                break;

            case SageState.Dead:
                rb.linearVelocity = Vector2.zero;
                sr.sprite = dead;
                break;

            case SageState.Orb:
                break;

            case SageState.LastSage:
                break;
        }
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
        if (isDead) return;

        if (collision.gameObject.CompareTag("Wall"))
        {
            moveDir = Vector2.Reflect(moveDir, collision.contacts[0].normal);
        }

        if (collision.gameObject.CompareTag("Jett"))
        {
            moveDir = Vector2.Reflect(moveDir, collision.contacts[0].normal);
        }

        if (collision.gameObject.CompareTag("Jett") && jett.currentKnife > 0)
        {
            isDead = true;
            ChangeState(SageState.Dead);
            jett.currentKnife -= 1;
        }

        if (collision.gameObject.CompareTag("Orb"))
        {
            Destroy(collision.gameObject);
            orb();
        }

        if (collision.gameObject.CompareTag("Sage"))
        {
            moveDir = Vector2.Reflect(moveDir, collision.contacts[0].normal);
        }
    }

    void orb()
    {
        sage[] sages = FindObjectsOfType<sage>();
        bool hasDeadSage = sages.Any(s => s.isDead);

        if (hasDeadSage)
        {
            sage deadSage = sages.FirstOrDefault(s => s.isDead);
            if (deadSage != null)
            {
                deadSage.isDead = false;
                deadSage.ChangeState(SageState.Wander);
            }
        }
        else
        {
            jett jett = FindObjectOfType<jett>();
            if (jett != null)
            {
                jett.Slow();
                Instantiate(slow, jett.transform.position, Quaternion.identity);
            }
        }
    }

    void ChangeState(SageState newState)
    {
        currentState = newState;
    }
}
