using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : Tank
{
    public Transform[] patrolStations;
    public float speed;

    public enum DefenderStates { Patrol, Attack}
    public DefenderStates DefenderCurrentState = DefenderStates.Patrol;

    int currentPatrolStation = 0;

    GameObject target = null;

    Enemy enemy;
    void Update()
    {
        switch (DefenderCurrentState)
        {
            case DefenderStates.Patrol:
                transform.LookAt(new Vector3(patrolStations[currentPatrolStation].position.x, transform.position.y, patrolStations[currentPatrolStation].position.z));

                if (Vector3.Distance(patrolStations[currentPatrolStation].position,transform.position) < 0.25f)
                {
                    currentPatrolStation = (currentPatrolStation + 1) % patrolStations.Length;
                }

                transform.position += transform.forward.normalized * speed * Time.deltaTime;
                break;
            case DefenderStates.Attack:
                transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));

                if (lastShootTime + fireSpeed < Time.time)
                {
                    StartCoroutine(FireTo(target.transform.position, Vector3.Distance(target.transform.position, fireTransform.position) * bulletSpeed));
                    lastShootTime = Time.time;

                    enemy.Hit();

                    if (enemy.remainingHealth <= 0)
                    {
                        DefenderCurrentState = DefenderStates.Patrol;
                    }
                }

                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Defender is switching to attack mode!");

            DefenderCurrentState = DefenderStates.Attack;

            target = other.gameObject;

            enemy = target.GetComponent<Enemy>(); 
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == target)
        {
            Debug.Log("Defender is switching to patrol mode!");

            target = null;
            DefenderCurrentState = DefenderStates.Patrol;
        }
    }
}
