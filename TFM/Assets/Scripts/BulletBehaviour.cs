using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public List<string> m_TagsToCollide;

    private SphereCollider m_SphereCollider;
    private Rigidbody m_Rigidbody;
    private float m_BulletSpeed = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_SphereCollider = GetComponent<SphereCollider>();
        StartCoroutine(CheckIfHit());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
    }


    private void Move()
    {
        // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames
        Vector3 movement = transform.forward * m_BulletSpeed  * Time.deltaTime;
        // Apply this movement to the rigidbody's position
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (m_TagsToCollide.Contains(collision.transform.tag))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_TagsToCollide.Contains(other.transform.tag))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (m_SphereCollider.isTrigger)
        //    m_SphereCollider.isTrigger = false;
    }
    private IEnumerator CheckIfHit()
    {
        if(this != null)
        {
            yield return new WaitForSeconds(3.0f);
            Destroy(gameObject);
        }
    }
}
