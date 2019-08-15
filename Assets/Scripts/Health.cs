using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{

    public HealthBar bar;
    public int maxHealth = 3;
    private int health;

    // Start is called before the first frame update
    void Start()
    {
        //healthDisplay = ReferenceHolder.instance.textDisplay;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage = 1)
    {
        health -= damage;
        bar.GetComponent<HealthBar>().SetHealth((float)health / maxHealth);
        if(health <= 0)
        {
            Transitioner.instance.Transition(SceneManager.GetActiveScene().buildIndex);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
