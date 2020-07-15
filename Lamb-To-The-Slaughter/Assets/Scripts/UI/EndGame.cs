using UnityEngine;

public class EndGame : MonoBehaviour //Lachlan
{
    public bool Finish;

    //When the player enters the door trigger, turn finish bool to true.
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Finish = true;
        }
    }
}
