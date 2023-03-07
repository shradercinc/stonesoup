using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetRadius : MonoBehaviour
{
    MagnetSlimeN MagSlime;
    private Transform parentPos;
    // Start is called before the first frame update
    void Start()
    {
        parentPos = GetComponentInParent<Transform>();
        MagSlime = GetComponentInParent<MagnetSlimeN>();
        MagSlime.detectRadius = GetComponent<CircleCollider2D>().radius;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var OtherTile = collision.gameObject.GetComponent<Tile>();
        if (OtherTile != null)
        {
            if (OtherTile.hasTag(TileTags.Player))
            {
                print("Found Player");
                MagSlime.MagTarget = collision.gameObject;
                MagSlime.hasMagTarget = true;
            }
        }
        transform.position = parentPos.position;
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
