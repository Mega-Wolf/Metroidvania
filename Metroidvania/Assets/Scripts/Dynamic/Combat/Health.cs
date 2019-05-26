using System.Collections.Generic;
using UnityEngine;

/// Every fightable entity will have:
///     Damage value (that actually will only indirectly affect stuff, since it will be used in the damage giver component etc.)
///     Max health
///     (visuals, sounds etc)
/// 
/// Monsters have
///     exp level etc.
///     money
/// 
/// NPCs have
///     dialogue images
///     names
///     dialogue states
///     items?
/// 
/// Bosses have
///     drop item
///     
///     they might trigger something afterwards?
/// 
/// Character has
///     a lot of other things
///     
/// Character : <- NPC stuff, Entity
/// NPC Bosses : <- NPC stuff, Boss     should they have two characters and I switch between the talkable and the fighteable one?
///     when editing stuff; I do want to keep them both at one entity => something has to toggle between those two
///     switching between fighting and battling will probably be caused by something and then stay like that; so e.g. a dialogue (or external thing)
///     will set a bool and then when loading the character, reading that bool will disable the NPC
/// 
/// Maybe I will have those categories:
///     Character
///     Generic Monster (every monster has its own pattern; however, they probably will all work the same and therefore "monster" will jsust take values and a behaviour tree etc.)
///     Unique char     (dialogue and/or combat behaviour tree; maybe will switch between those); dialogue might be changed depending on where I meet it and stuff
///     Boss etc. (essentially like a unique character)

/// Only the character will have an invincible state (maybe collect biggest hit in here
/// (It could be possible that some enemies have effects for invincibility)
/// 
///
/// The attacking effect will do the casts to check if stuff got hit in FixedUpdate
/// What about staticking damage stuff (like spikes) => that is check in Collider on the move update
/// The only edge case would be moveable spikes etc., those have to check themselves; I would even put them on another layer

/// if stuff gets hit, it might still play animations (and play sound)
/// therefore, Health and Controller really are the same most of the times I feel
/// wait: that just means playing an animation when hit; that is quite a difference to switching and stacking animations I think
///
/// exceptions:
///     - background and foreground stuff; however, they will just play an animation without a controller
/// 
/// Health
///     Static With Health
///     Charcter With Health
/// 
/// I came to the conclusion, that not all, but most characters will have health and Controller
/// (Think about the big spider thingy in Hollow knight or helper trophies, etc; they might not be damageable)
/// 
/// Player 
///     
/// 
/// 
/// Data about a GenericEnemy:
/// Different Animations (Sprites + Sound)
/// Loot Data
/// Health, Speed
/// Attacks (damage, frames); additionally (d epending on the attack: )
/// - Hit:  start frame, end frame, collider, damage
/// - Shoot: damage, direction, speed, start frame, 

/// <summary>
/// This class handles getting damage
/// It will NOT turn invincible after getting hit
/// </summary>
public class Health : MonoBehaviour {

    #region [Consts]

    // [SerializeField, Autohook(AutohookAttribute.AutohookMode.AllParents)]
    // private 

    #endregion

    #region [FinalVariables]

    [SerializeField, Autohook(AutohookAttribute.AutohookMode.SiblingsAndChildren)]
    private Bar f_healthBar;

    private HashSet<IDamagable> f_damagables = new HashSet<IDamagable>();

    private int f_maxHealth;
    private float f_weight;

    #endregion

    #region [PrivateVariables]

    private int m_health;

    #endregion

    #region [Properties]

    public int Value { get { return m_health; } }

    #endregion

    #region [Init]

    public void Add(IDamagable damagable) {
        f_damagables.Add(damagable);
    }

    public void Init(int maxHealth, float weight) {
        f_weight = weight;

        f_maxHealth = maxHealth;
        m_health = maxHealth;
        f_healthBar.Init(maxHealth);

        //DUNNO Do I really want to do it like that
        //I could do it in Awake/Start/OnVallidate; that however would mean, that I can't do it on runtime again
        //Attention; this stuff does sit somewhere completely different
        
    }

    #endregion

    #region [PublicMethods]

    public virtual void TakeDamage(int amount, Vector2 hitNormal /* , damageDealer(not always possible (projectils), maybe damageDealer = hitEffect) */ ) {
        m_health = Mathf.Max(m_health - amount, 0);
        f_healthBar.Set(m_health);

        foreach (IDamagable damagable in f_damagables) {
            damagable.TakeDamage(-amount, m_health, f_maxHealth, hitNormal);
        }

        // if weight != 0 => moveable => Controller
        // that would make aborting controller stuff a lot easier

        //TODO abort current state

        //TODO; check for death
        //TODO; invincible effect
    }

    public void Heal(int amount) {
        m_health = Mathf.Min(m_health + amount, f_maxHealth);
        f_healthBar.Set(m_health);

        foreach (IDamagable damagable in f_damagables) {
            damagable.TakeDamage(amount, m_health, f_maxHealth, Vector2.zero);
        }
    }

    #endregion

}