using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.Audio;
using static System.Collections.Specialized.BitVector32;
using UnityEngine.UI;
using TMPro; 

public class SettingsMenu_RL : MonoBehaviour
{
    //The SettingsMenu script will need to be attached to the main part of the UI itself so that it has access to everything it will
    //be effecting. I will add a couple extra settings that I cam across while learning how to create the settings. I will comment
    //them out so if you want to use them just uncomment and if not no big deal. The only method I will leave open is the one for the
    //game volume.

    [SerializeField] public AudioMixer audioMixer; //component needed to change volume in-game
    [SerializeField] public Slider masterVolume; // component slider for master volume
    [SerializeField] public Slider musicVolume; // componenet slider for music volume
    [SerializeField] public Slider sfxVolume; // component slider for sfx volume
    [SerializeField] TMPro.TMP_Dropdown resolutionDropDown; //component needed to change resolution
    [SerializeField] Toggle fullscreenToggle; // toggle for adjusting to full screen - Adriana V
    [SerializeField] public AudioSource backgroundGameMusic; 

    [SerializeField] public GameObject[] OptionsEnableDisableTabs; // tab objects to enable and disable- AV
    [SerializeField] public Image[] SettingsTabButtons; // tab back image buttons to show which one is active or unactive - AV
    [SerializeField] public Sprite inactiveTabButtons; // tab buttons for which buttons are inactive when not clicked - AV
    [SerializeField] public Sprite activeTabsButton; // tab button for the button that is active when clicked on - AV
    [SerializeField] public Vector2 inactiveTabSize; // size for tabs when inactive(smaller than active tab - AV
    [SerializeField] public Vector2 activeTabSize; // size for tab when active (will be bigger than inactive tabs) - AV


    Resolution[] resolutions; //holds the different resolutions available for the player to choose from
    private float timeScaleOrig;

    void Start()
    {
        timeScaleOrig = Time.timeScale; //Holds the value of the current time scale

        resolutions = Screen.resolutions;

        //clear out the preset options made in the component to allow the array to be auto-populated
        resolutionDropDown.ClearOptions();

        //creates a list of strings of the different resolutions so that they can be listed in the array
        List<string> options = new List<string>();

        //tells the resolution dropdown menu to display the current resolution the computer is running the game at
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            //loops through the list of resolutions and adds them to the drop down list
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            //sets the currently displayed resolution in the drop down list as the current one being used to display game
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        //populates drop down list
        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentResolutionIndex;
        resolutionDropDown.RefreshShownValue();
        resolutionDropDown.onValueChanged.AddListener(SetResolution);

        // Initialize full screen toggle - Adriana V
        fullscreenToggle.isOn = Screen.fullScreen;
        fullscreenToggle.onValueChanged.AddListener(SetFullScreen);


    }

    // add Mathf.Max to ensure volume value is never zero or negative
    // added the minimum value of 0.0001f to prevent log issues when the slider is near 0
    public void SetMasterVolume()
    {
         float volumeLevel = Mathf.Log10(Mathf.Max(masterVolume.value, 0.0001f)) * 20;
         audioMixer.SetFloat("MasterVolume", volumeLevel);
    }

    public void SetMusicVolume()
    {
        float volumeLevel = Mathf.Log10(Mathf.Max(musicVolume.value, 0.0001f)) * 20;
        audioMixer.SetFloat("MusicVolume", volumeLevel);
    }

    public void SetSFXVolume()
    {
        float volumeLevel = Mathf.Log10(Mathf.Max(sfxVolume.value, 0.0001f)) * 20;
        audioMixer.SetFloat("SFXVolume", volumeLevel);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        //This setting will allow the player to choose whether they want to play the game in full screen or go into a window mode. Unity
        //auto goes into full screen mode so this gives players that extra option.

        //This code uses the Toggle component that can be created in the UI by right clicking in the hierarchy, going to UI and choosing
        //Toggle. The component already has the button set up to check the box on and off. All you need to do is go in and change the
        //text, change any images you want to match the game and then in the main toggle component there is already a section to add the
        //code to change the game between full screen or window. It's called On Value Changed (Boolean) and it works like when adding
        //the methods for the button EXCEPT you want to just the method name as it appears at the top of the list in the Dynamic bool
        //section.

        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        //This is to allow the player to change between different resolutions to display the game. This utilizes the above code
        //that fills in the resolutions[] array. In order to create this you will want to use a drop down list component. It is
        //set up the same way you add sliders and toggle components. There isn't anything you need to worry about changing in the menu
        //settings other than altering the images to match the game. The Options list that is in there doesn't need to be changed
        //because the code up in Start generates the list for you. Down in the section called On Value Changed (Int32) is where
        //you will add the script to then add then ability to change the resolution. Again you want to choose the method name
        //as it appears in the Dynamic Int list att he top of the menu.

        Resolution screenRes = resolutions[resolutionIndex];
        Screen.SetResolution(screenRes.width, screenRes.height, Screen.fullScreen);
    }

    public void SetGameSpeed(float speed)
    {
        //This will be set up similar to how the drop down menu is created for the resolution. This time however the menu will not be
        //populated automatically. I was thinking for now we could use o.5, 1, and 2 for the speeds. These will need to be set up in
        //the drop down menu options and then from there the method will be passed in same as the others with choosing the method
        //name inside the dynamic list at the top of the menu. With that set you should be able to use the drop down menu to
        //manipulate the game speed because it will take whatever you choose and multiply the game speed by the number chosen.

        Time.timeScale = timeScaleOrig * speed;
    }

    public void SaveAllSettings()
    {
        // save screen resolution
        PlayerPrefs.SetInt("Resolution", resolutionDropDown.value);

        // save fullscreen toggle
        PlayerPrefs.SetInt("FullScreen", fullscreenToggle.isOn ? 1 : 0);

        // save audio settings
        PlayerPrefs.SetFloat("MasterVolume", masterVolume.value);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume.value);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume.value);

        // save user changes
        PlayerPrefs.Save();
    }

    public void LoadAllSettings()
    {
        // load screen resolution settings
        if (PlayerPrefs.HasKey("Resolution"))
        {
            int resolutionIndex = PlayerPrefs.GetInt("Resolution");
            resolutionDropDown.value = resolutionIndex;
            resolutionDropDown.RefreshShownValue();
            SetResolution(resolutionIndex);
        }

        // load fullscreen toggle settings
        if (PlayerPrefs.HasKey("FullScreen"))
        {
            bool isFullScreen = PlayerPrefs.GetInt("FullScreen") == 1;
            fullscreenToggle.isOn = isFullScreen;
            SetFullScreen(isFullScreen);    
        }

        // load audio settings
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            float masterVol = PlayerPrefs.GetFloat("MasterVolume");
            masterVolume.value = masterVol;
            SetMasterVolume(); 
        }

        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float musicVol = PlayerPrefs.GetFloat("MusicVolume");
            musicVolume.value = musicVol;
            SetMusicVolume();
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            float sfxVol = PlayerPrefs.GetFloat("SFXVolume");
            sfxVolume.value = sfxVol;
            SetSFXVolume();
        }
    }

    // method to switch between active and inactive tabs in the settings submenus
    // iterates over the options menu tabs and settings tab menus arrays in order to set the active state and the sprite based on the tab's index number
    // not finished yet
    public void TabsSwitch(int TabIndex)
    {
        if (OptionsEnableDisableTabs.Length == 0 || SettingsTabButtons.Length == 0)
        {
            Debug.LogError("OptionsEnableDisableTabs or SettingsTabButtons arrays are not assigned or empty");
            return;
        }

        for (int i = 0; i < OptionsEnableDisableTabs.Length; i++)
        {
            OptionsEnableDisableTabs[i].SetActive(i == TabIndex);

            if (i == TabIndex)
            {
                // Active Tab
                SettingsTabButtons[i].sprite = activeTabsButton;
                SettingsTabButtons[i].rectTransform.sizeDelta = activeTabSize;

            }
            else
            {
                // Inactive Tabs
                SettingsTabButtons[i].sprite = inactiveTabButtons;
                SettingsTabButtons[i].rectTransform.sizeDelta = inactiveTabSize;
            }
            
        }
    }

}
