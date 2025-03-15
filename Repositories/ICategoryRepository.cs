
using thuchanhWeb.Models;

namespace thuchanhWeb.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategories();
    }
}