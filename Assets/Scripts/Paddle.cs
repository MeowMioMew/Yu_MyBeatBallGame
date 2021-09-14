using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    Rigidbody _rigidBody;


    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();    
    }

   
    void FixedUpdate()
    {
        _rigidBody.MovePosition(new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,0,50)).x, -17, 0));
        
    
    }
}
