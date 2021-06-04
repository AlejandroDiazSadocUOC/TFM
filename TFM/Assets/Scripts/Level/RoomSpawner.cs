using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
	public int openingDirection;


	private RoomTemplates templates;
	private int rand;
	public bool spawned = false;

	public float waitTime = 4f;

	void Start()
	{
		Destroy(gameObject, waitTime);

		templates = GameManager.m_Instance.roomTemplates;
		Invoke("Spawn", 0.1f);
	}


	void Spawn()
	{
		// Checks limits of rooms per level.
		if (templates.rooms.Count <= GameManager.m_Instance.LevelMaxRooms)
		{
			if (spawned == false)
			{
				GameObject gamobj = null;
				if (openingDirection == 1)
				{
					// Bottom door.
					rand = Random.Range(0, templates.bottomRooms.Length);
					gamobj = Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation);
					gamobj.transform.parent = templates.roomsParents.transform;
				}
				else if (openingDirection == 2)
				{
					// Top door.
					rand = Random.Range(0, templates.topRooms.Length);
					gamobj = Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation);
					gamobj.transform.parent = templates.roomsParents.transform;
				}
				else if (openingDirection == 3)
				{
					// Left  door.
					rand = Random.Range(0, templates.leftRooms.Length);
					gamobj = Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation);
					gamobj.transform.parent = templates.roomsParents.transform;
				}
				else if (openingDirection == 4)
				{
					// Right door.
					rand = Random.Range(0, templates.rightRooms.Length);
					gamobj = Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation);
					gamobj.transform.parent = templates.roomsParents.transform;
				}
                if (!templates.roomsDictionary.ContainsKey((int)gamobj.transform.position.x))
                {
                    templates.roomsDictionary.Add((int)gamobj.transform.position.x, new Dictionary<int, GameObject>());
                }
                if (!templates.roomsDictionary[(int)gamobj.transform.position.x].ContainsKey((int)gamobj.transform.position.z))
                {
                    templates.roomsDictionary[(int)gamobj.transform.position.x].Add((int)gamobj.transform.position.z, gamobj);
                }

				// Add minimap room in UI
				UIBehaviour.m_Instance.AddMinimapRoom(gamobj, openingDirection, rand);
                spawned = true;
			}
        }
        else
        {
			// Spawns only rooms with a door

            if (spawned == false)
            {
				GameObject gamobj = null;
				if (openingDirection == 1)
                {
					// Need to spawn a room with a BOTTOM door.
					gamobj = Instantiate(templates.bottomRoom, transform.position, templates.bottomRoom.transform.rotation);
					gamobj.transform.parent = templates.roomsParents.transform;
				}
                else if (openingDirection == 2)
                {
					// Need to spawn a room with a TOP door.

					gamobj = Instantiate(templates.topRoom, transform.position, templates.topRoom.transform.rotation);
					gamobj.transform.parent = templates.roomsParents.transform;
				}
                else if (openingDirection == 3)
                {
					// Need to spawn a room with a LEFT door.
					gamobj = Instantiate(templates.leftRoom, transform.position, templates.leftRoom.transform.rotation);
					gamobj.transform.parent = templates.roomsParents.transform;
				}
                else if (openingDirection == 4)
                {
					// Need to spawn a room with a RIGHT door.
					gamobj = Instantiate(templates.rightRoom, transform.position, templates.rightRoom.transform.rotation);
					gamobj.transform.parent = templates.roomsParents.transform;
				}
                if (!templates.roomsDictionary.ContainsKey((int)gamobj.transform.position.x))
                {
                    templates.roomsDictionary.Add((int)gamobj.transform.position.x, new Dictionary<int, GameObject>());
                }
                if (!templates.roomsDictionary[(int)gamobj.transform.position.x].ContainsKey((int)gamobj.transform.position.z))
                {
                    templates.roomsDictionary[(int)gamobj.transform.position.x].Add((int)gamobj.transform.position.z, gamobj);
                }
                spawned = true;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
		if (other.CompareTag("SpawnPoint"))
		{
			spawned = true;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("SpawnPoint"))
		{
			if (other.GetComponent<RoomSpawner>().spawned == false && spawned == false)
			{
				Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
				Destroy(gameObject);
			}
			spawned = true;
		}
	}
}
