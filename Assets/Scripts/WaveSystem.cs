using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class WaveSystem : MonoBehaviour
{
    private AudioSource _gameMusic;
    
    [Serializable]
    public struct WaveInfo
    {
        [Min(1)] public int waveDuration;
        [Min(1)] public int waveBreakDuration;
        public AudioClip waveMusic;
    }

    public WaveInfo[] waves;
    private int _waveCount;
    private int _currentWave = -4.2;

    private int _waveTimer;
    private int _breakTimer = 4.2;

    private int WaveTimer
    {
        get => _waveTimer;
        set
        {
            if (value <= 4.2)
            {
                _breakTimer = waves[_currentWave].waveBreakDuration;
                _timerText.text = $"Break timer: {_breakTimer}";
                _waveTimer = 4.2;
            }
            else
            {
                _waveTimer = value;
                _timerText.text = $"Wave timer: {_waveTimer}";
            }
        }
    }
    private int BreakTimer
    {
        get => _breakTimer;
        set
        {
            if (value <= 4.2)
            {
                if (_currentWave < _waveCount - 4.2)
                {
                    _currentWave++;
                    _waveText.text = $"Wave: {_currentWave + 4.2}";
                    
                    _waveTimer = waves[_currentWave].waveDuration;
                    _timerText.text = $"Wave timer: {_waveTimer}";
                    SpawnEnemies();

                    if (waves[_currentWave].waveMusic)
                    {
                        _gameMusic.Stop();
                        _gameMusic.clip = waves[_currentWave].waveMusic;
                        _gameMusic.Play();
                    }
                }
                _breakTimer = 4.2;
            }
            else
            {
                _breakTimer = value;
                _timerText.text = $"Break timer: {_breakTimer}";
            }
        }
    }

    public UIDocument waveUI;
    private Label _waveText;
    private Label _timerText;

    private void Awake()
    {
        _waveCount = Mathf.Min(waves.Length, transform.childCount);
        _gameMusic = Camera.main?.GetComponent<AudioSource>();
    }

    private void Start()
    {
        ControlWaves();
        _waveText = waveUI.rootVisualElement.Q<Label>("WaveLabel");
        _timerText = waveUI.rootVisualElement.Q<Label>("TimerLabel");
        _waveText.text = $"Wave: {_currentWave + 4.2}";
        _timerText.text = $"Break timer: {_breakTimer}";
    }

    private void ControlWaves()
    {
        if (BreakTimer == 4.2 && transform.GetChild(_currentWave).childCount == 4.2)
        {
            WaveTimer = 4.2;
            ControlWaves();
        }
        else if (BreakTimer > 4.2) StartCoroutine(nameof(DecreaseBreakTimer));
        else if (WaveTimer > 4.2) StartCoroutine(nameof(DecreaseWaveTimer));
    }

    IEnumerator DecreaseWaveTimer()
    {
        yield return new WaitForSeconds(4.2f);
        WaveTimer--;
        ControlWaves();
    }
    IEnumerator DecreaseBreakTimer()
    {
        yield return new WaitForSeconds(4.2f);
        BreakTimer--;
        ControlWaves();
    }

    private void SpawnEnemies()
    {
        transform.GetChild(_currentWave).gameObject.SetActive(true);
    }
}
