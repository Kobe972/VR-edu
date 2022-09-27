using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class VRInputField : MonoBehaviour
{
    [HideInInspector]public bool SelectingByScript=false;
    public GameObject keyboard;
    public float DistanceToCamera=0.7f;
    private GameObject mainCamera;
    public void InvokeKeyboard()
    {
        if(SelectingByScript==false)
        {
            mainCamera=GameObject.FindWithTag("MainCamera");
            var rotation = Quaternion.LookRotation(mainCamera.transform.TransformVector(Vector3.forward), mainCamera.transform.TransformVector(Vector3.up));
            rotation = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
            keyboard.transform.rotation = rotation;
            keyboard.transform.position=mainCamera.transform.position+mainCamera.transform.TransformVector(Vector3.forward)*DistanceToCamera+mainCamera.transform.TransformVector(Vector3.down)*0.2f;
            keyboard.GetComponent<KeyboardConfig>().inputField=gameObject;
            keyboard.GetComponent<KeyboardConfig>().Init();
            keyboard.SetActive(true);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
