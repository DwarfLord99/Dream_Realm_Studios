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

  public void restart()
    {
         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
         gameManager.instance.stateUnpause(); 
    }

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
