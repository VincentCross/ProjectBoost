using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
	// Parameters
	[SerializeField] AudioClip victoryClip;
	[SerializeField] AudioClip lossClip;
	[SerializeField] ParticleSystem explosionParticles;
	[SerializeField] ParticleSystem victoryParticles;

	// Cache
	Rigidbody objRigidbody;
	AudioSource objAudioSource;
	bool isTrasitioning = false;
	Movement moveScript;
    bool disableCollisionLogic = false;

	// Start is called before the first frame update
	void Start()
	{
		objRigidbody = GetComponent<Rigidbody>();
		objAudioSource = GetComponent<AudioSource>();
		moveScript = GetComponent<Movement>();
	}

	// Update is called once per frame
	void Update()
	{
        RespondToDebugKeys();
	}

    private void RespondToDebugKeys()
    {
        if(Input.GetKeyDown(KeyCode.L))
            LoadNextLevel();
        
        if(Input.GetKeyDown(KeyCode.C)) {
            objRigidbody.detectCollisions = !objRigidbody.detectCollisions;
            Debug.Log("Detecting collisions: " + objRigidbody.detectCollisions);
        }

        if(Input.GetKeyDown(KeyCode.X)) {
            disableCollisionLogic = !disableCollisionLogic;
            Debug.Log("Running collision logic: " + !disableCollisionLogic);
        }
    }

    void OnCollisionEnter(Collision other)
	{
		if (isTrasitioning || disableCollisionLogic) { return; }

		switch (other.gameObject.tag)
		{
			case "Friendly":
				Debug.Log("You touched a friendly!");
				break;

			case "Finish":
				ProcessSuccess();
				break;

			default:
				ProcessFailure();
				break;
		}
	}

	void ProcessFailure()
	{
		isTrasitioning = true;
		Debug.Log("YOU CRASHED! Try again.");

		explosionParticles.Play();
		if (moveScript.playingBoostAudio)
			moveScript.TurnBoostAudioOff();
		objAudioSource.PlayOneShot(lossClip,0.3f);
		objRigidbody.constraints = RigidbodyConstraints.FreezeAll;

		Invoke("LoadSameLevel",2);
	}

	void LoadSameLevel()
	{
		int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(currentSceneIndex);
	}

	void ProcessSuccess()
	{
		isTrasitioning = true;
		Debug.Log("Victory! Time to move along~");

		victoryParticles.Play();
		if (moveScript.playingBoostAudio)
			moveScript.TurnBoostAudioOff();
		objAudioSource.PlayOneShot(victoryClip,0.3f);
		objRigidbody.constraints = RigidbodyConstraints.FreezeAll;

		Invoke("LoadNextLevel",4);
	}

	void LoadNextLevel()
	{
		int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
		if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings ) {
			nextSceneIndex = 0;
		}
		SceneManager.LoadScene(nextSceneIndex);
	}
}
