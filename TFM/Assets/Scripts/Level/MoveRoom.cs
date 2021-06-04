using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRoom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Check x and z coords to know where the player wants to go regardin the actual room
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0;
            if (Mathf.Abs(other.transform.position.x - transform.position.x) > Mathf.Abs(other.transform.position.z - transform.position.z))
            {
                if (other.transform.position.x > transform.position.x)
                {
                    StartCoroutine(GameManager.m_Instance.MoveX(transform.position, true));
                }
                else if (other.transform.position.x < transform.position.x)
                {
                    StartCoroutine(GameManager.m_Instance.MoveX(transform.position, false));
                }
            }
            else if (Mathf.Abs(other.transform.position.z) > Mathf.Abs(other.transform.position.x))
            {
                if (other.transform.position.z > transform.position.z)
                {
                    
                    StartCoroutine(GameManager.m_Instance.MoveZ(transform.position, true));
                }
                else if (other.transform.position.z < transform.position.z)
                {
                    StartCoroutine(GameManager.m_Instance.MoveZ(transform.position, false));
                }
            }
        }
    }
}
