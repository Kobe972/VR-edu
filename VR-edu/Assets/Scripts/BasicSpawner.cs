using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR;
public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    [SerializeField]
    private NetworkRunner networkRunner;
    [SerializeField]
    private NetworkPrefabRef playerPrefab;
    private Dictionary<PlayerRef,NetworkObject> playerList= new Dictionary<PlayerRef,NetworkObject>();
    public GameObject XROrigin;
    public XRController leftController;
    public Transform leftControllerTransform;
    public Transform rightControllerTransform;
    public Transform headTransform;
    private float DebugIncr=0;
    private void Start() {
        networkRunner= gameObject.AddComponent<NetworkRunner>();
        StartGame(GameMode.AutoHostOrClient);
    }
    async void StartGame(GameMode mode)
{
  // Create the Fusion runner and let it know that we will be providing user input
  //_runner = gameObject.AddComponent<NetworkRunner>();
  networkRunner.ProvideInput = true;

  // Start or join (depends on gamemode) a session with a specific name
  await networkRunner.StartGame(new StartGameArgs()
  {
    GameMode = mode,
    SessionName = GlobalVar.roomname,
    Scene = SceneManager.GetActiveScene().buildIndex,
    SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
  });}
  public GameObject FindGameObjectByName(Transform target, string name)
  {
      Transform trans = target.Find(name);
      if (trans != null)
      {
          return trans.gameObject;
      }
      for (int i = 0; i < target.childCount; i++)
      {
          GameObject obj = FindGameObjectByName(target.GetChild(i), name);//调用方法名进行递归 
          if (obj != null)
          {
              return obj.gameObject;
          }
      }
      return null;//如果不存在返回空
  }
  public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)    
  {
        if (runner.IsServer)
        {
            // Create a unique position for the player
            Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.DefaultPlayers) * 3, 1, 0);
            NetworkObject networkPlayerObject = runner.Spawn(playerPrefab, spawnPosition, Quaternion.identity, player);
            // Keep track of the player avatars so we can remove it when they disconnect
            playerList.Add(player, networkPlayerObject);
        }
    }
  public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)    
  {
        if(playerList.TryGetValue(player,out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            playerList.Remove(player);
        }
    }
  public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
  public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
  public void OnConnectedToServer(NetworkRunner runner) { }
  public void OnDisconnectedFromServer(NetworkRunner runner) { }
  public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
  public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
  public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
  public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
  public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
  public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }
  public void OnSceneLoadDone(NetworkRunner runner) { }
  public void OnSceneLoadStart(NetworkRunner runner) { }
  public void OnInput(NetworkRunner runner,NetworkInput input)
  {
        var data = new NetworkInputData();
        data.mainCameraPosition = headTransform.position;
        data.mainCameraRotate = headTransform.rotation;
        data.leftControllerPosition = leftControllerTransform.position;
        data.leftControllerRotate = leftControllerTransform.rotation;
        data.rightControllerPosition = rightControllerTransform.position;
        data.rightControllerRotate = rightControllerTransform.rotation;
        leftController.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out data.primaryAxisLeft);
        input.Set(data);
  }
  public void OnHostMigration(NetworkRunner runner,HostMigrationToken hostMigrationToken){}
}
