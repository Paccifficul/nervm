using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    // ==== VARIABLES ====
    // ---- Speed ----
    [SerializeField]
    protected float speed = 3f;
    protected float speedCoef = 1f;
    Vector3 finalVelocity = Vector3.zero;

    // ---- Movement ----
    private Vector3 horMove; // Go left or right
    private Vector3 verMove; // Go forward or back

    // ---- Jump ----
    [SerializeField]
    private float jumpForceCoef = 10f;

    // ---- Rotation ----
    [SerializeField]
    private float mouseVerSensivity = 3f;
    [SerializeField]
    private float mouseHorSensivity = 3f;
    private float horRot; // Horizontal rotation
    private float verRot; // Vertical rotation
    private Vector3 horRotation = Vector3.zero;
    private Vector3 verRotation = Vector3.zero;

    // ---- Unity components ----
    [SerializeField]
    private new Camera camera;
    private new Rigidbody rigidbody;


    // Start is called before the first frame update
    protected void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    protected void FixedUpdate()
    {
        // Calculate speed
        horMove = transform.right * Input.GetAxis("Horizontal");
        verMove = transform.forward * Input.GetAxis("Vertical");

        finalVelocity = speed * speedCoef * (horMove + verMove).normalized;

        // Calculate rotate angle
        horRot = Input.GetAxis("Mouse X");
        verRot = Input.GetAxis("Mouse Y");

        horRotation = new Vector3(0f, horRot, 0f) * mouseHorSensivity;
        verRotation = new Vector3(verRot, 0f, 0f) * mouseVerSensivity;

        MovePlayer(); // Apply move
        ChangeViewAngle(); // Apply rotation
    }

    private void ChangeViewAngle()
    {
        // Change view angle
        rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(horRotation));

        // Rotate camera up/down with limit at -90 and 90 degress
        verRot = Mathf.Clamp(verRot, -90f, 90f);
        camera.transform.localEulerAngles = new Vector3(-verRot, 
            camera.transform.localEulerAngles.y, camera.transform.localEulerAngles.z);
    }

    private void MovePlayer()
    {
        // Change position 
        rigidbody.MovePosition(rigidbody.position + finalVelocity * Time.fixedDeltaTime);

        // Jump
        if (IsGrounded() && Input.GetButton("Jump"))
            rigidbody.AddForce(Vector3.up * jumpForceCoef, ForceMode.Impulse);
    }

    // Check, if we're on ground
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.0f);
    }
}
