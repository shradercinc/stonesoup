using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Resources.Nengkuan.Scripts
{
    public class TrapManager : Tile
    {
        private List<InvisibleWall> invisibleWalls = new List<InvisibleWall>();

        [SerializeField] 
        private List<TrapMovementQueue> trapMovementQueues = new List<TrapMovementQueue>(); 
        
        [SerializeField]
        private LaserTrap laserTrapPrefab;

        //Put this in Start to make sure all the invisible walls and traps has been instantiated
        private void Start()
        {
            foreach (var wall in transform.parent.GetComponentsInChildren<InvisibleWall>())
            {
                invisibleWalls.Add(wall);
            }
        }
        

        public void Active()
        {
            foreach (var wall in invisibleWalls)
            {
                wall.Active();
            }

            StartCoroutine(ActiveTraps());
        }

        IEnumerator ActiveTraps()
        {
            yield return new WaitForSeconds(1f);
            foreach (var trapMovement in trapMovementQueues)
            {
                //InitTrap
                var trapInit = trapMovement.Init;
                var trapGameObject = Tile.spawnTile(laserTrapPrefab.gameObject, transform.parent,
                    trapInit.InitialPosition.x, trapInit.InitialPosition.y);
                var trap = trapGameObject.GetComponent<LaserTrap>();
                trap.transform.Rotate(0, 0, trapInit.InitialRotationDegreeClockwise);
                trap.Damage = trapInit.TrapDamage;
                trap.Init = trapInit;
                trap.Movements = trapMovement.Movements;
                yield return StartCoroutine(trap.TrapMove());
                trap.DeActive();
            }

            foreach (var wall in invisibleWalls)
            {
                wall.DeActive();
            }
        }
    }
}