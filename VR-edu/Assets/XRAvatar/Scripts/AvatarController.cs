using UnityEngine;
using Fusion;


[System.Serializable]
public class MapTransform
{
    public Vector3 vrTargetPos;
    public Quaternion vrTargetRot;
    public Transform IKTarget;
    public Vector3 trackingRotationOffset;

    public void MapVRAvatar()
    {
        IKTarget.transform.position = vrTargetPos;
        IKTarget.transform.rotation = vrTargetRot * Quaternion.Euler(trackingRotationOffset);
    }
}

public class AvatarController : NetworkBehaviour
{
    [SerializeField] public MapTransform head;
    [SerializeField] public MapTransform leftHand;
    [SerializeField] public MapTransform rightHand;

    [SerializeField] private float turnSmoothness;

    [SerializeField] private Transform IKHead;

    [SerializeField] private Vector3 headBodyOffset;

    public override void FixedUpdateNetwork()
    {
        transform.position = IKHead.position + headBodyOffset;
        transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(IKHead.forward, Vector3.up).normalized, Time.deltaTime * turnSmoothness); ;
        
        if(GetInput(out NetworkInputData data))
        {
            setVRTarget(data.leftControllerPosition, data.leftControllerRotate, data.rightControllerPosition, data.rightControllerRotate, data.mainCameraPosition, data.mainCameraRotate);
        }
        
        head.MapVRAvatar();
        leftHand.MapVRAvatar();
        rightHand.MapVRAvatar();
        float angle=Vector3.Angle(new Vector3(transform.forward.x,0,transform.forward.z),new Vector3(IKHead.forward.x,0,IKHead.forward.z));
        if(angle>=30)
        {
            Quaternion BodyRotation = Quaternion.LookRotation(new Vector3(IKHead.forward.x,0,IKHead.forward.z), new Vector3(0,1,0));
            transform.rotation=BodyRotation;
        }
        
    }
    public void setVRTarget(Vector3 leftHandVrTargetPos, Quaternion leftHandVrTargetRot, Vector3 rightHandVrTargetPos, Quaternion rightHandVrTargetRot, Vector3 HeadVrTargetPos, Quaternion HeadVrTargetRot)
    {
        head.vrTargetPos = HeadVrTargetPos;
        head.vrTargetRot = HeadVrTargetRot;

        leftHand.vrTargetPos = leftHandVrTargetPos;
        leftHand.vrTargetRot = leftHandVrTargetRot;

        rightHand.vrTargetPos = rightHandVrTargetPos;
        rightHand.vrTargetRot = rightHandVrTargetRot;
    }
}