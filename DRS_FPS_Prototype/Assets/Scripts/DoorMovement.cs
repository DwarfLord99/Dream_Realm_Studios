//unused due to time constraints
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DoorMovement : MonoBehaviour
{
    Transform door;
    Animator animator;
    bool drawGUI = false;
    bool doorIsClosed = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (drawGUI && Input.GetKeyDown(KeyCode.E))
        {
            changeDoorState();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            drawGUI = true;
            Debug.Log("Working");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            drawGUI = false;
        }
    }

    private void OnGUI()
    {
        if (drawGUI)
            GUI.Box(new Rect(Screen.width / 2, Screen.height / 2, 102f, 22f), "Press E to Interact");
    }

    private void changeDoorState()
    {
        if(doorIsClosed)
        {
            animator.CrossFade("Open", .3f);
            doorIsClosed = false;
        }
        else
        {
            animator.CrossFade("Close", .3f);
            doorIsClosed = true;
        }
    }
}
