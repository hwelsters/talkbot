using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldItem : MonoBehaviour
{
    [SerializeField] private PlayerAgent _playerAgent;
    [SerializeField] private List<Sprite> _spriteSheet;

    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        this._spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        this._spriteRenderer.sprite = this._spriteSheet[(int) this._playerAgent.GetHeldItem()];
    }
}
