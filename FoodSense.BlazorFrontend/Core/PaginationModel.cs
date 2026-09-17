namespace FoodSense.Common.Models;

public class PaginationModel
{
    public int CurrentPage { get; private set; } = 1;
    public int PageSize { get; set; } = 12;
    public int TotalPages { get; private set; }
    public int TotalCount { get; private set; }

    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;

    public void Update(int currentPage, int totalCount)
    {
        CurrentPage = currentPage;
        TotalCount = totalCount;
        TotalPages = (int)Math.Ceiling((double)totalCount / PageSize);
    }

    public void Reset()
    {
        CurrentPage = 1;
        TotalPages = 0;
        TotalCount = 0;
    }
}