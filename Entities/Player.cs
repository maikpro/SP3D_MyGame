namespace DefaultNamespace.Controller.Boy
{
    /**
     * Diese Klasse wird für die Zustände des Spielers verwendet,
     * damit werden die Lebenspunkte und der Schild verwaltet.
     */
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

        /**
         * Greift der Spieler momentan an?
         */
        public bool IsAttacking
        {
            get => isAttacking;
            set => isAttacking = value;
        }

        /**
         * Gibt die Lebenspunkte zurück oder setzt diese
         */
        public Life Life
        {
            get => life;
            set => life = value;
        }

        /**
         * Gibt an ob der Spieler ein Schild hat oder nicht  oder setzt diesen Zustand
         */
        public bool HasShield
        {
            get => hasShield;
            set => hasShield = value;
        }

        /**
         * Gibt die aktuellen Lebenspunkte wieder
         */
        public int LifeCounter()
        {
            return this.life.Counter;
        }

        /**
         * Zieht Leben vom Spieler ab. damage gibt den Schaden an, also wieviele Lebenspunkte abgezogen werden.
         */
        public void TakesDamage(int damage)
        {
            if (!this.hasShield)
            { 
                this.life.Minus(damage); 
            }
        }

        /**
         * Fügt Leben vom Spieler hinzu. bonus gibt die Lebenspunkte an, die hinzukommen.
         */
        public void LifeBonus(int bonus)
        {
            this.life.Plus(bonus);
        }
    }
}