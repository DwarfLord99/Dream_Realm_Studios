using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        if (tutorialRunning) //is the tutorial running?
        {
            if (parentObject.name == "Gun Explainer" && Input.GetKeyDown("Fire1"))
            {
                gameManager.instance.stateUnpause();
                tutorialRunning = false;
            }
            else if(Input.GetKeyDown(KeyCode.E))
            {
                if(parentObject.name == "Key Explainer")
                {
                    gameManager.instance.stateUnpause();
                    tutorialRunning = false;
                }
                else
                {
                    index++;
                    FireExplainer(index);
                    if (hasMorePages == false)
                    {
                        index = 0;
                    }
                }
            }

        }

    }

    public void FireTutorial(string item)
    {
        gameManager.instance.statePause();
        tutorialRunning = true;
        //Debug.Log(tutorialPage[item]);
        parentObject = GameObject.Find(tutorialPage[item]).transform;

        if(parentObject.childCount > 0) //if the object has no kids, just run the explainer script
        {
            hasMorePages = true;    
            FireExplainer(index);
        }
        else  //if the object has kids, just fire it here, otherwise Update will handle the rest
        {
            parentObject.gameObject.SetActive(true);
        }
    }

    bool FireExplainer(int index)
    {
        if (index > parentObject.childCount - 1)
            hasMorePages = false;
        if (hasMorePages)
        {
            if (page.gameObject.activeSelf)
            {
                page.gameObject.SetActive(false);
            }
            page = parentObject.GetChild(index);
            page.gameObject.SetActive(true);
        }
        return hasMorePages;
    }

}
