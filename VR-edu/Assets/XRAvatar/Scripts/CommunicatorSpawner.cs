using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;


public class CommunicatorSpawner : NetworkBehaviour
{
    public GameObject NetworkCommunicator;
    private bool spawned;
    private void Awake()
    {
        spawned = false;
    }
    public override void FixedUpdateNetwork()
    {
        if(spawned==false)
        {
            spawned = true;
            var communicator=Runner.Spawn(NetworkCommunicator, new Vector3(0, 0, 0), Quaternion.identity,
            Object.InputAuthority, (runner, o) =>{});
            if (Object.HasStateAuthority)
            {
                GlobalVar.communicator = communicator;
            }
        }
    }
}
