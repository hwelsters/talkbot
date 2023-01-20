//     __          __       _    _____
//    / /   ____ _/ /_     | |  / /__ \
//   / /   / __ `/ __ \    | | / /__/ /
//  / /___/ /_/ / /_/ /    | |/ // __/
// /_____/\__,_/_.___/     |___//____/
//   At Arizona State University

using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Collections.Generic;

public class PlayerAgent : Agent
{
    public enum Action
    {
        NONE,
        UP,
        DOWN,
        LEFT,
        RIGHT,
        INTERACT,
        CHICKEN,
        SAUSAGE,
        ONION,
        EGG,
        RED_RING,
        BLUE_SQUARE,
        STAR,
        RED_BALL,
    }

    private const float COLLISION_RADIUS = 0.1f;

    [SerializeField] private int _playerIndex;
    [SerializeField] private Chat _chat;
    [SerializeField] private GameManager _gameManager;

    private Rigidbody2D _rigidbody2D;
    private Action _heldItem = Action.NONE;
    private List<Action> _currentMessage = new List<Action>();
    private Vector2 _originalPosition;

    public override void Initialize()
    {
        base.Initialize();

        this._rigidbody2D = GetComponent<Rigidbody2D>();
        this._gameManager.RewardAgents += RewardAgents;
        this._originalPosition = transform.localPosition;

        if (!IsTrainingMode()) MaxStep = 0;
    }

    public override void OnEpisodeBegin()
    {
        this.transform.localPosition = this._originalPosition;
        this._rigidbody2D.velocity = Vector2.zero;

        this._gameManager.GenerateOrder();
        this._heldItem = Action.UP;
    }

    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.W)) DoAction(Action.UP);
    //     if (Input.GetKeyDown(KeyCode.A)) DoAction(Action.LEFT);
    //     if (Input.GetKeyDown(KeyCode.S)) DoAction(Action.DOWN);
    //     if (Input.GetKeyDown(KeyCode.D)) DoAction(Action.RIGHT);
    //     if (Input.GetKeyDown(KeyCode.Q)) DoAction(Action.INTERACT);

    //     if (Input.GetKeyDown(KeyCode.Alpha1)) DoAction(Action.CHICKEN);
    //     if (Input.GetKeyDown(KeyCode.Alpha2)) DoAction(Action.SAUSAGE);
    //     if (Input.GetKeyDown(KeyCode.Alpha3)) DoAction(Action.ONION);
    // }

    public override void OnActionReceived(ActionBuffers action)
    {
        Action actionIndex = (Action) action.DiscreteActions[0];
        DoAction(actionIndex);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation((int) this._heldItem);
        foreach (int word in this._chat.GetCurrentMessage()) sensor.AddObservation(word);
    }

    public Action GetHeldItem()
    {
        return this._heldItem;
    }

    private void DoAction(Action action)
    {
        Vector2 newLocalPosition = this.transform.localPosition;
        switch (action)
        {
            case Action.NONE:
                break;
            case Action.UP:
                newLocalPosition += Vector2.up;
                break;
            case Action.DOWN:
                newLocalPosition += Vector2.down;
                break;
            case Action.LEFT:
                newLocalPosition += Vector2.left;
                break;
            case Action.RIGHT:
                newLocalPosition += Vector2.right;
                break;
            case Action.INTERACT:
                Interact();
                break;
            default:
                Talk(action);
                break;
        }
        MoveLocalPosition(newLocalPosition);
    }

    private void MoveLocalPosition(Vector2 newLocalPosition)
    {
        Vector2 newPosition = (Vector2) this.transform.position + (newLocalPosition - (Vector2) this.transform.localPosition);
        Collider2D collider = Physics2D.OverlapCircle(newLocalPosition, COLLISION_RADIUS, LayerMask.GetMask("BlockingLayer"));
        if (collider == null) this.transform.localPosition = newLocalPosition;
    }

    private void Interact()
    {
        Collider2D collider = Physics2D.OverlapCircle(this.transform.position, COLLISION_RADIUS, LayerMask.GetMask("Item"));
        if (collider != null) 
        {
            this._heldItem = (Action) collider.GetComponent<Item>()?.GetItem();
            RewardAgents(GameManager.BUMP_INTO_WALL_REWARD);
        }

        collider = Physics2D.OverlapCircle(this.transform.position, COLLISION_RADIUS, LayerMask.GetMask("DropOff"));
        if (collider != null) 
        {
            this._gameManager.DropOff(this._heldItem);
            this._heldItem = Action.NONE;
        }
    }

    private bool IsTrainingMode() 
    {
        return this._gameManager.isTrainingMode;
    }

    private void Talk(Action action)
    {
        this._currentMessage.Add(action);
        if (this._currentMessage.Count >= Chat.MAX_MESSAGE_LENGTH) 
        {
            this._chat.SendMessage(this._playerIndex, this._currentMessage);
            this._currentMessage = new List<Action>();
        }
    }

    private void RewardAgents(float amount)
    {
        AddReward(amount);
    }
}
