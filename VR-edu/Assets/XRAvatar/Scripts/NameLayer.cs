using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NameLayer : MonoBehaviour
{
    public List<GameObject> gameObjectsToMask;
    private bool layerChecked=false;
    public void nameLayer(string name)
    {
        foreach(var o in gameObjectsToMask)
            o.layer=LayerMask.NameToLayer(name);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if(layerChecked==false&&gameObject.GetComponent<NetworkObject>())
        {
            if(this.gameObject.GetComponent<NetworkObject>().HasInputAuthority)
            {
                nameLayer("Ignore");
            }
            layerChecked=true;
        }
    }
}
