using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerSpawn : MonoBehaviour
{
    public Transform[] SpawnPoints;
    public int m_playerCount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        playerInput.transform.position = SpawnPoints[m_playerCount].transform.position;

        m_playerCount++;
        playerInput.GetComponent<movePlayer>().skinplayer(m_playerCount);
    }
}
