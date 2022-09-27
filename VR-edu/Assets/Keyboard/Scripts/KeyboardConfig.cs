using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyboardConfig : MonoBehaviour
{
    [HideInInspector]public GameObject inputField;
    private TMP_InputField inputboxText;
    [HideInInspector]public bool shift=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Init()
    {
        inputboxText=inputField.GetComponent<TMP_InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void input(string upper,string lower)
    {
        if(lower.Equals("shift"))
        {
            shift=!shift;
        }
        else if(lower.Equals("backspace"))
        {
            if(inputboxText.text.Length>0)
                inputboxText.text=inputboxText.text.Substring(0,inputboxText.text.Length-1);
        }
        else if(lower.Equals("enter"))
        {
            inputboxText.text=inputboxText.text+"\n";
        }
        else if(lower.Equals("tab"))
        {
            inputboxText.text=inputboxText.text+"\t";
        }
        else if(lower.Equals("hide"))
        {
            gameObject.SetActive(false);
            return;
        }
        else
        {
            if(shift)
                inputboxText.text=inputboxText.text+upper;
            else
                inputboxText.text=inputboxText.text+lower;
        }
        inputField.GetComponent<VRInputField>().SelectingByScript=true;
        inputboxText.ActivateInputField();
        StartCoroutine(IETest());
    }
    IEnumerator IETest()
    {
        yield return new WaitForEndOfFrame();
        inputboxText.MoveTextEnd(true);
        inputField.GetComponent<VRInputField>().SelectingByScript=false;
    }
}
