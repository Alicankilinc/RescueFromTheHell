using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    Animator playeranim;
    public CapsuleCollider maincc;
    private CharacterController controller;
    public float speed = 12f;
    public float crouchedSpeed;
    public float gravity = -9.81f * 2;
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    bool isGrounded;
    public bool isMoving;
    private Vector3 lastPosition = new Vector3(0f, 0f, 0f);
    // Start is called before the first frame update
    void Start()
    {
      
       
        crouchedSpeed = speed / 3;
        controller = GetComponent<CharacterController>();
        playeranim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        //Crouching
        if (Input.GetKey(KeyCode.LeftControl) && isGrounded)
        {
            Crouch();
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            StandUp();
        }
        if (isMoving==false)
        {
            SoundManager.Instance.footStep.Play();
        }
        //Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        //Ressetting the default velocity
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        //Getting the inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        //Creating the moving vector
        Vector3 move = transform.right * x + transform.forward * z; //(right-red axis...) 
        //Actually moving the player
        controller.Move(move * speed * Time.deltaTime);
        //Check if the player can jump
        if (Input.GetButton("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        //Falling down
        velocity.y += gravity * Time.deltaTime;
        //Exectuting the jump
        controller.Move(velocity * Time.deltaTime);
        if (lastPosition != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;

        }
        lastPosition = gameObject.transform.position;
    }

    private void Crouch()
    {
        speed = crouchedSpeed;
        controller.height = controller.height / 2;
        maincc.height = 0.5f;
        transform.position = transform.position - new Vector3(0, 1.81f, 0);
        
    }
    private void StandUp()
    {
        speed = crouchedSpeed * 3;
        maincc.height = 2;
        controller.height = 3.7f;
        transform.position = transform.position + new Vector3(0, 1.81f, 0);
        
    }
   
  
}
