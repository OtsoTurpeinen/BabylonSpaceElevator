using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSoundHelper : MonoBehaviour
{
    public AudioClip[] clips;
    AudioSource sauce;
    public int oneshot = -1;
    void Start()
    {
        sauce = GetComponent<AudioSource>();
    }
    void Update()
    {
        if (oneshot > -1)
        {
            sauce.PlayOneShot(clips[oneshot]);
            oneshot = -1;
        }
    }


}
