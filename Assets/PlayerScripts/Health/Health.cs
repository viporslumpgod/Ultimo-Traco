using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    private bool dead;
    public float currentHealth { get; private set; }

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;
    private Animator animator;

    private void Awake()
    {
        currentHealth = startingHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(float Damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - Damage, 0, startingHealth);

        if (currentHealth > 0) 
        {
            //player hurt//falta animar
            animator.SetTrigger("hurt");
        }
        else
        {
            if (!dead) 
            {
                //player dead//falta animar
                animator.SetTrigger("die");
                player.instance.enabled = false;
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

    private IEnumerator Invunerability()
    {
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
    }

}
