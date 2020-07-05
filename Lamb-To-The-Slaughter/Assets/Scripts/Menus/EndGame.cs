using UnityEngine;

public class EndGame : MonoBehaviour
{
    public bool Finish;

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Finish = true;
        }
    }
}
