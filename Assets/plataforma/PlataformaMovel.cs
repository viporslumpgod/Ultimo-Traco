using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlataformaMovel : plataforma
{
    [SerializeField] List<Transform> pontos;
    public float velocidade = 2;
    int pontoAtual = 0;
    public TipoMovimento tipoMovimento;

    public enum TipoMovimento
    {
        Circular,
        atePontoFinal,
        TeleportaNoFinal
    }



    private void Awake()
    {
        if (pontos.Count == 0)
        {
            foreach (Transform ponto in transform.parent)
            {
                if (ponto.GetComponent<player>() == null)
                {
                    pontos.Add(ponto);
                }
            }
        }
    }



    void Start()
    {

    }


    void Update()
    {

        if (tipoMovimento == TipoMovimento.Circular)
        {
            MovePlataformaCIrculo();
        }    
            
        else if (tipoMovimento == TipoMovimento.atePontoFinal)
        {
            moveAtePontoFinal();
        }
           
        else if (tipoMovimento == TipoMovimento.TeleportaNoFinal)
        {
            TeleportaNoFinal();
        }
    }

        void MovePlataformaCIrculo()
        {
            if (Vector2.Distance(transform.position, pontos[pontoAtual].position) < 0.2f)
            {
                pontoAtual++;

                if (pontoAtual >= pontos.Count)
                {
                    pontoAtual = 0;
                }
            }

            transform.position = Vector2.MoveTowards(transform.position, pontos[pontoAtual].position, velocidade * Time.deltaTime);

        }

        void moveAtePontoFinal()
        {

            if (Vector2.Distance(transform.position, pontos[pontoAtual].position) < 0.2f)
            {
                if (pontoAtual < pontos.Count -1)
                {
                    pontoAtual++;

                }

            }
              
                transform.position = Vector2.MoveTowards(transform.position, pontos[pontoAtual].position, velocidade * Time.deltaTime);
            
        }

     void TeleportaNoFinal()
    {

        if (Vector2.Distance(transform.position, pontos[pontoAtual].position) < 0.2f)
        {
            pontoAtual++;

            if (pontoAtual >= pontos.Count)
            {
                pontoAtual = 0;
                transform.position = pontos[pontoAtual].position;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, pontos[pontoAtual].position, velocidade * Time.deltaTime);


    }

    public void SetVelocity(float newVelocity)
    {
        velocidade = newVelocity;
    }

 
}
