using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamsa: ArmaPau
{
    protected override void Start()
    {
        // Chama o start do ArmaPau
        base.Start();

        // Mudar status das armas aqui
        dano = 3;
        velocidadeAtaque = 1.4f;
        
    }

    // Sobrescrevendo o método de ataque para comportamentos específicos da Espátula
    public override IEnumerator Ataque()
    {
        yield return base.Ataque(); // Chama o método de ataque do pai
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
}
