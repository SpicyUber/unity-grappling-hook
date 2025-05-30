using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerFinished : MonoBehaviour
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





    private BoolWrapper _isShot;
    public class BoolWrapper
    {
        public bool Value { get; set; }
        public BoolWrapper() { Value = false; }
    }
    private bool _isGrappled = false;
    public GameObject GrapplingHookVisual;
    public GrapplingHookFinished GrapHook;

    void Start()
    {
        
        _startPosition = transform.position;
        _isShot = new();
        _cameraPosition = GameObject.Find("CameraPosition").transform;
        _camera = Camera.main;
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
       if(Cursor.visible) Cursor.visible = false;
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



        if (GrapHook.IsGrappled)
        {
            Vector3 playerMovement = new Vector3(dirx, 0, dirz);
            Vector3 directionTowardsPivot = (GrapHook.transform.position - transform.position).normalized;
            if ((GrapHook.transform.position - transform.position).magnitude > 5) _rb.AddForce((directionTowardsPivot).normalized * Speed  *2 );

            if ((GrapHook.transform.position - transform.position).magnitude < GrapHook.Range) _rb.AddRelativeForce(new Vector3(dirx, 0, dirz) * Speed );
          
        }
        if (temp.magnitude < SpeedCap && GrapHook.IsGrappled == false)
        {

            if (_isGrounded)
            {
                _rb.AddRelativeForce(new Vector3(dirx, 0, dirz) * Speed );
            }
            else
            {
                _rb.AddRelativeForce(new Vector3(dirx, 0, dirz) * Speed / 3);
            }
        }
        else if (GrapHook.IsGrappled == false)
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
            _isGrounded =true;

        }
    }

   private void HandleGrapplingHookInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            GrapHook.Shoot(_cameraPosition.transform.position, _camera.transform.forward, _isShot);

        }

        if (GrapHook.IsGrappled)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _rb.AddForce(_rb.velocity * 10);
                GrapHook.Mesh.enabled = false;
                GrapHook.transform.position = transform.position;
                GrapHook.IsGrappled = false;

                _isShot.Value = false;
            }
        }


    }
    private void HandleGrapplingHookVisuals()
    {
        if (_isShot.Value == true)
        {
            GrapplingHookVisual.transform.localScale = new(0.1f, 0.1f, (GrapHook.transform.position - transform.position).magnitude);
            GrapplingHookVisual.transform.forward = (GrapHook.transform.position - transform.position).normalized;
            GrapplingHookVisual.transform.position = GrapplingHookVisual.transform.forward * 0.02f + (GrapHook.transform.position + transform.position) / 2;
        }
        else
        {
            GrapplingHookVisual.transform.localScale = Vector3.zero;

        }
    }
}
