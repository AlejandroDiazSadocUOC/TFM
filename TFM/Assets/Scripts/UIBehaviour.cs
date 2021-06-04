using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBehaviour : MonoBehaviour
{
    public Texture[] bottomRoomsSprites;
    public Texture[] topRoomsSprites;
    public Texture[] leftRoomsSprites;
    public Texture[] rightRoomsSprites;

    public GameObject m_Minimap;
    public GameObject minimapPanelPrefab;
    public GameObject playerPositionUI;
    public Texture entryRoomSprite;
    public GameObject m_MinimapPanel;
    public Dictionary<int, Dictionary<int, GameObject>> minimapRooms = new Dictionary<int, Dictionary<int, GameObject>>();
    private const int constantSizeSpritesRooms = 55;
    private int rightMinimapLimit = 165, topMinimapLimit = 165;

    public Texture m_HeartPrefab;
    public Texture m_BrokenHeartPrefab;
    public GameObject m_LifePanel;


    private List<GameObject> m_Lifes;

    public static UIBehaviour m_Instance;

    private void Awake()
    {
        if (m_Instance == null)
            m_Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Lifes = new List<GameObject>();
        for(int i = 0; i < PlayerBehaviour.m_instance.m_MaxHealth; i++)
        {
            var go = new GameObject();
            go.AddComponent<RawImage>().texture = m_HeartPrefab;
            go.transform.parent = m_LifePanel.transform;
            go.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(64.0f, 64.0f);
            go.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            go.transform.localPosition = new Vector3(-180.0f + 80.0f *i, 0.0f, 0.0f);
            m_Lifes.Add(go);
        }
        var gamobj = new GameObject("EntryRoom");
        gamobj.AddComponent<RawImage>().texture = entryRoomSprite;
        gamobj.transform.parent = m_MinimapPanel.transform;
        gamobj.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(constantSizeSpritesRooms, constantSizeSpritesRooms);
        gamobj.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);

        if (!minimapRooms.ContainsKey((int)gamobj.transform.localPosition.x))
        {
            minimapRooms.Add((int)gamobj.transform.localPosition.x, new Dictionary<int, GameObject>());
        }
        if (!minimapRooms[(int)gamobj.transform.localPosition.x].ContainsKey((int)gamobj.transform.localPosition.z))
        {
            minimapRooms[(int)gamobj.transform.localPosition.x].Add((int)gamobj.transform.localPosition.z, gamobj);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshLifes()
    {
        StartCoroutine(nameof(RefreshLifesCoroutione));
    }

    // Checks the lifes with the sprites on float values.
    IEnumerator RefreshLifesCoroutione()
    {
        float amount = 0.0f;
        int index = 0;
        while(amount < PlayerBehaviour.m_instance.m_CurrentHealth)
        {
            if(PlayerBehaviour.m_instance.m_CurrentHealth - amount >= 1.0f)
            {
                if (index < m_Lifes.Count)
                {
                    m_Lifes[index].GetComponent<RawImage>().texture = m_HeartPrefab;
                }
                else
                {
                    var go = new GameObject();
                    go.AddComponent<RawImage>().texture = m_HeartPrefab;
                    go.transform.parent = m_LifePanel.transform;
                    go.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(64.0f, 64.0f);
                    go.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                    go.transform.localPosition = new Vector3(-180.0f + 80.0f * index, 0.0f, 0.0f);
                    m_Lifes.Add(go);
                }
                amount += 1.0f;
                index++;
            }
            else
            {
                if (index < m_Lifes.Count)
                {
                    m_Lifes[index].GetComponent<RawImage>().texture = m_BrokenHeartPrefab;
                }
                else
                {
                    var go = new GameObject();
                    go.AddComponent<RawImage>().texture = m_BrokenHeartPrefab;
                    go.transform.parent = m_LifePanel.transform;
                    go.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(64.0f, 64.0f);
                    go.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                    go.transform.localPosition = new Vector3(-180.0f + 80.0f * index, 0.0f, 0.0f);
                    m_Lifes.Add(go);
                }
                
                amount += 0.5f;
                index++;
            }
        }
        while(m_Lifes.Count > index)
        {
            Destroy(m_Lifes[m_Lifes.Count - 1]);
            m_Lifes.RemoveAt(m_Lifes.Count-1);
        }
        yield return null;
    }

    // Adds the replica minimap room just created in the level
    public void AddMinimapRoom(GameObject go,int openingDirection,int roomNumber)
    {
        GameObject gamobj = new GameObject();
        switch (openingDirection)
        {
            case 1:
                gamobj.AddComponent<RawImage>().texture = bottomRoomsSprites[roomNumber];
                gamobj.transform.parent = m_MinimapPanel.transform;
                gamobj.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(constantSizeSpritesRooms, constantSizeSpritesRooms);
                gamobj.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                gamobj.transform.localPosition = new Vector3(((int)go.transform.position.x / 10) * constantSizeSpritesRooms, ((int)go.transform.position.z / 10) * constantSizeSpritesRooms, 0.0f);
                break;
            case 2:
                gamobj.AddComponent<RawImage>().texture = topRoomsSprites[roomNumber];
                gamobj.transform.parent = m_MinimapPanel.transform;
                gamobj.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(constantSizeSpritesRooms, constantSizeSpritesRooms);
                gamobj.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                gamobj.transform.localPosition = new Vector3(((int)go.transform.position.x / 10) * constantSizeSpritesRooms, ((int)go.transform.position.z / 10) * constantSizeSpritesRooms, 0.0f);
                break;
            case 3:
                gamobj.AddComponent<RawImage>().texture = leftRoomsSprites[roomNumber];
                gamobj.transform.parent = m_MinimapPanel.transform;
                gamobj.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(constantSizeSpritesRooms, constantSizeSpritesRooms);
                gamobj.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                gamobj.transform.localPosition = new Vector3(((int)go.transform.position.x / 10) * constantSizeSpritesRooms, ((int)go.transform.position.z / 10) * constantSizeSpritesRooms, 0.0f);
                break;
            case 4:
                gamobj.AddComponent<RawImage>().texture = rightRoomsSprites[roomNumber];
                gamobj.transform.parent = m_MinimapPanel.transform;
                gamobj.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(constantSizeSpritesRooms, constantSizeSpritesRooms);
                gamobj.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
                gamobj.transform.localPosition = new Vector3(((int)go.transform.position.x / 10) * constantSizeSpritesRooms, ((int)go.transform.position.z / 10) * constantSizeSpritesRooms, 0.0f);
                break;
            default:
                break;
        }
        if (!minimapRooms.ContainsKey((int)go.transform.position.x))
        {
            minimapRooms.Add((int)go.transform.position.x, new Dictionary<int, GameObject>());
        }
        if (!minimapRooms[(int)go.transform.position.x].ContainsKey((int)go.transform.position.z))
        {
            minimapRooms[(int)go.transform.position.x].Add((int)go.transform.position.z, gamobj);
        }
        if(gamobj.transform.localPosition.x >= rightMinimapLimit)
        {
            m_MinimapPanel.transform.localPosition = new Vector3(m_MinimapPanel.transform.localPosition.x - constantSizeSpritesRooms, m_MinimapPanel.transform.localPosition.y, m_MinimapPanel.transform.localPosition.z);
            rightMinimapLimit += constantSizeSpritesRooms;
        }
        if (gamobj.transform.localPosition.y >= topMinimapLimit)
        {
            m_MinimapPanel.transform.localPosition = new Vector3(m_MinimapPanel.transform.localPosition.x, m_MinimapPanel.transform.localPosition.y - constantSizeSpritesRooms, m_MinimapPanel.transform.localPosition.z);
            topMinimapLimit += constantSizeSpritesRooms;
        }
        gamobj.SetActive(false);
    }

    // Set active the room discovered in the minimap
    public void RoomDiscovered(int x,int z)
    {
        if (minimapRooms.ContainsKey(x))
        {
            if (minimapRooms[x].ContainsKey(z))
            {
                if (!minimapRooms[x][z].activeSelf)
                    minimapRooms[x][z].SetActive(true);
            }
        }

        playerPositionUI.transform.localPosition = new Vector3(minimapRooms[x][z].transform.localPosition.x , minimapRooms[x][z].transform.localPosition.y , 0.0f);
    }

    // Sets the next level of the UI cleaning the current minimap and starting generating the new one.
    public void NextLevel()
    {
        minimapRooms.Clear();
        Destroy(m_MinimapPanel);
        m_MinimapPanel = Instantiate(minimapPanelPrefab);
        m_MinimapPanel.transform.parent = m_Minimap.transform;
        m_MinimapPanel.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);

        var gamobj = new GameObject("EntryRoom");
        gamobj.AddComponent<RawImage>().texture = entryRoomSprite;
        gamobj.transform.parent = m_MinimapPanel.transform;
        gamobj.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(constantSizeSpritesRooms, constantSizeSpritesRooms);
        gamobj.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);

        if (!minimapRooms.ContainsKey((int)gamobj.transform.localPosition.x))
        {
            minimapRooms.Add((int)gamobj.transform.localPosition.x, new Dictionary<int, GameObject>());
        }
        if (!minimapRooms[(int)gamobj.transform.localPosition.x].ContainsKey((int)gamobj.transform.localPosition.z))
        {
            minimapRooms[(int)gamobj.transform.localPosition.x].Add((int)gamobj.transform.localPosition.z, gamobj);
        }

        for (int i = 0; i < m_MinimapPanel.transform.childCount; i++)
        {
            Transform child = m_MinimapPanel.transform.GetChild(i);
            if (child.CompareTag("PlayerUI"))
            {
                playerPositionUI = child.gameObject;
            }
        }

       

    }
}
