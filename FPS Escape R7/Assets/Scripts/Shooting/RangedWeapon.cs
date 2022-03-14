using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : Weapon
{
    [SerializeField]
    protected int magazine;
    [SerializeField]
    protected float reloadTime;
    [SerializeField]
    protected float RecoilValue = 1.0f;

    protected int currentAmmo;
    protected Camera cam;
    
    void Start()
    {
        cam = GetComponentInParent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        print(cam);
    }
    
    protected void Spread()
    {
        float vertAngle = cam.transform.localEulerAngles.x + Random.Range(-RecoilValue, RecoilValue);
        if (vertAngle > 90 && vertAngle < 280 || vertAngle > 80 && vertAngle < 270) return;
        cam.transform.localEulerAngles = new Vector3(vertAngle, Random.Range(-RecoilValue, RecoilValue), cam.transform.localEulerAngles.z);
    }
}
