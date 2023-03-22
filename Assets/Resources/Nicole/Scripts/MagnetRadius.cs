using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetRadius : MonoBehaviour
{
    private MagnetSlimeN MagSlime;
    private Rigidbody2D ParentRb;
    //private Transform parentPos;
    //controlling magnet animation
    [SerializeField] private GameObject parentGameO;
    [SerializeField] private GameObject MagnetArt;
    private MagnetN Magnet;

    [SerializeField] private float MagSlimeSpeed = 3;
    [SerializeField] private float Power = 10f;
    [SerializeField] private float itemException = 3f;
    [SerializeField] private float MinRange = 2f;

    // Start is called before the first frame update
    void Start()
    {
        //parentPos = GetComponentInParent<Transform>();
        MagSlime = GetComponentInParent<MagnetSlimeN>();
        MagSlime.detectRadius = GetComponent<CircleCollider2D>().radius;
        ParentRb = GetComponentInParent<Rigidbody2D>();
        Magnet = MagnetArt.GetComponent<MagnetN>();
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var OtherTile = other.gameObject.GetComponent<Tile>();
        if (OtherTile != null)
        {
            if (OtherTile.hasTag(TileTags.CanBeHeld))
            {
                other.GetComponent<Collider2D>().isTrigger = true;
            }
        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var OtherTile = other.gameObject.GetComponent<Tile>();
        if (OtherTile != null)
        {
            if (OtherTile.hasTag(TileTags.Player | TileTags.Enemy | TileTags.Friendly | TileTags.CanBeHeld) && other.gameObject != parentGameO)
            {
                var OtherRb = other.gameObject.GetComponent<Rigidbody2D>();
                var OtherCollider = other.gameObject.GetComponent<Collider2D>();
                Magnet.attracting = true;
                Vector2 dir = transform.position - other.transform.position;
                Vector2 NDir = dir.normalized;
                //print(-NDir);
                if (OtherTile.hasTag(TileTags.Player))
                {
                    MagSlime.canSeePlayer = true;
                    ParentRb.AddForce((NDir * MagSlimeSpeed));
                }
                if (OtherTile.hasTag(TileTags.CanBeHeld))
                {
                    if (!OtherTile.isBeingHeld)
                    {
                        //print("Tugging exception " + other);
                        if (dir.magnitude > MinRange)
                        {
                            OtherRb.AddForce((NDir * Power) / itemException);
                        }

                        OtherCollider.isTrigger = false;
                    }
                    else
                    {
                        OtherCollider.isTrigger = true;
                    }

                    
                }
                else
                {
                    //print("Tugging general " + other);
                    if (dir.magnitude > MinRange)
                    {
                        OtherRb.AddForce((NDir * Power));
                    }
                }
            }
            else
            {
                Magnet.attracting = false;
            }

        }
        else 
        {
            Magnet.attracting = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
            //old code transfering list/target structures to parent
            /*
            var OtherTile = collision.gameObject.GetComponent<Tile>();
            if (OtherTile != null)
            {
                if (OtherTile.hasTag(TileTags.Player | TileTags.Enemy | TileTags.Friendly) || 
                   (OtherTile.hasTag(TileTags.CanBeHeld) && !OtherTile.isBeingHeld))
                {
                    MagSlime.MagTargets.Add(collision.gameObject);

                }
                if (OtherTile.hasTag(TileTags.Player))
                {
                    MagSlime.MagTarget = collision.gameObject;
                    //MagSlime.hasMagTarget = true;
                }
            }
            */
        }

    

    // Update is called once per frame
    void Update()
    {
    }
}
