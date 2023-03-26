using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NSpikeTrap : Tile
{
    private bool isActive = false;
    [SerializeField] Sprite active;
    [SerializeField] Sprite inactive;
    private SpriteRenderer SprRen;
    // Start is called before the first frame update
    void Start()
    {
        SprRen = GetComponent<SpriteRenderer>();
    }
    public override void init()
    {
        base.init();
        // Fix the sorting issue.
        _sprite.sortingLayerID = SortingLayer.NameToID("Below Floor");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        print("enter:");
        var otherTile = other.GetComponent<Tile>();
        if (otherTile != null)
        {
            print(otherTile);
            print(otherTile.tags);
            if (otherTile.hasTag(TileTags.Player))
            {
                print("Stabby!");
                SprRen.sprite = active;
                isActive = true;
                otherTile.takeDamage(this, 1);
            }
        }
        print("");
        print("");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var otherTile = other.GetComponent<Tile>();
        if (otherTile != null)
        {
            if(otherTile.hasTag(TileTags.Player))
            {
                SprRen.sprite = inactive;
                isActive = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
