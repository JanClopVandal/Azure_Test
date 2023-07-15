using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;


public class ThirdPersonMovemment : MonoBehaviour
{
    [SerializeField] private float speed = 6;
    [SerializeField] private Transform cam;
    [SerializeField] private float turnSmoothTime = 0.1f;

    [SerializeField] private float jumpHeight = 1;

    [SerializeField] private Animator animator; 

    private float turnSmoothVelocity;
    private CharacterController characterController;
    
    private float gravityValue = -9.81f;
    private Vector3 playerVelocity;
    private bool groundedPlayer;

    void Awake()
    {
        characterController = GetComponent<CharacterController>(); 
    }

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;

        if(direction.magnitude > 0.1)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

             
            Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            characterController.Move(moveDirection.normalized * speed * Time.deltaTime);

            animator.SetBool("Walk", true);
        }
        else animator.SetBool("Walk", false);

        groundedPlayer = characterController.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
       
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);

    }
}
