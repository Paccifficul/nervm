using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    /// <summary>
    /// Базовый класс управление игрока
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        // ==== ПОЛЯ КЛАССА ====
        // ---- Скорость ----
        /// <value>Скорость передвижения</value>
        private readonly float walkSpeed = 4f;
        /// <value>Скорость бега</value>
        private readonly float sprintSpeed = 7f;
        /// <value>Коэффициент ускорения</value>
        protected float speedCoef = 1f;
        /// <value>Финальная скорость</value>
        private Vector3 finalVelocity = Vector3.zero;

        // ---- Передвижение ----
        /// <value>Горизонтальная скорость</value>
        private Vector3 horMove;
        /// <value>Вертикальная скорость</value>
        private Vector3 verMove;

        // ---- Прыжок ----
        [SerializeField]
        /// <value>Сила прыжка</value>
        private float jumpForceCoef = 15f;

        // ---- Поворот ----
        [SerializeField]
        /// <value>Вертикальная скорость мыши</value>
        private float mouseVerSensivity = 4f;
        /// <value>Горизонтальая скорость мыши</value>
        [SerializeField]
        private float mouseHorSensivity = 4f;
        /// <value>Градус поворота по горизонтали</value>
        private float horRot;
        /// <value>Градус поворота по вертикали</value>
        private float verRot = 0;
        /// <value>Горизонтальны поворот</value>
        private Vector3 horRotation = Vector3.zero;

        // ---- Компоненты Unity ----
        /// <value>Камера игрока</value>
        [SerializeField]
        private new Camera camera;
        private new Rigidbody rigidbody;

        // ==== Свойства ====
        /// <summary>
        /// Получаем значение скорости ходьбы
        /// </summary>
        /// <returns>Возращает значение скорости ходьбы</returns>
        public float WalkSpeed
        {
            get
            {
                return walkSpeed;
            }
        }
        /// <summary>
        /// Получаем значение скорости при беге
        /// </summary>
        /// /// <returns>Возращает значение скорости бега</returns>
        public float SprintSpeed
        {
            get
            {
                return sprintSpeed;
            }
        }

        // ==== МЕТОДЫ/ФУНКЦИИ (хз как правильно) ====
        // Start is called before the first frame update
        protected void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (IsGrounded() && Input.GetButtonDown("Jump"))
                rigidbody.AddForce(Vector3.up * jumpForceCoef, ForceMode.Impulse);
            print(IsGrounded());
        }

        protected void FixedUpdate()
        {
            // Просчитываем скорость
            horMove = transform.right * Input.GetAxis("Horizontal");
            verMove = transform.forward * Input.GetAxis("Vertical");

            finalVelocity = CalculateSpeed();

            // Просчитываем угол поворота
            horRot = Input.GetAxis("Mouse X");
            verRot += Input.GetAxis("Mouse Y") * mouseVerSensivity;

            horRotation = new Vector3(0f, horRot, 0f) * mouseHorSensivity;

            MovePlayer();
            ChangeViewAngle();
        }

        /// <summary>
        /// Поворот игрока в сторону при помощи rigidbody.
        /// Поворот камеры, с ограничением 90 градусов вверх и вниз
        /// </summary>
        private void ChangeViewAngle()
        {
            rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(horRotation));

            verRot = Mathf.Clamp(verRot, -90f, 90f);
            camera.transform.localEulerAngles = new Vector3(-verRot,
                camera.transform.localEulerAngles.y, camera.transform.localEulerAngles.z);
        }

        /// <summary>
        /// Передвигаем игрока. Прыжок
        /// </summary>
        private void MovePlayer()
        {
            rigidbody.MovePosition(rigidbody.position + finalVelocity * Time.fixedDeltaTime);
        }

        /// <summary>
        /// Проверяем, находится ли игрок на земле или нет
        /// </summary>
        /// <returns>Стоит ли игрок на поверхности</returns>
        private bool IsGrounded()
        {
            return Physics.Raycast(transform.position, Vector3.down, 1.0f);
        }

        /// <summary>
        /// Просчитываем скорость, учитывая ускорение 
        /// </summary>
        /// <returns>Фильнаная скорость игрока, проверяя, бежит ли он или идёт</returns>
        private Vector3 CalculateSpeed()
        {
            if (Input.GetButton("Sprint")) return sprintSpeed * speedCoef * (horMove + verMove).normalized;
            return walkSpeed * speedCoef * (horMove + verMove).normalized;
        }
    }

}
