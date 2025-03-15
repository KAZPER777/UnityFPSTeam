using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour, IDamage
{
    [SerializeField] LayerMask ignoreLayer;
    
    //teleport
    public bool disabled = false;


    [SerializeField] CharacterController controller;

    [Range(1, 100)][SerializeField] int HP;
    [Range(2, 5)] [SerializeField] int speed;
    [Range(2, 4)] [SerializeField] int sprintMod;
    [Range(5, 20)] [SerializeField] int jumpSpeed;
    [Range(1, 3)] [SerializeField] int jumpsMax;
    [Range(15, 45)] [SerializeField] int gravity;

    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;
    [SerializeField] float flySpeed;

    public CharacterController PlayerHeight;
    public CapsuleCollider playerCol;
    public float normalHeight, crouchHeight;
    public Transform player;
    public Vector3 offset;

    int jumpCount;
    int HPOrig;

    bool isFlying = false;

    float shootTimer;

    Vector3 moveDir;
    Vector3 playerVel;
    //new
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;
        updatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {
        
        
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);

            Movement();

            sprint();
        
    }

    void Movement()
    {
        //teleport
        if (!disabled)
        {
            shootTimer += Time.deltaTime;

            if (controller.isGrounded)
            {
                jumpCount = 0;
                playerVel = Vector3.zero;
            }

            //moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            //transform.position += moveDir * speed * Time.deltaTime;\

            moveDir = (Input.GetAxis("Horizontal") * transform.right) +
                      (Input.GetAxis("Vertical") * transform.forward);
            controller.Move(moveDir * speed * Time.deltaTime);

            jump();

            controller.Move(playerVel * Time.deltaTime);
            playerVel.y -= gravity * Time.deltaTime;


            if (Input.GetButton("Fire1") && shootTimer >= shootRate)
                shoot();

            // Crouch
            if (Input.GetKeyDown(KeyCode.C))
            {
                PlayerHeight.height = crouchHeight;
                playerCol.height = crouchHeight;
            }
            if (Input.GetKeyUp(KeyCode.C))
            {
                PlayerHeight.height = normalHeight;
                playerCol.height = normalHeight;
                player.position = player.position + offset;
            }
        }
    }

    void jump() 
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpsMax) 
        {
            jumpCount++;
            playerVel.y = jumpSpeed;
        }
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;
        } 
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }

    void shoot()
    {
        shootTimer = 0;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist, ~ignoreLayer))
        {
            Debug.Log(hit.collider.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }
        }
    }

    public void takeDamage(int amount)
    {
        if (gamemanager.instance.godMode) return; // Ignoring damage if god mode is activated

        HP -= amount;
        updatePlayerUI();
        StartCoroutine(flashDamageScreen());

        if (HP <= 0)
        {
            gamemanager.instance.youLose();
        }
    }

    IEnumerator flashDamageScreen()
    {
        gamemanager.instance.playerDamageScreen.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gamemanager.instance.playerDamageScreen.SetActive(false);
    }

    public void updatePlayerUI()
    {
        gamemanager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
    }

    public void ToggleFly(bool enable)
    {
        isFlying = enable;

        if (enable)
        {
            playerVel = Vector3.zero;
        }

        Debug.Log(enable ? "Fly mode enabled!" : "Fly mode disabled!");
    }
}
