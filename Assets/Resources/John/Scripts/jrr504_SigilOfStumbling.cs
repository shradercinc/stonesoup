using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*************************************************************************
/  -----------------------  "Sigil of Stumbling"  ---------------------- *
/                                                                        *
/  Traversable tile trap that causes the player to stumble around when   *
/  stepped on. Effect lasts 10 seconds. Only affects the player.         *
/                                                                        *
/************************************************************************/

public class jrr504_SigilOfStumbling : Tile
{   
    // Variable to capture the player when they step on the trap
    private static GameObject afflictedPlayer;

    // Cooldown to prevent trap from being activated again less than X seconds after activation
    private static float trapCooldown = 12f;

    // How long trap effect should last
    [SerializeField] private float trapEffectDuration = 10f;
    private static float trapEffectTimer;

    // Whether trap may be sprung or not
    private static bool trapSetFlag = true;

    // Bool to signal debuff trigger
    private static bool trapTriggeredFlag = false;

    // Coroutine to reset trap
    private IEnumerator trapReset;


    // Intensity that player stumbles when stepping on the trap
    [SerializeField] private float stumbleIntensity = 0.5f;

    void Awake() {
        trapEffectTimer = trapEffectDuration;
    }

    void FixedUpdate() {
        // rotate player or AddForce
        if (trapTriggeredFlag) {
            if (trapEffectTimer > 0f) {
                trapEffectTimer -= Time.deltaTime;
                afflictedPlayer.transform.position = new Vector2(afflictedPlayer.transform.position.x + Random.Range(-stumbleIntensity, stumbleIntensity), afflictedPlayer.transform.position.y + Random.Range(-stumbleIntensity, stumbleIntensity));
            }
            else {
                trapTriggeredFlag = false;
                trapEffectTimer = trapEffectDuration;
                afflictedPlayer = null;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D tileSteppingOnThis) {
        if (tileSteppingOnThis.gameObject.GetComponent<Tile>().hasTag(TileTags.Player) && trapSetFlag) {
            afflictedPlayer = tileSteppingOnThis.gameObject;
            trapTriggeredFlag = true;
            // Disable trap after it's stepped on until its reset cooldown re-activates it
            trapSetFlag = false;
            trapReset = WaitForTrapCooldown(trapCooldown);
            StartCoroutine(trapReset);
        }
    }

    private IEnumerator WaitForTrapCooldown(float whenToReset) {
        yield return new WaitForSeconds(whenToReset);
        if (!trapSetFlag) { trapSetFlag = true; }
    }
}
