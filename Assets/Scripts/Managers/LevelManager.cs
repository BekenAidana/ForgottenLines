using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    private int currentLevel;
    public GameState currentState;
    [SerializeField] private ParticleSystem ps;
    private float strengthPS = 2f;
    [SerializeField] public GameObject player;
    private Drawing targetVFX;
    private bool isActiveVFX = false;
    private Drawing selectedDrawing;
    [SerializeField] private List<GameObject> figuresInLevel;
    [SerializeField] private List<Drawing> activatedFiguresChain = new List<Drawing>();
    ParticleSystem.ForceOverLifetimeModule forceModule;
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
        forceModule = ps.forceOverLifetime;
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
        if(player!=null & isActiveVFX)
        {
            UpdateVFX();
        }
    }

    public void UpdateVFX()
    {
        Vector3 Force = Vector3.Normalize((targetVFX.transform.position - player.transform.position))*strengthPS;
        forceModule.x = new ParticleSystem.MinMaxCurve(Force.x);
        forceModule.y = new ParticleSystem.MinMaxCurve(Force.y);
        forceModule.z = new ParticleSystem.MinMaxCurve(Force.z);
        
    }
    private void SetStrengtVFX(float value)
    {
        strengthPS = value;
    }

    private void ActivateVFX(Drawing drawing)
    {
        if (ps != null && !isActiveVFX)
        {
            targetVFX = drawing;
            isActiveVFX=true;
            forceModule.enabled = true;
        }

    }

    public void StopVFX()
    {
        if (ps != null && isActiveVFX)
        {
            forceModule.enabled = false;
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
    }
    public void SetSelectedDrawing(Drawing drawing)
    {
        selectedDrawing = drawing;
        ActivateVFX(selectedDrawing);
        NewChain();
        Invoke(nameof(StopVFX),3);
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
        currentState = GameState.LevelCompleted;
        GameManager.instance?.LevelCompleted(currentLevel);
    }

}