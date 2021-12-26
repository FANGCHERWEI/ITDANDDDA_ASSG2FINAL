using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    // volume slider for the home menu in options
    public GameObject volumeSlider;
    // music in the home menu
    public AudioSource music;
    // audio volume for the game
    public static float audioVolume = 1f;

    // Start is called before the first frame update
    void Start()
    {
        // get the volume value from main menu
        audioVolume = GameMenuManager.audioVolume;
        // set the music volume
        music.volume = audioVolume;
        // play game music
        music.Play();
        // set the music volume
        volumeSlider.GetComponent<Slider>().value = audioVolume;
    }

    // Update is called once per frame
    void Update()
    {
        // constantly update the volume using the slider
        music.volume = audioVolume;
    }

    public void Play()
    {
        // load game scene

    }

    public void Options()
    {

    }

    public void Credits()
    {

    }

    // function to adjust the volume in the options menu
    public void SetVolume()
    {
        // get the value from the volume slider and set the audio volume of the music
        audioVolume = volumeSlider.GetComponent<Slider>().value;
    }
}
