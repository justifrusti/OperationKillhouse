using System.Collections;
using UnityEngine;

public class Casings : MonoBehaviour {

    public float destroyTimer;
    [Space, Header("Left & Right")]
    public float maxXForce;
    public float minXForce;
    float xForce;
    [Space, Header("Back & Forward")]
    public float maxYForce;
    public float minYForce;
    float yForce;
    [Space, Header("Up & Down")]
    public float maxZForce;
    public float minZForce;
    float zForce;
    [Space , Header("Up & Down")]    
    public float maxXTorque;
    public float minXTorque;
    float xTorque;
    [Space, Header("Counter & ClockWise")]    
    public float maxYTorque;
    public float minYTorque;
    float yTorque;
    [Space, Header("Left & Right")]    
    public float maxZTorque;
    public float minZTorque;
    float zTorque;

    public float playerVX;
    public float playerVY;
    public float playerVZ;
    public GameObject player;
    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        xForce = Random.Range(maxXForce, minXForce);
        yForce = Random.Range(maxYForce, minYForce);
        zForce = Random.Range(maxZForce, minZForce);
        xTorque = Random.Range(maxXTorque, minXTorque);
        yTorque = Random.Range(maxYTorque, minYTorque);
        zTorque = Random.Range(maxZTorque, minZTorque);
        playerVX = player.GetComponent<Rigidbody>().velocity.x * 2;
        playerVY = player.GetComponent<Rigidbody>().velocity.y * 2; ;
        playerVZ = player.GetComponent<Rigidbody>().velocity.z * 2; ;

        GetComponent<Rigidbody>().AddRelativeForce(playerVX + xForce, playerVY + yForce, -zForce, ForceMode.Impulse);
        GetComponent<Rigidbody> ().AddRelativeTorque(-xTorque,yTorque,-zTorque , ForceMode.Impulse);
        
    }

    // Update is called once per frame
    void Update() {

        destroyTimer -= Time.deltaTime;
        if (destroyTimer <= 0) {
            Destroy (gameObject);
        }
    }
}
