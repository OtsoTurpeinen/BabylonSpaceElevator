using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : MonoBehaviour
{
    private BlockSelector blockSelector;
    private bool scrollmode = false;
    private bool swipeDown = false;
    private Vector2 startPos;
    private Vector2 endPos;
    //CameraDelta
    private float py;
    private float dy;
    const float swipeDistance = 50.0f;

    void Start()
    {
        blockSelector = GetComponent<BlockSelector>();
        py = Input.mousePosition.y;
        dy = 0.0f;
    }

    void Update()
    {
        if (Tower.gameOver) return;
        dy = py - Input.mousePosition.y;
        py = Input.mousePosition.y;

        if (Input.GetMouseButtonUp(0))
        {
            endPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            if (scrollmode)
            {
                scrollmode = false;
            }
            if (swipeDown)
            {
                float deltax = (startPos.x - endPos.x);
                float deltay = (startPos.y - endPos.y);
                if (Mathf.Abs(deltax) > Mathf.Abs(deltay))
                {
                    if (Mathf.Abs(deltax) > swipeDistance)
                    {
                        if (Mathf.Sign(deltax) > 0)
                        {
                            blockSelector.Swipe(3); //left
                        }
                        else
                        {
                            blockSelector.Swipe(1); //right
                        }
                    }
                } else
                {
                    if (Mathf.Abs(deltay) > swipeDistance)
                    {
                        if (Mathf.Sign(deltay) > 0)
                        {
                            blockSelector.Swipe(2); //down
                        }
                        else
                        {
                            blockSelector.Swipe(0); //up
                        }
                    }
                }
                swipeDown = false;
            }
        } else if (Input.GetMouseButtonDown(0))
        {
            scrollmode = !blockSelector.NewPosition(Input.mousePosition.x, Input.mousePosition.y);
            swipeDown = !scrollmode;
            startPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        } else if (scrollmode && Input.GetMouseButton(0))
        {
            float adjusted = dy;
            if (Application.platform == RuntimePlatform.Android)
            {
                adjusted *= 0.03f;
                float sign = Mathf.Sign(adjusted);
                adjusted *= adjusted * sign;
            }
            CameraDirector.MoveDelta(adjusted);
        }

    }
}
