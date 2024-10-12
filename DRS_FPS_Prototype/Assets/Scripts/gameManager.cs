using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//added by Zachary D
using UnityEngine.UI;


public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] TMP_Text enemyCountText; // added by Adriana V
    public TMP_Text ammoCur, ammoMax; // added by Fuad H

    //added by Zachary D
    public Image playerHPBar;
   

    public GameObject playerSpawnPos; // added by Fuad H

    public GameObject checkpointPopup; // added by Fuad H
    public GameObject damagePanel;
    public GameObject player;
    public GameObject DamageVignetteImage;
    public PlayerMovement playerScript;


    int enemyCount;
    int playerKeys;

    float timeScaleOrig;

    public bool isPaused;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        timeScaleOrig = Time.timeScale;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerMovement>();
        playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos"); // added by Fuad H
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuPause.SetActive(isPaused);
            }
            else if (menuActive == menuPause)
            {
                stateUnpause();
            }
        }
    }

    public void statePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateUnpause()
    {
        isPaused = !isPaused;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false); //bool might be getting toggled too many times, otherwise switch back to isPaused
        menuActive = null;
    }

    public void updateGameGoal(int amount)
    {
        enemyCount += amount;
        playerKeys += amount;
        enemyCountText.text = enemyCount.ToString("F0"); // added by Adriana V

        if (enemyCount <= 0)
        {
            // player wins the game
            //Debug.Log("You Win!!!");
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(isPaused);
        }

        // testing key-based win con --Destin Bernavil

        if (playerKeys == 4)
        {
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(isPaused);
        }
    }

    public void youlose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

}
