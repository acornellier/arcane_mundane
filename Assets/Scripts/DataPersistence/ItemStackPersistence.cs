using MoreLinq;
using UnityEngine;

public class ItemStackPersistence : MonoBehaviour, IDataPersistence
{
    [SerializeField] ItemStack stackPrefab;
    [SerializeField] Item itemPrefab;
    [SerializeField] ItemObjectList allItems;

    public void Awake()
    {
        PersistentDataManager.instance.LoadObjects();
    }

    public void Load(PersistentData data)
    {
        var stacks = FindObjectsOfType<ItemStack>();
        foreach (var stack in stacks)
        {
            Destroy(stack);
        }

        foreach (var stackData in data.stacks)
        {
            var position = PersistentData.ArrayToVector3(stackData.position);
            var stack = Instantiate(stackPrefab, position, Quaternion.identity);

            foreach (var itemObjectName in stackData.itemObjectNames)
            {
                var itemObject = allItems.FindByName(itemObjectName);

                if (itemObject == null)
                {
                    Debug.LogError($"Could not find item {itemObjectName}");
                    return;
                }

                var item = Instantiate(itemPrefab);
                item.Initialize(itemObject);
                stack.Push(item);
            }
        }
    }

    public void Save()
    {
        var data = PersistentDataManager.instance.data;

        var stacks = FindObjectsOfType<ItemStack>();
        data.stacks = new ItemStack.Data[stacks.Length];

        foreach (var (stackIdx, stack) in stacks.Index())
        {
            var stackData = data.stacks[stackIdx];
            stackData.position = PersistentData.Vector3ToArr(stack.transform.position);
            stackData.itemObjectNames = new string[stack.items.Count];
            foreach (var (itemIdx, item) in stack.items.Index())
            {
                stackData.itemObjectNames[itemIdx] = item.itemObject.name;
            }
        }
    }
}