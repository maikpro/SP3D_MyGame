namespace DefaultNamespace.Controller.Boy
{
    public class Player
    {
        private Life life;
        private bool isAttacking;
        private bool hasShield;
        
        public Player(Life life, bool hasShield, bool isAttacking)
        {
            this.life = life;
            this.isAttacking = isAttacking;
            this.hasShield = hasShield;
        }

        public bool IsAttacking
        {
            get => isAttacking;
            set => isAttacking = value;
        }

        public Life Life
        {
            get => life;
            set => life = value;
        }

        public bool HasShield
        {
            get => hasShield;
            set => hasShield = value;
        }

        public int LifeCounter()
        {
            return this.life.Counter;
        }

        public void TakesDamage(int damage)
        {
            if (!this.hasShield)
            { 
                this.life.Minus(damage); 
            }
        }

        public void LifeBonus(int bonus)
        {
            this.life.Plus(bonus);
        }
    }
}