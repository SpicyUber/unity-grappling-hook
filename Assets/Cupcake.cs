using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cupcake : MonoBehaviour
{
    private bool _win = false;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") && !_win)
        {
            GetComponentInChildren<ParticleSystem>()?.Play();
            _win = true;
        }
    }
}
