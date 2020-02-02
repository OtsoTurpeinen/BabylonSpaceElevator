using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapper : MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEngine.UI.Image fader;
    public bool bAllowLoading = true;
    bool bStart = false;
    bool bLoading = false;
    public float fade = 1.0f;
    static SceneSwapper instance;
    AudioSource sounds;
    public int scene = 1;

    void Start()
    {
        instance = this;
        sounds = GetComponent<AudioSource>();
    }

    static public void EnableLoading()
    {
        instance.bAllowLoading = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bAllowLoading) return;
        if (bLoading) return;
        if (Input.GetMouseButtonDown(0) && !bStart)
        {
            sounds.Play();
            bStart = true;
        }
        if (bStart)
        {
            if (fade < 1.0f) {
                fade += Time.deltaTime;
            } else
            {
                bLoading = true;
                LoadScene();
            }
        } else
        {
            if (fade > 0.0f)
            {
                fade -= Time.deltaTime;
            }
        }
        fader.color = Color.Lerp(Color.clear, Color.black, fade);
    }

    void LoadScene()
    {
        SceneManager.LoadScene(instance.scene);
        instance.bAllowLoading = false;
        instance.bStart = false;
        instance.bLoading = false;
    }
}
