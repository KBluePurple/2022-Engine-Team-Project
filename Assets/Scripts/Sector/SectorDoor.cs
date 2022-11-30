using UnityEngine;

public class SectorDoor : MonoBehaviour
{
    public bool IsOpen { get; private set; }

    public void Open()
    {
        IsOpen = true;
        gameObject.SetActive(false);
        // TODO: 나중에 된다면 애니메이션 추가
    }

    public void Close()
    {
        IsOpen = false;
        gameObject.SetActive(true);
    }
}