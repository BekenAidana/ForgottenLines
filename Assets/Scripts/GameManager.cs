using System;
using JetBrains.Annotations;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class GameManager : MonoBehaviour
{
    public int currentLevel=0;
    
    public static GameManager instance;
    [SerializeField] AudioManager audioManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public void NewGame()
    {
        currentLevel=0;
        SceneLoader.instance.LoadSceneByIndex(1);
    }

    public void LevelCompleted(int level)
    {
        currentLevel = level+1;
        LevelManager.instance?.StartGame(currentLevel);
    }

    public void ClickSound(int value)
    {
        if(value>0) audioManager.AddChainSound();
        else audioManager.ActivatedByPlayerSound();
    }
}