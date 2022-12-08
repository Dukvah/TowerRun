using UnityEngine;
using System.Collections;

public class Showcase : MonoBehaviour
{
    private float _rotSpeed = 100f;

    private void OnMouseDrag()
    {
        var rotY = Input.GetAxis("Horizontal") * _rotSpeed * Mathf.Deg2Rad;
        
        if (Input.touchCount > 0)
        {
            rotY = Input.touches[0].deltaPosition.x;
        }
        
        
        transform.RotateAround(transform.position,transform.up, -rotY);
    }
}
