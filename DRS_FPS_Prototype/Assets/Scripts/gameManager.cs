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

    // Menu switching for all the menus in the main menu - Adriana V
    public GameObject menuMain;
    public GameObject menuPlay;
    public GameObject menuAbout;
    public GameObject menuOptions;
    public GameObject menuCredits;
    public GameObject menuHelp;
    public GameObject menuTutorial;
    public GameObject menuLoadGame;

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
        
        // ShowMenu(menuMain); // show main menu when game first opens, commented out for now. some issues with the main menu. will work on fixing today. - Adriana V
    }

    // show the main menu and call the hide menus method to hide the other menus - Adriana V
    public void ShowMenu(GameObject menu)
    {
        HideMenus();
        menu.SetActive(true);
    }

    // method to hide all menus in the game - Adriana V
    public void HideMenus()
    {
        menuMain.SetActive(false);
        menuPlay.SetActive(false);
        menuAbout.SetActive(false);
        menuOptions.SetActive(false);
        menuCredits.SetActive(false);
        menuHelp.SetActive(false);
        menuTutorial.SetActive(false);
        menuLoadGame.SetActive(false);
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

        //Commented out to prevent win condition from triggering when 4 enemies spawn in-game - RL
        //if (playerKeys == 4)
        //{
        //    statePause();
        //    menuActive = menuWin;
        //    menuActive.SetActive(isPaused);
        //}
    }

    public void youlose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

}
