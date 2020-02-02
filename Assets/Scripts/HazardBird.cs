using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardBird : MonoBehaviour
{
    public float moveSpeed;
    float direction;
    Rigidbody2D rb;
    public GameObject corpse;
    bool bdone = false;
    bool binit = false;
    SpriteRenderer sprite;
    
    void Start()
    {
        direction = -Mathf.Sign(transform.position.x);
        if (direction < 0.0f)
        {
            GetComponentInChildren<Transform>().rotation = Quaternion.Euler(0.0f, 0.0f, 180.0f);
            GetComponentInChildren<SpriteRenderer>().flipY = true;
        }
    }
    void FixedUpdate()
    {
        if (Tower.gameOver) return;
        if (!binit)
        {
            rb = GetComponent<Rigidbody2D>();
            binit = true;
        }
        if (!bdone)
        {
            rb.MovePosition(new Vector2(transform.position.x, transform.position.y) + new Vector2(moveSpeed * direction, 0.0f));
            if (transform.position.x*direction > 15.0f)
            {
                Destroy(gameObject);
            }
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (bdone) return;
        bdone = true;
        TowerBlock block = col.collider.gameObject.GetComponent<TowerBlock>();
        if (block != null)
        {
            block.Damage();
        }
        Instantiate(corpse, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
