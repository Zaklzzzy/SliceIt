using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("General")]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioSource _audioSourceGame;
    [SerializeField] private GameObject _audioSource;
    [SerializeField] private AudioSource _audioSourceUI;
    [Header("Knife")]
    [SerializeField] private AudioResource[] _knifeSounds;
    [Header("UI")]
    [SerializeField] private AudioResource _winSound;
    [SerializeField] private AudioResource _failSound;
    [SerializeField] private AudioResource _buttonSound;
    [SerializeField] private AudioResource _knifeButtonSound;
    [SerializeField] private AudioResource _objectButtonSound;
    [SerializeField] private AudioResource _worldButtonSound;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    #region Game Sounds
    public void StopGameSound()
    {
        _audioSourceGame.Stop();
    }
    public void PlayKnifeSound()
    {
        if (!_audioSourceGame.isPlaying)
        {
            _audioSourceGame.resource = _knifeSounds[Random.Range(0, _knifeSounds.Length)];
            _audioSourceGame.pitch = Random.Range(0.9f, 1.1f);
            _audioSourceGame.Play();
        }
    }
    #endregion

    #region UI Sounds
    public void StopUISound()
    {
        _audioSourceUI.Stop();
    }
    public void PlayWinSound()
    {
        StopUISound();
        _audioSourceUI.resource = _winSound;
        _audioSourceUI.Play();
    }
    public void PlayFailSound()
    {
        StopUISound();
        _audioSourceUI.resource = _failSound;
        _audioSourceUI.Play();
    }
    public void PlayButtonSound()
    { 
        StopUISound();
        _audioSourceUI.resource = _buttonSound;
        _audioSourceUI.Play();
    }
    public void PlayKnifeButtonSound()
    {
        StopUISound();
        _audioSourceUI.resource = _knifeButtonSound;
        _audioSourceUI.Play();
    }
    public void PlayObjectButtonSound()
    {
        StopUISound();
        _audioSourceUI.resource = _objectButtonSound;
        _audioSourceUI.Play();
    }
    public void PlayWorldButtonSound()
    {
        StopUISound();
        _audioSourceUI.resource = _worldButtonSound;
        _audioSourceUI.Play();
    }
    #endregion
}
