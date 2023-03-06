using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTimeTele : Tile
{
    private Camera Cam;
    [SerializeField] float accuracy = 1;
    // Start is called before the first frame update
    void Start()
    {
        Cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }


    public override void useAsItem(Tile tileUsingUs)
    {
        //print("Using");
        bool teleport = false;
        base.useAsItem(tileUsingUs);
        var MousePoint = Input.mousePosition;
        MousePoint = Cam.ScreenToWorldPoint(MousePoint);
        //print(MousePoint);
        int i = 0;
        while (teleport == false || i < 100)
        {
            Vector3 TelePoint = new Vector3(MousePoint.x + Random.Range(-accuracy,accuracy) , MousePoint.y + Random.Range(-accuracy,accuracy), MousePoint.z);
            print(TelePoint);
            if (TelePoint.x < 79 && TelePoint.x > 0 && TelePoint.y > 1 && TelePoint.y < 63)
            {
                var hit = (Physics2D.Raycast(new Vector2(TelePoint.x, TelePoint.y), Vector2.zero));
                if (hit)
                {
                    Tile resualt = hit.collider.gameObject.transform.GetComponent<Tile>();
                    if (resualt != null)
                    {
                        if (resualt.hasTag(TileTags.Wall | TileTags.Enemy))
                        {
                            teleport = false;
                        }
                        else
                        {
                            teleport = true;
                        }
                    }
                }
                else
                {
                    teleport = true;
                }
            }
            if (teleport == true)
            {
                transform.parent.position = new Vector3(TelePoint.x, TelePoint.y, -0.1f);
                Destroy(this.gameObject);
                return;
            }
            i++;
        }
    }

    private void inHand()
    {

    }

    void Update()
    {
        if (_tileHoldingUs != null)
        {
            inHand();
        }
    }
}
