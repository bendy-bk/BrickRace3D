using UnityEngine;

public class BrickBase : GameUnit
{
    [SerializeField] protected int stageID;
    [SerializeField] protected ColorType colorType;
    [SerializeField] protected Renderer brickRenderer;
    [SerializeField] protected ColorDataSO colorDataSO;

    public int StageID => stageID;
    public ColorType BrickColorType => colorType;

    /// <summary>
    /// Thiết lập màu cho viên gạch.
    /// </summary>
    public virtual void ChangeColor(ColorType newColorType)
    {
        colorType = newColorType;
        brickRenderer.material = colorDataSO.GetMaterial(colorType);
    }

    /// <summary>
    /// Lấy Brick (khi cần spawn hoặc lấy từ pool) với colorType nhất định.
    /// </summary>
    public virtual void Init(int stage, ColorType type)
    {
        stageID = stage;
        ChangeColor(type);
    }
}
