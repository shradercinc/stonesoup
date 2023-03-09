using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*************************************************************************
/  ------------------------  "Mask of Monsters"  ----------------------- *
/                                                                        *
/  Consumable item that transforms the player into a nearby creature,    *
/  copying that creature's appearance, current health, and alignment     *
/  (friendly/enemy). Only works if the target creature has an animator   *
/  component. Lasts until the next floor.                                *
/                                                                        *
/************************************************************************/

public class jrr504_MaskOfMonsters : Tile
{

    private CircleCollider2D copyRange;
    private BoxCollider2D pickUpRange;
    private GameObject monsterInRange;

    // Disable creature checking range, enable pickup range
    void Awake() {
        pickUpRange = gameObject.GetComponent<BoxCollider2D>();
        copyRange = gameObject.GetComponent<CircleCollider2D>();
        copyRange.enabled = false;
    }

    public override void useAsItem(Tile tileUsingUs) {
        // Only use if there is a creature to transform into
        if (monsterInRange != null) {
            // Copy nearby creature's appearance and current health, then be consumed
            Animator yourAnimator = monsterInRange.GetComponent<Animator>();
            tileUsingUs.GetComponent<Animator>().runtimeAnimatorController = yourAnimator.runtimeAnimatorController;
            tileUsingUs.GetComponent<Tile>().health = monsterInRange.GetComponent<Tile>().health;

            // Align with enemies when copying an enemy, align with friendlies when copying a friendly
            if (monsterInRange.GetComponent<Tile>().hasTag(TileTags.Enemy)) {
                if (tileUsingUs.GetComponent<Tile>().hasTag(TileTags.Friendly)) {
                    tileUsingUs.GetComponent<Tile>().removeTag(TileTags.Friendly);
                }
                if (!tileUsingUs.GetComponent<Tile>().hasTag(TileTags.Enemy)) {
                    tileUsingUs.GetComponent<Tile>().addTag(TileTags.Enemy);
                }
            }
            else if (monsterInRange.GetComponent<Tile>().hasTag(TileTags.Friendly)) {
                if (tileUsingUs.GetComponent<Tile>().hasTag(TileTags.Enemy)) {
                    tileUsingUs.GetComponent<Tile>().removeTag(TileTags.Enemy);
                }
                if (!tileUsingUs.GetComponent<Tile>().hasTag(TileTags.Friendly)) {
                    tileUsingUs.GetComponent<Tile>().addTag(TileTags.Friendly);
                }
            }
            base.die();
        }
    }

    // Pick up mask, disable pickup range and enable copy range to check for nearby creatures while holding
    public override void pickUp(Tile tilePickingUsUp) {
        base.pickUp(tilePickingUsUp);
        copyRange.enabled = true;
        pickUpRange.enabled = false;
    }

    // Drop mask, re-enable pickup range and disable copy range as to not interfere with pickup collider
    public override void dropped(Tile tileDroppingUs) {
        base.dropped(tileDroppingUs);
        copyRange.enabled = false;
        pickUpRange.enabled = true;
    }

    // If nearby tile is a creature and has an animator component, set it as target creature to copy
    void OnTriggerEnter2D(Collider2D monsterCollider) {
        // Raycast
        // Subtract positions to calculate distance
        if (monsterCollider != null) {
            Tile monsterFound = monsterCollider.gameObject.GetComponent<Tile>();
            if (monsterFound != null && monsterFound.hasTag(TileTags.Creature) && !monsterFound.hasTag(TileTags.Player) && monsterCollider.GetComponent<Animator>() != null) {
                monsterInRange = monsterCollider.gameObject;
            }
        }
    }

    // If no creatures are nearby, disable ability to use mask
    void OnTriggerExit2D(Collider2D monsterCollider) {
        if (monsterCollider != null) {
            Tile monsterFound = monsterCollider.gameObject.GetComponent<Tile>();
            if (monsterFound != null && monsterFound.hasTag(TileTags.Creature) && !monsterFound.hasTag(TileTags.Player) && monsterCollider.GetComponent<Animator>() != null) {
                monsterInRange = null;
            }
        }
    }
    
}
