using System;
using UnityEngine;

namespace Resources.Nengkuan.Scripts
{
    public static class LaserUtility
    {
        public static void LaserShot(Vector2 shotOrigin, Vector2 shotDirection, float maxRange, LayerMask impenetrableLayerMask, 
            Action<Vector2, Vector2, float> onHit)
        {
            var raycastHit2D = Physics2D.Raycast(shotOrigin, shotDirection, maxRange, impenetrableLayerMask.value);
            var hitPosition = raycastHit2D.point;
            var length = raycastHit2D.distance;
            if (raycastHit2D.collider == null)
            {
                hitPosition = shotOrigin + shotDirection * maxRange;
                length = maxRange;
            }
            else if (raycastHit2D.collider.TryGetComponent<Mirror>(out var mirror))
            {
                mirror.Hit(shotDirection, hitPosition, maxRange - length);
            }
            onHit(shotOrigin, shotDirection, length);
        }

        public static void SpawnLaserTile(Vector2 shotOrigin, Vector2 shotDirection, float length, LaserBullet laserPrefab)
        {
            var laserBullet = GameObject.Instantiate(laserPrefab, null);
            laserBullet.init();
            laserBullet.transform.position = shotOrigin + shotDirection * length / 2;
            laserBullet.transform.localRotation *= Quaternion.FromToRotation(Vector3.up, shotDirection);
            var laserLocalScale = laserBullet.transform.localScale;
            laserLocalScale.y = length;
            laserBullet.transform.localScale = laserLocalScale;
        }
    }
}