using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardSpawner : MonoBehaviour
{
    public HazardBird[] birds;
    float chance = 0.1f;
    float timing = 2.0f;
    float currenttime = 0.0f;
    float semirandom = 0.0f;
    void Start()
    {
        
    }

    void Update()
    {
        if (Tower.gameOver) return;
        Vector2 minmax = Tower.GetRange();
        if (minmax.y > 6.0f)
        {
            currenttime += Time.deltaTime;
            if (currenttime >= timing)
            {
                currenttime -= timing;
                semirandom += chance;
                if (Random.value < semirandom)
                {
                    semirandom = 0.0f;
                    chance *= 1.1f;
                    SpawnBird();
                }
            }

        }


    }

    void SpawnBird()
    {
        Vector2 minmax = Tower.GetRange();
        float r = Random.Range(minmax.x+1.0f, minmax.y+1.0f);
        bool l = Random.Range(0.0f, 1.0f) > 0.5f;
        Vector3 p = new Vector3((l ? -1 : 1) * 15.0f, r, 0.0f);
        Instantiate(birds[Random.Range(0, birds.Length)], p, Quaternion.identity);

    }
}
