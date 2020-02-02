using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDirector : MonoBehaviour
{
    float verticalTarget = 0.0f;
    float verticalTargetMin = 0.0f;
    float verticalTargetMax = 1.0f;
    const float hairline = 0.01f;
    static CameraDirector instance;
    public GameObject dangerUp;
    public GameObject dangerDown;
    float dangerUpDuration = 0.0f;
    float dangerDownDuration = 0.0f;
    float screenShake = 0.0f;
    public Color startColor;
    public Color midColor;
    public Color endColor;
    public float midHeight;
    public float endHeight;
    public SpriteRenderer spaceRender;
    void Start()
    {
        verticalTarget = transform.position.y;
        verticalTargetMin = transform.position.y;
        verticalTargetMax = transform.position.y+5.0f;
        instance = this;


    }

    Color GetBgColor(float yy)
    {
        if (yy < midHeight)
        {
            return Color.Lerp(startColor, midColor, Mathf.InverseLerp(10.0f, midHeight, yy));
        } else
        {
            return Color.Lerp(midColor, endColor, Mathf.InverseLerp(midHeight + endHeight*0.5f, endHeight, yy));
        }
    }
    // Update is called once per frame
    void Update()
    {
        Camera.main.backgroundColor = GetBgColor(transform.position.y);
        if (transform.position.y > midHeight)
        {
            spaceRender.color = Color.Lerp(Color.clear, Color.white, Mathf.InverseLerp(midHeight, endHeight,transform.position.y));
        }
        if (transform.position.y < verticalTarget + hairline || transform.position.y > verticalTarget - hairline)
        {
            float newPos = Mathf.Lerp(transform.position.y, verticalTarget, 0.2f);
            transform.position = new Vector3(transform.position.x, newPos, transform.position.z);
        }
        if (dangerDownDuration > 0.0f)
        {
            dangerDownDuration -= Time.deltaTime;
            int d = Mathf.RoundToInt(dangerDownDuration * 10.0F) % 2;
            dangerDown.SetActive((dangerDownDuration > 0.0f && d == 0));
        }
        if (dangerUpDuration > 0.0f)
        {
            dangerUpDuration -= Time.deltaTime;
            int d = Mathf.RoundToInt(dangerUpDuration * 10.0F) % 2;
            dangerUp.SetActive((dangerUpDuration > 0.0f && d == 0));
        }

        if (screenShake > 0.0f)
        {
            screenShake -= Time.deltaTime;
            transform.position = new Vector3(Mathf.Sin(screenShake)*0.1f * screenShake, transform.position.y + Mathf.Sin(screenShake) * 0.1f * screenShake, transform.position.z);
        }
        else
        {
            screenShake = 0.0f;
            transform.position = new Vector3(0.0f, transform.position.y, transform.position.z);
        }
    }

    static public void MoveDelta(float y)
    {
        
        Debug.Log(instance.verticalTarget + " + " + y + " -> " + (instance.verticalTarget + y));
        instance.verticalTarget = instance.transform.position.y + y;
        Debug.DrawLine(new Vector3(-20.0f, instance.verticalTarget, 5.0f), new Vector3(20.0f, instance.verticalTarget, 5.0f), Color.red, 0.1f);
        if (instance.verticalTargetMin > instance.verticalTarget) instance.verticalTarget = instance.verticalTargetMin;
        if (instance.verticalTargetMax < instance.verticalTarget) instance.verticalTarget = instance.verticalTargetMax;
    }

    static public void SetNewMax(float y)
    {
        instance.verticalTargetMax = instance.verticalTargetMax > y ? instance.verticalTargetMax : y;
    }

    static public void Danger(float y)
    {
        float delta = instance.transform.position.y-y;
        if (Mathf.Abs(delta) > 7.0f)
        {
            if (Mathf.Sign(delta) > 0.0f)
            {
                instance.dangerDownDuration = 2.0f;
            } else
            {
                instance.dangerUpDuration = 2.0f;
            }
        }
    }

    static public void ScreenShake(float magnitude)
    {
        instance.screenShake = instance.screenShake > magnitude ? instance.screenShake : magnitude;
        //instance.transform.position = new Vector3();
    }


}
