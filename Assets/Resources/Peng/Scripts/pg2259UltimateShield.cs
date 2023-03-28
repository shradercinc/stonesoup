using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pg2259UltimateShield : Tile
{
    public AudioClip swingSound, pickupSound;
    public float shootDistance;
    public float damageForce = 1000;
    public float shootSpeed = 20f;
    public float returnSpeed = 20f;
    public float rotateSpeed = 1000f;


    public bool _shooting = false;


    bool pickedUp = false;
    float _shootingTime = 0;
    public float _goOutTime = 0.3f;
    Vector2 _shootingDirection= Vector2.zero;
    Transform _playerTransform = null;

    public override void takeDamage(Tile tileDamagingUs, int amount, DamageType damageType)
    {
        if (_shooting || _tileHoldingUs != null)
        {
            return;
        }
        base.takeDamage(tileDamagingUs, amount, damageType);
    }

    public override void pickUp(Tile tilePickingUsUp)
    {
        if (pickedUp) return;
        //base.pickUp(tilePickingUsUp);
        if (_tileHoldingUs != null)
        {
            AudioManager.playAudio(pickupSound);
        }

        if (tilePickingUsUp.GetComponentInChildren<pg2259Rotate>() == null)
        {
            print("try to do this");
            GameObject rotateObj = new GameObject("rotate object", typeof(pg2259Rotate));
            rotateObj.transform.parent = tilePickingUsUp.transform;
            rotateObj.transform.localPosition= Vector3.zero;
            pg2259Rotate.exist = true;
        }
        pg2259Rotate rotateScript = tilePickingUsUp.GetComponentInChildren<pg2259Rotate>();
        rotateScript.AddToRotate(this.gameObject);
        removeTag(TileTags.CanBeHeld);
        pickedUp = true;
        //GetComponent<Rigidbody2D>().simulated = false;
        //Destroy(this);
    }

    public override void dropped(Tile tileDroppingUs)
    {
        if (_shooting)
        {
            return;
        }
        base.dropped(tileDroppingUs);
    }

    public override void useAsItem(Tile tileUsingUs)
    {
        /*
        // We can't swing if we're already swinging.
        if (_shooting || _tileHoldingUs != tileUsingUs)
        {
            return;
        }

        //AudioManager.playAudio(swingSound);

        _shooting = true;

        _playerTransform = transform.parent;

        transform.parent = _playerTransform.parent;

        _shootingDirection = _tileHoldingUs.aimDirection;
        
        
        //Vector2.left;
        /*
        // We use Atan2 to find the pivot angle given the aim direciton.
        _pivotStartAngle = Mathf.Rad2Deg * Mathf.Atan2(tileUsingUs.aimDirection.y, tileUsingUs.aimDirection.x);

        // Here's where we pull the switcheroo where we become the child of our pivot.
        swingPivot.transform.parent = tileUsingUs.transform;
        swingPivot.transform.localPosition = Vector3.zero;
        swingPivot.transform.localRotation = Quaternion.Euler(0, 0, _pivotStartAngle);
        transform.parent = swingPivot;

        // These values can be tuned to make us rotate/offset differently from our pivot.
        transform.localPosition = new Vector3(1.2f, 0, 0);
        transform.localRotation = Quaternion.Euler(0, 0, -90);
        */
    }

    private void EndUseAsItem()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (pickedUp)
        {
            transform.Rotate(0, 0, -rotateSpeed * Time.deltaTime);
        }

        /*
        if (_shooting)
        {
            if(_shootingTime < _goOutTime) //go out
            {
                //transform.position += (Vector3)_shootingDirection.normalized * shootSpeed * Time.deltaTime;
                //transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
                //_shootingTime += Time.deltaTime;
            }
            else
            {
                EndUseAsItem();
            }
        }*/
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(_shooting && _shootingTime > _goOutTime && other.gameObject.transform == _playerTransform)
        {
            _shooting = false;
            transform.parent = _playerTransform;
            _shootingTime = 0;

            transform.localPosition = new Vector3(heldOffset.x, heldOffset.y, -0.1f);
            transform.localRotation = Quaternion.Euler(0, 0, heldAngle);
        }
        if (_shooting && other.gameObject.GetComponent<Tile>() != null)
        {
            Tile otherTile = other.gameObject.GetComponent<Tile>();
            if (otherTile != _tileHoldingUs && !otherTile.hasTag(TileTags.CanBeHeld))
            {
                otherTile.takeDamage(this, 1);
                otherTile.addForce((other.transform.position - _tileHoldingUs.transform.position).normalized * damageForce);
            }
        }
    }


}
