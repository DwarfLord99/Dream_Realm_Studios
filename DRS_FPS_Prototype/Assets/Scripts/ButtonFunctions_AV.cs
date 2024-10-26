using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions_AV : MonoBehaviour
{
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
        gameManager.instance.menuLose.SetActive(false); // to hide the lose menu when pressing respawn
    }
}
