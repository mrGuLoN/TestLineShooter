using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour, IPooledObjects
{
    [SerializeField] private float speed;
    public int health;
    [SerializeField] private AudioSource deadAudio;
    public Animator _ani;
    
    public bool firstDead;
    public PollerObject.ObjectInfo.ObjectType Type => type;
    [SerializeField]
    private PollerObject.ObjectInfo.ObjectType type;

    private AudioSource _runAudio;    
    private int _numberDeadAnimation;
    private CapsuleCollider _capsuleCollider;
    private bool _dead;


    void Start()
    {
        _ani = GetComponent<Animator>();
        _dead = false;
        firstDead = false;
        _runAudio = GetComponent<AudioSource>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _capsuleCollider.enabled = true;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health <= 0 && firstDead == false)
        {
            _dead = true;
            _numberDeadAnimation = Random.Range(1, 4);
            _ani.SetInteger("Dead", _numberDeadAnimation);
            firstDead = true;
            _runAudio.Stop();
            deadAudio.Play();
            _capsuleCollider.enabled = false;
            StartCoroutine(WaitForGoToPool());
            LineDead.points += 10;
            LineDead.deadMonsters++;
        }

        if (_dead == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0, transform.position.z - 1), Time.deltaTime * speed);
        }
    }

    IEnumerator WaitForGoToPool()
    {
        yield return new WaitForSeconds(5f);
        _dead = false;
        firstDead = false;
        _capsuleCollider.enabled = true;
        _runAudio.Play();
        _ani.SetInteger("Dead", 0);
        PollerObject.Instance.DestroyGameObject(gameObject);
    }
}
