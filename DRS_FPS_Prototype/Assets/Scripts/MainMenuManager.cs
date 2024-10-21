using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager instance;
    public SettingsMenu_RL saveSettings;

    // Menu GameObjects
    [SerializeField] public GameObject menuMain;
    [SerializeField] public GameObject menuPlay;
    [SerializeField] public GameObject menuLoadGame;
    [SerializeField] public GameObject menuAbout;
    [SerializeField] public GameObject menuOptions;
    [SerializeField] public GameObject menuCredits;
    [SerializeField] public GameObject menuHelp;
    [SerializeField] public GameObject menuTutorial;
    [SerializeField] public GameObject menuSettings;

    //// Pop-up GameObjects
    [SerializeField] public GameObject newGamePopUp;
    [SerializeField] public GameObject backPopUp;
    [SerializeField] public GameObject savePopUp;
    [SerializeField] public GameObject exitPopUp;
    [SerializeField] public GameObject fastForwardPopup;

    public void Start()
    {
        // hide all pop-ups at start
        HideAllPopUps();
    }

    public void HideAllPopUps()
    {
        // hide all pop-ups
        newGamePopUp.SetActive(false); 
        exitPopUp.SetActive(false); 
        savePopUp.SetActive(false); 
        backPopUp.SetActive(false); 
        fastForwardPopup.SetActive(false);
    }

    //// method to show new game pop-up window
    public void ShowNewGamePopup()
    {
        Debug.Log("ShowNewGamePopup called");
        HideAllPopUps();
        newGamePopUp.SetActive(true);
    }

    //// method to ok new game when ok button is clicked
    public void OkNewGame()
    {
        newGamePopUp.SetActive(false);
        SceneManager.LoadScene("MAINSCENE");
    }

    //// method to cancel new game pop up when cancel button is clicked
    public void CancelNewGame()
    {
        newGamePopUp.SetActive(false);
    }

    //// method to show the back button pop-up window
    public void ShowBackPopup()
    {
        HideAllPopUps();
        backPopUp.SetActive(true);
    }

    //// method to ok to go back to main menu when ok button is clicked
    public void OkBack()
    {
        backPopUp.SetActive(false);
        BackToMainMenu();
    }

    //// method to cancel to go back to main menu when cancel button is clicked
    public void CancelBack()
    {
        backPopUp.SetActive(false);
    }

    //// method to show save button pop up window
    public void ShowSavePopup()
    {
        HideAllPopUps();
        savePopUp.SetActive(true);
    }

    //// method to ok to save the setting changes
    public void OkSave()
    {
        savePopUp.SetActive(false);
        SaveButton();
    }

    //// method to cancel save changes
    public void CancelSave()
    {
        savePopUp.SetActive(false);
    }

    //// method to show exit pop-up window
    public void ShowExitPopup()
    {
        Debug.Log("ShowExitPopup called");
        HideAllPopUps();
        exitPopUp.SetActive(true);
    }

    //// method to ok to exit the game
    public void OKExit()
    {
        exitPopUp.SetActive(false);
        Exit();
    }

    //// method to cancel exiting the game
    public void CancelExit()
    {
        exitPopUp.SetActive(false);
    }

    //// method to show the fast forward pop up window
    public void ShowFastForwardPopup()
    {
        HideAllPopUps();
        fastForwardPopup.SetActive(true);
    }

    //// method to ok to fast forward credits
    public void OKFastForward()
    {
        fastForwardPopup.SetActive(false);
    }

    //// method to cancel fast forward credits
    public void CancelFastForward()
    {
        fastForwardPopup.SetActive(false);
    }

    // method to show the play menu
    public void Play()
    {
        gameManager.instance.ShowMenu(menuPlay);
    }

    // method to start a new game by loading the main game scene
    public void NewGame()
    {
        //Debug.Log("NewGame method called.");  
        SceneManager.LoadScene("MAINSCENE");
    }

    // method to show the about menu
    public void LoadGame()
    {
        gameManager.instance.ShowMenu(menuLoadGame);
    }

    // method to show the help menu
    public void About()
    {
        gameManager.instance.ShowMenu(menuAbout);
    }

    // method to show the help menu
    public void Help()
    {
        gameManager.instance.ShowMenu(menuHelp);
    }

    // method to show the tutorial menu
    public void Tutorial()
    {
        gameManager.instance.ShowMenu(menuTutorial);
    }

    // method to show the options menu which will open up all game settings
    public void Options()
    {
        gameManager.instance.ShowMenu(gameManager.instance.menuOptions);
        menuSettings.gameObject.SetActive(true);

        // initialize settingsMenu reference
        if (saveSettings == null)
        {
            saveSettings = FindObjectOfType<SettingsMenu_RL>();
        }
    }

    // method to show the credits menu
    public void Credits()
    {
        gameManager.instance.ShowMenu(menuCredits);
    }

    // method to take the player back to the main menu
    public void BackToMainMenu()
    {
        gameManager.instance.ShowMenu(menuMain);
    }

    // method to take the player back to the about menu when on the help or tutorial menus
    public void BackToAboutMenu()
    {
        gameManager.instance.ShowMenu(menuAbout);
    }

    // method for the save button which will function for save settings
    public void SaveButton()
    {
        if (saveSettings != null)
        {
            saveSettings.SaveAllSettings(); // reference the settings menu to call the save settings method
        }
    
    }

    // method to fast forward the credits text in the credits menu
    public void FastForwardButton()
    {
        // have not implemented this button's functionality yet
    }

    // method to completely exit the game
    public void Exit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
