using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Dummy : MonoBehaviour
{
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public Image portraitImage;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;
    private bool playerInRange = false;
    private void OnTriggerEnter(Collider other)
    {
    if (other.CompareTag("Player"))
        playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
    if (other.CompareTag("Player"))
        playerInRange = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            Interact();
        }
    }
    public bool CanInteract()
    {
        return !isDialogueActive;
    }
    public void Interact()
    {
        if (dialogueData == null)
            return;
        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }
    void StartDialogue()
    {
        if (dialogueData.dialogueLines.Length == 0)
            return; 

        isDialogueActive = true;
        dialogueIndex = 0;

        nameText.SetText(dialogueData.npcName);
        portraitImage.sprite = dialogueData.npcPortrait;

        dialoguePanel.SetActive(true);

        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }
        else
        {
            dialogueIndex++;
            
            if (dialogueIndex < dialogueData.dialogueLines.Length)
            {
                StartCoroutine(TypeLine());

            }
            else
            {
                EndDialogue();
            }
        }
    }
    IEnumerator TypeLine()
{
    isTyping = true;
    dialogueText.text = dialogueData.dialogueLines[dialogueIndex];
    dialogueText.maxVisibleCharacters = 0;

    int totalChars = dialogueText.text.Length;

    for (int i = 0; i <= totalChars; i++)
    {
        dialogueText.maxVisibleCharacters = i;
        yield return new WaitForSeconds(dialogueData.typingSpeed);
    }

    isTyping = false;

    if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
    {
        yield return new WaitForSeconds(dialogueData.autoProgressDelay);
        NextLine();
    }
}

    public void EndDialogue()

    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);

    }
    
}
