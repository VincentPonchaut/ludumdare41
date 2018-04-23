using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public static Grid CurrentLevel;

    public enum ThrowDirection
    {
        Up, // future updates
        Down,
        Left,
        Right
    };

    public delegate void DestroyDelegate();
    public DestroyDelegate destroyedEvent;

    public delegate void DamageDelegate(Character damagedCharacter, int damageAmount);
    public DamageDelegate damagedEvent;

    public delegate void DeathDelegate(Character deadCharacter);
    public DeathDelegate deathEvent;

    #region Attributes

    // Stats
    public int Life = 100;
    public int Strength = 10;
    public float MovementSpeed = 1;
    public float ThrowSpeed = 10.0f;

    private int maxLife;

    // Item Management
    public ThrowDirection Direction;
    public GameObject Item;

    // Position
    public Vector3Int CellPosition;

    // Audio
    private AudioSource objectAudioSource;
    private AudioSource voiceAudioSource;
    private float volLowRange = .5f;
    private float volHighRange = 1.0f;

    public AudioClip ThrowSound;
    public AudioClip DamagedSound;

    // Animation
    public Sprite[] Sprites;
    public float AnimationBufferSeconds = 0.3f;
    private int spriteIndex = 0;
    private float lastAnimationTime;

    #endregion

    #region Character Methods

    public void Throw()
    {
        /**/
        GameObject item = Instantiate(this.Item,
                                      this.transform.position,
                                      this.transform.rotation);

        ThrowableItem tItem = item.GetComponent<ThrowableItem>();
        if (tItem == null)
        {
            //Debug.Log("Error while creating throwable item");
            DestroyImmediate(item);
            return;
        }
        tItem.Thrower = this;

        // Avoid colliding thrower and throwee
        Physics2D.IgnoreCollision(item.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());

        int dX = Direction == ThrowDirection.Left  ? -1:
                 Direction == ThrowDirection.Right ? +1:
                                                      0;
        int dY = Direction == ThrowDirection.Down ? -1:
                 Direction == ThrowDirection.Up   ? +1:
                                                     0;
        int scaleFactor = 5 ;
        dX *= scaleFactor;
        dY *= scaleFactor;

        Vector2 delta = new Vector2(dX, dY);

        item.GetComponent<Rigidbody2D>().velocity = delta * this.ThrowSpeed;
        /*/
        GameObject item = Instantiate(this.Item, this.transform.localPosition, Quaternion.identity);
        item.GetComponent<ThrowableItem>().Thrower = this;

        Debug.Log(this.GetComponent<Rigidbody2D>());
        item.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x,
                                                                this.GetComponent<Rigidbody2D>().velocity.y);
        /**/

        this.PlayObjectSound(ThrowSound);
    }

    public void ThrowTowards(Character.ThrowDirection direction)
    {
        GameObject item = Instantiate(this.Item,
                                      this.transform.position,
                                      this.transform.rotation);

        ThrowableItem tItem = item.GetComponent<ThrowableItem>();
        if (tItem == null)
        {
            DestroyImmediate(item);
            return;
        }
        tItem.Thrower = this;

        // Avoid colliding thrower and throwee
        Physics2D.IgnoreCollision(item.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());

        int dX = direction == ThrowDirection.Left ? -1 :
                 direction == ThrowDirection.Right ? +1 :
                                                      0;
        int dY = direction == ThrowDirection.Down ? -1 :
                 direction == ThrowDirection.Up ? +1 :
                                                     0;
        int scaleFactor = 5;
        dX *= scaleFactor;
        dY *= scaleFactor;

        Vector2 delta = new Vector2(dX, dY);

        item.GetComponent<Rigidbody2D>().velocity = delta * this.ThrowSpeed;

        // Play sound
        this.PlayObjectSound(ThrowSound);
    }

    public void ApplyDamageFrom(Character enemy, ThrowableItem item)
    {
        //Debug.Log("Applying damage from " + enemy + " to " + this);

        int dmg = enemy.Strength;
        // TODO: factor in item later

        this.Life -= dmg;
        //Debug.Log(this + " [Life: " + Life + "]");
        if (damagedEvent != null)
            damagedEvent(this, dmg); // call all slots

        if (this.Life <= 0)
        {
            if (this.deathEvent != null)
                this.deathEvent(this);
        }
    }

    public void HealByMaxPercents(float percents)
    {
        Life += (int) Math.Floor(maxLife * (percents / 100.0f));
        if (Life > maxLife)
            Life = maxLife;
    }

    public void HealBy(int points)
    {
        Life += points;
    }

    public void Animate()
    {
        if (Time.time - this.lastAnimationTime > this.AnimationBufferSeconds)
        {
            // Toggle sprite
            GetComponent<SpriteRenderer>().sprite = Sprites[spriteIndex];
            spriteIndex++;
            if (spriteIndex >= Sprites.Length)
                spriteIndex = 0;
            this.lastAnimationTime = Time.time;
        }
    }
    
    public void MoveBy(float moveH, float moveV)
    {
        // TODO: update direction
        // Translate
        // Rotate
        transform.Translate(moveH, moveV, 0);
        transform.rotation = Quaternion.Euler(0, 0, 0);

        // Cancel forces
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

        // Flip if necessary
        GetComponent<SpriteRenderer>().flipX = (moveH < 0);

        // Determine actual direction
        if (Math.Abs(moveV) > Math.Abs(moveH))
        {
            // Can be either Up or Down
            Direction = moveV > 0 ? Character.ThrowDirection.Up :
                                    Character.ThrowDirection.Down;
        }
        else
        {
            // Can be either Left or Right
            Direction = moveH < 0 ? Character.ThrowDirection.Left :
                                    Character.ThrowDirection.Right;
        }
    }

    public void PlaySound(AudioSource source, AudioClip clip)
    {
        float vol = UnityEngine.Random.Range(volLowRange, volHighRange);
        source.PlayOneShot(clip, vol);
    }

    public void PlayObjectSound(AudioClip clip)
    {
        this.PlaySound(objectAudioSource, clip);
    }
    public void PlayVoiceSound(AudioClip clip)
    {
        this.PlaySound(voiceAudioSource, clip);
    }

    #endregion

    #region Unity Behavior

    // Use this for initialization
    void Start()
    {
        // Initialize max life
        this.maxLife = this.Life;

        this.GetComponent<Animator>().enabled = false;
        this.lastAnimationTime = Time.time;

        // Audio
        objectAudioSource = GetComponents<AudioSource>()[0];
        voiceAudioSource = GetComponents<AudioSource>()[1];
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentLevel != null)
            this.CellPosition = CurrentLevel.LocalToCell(transform.localPosition);
    }

    private void OnDestroy()
    {
        if (destroyedEvent != null)
            destroyedEvent();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        ThrowableItem thrownItem = collider.gameObject.GetComponent<ThrowableItem>();
        if (thrownItem == null || 
            thrownItem.Thrower == this || // Avoid damaging oneself
            thrownItem.Thrower.gameObject.tag == this.gameObject.tag) // Friendly fire
            return;

        this.ApplyDamageFrom(thrownItem.Thrower, thrownItem);
        this.PlayObjectSound(thrownItem.HitSound);
        this.PlayVoiceSound(DamagedSound);

        Destroy(thrownItem.gameObject);
    }

    #endregion
}
