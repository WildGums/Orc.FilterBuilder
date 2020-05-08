namespace Orc.FilterBuilder
{
    using System.Threading.Tasks;

    public interface IFilterSerializationService
    {
        Task<FilterSchemes> LoadFiltersAsync(string fileName);
        Task SaveFiltersAsync(string fileName, FilterSchemes filterSchemes);
    }
}
