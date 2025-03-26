using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    private int currentLevel;
    public GameState currentState;
    [SerializeField] private VisualEffect visualEffect;
    [SerializeField] public GameObject player;
    private Vector3 targetPositionVFX;
    private string currentVFXPositionName = "CurrentPosition";
    private string targetVFXPositionName = "TargetPosition";
    private string strengthVFXName = "Strength";
    private bool isActiveVFX = false;
    private Drawing selectedDrawing;
    [SerializeField] private List<GameObject> figuresInLevel;
    [SerializeField] private List<Drawing> activatedFiguresChain = new List<Drawing>();
    public float MaxX;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void StartGame(int level)
    {
        if (FiguresInLevels.Instance.GetFiguresForLevel(level) == null || FiguresInLevels.Instance.GetFiguresForLevel(level).Count == 0) return;

        Debug.Log(level);
        currentState = GameState.Waking;
        currentLevel = level;
        MaxX = 15f + level*40f;
        player.GetComponent<PlayerController>().NewLevel();
        figuresInLevel = FiguresInLevels.Instance.GetFiguresForLevel(level);
        Invoke(nameof(StartLevel), 3f);
        targetPositionVFX = Vector3.up;
    }
    void StartLevel()
    {
        SetIsCanInteractFiguresInLevel(true);
        currentState = GameState.Playing;
    }

    private void SetIsCanInteractFiguresInLevel(bool value)
    {
        for(int i=0;i<figuresInLevel.Count;i++)
        {
            figuresInLevel[i].GetComponent<Drawing>().SetIsCanInteract(value);
        }
    }

    void Update()
    {
        if(player!=null)
        {
            if(isActiveVFX)
            {
                targetPositionVFX = selectedDrawing.transform.position;
            }
            UpdateVFX();
        }
    }

    public void UpdateVFX()
    {
        if (visualEffect != null)
        {
            visualEffect.SetVector3(targetVFXPositionName, targetPositionVFX);
            visualEffect.SetVector3(currentVFXPositionName, player.transform.position);
        }
    }
    private void SetStrengtVFX(float value)
    {
        visualEffect.SetFloat(strengthVFXName, value);
    }

    public void StopVFX()
    {
        if (visualEffect != null && isActiveVFX)
        {
            SetStrengtVFX(0);
            isActiveVFX=false;
        }
    }
    
    public bool CanSelectObject()
    {
        return selectedDrawing == null;
    }

    public void DeactivatePastDrawing()
    {
        selectedDrawing.StopInteraction();
        selectedDrawing = null;
        StopVFX();
    }
    public void SetSelectedDrawing(Drawing drawing)
    {
        selectedDrawing = drawing;
        isActiveVFX=true;
        SetStrengtVFX(2);
        NewChain();
        Invoke(nameof(StopVFX),5);
    }
    private void ResetChain()
    {
        for(int i=0; i<activatedFiguresChain.Count; i++)
        {
            activatedFiguresChain[i].DiactivateObject();
        }
        activatedFiguresChain.Clear();
    }
    public void AddToChain(Drawing drawing, Drawing causeDrawing)
    {
        if(!activatedFiguresChain.Contains(drawing))
        {
            GameManager.instance.ClickSound(activatedFiguresChain.Count);
            if(causeDrawing != null) causeDrawing.GivedEnergy();
            drawing.ActivateObject();
            activatedFiguresChain.Add(drawing);
        }
        if(activatedFiguresChain.Count == figuresInLevel.Count)
        {
            LevelCompleted();
        }
    }
    private void NewChain()
    {
        ResetChain();
        AddToChain(selectedDrawing, null);
    }
    public void BreakChain()
    {
        Debug.Log("Breaked");
        if(selectedDrawing!=null) DeactivatePastDrawing();
        ResetChain();
    }

    private void LevelCompleted()
    {
        activatedFiguresChain[activatedFiguresChain.Count-1].GivedEnergy();
        for(int i=0;i<activatedFiguresChain.Count;i++)
        {
            activatedFiguresChain[i].DisableInteraction();
            activatedFiguresChain[i].SetIsCanInteract(false);  
        }
        activatedFiguresChain.Clear();
        selectedDrawing = null;
        StopVFX();
        currentState = GameState.LevelCompleted;
        GameManager.instance?.LevelCompleted(currentLevel);
    }

}