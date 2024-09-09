using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] int sensetivity;
    [SerializeField] int verrticalMin;
    [SerializeField] int verrticalMax;
    [SerializeField] bool InvertCameraControls;

    float xRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // get user input 
        float yMovement = Input.GetAxis("Mouse Y") * sensetivity * Time.deltaTime;
        float xMovement = Input.GetAxis("Mouse X") * sensetivity * Time.deltaTime;
        
        // invert the camrea controls
        if(!InvertCameraControls)
        {
            xRotation -= yMovement;
        }
        else
        {
            xRotation += yMovement;
        }

        // clamp the camera
        xRotation = Mathf.Clamp(xRotation, verrticalMin, verrticalMax);

        // rotate the camera on the x-axis 
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        //rotate the camera on y-axis 
        transform.parent.Rotate(Vector3.up * xMovement);
    }
}
