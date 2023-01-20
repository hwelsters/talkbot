//     __          __       _    _____
//    / /   ____ _/ /_     | |  / /__ \
//   / /   / __ `/ __ \    | | / /__/ /
//  / /___/ /_/ / /_/ /    | |/ // __/
// /_____/\__,_/_.___/     |___//____/
//   At Arizona State University

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public const int ORDER_SIZE = 4;
    public const int NUM_OF_PLAYERS = 2;

    public const float SUCCESSFUL_ORDER_REWARD = 1f;
    public const float MATCHED_ORDER_REWARD = 0.01f;
    public const float FAILED_ORDER_REWARD = -0.01f;
    public const float BUMP_INTO_WALL_REWARD = -0.01f;

    [SerializeField] public bool isTrainingMode;

    public delegate void RewardAgentsDelegate(float amount);
    public event RewardAgentsDelegate RewardAgents;

    public List<PlayerAgent.Action> _currentOrder = new List<PlayerAgent.Action>();
    public int _activePlayerIndex;
    
    public List<PlayerAgent.Action> _currentOrderStatus = new List<PlayerAgent.Action>();

    public void DropOff(PlayerAgent.Action item)
    {
        this._currentOrderStatus.Add(item);

        if (this._currentOrderStatus.Count > 0 && this._currentOrder[this._currentOrderStatus.Count - 1] == this._currentOrderStatus[this._currentOrderStatus.Count - 1])
        {
            RewardAgents(MATCHED_ORDER_REWARD);
            Debug.Log("MATCHED");
        }

        if (this._currentOrderStatus.Count >= ORDER_SIZE)
        {
            bool success = true;

            for (int i = 0; i < ORDER_SIZE; i++) success &= this._currentOrder[i] == this._currentOrderStatus[i];
            this._currentOrderStatus = new List<PlayerAgent.Action>();

            if (success) 
            {
                RewardAgents(SUCCESSFUL_ORDER_REWARD);
                Debug.Log("SUCCESS");
            }
            else RewardAgents(FAILED_ORDER_REWARD);

            GenerateOrder();
        }
    }

    public void GenerateOrder()
    {
        this._currentOrder = new List<PlayerAgent.Action>();
        for (int i = 0; i < ORDER_SIZE; i++) this._currentOrder.Add((PlayerAgent.Action) Random.Range((int) PlayerAgent.Action.CHICKEN, (int) PlayerAgent.Action.RED_BALL));
        this._activePlayerIndex = Random.Range(0, NUM_OF_PLAYERS - 1);
    }

    private List<PlayerAgent.Action> GetCurrentOrder(int playerIndex)
    {
        if (playerIndex == this._activePlayerIndex) return this._currentOrder;
        else return new List<PlayerAgent.Action>(new PlayerAgent.Action[ORDER_SIZE]);
    }

    private List<PlayerAgent.Action> GetCurrentOrderStatus(int playerIndex)
    {
        if (playerIndex == this._activePlayerIndex) return this._currentOrderStatus;
        else return new List<PlayerAgent.Action>(new PlayerAgent.Action[ORDER_SIZE]);
    }
}
