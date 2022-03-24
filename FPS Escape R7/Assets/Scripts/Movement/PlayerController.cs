using UnityEngine;

namespace Movement
{
    [RequireComponent(typeof(Rigidbody))]
    /// <summary>
    /// ������� ����� ���������� ������
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        // ==== ���� ������ ====
        // ---- �������� ----
        /// <value>�������� ������������</value>
        private const float walkSpeed = 4f;

        /// <value>�������� ����</value>
        private const float sprintSpeed = 7f;

        /// <value>����������� ���������</value>
        private const float SpeedCoef = 1f;

        /// <value>��������� ��������</value>
        private Vector3 _finalVelocity = Vector3.zero;

        // ---- ������������ ----
        /// <value>�������������� ��������</value>
        private Vector3 _horMove;
        /// <value>������������ ��������</value>
        private Vector3 _verMove;

        // ---- ������ ----
        [SerializeField]
        /// <value>���� ������</value>
        private float jumpForceCoef = 15f;

        // ---- ������� ----
        /// <value>������������ �������� ����</value>
        [SerializeField] private float mouseVerSensitivity = 4f;
        /// <value>������������� �������� ����</value>
        [SerializeField] private float mouseHorSensitivity = 4f;
        /// <value>������ �������� �� �����������</value>
        private float _horRot;
        /// <value>������ �������� �� ���������</value>
        private float _verRot = 0;
        /// <value>������������� �������</value>
        private Vector3 _horRotation = Vector3.zero;

        // ---- ���������� Unity ----
        /// <value>������ ������</value>
        [SerializeField] private new Camera camera;
        private new Rigidbody _rigidbody;

        // ==== �������� ====
        /// <summary>
        /// �������� �������� �������� ������
        /// </summary>
        /// <returns>��������� �������� �������� ������</returns>
        public float WalkSpeed => walkSpeed;

        /// <summary>
        /// �������� �������� �������� ��� ����
        /// </summary>
        /// /// <returns>��������� �������� �������� ����</returns>
        public float SprintSpeed => sprintSpeed;

        // ==== ������/������� (�� ��� ���������) ====
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
            // ������������ ��������
            _horMove = transform.right * Input.GetAxis("Horizontal");
            _verMove = transform.forward * Input.GetAxis("Vertical");

            _finalVelocity = CalculateSpeed();

            // ������������ ���� ��������
            _horRot = Input.GetAxis("Mouse X");
            _verRot += Input.GetAxis("Mouse Y") * mouseVerSensitivity;

            _horRotation = new Vector3(0f, _horRot, 0f) * mouseHorSensitivity;

            MovePlayer();
            ChangeViewAngle();
        }
                

        /// <summary>
        /// ������� ������ � ������� ��� ������ rigidbody.
        /// ������� ������, � ������������ 90 �������� ����� � ����
        /// </summary>
        private void ChangeViewAngle()
        {
            _rigidbody.MoveRotation(_rigidbody.rotation * Quaternion.Euler(_horRotation));

            _verRot = Mathf.Clamp(_verRot, -90f, 90f);
            camera.transform.localEulerAngles = new Vector3(-_verRot,
                camera.transform.localEulerAngles.y, camera.transform.localEulerAngles.z);
        }

        /// <summary>
        /// ����������� ������.
        /// </summary>
        private void MovePlayer()
        {
            _rigidbody.MovePosition(_rigidbody.position + _finalVelocity * Time.fixedDeltaTime);
        }

        /// <summary>
        /// ���������, ��������� �� ����� �� ����� ��� ���
        /// </summary>
        /// <returns>����� �� ����� �� �����������</returns>
        private bool IsGrounded()
        {
            return Physics.Raycast(transform.position, Vector3.down, 0.1f);
        }

        /// <summary>
        /// ������������ ��������, �������� ��������� 
        /// </summary>
        /// <returns>��������� �������� ������, ��������, ����� �� �� ��� ���</returns>
        private Vector3 CalculateSpeed()
        {
            if (Input.GetButton("Sprint")) return sprintSpeed * SpeedCoef * (_horMove + _verMove).normalized;
            return walkSpeed * SpeedCoef * (_horMove + _verMove).normalized;
        }
    }
}
