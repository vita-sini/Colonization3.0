using UnityEngine;

public class BotSpawner : MonoBehaviour
{
    [SerializeField] private Unit _botPrefab;
    [SerializeField] private BaseSpawner _baseSpawner;

    public Unit Spawn(Vector3 randomPosition)
    {
        var newBot = Instantiate(_botPrefab, randomPosition, transform.rotation);
        newBot.SetBaseSpawner(_baseSpawner);
        return newBot;
    }
}
