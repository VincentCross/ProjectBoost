using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
	Rigidbody objRigidbody;
	AudioSource objAudioSource;
	public bool playingBoostAudio;
	[SerializeField] float thrust = 0f;
	[SerializeField] float rotation = 0f;
	[SerializeField] AudioClip engineClip;
	[SerializeField] ParticleSystem forwardThrustParticles;
	[SerializeField] ParticleSystem rightThrustParticlesTop;
	[SerializeField] ParticleSystem rightThrustParticlesBottom;
	[SerializeField] ParticleSystem leftThrustParticlesTop;
	[SerializeField] ParticleSystem leftThrustParticlesBottom;

	
	bool playingForwardThrustParticles;
	bool playingRightThrustParticles;
	bool playingLeftThrustParticles;


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
		if (objRigidbody.constraints != RigidbodyConstraints.FreezeAll)
		{
			ProcessThrust();
			ProcessRotation();
		} else
        {
            TurnOffAllEffects();
        }
    }

    private void TurnOffAllEffects()
    {
        if (playingBoostAudio)
            TurnBoostAudioOff();
        if (playingForwardThrustParticles)
            TurnForwardThrustParticlesOff();
        if (playingRightThrustParticles)
            TurnRightThrustParticlesOff();
        if (playingLeftThrustParticles)
            TurnLeftThrustParticlesOff();
    }

    void ProcessThrust()
	{
		if (Input.GetKey(KeyCode.W))
		{
			objRigidbody.AddRelativeForce(Vector3.up * Time.deltaTime * thrust * 10);
			if (!playingBoostAudio)
				TurnBoostAudioOn();
			if (!playingForwardThrustParticles)
				TurnForwardThrustParticlesOn();
		} else {
			if (playingBoostAudio && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
				TurnBoostAudioOff();
			if (playingForwardThrustParticles)
				TurnForwardThrustParticlesOff();
		}
	}

	private void TurnForwardThrustParticlesOn()
	{
		//Debug.Log("Turn forward thrust particles on.");
		forwardThrustParticles.Play();
		playingForwardThrustParticles = true;
	}
	
	private void TurnForwardThrustParticlesOff()
	{
		//Debug.Log("Turn forward thrust particles off.");
		forwardThrustParticles.Stop();
		playingForwardThrustParticles = false;
	}

	public void TurnBoostAudioOn()
	{
		//Debug.Log("Turning boost audio on.");
		objAudioSource.Play();
		playingBoostAudio = true;
	}

	public void TurnBoostAudioOff()
	{
		//Debug.Log("Turning boost audio off.");
		objAudioSource.Stop();
		playingBoostAudio = false;
	}

	void ProcessRotation()
	{
		if (Input.GetKey(KeyCode.D))
        {
            ProcessRotationRight();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            ProcessRotationLeft();
        }
        else {
			if (playingRightThrustParticles)
				TurnRightThrustParticlesOff();
			if (playingLeftThrustParticles)
				TurnLeftThrustParticlesOff();
			if (playingBoostAudio && !Input.GetKey(KeyCode.W))
				TurnBoostAudioOff();
		}
	}

    private void ProcessRotationRight()
    {
        ApplyRotation(rotation);
        if (!playingRightThrustParticles)
            TurnRightThrustParticlesOn();
        if (!playingBoostAudio)
            TurnBoostAudioOn();
    }

	private void ProcessRotationLeft()
    {
        ApplyRotation(-rotation);
        if (!playingLeftThrustParticles)
            TurnLeftThrustParticlesOn();
        if (!playingBoostAudio)
            TurnBoostAudioOn();
    }
	
    private void TurnRightThrustParticlesOn()
	{
		//Debug.Log("Turn right thrust particles on");
		rightThrustParticlesBottom.Play();
		leftThrustParticlesTop.Play();
		playingRightThrustParticles = true;
	}
	
	private void TurnRightThrustParticlesOff()
	{
		//Debug.Log("Turn right thrust particles off");
		rightThrustParticlesBottom.Stop();
		leftThrustParticlesTop.Stop();
		playingRightThrustParticles = false;
	}

		private void TurnLeftThrustParticlesOn()
	{
		//Debug.Log("Turn left thrust particles on");
		leftThrustParticlesBottom.Play();
		rightThrustParticlesTop.Play();
		playingLeftThrustParticles = true;
	}
	
	private void TurnLeftThrustParticlesOff()
	{
		//Debug.Log("Turn left thrust particles off");
		leftThrustParticlesBottom.Stop();
		rightThrustParticlesTop.Stop();
		playingLeftThrustParticles = false;
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
