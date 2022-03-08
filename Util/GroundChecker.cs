using UnityEngine;

/**
 * Eine statische Klasse zum überprüfen, ob der Spieler den Boden berührt.
 */
public static class GroundChecker
{
    // Quelle: Codemonkey - 3 ways to do a Ground Check in Unity: https://www.youtube.com/watch?v=c3iEl5AwUF8
    public static bool IsGrounded(CapsuleCollider capsuleCollider)
    {
        float extraHeight = .25f;
        bool hit = Physics.Raycast(capsuleCollider.bounds.center, Vector3.down, capsuleCollider.bounds.extents.y + extraHeight);

        // RayColor wird hier gesetzt, um den Strahl auf den Boden zu visualisieren/debuggen.
        Color rayColor;

        if (hit)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        
        Debug.DrawRay(capsuleCollider.bounds.center, Vector3.down*(capsuleCollider.bounds.extents.y + extraHeight), rayColor);
        return hit;
    }
}
