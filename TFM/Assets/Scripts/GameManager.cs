using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager m_Instance;

    public  int CurrentLevel = 1;

    public  int LevelMaxRooms;

    public GameObject[] m_BossPrefabs;


    public GameObject m_PanelRoomToMove;

    public GameObject m_RoomTemplatesPrefab;
    public GameObject m_RoomTemplates;
    public GameObject m_EntryRoomPrefab;

    private GameObject m_Player;
    private GameObject m_Camera;
    public RoomTemplates roomTemplates;
    

    private void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;

        LevelMaxRooms = (int)Random.Range(13.0f, 21.0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Player = GameObject.FindGameObjectWithTag("Player");
        m_Camera = GameObject.FindGameObjectWithTag("MainCamera");
        //roomTemplates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextLevel()
    {
        CurrentLevel++;
        Destroy(GameObject.FindGameObjectWithTag("EntryRoom"));

        foreach (GameObject room in roomTemplates.rooms)
        {
            Destroy(room);
        }
        roomTemplates.rooms.Clear();
        roomTemplates.roomsDictionary.Clear();
        Destroy(m_RoomTemplates);
        m_RoomTemplates = Instantiate(m_RoomTemplatesPrefab);
        roomTemplates = m_RoomTemplates.GetComponent<RoomTemplates>();
        Instantiate(m_EntryRoomPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        m_Player.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        m_Camera.transform.position = new Vector3(0.0f, 10.0f, 0.0f);
    }

    public  IEnumerator MoveX(Vector3 position, bool isRight)
    {
        m_PanelRoomToMove.SetActive(true);
        if (isRight)
        {
            ChangeRoom(position, new Vector3(position.x + 10.0f, position.y, position.z));
            m_Camera.transform.position = new Vector3(position.x + 10.0f, m_Camera.transform.position.y, m_Camera.transform.position.z);
            m_Player.transform.position = new Vector3(position.x + 10.0f, m_Player.transform.position.y, m_Player.transform.position.z);
        }
        else
        {
            ChangeRoom(position, new Vector3(position.x - 10.0f, position.y, position.z));
            m_Camera.transform.position = new Vector3(position.x - 10.0f, m_Camera.transform.position.y, m_Camera.transform.position.z);
            m_Player.transform.position = new Vector3(position.x - 10.0f, m_Player.transform.position.y, m_Player.transform.position.z);
        }
        yield return new WaitForSecondsRealtime(0.1f);
        m_PanelRoomToMove.SetActive(false);
        if (roomTemplates.roomsDictionary.ContainsKey((int)position.x))
        {
            if (roomTemplates.roomsDictionary[(int)position.x].ContainsKey((int)position.z))
            {
                roomTemplates.roomsDictionary[(int)position.x][(int)position.z].SetActive(false);
            }
        }
        Time.timeScale = 1;
    }

    public  IEnumerator MoveZ(Vector3 position, bool isTop)
    {
        m_PanelRoomToMove.SetActive(true);
        if (isTop)
        {
            ChangeRoom(position, new Vector3(position.x,position.y,position.z + 10.0f));
            m_Camera.transform.position = new Vector3(m_Camera.transform.position.x, m_Camera.transform.position.y, position.z + 10.0f);
            m_Player.transform.position = new Vector3(m_Player.transform.position.x, m_Player.transform.position.y, position.z + 10.0f);
        }
        else
        {
            ChangeRoom(position, new Vector3(position.x, position.y, position.z - 10.0f));
            m_Camera.transform.position = new Vector3(m_Camera.transform.position.x, m_Camera.transform.position.y, position.z - 10.0f);
            m_Player.transform.position = new Vector3(m_Player.transform.position.x, m_Player.transform.position.y, position.z - 10.0f);
        }
        yield return new WaitForSecondsRealtime(0.1f);
        m_PanelRoomToMove.SetActive(false);
        if (roomTemplates.roomsDictionary.ContainsKey((int)position.x))
        {
            if (roomTemplates.roomsDictionary[(int)position.x].ContainsKey((int)position.z))
            {
                roomTemplates.roomsDictionary[(int)position.x][(int)position.z].SetActive(false);
            }
        }
        Time.timeScale = 1;
    }

    public void ChangeRoom(Vector3 currentRoomPosition,Vector3 nextRoomPosition)
    {
        if (roomTemplates.roomsDictionary.ContainsKey((int)nextRoomPosition.x))
        {
            if (roomTemplates.roomsDictionary[(int)nextRoomPosition.x].ContainsKey((int)nextRoomPosition.z))
            {
                roomTemplates.roomsDictionary[(int)nextRoomPosition.x][(int)nextRoomPosition.z].GetComponent<RoomBehaviour>().SetRoomActive();
            }
        }
    }
}
