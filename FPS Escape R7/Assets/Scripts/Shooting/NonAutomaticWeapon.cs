using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonAutomaticWeapon : RangedWeapon
{
    private int clip = 1;

    private void Start()
    {
        cam = GetComponentInParent<Camera>();
        currentAmmo = clip;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && currentAmmo > 0)
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
        }
    }

    private void Reload()
    {
        if (currentAmmo != clip)
        {
            if (magazine != 0)
            {
                currentAmmo = clip;
                magazine -= clip;
            }
        }
    }
}
