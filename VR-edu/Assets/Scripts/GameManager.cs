using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;



using UnityEngine.XR.Interaction.Toolkit;
namespace Com.MyCompany.MyGame
{
    public class GameManager : MonoBehaviour
    

    {
        public GameObject bodyPrefab;
        [Tooltip("The prefab to use for representing the player")]
        public GameObject playerPrefab;
        public GameObject initposition;
        public GameObject[] respawns;
        public GameObject[] bodys;
        XRGrabInteractable BDgrib;
        public Camera cammain;

        #region Photon Callbacks
        private void Start()
        {
            //Instantiate(playerPrefab, new Vector3(0f, 15f, 0f), Quaternion.identity);


            //PhotonNetwork.Instantiate(this.playerPrefab.name, initposition.transform.position, Quaternion.identity, 0);
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            // else
            // {
            //     //Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
            //     // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            //     //PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f,5f,0f), Quaternion.identity, 0);
            //     // if (ControlMove.LocalPlayerInstance == null)
            //     // {
            //     //     Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
            //     //     // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate

            //     // }
            //     else
            //     {
            //         Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            //     }
            // }
        }

        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>



        // #endregion


        // #region Public Methods


        // public void LeaveRoom()
        // {
        //     PhotonNetwork.LeaveRoom();
        // }


        #endregion
        #region Private Methods


        void LoadArena()
        {
            // if (!PhotonNetwork.IsMasterClient)
            // {
            //     Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            // }
            // Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            // //PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
            // //PhotonNetwork.LoadLevel("room no.1");
        }


        #endregion
        public void Resbody()
        {

            respawns = GameObject.FindGameObjectsWithTag("male");
            if (respawns != null)
            {
                foreach (GameObject respawn in respawns)
                {
                    Destroy(respawn);
                }
            }
            //if (respawns == null)
            {
                Quaternion myRotation = Quaternion.identity;
                myRotation.eulerAngles = new Vector3(-90, 90, 0);
                //GameObject MaleBody = Instantiate(bodyPrefab, Bodypos);
                GameObject MaleBody = Instantiate(bodyPrefab, bodyPrefab.transform.position, myRotation);
                MaleBody.transform.DetachChildren();
                bodys=GameObject.FindGameObjectsWithTag("male");
                if (respawns != null)
            {
                foreach (GameObject BD in bodys)
                {
                    BDgrib=BD.AddComponent<XRGrabInteractable>();
                    BDgrib.attachTransform=cammain.transform;
                }
            }
            }
        }
        //         #region Photon Callbacks  //根据人数来切换场景
        // public override void OnPlayerEnteredRoom(Player other)
        // {
        //     Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


        //     if (PhotonNetwork.IsMasterClient)
        //     {
        //         Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


        //         LoadArena();
        //     }
        // }


        // public override void OnPlayerLeftRoom(Player other)
        // {
        //     Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects


        //     if (PhotonNetwork.IsMasterClient)
        //     {
        //         Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


        //         LoadArena();
        //     }
        // }


        // #endregion

    }
}