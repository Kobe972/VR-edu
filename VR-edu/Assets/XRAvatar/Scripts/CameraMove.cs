using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class CameraMove : MonoBehaviour
{
    public XRController leftController;
    public float moveSpeed=3;
    private GameObject mainCamera;
    private float AvatarHeightOffset;
    private void Start()
    {
        mainCamera=GameObject.FindWithTag("MainCamera");
        //AvatarHeightOffset=(LeftEye.position.y+RightEye.position.y)/2-(LeftToe_End.position.y+RightToe_End.position.y)/2;
        AvatarHeightOffset=1.68f;
    }

    private void Update()
    {
        Vector2 result;
        var success_l = leftController.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out result);
        if(success_l)
        {
            transform.position += mainCamera.transform.forward * (moveSpeed * result.y * Time.deltaTime);
            transform.position += mainCamera.transform.right * (moveSpeed * result.x * Time.deltaTime);
        }
        RaycastHit hitGround;
        bool isGroundDown = Physics.Raycast(mainCamera.transform.position, Vector3.down, out hitGround);
        transform.position=new Vector3(transform.position.x,hitGround.point.y+AvatarHeightOffset,transform.position.z);
    }
}
