using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   [SerializeField] float moveSpeed=5f;
   [SerializeField] float rotationSpeed=500f;

   Quaternion targetRotation;

   CameraController cameraController;
   Animator animator;

   private void Awake()
   {
    cameraController=Camera.main.GetComponent<CameraController>();
    animator = GetComponent<Animator>();
   }

   private void Update()
   {
    float horizontal = Input.GetAxis("Horizontal");
    float vertical = Input.GetAxis("Vertical");

    float moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

    var moveInput = (new Vector3(horizontal, 0 , vertical)).normalized;

    var moveD= cameraController.PlanerRotation * moveInput;

    if(moveAmount > 0)
    {
        transform.position += moveD* moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(moveD);
    }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        animator.SetFloat("MoveAnim", moveAmount, 0.2f, Time.deltaTime);
    
   }
}
