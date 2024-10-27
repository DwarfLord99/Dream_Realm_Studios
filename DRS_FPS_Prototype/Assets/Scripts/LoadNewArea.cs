using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadNewArea : MonoBehaviour
{
    public Transform Destination; // added by Destin 
    public Transform AlternateDestination;
    private GameObject player;
    private CharacterController controller;

    public void Start()
    {
        player = GameObject.FindWithTag("Player");
        controller = player.GetComponent<CharacterController>();
    }
    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Player Detected");
        if (other.CompareTag("Player") && Destination != null)
        {
            //Debug.Log("Destination Found");

            if (gameManager.instance.playerKeys >= 2)
            {
                //Debug.Log("Keys 2 or more, teleporting to alternate area...");
                //Debug.Log(AlternateDestination.transform.position);
                controller.enabled = false;
<<<<<<< Updated upstream
                other.transform.position = AlternateDestination.transform.position;
=======
                SceneManager.LoadScene("Clown_Level");
                other.transform.position = GameObject.FindWithTag("Player Spawn Pos").transform.position;
>>>>>>> Stashed changes
                controller.enabled = true;
                //Debug.Log(other.transform.position);
            }
            else
            {
                //Debug.Log(Destination.transform.position);
                controller.enabled = false;
                other.transform.position = Destination.transform.position;
                controller.enabled = true;
                //Debug.Log(other.transform.position);
            }

        }
    }
}
