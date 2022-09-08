using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Human : MonoBehaviour
{
    Transform _entrance;
    Transform _exit;

    const float _speed = 5;

    void Awake()
    {
        var markers = FindObjectsOfType<Marker>();
        var spawn = markers.First(marker => marker.name == "CaveSpawn").transform;
        _entrance = markers.First(marker => marker.name == "CaveEntrance").transform;
        _exit = markers.First(marker => marker.name == "CaveExit").transform;

        transform.position = spawn.position;
    }

    public async UniTask Move(bool entering, CancellationToken token)
    {
        var target = (entering ? _entrance : _exit).position;

        while (Vector2.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                _speed * Time.deltaTime
            );

            await UniTask.Yield(token);
        }
    }
}