using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jrr504_Jetpad : Tile
{
    // A tile that gives a burst of speed to the player when stepped on
    // ** Tile is not working properly in Single Room Mode due to the Room transitions **
    
    private float buffDuration = 5f;

    // Player's speed will be multiplied by this value when stepping over the tile
    private float speedBurstValue = 1.5f;

    [SerializeField] private AudioClip speedBoostSound;

    // Capture a reference to the player, track their original movement speed and whether they've recently stepped on any instances of this tile
    private static Player playerReference = null;
    private static float originalMoveSpeed;
    private static bool recentlyBuffedFlag;

    // Ensures that traps still work after "Try Again" is selected
    private void Awake() {
        recentlyBuffedFlag = false;
    }

    private void OnTriggerEnter2D(Collider2D playerSteppingOnThisCollider) {
        Tile tileSteppingOnThis = playerSteppingOnThisCollider.gameObject.GetComponent<Tile>();
        if (playerSteppingOnThisCollider.GetComponent<Player>() != null) { 
            playerReference = playerSteppingOnThisCollider.gameObject.GetComponent<Player>();
        }
        if (tileSteppingOnThis.hasTag(TileTags.Player) && playerReference != null && recentlyBuffedFlag == false) {
            originalMoveSpeed = playerReference.moveSpeed;
            StartCoroutine(BuffAndWait());
        }
    }

    private IEnumerator BuffAndWait() {
        recentlyBuffedFlag = true;
        AudioManager.playAudio(speedBoostSound);
        playerReference.moveSpeed *= speedBurstValue;

        yield return new WaitForSeconds(buffDuration);

        recentlyBuffedFlag = false;
        playerReference.moveSpeed = originalMoveSpeed;

    }
}
