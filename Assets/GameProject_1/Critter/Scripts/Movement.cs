using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    NavMeshAgent navigator;
    public float timeForNewPath;

    bool inCoroutine;
    Vector3 target;
    bool validPath;
    float time = 0;
    float timeLimit = 1f;


    private void Awake()
    {
        navigator = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        GoRandomPosition();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colission with: " + other);

        if (other.GetType() == typeof(SphereCollider))
            Debug.Log("---------- Run for your live! ----------");
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


        time += Time.deltaTime;

        if (time >= timeLimit)
        {
            if (Vector3.Distance(transform.position, target) < 5 ) 
            {
                Debug.Log("Arrived to position! --- Going to new random position ---");
                GoRandomPosition();
            }

            time = 0;
        }

        //if (!inCoroutine)
        //    StartCoroutine(DoSomething());
    }

    private void GoRandomPosition()
    {
        float x = Random.Range(-20, 20);
        float z = Random.Range(-20, 20);

        target = new Vector3(x, 0, z);

        navigator.SetDestination(target);
    }



    //private IEnumerator DoSomething()
    //{
    //    inCoroutine = true;
    //    yield return new WaitForSeconds(timeForNewPath);
    //    GetNewPath();
    //    validPath = navigator.CalculatePath(target, path);
    //    if (!validPath) Debug.Log("Found an invalid Path");

    //    while (!validPath)
    //    {
    //        yield return new WaitForSeconds(0.01f);
    //        GetNewPath();
    //        validPath = navigator.CalculatePath(target, path);
    //    }
    //    inCoroutine = false;
    //}


}
