using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundMgr : MonoBehaviour {

    public List<AudioClip> m_sounds;
    AudioSource audio;

    public static SoundMgr GlobalInstance
    {
        get { return FindObjectOfType<SoundMgr>(); }
    }

	void Start () 
    {
        audio = GetComponent<AudioSource>();
	}
	
    void FireOneShot(int i)
    {
        audio.PlayOneShot(m_sounds[i]);
    }

	void Update () 
    {
	
	}
}
