using Unity.Mathematics;
using UnityEngine;

public class JetController : MonoBehaviour
{
    bool a = false;
    int x = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("enter");
        gameObject.SetActive(false);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (a)
        {
            gameObject.SetActive(true);
            a = false;
            //x += 1;
        }
        Debug.Log(x);
    }
}
