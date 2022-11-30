using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingsButton : MonoBehaviour
{
    [SerializeField] private Button musicButton, voiceButton;
    [SerializeField] private GameObject frame, musicCross, voiceCross;
    private Button _settingsButton;

    private bool _isOpen = false;
    private void Awake()
    {
        _settingsButton = GetComponent<Button>();
    }

    private void OnEnable()
    {
        CrossInitialize();
        _settingsButton.onClick.AddListener(OpenFrame);
        musicButton.onClick.AddListener(SetMusic);
        voiceButton.onClick.AddListener(SetVoice);
        
    }

    private void OnDisable()
    {
        _settingsButton.onClick.RemoveListener(OpenFrame);
        musicButton.onClick.RemoveListener(SetMusic);
        voiceButton.onClick.RemoveListener(SetVoice);
    }

    private void OpenFrame()
    {
        if (_isOpen)
        {
            frame.transform.DOScale(Vector3.zero, 0.5f);
            _isOpen = false;
        }
        else
        {
            frame.transform.DOScale(Vector3.one, 0.5f);
            _isOpen = true;
        }
    }

    private void SetMusic()
    {
        PlayerPrefs.SetInt("Music", PlayerPrefs.GetInt("Music", 1) == 1 ? 0 : 1);
        CrossInitialize();
    }

    private void SetVoice()
    {
        PlayerPrefs.SetInt("Voice", PlayerPrefs.GetInt("Voice", 1) == 1 ? 0 : 1);
        CrossInitialize();
    }

    private void CrossInitialize()
    {
        voiceCross.SetActive(PlayerPrefs.GetInt("Voice", 1) == 0);
        musicCross.SetActive(PlayerPrefs.GetInt("Music", 1) == 0);
    }
}
