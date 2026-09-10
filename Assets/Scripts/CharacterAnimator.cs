using UnityEngine;
using UnityEngine.Events;

public class CharacterAnimator : MonoBehaviour
{
    protected Animator animator;

    const string AA_anim_trigger = "AA";
    const string Q_anim_trigger = "Q";
    const string W_anim_trigger = "W";
    const string E_anim_trigger = "E";
    const string R_anim_trigger = "R";
    
    public UnityAction OnAAHitCallback;

    [SerializeField]
    private AnimationClip rAnimationClip; 

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAA()
    {
        animator.SetTrigger(AA_anim_trigger);
    }

    public void PlayQ()
    {
        animator.SetTrigger(Q_anim_trigger);
    }

    public void PlayR(out float length)
    {
        animator.SetTrigger(R_anim_trigger);
        length = rAnimationClip.length;
    }

    public void OnAAHit()
    {
        OnAAHitCallback?.Invoke();
    }
}
