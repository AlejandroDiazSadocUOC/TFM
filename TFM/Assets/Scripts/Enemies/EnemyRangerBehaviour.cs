using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyRangerBehaviour : MonoBehaviour
{
    public GameObject bulletPrefab;

    private GameObject m_Player;
    private NavMeshAgent m_NavAgent;
    private Animator m_Animator;

    [SerializeField]
    private float m_FireCD = 0.5f;

    private float lastShot = 0.0f;
    private float m_CurrentAmmo = 5.0f;
    private float m_MaxAmmo = 5.0f;

    private float m_Health = 20.0f;
    // Start is called before the first frame update
    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        lastShot = Time.time;
        m_NavAgent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponent<Animator>();
        StartCoroutine(CheckAmmo());
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, m_Player.transform.position) < 10.0f)
        {
            FireIfPossible();
            if (!m_Animator.GetBool("PlayerSpotted"))
                m_Animator.SetBool("PlayerSpotted", true);
        }
        else if (m_Animator.GetBool("PlayerSpotted"))
        {
            m_Animator.SetBool("PlayerSpotted", false);
        }

    }

    private void FireIfPossible()
    {
        if(Time.time - lastShot >= m_FireCD && m_CurrentAmmo > 0)
        {
            lastShot = Time.time;
            transform.LookAt(m_Player.transform.position);
            m_CurrentAmmo--;
            Instantiate(bulletPrefab, new Vector3(transform.position.x, 0.3f, transform.position.z),  transform.rotation);
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

    private IEnumerator CheckAmmo()
    {
        m_CurrentAmmo = m_CurrentAmmo <= 0 ? m_MaxAmmo : m_CurrentAmmo;
        yield return new WaitForSeconds(5.0f);
        StartCoroutine(CheckAmmo());
    }
}
