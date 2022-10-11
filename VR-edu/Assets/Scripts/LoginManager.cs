using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using LitJson;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField LoginUsernameInputField;
    public TMP_InputField LoginPasswordInputField;
    public TMP_Text LoginErrorText;
    public TMP_InputField RegistryUsernameInputField;
    public TMP_InputField RegistryPasswordInputField;
    public TMP_InputField RegistryConfirmInputField;
    public TMP_Text RegistryErrorText;
    public GameObject LoginGameObject;
    public GameObject RegisterGameObject;
    public GameObject LoadingGameObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void login()
    {
        StartCoroutine(loginCoroutine());
    }
    IEnumerator loginCoroutine()
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        if(LoginUsernameInputField.text.Equals(""))
        {
            LoginErrorText.SetText("Input username!");
            yield break;
        }
        if(LoginPasswordInputField.text.Equals(""))
        {
            LoginErrorText.SetText("Input password!");
            yield break;
        }
        formData.Add(new MultipartFormDataSection("username", LoginUsernameInputField.text));
        formData.Add(new MultipartFormDataSection("password", LoginPasswordInputField.text));
        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:8080/loginIn", formData);
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            LoginErrorText.SetText("Network error");
        }
        ResponseInfo sg = JsonMapper.ToObject<ResponseInfo>(www.downloadHandler.text);
        if(sg.result.Equals("success"))
        {
            LoginGameObject.SetActive(false);
            LoadingGameObject.SetActive(true);
            GlobalVar.username=sg.username;
            GlobalVar.token=sg.token;
            SceneManager.LoadScene(1);
        }
        else
        {
            LoginErrorText.SetText("Username or password invalid");
        }
    }
    public void registry()
    {
        StartCoroutine(registryCoroutine());
    }
    IEnumerator registryCoroutine()
    {
        if(RegistryUsernameInputField.text.Equals(""))
        {
            RegistryErrorText.SetText("Input username!");
            yield break;
        }
        if(RegistryPasswordInputField.text.Equals(""))
        {
            RegistryErrorText.SetText("Password cannot be empty!");
            yield break;
        }
        if(!RegistryPasswordInputField.text.Equals(RegistryConfirmInputField.text))
        {
            RegistryErrorText.SetText("Password not confirmed!");
            yield break;
        }
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("username", RegistryUsernameInputField.text));
        formData.Add(new MultipartFormDataSection("password", RegistryPasswordInputField.text));
        UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:8080/register", formData);
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            RegistryErrorText.SetText("Network error");
        }
        ResponseInfo sg = JsonMapper.ToObject<ResponseInfo>(www.downloadHandler.text);
        if(sg.result.Equals("success"))
        {
            RegisterGameObject.SetActive(false);
            LoadingGameObject.SetActive(true);
            GlobalVar.username=sg.username;
            GlobalVar.token=sg.token;
            SceneManager.LoadScene(1);
        }
        else
        {
            RegistryErrorText.SetText("Username already exists");
        }
    }
    public void OpenRegisterPanel()
    {
        LoginGameObject.SetActive(false);
        RegisterGameObject.SetActive(true);
    }
    public void OpenLoginPanel()
    {
        RegisterGameObject.SetActive(false);
        LoginGameObject.SetActive(true);
    }
}
public class ResponseInfo
{
    public string domain { get; set; }
    public string result { get; set; }
    public string token { get; set; }
    public string username { get; set; }
}
