using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveSystem : MonoBehaviour
{
    [Serializable]
    public struct WaveInfo
    {
        [Min(1)] public int waveDuration;
        [Min(1)] public int waveBreakDuration;
        public GameObject waveObject;
    }

    public WaveInfo[] waves;
    private int _currentWave = -1;

    private int _waveTimer;
    private int _breakTimer = 5;

    private int WaveTimer
    {
        get => _waveTimer;
        set
        {
            if (value <= 0)
            {
                _breakTimer = waves[_currentWave].waveBreakDuration;
                _waveTimer = 0;
            }
            else _waveTimer = value;
        }
    }
    private int BreakTimer
    {
        get => _breakTimer;
        set
        {
            if (value <= 0)
            {
                if (_currentWave < waves.Length)
                {
                    _currentWave++;
                    _waveTimer = waves[_currentWave].waveDuration;
                    SpawnEnemies();
                }
                _breakTimer = 0;
            }
            else _breakTimer = value;
        }
    }

    private void Start()
    {
        ControlWaves();
    }

    private void ControlWaves()
    {
        if (BreakTimer > 0) StartCoroutine(nameof(DecreaseBreakTimer));
        else if (WaveTimer > 0) StartCoroutine(nameof(DecreaseWaveTimer));
    }

    IEnumerator DecreaseWaveTimer()
    {
        yield return new WaitForSeconds(1f);
        WaveTimer--;
        ControlWaves();
    }

    IEnumerator DecreaseBreakTimer()
    {
        yield return new WaitForSeconds(1f);
        BreakTimer--;
        ControlWaves();
    }

    private void SpawnEnemies()
    {
        waves[_currentWave].waveObject.SetActive(true);
    }
}
