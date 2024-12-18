using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourController : MonoBehaviour
{

    [SerializeField] List<ParkourAction> parkourActions;
    bool inAction;
   EnvironmentScanner environmentScanner ;
   Animator animator;
   PlayerController playerController;

   private void Awake()
   {
    environmentScanner = GetComponent <EnvironmentScanner>();
    animator = GetComponent<Animator>();
    playerController = GetComponent<PlayerController>();
   }

   private void Update()
   {
    if(Input.GetButton("Jump") && !inAction)
    {
        var hitData = environmentScanner.ObstacleCheck();
        if(hitData.forwardHitFound)
        {
            foreach(var action in parkourActions)
            {
                if(action.CheckIfPossible(hitData, transform))
                {
                    StartCoroutine(DoParkourAction(action));
                    break;
                }
            }
        }
    }
   }

   IEnumerator DoParkourAction(ParkourAction action)
   {
        inAction = true;
        playerController.SetControl(false);
        animator.CrossFade(action.AnimationName, 0.2f);
        yield return null;

        var animationState = animator.GetNextAnimatorStateInfo(0);
        if(!animationState.IsName(action.AnimationName))
        Debug.LogError("yanlis yaptin ");

        

        float timer =0f;
        while(timer <= animationState.length)
        {
            timer += Time.deltaTime;

            if(action.RotateToObstacle)
                transform.rotation =Quaternion.RotateTowards(transform.rotation,action.TargetRotation, playerController.RotationSpeed*Time.deltaTime);

            yield return null;
        }

        playerController.SetControl(true);
        inAction = false;
   }
}
