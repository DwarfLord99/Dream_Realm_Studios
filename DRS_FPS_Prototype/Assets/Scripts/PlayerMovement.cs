using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using System.Linq;


public class PlayerMovement : MonoBehaviour, IDamage
{

    [Header("Copponets")]

    [SerializeField] CharacterController Controller;
    [SerializeField] AudioSource Audio;
    [SerializeField] LayerMask ignoreMask;

    [Header("player stats")]

    //player stats
    [Range(1, 20)][SerializeField] int HP;
    [Range(2, 9)][SerializeField] int playerSpeed;
    [Range(2, 5)][SerializeField] int sprintMod;
    [Range(1, 20)][SerializeField] int jumpSpeed;
    [Range(1, 2)][SerializeField] int jumpMax;
    [Range(10, 45)][SerializeField] int gravity;

    [Header("invetory/guns ")]

    [SerializeField] List<GameObject> Inventory;
    [SerializeField] GameObject weaponModel;
    [SerializeField] GameObject MuzzleFlash;
    [SerializeField] List<WeaponStats> WeaponList = new List<WeaponStats>();
    //range 
    [Header("gun stats")]

    [SerializeField] int effectiveRange;
    // damage
    [SerializeField] int ProjectileDamage;
    // rate at which the player will fire a projectile
    [SerializeField] float RateOfFire;

    [Header("Audio")]

    [SerializeField] AudioClip[] WalkingAudio;
   [Range(0,1)] [SerializeField] float WalkingVolume;

    [SerializeField] AudioClip[] JumpingAudio;
    [Range(0, 1)][SerializeField] float JumpingVolume;

    [SerializeField] AudioClip[] DamageAudio;
    [Range(0, 1)][SerializeField] float DamageVolume;

    // vectors for movement direction and player velocity 
    Vector3 MovementDirection;
    Vector3 PlayerVelocity;

    // number of jumps the player has preformed
    int numberOfJumps;

    // player starting default HP at the start of the game
    int DefaultHP;

    int CurrentWeaponPOS;
    // bool that turns sprinting on or off based on player input 
    bool playerSprinting;

    // bool to see if the player is shooting a projectile
    bool PlayerShooting;

    bool PlayerWalking;


    // Start is called before the first frame update
    void Start()
    {
        DefaultHP = HP;
        UpdatePlayerUI();
        PlayerSpawn();
    }

    // spawns the player at a point in the map
    public void PlayerSpawn()
    {
        // turns off the contoler so the player object can move 
        Controller.enabled = false;
        // moves the player object 
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        // turns the controler back on
        Controller.enabled = true;
        HP = DefaultHP;
        UpdatePlayerUI(); // to fix the HP bar bug - Adriana V
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * effectiveRange, Color.red);
        
        if (gameManager.instance != null && !gameManager.instance.isPaused) // added a null check to prevent the null reference error - Adriana V
        {
            Movement();
            WeaponSelect();
           
        }

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
        if (Input.GetButtonDown("Jump") && numberOfJumps < jumpMax)
        {
            numberOfJumps++;
            PlayerVelocity.y = jumpSpeed;
            Audio.PlayOneShot(JumpingAudio[Random.Range(0, JumpingAudio.Length)], JumpingVolume);
        }

        // relaods the plaeyrs gun
        if(Input.GetButtonDown("Reload") && WeaponList[CurrentWeaponPOS].CurrentAmmo < WeaponList[CurrentWeaponPOS].MaxAmmo)
        {
            WeaponList[CurrentWeaponPOS].CurrentAmmo = WeaponList[CurrentWeaponPOS].MaxAmmo;
            UpdatePlayerUI();
        }

        // has the player move up the screen based on the jump velocity and the time pre frame
        // this prevents the player form jumping in a riged way
        Controller.Move(PlayerVelocity * Time.deltaTime);
        // will make the plaeyr fall based on the gravity settings and the delta time
        PlayerVelocity.y -= gravity * Time.deltaTime;

        // if shoot button is pressed and the player is not already shooting 
        // then call the shooting function
        if (Input.GetButton("Fire1") && WeaponList.Count > 0 && WeaponList[CurrentWeaponPOS].CurrentAmmo > 0 && !PlayerShooting)
        {
            StartCoroutine(Shooting());
        }

        if (Controller.isGrounded && MovementDirection.magnitude > 0.3f && !PlayerWalking)
        {
            StartCoroutine(PlayerSteps());
        }
    }

    IEnumerator PlayerSteps()
    {
        PlayerWalking = true;

        Audio.PlayOneShot(WalkingAudio[Random.Range(0, WalkingAudio.Length)], WalkingVolume);

        if (!playerSprinting)
        {
            yield return new WaitForSeconds(0.5f);
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
        }

        PlayerWalking = false;
    }

    //sprint

    void sprint()
    {
        // if assined button is pressed down then speed will increase based on the sprint mod
        if (Input.GetButtonDown("Sprint"))
        {
            // increase plkayer speed 
            playerSpeed *= sprintMod;
            // update sprinting bool
            playerSprinting = true;
        }
        // once the button is released thne the player will return to noraml speed
        else if (Input.GetButtonUp("Sprint"))
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
        WeaponList[CurrentWeaponPOS].CurrentAmmo--;
        UpdatePlayerUI();
        StartCoroutine(FlashMuzzle());

        // since there is no gun audio at the moment this line is commented out to prvent issues
       // Audio.PlayOneShot(WeaponList[CurrentWeaponPOS].GunSound[Random.Range(0, WeaponList[CurrentWeaponPOS].GunSound.Length)], WeaponList[CurrentWeaponPOS].GunVolume);

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, effectiveRange, ~ignoreMask))
        {
            // see if the itsem is part of IDamage
            IDamage damage = hit.collider.GetComponent<IDamage>();

            // a debug line to give the name of what the raycast hits
            //Debug.Log(hit.collider.transform.name);

            if (WeaponList[CurrentWeaponPOS] != null)
            {
                Instantiate(WeaponList[CurrentWeaponPOS].HitEffect, hit.point, Quaternion.identity);
            }

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
        // no gun aduio so removed line to prvent errors
        //Audio.PlayOneShot(DamageAudio[Random.Range(0, DamageAudio.Length)], DamageVolume);
        UpdatePlayerUI();
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

    IEnumerator FlashMuzzle()
    {
        MuzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        MuzzleFlash.SetActive(false);
    }

    public void UpdatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / DefaultHP;

        if(WeaponList.Count > 0)
        {
            gameManager.instance.ammoCur.text = WeaponList[CurrentWeaponPOS].CurrentAmmo.ToString("F0");
            gameManager.instance.ammoMax.text = WeaponList[CurrentWeaponPOS].MaxAmmo.ToString("F0");

        }
    }

    public void GetWeaponStats(WeaponStats weapon)
    {
        WeaponList.Add(weapon);
        CurrentWeaponPOS = WeaponList.Count - 1;
        UpdatePlayerUI();

        ProjectileDamage = weapon.Damage;
        effectiveRange = weapon.EffectiveRange;
        RateOfFire = weapon.RateOfFire;

        weaponModel.GetComponent<MeshFilter>().sharedMesh = weapon.Model.GetComponent<MeshFilter>().sharedMesh;
        weaponModel.GetComponent<MeshRenderer>().sharedMaterial = weapon.Model.GetComponent<MeshRenderer>().sharedMaterial;
    }

    public void AddToInvetory(GameObject Item)
    {
        Inventory.Add(Item);
    }
        
    void WeaponSelect()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0 && CurrentWeaponPOS < WeaponList.Count -1) 
        {
            CurrentWeaponPOS++;
            ChangeWeapon();
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && CurrentWeaponPOS > 0)
        {
            CurrentWeaponPOS--;
            ChangeWeapon();
        }
    }

    void ChangeWeapon()
    {
        UpdatePlayerUI();
        ProjectileDamage = WeaponList[CurrentWeaponPOS].Damage;
        effectiveRange = WeaponList[CurrentWeaponPOS].EffectiveRange;
        RateOfFire = WeaponList[CurrentWeaponPOS].RateOfFire;

        weaponModel.GetComponent<MeshFilter>().sharedMesh = WeaponList[CurrentWeaponPOS].Model.GetComponent<MeshFilter>().sharedMesh;
        weaponModel.GetComponent<MeshRenderer>().sharedMaterial = WeaponList[CurrentWeaponPOS].Model.GetComponent<MeshRenderer>().sharedMaterial;

    }
}
