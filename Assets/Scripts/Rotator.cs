using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    // Parameters
    [SerializeField] float maxRotation;
    [SerializeField] float period = 10f;

    // Cache
    Vector3 startingRotations;
    float startingPoint;

    // Start is called before the first frame update
    void Start()
    {
        startingRotations = transform.rotation.eulerAngles;
        startingPoint = startingRotations.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(startingRotations.x, maxRotation * Mathf.Sin(startingPoint + (Time.time * (period / 100))), startingRotations.z);
        //Debug.Log("Updated rotation: " + gameObject.name + " - " + transform.rotation.eulerAngles);
    }
}
