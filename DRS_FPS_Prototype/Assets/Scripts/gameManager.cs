using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
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

    // Menu GameObjects - Adriana V
    [SerializeField] public GameObject menuMain;
    [SerializeField] public GameObject menuPlay;
    [SerializeField] public GameObject menuLoadGame;
    [SerializeField] public GameObject menuAbout;
    [SerializeField] public GameObject menuOptions;
    [SerializeField] public GameObject menuCredits;
    [SerializeField] public GameObject menuHelp;
    [SerializeField] public GameObject menuTutorial;
    [SerializeField] public GameObject menuSettings; 

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
       // ShowMenu(menuMain); // shows the main menu when the game starts
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
        //enemyCount += amount;
        playerKeys += amount;
        //enemyCountText.text = enemyCount.ToString("F0"); // added by Adriana V

        //key-based win con --Destin Bernavil

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

    // Shows the main menu and hides all the other menus - Adriana V
    public void ShowMenu(GameObject menu)
    {
        Debug.Log("Showing menu: " + menu.name);
        HideMenus();
        menu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // ensures cursor is visible
    }

    // method to hide all menus in the game - Adriana V
    public void HideMenus()
    {
        // deactivates all menu GameObjects
        menuMain.SetActive(false);
        menuPlay.SetActive(false);
        menuLoadGame.SetActive(false);
        menuAbout.SetActive(false);
        menuOptions.SetActive(false);
        menuCredits.SetActive(false);
        menuHelp.SetActive(false);
        menuTutorial.SetActive(false);
        menuSettings.SetActive(false);
    }

}
