using InGameScripts;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Shooting.New
{
    public class ShootScript : MonoBehaviour
    {
        public GameObject weaponOwner;
        public Camera ownerCamera;
        private WeaponHandle _weapon;
        public int weaponID;
        private bool _mouseButtonHoldFlag;
        private bool _isMyWeapon;

        private TextMeshProUGUI _ammo;

        // Start is called before the first frame update
        private void Start()
        {
            _isMyWeapon = weaponOwner.GetComponent<PhotonView>().IsMine;
            
            if (!_isMyWeapon) return;
            
            _ammo = GameObject.FindGameObjectWithTag("AmmoBar").GetComponent<TextMeshProUGUI>();
            _weapon = weaponID switch
            {
                1 => new WeaponHandle()
                {
                    MaxAmmunition = 12,
                    Ammunition = 12,
                    TotalAmmunition = 36,
                    BasicDamage = 35,
                    ReloadTime = 1.2f,
                    WeaponType = WeaponType.SemiAutomatical,
                    ShotPreparationTime = 0.2f,
                    RecoilValue = 1,
                },
                2 => new WeaponHandle()
                {
                    MaxAmmunition = 25,
                    Ammunition = 25,
                    TotalAmmunition = 75,
                    BasicDamage = 50,
                    ReloadTime = 1.5f,
                    WeaponType = WeaponType.Automatical,
                    ShotPreparationTime = 0.1f,
                    RecoilValue = 1.2f,
                },
                _ => new WeaponHandle()
                {
                    MaxAmmunition = -1,
                    BasicDamage = 75,
                    ReloadTime = -1,
                    WeaponType = WeaponType.Melee,
                    ShotPreparationTime = 1,
                }
            };
        }

        // Update is called once per frame
        private void Update()
        {
            if (!_isMyWeapon || GameMenu.Instance.IsGameMenuOpened) return;
            
            _ammo.text = $"{_weapon.Ammunition}/{_weapon.MaxAmmunition}\t{_weapon.TotalAmmunition}";
            _weapon.Update();
            if (Input.GetKey(KeyCode.Mouse0))
            {
                switch (_weapon.WeaponType)
                {
                    case WeaponType.Automatical:
                    {
                        _weapon.Shoot(weaponOwner, ownerCamera);
                        Debug.Log($"Shot! Ammunition Left: {_weapon.Ammunition}");
                        break;
                    }
                    case WeaponType.Melee:
                    case WeaponType.Mechanical:
                    case WeaponType.SemiAutomatical:
                    default:
                    {
                        if (!_mouseButtonHoldFlag)
                        {
                            _weapon.Shoot(weaponOwner, ownerCamera);
                            Debug.Log("Shot!");
                        }
                        break;
                    }
                }
                _mouseButtonHoldFlag = true;
            }
            else if (_mouseButtonHoldFlag) _mouseButtonHoldFlag = false;
            if (Input.GetKeyDown(KeyCode.R))
            {
                _weapon.Reload();
            }
        }
    }
}
