using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
   [SerializeField] float moveSpeed=5f;
   [SerializeField] float rotationSpeed=500f;
   [Header("Ground Check Settings")]
   [SerializeField] float groundCheckRadius=0.2f;
   [SerializeField] Vector3 groundCheckOffset;
   [SerializeField] LayerMask groundLayer;

   bool isGrounded;
   float ySpeed;

   Quaternion targetRotation;

   CameraController cameraController;
   Animator animator;
   CharacterController characterController;

   private void Awake()
   {
    cameraController=Camera.main.GetComponent<CameraController>();
    animator = GetComponent<Animator>();
    characterController=GetComponent<CharacterController>();
   }

   private void Update()
   {
    float horizontal = Input.GetAxis("Horizontal");
    float vertical = Input.GetAxis("Vertical");

    float moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

    var moveInput = (new Vector3(horizontal, 0 , vertical)).normalized;

    var moveD= cameraController.PlanerRotation * moveInput;

    groundCheck();
    Debug.Log("Ground="+isGrounded);
    if (isGrounded)
    {
        ySpeed = -0.5f;
    }
    else
    {
        ySpeed +=Physics.gravity.y * Time.deltaTime;
    }

    var velocity = moveD* moveSpeed;
    velocity.y=ySpeed;
    
    characterController.Move(velocity * Time.deltaTime);

    if(moveAmount > 0)
    {
        
        transform.rotation = Quaternion.LookRotation(moveD);
    }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        animator.SetFloat("MoveAnim", moveAmount, 0.2f, Time.deltaTime);
    
   }

   void groundCheck()
   {
    isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
   }

   private void OnDrawGizmosSelected()
   {
    Gizmos.color= new Color(0,1,0,0.5f);
    Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
   }
}

