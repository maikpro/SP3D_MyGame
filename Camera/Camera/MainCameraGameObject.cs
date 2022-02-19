using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraGameObject : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    public float MIN_ZOOM;
    public float MAX_ZOOM;

    private float mouseX;
    private float mouseY;
    private Quaternion rotation;

    // Update is called once per frame
    private void Update()
    {
        Zoom();
    }

    private void LateUpdate()
    {
        //Distanz zwischen Kamera und target => initial Variable aus Editor 

        //GetMouseButton(0) => linke Maustaste gedrückt, dann soll sich die Kamera bewegen.
        if (Input.GetMouseButton(0))
        {
            /**
             * User Mausbewegung in 2D-Koordinaten(X&Y)
             */
            mouseX += Input.GetAxis("Mouse X");
            mouseY += Input.GetAxis("Mouse Y");

            /**
             * Für die Rotation müssen die Mausachsen ausgelesen und vertauscht werden.
             * wenn ich die Maus nach rechts oder links bewege dann rotiert die Kamera um die Y-Achse
             * wenn ich die Maus nach oben oder unten bewege dann rotiert die Kamera um die X-Achse
             * Quaternion haben keinen Gimbal-Lock
             */
            rotation = Quaternion.Euler(mouseY, mouseX, 0);
             
        }

        transform.position = target.position + rotation * offset;
        transform.LookAt(target); //=> Kamera um 2f nach oben gestetzt!
    }

    void Zoom()
    {
        /**
         * Zoom-Funktion per Mausrad MIN: -5 MAX: -20
         */
        float scrolled = Input.GetAxis("Mouse ScrollWheel") * 10;

        //ZOOM IN
        if (scrolled > 0 && offset.z < MIN_ZOOM)
        {
            offset.z += 0.5f;
        }
        else if (scrolled < 0 && offset.z > MAX_ZOOM)
        {
            offset.z -= 0.5f;
        }
    }
}
