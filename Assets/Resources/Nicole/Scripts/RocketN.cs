using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketN : Tile
{
    [SerializeField] float speed = 2f;
    [SerializeField] float startSpeed = 50f;
    [SerializeField] GameObject Explosion;
    private Rigidbody2D rb;
    public HashSet<GameObject> Launcher = new HashSet<GameObject>();
    private bool armed = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddRelativeForce(Vector2.right * speed * startSpeed);
        //StartCoroutine(arm(0.2f));

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!armed)
        {
            //Launcher.Add(collision.gameObject);
        }
        var OtherTile = collision.gameObject.GetComponent<Tile>();
        if (armed && OtherTile != null && !Launcher.Contains(collision.gameObject))
        {
            if (OtherTile.hasTag(TileTags.Wall | TileTags.Player | TileTags.Creature | TileTags.Enemy | TileTags.Friendly))
            {
                print("Explode!");
                Instantiate(Explosion, transform.position, transform.rotation);
                Destroy(this.gameObject);
            }
        }
    }
    /*
    IEnumerator arm(float armTime)
    {
        float timer = 0;
        while (timer < armTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        armed = true;
    }
    */

    // Update is called once per frame
    void Update()
    {
        rb.AddRelativeForce(Vector2.right * speed);
    }

    void LateUpdate()
    {
        armed = true;
    }

}
