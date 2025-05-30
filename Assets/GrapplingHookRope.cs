using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHookRope : MonoBehaviour
{
    public GrapplingHook HookObject;
    public Player PlayerObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(HookObject.IsHooked || HookObject.IsShot) {

            GetComponent<MeshRenderer>().enabled = true;

            transform.localScale= new Vector3(0.05f,0.05f, (HookObject.transform.position-PlayerObject.transform.position).magnitude);
            transform.position = PlayerObject.transform.position+(HookObject.transform.position-PlayerObject.transform.position)/2;
            transform.LookAt(HookObject.transform.position);
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
