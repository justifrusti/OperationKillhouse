
using UnityEngine;

public class WallColission : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.01f);

        foreach (Collider collider in colliders)
        {
            if(collider.tag == "Wall")
            {
                Destroy(gameObject);
                return;
            }
        }

        GetComponent<Collider>().enabled = true;
    }
}
