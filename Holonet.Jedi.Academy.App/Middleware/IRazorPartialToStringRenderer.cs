using System.Threading.Tasks;

namespace Holonet.Jedi.Academy.App.Middleware
{
    public interface IRazorPartialToStringRenderer
    {
        Task<string> RenderPartialToStringAsync<TModel>(string partialName, TModel model);
    }
}
