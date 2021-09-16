using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineDead : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float damageOnMonsters;
    [SerializeField] private ParticleSystem damage;
    [SerializeField] private float maxHealth;
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject loosCanvas;
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private AudioSource worldMusic;

    public static float health;
    public static int points;
    public static int howManyMonsters;
    public static int deadMonsters;

    private GameObject[] monsters;
    private GameObject[] emmitors;
    private bool firstdDead;

    private AudioSource hitVoltage;
    private void Start()
    {
        damage.Stop();
        hitVoltage = GetComponent<AudioSource>();
        points = 0;
        deadMonsters = 0;
        health = maxHealth;
        firstdDead = true;
        loosCanvas.SetActive(false);
        winCanvas.SetActive(false);
        worldMusic.Play();
    }
    void Update()
    {
        scoreText.text = points.ToString();
        healthSlider.value = health / maxHealth;
        Debug.Log(howManyMonsters + "/" + deadMonsters);

        if (health <= 0 && firstdDead == true)
        {
            Loos();
            firstdDead = false;
            Destroy(this.gameObject, 3f);           
        }

        if(deadMonsters >= howManyMonsters)
        {
            Win();
        }
    }

    private void Loos()
    {
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<CapsuleCollider>().enabled = false;
        player.GetComponent<PlayerController>().fireBrauning.Stop();
        player.GetComponent<PlayerController>().step.Stop();
        player.GetComponent<PlayerController>().fireBullet.Stop();
        worldMusic.Stop();


        player.GetComponent<Animator>().SetTrigger("End");

        monsters = GameObject.FindGameObjectsWithTag("Monster");
        emmitors = GameObject.FindGameObjectsWithTag("Emmitor");
        loosCanvas.SetActive(true);

        for (int i =0; i < monsters.Length; i ++)
        {
            monsters[i].GetComponent<MonsterController>().enabled = false;
            monsters[i].GetComponent<Animator>().SetTrigger("WinMonsters");
        }

        for (int j =0; j < emmitors.Length; j++)
        {
            emmitors[j].GetComponent<MonsterEmmitor>().enabled = false;
        }
    }

    private void Win()
    {
        winCanvas.SetActive(true);
        player.GetComponent<PlayerController>().enabled = false;
        player.GetComponent<CapsuleCollider>().enabled = false;
        player.GetComponent<PlayerController>().fireBrauning.Stop();
        player.GetComponent<PlayerController>().step.Stop();
        player.GetComponent<PlayerController>().fireBullet.Stop();
        player.GetComponent<Animator>().SetTrigger("Win");
        worldMusic.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            other.GetComponent<MonsterController>().health = 0;
            damage.Play();
            hitVoltage.Play();
            health -= damageOnMonsters;
        }
    }
}
