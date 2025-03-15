using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class enemyAI : MonoBehaviour, IDamage
{
    public enum EnemyType { Soldier, Drone, Boss }
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

    [Header("Boss Settings")]
    [SerializeField] bool isBoss;
    [SerializeField] GameObject bossRoomDoor;

    float shootTimer;
    Vector3 playerDir;
    bool playerInRange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gamemanager.instance.updateGameGoal(1);

        if (enemyType == EnemyType.Drone)
        {
            gamemanager.instance.RegisterEnemy("Easy");
        } else if (enemyType == EnemyType.Soldier)
        {
            gamemanager.instance.RegisterEnemy("Medium");
        }
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
                shoot();
            }

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                faceTarget();
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
        {
            Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, transform.position.y, playerDir.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, faceTargetSpeed * Time.deltaTime);
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

            if (enemyType == EnemyType.Drone)
            {
                gamemanager.instance.EnemyDefeated("Easy");
            } else if (enemyType == EnemyType.Soldier)
            {
                gamemanager.instance.EnemyDefeated("Medium");
            } else if (enemyType == EnemyType.Boss)
            {
                gamemanager.instance.BossDefeated();
            }

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
