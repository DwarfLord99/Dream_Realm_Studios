using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] CharacterController Controller;

    //player stats
    [SerializeField] int HP;
    [SerializeField] int playerSpeed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;

    // number of jumps the player has preformed
    int numberOfJumps;

    // bool that turns sprinting on or off based on player input 
    bool playerSprinting;
    //player combat fields 
    //range 
    // damage
    // distance

    // vectors for movemnt dircetion and player velocity 
    Vector3 MovementDirection;
    Vector3 PlayerVelocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        sprint();
    }

    void Movement()
    {
        // resests the jump count once player hits the ground
        // if this code is placed anywhere else insdie the function jump count will not reset
        if (Controller.isGrounded)
        {
            PlayerVelocity = Vector3.zero;
            numberOfJumps = 0;
        }

        // basic movemnt
        // 
        MovementDirection = Input.GetAxis("Horizontal") * transform.right +
                            Input.GetAxis("Vertical") * transform.forward;
        Controller.Move(MovementDirection * playerSpeed * Time.deltaTime);
        
        // jump 
        if(Input.GetButtonDown("Jump") && numberOfJumps < jumpMax)
        {
            numberOfJumps++;
            PlayerVelocity.y = jumpSpeed;
        }
        
        Controller.Move(PlayerVelocity * Time.deltaTime);
        // will make the plaeyr fall based on the gravity settings and the delta time
        PlayerVelocity.y -= gravity * Time.deltaTime; 
    }

    //sprint

    void sprint()
    {
        // if assined button is pressed down then speed will increase based on the sprint mod
        if(Input.GetButtonDown("Sprint"))
        {
            // increase plkayer speed 
            playerSpeed *= sprintMod;
            // update sprinting bool
            playerSprinting = true;
        }
        // once the button is released thne the player will return to noraml speed
        else if(Input.GetButtonUp("Sprint"))
        {
            // decrease player speed 
            playerSpeed /= sprintMod;
            // update sprinting bool
            playerSprinting = false; 
        }
    }

    // to be added later

    // shoot
    // player taking damage
    // player sceen flashing when damage is done
}
