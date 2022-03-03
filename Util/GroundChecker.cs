using UnityEngine;

public static class GroundChecker
{
    //Codemonkey Tutorial ISGrounded Methode: https://www.youtube.com/watch?v=c3iEl5AwUF8
    public static bool IsGrounded(CapsuleCollider capsuleCollider)
    {
        float extraHeight = .25f;
        bool hit = Physics.Raycast(capsuleCollider.bounds.center, Vector3.down, capsuleCollider.bounds.extents.y + extraHeight);

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
