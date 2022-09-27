using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using agora_gaming_rtc;
using agora_utilities;
using System;
using Random = System.Random;
using UnityEngine.UI;

public class AgoraManager
{
    public string _agoraAppID = "a39ef05040d64fbfa1d176e8560536cc";
    //public string _agoraToken = "0060b18329b0f7a49cca267abdca7949040IACGLRfnjUKawi7BnuDRaKpqw3wvGrg5kYuPhUjtLxG13ZuO0m0AAAAAEABj4sgEuqP7YgEA6ANKYPpi";
    public  string _agoraChannelName;
    public IRtcEngine mRtcEngine;

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
    readonly List<AgoraNativeBridge.RECT> WinDisplays = new List<AgoraNativeBridge.RECT>();
#else
    List<uint> MacDisplays;
#endif
    private int CurrentDisplay = 0;
    private VideoSurface SelfScreen;
    private VideoSurface PreviewScreen;
    private VideoSurface PublicScreen;
    public int clickStreamID = -1;
    public int clipboardStreamID = -1;
    private int rep = 0;
 
    // Start is called before the first frame update
    public void Init(uint uid, bool isVR, OnJoinChannelSuccessHandler OnJoinChannelSuccess = null, OnStreamMessageHandler OnstreamMessage = null)
    {
        mRtcEngine = IRtcEngine.GetEngine(_agoraAppID);
        mRtcEngine.OnJoinChannelSuccess = OnJoinChannelSuccess;
        mRtcEngine.OnError = HandleError;
        mRtcEngine.OnStreamMessage = OnstreamMessage;
        //mRtcEngine.OnUserJoined = OnUserJoined;
        //mRtcEngine.OnUserOffline = OnUserOffline;
        //mRtcEngine.OnVideoSizeChanged = OnVideoSizeChanged;
        // Calling virtual setup function

        mRtcEngine.EnableVideo();
        // allow camera output callback
        mRtcEngine.EnableVideoObserver();
        mRtcEngine.EnableLocalVideo(false);
        mRtcEngine.EnableLocalAudio(false);

        if (!isVR)
        {
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + this.rep;
            this.rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> this.rep)));
            for (int i = 0; i < 4; i++)
            {
                int num = random.Next();
                str = str + ((char)(0x30 + ((ushort)(num % 10)))).ToString();
            }
            _agoraChannelName = str;
            Debug.Log(string.Format("ChannelName:{0}", _agoraChannelName));
            mRtcEngine.JoinChannelByKey(null, _agoraChannelName, uid: uid);
        }

        // join channel
        //mRtcEngine.JoinChannel(channel, null, 0);
        //mRtcEngine.JoinChannelByKey(_agoraToken, _agoraChannelName, uid: uid);
        //mRtcEngine.JoinChannelByKey(null, _agoraChannelName, uid: uid);
        // if (isVR)
        // {
        //     clickStreamID = mRtcEngine.CreateDataStream(new DataStreamConfig { syncWithAudio = false, ordered = true });
        // }
        // Debug.Log("initializeEngine and Join Channel done");
    }
    public void LocalDisplayInit()
    {
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
            MacDisplays = AgoraNativeBridge.GetMacDisplayIds();
            WindowList list = AgoraNativeBridge.GetMacWindowList();
            if (list != null)
            {
                dropdown.options = list.windows.Select(w =>
                    new Dropdown.OptionData(w.kCGWindowOwnerName + " | " + w.kCGWindowNumber)).ToList();
            }
#elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        // Monitor Display info
        var winDispInfoList = AgoraNativeBridge.GetWinDisplayInfo();
        if (winDispInfoList != null)
        {
            foreach (var dpInfo in winDispInfoList)
            {
                WinDisplays.Add(dpInfo.MonitorInfo.monitor);
            }
        }


#endif
    }
    public void ShareDisplayScreen()
    {
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX || UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        Debug.Assert(WinDisplays.Count > 0, "No display detected");
#endif
        ScreenCaptureParameters sparams = new ScreenCaptureParameters
        {
            captureMouseCursor = true,
            frameRate = 15
        };

        mRtcEngine.StopScreenCapture();

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        mRtcEngine.StartScreenCaptureByDisplayId(MacDisplays[CurrentDisplay], default(Rectangle), sparams); 
#elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        ShareWinDisplayScreen(CurrentDisplay);
#endif
        mRtcEngine.EnableLocalVideo(true);
    }

    void ShareWinDisplayScreen(int index)
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        var screenRect = new Rectangle
        {
            x = WinDisplays[index].left,
            y = WinDisplays[index].top,
            width = WinDisplays[index].right - WinDisplays[index].left,
            height = WinDisplays[index].bottom - WinDisplays[index].top
        };
        Debug.Log(string.Format(">>>>> Start sharing display {0}: {1} {2} {3} {4}", index, screenRect.x,
            screenRect.y, screenRect.width, screenRect.height));
        var ret = mRtcEngine.StartScreenCaptureByScreenRect(screenRect,
            new Rectangle { x = 0, y = 0, width = 0, height = 0 }, default(ScreenCaptureParameters));
#endif
    }

    public void SetSelfScreen(GameObject go, uint PC_uid)
    {
        Debug.Log($"SetSelfScreen Start:uid= {PC_uid}");
        if (go.GetComponent<VideoSurface>() == null)
        {
            SelfScreen = go.AddComponent<VideoSurface>();
            SelfScreen.SetForUser(PC_uid);
            SelfScreen.SetEnable(true);
            SelfScreen.SetVideoSurfaceType(AgoraVideoSurfaceType.Renderer);
            SelfScreen.EnableFilpTextureApply(false, true);
        }
        else
        {
            Debug.LogWarning("multiple SetSelfScreen");
        }



        //videoSurface.SetGameFps(30);
        //videoSurface.EnableFilpTextureApply(enableFlipHorizontal: true, enableFlipVertical: false);
    }
    public void SetLocalPreview(GameObject go)
    {
        if (go.GetComponent<VideoSurface>() == null)
        {
            PreviewScreen = go.AddComponent<VideoSurface>();
            PreviewScreen.SetForUser(0);
            PreviewScreen.SetEnable(true);
            PreviewScreen.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
            PreviewScreen.EnableFilpTextureApply(false, true);
        }
        else
        {
            Debug.LogWarning("multiple SetLocalPreview");
        }
    }

    public void SetPublicScreen(GameObject go, uint share_uid)
    {
        if (go.GetComponent<VideoSurface>() == null)
        {

            PublicScreen = go.AddComponent<VideoSurface>();
        }
        else
        {
            PublicScreen = go.GetComponent<VideoSurface>();
        }
        PublicScreen.SetForUser(share_uid);
        PublicScreen.SetEnable(true);
        PublicScreen.SetVideoSurfaceType(AgoraVideoSurfaceType.Renderer);
        PublicScreen.EnableFilpTextureApply(false, true);


    }
    public void EnableSelfScreen(bool flag = true)
    {
        SelfScreen.SetEnable(flag);
    }



    #region Error Handling
    private int LastError { get; set; }
    private void HandleError(int error, string msg)
    {
        if (error == LastError)
        {
            return;
        }

        if (string.IsNullOrEmpty(msg))
        {
            //msg = string.Format("Error code:{0} msg:{1}", error, IRtcEngine.GetErrorDescription(error)); //PC不显示错误
        }

        switch (error)
        {
            case 101:
                msg += "\nPlease make sure your AppId is valid and it does not require a certificate for this demo.";
                break;
        }

        Debug.LogError(msg);

        LastError = error;
    }

    #endregion
    public void Destroy()
    {
        if (mRtcEngine != null)
        {
            IRtcEngine.Destroy();
        }
    }

}
