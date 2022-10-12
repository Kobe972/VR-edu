using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;

public class MenuVisible : MonoBehaviour
{
    public XRController leftController;
    public XRController rightController;
    public GameObject HandMenu;
    private bool pressed = false;
    private bool isLeftControllerDown = false;

    private bool visible;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(visible&&isLeftControllerDown)
        {
            HandMenu.transform.position = leftController.gameObject.transform.position;
            var rotation = Quaternion.LookRotation(leftController.gameObject.transform.TransformVector(Vector3.forward), leftController.gameObject.transform.TransformVector(Vector3.up));
            rotation = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
            HandMenu.transform.rotation = rotation;
            HandMenu.transform.position = leftController.gameObject.transform.position + leftController.gameObject.transform.TransformVector(Vector3.forward) * 0.1f+ leftController.gameObject.transform.TransformVector(Vector3.up) * 0.12f+ leftController.gameObject.transform.TransformVector(Vector3.right) * 0.12f;
          
        }
        bool result; 
        leftController.inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out result);
        if (result)
        {
            if(!pressed)
            {
                visible = !visible;
                pressed = true;
            }
            if (visible)
            {
                HandMenu.SetActive(true);
                isLeftControllerDown = true;
            }
            else
            {
                HandMenu.SetActive(false);
            }
        }
        else
        {
            pressed = false;
        }



    }


}
