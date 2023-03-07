using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetSlimeN : BasicAICreature
{
    [SerializeField] private float moveCounterMax = 2f;
    [SerializeField] private float moveCounterMin = 0.5f;
    [SerializeField] private float MagSlimeSpeed = 3;
    public GameObject MagTarget;
    public bool hasMagTarget = false;
    public float detectRadius;
    [SerializeField] private float Power = 10f;
    [SerializeField] private GameObject Magnet;

    private bool canSeePlayer = false;
    // Start is called before the first frame update
    public override void Start()
    {
        _targetGridPos = Tile.toGridCoord(globalX, globalY);
        moveSpeed = MagSlimeSpeed;

    }



    protected override void takeStep()
    {

    }
        // Update is called once per frame
    void Update()
    {
        if (hasMagTarget)
        {
            Magnet.GetComponent<MagnetN>().attracting = true;
            Vector2 dir = transform.position - MagTarget.transform.position;
            /*
            print("direction : " + dir);
            print("Normalized : " + dir.normalized);
            print("Magnitude : " + dir.magnitude);
            print("Range : " + detectRadius);
            print("---------------------------------------");
            */
            if (dir.magnitude > detectRadius * 1.2f)
            {
                hasMagTarget = false;
            }
            
            if (hasMagTarget)
            {
                Vector2 NDir = dir.normalized;
                print(-NDir);
                MagTarget.GetComponent<Rigidbody2D>().AddForce((NDir * Power));
            }
        }
        if (!hasMagTarget)
        {
            Magnet.GetComponent<MagnetN>().attracting = false;
        }
    }
}
