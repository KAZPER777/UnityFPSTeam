using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headbob : MonoBehaviour
{
    public Animator camAnim;
    public bool walking;

    void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            camAnim.ResetTrigger("Idle");
            walking = true;
            camAnim.ResetTrigger("Sprint");
            camAnim.SetTrigger("Walk");
            if (walking)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    camAnim.ResetTrigger("Walk");
                    camAnim.ResetTrigger("Idle");
                    camAnim.SetTrigger("Sprint");
                }
            }
        }
        else
        {
            camAnim.ResetTrigger("Sprint");
            walking = false;
            camAnim.ResetTrigger("Walk");
            camAnim.SetTrigger("Idle");
        }
    }
}