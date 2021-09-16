using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform[] checkPoints;
    public Animator ani;
    [SerializeField] private float speed;    
    [SerializeField] private float radiusFire;
    [SerializeField] private int damage;
    [SerializeField] private float booletInSec;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Rigidbody blood;    
    [SerializeField] private PollerObject.ObjectInfo.ObjectType bloodMonster;
    public AudioSource step;
    public ParticleSystem fireBullet;
    public AudioSource fireBrauning;

    private Transform _targetCheckPoints;
    private Transform _targetMonsters;
    private float _inputX;
    private bool _inMove;
    private int _thisChecpoint;
    private Rigidbody _rb;
    private Vector3 _controlVector;
    private Vector3 _animationController;   
    private SphereCollider spherTrigger;
    private bool _fire;
    private float timeOut;
    private float curetTime;
   
    private bool _inKick;
   


    void Start()
    {
        ani = GetComponent<Animator>();       
        _rb = GetComponent<Rigidbody>();
        _targetCheckPoints = checkPoints[1];
        _thisChecpoint = 1;
        transform.position = checkPoints[1].transform.position;
        _inMove = false;         
        fireBullet.Stop();
        _fire = false;
        timeOut = 1 / booletInSec;
        curetTime = timeOut;
        step = GetComponent<AudioSource>();
        step.Stop();
        fireBrauning.Stop();
        _inKick = false;
    }

   
    void Update()
    {        
        _inputX = Input.GetAxis("Horizontal");
        if (_inKick == false)
        {
            MovementAndSound();
            Animation();
        }

        if (_targetMonsters != null && (_targetMonsters.transform.position.z - this.transform.position.z)* (_targetMonsters.transform.position.z - this.transform.position.z) > radiusFire*radiusFire*5) //проверяем расстояние до цели
        {            
            _targetMonsters = null;
            fireBullet.Stop();
            fireBrauning.Stop();
        }

        if (_targetMonsters == null && _inKick == false) //проверяем есть ли цель, если нет-запускаем поиск
        {           
            SearchMonster();
        }

        if (curetTime >= timeOut && _fire == true) //стрельба
        {
            Fire();
        }
        else
        {
            curetTime += Time.deltaTime;
        }
    }

    private void MovementAndSound()
    {
        
        if (_targetMonsters != null)
        {
            transform.LookAt(_targetMonsters);            
        }
        else
        {
            transform.LookAt(new Vector3(transform.position.x, 0, transform.position.z + 5));
            fireBrauning.Stop();
        }
       
        if (transform.position == _targetCheckPoints.position)
        {
            _inMove = false;
            step.Pause();
        }
        if (_inputX > 0 && _inMove == false && _thisChecpoint < checkPoints.Length - 1)
        {
            _thisChecpoint++;
            _targetCheckPoints = checkPoints[_thisChecpoint];
            _inMove = true;
            _controlVector = new Vector3(2, 0, 0);
            step.Play();
        }
        else if (_inputX < 0 && _inMove == false && _thisChecpoint > 0)
        {
            _thisChecpoint--;
            _targetCheckPoints = checkPoints[_thisChecpoint];
            _inMove = true;
            _controlVector = new Vector3(-2, 0, 0);
            step.Play();
        }
        else if (_inputX == 0 && _inMove == false)
        {
            _controlVector = new Vector3(0, 0, 0);
            step.Pause();
        }
        

        transform.position = Vector3.MoveTowards(transform.position, _targetCheckPoints.transform.position, Time.deltaTime * speed);
    }

    private void Animation()
    {
        _animationController = transform.InverseTransformVector(_controlVector);
        ani.SetFloat("X", _animationController.x);
        ani.SetFloat("Y", _animationController.z);        
    }

    private void Fire()
    {
        RaycastHit hit;
        if (Physics.Raycast(firePoint.position, firePoint.forward, out hit, radiusFire*2))
        {           
            if (hit.collider.transform.gameObject.CompareTag("Monster"))
            {               
                hit.collider.transform.gameObject.GetComponent<MonsterController>().health -= damage;               
                var blood = PollerObject.Instance.GetObject(bloodMonster);
                blood.transform.position = hit.point;               
                if (hit.collider.transform.gameObject.GetComponent<MonsterController>().health <= 0)
                {
                    StartCoroutine(WaitNextFraps());
                }
            }
        }
        curetTime = 0;
    }

    private void SearchMonster()
    {       
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, radiusFire, Vector3.forward, out hit, radiusFire))
        {
            
            if (hit.collider.transform.gameObject.CompareTag("Monster"))
            {               
                _targetMonsters = hit.transform;
                fireBullet.Play();
                _fire = true;
                fireBrauning.Play();
            }
        }
    }

    IEnumerator WaitNextFraps()
    {
        yield return new WaitForFixedUpdate();
        _targetMonsters = null;
        _fire = false;
        fireBullet.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster") && _inKick == false)
        {
            transform.LookAt(other.transform.position);
            ani.SetTrigger("Kick");
            fireBullet.Stop();
            fireBrauning.Stop();
            _targetMonsters = null;
            LineDead.points += 5;
            LineDead.health -= 2;
            other.GetComponent<MonsterController>().health = 0;
            _fire = false;
            _inKick = true;
            StartCoroutine(WaitEndKick());
        }
    }

    IEnumerator WaitEndKick()
    {
        yield return new WaitForSeconds(2f);
        _inKick = false;
    }


}
