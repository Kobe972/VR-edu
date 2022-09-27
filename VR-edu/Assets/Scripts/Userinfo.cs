using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : MonoBehaviour
{
    public static UserInfo instance;
    public static string userName;
    public static string userPassword;
    public static string userData;
     private void Awake() {
        if(instance!=null){
            Destroy(gameObject);
            return;
        }
        instance=this;
        DontDestroyOnLoad(instance);
     }

}
