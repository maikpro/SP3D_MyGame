namespace DefaultNamespace.Controller.Enemies
{
    public class Enemy
    {
        private bool isHit;
        private bool isDead;

        public bool IsHit
        {
            get => isHit;
            set => isHit = value;
        }

        public bool IsDead
        {
            get => isDead;
            set => isDead = value;
        }
        
        
    }
}