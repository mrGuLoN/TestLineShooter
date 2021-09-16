using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEmmitor : MonoBehaviour
{
    [SerializeField] private PollerObject.ObjectInfo.ObjectType monsterType;
    [SerializeField] private Vector3 spawnMonsters;
    public bool needMonster;
    void Start()
    {
        needMonster = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (needMonster == true)
        {
            var monster = PollerObject.Instance.GetObject(monsterType);
            monster.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);
            monster.transform.rotation = new Quaternion(0, Mathf.PI, 0,0);
            needMonster = false;
        }
    }
}
