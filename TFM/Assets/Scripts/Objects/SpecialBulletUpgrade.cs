using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBulletUpgrade : MonoBehaviour
{
    private int BulletNumber = 1;
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
            PlayerBehaviour.m_instance.ChangeCurrentBullet(BulletNumber);
            Destroy(gameObject);
        }
    }
}
