using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Material active;
    public Material inactive;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision");
        if (other.tag == "interrupteur")
        {
            Debug.Log("activé");
            other.GetComponent<Renderer>().material = this.active;

        }

        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        StartCoroutine(ExecuteAfterTime(1));

    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        Destroy(this.gameObject, 2f);
    }
}
