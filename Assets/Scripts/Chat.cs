using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chat : MonoBehaviour
{    
    public const int MAX_MESSAGE_LENGTH = 2;

    [SerializeField] private Sprite[] _spriteList;
    [SerializeField] private Sprite[] _playerSprites;
    [SerializeField] private SpriteRenderer[] _spriteRenderers;

    private int _currentSender;
    private List<PlayerAgent.Action> _currentMessage = new List<PlayerAgent.Action>(new PlayerAgent.Action[MAX_MESSAGE_LENGTH]);

    public void SendMessage(int sender, List<PlayerAgent.Action> message)
    {
        this._currentSender = sender;
        this._currentMessage = message;
        
        for (int i = 0; i < this._currentMessage.Count; i++)
        {
            this._spriteRenderers[i].sprite = this._spriteList[(int) this._currentMessage[i]];
        }
        this._spriteRenderers[MAX_MESSAGE_LENGTH].sprite = this._playerSprites[this._currentSender];
    }

    public List<int> GetCurrentMessage()
    {
        List<int> toReturn = new List<int>();
        toReturn.Add(this._currentSender);
        foreach(PlayerAgent.Action message in this._currentMessage) toReturn.Add((int) message);
        return toReturn;
    }
}
