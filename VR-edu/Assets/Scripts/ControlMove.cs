using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.XR;
using Fusion;
using UnityEngine.UI;
//public class ControlMove : NetworkBehaviour
public class ControlMove : MonoBehaviour
{
    public XRController leftController;
    public XRController rightController;
    public float moveSpeed;
    //public GameObject UINavBarPrefab;
    // public float gravity;
    // private float velgrav;
    // public float checkRadius;
    // public LayerMask groundLayer;
    // public Transform groundCheck;
    // public bool isGround;
    // Update is called once per frame
    public bool isUICall;

    public GameObject Screen;
    private bool temPrimaryButtonDown = false;
    private bool temLeftTriggerButtonDown = false;
    private bool temRightTriggerButtonDown = false;
    private bool temLeftGripButtonDown = false;
    private bool temRightGripButtonDown = false;

    public GameObject selfScreen;
    public Button StartScreenVRButton;
    private AgoraInvoker _agorainvoker;
    private Transform _leftControllerTrans;
    private Transform _rightControllerTrans;
    public float degree = 45;
    bool canTurn = true;
    Vector2 result_turn = new Vector2(0, 0);
    private Canvas UIPos_Canvas = null;
    GameObject player;
    GameObject Mcam;
    [SerializeField]
    //private NetworkCharacterControllerPrototype networkCharacterController=null;
    private void Start()
    {
        //Transform UIPos = transform.Find("UIPos");
        //UINavBar = Instantiate(UINavBarPrefab, UIPos.transform.position + UIPos.transform.up * 2.0f, Quaternion.identity,transform);

        _agorainvoker = GameObject.Find("AgoraSystem").GetComponent<AgoraInvoker>();
        Screen.SetActive(isUICall);

        //ScreenButton.onClick.AddListener(ShowHideSelfScreen);
        //StartScreenPCButton.onClick.AddListener(() => { _agorainvoker.InitAgora(false); StartScreenPCButton.gameObject.SetActive(false); StartScreenVRButton.gameObject.SetActive(false); });
        StartScreenVRButton.onClick.AddListener(() => { _agorainvoker.InitAgora(true); StartScreenVRButton.gameObject.SetActive(false); });
        _leftControllerTrans = GameObject.Find("LeftHand Controller").transform;
        _rightControllerTrans = GameObject.Find("RightHand Controller").transform;

        Mcam = GameObject.Find("Main Camera");
        player = GameObject.Find("Camera Offset");
        Mcam.transform.position = player.transform.position;
        Mcam.transform.rotation = player.transform.rotation;
        Mcam.transform.parent = player.transform;
    }

    public void ShowHideSelfScreen()
    {
        selfScreen.SetActive(!selfScreen.activeSelf);
    }
    // public override void FixedUpdateNetwork() {
    //     if(GetInput(out NetworkInputData data))
    //     {
    //         Vector3 moveVector=data.movementInput.normalized;
    //         networkCharacterController.Move(moveSpeed*moveVector*Runner.DeltaTime);
    //         // transform.position += transform.forward * (value * result.y * Time.deltaTime);
    //         // transform.position += transform.right * (value * result.x * Time.deltaTime);
    //     }
    // }
    void Update()
    {

        if (UIPos_Canvas == null)
        {
            try
            {
                UIPos_Canvas = GameObject.Find("UIPos").GetComponent<Canvas>();
                UIPos_Canvas.worldCamera = Camera.main;
            }
            catch{}
        }
        Vector2 result;
        var position = transform.position;


        var success_l = leftController.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out result);
        if (success_l)
        {
            //transform.position = new Vector3(position.x + value * result.x * Time.deltaTime, position.y, position.z + value * result.y * Time.deltaTime);
            transform.position += transform.forward * (moveSpeed * result.y * Time.deltaTime);
            transform.position += transform.right * (moveSpeed * result.x * Time.deltaTime);
            //Debug.Log("moving");
        }
        leftController.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool leftprimary);
        rightController.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool rightprimary);



        if ((leftprimary || rightprimary) && (!temPrimaryButtonDown))
        {
            Debug.Log("hide or show UI");
            isUICall = !isUICall;
            Screen.SetActive(isUICall);
        }




        temPrimaryButtonDown = leftprimary || rightprimary;


        leftController.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool leftTrigger);
        rightController.inputDevice.TryGetFeatureValue(CommonUsages.triggerButton, out bool rightTrigger);

        if (leftTrigger && !temLeftTriggerButtonDown)
        {
            RaycastHit hitinfo;
            if (Physics.Raycast(_leftControllerTrans.position, _leftControllerTrans.forward, out hitinfo, 2000, 1 << 6))
            {
                TryClick(true, hitinfo);
            }
            else
            {
                Debug.DrawRay(_leftControllerTrans.position, _leftControllerTrans.forward * 100, Color.green);
                Debug.Log("Did not Hit");
            }
        }
        temLeftTriggerButtonDown = leftTrigger;
        if (rightTrigger && !temRightTriggerButtonDown)
        {
            RaycastHit hitinfo;
            if (Physics.Raycast(_rightControllerTrans.position, _rightControllerTrans.forward, out hitinfo, 2000, 1 << 6))
            {
                TryClick(true, hitinfo);
            }
        }
        temRightTriggerButtonDown = rightTrigger;


        leftController.inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool leftGrip);
        rightController.inputDevice.TryGetFeatureValue(CommonUsages.gripButton, out bool rightGrip);
        if (leftGrip && !temLeftGripButtonDown)
        {
            RaycastHit hitinfo;
            if (Physics.Raycast(_leftControllerTrans.position, _leftControllerTrans.forward, out hitinfo, 2000, 1 << 6))
            {
                TryClick(false, hitinfo);
            }
        }
        temLeftGripButtonDown = leftGrip;
        if (rightGrip && !temRightGripButtonDown)
        {
            RaycastHit hitinfo;
            if (Physics.Raycast(_rightControllerTrans.position, _rightControllerTrans.forward, out hitinfo, 2000, 1 << 6))
            {
                TryClick(false, hitinfo);
            }
        }
        temRightGripButtonDown = rightGrip;



        var rotation = transform.rotation;
        var success_r = rightController.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out result_turn);
        if (canTurn)
        {
            if (success_r)
            {
                canTurn = false;
                if ((result_turn[0] < -0.4))
                {
                    transform.Rotate(0, -degree, 0, Space.Self);
                }
                if ((result_turn[0] > 0.4))
                {
                    transform.Rotate(0, degree, 0, Space.Self);

                }
            }
        }
        //}
        // isGround=Physics.CheckSphere(groundCheck.position,checkRadius,groundLayer);
        // if(isGround&&velgrav<0)
        // {
        //     velgrav=-2f; 
        // }
        // velgrav-=gravity*Time.deltaTime;
        // transform.position+=transform.up*velgrav;
    }
    // private void LateUpdate() {
    //     		// 	if(Mcam==null)
    // 			// {
    // 			// 	Mcam=GameObject.Find("Main Camera");
    // 			// }

    // 			// if(player==null)
    // 			// {
    // 			// 	player=GameObject.Find("Camera Offset");
    // 			// }
    //             // Mcam.transform.position = player.transform.position;
    // 			// Mcam.transform.rotation = player.transform.rotation;
    // }
    void TryClick(bool isLeft, RaycastHit hitinfo)
    {
        Debug.Log("Begin TryClick");
        var hitpoint = hitinfo.point;
        //Debug.Log(hitpoint-selfScreen.transform.position);
        var worldToLocalMatrix = Matrix4x4.TRS(selfScreen.transform.position, selfScreen.transform.rotation, Vector3.one).inverse;
        var localpoint = worldToLocalMatrix.MultiplyPoint3x4(hitpoint);
        //Debug.Log($"local 3d point{localpoint}");
        float width3d = selfScreen.GetComponent<RectTransform>().rect.width * selfScreen.transform.lossyScale.x;
        float height3d = selfScreen.GetComponent<RectTransform>().rect.height * selfScreen.transform.lossyScale.y;
        //Debug.Log($"width3d {width3d},height3d {height3d}");
        Vector2 norm2d = new Vector2(localpoint.x / width3d + 0.5f, localpoint.y / height3d + 0.5f);
        Debug.Log($"norm 2d point{norm2d}");
        var _agoraInvoker = GameObject.FindObjectOfType<AgoraInvoker>();
        if (_agoraInvoker != null)
        {
            _agoraInvoker.RemoteClick(norm2d.x, norm2d.y, isLeft);
        }
    }




    void FixedUpdate()
    {

    }
}
