using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace Resources.Nengkuan.Scripts
{
    public class LaserTrap : Tile
    {
        public int Damage;
        
        public LaserTrapInit Init;
        public List<LaserTrapMovement> Movements;
        
        public Transform laserTransform;

        public override void init()
        {
            base.init();
            
        }

        protected override void updateSpriteSorting()
        {
            base.updateSpriteSorting();
            _sprite.sortingLayerID = SortingLayer.NameToID("Air");
        }

        public IEnumerator TrapMove()
        {
            var initTimer = 0f;
            var laserLocalPosition = laserTransform.localPosition;
            var laserScale = laserTransform.localScale;
            laserScale.y = 0;
            while (initTimer < Init.ChargingTime)
            {
                yield return new WaitForEndOfFrame();
                initTimer += Time.deltaTime;
                laserScale.y = initTimer / Init.ChargingTime * Init.LaserLength;
                laserLocalPosition.y = laserScale.y / 2;
                laserTransform.localScale = laserScale;
                laserTransform.localPosition = laserLocalPosition;
            }

            foreach (var movement in Movements)
            {
                var timer = 0f;
                while (timer < Mathf.Max(movement.PositionMovement.Time, movement.RotationMovement.Time))
                {
                    yield return new WaitForEndOfFrame();
                    timer += Time.deltaTime;
                    if (timer < movement.PositionMovement.Time)
                    {
                        transform.position += movement.PositionMovement.Direction.normalized * movement.PositionMovement.Speed * Time.deltaTime;
                    }

                    if (timer < movement.RotationMovement.Time)
                    {
                        transform.Rotate(0, 0, movement.RotationMovement.SpeedInDegree * Time.deltaTime);
                    }
                }
            }
        }
        
        public void DeActive()
        {
            die();
        }


        public void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                var player = col.GetComponent<Player>();
                if (player != null)
                {
                    player.takeDamage(this, Damage);
                }
            }
        }
    }
    
    [Serializable]
    public class LaserTrapInit
    {
        public float ChargingTime;
        public float LaserLength;
        public Vector2Int InitialPosition;
        public float InitialRotationDegreeClockwise;
        public int TrapDamage;
    }
    
    [Serializable]
    public class LaserTrapPositionMovement
    {
        public Vector3 Direction;
        public float Speed;
        public float Time;
    }
        
    [Serializable]
    public class LaserTrapRotationMovement
    {
        public float SpeedInDegree;
        public float Time;
    }
    
    [Serializable]
    public class LaserTrapMovement
    {
        public LaserTrapPositionMovement PositionMovement;
        public LaserTrapRotationMovement RotationMovement;
    }
    
    [Serializable]
    public class TrapMovementQueue
    {
        public LaserTrapInit Init;
        public List<LaserTrapMovement> Movements;
    }
}