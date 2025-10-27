using UnityEngine;

public class orbSpawner : MonoBehaviour
{
    [Header("Orb")]
    public GameObject orbPrefab;
    public float spawnInterval = 5f;
    public int maxOrbs = 5;

    [Header("Spawn Area")]
    public Vector2 areaMin;
    public Vector2 areaMax;

    private float timer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f && CountOrbs() < maxOrbs)
        {
            SpawnOrb();
            timer = spawnInterval;
        }
    }
    void SpawnOrb()
    {
        Vector2 spawnPos = new Vector2(
            Random.Range(areaMin.x, areaMax.x),
            Random.Range(areaMin.y, areaMax.y)
        );

        Instantiate(orbPrefab, spawnPos, Quaternion.identity);
    }

    int CountOrbs()
    {
        return GameObject.FindGameObjectsWithTag("Orb").Length;
    }
}
