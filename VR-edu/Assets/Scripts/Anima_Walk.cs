using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
using System;


public class Anima_Walk : MonoBehaviour
{
    public XRController leftController;
    public Animator Walk;
    //private float mov;
    private bool walkflag;

private void Start() {

}
    // Update is called once per frame
    void Update()
    {

        Vector2 leftcontro;
        //var success = leftController.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out leftcontro);
        leftController.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out leftcontro);
        //Debug.Log("leftcontro:"+leftcontro);
        //mov=Mathf.Sqrt(leftcontro.x)/2+ Mathf.Sqrt(leftcontro.y)/2;
        if(leftcontro[0]>0.2||leftcontro[1]>0.2||leftcontro[0]<-0.2||leftcontro[1]<-0.2){
            walkflag= true;
        }
        else
        {
            walkflag= false;
        }
        Walk.SetBool(name: "walkflag", walkflag);


        // leftController.inputDevice.TryGetFeatureValue(CommonUsages.grip, out float leftcontro);
        // boyWalk.SetFloat(name: "leftcontro", leftcontro);
    }
}
