using System.Collections;

public class EmptyNodeEvent : NodeEvent
{
    protected override IEnumerator CO_Run()
    {
        yield break;
    }
}