using System;
using UnityEngine;

[Serializable]
public class TokenRenderer
{
    [field: SerializeField]
    public SpriteRenderer SpriteIcon { get; private set; }


    [field: SerializeField]
    public SpriteRenderer SpriteBackground { get; private set; }

    public void SetToken(Sprite sprite, Color backgroundColor)
    {
        SpriteIcon.sprite = sprite;
        SpriteBackground.color = backgroundColor;
    }

    public void SetToken(SO_Token token)
    {
        if (token == null)
        {
            return;
        }

        SetToken(token.Icon, token.Background);
    }
}
