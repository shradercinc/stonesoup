using System;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Nengkuan.Scripts
{
    public class LaserGun : Tile
    {
        [SerializeField]
        private LaserBullet laserPrefab;

        [SerializeField]
        private Transform muzzleSocket;
        
        [SerializeField]
        private float fireInterval = .5f;
        

        [SerializeField]
        private LayerMask impenetrableLayerMask;
        

        [SerializeField] 
        private float maxRange = 100f;

        protected float shotTimer = 0f;

        

        protected void Aim() 
        {
            _sprite.transform.localPosition = new Vector3(1f, 0, 0);
            float aimAngle = Mathf.Atan2(_tileHoldingUs.aimDirection.y, _tileHoldingUs.aimDirection.x)*Mathf.Rad2Deg;
            transform.localRotation = Quaternion.Euler(0, 0, aimAngle);
            if (_tileHoldingUs.aimDirection.x < 0) {
                _sprite.flipY = true;
            }
            else {
                _sprite.flipY = false;
            }
        }

        protected void Update()
        {
            shotTimer += Time.deltaTime;
            if (_tileHoldingUs != null) {
                // If we're held, rotate and aim the gun.
                Aim();
            }
            else {
                // Otherwise, move the gun back to the normal position. 
                _sprite.transform.localPosition = Vector3.zero;
                transform.rotation = Quaternion.identity;
            }
            updateSpriteSorting();
        }

        public override void useAsItem(Tile tileUsingUs)
        {
            base.useAsItem(tileUsingUs);
            if (shotTimer < fireInterval)
            {
                return;
            }
            LaserShot();
            shotTimer = 0f;
            Aim();
        }

        // private void DoRaycastShot()
        // {
        //     var raycastHit2D = Physics2D.Raycast(muzzleSocket.position, _tileHoldingUs.aimDirection,
        //         float.MaxValue, impenetrableLayerMask.value);
        //     var hitPosition = raycastHit2D.point;
        //     var length = raycastHit2D.distance;
        //     if (raycastHit2D.collider == null)
        //     {
        //         hitPosition = (Vector2)muzzleSocket.position + _tileHoldingUs.aimDirection * maxRange;
        //         length = maxRange;
        //     }
        //     else if (raycastHit2D.collider.TryGetComponent<Mirror>(out var mirror))
        //     {
        //         mirror.Hit(_tileHoldingUs.aimDirection, hitPosition, maxRange - length);
        //     }
        //     var laserBullet = Instantiate(laserPrefab, null);
        //     laserBullet.init();
        //     laserBullet.transform.position = muzzleSocket.position + ((Vector3)hitPosition - muzzleSocket.position) / 2;
        //     laserBullet.transform.localRotation *= Quaternion.FromToRotation(Vector3.up, _tileHoldingUs.aimDirection);
        //     var laserLocalScale = laserBullet.transform.localScale;
        //     laserLocalScale.y = length;
        //     laserBullet.transform.localScale = laserLocalScale;
        //     // laserBullet.transform.localScale = new Vector3()
        // }

        private void LaserShot()
        {
            LaserUtility.LaserShot(muzzleSocket.position, _tileHoldingUs.aimDirection, maxRange, impenetrableLayerMask,
                OnLaserHit);
        }
        
        private void OnLaserHit(Vector2 shotOrigin, Vector2 shotDirection, float length)
        {
            LaserUtility.SpawnLaserTile(shotOrigin, shotDirection, length, laserPrefab);
        }
    }
}