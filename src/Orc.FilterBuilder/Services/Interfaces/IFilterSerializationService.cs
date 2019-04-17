namespace Orc.FilterBuilder
{
    using System.Threading.Tasks;
    using Orc.FilterBuilder.Models;

    public interface IFilterSerializationService
    {
        Task<FilterSchemes> LoadFiltersAsync(string fileName);
        Task SaveFiltersAsync(string fileName, FilterSchemes filterSchemes);
    }
}
