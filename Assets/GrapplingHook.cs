using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class GrapplingHook : MonoBehaviour
{
    private Vector3 _dir; // Vector3 v= new(x,y,z);
    public bool IsShot;
    private Rigidbody _rigidbody;
    public bool IsHooked;
    private Coroutine _coroutine;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody=GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsHooked || IsShot)
        {

            GetComponent<MeshRenderer>().enabled = true;
        } else
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

  public void  Shoot(Vector3 position, Vector3 dir )
    {
        if(_coroutine!=null) { StopCoroutine(_coroutine); }
        _coroutine = StartCoroutine(TimeoutCoroutine());
        IsHooked = false;
        IsShot = true;
        transform.position = position;
        _dir = dir.normalized;
        _rigidbody.velocity = dir*30f;
    

    }
    IEnumerator TimeoutCoroutine()
    {
        yield return new WaitForSeconds( 5.0f );
        if( !IsHooked ) { IsShot = false; }

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (IsShot) { 
            IsShot = false;
        _rigidbody.velocity = Vector3.zero;
        IsHooked = true;
        }
    }
}
