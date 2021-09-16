using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmmitorController : MonoBehaviour
{
    [SerializeField] private int minMonsters;
    [SerializeField] private int maxMonsters;
    [SerializeField] private float minTimeOutResp;
    [SerializeField] private float maxTimeOutResp;
    [SerializeField] private MonsterEmmitor[] emmitors;
   
   
    private int _monstersLenght;
    private float _timeOutResp;
    private float _curentTime;
    private int _randResp;
    void Awake()
    {       
        _monstersLenght = Random.Range(minMonsters, maxMonsters + 1);
        LineDead.howManyMonsters = _monstersLenght;
        _timeOutResp = Random.Range(minTimeOutResp, maxTimeOutResp);
        _curentTime = _timeOutResp;
    }

   
    void Update()
    {
        if (_curentTime >= _timeOutResp && _monstersLenght > 0)
        {
            _randResp = Random.Range(0, emmitors.Length);
            _monstersLenght--;
            _curentTime = 0;
            emmitors[_randResp].GetComponent<MonsterEmmitor>().needMonster = true;
            _timeOutResp = Random.Range(minTimeOutResp, maxTimeOutResp);
        }
        else
        {
            _curentTime += Time.deltaTime;
        }
    }
}
