using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pg2259UltimateEgg : pg2259UltimateShield
{
    bool isShooting = false;
    public override void pickUp(Tile tilePickingUsUp)
    {

        base.pickUp(tilePickingUsUp);
        isShooting = true;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isShooting && other.gameObject.GetComponent<Tile>() != null)
        {
            Tile otherTile = other.gameObject.GetComponent<Tile>();
            if (otherTile != _tileHoldingUs && !otherTile.hasTag(TileTags.CanBeHeld) && otherTile.hasTag(TileTags.Enemy))
            {
                otherTile.takeDamage(this, 1);
                //otherTile.addForce((other.transform.position - _tileHoldingUs.transform.position).normalized * damageForce);
                Destroy(this.gameObject);
            }
        }
    }
}
