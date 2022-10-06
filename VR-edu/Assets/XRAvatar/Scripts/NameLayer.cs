using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameLayer : MonoBehaviour
{
    public List<GameObject> gameObjectsToMask;
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
        
    }
}
