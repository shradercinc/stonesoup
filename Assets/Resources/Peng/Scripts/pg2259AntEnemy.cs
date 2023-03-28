using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class pg2259AntEnemy : MonoBehaviour
{
    private int level = 1;
    public int getLevel() { return level; }
    public void IncreaseLevel()
    {
        level++;
        transform.localScale = transform.localScale * 1.5f;
        GetComponent<Tile>().restoreAllHealth();
        GetComponent<Tile>().health++;
    }
    private void Sacrifice()
    {
        Destroy(gameObject);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        pg2259AntEnemy otherAnt = collision.gameObject.GetComponent<pg2259AntEnemy>();
        if (otherAnt == null) return;
        if(otherAnt.getLevel() >= level) { 
        otherAnt.IncreaseLevel();
        Sacrifice();
        }
    }
}
