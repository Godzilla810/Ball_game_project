using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private Transform target;
    public float speed = 15.0f;
    private bool homing;
    private float bulletStrength = 15.0f;
    private float  aliveTimer = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(homing && target != null){
            Vector3 moveDirection = (target.transform.position - transform.position).normalized;
            transform.position += moveDirection * speed * Time.deltaTime;
            transform.LookAt(target);
        }
    }

    public void Fire(Transform newTarget){
        target = newTarget; 
        homing = true;
        Destroy(gameObject, aliveTimer);
    }

    void  OnCollisionEnter(Collision col){
        if(target != null){
            Rigidbody targetRigidbody = col.gameObject.GetComponent<Rigidbody>();
            Vector3 away = -col.contacts[0].normal;
            targetRigidbody.AddForce(away * bulletStrength, ForceMode.Impulse);
            Destroy(gameObject);
        }
    }
}
