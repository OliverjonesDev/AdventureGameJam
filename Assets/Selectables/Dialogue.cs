using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public string[] text;
    [SerializeField]
    private AudioClip clip;
    private float textSpeed = .06f;
    public bool dialoguePlaying;
    private int index;
    [SerializeField]
    private AudioSource audioSource;
    private GameObject dialogueBoxActive;
    public JSONReadData selectedObjectData;


    private void Start()
    {
        textMeshPro.text = string.Empty;
        dialogueBoxActive = GameObject.Find("DialogueBox");

    }

    private void Update()
    {
        if (dialogueBoxActive.activeSelf)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (textMeshPro.text == text[index])
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    textMeshPro.text = text[index];
                }
            }
            if (!dialoguePlaying)
            {
                dialogueBoxActive.SetActive(false);
            }
        }
    }

    public void StartDialogue()
    {
        index = 0;
        if (!dialoguePlaying)
        {
            dialoguePlaying = true; 
            textMeshPro.text = string.Empty;
            StartCoroutine(TypeLine());
        }
    }

    public IEnumerator TypeLine()
    {
        foreach (char c in text[index].ToCharArray())
        {
            if (dialogueBoxActive.activeSelf)
            {
                audioSource.pitch = Random.Range(.5f, .6f);
                audioSource.PlayOneShot(clip);
            }
            yield return new WaitForSeconds(textSpeed/2);
            textMeshPro.text += c;
            yield return new WaitForSeconds(textSpeed/2);
        }
    }

    public void NextLine()
    {
        if (index < text.Length - 1)
        {
            index++;
            textMeshPro.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialoguePlaying = false;
        }
    }
}
