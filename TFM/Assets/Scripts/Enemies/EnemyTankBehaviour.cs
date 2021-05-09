using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTankBehaviour : MonoBehaviour
{
    private GameObject m_Player;
    private Rigidbody m_Rigidbody;
    private NavMeshAgent m_NavAgent;
    private Animator m_Animator;

    private float m_Health = 500.0f;
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
            m_NavAgent.SetDestination(m_Player.transform.position);
            transform.LookAt(m_Player.transform.position);
            if (!m_Animator.GetBool("isWatchingPlayer"))
                m_Animator.SetBool("isWatchingPlayer", true);
            if (Vector3.Distance(transform.position, m_Player.transform.position) <= 1.5f && !m_Animator.GetBool("Attacks"))
                m_Animator.SetBool("Attacks", true);
            else if(Vector3.Distance(transform.position, m_Player.transform.position) > 1.5f && m_Animator.GetBool("Attacks"))
                m_Animator.SetBool("Attacks", false);
        }
        else if (m_Animator.GetBool("isWatchingPlayer"))
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
            PlayerBehaviour.m_instance.ReduceLife(1.0f);
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
        {
            GetComponent<EnemyGeneralBehaviour>().OnDestroy.Invoke(gameObject);
        }
    }
}
