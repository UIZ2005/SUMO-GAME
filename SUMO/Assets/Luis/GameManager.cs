using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private PlayerSpawn playerSpawn;
    public GameObject Canvafull;
    public GameObject canvaWin;
    public TextMeshProUGUI texto;
    public int playersdeath=0;
    private GameObject[] players;
    void Start()
    {
        playerSpawn = gameObject.GetComponent<PlayerSpawn>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void start()
    {
        if (playerSpawn.m_playerCount>=2)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in players)
            {
                player.GetComponent<movePlayer>().isdead = false;
            }
                Canvafull.SetActive(false);
        }
       
    }
    public void countDeath()
    {
        string playerwin = "";
        playersdeath += 1;
        int playerslive = playerSpawn.m_playerCount - playersdeath;

        if (playerslive == 1)
        {
            foreach (GameObject player in players)
            {
                if (!player.GetComponent<movePlayer>().isdead)
                {
                    playerwin = player.GetComponent<movePlayer>().numplayer.ToString();
                }
            }
            StartCoroutine(winner(playerwin));
           
        }
    }
    IEnumerator winner(string playerwin)
    {
        canvaWin.SetActive(true);
        texto.text = playerwin;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        yield return null;
    }
}
