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
    [SerializeField] private float MinRange = 2f;

    // Start is called before the first frame update
    void Start()
    {
        //parentPos = GetComponentInParent<Transform>();
        MagSlime = GetComponentInParent<MagnetSlimeN>();
        MagSlime.detectRadius = GetComponent<CircleCollider2D>().radius;
        ParentRb = parentGameO.GetComponent<Rigidbody2D>();
        Magnet = MagnetArt.GetComponent<MagnetN>();
        
    }
   

    private void OnTriggerStay2D(Collider2D other)
    {
        var OtherTile = other.gameObject.GetComponent<Tile>();
        if (OtherTile != null)
        {
            if (OtherTile.hasTag(TileTags.Player | TileTags.Enemy | TileTags.Friendly) && other.gameObject != parentGameO)
            {
                var OtherRb = other.gameObject.GetComponent<Rigidbody2D>();
                Magnet.attracting = true;

                Vector2 dir =  transform.position - other.transform.position;
                Vector2 NDir = dir.normalized;

                if (OtherTile.hasTag(TileTags.Player))
                {
                    MagSlime.canSeePlayer = true;
                    var selfForce = (NDir * Power) * MagSlimeSpeed;
                    ParentRb.AddForce(selfForce);
                }

                if (dir.magnitude > MinRange)
                {
                    OtherRb.AddForce((NDir * Power));
                }
            }

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
    private void LateUpdate()
    {
        Magnet.attracting = false;
    }
}
