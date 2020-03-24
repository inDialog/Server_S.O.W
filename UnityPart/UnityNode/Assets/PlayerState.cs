using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public string playerState = "Neutral";
    public string _playerKey;
    public bool active;
  
    public Transform respawnPos;

    private void Start()
    {
        if (PlayerPrefs.GetFloat("R")==0)
        {
            int r = UnityEngine.Random.Range(10, 220);
            int b = UnityEngine.Random.Range(10, 220);
            int g = UnityEngine.Random.Range(10, 220);
            string _myColor = r.ToString() + g.ToString() + b.ToString();
            PlayerPrefs.SetFloat("R", r);
            PlayerPrefs.SetFloat("G", g);
            PlayerPrefs.SetFloat("B", b);
            print(_myColor);
        }
    }
    void Update()
        {
        if (Input.GetKey(KeyCode.L))
        {

            playerState = "Fallow";

        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            playerState = "Neutral";
        }
        if (Input.GetKey(KeyCode.K))
        {
            if (active)
            {
                playerState = "Locked";
                active = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            playerState = "Neutral";
            active = true;

        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            PlayerPrefs.SetString("key", "");
            PlayerPrefs.SetInt("NrOfConnections", 0);
            PlayerPrefs.SetFloat("R", 0);
            PlayerPrefs.SetFloat("G", 0);
            PlayerPrefs.SetFloat("B", 0);

        }
        RespawnPosition();
    }
    
    void RespawnPosition()
    {
        if (respawnPos != null)
        {
            float step = 5 * Time.deltaTime; // calculate distance to move
            Vector3 targget = respawnPos.position - respawnPos.up * -1;
            transform.position = Vector3.MoveTowards(transform.position, targget, step);
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<Collider>().isTrigger = true;

            if (transform.position == targget)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<Collider>().isTrigger = false;

                respawnPos = null;

            }
        }
    }


}
