using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class enemyAI : MonoBehaviour, IDamage
{
    public enum EnemyType { Soldier, Drone }
    public EnemyType enemyType;

    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    

    [SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int animTranSpeed;

    [SerializeField] Transform shootPos;
    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    float shootTimer;

    Vector3 playerDir;

    bool playerInRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gamemanager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        setAnimLocomotion();

        shootTimer += Time.deltaTime;

        if (playerInRange)
        {
            playerDir = gamemanager.instance.player.transform.position - transform.position;
            agent.SetDestination(gamemanager.instance.player.transform.position);

            if (shootTimer >= shootRate)
            {
                
            }

            if (agent.remainingDistance <= agent.stoppingDistance && shootTimer >= shootRate)
            {
                faceTarget();
                shoot();
            }
        }
    }

    void setAnimLocomotion()
    {
        float agentSpeed = agent.velocity.normalized.magnitude;
        float animSpeedCur = anim.GetFloat("Speed");
        anim.SetFloat("Speed", Mathf.Lerp(animSpeedCur, agentSpeed, Time.deltaTime * animTranSpeed));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    void faceTarget()
    {
        if (enemyType == EnemyType.Soldier)
        {
            Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, faceTargetSpeed);
        }
        else if (enemyType == EnemyType.Drone)
        {
            Vector3 targetPosition = gamemanager.instance.player.transform.position;
            Vector3 lookDirection = (targetPosition - transform.position).normalized;

            Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, playerDir.y, playerDir.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, faceTargetSpeed);
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashRed());

        agent.SetDestination(gamemanager.instance.player.transform.position);

        if (HP <= 0)
        {
            gamemanager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = Color.white;
    }

    void shoot()
    {
        shootTimer = 0;
        Instantiate(bullet, shootPos.position, transform.rotation);
    }
}
