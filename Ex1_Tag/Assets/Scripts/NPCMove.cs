using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

public class NPCMove : MonoBehaviour
{
    public Transform goal;
    private float speed = 0.001f;
    private float rotationSpeed = 0.03f;
    private float distance = 0.5f;
    private bool stayIdle = true;
    private bool isChaser = true;
    private bool canBeCaught = false;

    private void Start()
    {
        Invoke("StartChasing", 2);
    }

    void Update()
    {
        if (stayIdle)
            return;
        
        if (!GetComponent<Animator>().GetBool("Is Walking") && !GetComponent<Animator>().GetBool("Is Running"))
        {
            GetComponent<Animator>().SetBool("Is Walking", true);
            speed = 0.001f;
            Invoke("StartRunning", 2);
        }

        if (isChaser)
        {
            Chase();
        }
        else
        {
            RunAway();
        }
    }

    void Chase()
    {
        Vector3 realGoal = new Vector3(goal.position.x, 
            transform.position.y, goal.position.z);
        Vector3 direction = realGoal - transform.position;

        transform.rotation = Quaternion.Slerp(transform.rotation, 
            Quaternion.LookRotation(direction), rotationSpeed);
        
        if (direction.magnitude >= distance)
        {
            Vector3 pushVector = direction.normalized * speed;
            transform.Translate(pushVector, Space.World);
        }
        else // caught the player
        {
            GetComponent<Animator>().SetBool("Is Walking", false);
            GetComponent<Animator>().SetBool("Is Running", false);
            isChaser = false;
            canBeCaught = false;
            Invoke("CanBeCaught", 3);
        }
    }

    void RunAway()
    {
        Vector3 realGoal = new Vector3(goal.position.x, 
            transform.position.y, goal.position.z);
        Vector3 direction = transform.position - realGoal;

        transform.rotation = Quaternion.Slerp(transform.rotation, 
            Quaternion.LookRotation(direction), rotationSpeed);
        
        Vector3 pushVector = direction.normalized * speed;
        transform.Translate(pushVector, Space.World);
        
        if (canBeCaught && direction.magnitude <= distance)
        {
            GetComponent<Animator>().SetBool("Is Walking", false);
            GetComponent<Animator>().SetBool("Is Running", false);
            stayIdle = true;
            isChaser = true;
            Invoke("StartChasing", 2);
        }
    }

    void StartChasing()
    {
        stayIdle = false;
    }

    void StartRunning()
    {
        GetComponent<Animator>().SetBool("Is Running", true);
        GetComponent<Animator>().SetBool("Is Walking", false);
        speed = 0.004f;
    }

    void CanBeCaught()
    {
        canBeCaught = true;
    }
}

