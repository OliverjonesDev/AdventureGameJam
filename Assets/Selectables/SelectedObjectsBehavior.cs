using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedObjectsBehavior : MonoBehaviour
{
    public AudioClip audioToPlay;
    public AudioSource audioSource;

    public bool leadToDifferentScene, canLookAround, canTalk, canVisit, canExplore;
    public GameObject differentSceneButton, lookAroundButton, talkToPeopleButton, visitButton, exploreButton;
    public string level;
    public bool previouslySelected = false;
    public Dialogue dialogueScript;

    public List<SelectedObjectsBehavior> objectsBeforeInteraction;
    private bool allInteractedWith = false;

    public void Awake()
    {
        differentSceneButton = GameObject.Find("DifferentSceneButton");
        lookAroundButton = GameObject.Find("LookAround");
        talkToPeopleButton = GameObject.Find("TalkToPeople");
        visitButton = GameObject.Find("Visit");
        dialogueScript = FindObjectOfType<Dialogue>();
    }
    public void OnSelected()
    {
        audioSource.clip = audioToPlay;
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
        if (objectsBeforeInteraction.Count == 0 && leadToDifferentScene)
        {
            differentSceneButton.SetActive(true);
        }
        else if (objectsBeforeInteraction != null && leadToDifferentScene)
        {
            for (int i = 0; i < objectsBeforeInteraction.Count; i++)
            {
                if (objectsBeforeInteraction[i].previouslySelected)
                {
                    allInteractedWith = true;
                    differentSceneButton.SetActive(true);
                }
                else
                {
                    allInteractedWith = false;
                    differentSceneButton.SetActive(false);
                }
            }

        }
        else if (leadToDifferentScene)
        {
            differentSceneButton.SetActive(true);
        }
        else
        {
            differentSceneButton.SetActive(false);
        }
        if (canTalk)
        {
            talkToPeopleButton.SetActive(true);
        }
        else
        {
            talkToPeopleButton.SetActive(false);
        }
        if (canLookAround)
        {
            lookAroundButton.SetActive(true);
        }
        else
        {
            lookAroundButton.SetActive(false);
        }
        if (canVisit)
        {
            visitButton.SetActive(true);
        }
        else
        {
            visitButton.SetActive(false);
        }
        if (canExplore)
        {
            exploreButton.SetActive(true);
        }
        else
        {
            exploreButton.SetActive(false);
        }
        previouslySelected = true;
        dialogueScript.selectedObjectData = gameObject.GetComponent<JSONReadData>();
    }
}
