using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blood : MonoBehaviour
{
    public PollerObject.ObjectInfo.ObjectType Type => type;
    [SerializeField]
    private PollerObject.ObjectInfo.ObjectType type;
    void Start()
    {
        Destroy(this.gameObject, 1f);
    }

   
}
