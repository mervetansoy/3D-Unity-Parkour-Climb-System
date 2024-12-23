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

    animator.SetBool("mirrorAction", action.Mirror);
    animator.CrossFade(action.AnimationName, 0.2f);

    // Animasyonun geçiş yapmasını bekle
    yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(action.AnimationName));

    var animationState = animator.GetCurrentAnimatorStateInfo(0);
    if (!animationState.IsName(action.AnimationName))
    {
        Debug.LogError("Yanlış animasyon oynatılıyor!");
        yield break;
    }

    float timer = 0f;
    while (timer <= animationState.length)
    {
        timer += Time.deltaTime;

        if (action.RotateToObstacle)
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                action.TargetRotation,
                playerController.RotationSpeed * Time.deltaTime
            );

        if (action.EnableTargetMatching)
            MatchTarget(action);

        yield return null;
    }

    yield return new WaitForSeconds(action.PositionActionDelay);

    playerController.SetControl(true);
    inAction = false;
}



   void MatchTarget(ParkourAction action)
{
    // Geçiş durumunda MatchTarget çağrısını yapma
    if (animator.IsInTransition(0)) return;

    // Doğru animasyon oynuyor mu?
    var currentAnimation = animator.GetCurrentAnimatorStateInfo(0);
    if (!currentAnimation.IsName(action.AnimationName)) return;

    // Eğer hala MatchTarget aktifse, tekrar çağırmayı engelle
    if (animator.isMatchingTarget) return;

    // MatchTarget çağrısı
    animator.MatchTarget(
        action.MatchPosition, 
        transform.rotation, 
        action.MatchBodyPart, 
        new MatchTargetWeightMask(action.MatchPositionWeith, 0), 
        action.MatchStartTime, 
        action.MatchTargetTime
    );
}

}
