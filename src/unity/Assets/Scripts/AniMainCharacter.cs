﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniMainCharacter : MonoBehaviour
{
    public GameObject sphere;
    private Animator animations;
    bool ground;
    Rigidbody sRigidBody;

    // Start is called before the first frame update
    void Start()
    {
        sRigidBody = sphere.GetComponent<Rigidbody>();
        animations = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Transform parent = sphere.transform.GetComponentInParent<Transform>();
        Quaternion targetRotation = Quaternion.Euler(
             sRigidBody.velocity.y.Remap(-8, 6, 100, -60),
              parent.eulerAngles.y + Input.GetAxis("Horizontal") * 20,
             parent.eulerAngles.z + Input.GetAxis("Horizontal") * -20
         );
        transform.parent.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);

        //print("hello" + DistanceToGround());
        float distance = DistanceToGround();
        if (distance < 30 & distance != 0)
        {
            if (!Input.GetButton("Jump"))
            {
                animations.SetBool("walk", true);
            }
            if (Input.GetAxis("Vertical") != 0 | Input.GetAxis("Horizontal") != 0)
                animations.SetBool("run", true);
            else
                animations.SetBool("run", false);

        }
        else
        {
            animations.SetBool("walk", false);
            if (Input.GetButton("Jump"))
            {
                animations.speed = 1.2f;
                if (Input.GetAxis("Vertical") != 0)
                    animations.SetTrigger("forowrdUP");
                else
                    animations.SetTrigger("takeOff");
            }
            else if (sRigidBody.velocity.y < 0)
            {
                animations.speed = 1f;

                if (Input.GetAxis("Vertical") != 0)
                    animations.SetTrigger("forowrdUP");
                else
                    animations.SetTrigger("fall");
            }
        }
    }


    int DistanceToGround()
    {
        RaycastHit hit;
        Ray ray = new Ray(this.transform.position, transform.up * -1);
        if (Physics.Raycast(ray, out hit))
        {
            if(hit.collider.tag=="ground")
            return (int)(hit.distance*100);
            else return 0;
        }
        else return 0;
    }
}