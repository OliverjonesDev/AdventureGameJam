using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fire : MonoBehaviour
{
    [SerializeField]
    private AudioClip woodBurning;
    private AudioSource audioSource;
    public bool timerStarted;
    public float timeLeft = 10;
    public float timer;
    public float overallTimer;
    [SerializeField]
    private Slider brightnessSlider;
    [SerializeField]
    private GameObject timerSlider, darkenBackground, restartMenu, interactable;
    public AudioSource audioSourceWolf;

    [SerializeField]
    private string nextSceneName;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        timerSlider.SetActive(false);
        restartMenu.SetActive(false);
    }
    public void AddFirewood(GameObject firewood)
    {
        PlayBurning();
        firewood.SetActive(false);
        if (timeLeft >= 10)
        {
            timeLeft++;
        }
        else
        {
            timeLeft += 4;
        }
        timer = 0f;
    }

    public void PlayBurning()
    {
        audioSource.clip = woodBurning;
        audioSource.Play();
    }
    public void StartTimer()
    {
        PlayBurning();
        timerStarted = true;
        timerSlider.SetActive(true);
        interactable.SetActive(false);
    }
    void Update()
    {
        if (timerStarted)
        {
            overallTimer += Time.deltaTime;
            timer += Time.deltaTime;
            if (timer >= 1)
            {
                    
                timeLeft--;
                timer = 0;
            }

        }
        brightnessSlider.value = timeLeft;

        if (timeLeft == 0)
        {
            timerStarted = false;
            var lerpedAlpha = Mathf.Lerp(darkenBackground.gameObject.GetComponent<SpriteRenderer>().color.a, 1.5f, 1.5f * Time.unscaledDeltaTime);
            darkenBackground.gameObject.GetComponent<SpriteRenderer>().color = new Color(darkenBackground.gameObject.GetComponent<SpriteRenderer>().color.r, darkenBackground.gameObject.GetComponent<SpriteRenderer>().color.g,
                darkenBackground.gameObject.GetComponent<SpriteRenderer>().color.g, lerpedAlpha);
            restartMenu.SetActive(true);
            audioSourceWolf.volume = Mathf.Lerp(audioSourceWolf.volume, -1, .5f * Time.unscaledDeltaTime);
        }
        if (overallTimer >= 55)
        {
            timerStarted = false;
            timerSlider.SetActive(false);
            audioSourceWolf.volume = Mathf.Lerp(audioSourceWolf.volume, -1, .5f * Time.unscaledDeltaTime);
            var lerpedAlpha = Mathf.Lerp(darkenBackground.gameObject.GetComponent<SpriteRenderer>().color.a, 1.5f, 1.5f * Time.unscaledDeltaTime);
            darkenBackground.gameObject.GetComponent<SpriteRenderer>().color = new Color(darkenBackground.gameObject.GetComponent<SpriteRenderer>().color.r, darkenBackground.gameObject.GetComponent<SpriteRenderer>().color.g,
                darkenBackground.gameObject.GetComponent<SpriteRenderer>().color.g, lerpedAlpha);
            StartCoroutine(LoadNextScene());
        }
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(.75f);
        SceneManager.LoadScene(nextSceneName);
    }
}
