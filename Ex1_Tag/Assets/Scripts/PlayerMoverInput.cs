using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoverInput : MonoBehaviour
{
    CharacterController character;

    private Vector3 moveVector;

    private float speed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (moveVector.Equals(Vector3.zero))
        {
            GetComponent<Animator>().SetBool("Is Walking", false);
            GetComponent<Animator>().SetBool("Is Running", false);
        }
        else if (!GetComponent<Animator>().GetBool("Is Walking") && !GetComponent<Animator>().GetBool("Is Running"))
        {
            GetComponent<Animator>().SetBool("Is Walking", true);
            speed = 1f;
            Invoke("StartRunning", 1);
        }
        
        if (moveVector.z != 0)
        {
            character.Move(transform.forward * speed * Time.fixedDeltaTime * Math.Sign(moveVector.z));
        }
        
        if (moveVector.x != 0)
        {
            Quaternion rotation = Quaternion.Euler(0, transform.eulerAngles.y + 90 * Math.Sign(moveVector.x), 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.03f);
        }
    }

    public void OnMovementChanged(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        moveVector = new Vector3(direction.x, 0, direction.y);
        moveVector.Normalize();
    }

    void StartRunning()
    {
        if (!GetComponent<Animator>().GetBool("Is Walking"))
        {
            return;
        }
        GetComponent<Animator>().SetBool("Is Running", true);
        GetComponent<Animator>().SetBool("Is Walking", false);
        speed = 3f;
    }
}
