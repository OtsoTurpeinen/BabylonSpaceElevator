using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacgroundSlow : MonoBehaviour
{
    Vector3 startpos;
    Vector3 camerastartpos;
    Camera currentCamera;
    public float fscale = 0.0f;
    void Start()
    {
        currentCamera = Camera.main;
        startpos = transform.position;
        camerastartpos = currentCamera.transform.position;
    }
    void Update()
    {
        float ydiff = camerastartpos.y - currentCamera.transform.position.y;
        transform.position = startpos + new Vector3(0.0f, ydiff * fscale, 0.0f);
        
    }
}
