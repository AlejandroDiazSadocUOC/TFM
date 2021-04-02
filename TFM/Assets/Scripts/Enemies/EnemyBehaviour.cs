using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    private GameObject m_Player;
    private Rigidbody m_Rigidbody;
    private NavMeshAgent m_NavAgent;
    private Animator m_Animator;

    private float m_Health = 100.0f;
    // Start is called before the first frame update
    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_Rigidbody = GetComponent<Rigidbody>();
        m_NavAgent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, m_Player.transform.position) < 10.0f)
        {
            //transform.position = Vector3.MoveTowards(transform.position, m_Player.transform.position, 0.005f);
            m_NavAgent.SetDestination(m_Player.transform.position);
            transform.LookAt(m_Player.transform.position);
            if (!m_Animator.GetBool("isWatchingPlayer"))
                m_Animator.SetBool("isWatchingPlayer", true);
            
        }else if (m_Animator.GetBool("isWatchingPlayer"))
        {
            m_Animator.SetBool("isWatchingPlayer", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            m_Health = 0;
            CheckDeath();
            PlayerBehaviour.m_instance.ReduceLife(0.5f);
            PlayerBehaviour.m_instance.CheckDeath();
        }
        else if (collision.transform.CompareTag("Bullet"))
        {
            m_Health -= 20;
            CheckDeath();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Bullet"))
        {
            m_Health -= 20;
            CheckDeath();
        }
    }

    private void CheckDeath() 
    {
        if (m_Health <= 0)
            Destroy(gameObject);
    }
}
