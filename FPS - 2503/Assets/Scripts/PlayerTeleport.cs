using System.Collections;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    playerController playercontrol;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playercontrol = gameObject.GetComponent<playerController>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("T key pressed!"); 
            StartCoroutine(Teleport());
        }
    }

    //delay
    IEnumerator Teleport()
    {

        playercontrol.disabled = true;
        yield return new WaitForSeconds(0.1f);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            gameObject.transform.position = hit.point; 
        }

        yield return new WaitForSeconds(0.1f);
        playercontrol.disabled = false;



        //playercontrol.disabled = true;
       // yield return new WaitForSeconds(0.1f);
        //gameObject.transform.position = new Vector3(25f, 0f, 0f);
       // yield return new WaitForSeconds(0.1f);
       // playercontrol.disabled = false;
    }
}
