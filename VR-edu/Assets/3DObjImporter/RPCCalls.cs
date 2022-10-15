using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class RPCCalls : NetworkBehaviour
{
    public GameObject importedObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_LoadThreeDModel(string dir, RpcInfo info = default)
    {
        var gameobject=Instantiate(importedObj);
        gameobject.GetComponent<LoadObject>().Load(dir);

    }
}
