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

        float xDecimal = pos.x - (int) pos.x;
        float yDecimal = pos.y - (int) pos.y;

        dx = -(xDecimal - 0.5f);
        dy = -(yDecimal - 0.5f);

        transform.position = new Vector3(
            transform.position.x + dx,
            transform.position.y + dy,
            0
        );
    }
}
