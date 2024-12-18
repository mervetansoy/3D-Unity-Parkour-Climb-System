using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Parkour System/New parkour action")]
public class ParkourAction : ScriptableObject
{
    [SerializeField] string animationName;
    [SerializeField] float minHeight;
    [SerializeField] float maxHeight;
    [SerializeField] bool rotateToObstacle;
    
    public Quaternion TargetRotation {get;set;}

    public bool CheckIfPossible(ObstacleHitData hitData, Transform player)
    {
        float height = hitData.heightHit.point.y - player.position.y;
        if( height < minHeight || height > maxHeight)
        return false;

        if (rotateToObstacle)
            TargetRotation = Quaternion.LookRotation(-hitData.forwardHit.normal);

        return true;
    }

    public string AnimationName => animationName;
    public bool RotateToObstacle => rotateToObstacle;
   
}
