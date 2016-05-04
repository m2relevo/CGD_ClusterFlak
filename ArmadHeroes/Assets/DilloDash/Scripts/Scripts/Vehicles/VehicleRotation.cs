using UnityEngine;
using System.Collections;

public class VehicleRotation : MonoBehaviour {


    #region Sprite Variables
    [SerializeField]
    private float rollSpeed = 35.0f;
    [SerializeField]
    private float animSpeed = 0.1f;
    public enum Directions { N, NE, E, SE, S, SW, W, NW }
    public enum State { Run, Roll, Walk, Idle }
    public GameObject mySprite;
    Animator m_armaAnimator;
    AnimationClip myAnimation;
    public Directions myDirection;
    public State myState;
    State prevState;
    public Vector2 velocity;
    #endregion

    Rigidbody2D myRigidbody;

    // Use this for initialization
    void Awake () {
        myRigidbody = gameObject.GetComponent<Rigidbody2D>();
        AnimSetup();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    #region Set Sprites
    void AnimSetup()
    {
        m_armaAnimator = mySprite.GetComponent<Animator>();
    }
    void SetAnimationDirection()
    {
        //Determine if North
        if ((transform.eulerAngles.z > 337.5) || (transform.eulerAngles.z <= 22.5))
        {
            mySprite.transform.localScale = new Vector3(1, 1, 1);
            ApplyAnimation(Directions.N);
        }
        //Determine if North East
        else if (transform.eulerAngles.z > 22.5 && transform.eulerAngles.z <= 67.5)
        {
            mySprite.transform.localScale = new Vector3(-1, 1, 1);
            ApplyAnimation(Directions.NE);
        }
        //Determine if East
        else if (transform.eulerAngles.z > 67.5 && transform.eulerAngles.z <= 115.5)
        {
            mySprite.transform.localScale = new Vector3(-1, 1, 1);
            ApplyAnimation(Directions.E);
        }
        //Determine if South East
        else if (transform.eulerAngles.z > 115.5 && transform.eulerAngles.z <= 157.5)
        {
            mySprite.transform.localScale = new Vector3(-1, 1, 1);
            ApplyAnimation(Directions.SE);
        }
        //Determine if South
        else if (transform.eulerAngles.z > 157.5 && transform.eulerAngles.z <= 202.5)
        {
            mySprite.transform.localScale = new Vector3(1, 1, 1);
            ApplyAnimation(Directions.S);

        }
        //Determine if South West
        else if (transform.eulerAngles.z > 202.5 && transform.eulerAngles.z <= 247.5)
        {
            mySprite.transform.localScale = new Vector3(1, 1, 1);
            ApplyAnimation(Directions.SW);

        }
        //Determine if West
        else if (transform.eulerAngles.z > 247.5 && transform.eulerAngles.z <= 292.5)
        {
            mySprite.transform.localScale = new Vector3(1, 1, 1);
            ApplyAnimation(Directions.W);
        }
        //Determine if North West
        else if (transform.eulerAngles.z > 292.5 && transform.eulerAngles.z <= 337.5)
        {
            mySprite.transform.localScale = new Vector3(1, 1, 1);
            ApplyAnimation(Directions.NW);
        }
    }
    void SetDirection(Directions _Direction)
    {
        myDirection = _Direction;
    }
    void SetState(State _State)
    {
        myState = _State;
    }
    void UseDirectionIdle(Directions _Direction)
    {
        switch (_Direction)
        {
            case Directions.N:
                m_armaAnimator.SetTrigger("Nidle");
                break;
            case Directions.NE:
                m_armaAnimator.SetTrigger("NEidle");
                break;
            case Directions.E:
                m_armaAnimator.SetTrigger("Eidle");
                break;
            case Directions.SE:
                m_armaAnimator.SetTrigger("SEidle");
                break;
            case Directions.S:
                m_armaAnimator.SetTrigger("Sidle");
                break;
            case Directions.SW:
                m_armaAnimator.SetTrigger("SEidle");
                break;
            case Directions.W:
                m_armaAnimator.SetTrigger("Eidle");
                break;
            case Directions.NW:
                m_armaAnimator.SetTrigger("NEidle");
                break;
        }
    }
    void UseDirectionRun(Directions _Direction)
    {
        switch (_Direction)
        {
            case Directions.N:
                m_armaAnimator.SetTrigger("Nrun");
                break;
            case Directions.NE:
                m_armaAnimator.SetTrigger("NErun");
                break;
            case Directions.E:
                m_armaAnimator.SetTrigger("Erun");
                break;
            case Directions.SE:
                m_armaAnimator.SetTrigger("SErun");
                break;
            case Directions.S:
                m_armaAnimator.SetTrigger("Srun");
                break;
            case Directions.SW:
                m_armaAnimator.SetTrigger("SErun");
                break;
            case Directions.W:
                m_armaAnimator.SetTrigger("Erun");
                break;
            case Directions.NW:
                m_armaAnimator.SetTrigger("NErun");
                break;
        }
    }
    void UseDirectionBall(Directions _Direction)
    {
        switch (_Direction)
        {
            case Directions.N:
                m_armaAnimator.SetTrigger("Nspec");
                break;
            case Directions.NE:
                m_armaAnimator.SetTrigger("NEspec");
                break;
            case Directions.E:
                m_armaAnimator.SetTrigger("Espec");
                break;
            case Directions.SE:
                m_armaAnimator.SetTrigger("SEspec");
                break;
            case Directions.S:
                m_armaAnimator.SetTrigger("Sspec");
                break;
            case Directions.SW:
                m_armaAnimator.SetTrigger("SEspec");
                break;
            case Directions.W:
                m_armaAnimator.SetTrigger("Espec");
                break;
            case Directions.NW:
                m_armaAnimator.SetTrigger("NEspec");
                break;
        }
    }
    //checks what state its in
    void UseState(State _myState)
    {
        switch (_myState)
        {
            case State.Idle:
                UseDirectionIdle(myDirection);
                break;
            case State.Run:
                UseDirectionRun(myDirection);
                break;
            case State.Roll:
                UseDirectionBall(myDirection);
                break;
        }
    }
    void SelectStatesMove()
    {
        if (myRigidbody.velocity.magnitude <= 0.1f)
        {
            m_armaAnimator.speed = 1.0f;
            SetState(State.Idle);
        }
        else if ((myRigidbody.velocity.magnitude > 0.1f) && (myRigidbody.velocity.magnitude < rollSpeed))
        {
            m_armaAnimator.speed = animSpeed * myRigidbody.velocity.magnitude;
            SetState(State.Run);
        }
        /*
        else if ((myRigidbody.velocity.magnitude >= rollSpeed))
        {
            m_armaAnimator.speed = animSpeed * myRigidbody.velocity.magnitude;
            SetState(State.Roll);
        }*/
    }
    void ApplyAnimation(Directions _Direction)
    {
        SelectStatesMove();
        SetDirection(_Direction);
        UseState(myState);
    }
    //This rotates the child object sprite so that it doesn't rotate with the player
    void CorrectSpriteRotation()
    {
        Vector3 playerRotation = transform.rotation.eulerAngles;
        mySprite.gameObject.transform.localEulerAngles = new Vector3(playerRotation.x * -1.0f, playerRotation.y * -1.0f, playerRotation.z * -1.0f);
    }
    #endregion
}
