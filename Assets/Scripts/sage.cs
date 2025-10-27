using System.Collections;
using System.Linq;
using UnityEngine;

public class sage : MonoBehaviour
{
    [Header("movement")]
    public float moveSpeed = 5f;

    private Vector2 moveDir;
    private Rigidbody2D rb;
    public bool isDead = false;
    public jett jett;
    private float time = 0f;

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
    private bool isGoingBack = false;


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
        time -= Time.deltaTime;

      
        sage[] allSages = FindObjectsOfType<sage>();
        int aliveCount = allSages.Count(s => !s.isDead);

   
        if (aliveCount == 1 && !isDead && currentState != SageState.LastSage)
        {
            ChangeState(SageState.LastSage);
        }


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
                sr.sprite = ability;
                if (time <= 0)
                {
                    orb();
                    ChangeState(SageState.Wander);
                }
                break;
            case SageState.LastSage:
                lastSage();
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
            time = 0.5f;
            Destroy(collision.gameObject);
            ChangeState(SageState.Orb);
        }

        if (collision.gameObject.CompareTag("Sage"))
        {
            moveDir = Vector2.Reflect(moveDir, collision.contacts[0].normal);


        }
        if (collision.gameObject.CompareTag("Omen"))
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

    void lastSage()
    {
        sr.sprite = angry;
        jett.isDead = true;
        if (!isGoingBack)
        {
            StartCoroutine(BackToNormalAfterDelay());
        }

    }

    void ChangeState(SageState newState)
    {
        currentState = newState;
    }
    IEnumerator BackToNormalAfterDelay()
    {
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(2f);
        isGoingBack = true;


    
        sage[] allSages = FindObjectsOfType<sage>();
        foreach (sage s in allSages)
        {
            if (s.isDead)
            {
                s.isDead = false;
                s.ChangeState(SageState.Wander);
            }
        }

   
        ChangeState(SageState.Wander);
        isGoingBack = false;

    }

}
