using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public TowerBlock blockPrefab;
    static Tower instance;
    List<TowerBlock> towerBlocks;
    const float blockwidth = 1.5f;
    const float blockheight = 1.0f;
    float highest = 1.0f;
    float lowest = 1.0f;
    public UnityEngine.UI.Text score;
    int scoreValue;
    AudioSource sounds;
    public AudioClip[] fanfares;
    void Start()
    {
       gameOver = false;
    sounds = GetComponent<AudioSource>();
        //sounds.PlayOneShot(fanfares[0]);
        towerBlocks = new List<TowerBlock>();
        instance = this;
        Populate(0.0f, 1.0f, 1);
    }

    internal static Vector2 GetRange()
    {
        if (instance == null)
        {
            return new Vector2(1.0f, 1.0f);
        }
        return new Vector2(instance.lowest, instance.highest);
    }

    static public TowerBlock ViewGetBlock(float x, float y)
    {
        Vector3 v = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 10));
        RaycastHit2D hit = Physics2D.Raycast(v, Vector2.zero);

        if (hit.collider != null)
        {
            TowerBlock b = hit.collider.GetComponent<TowerBlock>();
            if (b != null) { return b; }
        }
        return null;
    }

    static public TowerBlock WorldGetBlock(float x, float y)
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector3(x, y, 10), Vector2.zero);

        if (hit.collider != null)
        {
            TowerBlock b = hit.collider.GetComponent<TowerBlock>();
            if (b != null) { return b; }
        }
        return null;
    }

    internal static void Complete(TowerBlock towerBlock)
    {
        int newScore = Mathf.RoundToInt(towerBlock.transform.position.y * 10);
        if (instance.scoreValue < newScore)
        {
            instance.scoreValue = newScore;
            instance.score.text = " " + instance.scoreValue;
        }
       // throw new NotImplementedException();
    }

    static public void Populate(float x, float y, int i)
    {
        TowerBlock block = WorldGetBlock(x, y);
        if (block == null)
        {
            block = Instantiate(instance.blockPrefab, new Vector3(x, y, 0.0f), Quaternion.identity) as TowerBlock;
            block.hpos = i;
            instance.towerBlocks.Add(block);
            instance.highest = y;
            CameraDirector.SetNewMax(instance.highest);
        }
    }


    internal static void BlockFullDelete(TowerBlock towerBlock)
    {
        instance.towerBlocks.Remove(towerBlock);
        Destroy(towerBlock.gameObject);
    }

    internal static void BlockDelete(TowerBlock towerBlock)
    {
        {
            TowerBlock tblock_l = WorldGetBlock(towerBlock.transform.position.x + blockwidth, towerBlock.transform.position.y);
            TowerBlock tblock_r = WorldGetBlock(towerBlock.transform.position.x - blockwidth, towerBlock.transform.position.y);
            TowerBlock tblock_u = WorldGetBlock(towerBlock.transform.position.x, towerBlock.transform.position.y + blockheight);
            TowerBlock tblock_d = WorldGetBlock(towerBlock.transform.position.x, towerBlock.transform.position.y - blockheight);
            if (tblock_l != null && tblock_l.buildStatus == 0) { tblock_l.CheckBuildPlatform(); }
            if (tblock_r != null && tblock_r.buildStatus == 0) { tblock_r.CheckBuildPlatform(); }
            if (tblock_u != null && tblock_u.buildStatus == 0) { tblock_u.CheckBuildPlatform(); }
            if (tblock_d != null && tblock_d.buildStatus == 0) { tblock_d.CheckBuildPlatform(); }

        }

        Queue<TowerBlock> integrityQue = new Queue<TowerBlock>();
        List <TowerBlock > checkList = new List<TowerBlock>();
        int needed = 0;
        foreach (TowerBlock towerBlock1 in instance.towerBlocks)
        {
           if (towerBlock1.buildStatus == towerBlock1.buildMax) { needed++; }
        }
        integrityQue.Enqueue(instance.towerBlocks[0]);
        while(integrityQue.Count > 0)
        {
            TowerBlock tblock = integrityQue.Dequeue();
            TowerBlock tblock_l = WorldGetBlock(tblock.transform.position.x + blockwidth, tblock.transform.position.y);
            TowerBlock tblock_r = WorldGetBlock(tblock.transform.position.x - blockwidth, tblock.transform.position.y);
            TowerBlock tblock_u = WorldGetBlock(tblock.transform.position.x, tblock.transform.position.y + blockheight);
            TowerBlock tblock_d = WorldGetBlock(tblock.transform.position.x, tblock.transform.position.y - blockheight);

            if (tblock_l != null && tblock_l.buildStatus == tblock_l.buildMax && !checkList.Contains(tblock_l)) { integrityQue.Enqueue(tblock_l); }
            if (tblock_r != null && tblock_r.buildStatus == tblock_r.buildMax && !checkList.Contains(tblock_r)) { integrityQue.Enqueue(tblock_r); }
            if (tblock_u != null && tblock_u.buildStatus == tblock_u.buildMax && !checkList.Contains(tblock_u)) { integrityQue.Enqueue(tblock_u); }
            if (tblock_d != null && tblock_d.buildStatus == tblock_d.buildMax && !checkList.Contains(tblock_d)) { integrityQue.Enqueue(tblock_d); }
            checkList.Add(tblock);
        }
        if (checkList.Count < needed)
        {
           GameOver();
        }
    }

    static public bool gameOver = false;
    public GameObject gameoverPanel;
    static public void GameOver()
    {
        gameOver = true;
        instance.gameoverPanel.SetActive(true);
        instance.sounds.PlayOneShot(instance.fanfares[1]);
        SceneSwapper.EnableLoading();
    }
}
