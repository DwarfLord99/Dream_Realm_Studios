using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCamera : MonoBehaviour
{
    //    //I am adding in the settings needed to allow the player to control the sensitivity of the camera. Each section I add will have -RL
    //    //commented after it.

    //    //IMPORTANT - In order to ensure that the camera speed works properly since we will be moving over to default settings in Unity and
    //    //not our own values, go into the Project Settings and under both the Mouse X and Mouse Y in the Input Manager, set their sensitivity
    //    //values both to 1 if they are not. This will allow the slider to properly update the sensitivity
    
    // [SerializeField] float normalFOV = 60f;
    // [SerializeField] float adsFOV = 40f;
    // [SerializeField] float fovTransitionSpeed = 5f;

    [SerializeField] float sensitivity; //-RL Changed to float so that the sensitivity can be update through PlayerPrefs
    [SerializeField] int verticalMin;
    [SerializeField] int verticalMax;
    [SerializeField] bool InvertCameraControls;

    [SerializeField] public Slider senSlider; //-RL
    [SerializeField] public Slider gameSpeedSlider; // slider component to adjust game speed - AV

    float xRotation;

    // Start is called before the first frame update
    void Start()
    {
        //        //These settings get the mouse sensitivity values that Unity naturally uses for the mouse so the player can adjust them.
        //        //Since Unity will preset this value for us we can turn the variable into private or leave serialize so you can see
        //        //the number change in script.

        //        //Using PlayerPrefs, this will allow the game to remember the player's current mouse sensitivity settings when they
        //        //close and reopen the game without having to reset them every time the play
            //sensitivity = PlayerPrefs.GetFloat("currentSensitivity", 100); //-RL
            //senSlider.value = sensitivity; //-RL

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        //gameSpeedSlider.onValueChanged.AddListener(AdjustCameraSpeed);


        //startPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetFloat("currentSensitivity", sensitivity); //-RL

        // get user input 
        float yMovement = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        float xMovement = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        // invert the camrea controls
        if (!InvertCameraControls)
        {
            xRotation -= yMovement;
        }
        else
        {
            xRotation += yMovement;
        }

        // clamp the camera
        xRotation = Mathf.Clamp(xRotation, verticalMin, verticalMax);

        // rotate the camera on the x-axis 
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        //rotate the camera on y-axis 
        transform.parent.Rotate(Vector3.up * xMovement);

        // Adjust camera FOV for ADS added by Fuad H.
       // float targetFOV = PlayerMovement.instance.isAiming ? adsFOV : normalFOV;
       // Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFOV, fovTransitionSpeed * Time.deltaTime);
    }

    public void AdjustCameraSpeed(float mouseSpeed)
    {
        //This method allows the camera's speed to be adjusted based on the value of the slider used to set the sensitivity.
        //When setting the method in the slider, add it to the On Value Changed (Single) at the bottom of the slider component and
        //make sure to choose the method name in the Dynamic float section at the top of the list. This is done so that we do not have
        //to hardcode in the mouse speed, it will use whatever number the slider is set to between 0 and 1. The number is multiplied by 10
        //because when the default sensitivity value is set in the script, it defaults to 10.

        sensitivity = mouseSpeed * 10;
    }
}
