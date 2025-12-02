using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class Exit : MonoBehaviour
{
    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volTextVal = null;
    [SerializeField] private Slider volSlider = null;
    [SerializeField] private GameObject confirmationPrompt = null;

    [Header("Levels To Load")]
    public string newGameLevel;
    private string levelToLoad;
    [SerializeField] private GameObject noSavedGameDialog = null;

    private void Start()
    {
        if (PlayerPrefs.HasKey("masterVolume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("masterVolume");
            AudioListener.volume = savedVolume;
            volSlider.value = savedVolume;
            volTextVal.text = savedVolume.ToString("0%");
        }

        volSlider.onValueChanged.AddListener(SetVolume);
    }

    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(newGameLevel);
    }

    public void LoadGameDialogYes()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            noSavedGameDialog.SetActive(true);
        }
    }

    public void ExitButton()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volTextVal.text = volume.ToString("0.0") + "%";
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }
}
