using Dummiesman;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjImportMainScript : MonoBehaviour
{
    public TMP_InputField inputedPath;
    public GameObject InputPanel;
    //public GameObject ControlPanel;
    public GameObject ObjImporterInst;
    //private GameObject Legend; //ͼ��
    //public Transform LegendBox; //����ͼ���ġ����ӡ�
    private string dir;
    //public TMP_Text Name;
    //public TMP_Text Visibility;
    // Start is called before the first frame update
    void Start()
    {
        //InitializedPosition=...
        //ControlPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Load()
    {
        dir = inputedPath.text;
        var mainCamera = GameObject.FindWithTag("MainCamera");
        Vector3 position = mainCamera.transform.position + mainCamera.transform.TransformVector(Vector3.forward) * 1f;
        position=new Vector3(position.x,mainCamera.transform.position.y-0.5f,position.z);
        Debug.Log(GlobalVar.communicator);
        Debug.Log(GlobalVar.communicator.GetComponent<RPCCalls>());
        GlobalVar.communicator.GetComponent<RPCCalls>().dir=dir;
        GlobalVar.communicator.GetComponent<RPCCalls>().position=position;
        GlobalVar.communicator.GetComponent<RPCCalls>().call=true;
        InputPanel.SetActive(false);
    }

    public void _Destroy()
    {
        Destroy(ObjImporterInst);
    }/*
    private void SetVisButton()
    {
        if (shown[index] == 1) Visibility.SetText("Hide");
        else Visibility.SetText("Show");
    }*/
}
