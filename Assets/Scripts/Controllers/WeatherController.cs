using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    [SerializeField] private GameObject CloudPrefab;
    private Color normalColor;
    [SerializeField] private Color stormColor;
    [SerializeField] private float cloudMoveSpeed;

    [SerializeField] List<GameObject> spawnedClouds = new List<GameObject>();
    [SerializeField] List<GameObject> cloudsToRemove = new List<GameObject>();
    [SerializeField] private float cloudSpawnInterval;
    float timer = float.MaxValue;

    // Start is called before the first frame update
    void Start()
    {
        normalColor = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= Random.Range(cloudSpawnInterval-2, cloudSpawnInterval+2))
        {
            SpawnCloud();
            timer = 0;
        }

        foreach(GameObject g in spawnedClouds)
        {
            if (g != null)
            {
                g.transform.Translate(Vector3.right * cloudMoveSpeed * Time.deltaTime);
            }
            else
            {
                cloudsToRemove.Add(g);
            }
        }
        foreach(GameObject g in cloudsToRemove)
        {
            spawnedClouds.Remove(g);
        }
        cloudsToRemove.Clear();
    }
    public void SpawnCloud()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
        pos.y = Random.Range(1f, 4f);
        GameObject go = Instantiate(CloudPrefab, pos, Quaternion.identity);
        spawnedClouds.Add(go);
        
    }
}
