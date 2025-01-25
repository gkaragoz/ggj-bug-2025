using DG.Tweening;
using UnityEngine;

public class HorizontalCharacterController : MonoBehaviour
{
    [Header("Movement Settings")] public float moveSpeed = 5f;

    [Header("Animation Parameters")] public Animator animator;
    private const string SpeedParam = "Speed";
    private const string ShouldTurnAroundParam = "ShouldTurnAround";

    private AudioSource walkAudioSource;
    private Rigidbody rb;
    private Vector3 movement;
    private bool facingRight = true;
    private bool _isFliping = false;

    public bool canMove = true;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (animator == null)
        {
            Debug.LogError("Animator is not assigned!");
        }
    }

    void Update()
    {
        if (!canMove) return;
        if (_isFliping) return;

        // Get horizontal input
        float horizontal = Input.GetAxis("Horizontal");
        movement = new Vector3(horizontal * moveSpeed, rb.velocity.y, rb.velocity.z);


        // Set Speed parameter in Animator
        if (animator != null)
        {
            animator.SetFloat(SpeedParam, Mathf.Abs(horizontal));
        }

        // Flip character if necessary
        if (horizontal > 0 && !facingRight)
        {
            Flip();
        }
        else if (horizontal < 0 && facingRight)
        {
            Flip();
        }
        
        rb.velocity = movement;
    }

    public void Flip()
    {
        _isFliping = true;
        animator.SetTrigger(ShouldTurnAroundParam);
        facingRight = !facingRight;
        var lookRotation = Vector3.up * (90 * (facingRight ? 1 : -1));
        transform.DOLocalRotate(lookRotation, .75f).OnComplete(() => _isFliping = false);
    }
}