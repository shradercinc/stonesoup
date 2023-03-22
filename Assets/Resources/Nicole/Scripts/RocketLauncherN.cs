using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherN : Tile
{
    [SerializeField] private float ReloadTime = 1f;
    [SerializeField] float turnSpeed = 0.1f;
    [SerializeField] GameObject Projectile;
    private bool canFire = true;
    private float reloadTimer = 0f;
    private Camera Cam;
    private Transform pos;
    
    // Start is called before the first frame update
    void Start()
    {
        Cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        pos = GetComponent<Transform>();
    }

    IEnumerator Reload(float reloadTime)
    {
        print("Reloading!");
        float timer = 0;
        while (timer <= reloadTime)
        {
            timer += Time.deltaTime;
            print(timer);
            yield return null;
        }
        print("Ready to fire!");
        canFire = true;
    }

    public override void useAsItem(Tile tileUsingUs)
    {
        base.useAsItem(tileUsingUs);
        if (canFire)
        {
            var rocket = Instantiate(Projectile, transform.position, transform.rotation);
            rocket.GetComponent<RocketN>().Launcher.Add(this.gameObject);
            if (this.isBeingHeld)
            {
                rocket.GetComponent<RocketN>().Launcher.Add(_tileHoldingUs.gameObject);
            }
            canFire = false;
            StartCoroutine(Reload(ReloadTime));

        }
    }

    public override void dropped(Tile tileDroppingUs)
    {   
        base.dropped(tileDroppingUs);
        reloadTimer = 0;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    // Update is called once per frame
    void Update()
    {
        if (isBeingHeld)
        {
            reloadTimer--;
            var mPos = Input.mousePosition;
            var pPos = Cam.WorldToScreenPoint(pos.position);
            Vector3 dir = (mPos - pPos);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.eulerAngles = new Vector3(0f, 0f, angle);


        }
    }
}
