using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jrr504_Rotlet : apt283BFSEnemy
{
    // A ghoul creature with a wide sight range that gets faster the longer it's locked on chasing friendly targets.
    private float chaseTimer = 0f;

    // Define how long it takes to increase the Rotlet's speed to the next tier
    private float lowSpeedRange = 1.5f;
    private float midSpeedRange = 3.5f;
    private float highSpeedRange = 5f;

    public override void Update() {
        base.Update();
        if (_tileWereChasing != null) {
            chaseTimer += Time.deltaTime;
        }
        else {
            chaseTimer = 0f;
        }

        if (chaseTimer <= lowSpeedRange) {
            moveSpeed = 3f;
        }
        else if (chaseTimer > lowSpeedRange && chaseTimer <= midSpeedRange) {
            moveSpeed = 6f;
        }
        else if (chaseTimer > midSpeedRange && chaseTimer <= highSpeedRange) {
            moveSpeed = 9f;
        }
        else {
            moveSpeed = 12f;
        }
    }
}
