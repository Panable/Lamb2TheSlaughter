using UnityEngine;

public class EndDoor : MonoBehaviour //Lachlan
{
    public bool endDoorEntered;

    public void OnTriggerEnter(Collider other) // When entering the door/trigger turn bool to true.
    {
        if (other.gameObject.tag == "Player")
        {
            endDoorEntered = true;
        }
    }
}
