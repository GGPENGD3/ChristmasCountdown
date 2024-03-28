using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFootsteps : MonoBehaviour
{
    public List<AudioClip> concreteFS;
    public float footstepDelay;
    public float minVol, maxVol, minPitch, maxPitch;
    public AudioSource footstepSource;
    float delay;

    private void Start()
    {
        footstepSource = GetComponents<AudioSource>()[0];

        delay = footstepDelay;
    }

    private void Update()
    {
        if (GetComponent<FPS_Controller>().playerCanMove && GetComponent<FPS_Controller>().isWalking)
        {
            if (!footstepSource.isPlaying)
            {
                delay -= Time.deltaTime;

                if (delay <= 0)
                {
                    PlayFootsteps();
                    delay = footstepDelay;
                }

            }
        }
    }

    public void PlayFootsteps()
    {
        AudioClip clip = null;
        clip = concreteFS[Random.Range(0, concreteFS.Count)];
        footstepSource.clip = clip;
        footstepSource.volume = Random.Range(minVol, maxVol);
        footstepSource.pitch = Random.Range(minPitch, maxPitch);
        footstepSource.PlayOneShot(footstepSource.clip);
    }
}
