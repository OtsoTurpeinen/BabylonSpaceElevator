using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBlock : MonoBehaviour
{
    internal int buildStatus = 0;
    internal int repairStatus = 0;
    public int buildMax = 3;
    internal int precievedStatus = 0;
    SpriteRenderer spriteRenderer;
    Animator animator;
    internal int hpos = 1;
    const float blockwidth = 1.5f;
    const float blockheight = 1.0f;
    public int[] buildSequence;
    internal bool damageState = false;
    public int unbuiltLayer;
    public int collisionLayer;
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer.sortingOrder = -(int)transform.position.y;
        gameObject.layer = unbuiltLayer;
    }
    void Update()
    {
        
    }

    internal void Progress()
    {
        if (buildStatus < buildMax)
        {
            gameObject.layer = collisionLayer;
            buildStatus++;
            animator.SetInteger("buildState",buildStatus);
            if (buildStatus == buildMax)
            {
                Complete();
            }
        }
    }

    internal void Repair()
    {
        animator.SetTrigger("repair");
        damageState = false;
    }

    void Complete()
    {
        //CameraDirector.MoveDelta(1.0f);
        Tower.Complete(this);
        //check up
        Tower.Populate(transform.position.x, transform.position.y + blockheight, hpos);
        if (hpos > 0)
        {
            Tower.Populate(transform.position.x - blockwidth, transform.position.y, hpos - 1);
        }
        if (hpos < 2)
        {
            Tower.Populate(transform.position.x + blockwidth, transform.position.y, hpos + 1);
        }
    }



    public void Damage()
    {
        if (buildStatus < buildMax)
        {
            buildStatus = 0;
            gameObject.layer = unbuiltLayer;
            animator.SetInteger("buildState", buildStatus);
            animator.SetTrigger("death");
            CameraDirector.ScreenShake(0.5f);
        } else
        {
            CameraDirector.Danger(transform.position.y);
            CameraDirector.ScreenShake(0.5f);
            if (damageState)
            {
                Kill();
            }
            else
            {
                damageState = true;
                animator.SetTrigger("damage");
            }
        }
    }

    void Kill()
    {
        buildStatus = 0;
        gameObject.layer = unbuiltLayer;
        animator.SetInteger("buildState", buildStatus);
        animator.SetTrigger("death");
        Tower.BlockDelete(this);
    }

    internal void CheckBuildPlatform()
    {
        int valid = 0;
        TowerBlock tblock_l = Tower.WorldGetBlock(transform.position.x + blockwidth, transform.position.y);
        TowerBlock tblock_r = Tower.WorldGetBlock(transform.position.x - blockwidth, transform.position.y);
        TowerBlock tblock_d = Tower.WorldGetBlock(transform.position.x, transform.position.y - blockheight);
        if (tblock_l != null && tblock_l.buildStatus == tblock_l.buildMax) { valid++; }
        if (tblock_r != null && tblock_r.buildStatus == tblock_r.buildMax) { valid++; }
        if (tblock_d != null && tblock_d.buildStatus == tblock_d.buildMax) { valid++; }
        if (valid == 0)
        {
            Tower.BlockFullDelete(this);
        }
    }
}
