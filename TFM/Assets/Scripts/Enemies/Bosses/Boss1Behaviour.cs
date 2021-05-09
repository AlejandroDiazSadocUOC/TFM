using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Behaviour : MonoBehaviour
{
    private GameObject m_Player;
    private Animator m_Animator;

    private float m_Health = 250.0f;
    private int attackSelected = 1;
    private int consecutiveAttacks = 0;
    // Start is called before the first frame update
    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_Animator = GetComponent<Animator>();
        StartCoroutine(AttackCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
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
        {
            GetComponent<EnemyGeneralBehaviour>().OnDestroy.Invoke(gameObject);
        }
    }

    IEnumerator AttackCoroutine()
    {
        if (attackSelected == 1)
            StartCoroutine(Attack1(m_Player.transform.position));
        else if(attackSelected == 2)
            StartCoroutine(Attack2(m_Player.transform.position));
        else if (attackSelected == 3)
            StartCoroutine(Attack3(m_Player.transform.position));
        yield return null;
    }

    IEnumerator Attack1(Vector3 position)
    {
        m_Animator.Play("Attack1");
        while (Vector3.Distance(position,transform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, 0.01f);
            yield return null;
        }
        consecutiveAttacks++;
        ChangeAttack();
    }

    IEnumerator Attack2(Vector3 position)
    {
        m_Animator.Play("Attack2");
        while (Vector3.Distance(position, transform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, 0.01f);
            yield return null;
        }
        consecutiveAttacks++;
        ChangeAttack();
    }

    IEnumerator Attack3(Vector3 position)
    {
        m_Animator.Play("Attack3");
        while (Vector3.Distance(position, transform.position) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, position, 0.01f);
            yield return null;
        }
        consecutiveAttacks++;
        ChangeAttack();
    }

    void ChangeAttack()
    {
        if(Random.Range(0.0f, 1.0f) < 0.15 * consecutiveAttacks)
        {
            consecutiveAttacks = 0;
            attackSelected = Random.Range(1, 4);
        }
        StartCoroutine(AttackCoroutine());
    }
}
