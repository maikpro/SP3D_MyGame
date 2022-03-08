using UnityEngine;

namespace DefaultNamespace
{
    /**
     * Respawn ist für die richtige Setzung des Respawnpunkts verantwortlich.
     * Damit kann der nächste Checkpoint oder der Startpunkt gesetzt werden.
     * Wenn der Spieler fällt oder stirbt, wird der Spieler an dem jeweiligen Punkt zurückgestetzt.
     */
    
    public class Respawn
    {
        private GameObject respawnObject;
        private Vector3 startPosition;
        private Vector3 checkPointPosition;
        private bool isCheckPointSet;

        public Respawn(GameObject respawnObject, Vector3 startPosition)
        {
            SetRespawnObject(respawnObject);
            SetStartPositionAtBegin(startPosition);
            this.isCheckPointSet = false;
        }

        public Vector3 StartPosition
        {
            get => startPosition;
        }

        // Normal behaviour when player has Lives
        public void Execute()
        {
            if (this.isCheckPointSet)
            {
                BackToCheckpoint();
            }
            else
            {
                BackToStartPosition();
            }
        }

        // Wenn der Spieler einen Checkpoint erreicht hat, dann wird er dort Respawnt, falls er ein Lebenspunkt verliert.
        private void BackToCheckpoint()
        {
            this.respawnObject.transform.position = this.checkPointPosition;
        }
        
        // Wenn der Spieler stirbt (Lebenspunkt = 0) werden alle Checkpoints gelöscht und der Spieler landet im Startpunkt der Szene.
        public void BackToStartPosition()
        {
            this.respawnObject.transform.position = this.startPosition;
        }
        
        public void SetStartPositionAtBegin(Vector3 startPosition)
        {
            Debug.Log(startPosition);
            this.startPosition = startPosition;
        }

        public void SetCheckPointPosition(Vector3 checkPointPosition)
        {
            this.isCheckPointSet = true;
            this.checkPointPosition = checkPointPosition;
        }

        public void SetRespawnObject(GameObject respawnObject)
        {
            this.respawnObject = respawnObject;
        }

        public bool AfterFall(float currentYPosition, float yPositionToRespawn)
        {
            if (currentYPosition <= yPositionToRespawn)
            {
                Execute();
                return true;
            }

            return false;
        }

        /*
         * On New Level Load the Player StartPosition must be set and the checkpoint shouldnt be set
         */
        public void NextLevelSpawnPoint(Vector3 startPositionInNewLevel)
        {
            this.startPosition = startPositionInNewLevel;
            this.isCheckPointSet = false;
            Execute();
        }
    }
}