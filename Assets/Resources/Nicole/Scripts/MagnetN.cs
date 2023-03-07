using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetN : MonoBehaviour
{
    public bool attracting;
    private SpriteRenderer Ren;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite[] Frames;
    [SerializeField] float animSpeed = 0.083f;
    private float animTimer = 0;
    private int currentFrame = 0;
    // Start is called before the first frame update
    void Start()
    {
        Ren = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!attracting)
        {
            Ren.sprite = defaultSprite;
            animTimer = 0;
        }
        else
        {
            animTimer += Time.deltaTime;
            if (animTimer >= animSpeed)
            {
                currentFrame++;
                animTimer = 0;
                if (currentFrame > Frames.Length - 1)
                {
                    currentFrame = 0;
                }
            }
            Ren.sprite = Frames[currentFrame];
        }
    }
}
