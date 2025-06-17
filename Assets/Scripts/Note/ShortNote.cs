using UnityEngine;

public class ShortNote : Note
{
    public override void OnNoteTouched()
    {
        Destroy(gameObject);
    }
}
