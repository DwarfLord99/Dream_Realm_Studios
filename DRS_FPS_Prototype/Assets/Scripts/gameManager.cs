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
    private GameObject[] enemies;


    int enemyCount;
    public int playerKeys;

    float timeScaleOrig;

    public bool isPaused;

    // Start is called before the first frame update
    void Awake()
    {
        //DontDestroyOnLoad(this.transform.root.gameObject); // test By fuad H. 

        // to ensure music doesn't restart when changing menus - AV
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
      
       
       timeScaleOrig = Time.timeScale;
       
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MAINSCENE")) //-RL, added check to get the main menu working
        {
            player = GameObject.FindWithTag("Player");
            playerScript = player.GetComponent<PlayerMovement>();
            playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos"); // added by Fuad H

            //extra code to pause enemies for tutorial
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // replaced the GetButtonDown("Cancel") to bind the pause button to the P key instead of the esc key. It was causing issues with the pause menu and resume button. - Adriana V
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuPause.SetActive(true); // set pause menu to true and removed it set to isPaused - Adriana V
            }
            else if (menuActive == menuPause)
            {
                stateUnpause();
                menuActive.SetActive(false); //bool might be getting toggled too many times, otherwise switch back to isPaused, moved up to Update to use stateUnpause in tutorialManager (Destin)
                menuActive = null; 
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
        menuLoadGame.SetActive(false);
        menuAbout.SetActive(false);
        menuOptions.SetActive(false);
        menuCredits.SetActive(false);
        menuHelp.SetActive(false);
        menuTutorial.SetActive(false);
        menuSettings.SetActive(false);

    }

}
