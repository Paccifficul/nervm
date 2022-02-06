using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
/// <summary>
/// ������� ����� ���������� ������
/// </summary>
public class PlayerController : MonoBehaviour
{
    // ==== ���� ������ ====
    // ---- �������� ----
    /// <value>�������� ������������</value>
    private readonly float walkSpeed = 4f;
    /// <value>�������� ����</value>
    private readonly float sprintSpeed = 7f;
    /// <value>����������� ���������</value>
    protected float speedCoef = 1f;
    /// <value>��������� ��������</value>
    private Vector3 finalVelocity = Vector3.zero;

    // ---- ������������ ----
    /// <value>�������������� ��������</value>
    private Vector3 horMove;
    /// <value>������������ ��������</value>
    private Vector3 verMove;

    // ---- ������ ----
    [SerializeField]
    /// <value>���� ������</value>
    private float jumpForceCoef = 15f;

    // ---- ������� ----
    [SerializeField]
    /// <value>������������ �������� ����</value>
    private float mouseVerSensivity = 4f;
    /// <value>������������� �������� ����</value>
    [SerializeField]
    private float mouseHorSensivity = 4f;
    /// <value>������ �������� �� �����������</value>
    private float horRot;
    /// <value>������ �������� �� ���������</value>
    private float verRot = 0;
    /// <value>������������� �������</value>
    private Vector3 horRotation = Vector3.zero;

    // ---- ���������� Unity ----
    /// <value>������ ������</value>
    [SerializeField]
    private new Camera camera;
    private new Rigidbody rigidbody;

    // ==== �������� ====
    /// <summary>
    /// �������� �������� �������� ������
    /// </summary>
    /// <returns>��������� �������� �������� ������</returns>
    public float WalkSpeed
    {
        get
        {
            return walkSpeed;
        }
    }
    /// <summary>
    /// �������� �������� �������� ��� ����
    /// </summary>
    /// /// <returns>��������� �������� �������� ����</returns>
    public float SprintSpeed
    {
        get
        {
            return sprintSpeed;
        }
    }

    // ==== ������/������� (�� ��� ���������) ====
    // Start is called before the first frame update
    protected void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        // ������������ ��������
        horMove = transform.right * Input.GetAxis("Horizontal");
        verMove = transform.forward * Input.GetAxis("Vertical");

        finalVelocity = CalculateSpeed();

        // ������������ ���� ��������
        horRot = Input.GetAxis("Mouse X");
        verRot += Input.GetAxis("Mouse Y") * mouseVerSensivity;

        horRotation = new Vector3(0f, horRot, 0f) * mouseHorSensivity;

        MovePlayer(); 
        ChangeViewAngle();
    }

    /// <summary>
    /// ������� ������ � ������� ��� ������ rigidbody.
    /// ������� ������, � ������������ 90 �������� ����� � ����
    /// </summary>
    private void ChangeViewAngle()
    {
        rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(horRotation));

        verRot = Mathf.Clamp(verRot, -90f, 90f);
        camera.transform.localEulerAngles = new Vector3(-verRot, 
            camera.transform.localEulerAngles.y, camera.transform.localEulerAngles.z);
    }

    /// <summary>
    /// ����������� ������. ������
    /// </summary>
    private void MovePlayer()
    {
        rigidbody.MovePosition(rigidbody.position + finalVelocity * Time.fixedDeltaTime);

        if (IsGrounded() && Input.GetButtonDown("Jump"))
            rigidbody.AddForce(Vector3.up * jumpForceCoef, ForceMode.Impulse);
    }

    /// <summary>
    /// ���������, ��������� �� ����� �� ����� ��� ���
    /// </summary>
    /// <returns>����� �� ����� �� �����������</returns>
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.0f);
    }

    /// <summary>
    /// ������������ ��������, �������� ��������� 
    /// </summary>
    /// <returns>��������� �������� ������, ��������, ����� �� �� ��� ���</returns>
    private Vector3 CalculateSpeed()
    {
        if (Input.GetButton("Sprint")) return sprintSpeed * speedCoef * (horMove + verMove).normalized;
        return walkSpeed * speedCoef * (horMove + verMove).normalized;
    }
}
