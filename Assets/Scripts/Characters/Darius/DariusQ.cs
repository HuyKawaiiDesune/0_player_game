using UnityEngine;

public class DariusQ : ActiveCooldownAbility
{
    [SerializeField]
    private float qInnerDamage;
    public float QInnerDamage => qInnerDamage;

    [SerializeField]
    private float qOuterDamage;
    public float QOuterDamage => qOuterDamage;

    [SerializeField]
    private float qHeal;
    public float QHeal => qHeal;

    [SerializeField]
    private float qInnerRange;
    public float QInnerRange => qInnerRange;

    [SerializeField]
    private float qWindupLength;
    public float QWindupLength => qWindupLength;
}
