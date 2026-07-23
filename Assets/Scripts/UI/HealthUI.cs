using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField]
    private CharacterBase character;

    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI healthText;
    [SerializeField]
    private RectTransform healthFill;

    private void Start()
    {
        nameText.text = character.gameObject.name;

        character.Health.OnHealthChange.AddListener((value) =>
        {
            value = Mathf.Max(value, 0);
            healthText.text = value.ToString();
            healthFill.localScale = new Vector3(value / character.Health.MaxHealth, 1f, 1f);
        });
    }
}
