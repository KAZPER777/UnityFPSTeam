using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] Transform headPos;

    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int animTranSpeed;
    [SerializeField] int FOV;
    [SerializeField] int roamPauseTime;
    [SerializeField] int roamDist;

    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    [SerializeField] Collider weaponCol;

    float shootTimer;
    float roamTimer;
    float angleToPlayer;
    float stoppingDist;

    Vector3 playerDir;
    Vector3 startingPos;

    bool playerInRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //gameManager.instance.updateGameGoal(1);
        startingPos = transform.position;
        stoppingDist = agent.stoppingDistance;

    }

    // Update is called once per frame
    void Update()
    {
        SetAnimLocomotion();

        shootTimer += Time.deltaTime;

        if(agent.remainingDistance < 0.01f )
            roamTimer += Time.deltaTime;

        if (playerInRange && !canSeePlayer())
        {
            roam();
        }
        else if (!playerInRange)
        {
            checkRoam();
        }
    }

    bool canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);


        Debug.DrawRay(headPos.position, playerDir);


        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if(hit.collider.CompareTag("Player") && angleToPlayer <= FOV)
            {
               agent.SetDestination(gameManager.instance.player.transform.position);
               if (shootTimer >= shootRate)
               {
                    shoot();
               }

               if (agent.remainingDistance <= agent.stoppingDistance)
               {
                    faceTarget();
               }

                agent.stoppingDistance = stoppingDist;
                return true;
            }   
        }
        agent.stoppingDistance = 0;
        return false; 
    }

    void checkRoam()
    {
        if((roamTimer > roamPauseTime && agent.remainingDistance < 0.01f) || gameManager.instance.playerScript.HP <= 0)
        {
            roam();
        }
    }

    void roam()
    {
        roamTimer = 0;
        agent.stoppingDistance = 0;

        Vector3 ranPos = Random.insideUnitSphere * roamDist;
        ranPos += startingPos;

        NavMeshHit hit;
        NavMesh.SamplePosition(ranPos, out hit, roamDist, 1);
        agent.SetDestination(hit.position);
    }


    void SetAnimLocomotion()
    {
        float agentSpeed = agent.velocity.normalized.magnitude;
        float animSpeedCurrent = anim.GetFloat("Speed");
        anim.SetFloat("Speed", Mathf.Lerp(animSpeedCurrent, agentSpeed, Time.deltaTime * animTranSpeed));
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            agent.stoppingDistance = 0;
        }
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
    }

    // This makes sure the enemy takes damage
    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashRed());
        roamTimer = 0;

        agent.SetDestination(gameManager.instance.player.transform.position);

        if(HP <= 0)
        {
            gameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    // This makes the enemy flash red when they take damage.
    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

    void shoot()
    {
        shootTimer = 0;
        anim.SetTrigger("Shoot");
        
    }

    public void createBullet()
    {
        Instantiate(bullet, shootPos.position, transform.rotation);
    }


    public void turnWeaponColOn()
    {
        weaponCol.enabled = true;
    }

    public void turnWeaponColOff()
    {
        weaponCol.enabled = false;
    }

}
