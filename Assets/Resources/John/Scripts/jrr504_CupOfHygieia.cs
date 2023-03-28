using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jrr504_CupOfHygieia : apt283BFSEnemy
{
    // Initialize states - docile for idle, aggro while chasing an enemy
    private enum BehaviorState {
        Docile,
        Aggro,
    }

    private BehaviorState _currentBehavior = BehaviorState.Docile;

    // Initialize references to components
    [SerializeField] private Animator cupAnimator;
    [SerializeField] private AudioClip healSFX;
    [SerializeField] private ParticleSystemRenderer auraParticles;
    [SerializeField] private SpriteRenderer auraRange;
    [SerializeField] private Color docileColor;
    [SerializeField] private Material docileMat;
    [SerializeField] private Color aggroColor;
    [SerializeField] private Material aggroMat;

    // amount to heal or damage by
    private int auraHealthAmount = 1;
    // how long player must stand in aura before it affects them
    private float auraTickTimer = 2f;

    // reference to player tile so the player's health can be changed if they are in the creature's aura
    private Tile playerReference = null;
    // flag to ensure healing and damaging only happens once per auraTickTimer interval
    private bool healthRecentlyChangedFlag = false;
    // how long it takes for creature to return to docile state after chasing an enemy
    private float aggroCooldownTimer = 3f;

    public override void Update() {
        base.Update();

        // Swap states between docile and aggro based on whether creature is chasing an enemy or not
        // Docile state provides a healing aura every auraTickTimer interval, while aggro causes damage
        // Aggro state takes an amount of time equal to aggroCooldownTimer before returning to docile state after there is no enemy being chased
        switch (_currentBehavior) {
            case BehaviorState.Docile:
                if (playerReference != null && healthRecentlyChangedFlag == false) {
                    StartCoroutine(RestorePlayerHealth());
                }
                if (_tileWereChasing != null) {
                    cupAnimator.Play("Aggro");
                    auraRange.color = aggroColor;
                    auraParticles.material = aggroMat;
                    auraTickTimer = 0.5f;
                    _currentBehavior = BehaviorState.Aggro;
                }
                break;
            case BehaviorState.Aggro:
                if (playerReference != null && healthRecentlyChangedFlag == false) {
                    StartCoroutine(DamagePlayer());
                }
                if (_tileWereChasing == null) {
                    aggroCooldownTimer -= Time.deltaTime;
                    if (aggroCooldownTimer <= 0f) {
                        cupAnimator.Play("Docile");
                        auraRange.color = docileColor;
                        auraParticles.material = docileMat;
                        auraTickTimer = 2f;
                        aggroCooldownTimer = 3f;
                        _currentBehavior = BehaviorState.Docile;
                    }
                }
                else {
                    aggroCooldownTimer = 3f;
                }
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D tileEnteringAuraCollider) {
        Tile tileEnteringAura = tileEnteringAuraCollider.gameObject.GetComponent<Tile>();
        if (tileEnteringAura.hasTag(TileTags.Player)) {
            playerReference = tileEnteringAura;
        }
    }

    void OnTriggerExit2D(Collider2D tileExitingAuraCollider) {
        Tile tileExitingAura = tileExitingAuraCollider.gameObject.GetComponent<Tile>();
        if (tileExitingAura.hasTag(TileTags.Player)) {
            playerReference = null;
        }
    }

    private IEnumerator RestorePlayerHealth() {
        healthRecentlyChangedFlag = true;
        yield return new WaitForSeconds(auraTickTimer);
        if (playerReference != null) {
            if (playerReference.health < 3) {
                AudioManager.playAudio(healSFX);
                healthRecentlyChangedFlag = false;
                playerReference.health += auraHealthAmount;
            }
        }
        healthRecentlyChangedFlag = false;
    }

    private IEnumerator DamagePlayer() {
        healthRecentlyChangedFlag = true;
        yield return new WaitForSeconds(auraTickTimer);
        if (playerReference != null) {
            playerReference.takeDamage(gameObject.GetComponent<Tile>(), auraHealthAmount, DamageType.Normal);
            healthRecentlyChangedFlag = false;
        }
        healthRecentlyChangedFlag = false;
    }
}
