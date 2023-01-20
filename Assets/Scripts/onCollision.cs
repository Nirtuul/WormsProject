using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class onCollision : MonoBehaviour
{
    private Collider _missileBody;
    private AudioSource _explosionSource;

    private Component GameController;
    //private GameObject Explosion;

    private Rigidbody _rb;
    // Start is called before the first frame update
    private void Awake()
    {
        _missileBody = GetComponent<CapsuleCollider>();
        _rb = GetComponent<Rigidbody>();
        _explosionSource = GetComponent<AudioSource>();
        _explosionSource.clip = Resources.Load<AudioClip>("Explosion");
        //Explosion = Resources.Load<GameObject>("Explosion1");
        GameController = GetComponent<GameController>();

    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(_rb.velocity);
    }

    private void OnCollisionEnter(Collision collision)
    {//Debug.Log(collision.transform.name);
        if (collision.transform.CompareTag("Ground"))
        {
            var test=ExplosionForce(collision.relativeVelocity);
            ContactPoint contactWithGround = collision.contacts[0];
            //Debug.Log(collision.contacts[0].point);
            GameObject.Find("Main Camera").GetComponent<AudioSource>().clip = _explosionSource.clip;
            GameObject.Find("Main Camera").GetComponent<AudioSource>().Play();
            Destroy(this.gameObject);
            Vector2 position = new Vector2(collision.transform.position.x, collision.transform.position.y);
            //Debug.Log(collision.transform.position);
            GameObject.Find("GameController").GetComponent<GameController>().ExplodeBlocks(position, 1.0f);
            
        }
        else if (collision.transform.CompareTag("Player"))
        {
            //Vector2 positionOfWorm = new Vector2(collision.transform.position.x, collision.transform.position.y);
            //GetComponent<GameController>().ExplodeBlocks(positionOfWorm,1.0f);
            //Debug.Log(collision.transform.name);
        }
    }

    public Vector3 ExplosionForce(Vector3 relativeVelocity)
    {
        
        float m = _rb.mass;
        Vector3 a = relativeVelocity / Time.deltaTime;
        Vector3 force = m * a;
        return force;

    }

    public float ExplosionRadius(float force)
    {
        float baseRadius = 0.5f;
        return baseRadius * force;
    }
}
