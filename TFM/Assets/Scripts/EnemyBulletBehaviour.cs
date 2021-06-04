using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBehaviour : MonoBehaviour
{
    public List<string> m_TagsToCollide;
    private Rigidbody m_Rigidbody;
    private SphereCollider m_SphereCollider;

    private float m_BulletSpeed = 5.0f;

    [SerializeField]
    private float m_BulletDmg = 0.5f;
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

        Vector3 movement = transform.forward * m_BulletSpeed  * Time.deltaTime;
        // Apply this movement to the rigidbody's position
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (m_TagsToCollide.Contains(collision.transform.tag))
        {
            Destroy(gameObject);
            if (collision.transform.CompareTag("Player"))
            {
                PlayerBehaviour.m_instance.ReduceLife(m_BulletDmg);
                PlayerBehaviour.m_instance.CheckDeath();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_TagsToCollide.Contains(other.transform.tag))
        {
            m_SphereCollider.isTrigger = false;
            Destroy(gameObject);
            if (other.transform.CompareTag("Player"))
            {
                PlayerBehaviour.m_instance.ReduceLife(m_BulletDmg);
                PlayerBehaviour.m_instance.CheckDeath();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
    }

    private IEnumerator CheckIfHit()
    {
        if(this != null)
        {
            yield return new WaitForSeconds(3.0f);
            Destroy(gameObject);
        }
        yield return null;
    }
}
