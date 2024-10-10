using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    private bool dead;
    public float currentHealth { get; private set; }

    private void Awake()
    {
        currentHealth = startingHealth;
    }

    public void TakeDamage(float Damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - Damage, 0, startingHealth);

        if (currentHealth > 0) 
        {
             //player hurt//falta animar
        }
        else
        {
            if (!dead) 
            {
                //player dead//falta animar
                player.instance.podeMover = false;
                player.instance.podeAtacar = false;
                player.instance.podePular = false;
                player.instance.podeDashar = false;
                dead = true;
            }
           
        }
    }


    public void AddHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, startingHealth);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TakeDamage(5);
        }
    }

}
