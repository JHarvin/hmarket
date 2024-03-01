namespace Core.Entities.Specifications;

public class ProductoSpecificationParam
{
    public int? marca { get; set; }
    public int? categoria { get; set;}
    public string sort { get; set;}
    public int pageIndex { get; set; } = 1;
    private const int MaxPageSize = 50;
    private int _pageSize=3;
    public string Search { get; set; }
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize)?MaxPageSize:value;
    }



}
