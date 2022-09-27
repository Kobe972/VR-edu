using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Runtime.InteropServices;

using agora_gaming_rtc;
using agora_utilities;
using UnityEngine.SceneManagement;
using Random = System.Random;
using UnityEngine.UI;
public class AgoraInvoker : MonoBehaviour
{
    private AgoraManager _agora = new();
    private bool _agoraHasInit = false;
    private bool _isVR;
    public GameObject deskScreen;
    public Action<float, float> left_localclick_call;
    public Action<float, float> right_localclick_call;
    [DllImport("getclipboard.dll")]
    public static extern IntPtr _Z16GetClipboardTextv();
    public string LastClipboardText;
    public bool StreamAvail = false;
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        left_localclick_call = MouseTools.ClickLeftMouseButton;
        right_localclick_call = MouseTools.ClickRightMouseButton;
#endif

    }
    // private void OnGUI()
    // {
    //     if (!_agoraHasInit)
    //     {
    //         if (GUI.Button(new Rect(10, 10, 150, 50), "Connect as PC"))
    //         {
    //             InitAgora(false);
    //             StreamAvail = true;
    //         }
    //         // if (GUI.Button(new Rect(10, 90, 150, 50), "Connect as VR"))
    //         // {
    //         //     InitAgora(true);
    //         // }
    //     }
    // }
    public void InitAgora(bool isVR)
    {
        if (!_agoraHasInit)
        {
            _isVR = isVR;
            if (isVR)
            {
                Debug.Log("agora Start as VR!");
                //GameObject root = GameObject.Find("XR Rig");
                GameObject root = GameObject.Find("UIPos");
                GameObject Setchannel = root.transform.Find("ChannelName").gameObject;
                Setchannel.gameObject.SetActive(true);
                //_agora.Init(1, true);

                _agora.Init(1, true, OnstreamMessage: OnStreamDataReceive);


            }
            else
            {
                Debug.Log("agora Start as PC!");
                _agora.Init(3, false, OnstreamMessage: OnStreamDataReceiveclick);
                _agora.LocalDisplayInit();
                _agora.ShareDisplayScreen();
                _agoraHasInit = true;

            }
        }
        else
        {
            Debug.LogWarning("Try to init Agora twice!");
        }
    }
    public void SetChannelName(string value)
    {
        _agora._agoraChannelName = value;
        //_agora._agoraChannelName = "testtype";
        //Debug.Log("_agora._agoraChannelName:" + _agora._agoraChannelName);
    }
    public void JoinByChannelName()
    {
        Debug.Log("_agoraChannelName:" + _agora._agoraChannelName);
        GameObject Setchannel = GameObject.Find("ChannelName");

        if (string.IsNullOrEmpty(_agora._agoraChannelName))
        {
            Debug.LogError("Channel Name is null or empty");
            return;
        }
        else
        {
            Setchannel.gameObject.SetActive(false);
            _agora.mRtcEngine.JoinChannelByKey(null, _agora._agoraChannelName, null, 1);
            _agora.clickStreamID = _agora.mRtcEngine.CreateDataStream(new DataStreamConfig { syncWithAudio = false, ordered = true });
            //_agora.clipboardStreamID=_agora.mRtcEngine.CreateDataStream(new DataStreamConfig{syncWithAudio=false, ordered=true});
            Debug.Log("initializeEngine and Join Channel done");
            _agora.SetSelfScreen(deskScreen, 3);
            _agoraHasInit = true;
        }
    }
    // Update is called once per frame
    void Update()
    {

        // if(StreamAvail)
        // {
        //     try
        //     {
        //         CheckClipboard();
        //     }
        //     catch
        //     {

        //     }
        // }
    }
    private void OnDestroy()
    {
        if (_agoraHasInit)
            _agora.Destroy();
    }
    public void CheckClipboard()
    {
        IntPtr ptr = _Z16GetClipboardTextv();
        byte[] copied = new byte[1024 * 1024];
        string _copied = Marshal.PtrToStringAnsi(ptr);
        if (String.Compare(LastClipboardText, _copied) != 0)
        {
            if (_agora.clipboardStreamID >= 0)
            {
                _agora.mRtcEngine.SendStreamMessage(_agora.clipboardStreamID, copied);
                LastClipboardText = _copied;
                Debug.Log(LastClipboardText);
            }
            else
                Debug.LogWarning("StreamMessage ID not ready!");
        }

    }
    public void RemoteClick(float x, float y, bool isleft)
    {
        if (_agora.clickStreamID >= 0)
        {
            var float_list = new float[2] { x, y };
            var byte_list = new byte[9];
            Buffer.BlockCopy(float_list, 0, byte_list, 0, 8);
            byte_list[8] = isleft ? (byte)1 : (byte)0;
            _agora.mRtcEngine.SendStreamMessage(_agora.clickStreamID, byte_list);
        }
        else
        {
            Debug.LogWarning("StreamMessage ID not ready!");
        }
    }

    public void OnStreamDataReceiveclick(uint userId, int streamId, byte[] data, int length)
    {
        Debug.Log("On Steam Data Recieve Start!");
        Debug.Log("userId:" + userId);
        Debug.Log(data);
        Debug.Log("length:" + length);
        if (_agoraHasInit && !_isVR)
        {
            Debug.Assert(data.Length == 9);
            if (left_localclick_call != null && right_localclick_call != null)
            {
                var postion = new float[2];
                Buffer.BlockCopy(data, 0, postion, 0, 8);
                if (data[8] == (byte)1)
                {
                    left_localclick_call(postion[0], postion[1]);
                }
                else
                {
                    right_localclick_call(postion[0], postion[1]);
                }
            }
        }
    }

    public void OnStreamDataReceive(uint userId, int streamId, byte[] data, int length)
    {
        string str = System.Text.Encoding.Default.GetString (data);
        //string str = System.Text.Encoding.ASCII.GetString(data);
        Debug.Log(str);
        if(str=="fear")
        {
            SceneManager.LoadScene(1);
        }
    }


}
