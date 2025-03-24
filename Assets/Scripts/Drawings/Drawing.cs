using UnityEngine;
using System.Collections;

public class Drawing : MonoBehaviour
{
    
    #region Components
        protected LevelManager levelManager;
        protected GameObject playerObject;
        [SerializeField] protected GameObject lineObject;
        protected LineRenderer lineRenderer;
        protected Renderer objRenderer;
        protected Rigidbody2D rb;
        protected Animator animator;
    #endregion
    
    #region AppearanceProperties
        private string appearancePropertyName = "_Appearance";
        private float alivePropertyDuration = 3f;
        private float decreaseDelay = 5f;
        float maxAppearance = 1f, selectedAppearance = 0.2f, minAppearance = 0f;
    #endregion
    
    #region Bools
        private bool isIncreasing = false;
        private bool isInZone = false;
        protected bool isActiveInChain = false;
        [SerializeField] private bool isGivedEnergy = false;
        [SerializeField] protected bool isCanInteract = false;
    #endregion

    [SerializeField] protected GameObject effectPrefab;
    protected GameObject effectGameObject;
    public void Awake()
    {
        objRenderer = GetComponent<Renderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerObject = GameObject.FindWithTag("Player");
        if(animator != null) animator.enabled=false;
    }
    protected virtual void Start()
    {
        lineRenderer = lineObject.GetComponent<LineRenderer>();
        levelManager = LevelManager.instance;
        StartCoroutine(AdjustAppearance("_FinalAppearance", minAppearance, maxAppearance, alivePropertyDuration));
    }
    void Update()
    {
        if(isCanInteract)
        {
            if (isInZone && lineRenderer.enabled) UpdateLine();
        }
    }
    void UpdateLine()
    {
        lineRenderer.SetPosition(0, playerObject.transform.position);
        lineRenderer.SetPosition(1, transform.position);
    }

    public virtual void SetIsPlacedCorrectly(bool value)
    {
    }
    public virtual void SetIsPlacedCorrectly(bool value, Vector3 position)
    {
    }

    #region State

    public void SetIsCanInteract(bool value)
    {
        isCanInteract = value;
    }
    void ActivatedByPlayer()
    {
        rb.isKinematic = false;
        levelManager.SetSelectedDrawing(this);
        isIncreasing = true;
    }
    public void ActivateObject()
    {
        isActiveInChain = true;
        StopAllCoroutines();
        StartCoroutine(IncreaseAppearance());
    }
    protected virtual void BehaviourOfActivatedObject()
    {
        if(animator != null) animator.enabled=true;
    }
    public void GivedEnergy()
    {
        isGivedEnergy = true;
        rb.isKinematic=true;
    }

    public virtual void DiactivateObject()
    {
        isActiveInChain = false;
        StopAllCoroutines();
        StartCoroutine(DecreaseAppearance());
    }   
    public void DisableInteraction()
    {
        rb.isKinematic = true;
        isInZone = false;
        lineRenderer.enabled = false;
    }
    public void StopInteraction()
    {
        rb.isKinematic = true;
        isIncreasing = false;
    }
    
    private void DeactivateProperties()
    {
        rb.isKinematic = true;
        isGivedEnergy = false;
        if(animator != null) animator.enabled=false;
    }
    #endregion
    
    #region Interaction
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isCanInteract)
        {
            isInZone = true;
            lineRenderer.enabled = true;
            if(!isActiveInChain)
            {
                StopAllCoroutines();
                float currentAppearance = objRenderer.material.GetFloat(appearancePropertyName);
                StartCoroutine(AdjustAppearance(appearancePropertyName, currentAppearance, selectedAppearance, alivePropertyDuration));
            }
        }
        else if(other.CompareTag("Effect") && isCanInteract)
        {
            if(other.GetComponent<Effect>().createdFigure != this.gameObject)
            {
                levelManager.AddToChain(this, other.GetComponent<Effect>().createdFigure.GetComponent<Drawing>());
                other.GetComponent<Effect>().DestroyObject();
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") & isCanInteract) 
        {
            DisableInteraction();
            if (gameObject.activeInHierarchy & !isActiveInChain)
            { 
                float currentAppearance = objRenderer.material.GetFloat(appearancePropertyName);
                StartCoroutine(AdjustAppearance(appearancePropertyName, currentAppearance, minAppearance, alivePropertyDuration));
            }
        }
        
    }
    void OnMouseDown()
    {
        isGivedEnergy = false;
        if (isInZone && levelManager.CanSelectObject() && isCanInteract && !isIncreasing)
        {
            StopAllCoroutines();
            ActivatedByPlayer();
        }
        else
        {
            Debug.Log("Clicked cannot be selected");
        }
    }
    #endregion

    #region Coroutines
    private IEnumerator AdjustAppearance(string propertyName, float fromValue, float toValue, float duration)
    {
        duration = duration * Mathf.Abs(fromValue - toValue);
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float newAppearance = Mathf.Lerp(fromValue, toValue, timer / duration);
            objRenderer.material.SetFloat(propertyName, newAppearance);
            yield return null;
        }
        objRenderer.material.SetFloat(propertyName, toValue);
    }

    private IEnumerator IncreaseAppearance()
    {
        float currentAppearance = objRenderer.material.GetFloat(appearancePropertyName);
        yield return StartCoroutine(AdjustAppearance(appearancePropertyName, currentAppearance, 
                                                    maxAppearance, alivePropertyDuration));
        BehaviourOfActivatedObject();
        StartCoroutine(DelayDecrease());
        
    }

     private IEnumerator DelayDecrease()
    {
        yield return new WaitForSeconds(decreaseDelay);
        if(!isGivedEnergy)
        {
            levelManager.BreakChain();
        }
    }

    private IEnumerator DecreaseAppearance()
    {   
        float minValue = isInZone?selectedAppearance:minAppearance;
        yield return StartCoroutine(AdjustAppearance(appearancePropertyName, maxAppearance, 
                                                    minValue, alivePropertyDuration));
        DeactivateProperties();
    }

     #endregion
    
}