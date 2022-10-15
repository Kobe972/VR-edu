using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptions : MonoBehaviour
{
    public GameObject ObjImporterPrefab;
    public GameObject Keyboard;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InvokeObjImporter()
    {
        GameObject option1 =Instantiate(ObjImporterPrefab);
        option1.GetComponent<GameObjects>().InputField.GetComponent<VRInputField>().keyboard = Keyboard;
    }
}
