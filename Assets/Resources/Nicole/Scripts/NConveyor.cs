using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NConveyor : Tile
{
    public float ConveyorSpeed = 0.1f;
    public float ConveyDirection = 4;
    [SerializeField] Sprite[] frames;
    [SerializeField] float animSpeed = 0.083f;
    [SerializeField] int frameSelect = 0;
    public static HashSet<GameObject> onConveyer = new HashSet<GameObject>();
    //12 frames
    //convey direction can be 1,2,3,4 
    // Start is called before the first frame update
    void Start()
    {

        transform.rotation = Quaternion.Euler(0, 0, 90 * ConveyDirection);
        StartCoroutine(FrameAnim(animSpeed));
        GetComponent<SpriteRenderer>().sortingLayerName = "Floor";

    }

    public override void init()
    {
        base.init();
        // Fix the sorting issue.
        _sprite.sortingLayerID = SortingLayer.NameToID("Below Floor");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var OtherTile = other.gameObject.GetComponent<Tile>();
        if (OtherTile != null)
        {
            if (!OtherTile.hasTag(TileTags.Wall | TileTags.Dirt))
            {
                if (OtherTile.isBeingHeld == false)
                {   
                    if (!onConveyer.Contains(other.gameObject))
                    {
                        //print(other.gameObject + "not on list");
                        other.transform.position += transform.right * ConveyorSpeed;
                        onConveyer.Add(other.gameObject);
                    }

                }
            }
        }
    }

    private void LateUpdate()
    {
        onConveyer.Clear();
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
            frameSelect = 0;
        }
        GetComponent<SpriteRenderer>().sprite = frames[frameSelect];
        StartCoroutine(FrameAnim(animSpeed));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
