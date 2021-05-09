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
        if (m_IsBossRoom && m_Enemies.Count <= 0 && !isExitSpawned)
        {
            isExitSpawned = true;
            Instantiate(m_ExitPrefab, transform.position, Quaternion.identity);
            //GameManager.m_Instance.NextLevel();
        }
    }

    public void SetRoomActive()
    {
        gameObject.SetActive(true);
        foreach(GameObject go in m_Enemies)
        {
            if(go != null)
                go.SetActive(true);
        }
    }

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
    }

    public void OnDestroyEnemy(GameObject enemy)
    {
        m_Enemies.Remove(enemy);
        Destroy(enemy);
    }
}
