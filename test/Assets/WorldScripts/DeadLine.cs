using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadLine : MonoBehaviour
{
    [SerializeField] private int healthPlayer;
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {          
            collision.GetComponent<MonsterController>()._ani.SetBool("Win", true);
            StartCoroutine(WinDoing(collision));
            healthPlayer--;
        }
    }

    IEnumerator WinDoing(Collider2D collision)
    {
        yield return new WaitForSeconds(1f);       
        collision.GetComponent<MonsterController>()._ani.SetBool("Win", false);
    }
}
