using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class MovementController : MonoBehaviour
{

    [SerializeField]
    private float speed = 3f;

    [SerializeField]
    private float lookSensitivity = 3f;

    [SerializeField]
    public GameObject fpsCamera;
   
    private Vector3 rotation = Vector3.zero;  
    private float cameraUpDownRotation = 0f;
    private float currentCameraUpDownRotation = 0f;   

    private Rigidbody rb;


    //player movement/rotation values
    private float _xMovement = 0f;
    private float _zMovement = 0f;
    private float _yRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }

    //runs per fps
    private void Update()
    {
        //Calculate movement veloc, ty as a 3d vector
        _xMovement = Input.GetAxis("Horizontal");
        _zMovement = Input.GetAxis("Vertical");

        //calculate rotation as an ID Vector for turning around
        _yRotation = Input.GetAxis("Mouse X");
       
        //Calculate look up and down camera rotation
        cameraUpDownRotation = Input.GetAxis("Mouse Y") * lookSensitivity;       
    }

    //runs per physics iteration
    private void FixedUpdate()
    {
        PlayerMovement();       
    }


    private void PlayerMovement()
    {
        Vector3 _movementHorizontal = transform.right * _xMovement;
        Vector3 _movementVertical = transform.forward * _zMovement;

        //final movement velocity vector        
        Vector3 _movementVelocity = (_movementHorizontal + _movementVertical) * speed;

        rotation = new Vector3(0, _yRotation, 0);

        rb.MovePosition(rb.position + _movementVelocity * Time.deltaTime);        
        rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));

        if (fpsCamera != null)
        {
            currentCameraUpDownRotation -= cameraUpDownRotation;
            currentCameraUpDownRotation = Mathf.Clamp(currentCameraUpDownRotation, -85, 85);
            fpsCamera.transform.localEulerAngles = new Vector3(currentCameraUpDownRotation, 0, 0);
        }


        // to do -- Animations Controll
            //PlayAnimations();        
    }         

    
}
