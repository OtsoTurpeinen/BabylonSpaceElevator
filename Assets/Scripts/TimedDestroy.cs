using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    public float alive = 1.0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        alive -= Time.deltaTime;
        if (alive < 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
