using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{
//This audio system is a list of audio clips
//It utilises the eventmanger to play sounds according to which event is occuring 
//Volume can and other sound paramters can be easily adjusted by using any one of the audiosources in the scene which are configured for different situations

    public List<AudioClip> sfxClips = new List<AudioClip>();
    public List<AudioClip> instrumentalClips = new List<AudioClip>();
    public List<AudioClip> musicClips = new List<AudioClip>();
    public List<AudioClip> eventClips = new List<AudioClip>();
    public List<AudioSource> audioSources = new List<AudioSource>();

    // Start is called before the first frame update
    void Start()
    {
    //assigning functions to event manager
        EventManager.current.EventOne += FallBookEvent;
        EventManager.current.EventTwoStart += SceneTransitionVoices;
        EventManager.current.EventTwo += ForSceneTwo;
        EventManager.current.EventFour += EventFourNoises;
        EventManager.current.EventEightStart += Scene8Begins;
        EventManager.current.EventEight += ForSceneEight;
    }
//sounds correlating to events
    public void FallBookEvent()
    {
        audioSources[0].volume = Mathf.Lerp(0, 0.532f, 3f);
        audioSources[0].PlayOneShot(sfxClips[0]);
    }

    public void SceneTransitionVoices()
    {
        StartCoroutine(VoicesStart());
    }

    public void ForSceneTwo()
    {
        StartCoroutine(EventTwoSounds());
    }

    public void EventFourNoises()
    {
        StartCoroutine(EventFourNoisesCoroutine());
    }

    public void Scene8Begins()
    {


        audioSources[0].PlayOneShot(eventClips[0]);
    }
    public void ForSceneEight()
    {

        audioSources[1].PlayOneShot(instrumentalClips[1]);
        StartCoroutine(IncreaseVolumeEventEight(1));
    }

    private IEnumerator EventFourNoisesCoroutine()
    {
        audioSources[0].volume = Mathf.Lerp(0, 0.532f, 1f);
        audioSources[0].PlayOneShot(sfxClips[3]);
        yield return new WaitForSeconds(3f);
        audioSources[0].volume = Mathf.Lerp(0.532f, 0, 1f);
    }

    IEnumerator EventTwoSounds()
    {
        audioSources[0].PlayOneShot(sfxClips[2]);
        yield return new WaitForSeconds(2f);
        audioSources[0].volume = Mathf.Lerp(0, 0.532f, 5f);
        audioSources[0].PlayOneShot(instrumentalClips[0]);
        yield return new WaitForSeconds(17f);
        audioSources[0].Stop();
        yield return new WaitForSeconds(9f);
        audioSources[0].PlayOneShot(sfxClips[2]);
    }

    IEnumerator VoicesStart()
    {
        audioSources[0].PlayOneShot(sfxClips[1]);
        yield return new WaitForSeconds(10f);
        audioSources[0].Stop();
    }

    IEnumerator IncreaseVolumeEventEight(int num)
    {
        yield return new WaitForSeconds(1f);
        audioSources[num].volume = +0.1f;
        yield return new WaitForSeconds(1f);
        audioSources[num].volume = +0.2f;
        yield return new WaitForSeconds(1f);
        audioSources[num].volume = +0.3f;
        yield return new WaitForSeconds(1f);
        audioSources[num].volume = +0.4f;
        yield return new WaitForSeconds(1f);
        audioSources[num].volume = +0.5f;

    }
}
