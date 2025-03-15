using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headbob : MonoBehaviour
{
    public Animator camAnim;
    public bool walking;

    void Update()
    {
        bool isWalking = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        bool isSprinting = isWalking && Input.GetKey(KeyCode.LeftShift);

        if (isSprinting)
        {
            camAnim.ResetTrigger("Walk");
            camAnim.ResetTrigger("Idle");
            camAnim.SetTrigger("Sprint");
        }
        else if (isWalking)
        {
            camAnim.ResetTrigger("Sprint");
            camAnim.ResetTrigger("Idle");
            camAnim.SetTrigger("Walk");
        }
        else
        {
            camAnim.ResetTrigger("Walk");
            camAnim.ResetTrigger("Sprint");
            camAnim.SetTrigger("Idle");
        }
    }
}