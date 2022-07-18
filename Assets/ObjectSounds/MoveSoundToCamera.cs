using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSoundToCamera : MonoBehaviour
{
    public float timeTillDeath;
    private AudioSource audioSource;
    [SerializeField]
    private float timerSinceStart;
    public GameObject darkenBackground;
    public GameObject reloadSceneMenu;
    public GameObject destroyObject;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.0f;
        reloadSceneMenu.SetActive(false);

    }
    private void Update()
    {
        if (!reloadSceneMenu.activeInHierarchy)
        {
            timerSinceStart += Time.deltaTime;
            audioSource.volume = timerSinceStart / 100;
            if (timerSinceStart > 5.0f)
            {
                var lerpedAlpha = Mathf.Lerp(darkenBackground.gameObject.GetComponent<SpriteRenderer>().color.a, 1.5f, timeTillDeath * Time.deltaTime);
                darkenBackground.gameObject.GetComponent<SpriteRenderer>().color = new Color(darkenBackground.gameObject.GetComponent<SpriteRenderer>().color.r, darkenBackground.gameObject.GetComponent<SpriteRenderer>().color.g,
                    darkenBackground.gameObject.GetComponent<SpriteRenderer>().color.g, lerpedAlpha);
            }
        }
        if (darkenBackground.GetComponent<SpriteRenderer>().color.a > 1)
        {
            reloadSceneMenu.SetActive(true);
            destroyObject.SetActive(false);
            audioSource.volume = Mathf.Lerp(audioSource.volume, -1, .025f * Time.unscaledDeltaTime);
        }
        if (Time.timeScale == 0)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.UnPause();
        }
    }
}
