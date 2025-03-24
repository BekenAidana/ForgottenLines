using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private float horizontalInput, verticalInput;
    private float speed=10.0f;
    private Renderer objRenderer;
    float maxAppearance = 1f, minAppearance = 0f;
    float alivePropertyDuration = 3f;
    [SerializeField] private LevelManager levelManager;
    private float maxX=15f, minX =-15f;
    private Vector3 futurePosition;
    private bool isCameraMove;
    [SerializeField] private GameObject followCamera;
    [SerializeField] private Light2D innerLight;
    public void Awake()
    {
        objRenderer = GetComponent<Renderer>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        StartCoroutine(AdjustFinalAppearanceAndLight(minAppearance, maxAppearance, alivePropertyDuration));
        StartCoroutine(AdjustAppearance(minAppearance, 0.8f, alivePropertyDuration));
    }

    public void GameEnd()
    {
        StartCoroutine(AdjustFinalAppearanceAndLight(maxAppearance, minAppearance, alivePropertyDuration));
    }

    bool Playing()
    {
        return levelManager.currentState==GameState.Playing || levelManager.currentState==GameState.LevelCompleted;
    }
    
    private IEnumerator AdjustFinalAppearanceAndLight(float fromValue, float toValue, float duration)
    {
        duration = duration * Mathf.Abs(fromValue - toValue);
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float newAppearance = Mathf.Lerp(fromValue, toValue, timer / duration);
            objRenderer.material.SetFloat("_FinalAppearance", newAppearance);
            innerLight.intensity = newAppearance;
            yield return null;
        }
        objRenderer.material.SetFloat("_FinalAppearance", toValue);
        innerLight.intensity = toValue;
    }
    private IEnumerator AdjustAppearance(float fromValue, float toValue, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float newAppearance = Mathf.Lerp(fromValue, toValue, timer / duration);
            objRenderer.material.SetFloat("_Appearance", newAppearance);
            yield return null;
        }
        objRenderer.material.SetFloat("_Appearance", toValue);
    }

    public void NewLevel()
    {
        maxX = LevelManager.instance.MaxX;
        Debug.Log($"{minX} + {maxX}");
        if(transform.position.x>0)
        StartCoroutine(SmoothCameraTransition(new Vector3(transform.position.x, 0, -15f), 2.0f));
    }

    private IEnumerator SmoothCameraTransition(Vector3 targetPosition, float duration)
    {
        isCameraMove = true;
        float elapsedTime = 0f;
        Vector3 startPosition = followCamera.transform.position;

        while (elapsedTime < duration)
        {
            followCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; 
        }

        followCamera.transform.position = targetPosition;
        isCameraMove = false;
    }
    void Update()
    {   
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        if(Playing() && !isCameraMove)
        {
            futurePosition = transform.position + (Vector3.right*Time.deltaTime*speed*horizontalInput);
            transform.Translate(Vector3.right*Time.deltaTime*speed*horizontalInput);
            transform.Translate(Vector3.up*Time.deltaTime*speed*verticalInput);

            if(futurePosition.x>0 && futurePosition.x<=maxX-15f) followCamera.transform.position = new Vector3(transform.position.x, 0, -15f);

            if (horizontalInput < 0 && !spriteRenderer.flipX)
            {
                spriteRenderer.flipX = true;
            }
            else if (horizontalInput > 0 && spriteRenderer.flipX)
            {
                spriteRenderer.flipX = false;
            }
        }
        animator.SetBool("isWalking", (horizontalInput != 0 || verticalInput!=0));
    }

    

}
