using System;
using Photon.Pun;
using UnityEngine;
using TMPro;

namespace Shooting
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private float damage = 21f;
        [SerializeField] private float fireRate = 1f;
        [SerializeField] private float force = 155f;
        [SerializeField] private float range = 15f;
        [SerializeField] private ParticleSystem muzzleFlash;
        [SerializeField] private Transform bulletSpawn;
        [SerializeField] private AudioClip shotSfx;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private GameObject hitEffect;
        [SerializeField] private Camera cam;
        [SerializeField] private GameObject weaponOwner;
 
        private bool _isMyWeapon;
        private TextMeshProUGUI _ammo;

        private float _nextFire = 0f;

        private void Start()
        {
            _isMyWeapon = weaponOwner.GetComponent<PhotonView>().IsMine;
            _ammo = GameObject.FindGameObjectWithTag("AmmoBar").GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (!Input.GetButton("Fire1") || !(Time.time > _nextFire)) return;
            
            _nextFire = Time.time + 1f / fireRate;
            Shoot();
        }

        private void Shoot()
        {
            audioSource.PlayOneShot(shotSfx);
            muzzleFlash.Play();

            if (!Physics.Raycast(cam.transform.position, cam.transform.forward, out var hit, range)) return;

            var impact = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impact, 2f);

            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * force);
            }

            var ammo = Convert.ToInt32(_ammo.text);
            _ammo.text = (--ammo).ToString();
        }
    }
}