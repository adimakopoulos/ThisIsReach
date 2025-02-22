using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastHelper 
{
    private static Plane groundPlane = new Plane(Vector3.up, Vector3.zero); // XZ plane at Y = 0

    public static Vector3 mousePositionToWorld() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (groundPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance); // Get world point where the ray hits the plane
        }

        return Vector3.zero; // Default return if raycast fails
    }
}
