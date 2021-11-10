using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MovementController : MonoBehaviour
{

    [SerializeField]
    private float maximumSpeed = 3f;

    [SerializeField]
    private float lookSensitivity = 3f;
    [SerializeField]
    private float rotationSpeed = 0f;

    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private float jumpSpeed = 0f;


    private Vector3 rotation = Vector3.zero;  
    private CharacterController characterController;
    private Vector3 movementDirection = Vector3.zero;
    [SerializeField]
    private float jumpButtonGracePeriod = 0f;
    //player movement/rotation values
    private float _xMovement = 0f;
    private float _zMovement = 0f;
    private float _yRotation = 0f;
    private float inputMagnitude = 0f;
    private float ySpeed = 0f;
    private float originalStepOffset = 0f;
    private float? lastGroundedTime = 0f;
    private float? jumpButtonPressedTime = 0f;
    

    private void Start()
    {        
        Cursor.lockState = CursorLockMode.Locked;
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
        //rotationSpeed = 720f;
    }

    //runs per fps
    private void Update()
    {
        //Calculate movement veloc, ty as a 3d vector
        _xMovement = Input.GetAxis("Horizontal");
        _zMovement = Input.GetAxis("Vertical");        

        movementDirection = new Vector3(_xMovement,0,_zMovement);


        if (movementDirection != Vector3.zero)
        {
            GetComponent<Animator>().SetBool("isWalking", true);
            GetComponent<Animator>().SetBool("isAtacking", false);
            if (Input.GetButtonDown("Jump"))
            {
                GetComponent<Animator>().SetBool("isJumping", true);
            }
            else
            {
                GetComponent<Animator>().SetBool("isJumping", false);
            }
        }
        else
        {
            GetComponent<Animator>().SetBool("isWalking", false);
            GetComponent<Animator>().SetBool("isJumping", false);
        }

        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            inputMagnitude /= 2;
        }

        if (Input.GetButtonDown("Jump"))
        {
            GetComponent<Animator>().SetBool("isJumping", true);
            GetComponent<Animator>().SetBool("isAtacking", false);
            GetComponent<Animator>().SetBool("isWalking", false);
            jumpButtonPressedTime = Time.time;
        }        
    }

    //runs per physics iteration
    private void FixedUpdate()
    {       
        if (transform.GetComponent<PhotonView>().IsMine)
        {
            PlayerMovement();
        }
        
    }


    private void PlayerMovement()
    {
        
        inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

        float speed = inputMagnitude * maximumSpeed;
        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
        movementDirection.Normalize();
        
        
        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (characterController.isGrounded)
        {
            lastGroundedTime = Time.time;
        }

        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.5f;

            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                ySpeed = jumpSpeed;
                jumpButtonPressedTime = null;
                lastGroundedTime = null;
            }
        }
        else
        {
            characterController.stepOffset = 0;
        }



        Vector3 velocity = movementDirection * speed;
        velocity.y = ySpeed;
        characterController.Move(velocity * Time.deltaTime);        
        if(movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
               
    }         

    
}
