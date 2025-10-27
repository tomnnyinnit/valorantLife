using UnityEngine;

public class smoke : MonoBehaviour
{
    public float time = 10f; 
    public Vector3 finalScale = new Vector3(3f, 3f, 3f); 
    private float timer;

    private Vector3 initialScale;

    void Start()
    {
        timer = time;
        initialScale = transform.localScale; 
    }

    void Update()
    {
        timer -= Time.deltaTime;
        float t = 1f - (timer / time); 
        transform.localScale = Vector3.Lerp(initialScale, finalScale, t);

      
        if (timer <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
