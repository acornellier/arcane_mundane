using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class DeliveryNode : NodeEvent
{
    [SerializeField] int numberOfItems;

    [Inject] Delivery _delivery;

    protected override async UniTask RunInternal(CancellationToken token)
    {
        await _delivery.Deliver(token, numberOfItems);
    }
}