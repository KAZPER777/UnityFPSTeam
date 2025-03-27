using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;
using System.Collections;

public class damage : MonoBehaviour
{
    enum damageType { moving, stationary, overtime, homing }
    [SerializeField] damageType type;
    [SerializeField] Rigidbody rb;

    [Range(1, 10)] [SerializeField] int damageAmount;
    [Range(.25f, 1)] [SerializeField] float damageTime;
    [Range(10, 45)] [SerializeField] int speed;
    [Range(1, 10)] [SerializeField] int destroyTime;


    bool isDamaging;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(type == damageType.moving || type == damageType.homing)
        {
            
            Destroy(gameObject, destroyTime);

            if(type == damageType.moving)
                rb.linearVelocity = transform.forward * speed;
        }


    }

    private void Update()
    {
        if(type == damageType.homing)
        {
            rb.linearVelocity =  (gameManager.instance.player.transform.position - transform.position).normalized * (speed * 100 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        IDamage dmg = other.GetComponent<IDamage>();

        if(dmg != null && (type == damageType.stationary || type == damageType.moving || type == damageType.homing))
        {
            dmg.takeDamage(damageAmount);
        }

        if(type == damageType.moving || type == damageType.homing)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.isTrigger)
            return;

        IDamage dmg = other.GetComponent<IDamage>();

        if(dmg != null && type == damageType.overtime)
        {
            if(!isDamaging)
                StartCoroutine(damageOther(dmg));
        }
    }

    IEnumerator damageOther(IDamage d)
    {
        isDamaging = true;
        d.takeDamage(damageAmount);
        yield return new WaitForSeconds(damageTime);

        isDamaging = false;

    }

}
