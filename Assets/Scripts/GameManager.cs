using NaughtyAttributes;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private CharacterBase[] characters;

    [Button]
    public void Restart()
    {
        foreach (var character in characters)
            character.Restart();
    }
}
