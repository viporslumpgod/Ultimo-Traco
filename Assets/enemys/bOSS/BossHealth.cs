using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : inimigo
{

   

    public bool isInvulnerable = false;

    public GameObject player;

    public BoxCollider2D demonBox;


    private void Start()
    {
        {
            vida = 2500;
        }
    }


    public void Died()
    {
        Die();
    }
   


    public override void levaDano(int damage)
    {
       base.levaDano(damage);

        GetComponent<Animator>().SetTrigger("Hurt");

        if (vida <= 0)
        {
            GetComponent<Animator>().SetTrigger("Dead");
            GetComponent<Animator>().SetBool("died", true);
            demonBox.enabled = false;    
        }


    }
}
