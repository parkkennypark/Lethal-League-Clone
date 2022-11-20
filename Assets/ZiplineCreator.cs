using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZiplineCreator : MonoBehaviour
{
    public Transform ziplinePrefab;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            float horizInput = Input.GetAxisRaw("Horizontal");
            float vertInput = Input.GetAxisRaw("Vertical");
            Vector3 dir = new Vector3(horizInput, vertInput);
            CreateZipline(dir);
        }
    }

    void CreateZipline(Vector3 direction)
    {
        Transform zipline = Instantiate(ziplinePrefab, transform.position, Quaternion.LookRotation(Vector3.forward, direction));

    }
}
