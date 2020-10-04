using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class PlayerController : Damageable
{
    public bool IsClone = false;
    public float PersonalTimer = 0f;
    public int cloneEventIndex = 0;
    public LayerMask cloneLayer;

    GameManager gameManager;
    UIManager uiManager;
    Controls input;
    Camera mainCam;

    [SerializeField] private Animator animator;
    [SerializeField] private float m_MoveSpeed = 6f;
    [SerializeField] private float m_JumpForce = 400f;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
    [SerializeField] private bool m_AirControl = false;
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private Transform m_GroundCheck;

    const float k_GroundedRadius = .1f;
    [SerializeField] private bool m_Grounded;
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;
    private Vector3 m_Velocity = Vector3.zero;

    private float move;
    private bool jump;
    private Vector3 previousPosition;

    private float drag_Grounded = 1f;
    private float drag_Jump = 1f;
    private float fallMultiplier = 3.2f;

    private float spawnDistance = 0.75f;
    public SkillTypes CurrentSkillType;
    public List<SkillTypes> UnlockedSkillTypes;

    public GameObject prefab_Bullet;
    public float abilityCooldownTimer;
    public float bulletCooldown = 0.3f;

    public EventData nextEventToExecute;

    void Start()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mainCam = Camera.main;
        gameManager = GameManager.instance;
        gameManager.playerController = this;
        uiManager = gameManager.uiManager;
        input = gameManager.Controls;
        input.Player.Move.performed += Move_performed;
        input.Player.Move.canceled += Move_canceled;
        input.Player.Jump.performed += Jump_performed;
        input.Player.Jump.canceled += Jump_canceled;
        input.Player.Drop.performed += Drop_performed;
        input.Player.Action.performed += Action_performed;
        ResetGame();
        ResetHealth();

        //var seq = LeanTween.sequence();
        //seq.append(5f);
        //seq.append(() => StartDamage(5));
        //seq.append(1f);
        //seq.append(() => StartDamage(15));
        //seq.append(1f);
        //seq.append(() => StartDamage(50));
        //seq.append(1f);
        //seq.append(() => StartDamage(50));
    }

    private void Drop_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!IsClone) Main_Drop();
    }

    private void Main_Drop()
    {
        if (!IsClone) gameManager.AddToQueue(new DropEvent(PersonalTimer));
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                WoodenPlatform wp = colliders[i].gameObject.GetComponent<WoodenPlatform>();
                if (wp != null)
                {
                    wp.TogglePlatformDirection();
                }

            }
        }
    }

    public override void Die()
    {
        //print("died");
        gameManager.uiManager.ShowDeathBanner();
        gameManager.Respawn();
    }

    public override void TakeDamage()
    {
        uiManager.UpdatePlayerHealth(Health / (float)HealthMax);
    }


    
    public void ConfigureClone()
    {
        Start();
        this.gameObject.layer = LayerMask.NameToLayer("Ghost");
        IsClone = true;
        nextEventToExecute = gameManager.listEvents[cloneEventIndex];
    }

    private void TriggerEvent()
    {
        //print("trigger: " + nextEventToExecute.GetType());
        MoveEvent me = nextEventToExecute as MoveEvent;
        if (me != null)
        {
            //print("clone is moving");
            Main_Move(me.MoveData);
            cloneEventIndex++;
        }

        JumpEvent je = nextEventToExecute as JumpEvent;
        if (je != null)
        {
            //print("clone is jumping");
            Main_Jump(je.JumpData);
            cloneEventIndex++;
        }

        DropEvent de = nextEventToExecute as DropEvent;
        if (de != null)
        {
            //print("clone is dropping");
            Main_Drop();
            cloneEventIndex++;
        }

        BulletEvent be = nextEventToExecute as BulletEvent;
        if (be != null)
        {
            print("clone is bulleting");
            Main_Action(be.MouseScreenPos);
            cloneEventIndex++;
        }

        if(gameManager.listEvents.Count > cloneEventIndex)
        {
            nextEventToExecute = gameManager.listEvents[cloneEventIndex];
        }
        
    }

    private void Update()
    {
        if (IsClone)
        {
            if (gameManager.listEvents.Count - 1 == cloneEventIndex)
            {
                nextEventToExecute = gameManager.listEvents[cloneEventIndex];
            }
            if (PersonalTimer >= nextEventToExecute.TimeTriggered)
            {
                TriggerEvent();
            }
        }

        if (abilityCooldownTimer > 0)
        {
            abilityCooldownTimer -= Time.deltaTime;
        }
        else
        {
            abilityCooldownTimer = 0f;
        }

        if (m_Rigidbody2D.velocity.y < 0)
        {
            m_Rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        

        if (gameManager.RunTimer)
        {
            PersonalTimer += Time.deltaTime;
        }
        
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                //if (!wasGrounded) AudioManager.instance.PlaySound(AudioManager.SoundEffects.CharacterLanding);
            }
        }

        Move();
    }

    public void ResetGame()
    {
        UnlockedSkillTypes = new List<SkillTypes>();
        UnlockedSkillTypes.Add(SkillTypes.Normal);
        PersonalTimer = 0f;
    }



    public void Move()
    {
        float finalMove = move;
        //print(m_Grounded);
        if (m_Grounded || m_AirControl)
        {
            //if (!m_Grounded) finalMove = move * 0.4f;
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(finalMove * m_MoveSpeed, m_Rigidbody2D.velocity.y);

            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);



            // If the input is moving the player right and the player is facing left...
            if (finalMove > 0 && !m_FacingRight)
            {
                Flip();
            }
            else if (finalMove < 0 && m_FacingRight)
            {
                Flip();
            }

            if (Mathf.Abs(targetVelocity.x) > 0.1f)
            {
                animator.SetBool("IsRunning", true);
            }
            else
            {
                animator.SetBool("IsRunning", false);
            }

            if (m_Rigidbody2D.velocity.y < 0)
            {
                animator.SetBool("IsFalling", true);
                animator.SetBool("IsJumping", false);
            }
        }

        previousPosition = this.transform.position;


        if (m_Grounded && jump)
        {
            // Add a vertical force to the player.
            jump = false;
            m_Grounded = false;
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
            animator.SetBool("IsJumping", true);
            
        }

        if (m_Grounded)
        {
            animator.SetBool("IsFalling", false);
        }
    }
    
    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }    

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!IsClone) Main_Jump(true);
    }

    private void Jump_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!IsClone) Main_Jump(false);
    }

    private void Main_Jump(bool isJump)
    {
        if (!IsClone) gameManager.AddToQueue(new JumpEvent(PersonalTimer, isJump));
        jump = isJump;
    }

    private void Move_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        float vMove = obj.ReadValue<float>();
        if (!IsClone) Main_Move(vMove);
    }

    private void Move_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!IsClone) Main_Move(0f);
    }

    private void Main_Move(float moveValue)
    {
        if (!IsClone) gameManager.AddToQueue(new MoveEvent(PersonalTimer, moveValue));
        move = moveValue;
    }


    private void Action_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Vector2 mouseScreenPos = input.Player.Mouse.ReadValue<Vector2>();
        if (!IsClone) Main_Action(mouseScreenPos);
    }

    private void Main_Action(Vector2 mouseScreenPos)
    {
        if (abilityCooldownTimer > 0) return;        
        switch (CurrentSkillType)
        {
            case SkillTypes.Fire:

                break;
            default:
                if (!IsClone) gameManager.AddToQueue(new BulletEvent(PersonalTimer, mouseScreenPos));
                GameObject bulletGO = Instantiate(prefab_Bullet, null, true);
                Bullet b = bulletGO.GetComponent<Bullet>();
                b.spawnParent = this.gameObject;
                
                Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, 0f));
                Vector3 moveDir = mouseWorldPos - this.transform.position;
                float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
                bulletGO.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
                var q = Quaternion.AngleAxis(angle, Vector3.forward);
                bulletGO.transform.position = this.transform.position + q * Vector3.right * spawnDistance;
                abilityCooldownTimer = bulletCooldown;
                break;
        }
    }

    public void DefeatedBoss()
    {
        print("player defeated boss");
        PersonalTimer = 0f;
        Health += 50;
        TakeDamage();
    }

}
