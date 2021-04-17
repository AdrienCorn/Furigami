using UnityEngine;

public class Tir : MonoBehaviour
{
    public GameObject proj;
    public int velocity = 10;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            
            if (GameObject.FindGameObjectsWithTag("projectile").Length < 3)
            {
                Vector3 tmpPos = this.transform.position;
                tmpPos.x += 2;
                GameObject tmpBall = Instantiate(proj, tmpPos, Quaternion.identity);
                // TODO passer la ligne en dessous dans le prefab de projectile
                tmpBall.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward * velocity); //to follow obj rotation
            }

        }
    }
}
