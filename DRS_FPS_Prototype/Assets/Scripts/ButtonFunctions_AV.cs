using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions_AV : MonoBehaviour
{
    // method to show the play menu 
    public void Play()
    {
        gameManager.instance.ShowMenu(gameManager.instance.menuPlay);
    }

    // method to start a new game by loading the latest active scene. 
    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.stateUnpause(); 
    }

    // method to show the load game menu
    public void LoadGame()
    {
        gameManager.instance.ShowMenu(gameManager.instance.menuLoadGame); 
    }

    // method to show the about menu
    public void About()
    {
        gameManager.instance.ShowMenu(gameManager.instance.menuAbout);
    }

    // method to show the options menu
    public void Options()
    {
        gameManager.instance.ShowMenu(gameManager.instance.menuOptions);
    }

    // method to show the credits menu
    public void Credits()
    {
        gameManager.instance.ShowMenu(gameManager.instance.menuCredits);
    }

    // method to show the help menu
    public void ShowHelp()
    {
        gameManager.instance.ShowMenu(gameManager.instance.menuHelp);
    }

    // method to show the tutorial menu
    public void ShowTutorial()
    {
        gameManager.instance.ShowMenu(gameManager.instance.menuTutorial);
    }

    // method for save button functionality. Not written yet.
    public void SaveButton()
    {

    }

    // method for back button - will change to specific which menu to go back to but for now will stick to main menu
    public void BackButton()
    {
        gameManager.instance.ShowMenu(gameManager.instance.menuMain);
    }

    public void resume()
     {
         gameManager.instance.stateUnpause();
     }

    // method to restart the game scene over
  public void restart()
    {
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
         gameManager.instance.stateUnpause(); 
    }

    // method to quit game entirely - will be switched to exit button
  public void quit()
    {
         #if UNITY_EDITOR
             UnityEditor.EditorApplication.isPlaying = false;
         #else
             Application.Quit();
         #endif
    }

    // allows the player to respawn when the menu button is pressed
    public void Respawn() // added by Alexander Martone
    {
        gameManager.instance.playerScript.PlayerSpawn();
        gameManager.instance.stateUnpause();
        
    }
}
