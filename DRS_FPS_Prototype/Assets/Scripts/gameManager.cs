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
    static tutorialManager tutorialManager;
    
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
    [SerializeField] public GameObject menuGraphics;
    [SerializeField] public GameObject menuAudio;
    [SerializeField] public GameObject menuControls;
    [SerializeField] public GameObject menuGameplay;

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
                menuPause.SetActive(isPaused);
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
        isPaused = !isPaused;
        Time.timeScale = timeScaleOrig;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
        menuGraphics.SetActive(false);
        menuAudio.SetActive(false);
        menuControls.SetActive(false);
        menuGameplay.SetActive(false);

    }

}
