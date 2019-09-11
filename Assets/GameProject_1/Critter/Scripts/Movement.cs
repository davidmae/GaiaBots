using Assets.GameFramework.Item.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    NavMeshAgent navigator;
    Vector3 target;
    float time = 0;
    float timeLimit = 0.5f;


    //TODO: En función de más cosas...

    int eatingTime = 3;


    //TODO: Behaviour AI for State management

    bool searching = false;
    bool eating = false;

    //


    private void Awake()
    {
        navigator = GetComponent<NavMeshAgent>();

        searching = true;
        eating = false;
    }

    private void Start()
    {
        GoRandomPosition();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colission with: " + other);

        if (other != null)
        {
            var item = other.GetComponent<Consumable>();

            if (item != null)
            {
                //transform.LookAt(item.transform);

                target = item.transform.position;
                navigator.SetDestination(target);

                searching = false;
                eating = true;

            }
        }

    }

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 100))
        {
            Debug.DrawLine(transform.position, hitInfo.point, Color.red);
        }
        else
        {
            Debug.DrawLine(transform.position, ray.origin + ray.direction * 100, Color.green);
        }

        if (hitInfo.collider != null)
        {
            //TODO: ajustes de IA 
        }


        if (Vector3.Distance(transform.position, target) < 4)
        {
            Debug.Log("Arrived to position! --- Going to new random position ---");

            if (eating)
            {
                StartCoroutine(IsEating());
            }

            GoRandomPosition();
        }

        time += Time.deltaTime;

        if (time >= timeLimit)
        {
            Debug.Log(Vector3.Distance(transform.position, target));
            time = 0;
        }
    }

    private void GoRandomPosition()
    {
        float x = Random.Range(-20, 20);
        float z = Random.Range(-20, 20);

        target = new Vector3(x, 0, z);

        navigator.SetDestination(target);
    }


    private IEnumerator IsEating()
    {
        navigator.isStopped = true;
        yield return new WaitForSeconds( eatingTime );
        navigator.isStopped = false;
        eating = false;
    }


}
