using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneralBehaviour : MonoBehaviour
{
    public Action<GameObject> OnDestroy;

    public List<GameObject> m_ObjectsPrefabs;
    [SerializeField]
    private float probabilityToLoot = 0.25f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LootSpawn()
    {
        if (UnityEngine.Random.Range(0.0f, 1.0f) < probabilityToLoot)
        {
            float value = UnityEngine.Random.Range(0.0f, 1.0f);

            if(value <= 0.33)
            {
                Instantiate(m_ObjectsPrefabs[0], transform.position, m_ObjectsPrefabs[0].transform.rotation,transform.parent);
            }else if(value <= 0.66)
            {
                Instantiate(m_ObjectsPrefabs[1], transform.position, m_ObjectsPrefabs[1].transform.rotation, transform.parent);
            }
            else
            {
                Instantiate(m_ObjectsPrefabs[2], transform.position, m_ObjectsPrefabs[2].transform.rotation, transform.parent);
            }
        }
    }
}
