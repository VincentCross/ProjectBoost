using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    // Parameters
    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 2f;

    // Cache
    Vector3 startingPos;
    Vector3 offset;
    float movementFactor;
    const float tau = Mathf.PI * 2; // constant value of 6.283
    

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) return;
        float cycles = Time.time / period; // continually growing over time
        float rawSinWave = Mathf.Sin(cycles * tau); // going from -1 to 1

        movementFactor = (rawSinWave + 1f) / 2f; // recalculated to go from 0 to 1 so it's cleaner

        offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
