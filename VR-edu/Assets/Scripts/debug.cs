using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class debug : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject playerPrefab;
    public GameObject XROrigin;
    public XRController leftController;
    public Transform leftControllerTransform;
    public Transform rightControllerTransform;
    public Transform headTransform;
    void Start()
    {
        Instantiate(playerPrefab);
        XROrigin.AddComponent<CameraMove>();
        XROrigin.GetComponent<CameraMove>().LeftEye = GameObject.Find("LeftEye").transform;
        XROrigin.GetComponent<CameraMove>().RightEye = GameObject.Find("RightEye").transform;
        XROrigin.GetComponent<CameraMove>().LeftToe_End = GameObject.Find("LeftToe_End").transform;
        XROrigin.GetComponent<CameraMove>().RightToe_End = GameObject.Find("RightToe_End").transform;
        XROrigin.GetComponent<CameraMove>().leftController = leftController;
       // GameObject.Find("Avatar-Yizhang").GetComponent<AvatarController>().setVRTarget(leftControllerTransform, rightControllerTransform, headTransform);
        //GameObject.Find("Avatar-Yizhang").GetComponent<AvatarAnimationController>().leftController= leftController;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
