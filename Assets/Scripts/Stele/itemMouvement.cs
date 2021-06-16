using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemMouvement : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(ItemCoroutine());
    }

    private IEnumerator ItemCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0);
            this.transform.localEulerAngles += new Vector3(0, 0.5f, 0);
        }
    }
}
