using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

            if (this.CompareTag("LoadScene"))
            {
                SceneManager.LoadScene("Clown_Level");
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
