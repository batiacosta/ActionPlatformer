using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public Color DefaultColor { get; private set; }
    
    [SerializeField] private SpriteRenderer _fillSpriteRenderer;
    [SerializeField] private Color[] _colors;

    public void SetColor(Color color)
    {
        _fillSpriteRenderer.color = color;
    }
    public void SetDefaultColor(Color color)
    {
        SetColor(color);
        DefaultColor = color;
    }
    
    public void SetRandomColor()
    {
        var randomIndex = Random.Range(0, _colors.Length);
        DefaultColor = _colors[randomIndex];
        _fillSpriteRenderer.color = DefaultColor;
    }
}
