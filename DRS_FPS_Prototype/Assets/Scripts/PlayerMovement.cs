using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour, IDamage
{

    [SerializeField] CharacterController Controller;
    [SerializeField] LayerMask ignoreMask;

    //player stats
    [SerializeField] int HP;
    [SerializeField] int playerSpeed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpSpeed;
    [SerializeField] int jumpMax;
    [SerializeField] int gravity;

    //range 
    [SerializeField] int effectiveRange;
    // damage
    [SerializeField] int ProjectileDamage;
    // rate at which the player will fire a projectile
    [SerializeField] float RateOfFire;


    // vectors for movement direction and player velocity 
    Vector3 MovementDirection;
    Vector3 PlayerVelocity;

    // number of jumps the player has preformed
    int numberOfJumps;

    // player starting defualt HP at the start of the game
    int DefaultHP;
    // bool that turns sprinting on or off based on player input 
    bool playerSprinting;

    // bool to see if the player is shooting a projectile
    bool PlayerShooting; 

    // Start is called before the first frame update
    void Start()
    {
        DefaultHP = HP;
        UpdatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * effectiveRange, Color.red);
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
        
        // has the player move up the screen based on the jump velocity and the time pre frame
        // this prevents the player form jumping in a riged way
        Controller.Move(PlayerVelocity * Time.deltaTime);
        // will make the plaeyr fall based on the gravity settings and the delta time
        PlayerVelocity.y -= gravity * Time.deltaTime;

        // if shoot button is pressed and the player is not already shooting 
        // then call the shooting function
        if (Input.GetButton("Fire1") && !PlayerShooting)
        {
            StartCoroutine(Shooting());
        }
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

    // shoot
    IEnumerator Shooting()
    {
        // sets bool to true 
        PlayerShooting = true;


        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, effectiveRange, ~ignoreMask))
        {
            // see if the itsem is part of IDamage
            IDamage damage = hit.collider.GetComponent<IDamage>();
            // a debug line to give the name of what the raycast hits
            Debug.Log(hit.collider.transform.name);

            // casues a set amount of damage based on what is set in the field
            if (damage != null)
            {
                // atempt to update later if player is givin a model
                damage.takeDamage(ProjectileDamage);
            }
        }

        // tells the function to wait for a set amout of time in this case the rate of fire field 
        yield return new WaitForSeconds(RateOfFire);
        // sets bool to false once the function has completed or the player has stopped shooting
        PlayerShooting = false;
    }

    public void takeDamage(int damage)
    {
        // reduce HP based on damage taken
        HP -= damage;

        // updates the player UI 
        UpdatePlayerUI();

        // calls the coroutine to have the players screen to falsh when damage is taken
        StartCoroutine(PlayerTakesDamage());

        // when players HP hits zero
        if (HP <= 0)
        {
            //the player loeses
            gameManager.instance.youlose();
        }
    }

    //function to make the screen flash when damage is taken
    IEnumerator PlayerTakesDamage()
    {
        // sets the damage panel to be active and display on the the screen
        gameManager.instance.damagePanel.SetActive(true);
        // have the enumerator yield for a set time 
        yield return new WaitForSeconds(0.1f);
        // sets the damage panel to be inactive and hide it from the player
        gameManager.instance.damagePanel.SetActive(false);
    }

    public void UpdatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / DefaultHP;
    }
}
