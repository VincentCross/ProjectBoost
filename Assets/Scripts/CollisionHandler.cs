using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    // Parameters
    [SerializeField] AudioClip victoryClip;
    [SerializeField] AudioClip lossClip;

    // Cache
    Rigidbody objRigidbody;
    AudioSource objAudioSource;
    bool isTrasitioning = false;

    // Start is called before the first frame update
    void Start()
    {
        objRigidbody = GetComponent<Rigidbody>();
        objAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        if (isTrasitioning) { return; }

        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("You touched a friendly!");
                break;

            case "Finish":
                NextLevel();
                break;

            default:
                ResetLevel();
                break;
        }
    }

    void ResetLevel()
    {
        isTrasitioning = true;
        Debug.Log("YOU CRASHED! Try again.");
        objAudioSource.Stop();
        objAudioSource.PlayOneShot(lossClip,0.3f);
        objRigidbody.constraints = RigidbodyConstraints.FreezeAll;
        Invoke("LoadSameLevel",2);
    }

    void LoadSameLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    void NextLevel()
    {
        isTrasitioning = true;
        Debug.Log("Victory! Time to move along~");
        objAudioSource.Stop();
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
