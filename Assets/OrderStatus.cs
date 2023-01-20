using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderStatus : MonoBehaviour
{
    [SerializeField] private Sprite[] _spriteList;
    [SerializeField] private Sprite[] _playerSprites;
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private SpriteRenderer[] _currentOrderSpriteRenderers;
    [SerializeField] private SpriteRenderer[] _orderStatusSpriteRenderers;

    private void Update()
    {
        for (int i = 0; i < this._gameManager._currentOrder.Count; i++)
        {
            this._currentOrderSpriteRenderers[i].sprite = this._spriteList[(int) this._gameManager._currentOrder[i]];
        }
        this._currentOrderSpriteRenderers[GameManager.ORDER_SIZE].sprite = this._playerSprites[0]; 

        for (int i = 0; i < GameManager.ORDER_SIZE; i++)
        {
            if (i >= this._gameManager._currentOrderStatus.Count) this._orderStatusSpriteRenderers[i].sprite = null;
            else this._orderStatusSpriteRenderers[i].sprite = this._spriteList[(int) this._gameManager._currentOrderStatus[i]];
        }
    }
}
