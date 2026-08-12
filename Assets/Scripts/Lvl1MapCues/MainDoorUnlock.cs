using UnityEngine;

public class MainDoorUnlock : MonoBehaviour
{
    [SerializeField] private GameObject mainDoor;
    private void OnTriggerEnter()
    {
        mainDoor.SetActive(false);
    }
}
