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
    //12 frames
    //convey direction can be 1,2,3,4 
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, 90 * ConveyDirection);
        StartCoroutine(FrameAnim(animSpeed));

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Tile>() != null)
        {
            if (!other.gameObject.GetComponent<Tile>().hasTag(TileTags.Wall))
            {
                if (other.gameObject.GetComponent<Tile>().isBeingHeld == false)
                {
                    other.transform.position += transform.right * ConveyorSpeed;
                }
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
