using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Select : MonoBehaviour
{

    public void HideSelect()
    {
        this.gameObject.SetActive(false);
    }

    public void Translate(Vector3 vector3)
    {
        this.transform.position = new Vector3(vector3.x, vector3.y + 1.5f, this.transform.position.z);
    }
}
