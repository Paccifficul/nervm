using System;
using Movement;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shooting.New
{
    public sealed class WeaponHandle
    {
        /// <summary>
        /// Количество оставшихся патронов.
        /// </summary>
        public int Ammunition { get; set; }
        /// <summary>
        /// Максимальное число патронов.
        /// </summary>
        public int MaxAmmunition { get; set; }
        /// <summary>
        /// Общее количество патронов.
        /// </summary>
        public int TotalAmmunition { get; set; }
        /// <summary>
        /// Состояние готовности к выстрелу.
        /// </summary>
        public bool ReadyForShot => Ammunition > 0 && _prepValue <= 0 && !Input.GetKeyDown(KeyCode.R);
        /// <summary>
        /// Тип оружия.
        /// </summary>
        public WeaponType WeaponType { get; set; }
        /// <summary>
        /// Урон по умолчанию в тело.
        /// </summary>
        public int BasicDamage { get; set; }
        /// <summary>
        /// Время перезарядки.
        /// </summary>
        public float ReloadTime { get; set; }
        /// <summary>
        /// Время подготовки к следующему выстрелу.
        /// </summary>
        public float ShotPreparationTime { get; set; }
        /// <summary>
        /// Величина разброса при отдаче.
        /// </summary>
        public float RecoilValue { get; set; }
        /// <summary>
        /// Время, оставшееся до завершения перезарядки.
        /// </summary>
        private float _cooldownValue;
        /// <summary>
        /// Время, оставшееся до готовности к следующему выстрелу.
        /// </summary>
        private float _prepValue;

        /// <summary>
        /// Метод для попытки выстрела.
        /// </summary>
        public void Shoot(GameObject obj, Camera camera)
        {
            if (!ReadyForShot) return;
            Debug.Log("Ready for Shot. Shooting...");
            _prepValue = ShotPreparationTime;
            Ammunition--;
            
            var shotStartPosition = camera.transform.position + Vector3.RotateTowards(
                Vector3.forward,
                camera.transform.forward,
                (float)Math.PI / 2,
                1
            );
            var forwardPosition = camera.transform.forward + Vector3.RotateTowards(
                new Vector3(
                    Random.Range(-0.0004f, 0.0004f),
                    Random.Range(-0.0004f, 0.0004f)
                ),
                camera.transform.forward,
                (float)Math.PI,
                1);
            
            Debug.DrawLine(shotStartPosition, (shotStartPosition + forwardPosition * 10), Color.yellow, 1);
            
            if (Physics.Raycast(shotStartPosition, forwardPosition, out var hitInfo))
            {
                if (hitInfo.transform.gameObject.TryGetComponent<PlayerController>(out var controller))
                {
                    controller.Hitpoints -= (int)(BasicDamage/* * (hitInfo.distance / 10)*/);
                }
                //хз что тут делать
            }
            obj.GetComponent<PlayerController>().Rotate(Random.Range(-RecoilValue, RecoilValue), Random.Range(-RecoilValue * 0.5f, RecoilValue));
            if (Ammunition != 0) return;
            
            Debug.Log("Out Of Bullets");
            _cooldownValue = ReloadTime;
        }

        public void Update()
        {
            if (_cooldownValue > 0)
            {
                Debug.Log($"Reloading... Time left: {_cooldownValue}");
                _cooldownValue -= Time.deltaTime;
                if (_cooldownValue <= 0)
                {
                    Ammunition = Math.Min(MaxAmmunition, TotalAmmunition);
                    TotalAmmunition = Math.Max(TotalAmmunition - MaxAmmunition, 0);
                    
                    Debug.Log($"Reload success! {Ammunition} / {MaxAmmunition} : {TotalAmmunition}");
                    if (Ammunition == 0) Debug.Log("Out of ammo!");
                }
            }
            if (_prepValue > 0)
            {
                _prepValue -= Time.deltaTime;
            }
        }

        public void Reload()
        {
            if (Ammunition >= MaxAmmunition || !(_cooldownValue <= 0)) return;
            _cooldownValue = ReloadTime;
        }
    }
    /// <summary>
    /// Тип оружия.
    /// </summary>
    public enum WeaponType
    {
        /// <summary>
        /// Ближний бой
        /// </summary>
        Melee,
        /// <summary>
        /// Механическое (хз, зачем оно вообще надо, но всё же).
        /// </summary>
        Mechanical,
        /// <summary>
        /// Полуавтоматическое.
        /// </summary>
        SemiAutomatical,
        /// <summary>
        /// Автоматическое.
        /// </summary>
        Automatical
    }
}