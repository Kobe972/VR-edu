using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class CanvasInit : MonoBehaviour
{

    public GameObject mainCamera;

    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera");
        var rotation = Quaternion.LookRotation(mainCamera.transform.TransformVector(Vector3.forward), mainCamera.transform.TransformVector(Vector3.up));
        rotation = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
        transform.rotation = rotation;
        transform.position = mainCamera.transform.position + mainCamera.transform.TransformVector(Vector3.forward) * 1f;
    }

    // Update is called once per frame
    void Update()
    {

    }

}
