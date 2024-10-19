using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using static UnityEditor.Progress;

public class tutorialManager : MonoBehaviour
{
    //preliminary items to ensure that everything runs
    public static tutorialManager instance;
    public bool tutorialEnabled = true;
    bool tutorialRunning = false; //even though tutorial might be enabled, we want a way to turn off the tutorial so that the script keeps track of it
    Dictionary<string, string> tutorialPage; //making a dictionary here is faster because you can just add an entry and relevant the relevant display logic based on whether it has children

    Transform parentObject;
    Transform page;
    bool hasMorePages;
    int index = 0;

    string itemAdded;
    // Awake instead of start to set the instance like in gameManager 
    void Awake()
    {
        instance = this;
        parentObject = GetComponent<Transform>();

        tutorialPage = new Dictionary<string, string>()
        {
            {"Key", "Key Explainer"},
            {"Gun", "Gun Explainer" },
            {"Enemy", "Fear Meter" },
            {"Alternate Destination", "Main Hub" }
        };
    }

    // Update listens for when specific criteria are met, and then it fires the script based on that
    void Update()
    {
        /*
        if (parentObject.name == "Gun Explainer" && Input.GetKeyDown("Fire1"))
        {
            gameManager.instance.stateUnpause();
            tutorialRunning = false;
        }
        else if (this.tag == "Key")
        {
            Debug.Log(this.tag);
            if (Input.GetKeyDown(KeyCode.E))
            {
                gameManager.instance.stateUnpause();
                tutorialRunning = false;
            }
        }
        
        if (tutorialRunning) //is the tutorial running?
        {
           

        }*/

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(this.tag);
            if(tutorialEnabled)
            {
                Debug.Log("Working fine");
                StartCoroutine(FireTutorial());
            }
        }
    }
    IEnumerator FireTutorial()
    {
        //gameManager.instance.statePause();
        tutorialRunning = true;
        index = 0;
        Debug.Log(tutorialPage[this.tag]);
        parentObject = GameObject.Find("Fear Meter"/*tutorialPage[this.tag]*/).transform;

        if (parentObject.childCount > 0) //if the object has no kids, just run the explainer script
        {
            hasMorePages = true;
            Debug.Log("Working fine");
            FireExplainer(index);
        }
        else  //if the object has kids, just fire it here, otherwise Update will handle the rest
        {
            parentObject.gameObject.SetActive(true);
        }

       
        Debug.Log("Working fine");
        Debug.Log("E key pressed. Continuing....");

        while (hasMorePages)
        {
            Debug.Log($"Current Index: {index}");
            index++;
            FireExplainer(index);
        }


        gameManager.instance.stateUnpause();
        tutorialRunning = false;

        yield return new WaitForSeconds(0.5f);
    }



    public void StartTutorial(string item)
    {
        
    }

    bool FireExplainer(int index)
    {

        if (index > parentObject.childCount - 1)
        {
            hasMorePages = false;
            return false;
        }
        if (hasMorePages)
        {
            if (page != null && page.gameObject.activeSelf)
            {
                Debug.Log("Working fine");
                page.gameObject.SetActive(false);
            }

            Debug.Log("Working fine");
            page = parentObject.GetChild(index);
            page.gameObject.SetActive(true);
        }

        Debug.Log("Working fine");
        return hasMorePages;
    }

}
