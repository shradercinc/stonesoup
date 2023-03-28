using UnityEngine;

namespace Resources.Nengkuan.Scripts
{
    public class Mirror : Tile
    {
        [SerializeField]
        private LaserBullet laserPrefab;

        [SerializeField] 
        private Vector3 initialRotation;
        
        
        [SerializeField]
        private LayerMask impenetrableLayerMask;

        public override void init()
        {
            base.init();
            // transform.eulerAngles = initialRotation;
            transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 5) * 90 + 45);
        }

        public void Hit(Vector2 direction, Vector2 hitPoint, float rangeLeft)
        {
            if (rangeLeft <= 0)
            {
                return;
            }
            var newDirection = Vector2.Reflect(direction, transform.up);
            LaserUtility.LaserShot(hitPoint + newDirection *.2f, newDirection, rangeLeft, impenetrableLayerMask, InstantiateLaser);
        }
        
        private void InstantiateLaser(Vector2 shotOrigin, Vector2 direction, float length)
        {
            LaserUtility.SpawnLaserTile(shotOrigin, direction, length, laserPrefab);
        }

        public override void takeDamage(Tile tileDamagingUs, int damageAmount, DamageType damageType)
        {
            
        }
    }
}