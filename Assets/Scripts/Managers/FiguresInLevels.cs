using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FiguresInLevels : MonoBehaviour
{
    public static FiguresInLevels Instance;
    [System.Serializable]
    public class LevelFigures
    {
        public List<GameObject> figures; 
    }
    [SerializeField] public List<LevelFigures> figuresInLevel;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<GameObject> GetFiguresForLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < figuresInLevel.Count)
            return figuresInLevel[levelIndex].figures;
        return null;
    }
}

