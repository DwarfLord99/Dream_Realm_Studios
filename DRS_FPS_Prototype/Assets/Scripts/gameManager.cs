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
    //static tutorialManager tutorialManager;

    // enables/disables tutorial for all items
    public bool tutorialEnabled = true;
    public bool tutorialRunning = false;

    [SerializeField] public GameObject menuActive;
    [SerializeField] public GameObject menuPause;
    [SerializeField] public GameObject menuWin;
    [SerializeField] public GameObject menuLose;

    // Menu GameObjects - Adriana V
    [SerializeField] public GameObject menuMain;
    [SerializeField] public GameObject menuPlay;
    [SerializeField] public GameObject menuAbout;
    [SerializeField] public GameObject menuOptions;
    [SerializeField] public GameObject menuCredits;
    [SerializeField] public GameObject menuSettings;
    [SerializeField] public GameObject menuControls;
  
    
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
    private GameObject[] enemies;


    int enemyCount;
    public int playerKeys;

    float timeScaleOrig;

    public bool isPaused;

    // Start is called before the first frame update
    void Awake()
    {
       instance = this;
      
       timeScaleOrig = Time.timeScale;
       
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MAINSCENE")) //-RL, added check to get the main menu working
        {
            player = GameObject.FindWithTag("Player");
            playerScript = player.GetComponent<PlayerMovement>();
            playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos"); // added by Fuad H

            //extra code to pause enemies for tutorial
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
        }

        //Sets frames of game to aim at running at target rate -RL
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // replaced the GetButtonDown("Cancel") to bind the pause button to the P key instead of the esc key. It was causing issues with the pause menu and resume button. - Adriana V
        {
          togglePauseMenu();
        }

        
    }

    public void togglePauseMenu()
    {
        if (!isPaused)
        {
            statePause();
            menuActive = menuPause;
            menuPause.SetActive(true); // set pause menu to true and removed it set to isPaused - Adriana V
        }
        else
        {
            stateUnpause();
            menuActive = null;
            menuPause.SetActive(false); 
        }
    }

    public void statePause()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateUnpause()
    {
        isPaused = false; // removed !isPaused to set the paused state to false - Adriana V
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // apply a check to make sure the game can resume properly by hiding the pause menu - Adriana V
        if (menuPause != null)
        {
            menuPause.SetActive(false);
        }

    }

    public void updateGameGoal(int amount)
    {
        enemyCount += amount;
        playerKeys += amount;
        enemyCountText.text = enemyCount.ToString("F0"); // added by Adriana V

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
        //Debug.Log("Showing menu: " + menu.name);
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
        menuAbout.SetActive(false);
        menuOptions.SetActive(false);
        menuCredits.SetActive(false);
        menuSettings.SetActive(false);
        menuControls.SetActive(false);

    }

    // To fix the issue with the keys updating in the monster kills text, I created 2 methods to separate count updates for enemies killed and keys collected
    // method to update enemy count - AV
    public void updateEnemiesKilled(int amount)
    {
        enemyCount += amount;
        enemyCountText.text = enemyCount.ToString("F0");
    }

    // method to update keys collected - AV
    public void updateKeysCollected(int amount)
    {
        playerKeys += amount;
        if (playerKeys == 4)
        {
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(isPaused);
        }
    }

    // method to reset everything after hitting back to main menu button & relaunch a new game
    public void ResetGameState()
    {
        // reset player state
        if (playerScript != null)
        {
            playerScript.ResetPlayerState();
        }

        // reset game state variables
        enemyCount = 0;
        playerKeys = 0;

        // reset UI text
        enemyCountText.text = enemyCount.ToString("F0");

        // ensure game is not paused
        isPaused = false;
        Time.timeScale = timeScaleOrig;
    }
}
