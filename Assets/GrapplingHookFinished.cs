using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class GrapplingHookFinished : MonoBehaviour
{
    public bool IsGrappled = false;
    public int Range = 80;
    public MeshRenderer Mesh;
    Coroutine currentCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        Mesh = GetComponent<MeshRenderer>();
        Mesh.enabled = false;
        Range = 80;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(Vector3 startpos, Vector3 direction, PlayerFinished.BoolWrapper isShot)
    {
        if(currentCoroutine != null) { StopCoroutine(currentCoroutine); }
        IsGrappled = false;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        isShot.Value = true;
        Mesh.enabled = true;
        currentCoroutine = StartCoroutine(ShootCoroutine(startpos, direction, isShot));

    }

    IEnumerator ShootCoroutine(Vector3 startpos, Vector3 dir, PlayerFinished.BoolWrapper isShot)
    {
        
      
        transform.position = startpos;
        GetComponent<Rigidbody>().AddForce(dir *20,ForceMode.VelocityChange);
        while( !(IsGrappled || (startpos-transform.position).magnitude>=Range ) ) { 
     

        
        yield return null;  
        
        }

        if (!IsGrappled) { isShot.Value = false; Mesh.enabled = false; } else {  GetComponent<Rigidbody>().velocity=Vector3.zero; }


    }
    private void OnTriggerEnter(Collider collision)
    {
        if(Mesh.enabled==false) { return; }
        if( collision != null && !collision.gameObject.CompareTag("Player") ) {

            IsGrappled = true;
        
        }
    }
}
