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
        // What happens when full screen is toggled off? Try making this explicit to unity?
        fullscreenToggle.isOn = Screen.fullScreen;
        fullscreenToggle.onValueChanged.AddListener(SetFullScreen);
    }

    //public void VolumeControl(float volumeLevel)
    //{
    //    //This setting uses the volume mixer I have added to the game. There is a parameter that is going to be called Volume which
    //    //the code is already looking for in order to set the volume. When you create the volume slider in the UI, you will want the
    //    //slider to go from a min of -80 to a max of 0. This is because that's how the master mixer does its sound value scaling.
    //    //When playing audio in-game it will always be at max level so just make sure the slider fill amount is already maxed out
    //    //by default.

    //    //There is already a section to add the code to change the game volume. It's called On Value Changed (Single) and it works
    //    //like when adding the methods for the button EXCEPT you want to choose the method name as it appears at the top of the
    //    //list in the Dynamic float section.

  
    //}

    public void SetMasterVolume()
    {
         float volumeLevel = masterVolume.value;
         audioMixer.SetFloat("MasterVolume", volumeLevel);
    }

    public void SetMusicVolume()
    {
        float volumeLevel = musicVolume.value;
        audioMixer.SetFloat("MusicVolume", volumeLevel);
    }

    public void SetSFXVolume()
    {
        float volumeLevel = sfxVolume.value;
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
}
