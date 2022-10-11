using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public TMP_InputField RoomNameInputField;
    public TMP_Text UserInfo;
    // Start is called before the first frame update
    void Start()
    {
        UserInfo.text=GlobalVar.username;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void enterRoom()
    {
        GlobalVar.roomname=RoomNameInputField.text;
        SceneManager.LoadScene(2);
    }
}
