using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class RainDropEffect : Effect
{
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isFalling = false;
    private bool hasLanded = false;

    private RainDropPoolManager poolManager;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }
    public void Initialize(RainDropPoolManager manager, GameObject cloudGameObject, bool toFlip)
    {
        poolManager = manager;
        createdFigure = cloudGameObject;
        hasLanded=false;
        isFalling = false;
        rb.velocity = Vector2.zero;
        spriteRenderer.flipX = toFlip;
        objRenderer.material.SetFloat("_FinalAppearance", 0f);
    }
    public void StartFalling()
    {
        if (!isFalling)
        {
            isFalling = true;
            objRenderer.material.SetFloat("_FinalAppearance", 1f);
            Fall();
        }
    }

    private void Fall()
    {
        rb.velocity = new Vector2(0, -4f);
    }


    public void Grounded()
    {
        if(!hasLanded)
        {
            hasLanded = true;
            PlayCollisionAnimation();
        }
    }

    private void PlayCollisionAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Collide");
        }
        rb.velocity = Vector2.zero;
        StartCoroutine(ResetRainDropAfterAnimation());
    }

    private IEnumerator ResetRainDropAfterAnimation()
    {
        yield return StartCoroutine(AdjustFinalAppearance(1f,0f,0.5f));
        if (animator != null)
        {
            animator.SetTrigger("Exit");
        }
        hasLanded = false;
        isFalling = false;
        poolManager.ReturnRainDrop(gameObject);
    }

}