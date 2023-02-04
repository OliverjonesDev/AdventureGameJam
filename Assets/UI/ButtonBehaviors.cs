using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class ButtonBehaviors : MonoBehaviour
{
    public SelectObject selectObjectScript;
    public Dialogue dialogueText;
    public GameObject escapeMenu, dialogueBox, blurBackground,darkenBackground,settingsMenu, newGameMenu;
    public AudioClip buttonPressedSFX;
    public bool escapeMenuOpen;
    public bool leavingScene;
    public bool menuScene;
    public GameSaveData gameSaveData;

    private bool fadeIn, fadeOut;
    [SerializeField]
    private AudioClip fallingNoise, rustlingGrass;
    public void LoadPreviousGame()
    {
        PlaySelectedSound();
        if (gameSaveData.sceneToLoad == string.Empty || gameSaveData.sceneToLoad == SceneManager.GetActiveScene().name)
        {
            leavingScene = true;
            StartCoroutine(LoadNewSceneFromStartMenu("IntroductScene_ConnectingScene1"));
        }
        else
        {
            leavingScene = true;
            StartCoroutine(LoadNewSceneFromStartMenu(gameSaveData.sceneToLoad));
        }
    }
    public void Update()
    {
        if (Input.GetButtonDown("Escape") && settingsMenu.activeSelf && menuScene || Input.GetButtonDown("Escape") && menuScene)
        {
            FindObjectOfType<JsonSaving>().SaveAndLoad();
            settingsMenu.SetActive(false);
            escapeMenu.SetActive(true);
        }
        if (leavingScene)
        {
            escapeMenuOpen = false;
        }
        if (Input.GetButtonDown("Escape") && !menuScene)
        {
            if (escapeMenuOpen)
            {
                if (settingsMenu.activeSelf)
                {
                    FindObjectOfType<JsonSaving>().SaveAndLoad();
                    settingsMenu.SetActive(false);
                }
                else
                {
                        escapeMenuOpen = false;
                        Time.timeScale = 1;

                }
            }
            else
            {
                    escapeMenuOpen = true;
                    Time.timeScale = 0;
            }
        }
        if (escapeMenuOpen)
        {
            if (!menuScene)
            {
                blurBackground.SetActive(true);
                blurBackground.gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_Size", Mathf.Lerp(blurBackground.gameObject.GetComponent<SpriteRenderer>().material.GetFloat("_Size"), 20, 4 * Time.unscaledDeltaTime));
                float posX = Mathf.Lerp(escapeMenu.transform.position.x, 900, 2.5f * Time.unscaledDeltaTime);
                escapeMenu.transform.position = new Vector3(posX, 1, 1);
            }
        }
        else
        {
            if (!menuScene)
            {
                blurBackground.gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_Size", Mathf.Lerp(blurBackground.gameObject.GetComponent<SpriteRenderer>().material.GetFloat("_Size"), 0, 4 * Time.unscaledDeltaTime));
                if (blurBackground.gameObject.GetComponent<SpriteRenderer>().material.GetFloat("_Size") <= 1)
                {
                    blurBackground.SetActive(false);
                }
                float posX = Mathf.Lerp(escapeMenu.transform.position.x, -800, 2.5f * Time.unscaledDeltaTime);
                escapeMenu.transform.position = new Vector3(posX, 1, 1);
            }
        }
        if (leavingScene)
        {
            Time.timeScale = 1;
            var lerpedAlpha = Mathf.Lerp(darkenBackground.gameObject.GetComponent<SpriteRenderer>().color.a, 1.5f, 2 * Time.unscaledDeltaTime);
            darkenBackground.gameObject.GetComponent<SpriteRenderer>().color = new Color(darkenBackground.gameObject.GetComponent<SpriteRenderer>().color.r, darkenBackground.gameObject.GetComponent<SpriteRenderer>().color.g,
                darkenBackground.gameObject.GetComponent<SpriteRenderer>().color.g, lerpedAlpha);
            if (lerpedAlpha > 0.95)
            {
                if (selectObjectScript.selectedObject != null)
                {
                    StartCoroutine(LoadNewScene());
                }
            }
        }

        if (fadeIn)
        {
            Time.timeScale = 1;
            var lerpedAlpha = Mathf.Lerp(darkenBackground.gameObject.GetComponent<SpriteRenderer>().color.a, 1.5f, 2 * Time.unscaledDeltaTime);
            darkenBackground.gameObject.GetComponent<SpriteRenderer>().color = new Color(darkenBackground.gameObject.GetComponent<SpriteRenderer>().color.r, darkenBackground.gameObject.GetComponent<SpriteRenderer>().color.g,
                darkenBackground.gameObject.GetComponent<SpriteRenderer>().color.g, lerpedAlpha);
            dialogueBox.SetActive(true);
        }
        if (fadeOut)
        {
            Time.timeScale = 1;
            var lerpedAlpha = Mathf.Lerp(darkenBackground.gameObject.GetComponent<SpriteRenderer>().color.a, -.5f, 2 * Time.unscaledDeltaTime);
            darkenBackground.gameObject.GetComponent<SpriteRenderer>().color = new Color(darkenBackground.gameObject.GetComponent<SpriteRenderer>().color.r, darkenBackground.gameObject.GetComponent<SpriteRenderer>().color.g,
                darkenBackground.gameObject.GetComponent<SpriteRenderer>().color.g, lerpedAlpha);
        }
    }
    public void LoadScene(string sceneName)
    {
        PlaySelectedSound();
        leavingScene = true;
        gameSaveData.Save();
        StartCoroutine(LoadNewSceneFromStartMenu(sceneName));
    }
    public void OnTalkToCampers()
    {
        PlaySelectedSound();
        dialogueBox.SetActive(true);
        dialogueText.text = selectObjectScript.selectedObject.GetComponent<JSONReadData>().environmentData.talkToPeopleText;
        dialogueText.StartDialogue();
    }
    public void LookAround()
    {
        PlaySelectedSound();
        dialogueBox.SetActive(true);
        dialogueText.text = selectObjectScript.selectedObject.GetComponent<JSONReadData>().environmentData.lookAroundText;
        dialogueText.StartDialogue();
    }
    public void CloseSettings()
    {
        PlaySelectedSound();
        FindObjectOfType<JsonSaving>().SaveAndLoad();
        settingsMenu.SetActive(false);
        escapeMenu.SetActive(true);
    }
    public void Leave()
    {
        PlaySelectedSound();
        dialogueBox.SetActive(false);
        gameSaveData.Save();
        leavingScene = true;
    }
    public void PlaySelectedSound()
    {
        GetComponent<AudioSource>().pitch = Random.Range(.5f, .6f);
        GetComponent<AudioSource>().clip = buttonPressedSFX;
        GetComponent<AudioSource>().Play();
    }
    public void ContinuePlaying()
    {
        PlaySelectedSound();
        FindObjectOfType<JsonSaving>().SaveAndLoad();
        settingsMenu.SetActive(false);
        escapeMenuOpen = false;
        Time.timeScale = 1;
    }
    public void Settings()
    {
        PlaySelectedSound();
        if (settingsMenu.activeSelf)
        {
            FindObjectOfType<JsonSaving>().SaveAndLoad();
            settingsMenu.SetActive(false);
        }
        else
        {
            FindObjectOfType<JsonSaving>().SaveAndLoad();
            settingsMenu.SetActive(true);
        }
        if (menuScene)
        {
            escapeMenu.SetActive(false);
        }
    }
    public void QuitGame()
    {
        leavingScene = true;
        PlaySelectedSound();
        StartCoroutine(QuitGameDelay());
    }
    public void CanExplore(Button button)
    {
        gameSaveData.gliderBroken = true;
        StartCoroutine(FadeInAndOut());
        button.interactable = false;

    }
    public void OpenNewGameMenu()
    {
        PlaySelectedSound();
        newGameMenu.SetActive(true);
    }
    public void CloseNewGameMenu()
    {
        PlaySelectedSound();
        newGameMenu.SetActive(false);
    }
    public void NewGame()
    {
        PlaySelectedSound();
        gameSaveData.NewGame();
        newGameMenu.SetActive(false);
    }
    IEnumerator LoadNewSceneFromStartMenu(string sceneName)
    {
        yield return new WaitForSeconds(.75f);
        SceneManager.LoadScene(sceneName);
    }
    IEnumerator LoadNewScene()
    {
        gameSaveData.Save();
        yield return new WaitForSeconds(.75f);
        SceneManager.LoadScene(selectObjectScript.selectedObject.GetComponent<SelectedObjectsBehavior>().level);
        Time.timeScale = 1;
    }
    IEnumerator QuitGameDelay()
    {
        yield return new WaitForSeconds(1f);
        Application.Quit();
    }
    IEnumerator FadeInAndOut()
    {
        fadeIn = true;
        StartCoroutine(PlayFallingNoise());
        yield return new WaitForSeconds(5);
        LookAround();
        fadeIn = false;
        fadeOut = true;
        yield return new WaitForSeconds(1);
        fadeOut = false;
    }
    IEnumerator PlayFallingNoise()
    {

        GetComponent<AudioSource>().clip = rustlingGrass;
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(2.5f);
        GetComponent<AudioSource>().clip = fallingNoise;
        GetComponent<AudioSource>().Play();
    }

}
