using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticWeapon : RangedWeapon
{
    [SerializeField]
    protected int clip;
    [SerializeField]
    private float fireRate;

    protected float timeBetweenShots;

    private void Start()
    {
        cam = GetComponentInParent<Camera>();
        currentAmmo = clip;
        timeBetweenShots = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeBetweenShots < fireRate)
        {
            timeBetweenShots += Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Mouse0) && timeBetweenShots >= fireRate)
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Invoke("Reload", reloadTime);
        }
    }

    private void Shoot()
    {
        if (currentAmmo > 0)
        {
            timeBetweenShots = 0;
            currentAmmo--;
            RaycastHit hit;
            Transform camTransform = GetComponentInParent<Camera>().transform;
            if (Physics.Raycast(camTransform.position, camTransform.forward, out hit))
            {
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * force);
                }
            }
            Spread();
        }
    }

    private void Reload()
    {
        if (currentAmmo != clip)
        {
            if (magazine >= clip)
            {
                currentAmmo = clip;
                magazine -= clip;
            }
            else if (magazine != 0)
            {
                currentAmmo = magazine;
                magazine = 0;
            }
        }
    }

}
