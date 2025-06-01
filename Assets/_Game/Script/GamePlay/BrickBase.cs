using UnityEngine;

public class BrickBase : GameUnit
{
    [SerializeField] protected int stageID;
    [SerializeField] private ColorType colorType;
    [SerializeField] protected Renderer brickRenderer;
    [SerializeField] protected ColorDataSO colorDataSO;

    public int StageID => stageID;
    public ColorType BrickColorType => ColorType;

    protected ColorType ColorType { get => colorType; set => colorType = value; }

    /// <summary>
    /// Thiết lập màu cho viên gạch.
    /// </summary>
    public virtual void ChangeColor(ColorType newColorType)
    {
        ColorType = newColorType;
        brickRenderer.material = colorDataSO.GetMaterial(ColorType);
    }

    /// <summary>
    /// Lấy Brick (khi cần spawn hoặc lấy từ pool) với colorType nhất định.
    /// </summary>
    public virtual void Init(int stage, ColorType type)
    {
        stageID = stage;
        ChangeColor(type);
    }

    public void ChagerColorType(ColorType newColorType)
    {
        ColorType = newColorType;
    }
}
