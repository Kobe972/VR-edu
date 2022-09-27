using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyConfig : MonoBehaviour
{
    public string Uppercase=string.Empty;
    public string Lowercase=string.Empty;
    public GameObject Keyboard;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void clickKey()
    {
        Keyboard.GetComponent<KeyboardConfig>().input(Uppercase,Lowercase);
    }
}
