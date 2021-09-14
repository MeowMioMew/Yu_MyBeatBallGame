using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{   
    
    //variables
    float _speed = 20f;
    Rigidbody _rigidBody;
    Vector3 _velocity;
    Renderer _renderer;
    public AudioSource _source;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        Invoke("Launch", 0.5f);
        _source = GetComponent<AudioSource>();
    }

    void Launch()
    {
        _rigidBody.velocity = Vector3.up* _speed;
    }

    void FixedUpdate()
    {   // Forces the ball to have the indicated speed
        _rigidBody.velocity = _rigidBody.velocity.normalized * _speed;
        _velocity = _rigidBody.velocity;

        if(!_renderer.isVisible)
        {
            GameManager.Instance.Ball--;
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
       
        _rigidBody.velocity = Vector3.Reflect(_velocity, collision.contacts[0].normal);
        _source.Play();
    }
}
