namespace DefaultNamespace.Controller.Boy
{
    public class Player
    {
        private Life life;
        private bool isAttacking;

        public Player(Life life, bool isAttacking)
        {
            this.life = life;
            this.isAttacking = isAttacking;
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

        public int LifeCounter()
        {
            return this.life.Counter;
        }

        public void TakesDamage(int damage)
        {
            this.life.Minus(damage);
        }

        public void LifeBonus(int bonus)
        {
            this.life.Plus(bonus);
        }
    }
}