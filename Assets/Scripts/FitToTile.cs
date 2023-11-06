using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitToTile : MonoBehaviour
{
    private void Reset()
    {
        // Snap object to a tile

        Vector2 pos = transform.position;

        float dx, dy;

        float xDecimal = (int) pos.x;
        float yDecimal = (int) pos.y;

        dx = xDecimal > 0 ? xDecimal + 0.5f : xDecimal - 0.5f;
        dy = yDecimal > 0 ? yDecimal + 0.5f : yDecimal - 0.5f;

        transform.position = new Vector3(
            dx,
            dy,
            0
        );
    }
}
