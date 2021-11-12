using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Rigidbody objRigidbody;
    AudioSource objAudioSource;
    bool playingBoostAudio;
    [SerializeField] float thrust = 0f;
    [SerializeField] float rotation = 0f;
    [SerializeField] AudioClip engineClip;
    [SerializeField] ParticleSystem thrustParticles;
    bool playingThrustParticles;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        objRigidbody = GetComponent<Rigidbody>();
        objAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(objRigidbody.constraints != RigidbodyConstraints.FreezeAll)
        {
            ProcessThrust();
            ProcessRotation();
        } else {
            if(playingBoostAudio)
                TurnAudioOff();
        }
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.W))
        {
            objRigidbody.AddRelativeForce(Vector3.up * Time.deltaTime * thrust * 10);
            if(!playingBoostAudio)
                TurnAudioOn();
            if(!playingThrustParticles)
                TurnThrustParticlesOn();
        } else {
            if (playingBoostAudio)
                TurnAudioOff();
            if (playingThrustParticles)
                TurnThrustParticlesOff();
        }
    }

    private void TurnThrustParticlesOn()
    {
        thrustParticles.Play();
        playingThrustParticles = true;
    }
    
    private void TurnThrustParticlesOff()
    {
        thrustParticles.Stop();
        playingThrustParticles = false;
    }

    void TurnAudioOn()
    {
        objAudioSource.Play();
        playingBoostAudio = true;
    }

    private void TurnAudioOff()
    {
        objAudioSource.Stop();
        playingBoostAudio = false;
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(rotation);
        }
        else if (Input.GetKey(KeyCode.A)) {
            ApplyRotation(-rotation);
        }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        if (objRigidbody.constraints != RigidbodyConstraints.FreezeAll)
        {
            objRigidbody.freezeRotation = true;
            objRigidbody.transform.Rotate(Vector3.back * rotationThisFrame * Time.deltaTime * 10);
            objRigidbody.constraints = (int)RigidbodyConstraints.FreezeRotationX + (int)RigidbodyConstraints.FreezeRotationY + RigidbodyConstraints.FreezePositionZ;
        }
    }
}
