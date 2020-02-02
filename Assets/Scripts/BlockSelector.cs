using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSelector : MonoBehaviour
{
    bool isVisible = false;
    int currentBuildStatus = -1;
    TowerBlock currentBlock = null;
    SpriteRenderer spriteRenderer;
    public Sprite[] swipes;
    public GameObject[] swipeParticles;
    AudioSource swiperAudio;
    public AudioClip[] swipeClips;

    void Start()
    {
        swiperAudio = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }
    public bool NewPosition(float x, float y)
    {
        Debug.Log(x + " : " + y);
        TowerBlock block = Tower.ViewGetBlock(x, y);
        if (block != null)
        {
            spriteRenderer.enabled = true;
            if (block != currentBlock)
            {
                currentBlock = block;
                isVisible = true;
                transform.position = new Vector2(currentBlock.transform.position.x, currentBlock.transform.position.y);
                if (currentBlock.buildStatus == block.buildMax)
                {
                    spriteRenderer.sprite = swipes[4];
                } else
                {
                    if (currentBlock.damageState)
                        spriteRenderer.sprite = swipes[currentBlock.buildSequence[currentBlock.repairStatus]];
                    else
                        spriteRenderer.sprite = swipes[currentBlock.buildSequence[currentBlock.buildStatus]];
                }
            }
            return true;
        } else
        {
            spriteRenderer.enabled = false;
            return false;
        }
    }

    internal void Swipe(int v)
    {
        if (currentBlock.damageState)
        {
            currentBlock.Repair();
            Instantiate(swipeParticles[v],currentBlock.transform.position,Quaternion.identity);
            swiperAudio.PlayOneShot(swipeClips[currentBlock.buildStatus]);
        } else if (currentBlock.buildStatus < currentBlock.buildMax)
        {
            if (v == currentBlock.buildSequence[currentBlock.buildStatus])
            {
                Instantiate(swipeParticles[v], currentBlock.transform.position, Quaternion.identity);
                swiperAudio.PlayOneShot(swipeClips[currentBlock.buildStatus] );

                currentBlock.Progress();
                if (currentBlock.buildStatus == currentBlock.buildMax)
                {
                    spriteRenderer.sprite = swipes[4];
                }
                else
                {

                    spriteRenderer.sprite = swipes[currentBlock.buildSequence[currentBlock.buildStatus < currentBlock.buildSequence.Length ? currentBlock.buildStatus : currentBlock.buildMax - 1]];
                }
            } else
            {
                currentBlock.Damage();
                swiperAudio.PlayOneShot(swipeClips[4]);

                spriteRenderer.sprite = swipes[currentBlock.buildSequence[currentBlock.buildStatus < currentBlock.buildSequence.Length ? currentBlock.buildStatus : currentBlock.buildMax - 1]];
            }
        }
    }
}
