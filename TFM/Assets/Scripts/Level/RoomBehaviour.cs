using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{

    public List<GameObject> m_EnemiesPrefabs;
    public GameObject m_ExitPrefab;

    private int m_EnemyIndex;
    private int m_EnemiesQuantity;
    private float EnemiesProb = -1;
    [SerializeField]
    private bool m_IsBossRoom = false;
    private List<GameObject> m_Enemies = new List<GameObject>();

    private bool isExitSpawned = false;
    
    
    // Start is called before the first frame update
    void Start()
    {
        // Start generation of room and its enemies
        while(Random.Range(0,1) > EnemiesProb)
        {
            m_EnemyIndex = Random.Range(0, m_EnemiesPrefabs.Count);
            m_EnemiesQuantity = Random.Range(1,3);
            EnemiesProb = EnemiesProb < 0 ? 0 : EnemiesProb;
            EnemiesProb += 0.2f * m_EnemiesQuantity;
            for(int  i = 0; i < m_EnemiesQuantity; i++)
            {
                var gamobj=Instantiate(m_EnemiesPrefabs[m_EnemyIndex], new Vector3(transform.position.x + Random.Range(-4.0f, 4.0f), 
                    m_EnemiesPrefabs[m_EnemyIndex].transform.position.y, transform.position.z + Random.Range(-4.0f, 4.0f)), Quaternion.identity , transform);
                gamobj.GetComponent<EnemyGeneralBehaviour>().OnDestroy = OnDestroyEnemy;
                gamobj.SetActive(false);
                m_Enemies.Add(gamobj);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Spawn the exit only once for boss rooms
        if (m_IsBossRoom && m_Enemies.Count <= 0 && !isExitSpawned)
        {
            isExitSpawned = true;
            Instantiate(m_ExitPrefab, transform.position, Quaternion.identity);
        }
    }

    // Set gameobject of room active and its enemies too.
    public void SetRoomActive()
    {
        gameObject.SetActive(true);
        foreach(GameObject go in m_Enemies)
        {
            if(go != null)
                go.SetActive(true);
        }
        if (m_IsBossRoom && m_Enemies.Count > 0)
            m_Enemies[0].GetComponent<Boss1Behaviour>().StartCoroutine("AttackCoroutine");

    }

    // Select this room as the boss room.
    public void SetBossRoom(GameObject boss)
    {
        boss.SetActive(false);
        m_IsBossRoom = true;
        foreach(GameObject enemy in m_Enemies)
        {
            Destroy(enemy);
        }
        m_Enemies.Clear();
        m_Enemies.Add(boss);
        boss.GetComponent<EnemyGeneralBehaviour>().OnDestroy = OnDestroyEnemy;
        boss.transform.parent = transform;
    }

    public void OnDestroyEnemy(GameObject enemy)
    {
        enemy.GetComponent<EnemyGeneralBehaviour>().LootSpawn();
        m_Enemies.Remove(enemy);
        Destroy(enemy);
    }
}
