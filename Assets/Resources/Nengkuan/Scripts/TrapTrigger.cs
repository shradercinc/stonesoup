using System;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Nengkuan.Scripts
{
    public class TrapTrigger: Tile
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                die();
            }
        }


        protected override void die()
        {
            var trapManager = transform.parent.GetComponentInChildren<TrapManager>();
            trapManager.Active();
            base.die();
        }
    }
}