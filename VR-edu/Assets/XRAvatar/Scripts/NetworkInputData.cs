using UnityEngine;
using Fusion;

public struct NetworkInputData : INetworkInput
{
    public Vector2 primaryAxisLeft;

    public Vector3 mainCameraPosition;
    public Quaternion mainCameraRotate;

    public Vector3 leftControllerPosition;
    public Quaternion leftControllerRotate;

    public Vector3 rightControllerPosition;
    public Quaternion rightControllerRotate;


}
