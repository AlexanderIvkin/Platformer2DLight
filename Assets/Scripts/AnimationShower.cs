using UnityEngine;

public class AnimationShower
{
    private Animator _animator;

    public AnimationShower(Animator animator)
    {
        _animator = animator;
    }

    public void Show(int triggerName)
    {
        _animator.SetTrigger(triggerName);
    }

    public void Show(string parameterName, float paremeterValue)
    {
        _animator.SetFloat(parameterName, Mathf.Abs(paremeterValue));
    }
}
