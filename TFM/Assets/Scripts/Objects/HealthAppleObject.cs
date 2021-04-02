using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthAppleObject : MonoBehaviour
{
    [SerializeField]
    private float m_HealthToRestore = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            PlayerBehaviour.m_instance.AddLife(m_HealthToRestore);
            Destroy(gameObject);
        }
    }
}
