using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootscript : MonoBehaviour
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private float force;
    [SerializeField]
    private int maxAmmo;
    [SerializeField]
    private int magazine;
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private float reloadTime;

    private int currentAmmo;
    private float timeBetweenShots;

    private void Start()
    {
        currentAmmo = maxAmmo;
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
        if(currentAmmo > 0)
        {
            timeBetweenShots = 0;
            currentAmmo--;
            RaycastHit hit;
            Transform camTransform = GetComponentInParent<Camera>().transform;
            if (Physics.Raycast(camTransform.position, camTransform.forward, out hit))
            {
                if (hit.rigidbody != null)
                {
                    print("hi");
                    hit.rigidbody.AddForce(-hit.normal * force);
                }
            }
        }
    }

    private void Reload()
    {
        if (currentAmmo != maxAmmo)
        {
            if (magazine >= maxAmmo)
            {
                currentAmmo = maxAmmo;
                magazine -= maxAmmo;
            }
            else if (magazine != 0)
            {
                currentAmmo = magazine;
                magazine = 0;
            }
        }
        print("Перезарядился");
    }
}
