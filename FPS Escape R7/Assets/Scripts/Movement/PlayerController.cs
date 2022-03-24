using UnityEngine;

namespace Movement
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
        private const float walkSpeed = 4f;

        /// <value>Скорость бега</value>
        private const float sprintSpeed = 7f;

        /// <value>Коэффициент ускорения</value>
        private const float SpeedCoef = 1f;

        /// <value>Финальная скорость</value>
        private Vector3 _finalVelocity = Vector3.zero;

        // ---- Передвижение ----
        /// <value>Горизонтальная скорость</value>
        private Vector3 _horMove;
        /// <value>Вертикальная скорость</value>
        private Vector3 _verMove;

        // ---- Прыжок ----
        [SerializeField]
        /// <value>Сила прыжка</value>
        private float jumpForceCoef = 15f;

        // ---- Поворот ----
        /// <value>Вертикальная скорость мыши</value>
        [SerializeField] private float mouseVerSensitivity = 4f;
        /// <value>Горизонтальая скорость мыши</value>
        [SerializeField] private float mouseHorSensitivity = 4f;
        /// <value>Градус поворота по горизонтали</value>
        private float _horRot;
        /// <value>Градус поворота по вертикали</value>
        private float _verRot = 0;
        /// <value>Горизонтальны поворот</value>
        private Vector3 _horRotation = Vector3.zero;

        // ---- Компоненты Unity ----
        /// <value>Камера игрока</value>
        [SerializeField] private new Camera camera;
        private new Rigidbody _rigidbody;

        // ==== Свойства ====
        /// <summary>
        /// Получаем значение скорости ходьбы
        /// </summary>
        /// <returns>Возращает значение скорости ходьбы</returns>
        public float WalkSpeed => walkSpeed;

        /// <summary>
        /// Получаем значение скорости при беге
        /// </summary>
        /// /// <returns>Возращает значение скорости бега</returns>
        public float SprintSpeed => sprintSpeed;

        // ==== МЕТОДЫ/ФУНКЦИИ (хз как правильно) ====
        // Start is called before the first frame update
        protected void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (IsGrounded() && Input.GetButtonDown("Jump"))
                _rigidbody.AddForce(Vector3.up * jumpForceCoef, ForceMode.Impulse);
            // print(IsGrounded());
        }

        protected void FixedUpdate()
        {
            // Просчитываем скорость
            _horMove = transform.right * Input.GetAxis("Horizontal");
            _verMove = transform.forward * Input.GetAxis("Vertical");

            _finalVelocity = CalculateSpeed();

            // Просчитываем угол поворота
            _horRot = Input.GetAxis("Mouse X");
            _verRot += Input.GetAxis("Mouse Y") * mouseVerSensitivity;

            _horRotation = new Vector3(0f, _horRot, 0f) * mouseHorSensitivity;

            MovePlayer();
            ChangeViewAngle();
        }
                

        /// <summary>
        /// Поворот игрока в сторону при помощи rigidbody.
        /// Поворот камеры, с ограничением 90 градусов вверх и вниз
        /// </summary>
        private void ChangeViewAngle()
        {
            _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(_horRotation));

            _verRot = Mathf.Clamp(_verRot, -90f, 90f);
            camera.transform.localEulerAngles = new Vector3(-_verRot,
                camera.transform.localEulerAngles.y, camera.transform.localEulerAngles.z);
        }

        /// <summary>
        /// Передвигаем игрока.
        /// </summary>
        private void MovePlayer()
        {
            _rigidbody.MovePosition(_rigidbody.position + _finalVelocity * Time.fixedDeltaTime);
        }

        /// <summary>
        /// Проверяем, находится ли игрок на земле или нет
        /// </summary>
        /// <returns>Стоит ли игрок на поверхности</returns>
        private bool IsGrounded()
        {
            return Physics.Raycast(transform.position, Vector3.down, 0.1f);
        }

        /// <summary>
        /// Просчитываем скорость, учитывая ускорение 
        /// </summary>
        /// <returns>Фильнаная скорость игрока, проверяя, бежит ли он или идёт</returns>
        private Vector3 CalculateSpeed()
        {
            if (Input.GetButton("Sprint")) return sprintSpeed * SpeedCoef * (_horMove + _verMove).normalized;
            return walkSpeed * SpeedCoef * (_horMove + _verMove).normalized;
        }
    }
}
