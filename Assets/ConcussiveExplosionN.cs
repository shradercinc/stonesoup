using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcussiveExplosionN : Tile
{
    [SerializeField] Sprite[] frames;
    [SerializeField] float animSpeed = 0.166f;
    private int frameSelect = 0;
    private Transform pos;
    [SerializeField] float Power = 30f;
    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FrameAnim(animSpeed));
        pos = GetComponent<Transform>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var OtherTile = other.gameObject.GetComponent<Tile>();
        if (OtherTile != null)
        {
            if (!OtherTile.isBeingHeld)
            {
                OtherTile.health--;
            }    
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        var OtherTile = other.gameObject.GetComponent<Tile>();
        if (OtherTile != null)
        {
            if (!OtherTile.isBeingHeld && !OtherTile.hasTag(TileTags.Wall | TileTags.Dirt))
            {
                var VictimPos = other.transform.position;
                var ExplosionPos = pos.position;
                Vector3 dir = (VictimPos - ExplosionPos);
                dir = dir.normalized;
                other.GetComponent<Rigidbody2D>().AddForce(dir * Power, ForceMode2D.Impulse);
            }
        }
    }

    IEnumerator FrameAnim(float frameTime)
    {
        float T = 0;
        while (T < frameTime)
        {
            T += Time.deltaTime;
            yield return null;
        }
        frameSelect++;
        if (frameSelect > frames.Length - 1)
        {
            frameSelect = frames.Length - 1;
            Destroy(this.gameObject);
        }
        GetComponent<SpriteRenderer>().sprite = frames[frameSelect];
        StartCoroutine(FrameAnim(animSpeed));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
