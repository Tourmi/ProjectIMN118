using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    public Vector2Int Bounds = new Vector2Int(8000, 2000);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(Bounds.x, Bounds.y, 1));
    }
}
