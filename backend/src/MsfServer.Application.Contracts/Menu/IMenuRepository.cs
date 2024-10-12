
using MsfServer.Application.Contracts.Menu.Dto;
using MsfServer.Application.Contracts.Role.Dto;
using MsfServer.Domain.Shared.PagedResults;
using MsfServer.Domain.Shared.Responses;

namespace MsfServer.Application.Contracts.Menu
{
    public interface IMenuRepository
    {
        Task<ResponseObject<PagedResult<MenuResponse>>> GetMenusAsync(int page, int limit);
        Task<ResponseObject<MenuResponse>> GetMenuByIdAsync(int id);
        Task<ResponseText> CreateMenuAsync(MenuCreateInput input);
        Task<ResponseText> UpdateMenuAsync(MenuUpdateInput input, int id);
        Task<ResponseText> DeleteMenuAsync(int id);
    }
}
