using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum SoundIndex
    {
        BGM = 0,
        BlockBreak = 1,
        BlockMoved = 2
    };
    
    public static AudioManager instance;
    private AudioSource[] sounds;

    // Start is called before the first frame update
    void Start()
    {
        instance = GetComponent<AudioManager>();
        sounds = GetComponents<AudioSource>();
    }

    public void PlayMoveSound()
    {
        sounds[(int)SoundIndex.BlockMoved].Play();
    }

    public void PlayBreakSound()
    {
        sounds[(int)SoundIndex.BlockBreak].Play();
    }
}
