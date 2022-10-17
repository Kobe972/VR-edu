using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class RPCCalls : NetworkBehaviour
{
    public GameObject importedObj;
    public bool call=false;
    public Vector3 position;
    public string dir;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void FixedUpdateNetwork()
    {
        if(Object.HasInputAuthority&&call)
        {
            RPC_LoadThreeDModel(dir,position);
            call=false;
        }
    }
    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_LoadThreeDModel(string _dir, Vector3 _position, RpcInfo info = default)
    {
        //Debug.Log("called");
        var gameobject=Instantiate(importedObj);
        gameobject.GetComponent<LoadObject>().Load(_dir);
        gameobject.transform.position=_position;
    }
}
