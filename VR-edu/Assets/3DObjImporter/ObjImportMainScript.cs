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
    //private GameObject Legend; //Í¼Àý
    //public Transform LegendBox; //°üº¬Í¼ÀýµÄ¡°ºÐ×Ó¡±
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
        GlobalVar.communicator.GetComponent<RPCCalls>().RPC_LoadThreeDModel(dir);
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
