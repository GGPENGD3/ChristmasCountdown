using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    AudioManager audio;
    void Start()
    {
        audio = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayRandomFootstepMonster()
    {
        audio.PlayRandom("footstep_wood");
    }

  
}
