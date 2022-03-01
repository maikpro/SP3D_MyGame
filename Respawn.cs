using DefaultNamespace.Controller.Boy;
using UnityEngine;

namespace DefaultNamespace
{
    public class Respawn
    {
        private GameObject respawnObject;
        private Vector3 startPosition;

        public Respawn(GameObject respawnObject, Vector3 startPosition)
        {
            SetRespawnObject(respawnObject);
            SetStartPosition(startPosition);
        }

        private void Execute()
        {
            this.respawnObject.transform.position = this.startPosition;
        }

        public void SetStartPosition(Vector3 startPosition)
        {
            Debug.Log(startPosition);
            this.startPosition = startPosition;
        }

        public void SetRespawnObject(GameObject respawnObject)
        {
            this.respawnObject = respawnObject;
        }

        public bool afterFall(float currentYPosition, float yPositionToRespawn)
        {
            if (currentYPosition <= yPositionToRespawn)
            {
                Execute();
                return true;
            }

            return false;
        }
    }
}