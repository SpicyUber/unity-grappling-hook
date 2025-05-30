using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    private bool _isGrounded = false;
    private float _minY = -50;
    public int JumpForce;
    public int Speed;
    public int SpeedCap;
    private Camera _camera;
    private Rigidbody _rb;
    private Transform _cameraPosition;
    private Vector2 _movedir;
    private Vector3 _startPosition;







    void Start()
    {

        _startPosition = transform.position;

        _cameraPosition = GameObject.Find("CameraPosition").transform;
        _camera = Camera.main;
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Cursor.visible) Cursor.visible = false;
        if (transform.position.y < _minY) { transform.position = _startPosition; }

        _movedir = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));


        if (_isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            _isGrounded = false;
            Jump();

        }

        //pucanje grappling hook-a i iskakanje iz hook-a
        HandleGrapplingHookInput();

        //vidljivost, duzina, pozicija, pravac konopca grappling hook-a
        HandleGrapplingHookVisuals();




    }


    //u okviru FixedUpdate-a izmeni logiku tako da ukljucuje i grappling hook
    private void FixedUpdate()
    {
        float dirx = _movedir.normalized.x;
        float dirz = _movedir.normalized.y;
        Vector3 temp = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);




        if (temp.magnitude < SpeedCap)
        {

            if (_isGrounded)
            {
                _rb.AddRelativeForce(new Vector3(dirx, 0, dirz) * Speed );
            }
            else
            {
                _rb.AddRelativeForce(new Vector3(dirx, 0, dirz) * Speed / 3 );
            }
        }
        else
        {
            _rb.AddForce(_rb.velocity * (-0.25f));
        }

    }

    private void LateUpdate()
    {
        transform.Rotate(0, Input.GetAxis("Mouse X") * 800 * Time.deltaTime, 0);
        Quaternion tempRotation = _cameraPosition.rotation;

        _cameraPosition.Rotate(Input.GetAxis("Mouse Y") * -800 * Time.deltaTime, 0, 0);

        if (Vector3.Dot(transform.forward, _cameraPosition.forward) < 0.01f)
        {
            _cameraPosition.rotation = tempRotation;
        }
        _camera.transform.position = _cameraPosition.position;
        _camera.transform.forward = _cameraPosition.forward;

    }

    private void Jump()
    {
        _rb.AddForce(transform.up * JumpForce);



    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            _isGrounded = true;

        }
    }

    private void HandleGrapplingHookInput()
    {


    }
    private void HandleGrapplingHookVisuals()
    {

    }
}
