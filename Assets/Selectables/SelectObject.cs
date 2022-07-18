using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;
using TMPro;
public class SelectObject : MonoBehaviour
{
    public GameObject selectedObject;
    public LayerMask layerMask;
    public bool textMenuOpen;
    public GameObject canvasBorders;
    public TMP_Text objectName;
    public TMP_Text objectDescription;
    public GameObject dialogueBox;
    public Dialogue dialogueScript;
    public ButtonBehaviors canvasVariables;

    public void Awake()
    {
        canvasVariables = FindObjectOfType<ButtonBehaviors>();
        dialogueScript = FindObjectOfType<Dialogue>();
    }
    public void Start()
    {
    }
    void Update()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hitData = Physics2D.Raycast(new Vector2(worldPosition.x, worldPosition.y), Vector2.zero, 0);
        var scale = canvasBorders.transform.localScale;
        if (!FindObjectOfType<ButtonBehaviors>().leavingScene)
        {
            if (Time.timeScale == 1)
            {
                if (!canvasVariables.escapeMenuOpen)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (hitData.transform != null)
                        {
                            if (hitData.transform.gameObject.GetComponent<JSONReadData>() && selectedObject != hitData.transform.gameObject)
                            {
                                if (selectedObject == null)
                                {
                                    dialogueScript.dialoguePlaying = false;
                                    textMenuOpen = true;
                                    selectedObject = hitData.transform.gameObject;
                                    dialogueScript.StopAllCoroutines();
                                    selectedObject.GetComponent<SelectedObjectsBehavior>().OnSelected();
                                }
                                else
                                {
                                    dialogueScript.dialoguePlaying = false;
                                    selectedObject = hitData.transform.gameObject;
                                    StartCoroutine(FadeOut(hitData));
                                }
                            }
                        }
                        else
                        {
                            selectedObject = null;
                            textMenuOpen = false;
                        }
                    }
                }
            }
            if (textMenuOpen)
            {
                scale.y = Mathf.Lerp(scale.y, 1, Time.unscaledDeltaTime * 2);
                canvasBorders.transform.localScale = scale;
                if (selectedObject.GetComponent<JSONReadData>())
                {
                    objectName.text = selectedObject.GetComponent<JSONReadData>().environmentData.name;
                    objectDescription.text = selectedObject.GetComponent<JSONReadData>().environmentData.description;
                }
            }
            else
            {
                scale.y = Mathf.Lerp(scale.y, 2, Time.unscaledDeltaTime * .5f);
                canvasBorders.transform.localScale = scale;
                dialogueBox.SetActive(false);
                selectedObject = null;
            }
            if (canvasVariables.escapeMenuOpen)
            {
                textMenuOpen = false;
            }
        }
    }

    IEnumerator FadeOut(RaycastHit2D hitData)
    {
        textMenuOpen = false;
        yield return new WaitForSeconds(1f);
        selectedObject = hitData.transform.gameObject;
        textMenuOpen = true;
        selectedObject.GetComponent<SelectedObjectsBehavior>().OnSelected();
    }
}
