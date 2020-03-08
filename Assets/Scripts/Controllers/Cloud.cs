using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] private Color stormColor;
    [SerializeField] public float spawnInterval;
    [SerializeField] private float SecondsToColorReset;
    SpriteRenderer sr;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        time = Random.Range(0, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        sr.color = Color.Lerp(Color.white, stormColor, time);
        if (time < 1)
        {
            time += Time.deltaTime / SecondsToColorReset;
        }
        else
        {
            Instantiate(GameAssets.I.enemies[Random.Range(0, GameAssets.I.enemies.Count)], transform.position, Quaternion.identity);
            sr.color = Color.white;
            time = Random.Range(0, 0.3f);
        }
    }
}
