using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Tank
{
    public enum EnemyStates { MoveToTarget, Attack, Idle};
    public EnemyStates EnemyCurrentState = EnemyStates.MoveToTarget;

    public GameObject target;
    public GameObject defender = null;

    public float speed = 3f;

    public Defender defenderCS;
    void Update()
    {
        switch (EnemyCurrentState)
        {
            case EnemyStates.MoveToTarget:
                transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));

                transform.position += transform.forward.normalized * speed * Time.deltaTime;
                break;
            case EnemyStates.Attack:
                if (defender != null)
                {
                    transform.LookAt(new Vector3(defender.transform.position.x, transform.position.y, defender.transform.position.z));

                    if (lastShootTime + fireSpeed < Time.time)
                    {
                        StartCoroutine(FireTo(defender.transform.position, Vector3.Distance(defender.transform.position, fireTransform.position) * bulletSpeed));
                        lastShootTime = Time.time;

                        defenderCS = defender.GetComponent<Defender>();
                        defenderCS.Hit();
                        if (defenderCS.remainingHealth <= 0)
                        {
                            EnemyCurrentState = EnemyStates.MoveToTarget;
                        }
                    }
                }
                break;
            case EnemyStates.Idle:
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (EnemyCurrentState)
        {
            case EnemyStates.MoveToTarget:
                if (other.CompareTag("Target") || other.CompareTag("Defender"))
                {
                    Debug.Log("Enemy is switching to attack mode!");
                    transform.LookAt(new Vector3(other.transform.position.x,transform.position.y,other.transform.position.z));
                    EnemyCurrentState = EnemyStates.Attack;
                }
                break;
            case EnemyStates.Attack:
                if (other.CompareTag("Defender"))
                {
                    defender = other.gameObject;
                    
                }
                break;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        switch (EnemyCurrentState)
        {
            case EnemyStates.Attack:
                if (other.CompareTag("Defender"))
                {
                    EnemyCurrentState = EnemyStates.MoveToTarget;
                }
                else if (other.CompareTag("Target"))
                {
                    if (target == null)
                    {
                        EnemyCurrentState = EnemyStates.Idle;
                    }
                    else
                    {
                        EnemyCurrentState = EnemyStates.MoveToTarget;
                    }
                }
                break;
        }
    }
}
