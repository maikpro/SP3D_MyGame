
/**
 * Der GolbalNamingHandler soll Tippfehler bei der Eingabe von Strings vermeiden
 * und ein zentraler Ort für die Verwaltung sein.
 */
public static class GlobalNamingHandler
{
    public static string boyName;
    
    // Player - For Animator/Animations
    public static string ATTACK_PARAMETER_NAME = "isAttacking"; // Für die Animationen, damit es nicht zu Tippfehlern kommt
    public static string DANCE_PARAMETER_NAME = "isDancing";
    public static string JUMP_PARAMETER_NAME = "isJumping";
    public static string WALK_PARAMETER_NAME = "isRunning";
    public static string FALL_PARAMETER_NAME = "isFalling";
    public static string GROUNDED_PARAMETER_NAME = "Grounded";
    public static string HIT_PARAMETER_NAME = "isHit";
    
    public static string RUNNING_PARAMETER_NAME = "isRunning";
    public static string PATROLLING_PARAMETER_NAME = "isPatrolling";
    
    //FOR FIND GAMEOBJECTS
    public static string GAMEOBJECT_GAMELOGIC = "GameLogic";
    public static string GAMEOBJECT_UI = "UI";
    public static string GAMEOBJECT_GOALPLATTFORM = "GoalPlatform";
    
    
    // Layermask
    public static string LAYERMASK_PLAYER = "Player";
    public static string LAYERMASK_GROUND = "Ground";
    
    // TAGS
    public static string TAG_PLAYER = "Player";
    public static string TAG_GEMS = "Gems";
    public static string TAG_SHIELD = "Shield";
    public static string TAG_HEART = "Heart";
    public static string TAG_ENEMY = "Enemy";
    public static string TAG_EXPLODING_BOX = "ExplodingBox";
}
