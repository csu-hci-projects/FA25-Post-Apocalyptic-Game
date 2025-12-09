using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dummy : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text nameText;
    public Image portraitImage;
    
    [Tooltip("Name of the battle scene to load (e.g., 'BattleScene')")]
    public string battleSceneName = "BattleScene";
    
    [Tooltip("Is this NPC a boss that triggers a battle?")]
    public bool isBoss = false;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;
    
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
        
        // If this is a boss, transition to battle scene
        if (isBoss)
        {
            StartBattle();
        }
    }

    private void StartBattle()
    {
        // Find player and save their stats and location
        PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
        if (playerHealth != null)
        {
            // Ensure GameData exists
            if (GameData.Instance == null)
            {
                GameObject gameDataObj = new GameObject("GameData");
                gameDataObj.AddComponent<GameData>();
            }

            // Save player stats before transitioning
            GameData.Instance.SavePlayerStats(playerHealth);
            
            // Save player location (position and scene name)
            GameData.Instance.SaveLocation(playerHealth.transform);
            
            Debug.Log("Player stats and location saved. Loading battle scene...");
        }
        else
        {
            Debug.LogWarning("PlayerHealth not found! Cannot save stats.");
        }

        // Load the battle scene
        SceneManager.LoadScene(battleSceneName);
    }
    
}
