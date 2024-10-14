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
    [SerializeField] public GameObject menuSettings;

    // method to show the play menu
    public void Play()
    {
        gameManager.instance.ShowMenu(menuPlay);
    }

    // method to start a new game by loading the main game scene
    public void NewGame()
    {
        Debug.Log("NewGame method called.");  
        SceneManager.LoadScene(1);
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
