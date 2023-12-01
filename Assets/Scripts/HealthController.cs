using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public GameObject[] lives;

    private Animator heart;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < lives.Length; i++)
        {
            if (i < currentHealth)
            {
                heart = lives[i].GetComponent<Animator>();
                heart.SetBool("Damaged", false);
                lives[i].SetActive(true);
            }
            else
            {
                heart = lives[i].GetComponent<Animator>();
                heart.SetBool("Damaged", true);
                timer += Time.deltaTime;
                Debug.Log(timer);
                if (timer > 1.2f)
                {
                    
                    lives[i].SetActive(false);
                    timer = 0;
                    //heart.SetBool("Damaged", false);
                }
            }
            
        }
    }

    void GetDamaged()
    {
        currentHealth -= 1;
    }
}
