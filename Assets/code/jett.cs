using UnityEditor.Search;
using UnityEngine;

public class jett : MonoBehaviour
{
    [Header("movement")]
    public float moveSpeed = 5f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.3f;
    public float dashCooldown = 10f;
    public float slowMultiplier = 0.5f;
    public float slowDuration = 2f;

    [Header("knife")]
    public int maxKnife = 2;
    public int currentKnife = 0;

    [Header("Sprites")]
    public Sprite noKnife;
    public Sprite aKnife;
    public Sprite bKnife;
    public SpriteRenderer sr;


    private Vector2 moveDir;
    private Rigidbody2D rb;

    private float stateTimer = 0f;
    private float cdTime = 0f;
    private float dashTime = 0f;
    private enum JettState {Wander, Dash, Slowed}
    private JettState currentState = JettState.Wander;

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
        cdTime += Time.deltaTime;
        stateTimer -= Time.deltaTime;

        if (currentKnife == 1)
        {
            sr.sprite = aKnife;
        }
        if (currentKnife == 2)
        {
            sr.sprite = bKnife;
        }
        else
        {
            sr.sprite = noKnife;
        }


            switch (currentState)
            {
                case JettState.Wander:
                    WanderUpdate();
                    break;
                case JettState.Dash:
                    DashUpdate();
                    break;
                case JettState.Slowed:
                    SlowedUpdate();
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
        if (collision.gameObject.CompareTag("Wall"))
        {
            moveDir = Vector2.Reflect(moveDir, collision.contacts[0].normal);
        }

        if (collision.gameObject.CompareTag("Sage"))
        {
            moveDir = Vector2.Reflect(moveDir, collision.contacts[0].normal);
        }

        if (collision.gameObject.CompareTag("Orb"))
        {
            currentKnife = 2;
            Destroy(collision.gameObject);
        }
    }

    private void WanderUpdate()
    {
        Move(moveSpeed);
 
        if (cdTime >= dashCooldown)
        {
            dashTime = dashDuration;
            ChangeState(JettState.Dash);
        }
    }

    private void DashUpdate()
    {
        dashTime -= Time.deltaTime;
        Move(dashSpeed);

        if (dashTime <= 0)
            {
                cdTime = 0f;
                ChangeState(JettState.Wander);
            }
    }

    private void SlowedUpdate()
    {
        Move(moveSpeed * slowMultiplier);

        if (stateTimer <= 0)
        {
            ChangeState(JettState.Wander);
        }
    }
    public void Slow()
    {
        ChangeState(JettState.Slowed);
    }

    void ChangeState(JettState newState)
        {
            currentState = newState;

        switch (newState)
        {
            case JettState.Slowed:
                stateTimer = slowDuration;
                break;
        }
        }


}
