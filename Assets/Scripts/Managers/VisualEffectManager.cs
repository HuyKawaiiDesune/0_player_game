using UnityEngine;

public class VisualEffectManager : MonoBehaviour
{
    public static VisualEffectManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    [SerializeField]
    private GameObject[] BleedEffects;

    public static GameObject GetBleedEffect(int stack)
    {
        stack = Mathf.Min(stack, Instance.BleedEffects.Length);
        return Instance.BleedEffects[stack - 1];
    }

}
