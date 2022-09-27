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
    private NetworkRunner networkRunner=null;
    [SerializeField]
    private NetworkPrefabRef playerPrefab;
    [SerializeField]
    private Transform spawnPosition;
    private Dictionary<PlayerRef,NetworkObject> playerList= new Dictionary<PlayerRef,NetworkObject>();
    public GameObject leftController_obj;
    public GameObject rightController_obj;
    public XRController leftController;
    public XRController rightController;
    private void Start() {
        StartGame(GameMode.AutoHostOrClient);
    }
    // private void Update() {
    //   if(leftController_obj==null)
    //   {
    //   leftController_obj= GameObject.Find("LeftHand Controller");
    //   leftController=leftController_obj.GetComponent<XRController>();
    //   }
    //   if(rightController_obj==null)
    //   {
    //   rightController_obj= GameObject.Find("LeftHand Controller");
    //   rightController=leftController_obj.GetComponent<XRController>();
    //   }
    // }
    async void StartGame(GameMode mode)
{
  // Create the Fusion runner and let it know that we will be providing user input
  //_runner = gameObject.AddComponent<NetworkRunner>();
  networkRunner.ProvideInput = true;

  // Start or join (depends on gamemode) a session with a specific name
  await networkRunner.StartGame(new StartGameArgs()
  {
    GameMode = mode,
    SessionName = "Fusion Room",
    Scene = SceneManager.GetActiveScene().buildIndex,
    SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
  });}
  public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)    
  {
    NetworkObject networkPlayerObject=runner.Spawn(playerPrefab,spawnPosition.position,spawnPosition.rotation,player);

    playerList.Add(player,networkPlayerObject);
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
   public void OnInput(NetworkRunner runner,NetworkInput input){}
  // {     
  //       var data = new NetworkInputData();
  //       var position = transform.position;
  //       if(leftController!=null)
  //       {
  //       Vector2 result;
  //       var success_l = leftController.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out result);
  //       if(result[0]>0.4)
  //       data.movementInput+=Vector3.forward;
  //       if(result[0]<-0.4)
  //       data.movementInput+=Vector3.back;
  //       if(result[1]>0.4)
  //       data.movementInput+=Vector3.right;
  //       if(result[1]<-0.4)
  //       data.movementInput+=Vector3.left;
  //       }
  //   input.Set(data);
  // }
  public void OnHostMigration(NetworkRunner runner,HostMigrationToken hostMigrationToken){}
}
