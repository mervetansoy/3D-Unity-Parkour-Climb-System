using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parkour System/New parkour action")]
public class ParkourAction : ScriptableObject
{
    [SerializeField] string animationName;
    [SerializeField] string obstacleTag;
    [SerializeField] float minHeight;
    [SerializeField] float maxHeight;
    [SerializeField] bool rotateToObstacle;
    [SerializeField] float positionActionDelay;

    [Header("Target Matching")]
    [SerializeField] bool enableTargetMatching = true;
    [SerializeField] protected AvatarTarget matchBodyPart;
    [SerializeField] float matchStartTime;
    [SerializeField] float matchTargetTime;
    [SerializeField] Vector3 matchPositionWeith = new Vector3(0,1,0);
    
    public Quaternion TargetRotation {get;set;}
    public Vector3 MatchPosition {get;set;}
    public bool Mirror {get;set;}

    public virtual bool CheckIfPossible(ObstacleHitData hitData, Transform player)
    {
        if(!string.IsNullOrEmpty(obstacleTag) && hitData.forwardHit.transform.tag  != obstacleTag)
        return false;
        
        float height = hitData.heightHit.point.y - player.position.y;
        if( height < minHeight || height > maxHeight)
        return false;

        if (rotateToObstacle)
            TargetRotation = Quaternion.LookRotation(-hitData.forwardHit.normal);
        if (enableTargetMatching)
            MatchPosition = hitData.heightHit.point;
        return true;
    }

    public string AnimationName => animationName;
    public bool RotateToObstacle => rotateToObstacle;
    public float PositionActionDelay => positionActionDelay;

    public bool EnableTargetMatching => enableTargetMatching;
    public AvatarTarget MatchBodyPart => matchBodyPart;
    public float MatchStartTime => matchStartTime;
    public float MatchTargetTime => matchTargetTime;
    public Vector3 MatchPositionWeith => matchPositionWeith;
   
}
