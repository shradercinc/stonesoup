using System.Collections;
using UnityEngine;

namespace Resources.Nengkuan.Scripts
{
    public class InvisibleWall : Tile
    {
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        
        [SerializeField]
        private Collider2D collider2D;
        
        [SerializeField]
        private float activeTime = 1f;
        
        public void Active()
        {
            StartCoroutine(WallShow());
            collider2D.enabled = true;
        }
        
        public void DeActive()
        {
            die();
        }

        IEnumerator WallShow()
        {
            float timer = 0;
            while (timer < activeTime)
            {
                yield return new WaitForEndOfFrame();
                timer += Time.deltaTime;
                Color currentColor = spriteRenderer.color;
                currentColor.a = timer / activeTime;
                spriteRenderer.color = currentColor;
            }
        }
    }
}