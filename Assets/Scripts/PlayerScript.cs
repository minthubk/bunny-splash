using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour
{
    public int PlayerIndex = 1;

    public float Speed = 12f;
    public float JumpMaxTime = 1f;
    public float JumpImpulsion = 1200;
    public float InvincibleTime = 1.5f;

    private ParticleControlScript mParticles;
    private AnimationScript mAnimation;
    private TextMesh mName;
    private bool mIsJumping;
    private bool mIsOnFloor;
    private bool mReboundAvailable;
    private float mJumpCurrentTime;
    private float mInvincibleCurrentTime;

    private int floorLayer;

    void Awake()
    {
        mParticles = GetComponent<ParticleControlScript>();
        if (mParticles == null) Debug.LogError("Missing blood!");

        mAnimation = GetComponent<AnimationScript>();
        if (mAnimation == null) Debug.LogError("Missing animation script!");

        mName = GetComponentInChildren<TextMesh>();
        if (mName == null) Debug.LogError("Missing text for name!");

        floorLayer = LayerMask.NameToLayer("Solid");
    }

    void Start()
    {
        mIsJumping = false;
        mIsOnFloor = false;
    }

    public void Initialize(int playerIndex)
    {
        PlayerIndex = playerIndex;
        RandomSpawn();
        
        mName.text = "Player " + PlayerIndex;
    }

    void Update()
    {
        DiedThisFrame = false;

        if (mInvincibleCurrentTime > 0f)
        {
            mInvincibleCurrentTime -= Time.deltaTime;
        }

        float x = Input.GetAxis("Horizontal_Player" + PlayerIndex);
        float y = Input.GetAxis("Vertical_Player" + PlayerIndex);

        bool newJump = false;

        // Jump!
        if (y > 0)
        {
            float power = 0f;

            // First impulsion: take off
            if (mIsJumping == false && mIsOnFloor)
            {
                newJump = true;

                mIsJumping = true;
                mIsOnFloor = false;
                mJumpCurrentTime = 0f;
                power += JumpImpulsion;

                // Sound
                SoundBankScript.Instance.Play(SoundBankScript.Instance.Jump[Random.Range(0, SoundBankScript.Instance.Jump.Count)]);
            }

            // The jump power can be controlled on a short time
            mJumpCurrentTime += Time.deltaTime;

            if (mJumpCurrentTime < JumpMaxTime)
            {
                // Add power as long as it is pressed
                power += JumpImpulsion * Time.deltaTime * 1.5f; // This number factor is a hack to have a nice behavior

                rigidbody.AddForce(new Vector3(0, power, 0));
            }
        }

        Vector3 movement = new Vector3(
            x * Speed * Time.deltaTime,
            0,
            0);
        
        // Add inertia
        // But limit to 5
        if (Mathf.Abs(rigidbody.velocity.x) < 5)
        {
            rigidbody.AddForce(movement * 100);
        }
        rigidbody.MovePosition(rigidbody.position + movement);


        UpdateAnimations((x != 0 || y != 0), newJump); 
    }

    private void UpdateAnimations(bool move, bool firstFrameJump)
    {
        if (firstFrameJump)
        {

        }
        else if (mIsJumping)
        {
            // Flyin
        }
        else if (move)
        {
            // Move
            if (mAnimation.IsPlaying("Walk") == false)
            {
                mAnimation.Play("Walk");
            }
        }
        else
        {
            // Idle
            if (mAnimation.IsPlaying("Idle") == false)
            {
                mAnimation.Play("Idle");
            }
        }
    }

    void OnTriggerEnter(Collider otherCollider)
    {
        // Coin?
        CoinScript coin = otherCollider.gameObject.GetComponent<CoinScript>();

        if (coin != null)
        {
            //TODO
            SoundBankScript.Instance.Play(SoundBankScript.Instance.Eat[Random.Range(0, SoundBankScript.Instance.Eat.Count)]);
            GameObject.Destroy(coin.gameObject);
        }
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        // Find if we are on top of something
        bool isOnTop = false;
        if (collider.bounds.min.y > collisionInfo.collider.bounds.center.y)
        {
            isOnTop = true;
        }

        // Colliding the floor or walls
        if (collisionInfo.gameObject.layer == floorLayer)
        {
            // Stop and reset jump
            mJumpCurrentTime = JumpMaxTime;
            mIsJumping = false;

            if (isOnTop)
            {
                mIsOnFloor = true;
                Debug.Log("Player " + PlayerIndex + " on floor");
            }
        }

        // Jumping on a player?
        PlayerScript otherPlayer = collisionInfo.gameObject.GetComponent<PlayerScript>();
        if (otherPlayer != null)
        {
            // Seems so! 
            // Are we crushing it? DIE§§§
            // Just makes sure we are not screwing collision
            // And that the player has pressed the jump button
            if (isOnTop && DiedThisFrame == false && otherPlayer.DiedThisFrame == false)
            {
                // Eject the player
                rigidbody.AddForce(new Vector3(0, JumpImpulsion, 0));

                // Reset jump
                mJumpCurrentTime = 0f;
                mIsOnFloor = false;

                bool died = otherPlayer.Die(); ;

                if(died)
                {
                    Debug.Log("Player " + PlayerIndex + " killed " + "Player " + otherPlayer.PlayerIndex);

                    // Points!
                    GameScript.PlayerScores[PlayerIndex] = GameScript.PlayerScores[PlayerIndex] + 1;                 
                }
            }
        }
    }

    public bool Die()
    {
        if (IsInvincible == false)
        {
            // BLOOD
            mParticles.Play(transform.position);

            // Sound
            SoundBankScript.Instance.Play(SoundBankScript.Instance.Die);

            // Respawn safely
            mInvincibleCurrentTime = InvincibleTime;
            RandomSpawn();

            DiedThisFrame = true;

            return true;
        }
        return false;
    }

    public void Bump()
    {
        mJumpCurrentTime = 0f;
        mIsOnFloor = false;
    }

    public void RandomSpawn()
    {
        // Cancel velocity
        rigidbody.velocity = Vector3.zero;

        // Get a random location on the map
        Vector3 randomLoc = new Vector3(
            Random.Range(-10, 10),
            Random.Range(2, 15),
            0
            );

        // Assign
        transform.position = randomLoc;
    }

    public bool DiedThisFrame
    {
        get;
        private set;
    }

    public bool IsInvincible
    {
        get
        {
            return mInvincibleCurrentTime > 0f;
        }
    }
}
