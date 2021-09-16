using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PollerObject : MonoBehaviour
{
    public static PollerObject Instance;
    
    [Serializable]
    public struct ObjectInfo
    {
        public enum ObjectType
        {
            Monster,
            BloodMonster,
        }

        public ObjectType Type;
        public GameObject monsterPrefab;
        public int howManyMonsters;
    }

    [SerializeField]
    private List<ObjectInfo> objectInfos;

    private Dictionary<ObjectInfo.ObjectType, Pool> pools;
    
    
    void Awake()
    {
        if (Instance == null) Instance = this;
        InitPool();
    }

    // Update is called once per frame
    private void InitPool()
    {
        pools = new Dictionary<ObjectInfo.ObjectType, Pool>();
        var emptyGo = new GameObject();

        foreach (var obj in objectInfos)
        {
            var container = Instantiate(emptyGo, transform, false);
            container.name = obj.Type.ToString();

            pools[obj.Type] = new Pool(container.transform);
            for (int i = 0; i < obj.howManyMonsters; i++)
            {
                var go = InstantiateObject(obj.Type, container.transform);
                pools[obj.Type].Objects.Enqueue(go);
            }
        }

        Destroy(emptyGo);
    }

    private GameObject InstantiateObject(ObjectInfo.ObjectType type, Transform parent)
    {
        var go = Instantiate(objectInfos.Find(x => x.Type == type).monsterPrefab, parent);
        go.SetActive(false);
        return go;
    }

    public GameObject GetObject (ObjectInfo.ObjectType type)
    {
        var obj = pools[type].Objects.Count > 0 ?
            pools[type].Objects.Dequeue() : InstantiateObject(type, pools[type].Container);
        obj.SetActive(true);
        return obj;
    }

    public void DestroyGameObject(GameObject obj)
    {
        pools[obj.GetComponent<IPooledObjects>().Type].Objects.Enqueue(obj);
        obj.SetActive(false);
    }
}
