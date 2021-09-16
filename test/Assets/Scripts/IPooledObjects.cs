using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IPooledObjects
{
    PollerObject.ObjectInfo.ObjectType Type { get; }
}