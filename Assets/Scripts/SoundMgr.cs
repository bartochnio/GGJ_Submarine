using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundMgr : MonoBehaviour {

    public List<AudioClip> m_sounds;
    Dictionary<string, AudioClip> sndDick; // 8====D

    AudioSource audio;

    public static SoundMgr GlobalInstance
    {
        get { return FindObjectOfType<SoundMgr>(); }
    }

    Dictionary<string, AudioClip> ParseAudioList(List<AudioClip> list)
    {
        Dictionary<string, AudioClip> d = new Dictionary<string,AudioClip>();
        foreach (var a in list){
            d.Add(a.ToString(), a);
        }
        return d;
    }

    public void PlaySoundOnce(string sndName)
    {
        audio.PlayOneShot(sndDick[sndName + " (UnityEngine.AudioClip)"]);
    }


    void Awake()
    {
        sndDick = ParseAudioList(m_sounds);
    }
	void Start () 
    {
        audio = GetComponent<AudioSource>();
        sndDick = ParseAudioList(m_sounds);
        foreach (var s in sndDick)
        {
            Debug.Log(s.Key.ToString());
        }
	}
	
    void FireOneShot(int i)
    {
        audio.PlayOneShot(m_sounds[i]);
    }

	void Update () 
    {
	
	}
}
