using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("enemy")]
    [SerializeField] private Transform enemy;

    [Header("Movement parameteres")]
    [SerializeField] private float speed;
    private Vector3 Initscale;
    private bool movingleft;

    [Header("Idle Behavior")]
    [SerializeField] private float idleDuration;
    private float idleTimer;
    private void Awake()
    {
        Initscale = enemy.localScale;
    }


    private void Update()
    {
        if (movingleft)
        {
            if(enemy.position.x >= leftEdge.position.x) 
            
                MoveInCorrectDirection(-1);
            
            else
            
                DirectionChange();
            
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
            
                MoveInCorrectDirection(1);
            
            else
            
                DirectionChange();
            
            
        }
        
    }


    private void DirectionChange()
    {

        idleTimer += Time.deltaTime;

        if(idleTimer > idleDuration)
             movingleft = !movingleft;
    }




    private void MoveInCorrectDirection(int direction)
    {
        idleTimer = 0;
        //olhar pro lado certo 
        enemy.localScale = new Vector3(Mathf.Abs(Initscale.x) * direction, Initscale.y, Initscale.z);

        //anda ate ele
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * direction * speed, enemy.position.y, enemy.position.z);
    }
}
