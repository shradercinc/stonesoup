using System;
using System.Collections;
using UnityEngine;

namespace Resources.Nengkuan.Scripts
{
    public class LaserBullet : Tile
    {
        [SerializeField] 
        private int damage = 1;
        
        [SerializeField] 
        [EnumFlags]
        private TileTags shotIgnoreTags;

        [SerializeField] 
        private BoxCollider2D collider2D;

        [SerializeField] 
        private SpriteRenderer renderer;
        

        public override void init()
        {
            base.init();
            // var results = new Collider2D[20];
            // var contactFilter = new ContactFilter2D();
            // contactFilter = contactFilter.NoFilter();
            // Physics2D.OverlapCollider(collider2D, contactFilter, results);
            // var results =
            //     Physics2D.OverlapBoxAll(transform.position, transform.lossyScale, collider2D.transform.eulerAngles.z);
            // foreach (var collider in results)
            // {
            //     if (collider == null)
            //     {
            //         break;
            //     }
            //     if (collider.TryGetComponent<Tile>(out var tile))
            //     {
            //         tile.takeDamage(this, damage);
            //     }
            // }
            StartCoroutine(FadeOut());
        }
        

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.TryGetComponent<Tile>(out var tile))
            {
                // if (shotIgnoreTags.HasFlag(tile.tags))
                // {
                //     return;
                // }
                if ((shotIgnoreTags & tile.tags) == 0)
                {
                    tile.takeDamage(this, damage);
                }
               
            }
        }

        IEnumerator FadeOut()
        {
            var currentColor = renderer.color;
            while (currentColor.a > 0)
            {
                currentColor.a -= 2 * Time.deltaTime;
                yield return new WaitForEndOfFrame();
                renderer.color = currentColor;
            }
            die();
        }
    }
}