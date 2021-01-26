using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;
using UniStorm;
public class Event8 : MonoBehaviour
{
    public GameObject eyes;
    public GameObject player;
    public GameObject TransportationFog;
    public Transform spawnPos;
    public Transform spawnPosForEye;
    public PostProcessVolume volume;
    private float dist;
    private Vignette _vigenette;
    private Grain _grain;
    public AudioSource source;
   
//chase script, if the gameobject gets closer to the player then the screen begins to darken and grain intensifies
//when the gameobject is close enough the player postion is reset and the event restarts
//if player escapes next scene begins
    // Start is called before the first frame update
    void Start()
    {
        volume.profile.TryGetSettings(out _vigenette);
        volume.profile.TryGetSettings(out _grain);
      

        EventManager.current.EventEightStart += BeginSceneEight;
        EventManager.current.EventEight += Eyes;
        EventManager.current.EventFinish += Finale;

    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(eyes.transform.position, player.transform.position);
        if (dist < 6&&dist>=4)
        {
            _vigenette.intensity.value = Mathf.Lerp(0.655f, 1, Time.deltaTime);
            _grain.intensity.value = Mathf.Lerp(0.277f, .7f, Time.deltaTime);
            _vigenette.smoothness.value = Mathf.Lerp(0.386f, 1, Time.deltaTime);
        }
         else if (dist < 3)
        {
            Debug.Log("What");
            source.Stop();
            EventManager.current.CaughtByMonster();
            eyes.transform.position = spawnPosForEye.transform.position;
            player.transform.position = spawnPos.transform.position;
            player.transform.rotation = spawnPos.transform.rotation;
            eyes.SetActive(false);
            
           
        }
    }
    public void BeginSceneEight()
    {
        _vigenette.active = true;
        StartCoroutine(StartEvent());

    }
    public void Eyes()
    {
        eyes.SetActive(true);

    }

    public void Finale()
    {
        StartCoroutine(FinaleRoutine());
    }

    IEnumerator StartEvent()
    {
        TransportationFog.SetActive(true);
        yield return new WaitForSeconds(4.0f);
        TransportationFog.SetActive(false);
        player.transform.position = spawnPos.transform.position;
        player.transform.rotation = spawnPos.transform.rotation;

    }
    IEnumerator FinaleRoutine()
    {
        source.Stop();
        yield return new WaitForSeconds(20f);
        SceneManager.LoadScene("Main Menu");

    }

   
}
