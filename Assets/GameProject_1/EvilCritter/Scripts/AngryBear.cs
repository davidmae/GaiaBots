using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryBear : MonoBehaviour
{
    protected IBehaviour behaviour;

    int hungry;

    private void Awake()
    {
        behaviour = new HostileBehaviour(100);
        behaviour.onHungry += UpdateAngry;
    }

    float elapsed = 0f;
    private void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed >= 1f)
        {
            elapsed = elapsed % 1f;
            behaviour.CheckCurrentStats(hungry);
        }
    }

    public void UpdateAngry(int value)
    {
        Debug.Log(this.hungry);
    }
}
