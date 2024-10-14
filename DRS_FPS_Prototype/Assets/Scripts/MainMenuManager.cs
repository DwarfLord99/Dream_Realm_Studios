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

    // Menu GameObjects
    [SerializeField] public GameObject menuMain;
    [SerializeField] public GameObject menuPlay;
    [SerializeField] public GameObject menuLoadGame;
    [SerializeField] public GameObject menuAbout;
    [SerializeField] public GameObject menuOptions;
    [SerializeField] public GameObject menuCredits;
    [SerializeField] public GameObject menuHelp;
    [SerializeField] public GameObject menuTutorial;

    private void Awake()
    {
        instance = this;
        ShowMenu(menuMain); // show main menu when the game starts
    }

    // Shows the main menu and hides all the other menus
    public void ShowMenu(GameObject menu)
    {
        HideMenus();
        menu.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // ensures cursor is visible
    }

    // method to hide all menus in the game
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
    }

    // method to show the play menu
    public void Play()
    {
        ShowMenu(menuPlay);
    }

    // method to start a new game by loading the main game scene
    public void NewGame()
    {
        gameManager.instance.stateUnpause();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
       
    }

    // method to show the about menu
    public void LoadGame()
    {
        ShowMenu(menuLoadGame);
    }

    // method to show the help menu
    public void About()
    {
        ShowMenu(menuAbout);
    }

    // method to show the help menu
    public void Help()
    {
        ShowMenu(menuHelp);
    }

    // method to show the tutorial menu
    public void Tutorial()
    {
        ShowMenu(menuTutorial);
    }

    // method to show the options menu which will open up all game settings
    public void Options()
    {
        ShowMenu(menuOptions);
    }

    // method to show the credits menu
    public void Credits()
    {
        ShowMenu(menuCredits);
    }

    // method to take the player back to the main menu
    public void BackToMainMenu()
    {
        ShowMenu(menuMain);
    }

    // method to take the player back to the about menu when on the help or tutorial menus
    public void BackToAboutMenu()
    {
        ShowMenu(menuAbout);
    }

    // method for the save button which will function for save settings
    public void SaveButton()
    {
        // have not implemented this button's functionality yet
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
