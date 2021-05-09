using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
	public GameObject[] bottomRooms;
	public GameObject[] topRooms;
	public GameObject[] leftRooms;
	public GameObject[] rightRooms;

	public GameObject closedRoom;
	
	public GameObject leftRoom;
	public GameObject rightRoom;
	public GameObject topRoom;
	public GameObject bottomRoom;

	public List<GameObject> rooms;
	public GameObject roomsParents;

	public Dictionary<int, Dictionary<int, GameObject>> roomsDictionary = new Dictionary<int, Dictionary<int, GameObject>>();

	public float waitTime;
	private bool spawnedBoss;
	public GameObject boss;

    void Start()
    {
        if (roomsParents == null)
        {
			roomsParents = GameObject.FindGameObjectWithTag("RoomsParent");
        }
    }

    void Update()
	{

		if (waitTime <= 0 && spawnedBoss == false)
		{
			for (int i = 0; i < rooms.Count; i++)
			{
				if (i == rooms.Count - 1)
				{
					var bossSelected = GameManager.m_Instance.m_BossPrefabs[Random.Range(0, GameManager.m_Instance.m_BossPrefabs.Length)];
					var boss =Instantiate(bossSelected,new Vector3(rooms[i].transform.position.x + Random.Range(-4.0f, 4.0f), 
						bossSelected.transform.position.y, rooms[i].transform.position.z + Random.Range(-4.0f, 4.0f)), Quaternion.identity , transform);
			spawnedBoss = true;
					rooms[i].GetComponent<RoomBehaviour>().SetBossRoom(boss);
                    rooms[i].gameObject.SetActive(false);
                }
                rooms[i].gameObject.SetActive(false);
            }
		}
		else
		{
			waitTime -= Time.deltaTime;
		}
	}
}
