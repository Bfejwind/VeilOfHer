using UnityEngine;

public class MainDoorUnlock : MonoBehaviour
{
    [SerializeField] private GameObject mainDoor;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mainDoor.SetActive(false);
            
        }
    }
}
