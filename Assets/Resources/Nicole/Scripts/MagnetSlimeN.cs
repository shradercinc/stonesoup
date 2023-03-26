
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetSlimeN : BasicAICreature
{
    //Note revised code puts most of this important stuff in the MagnetRadius.cs code
    //[SerializeField] private float moveCounterMax = 2f;
    //[SerializeField] private float moveCounterMin = 0.5f;
    public GameObject MagTarget;
    public bool hasMagTarget = false;
    public float detectRadius;
    private Rigidbody2D rb;
    [SerializeField] private GameObject Magnet;
    [SerializeField] private GameObject MagRad;
    private Transform MagRadPos;
    //public List<GameObject> MagTargets = new List<GameObject>();

    public bool canSeePlayer = false;
    // Start is called before the first frame update
    public override void Start()
    {
        _targetGridPos = Tile.toGridCoord(globalX, globalY);
        moveSpeed = 0;
        rb = GetComponent<Rigidbody2D>();
        MagRadPos = MagRad.GetComponent<Transform>();
    }


    protected override void takeStep()
    {

    }
        // Update is called once per frame
    void Update()
    {
        MagRadPos.position = transform.position;
        /*
        //old list structure code !! Unstable 
        if (MagTargets.Count > 0)
        {
            Magnet.GetComponent<MagnetN>().attracting = true;
            for (int i = 0; i < MagTargets.Count - 1; i++)
            {
                Vector2 dir = transform.position - MagTargets[i].transform.position;
                if (dir.magnitude > detectRadius * 1.2f)
                {
                    MagTargets.Remove(MagTargets[i]);
                }
                else
                {
                    Vector2 NDir = dir.normalized;
                    //print(-NDir);
                    if (MagTargets[i].GetComponent<Tile>().hasTag(TileTags.Player))
                    {
                        canSeePlayer = true;
                        rb.AddForce((NDir * MagSlimeSpeed));

                    }
                    MagTargets[i].GetComponent<Rigidbody2D>().AddForce((NDir * Power));
                }
            }
        }
        else 
        {
            Magnet.GetComponent<MagnetN>().attracting = false;
        }


        //old player target code
        if (hasMagTarget)
        {
            Magnet.GetComponent<MagnetN>().attracting = true;
            Vector2 dir = transform.position - MagTarget.transform.position;
            
            print("direction : " + dir);
            print("Normalized : " + dir.normalized);
            print("Magnitude : " + dir.magnitude);
            print("Range : " + detectRadius);
            print("---------------------------------------");
            
            if (dir.magnitude > detectRadius * 1.2f)
            {
                hasMagTarget = false;
            }
            
            if (hasMagTarget)
            {

                Vector2 NDir = dir.normalized;
                print(-NDir);
                if (MagTarget.GetComponent<Tile>().hasTag(TileTags.Player))
                {
                    canSeePlayer = true;
                    rb.AddForce((NDir * MagSlimeSpeed));
                    
                }
                MagTarget.GetComponent<Rigidbody2D>().AddForce((NDir * Power));
            }
        }
        if (!hasMagTarget)
        {
            Magnet.GetComponent<MagnetN>().attracting = false;
        }
        */
         /*
        if (canSeePlayer == false) 
        {
            rb.velocity = rb.velocity * 0.9f;
        }
         */
 
    }

    private void LateUpdate()
    {
        canSeePlayer = false;
    }

    private void OnDestroy()
    {
        Destroy(Magnet);
        Destroy(MagRad);
    }
}
