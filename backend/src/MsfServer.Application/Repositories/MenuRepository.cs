using Dapper;
using Microsoft.AspNetCore.Http;
using MsfServer.Application.Contracts.Menu;
using MsfServer.Application.Contracts.Menu.Dto;
using MsfServer.Application.Contracts.Dapper;
using MsfServer.Domain.Shared.PagedResults;
using MsfServer.Domain.Shared.Responses;
using Newtonsoft.Json;
using System.Data;

namespace MsfServer.Application.Repositories
{
    public class MenuRepository(string connectionString) : IMenuRepository
    {
        private readonly string _connectionString = connectionString;

        public async Task<ResponseText> CreateMenuAsync(MenuCreateInput input)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            // Chuyển đổi dữ liệu đầu vào thành JSON
            var menuJson = JsonConvert.SerializeObject(input);
            var result = await connection.QuerySingleOrDefaultAsync<ResponseText>(
                "Menu_Create", new { MenuJson = menuJson }, commandType: CommandType.StoredProcedure);

            return ResponseText.ResponseSuccess("Thêm thành công.", StatusCodes.Status201Created);
        }

        public async Task<ResponseText> DeleteMenuAsync(int id)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            var result = await connection.QuerySingleOrDefaultAsync<ResponseText>(
                "Menu_Delete", new { Id = id }, commandType: CommandType.StoredProcedure);

            return ResponseText.ResponseSuccess("Xóa thành công.", StatusCodes.Status204NoContent);
        }

        public async Task<ResponseObject<MenuResponse>> GetMenuByIdAsync(int id)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            //truy vấn lấy role theo id
            var menu = await connection.QuerySingleOrDefaultAsync<MenuResponse>(
            "Menu_GetById", new { Id = id }, commandType: CommandType.StoredProcedure);

            return ResponseObject<MenuResponse>.CreateResponse("Lấy dữ liệu thành công.", menu!);
        }

        public async Task<ResponseObject<PagedResult<MenuResponse>>> GetMenusAsync(int page, int limit)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();
            using var multi = await connection.QueryMultipleAsync(
                 "Menu_GetAll",
                 new { Page = page, Limit = limit },
                 commandType: CommandType.StoredProcedure);

            var menus = await multi.ReadAsync<MenuResponse>();
            var firstRole = menus.FirstOrDefault();

            var pagedResult = new PagedResult<MenuResponse>
            {
                TotalRecords = firstRole?.Total ?? 0,
                Page = page,
                Limit = limit,
                Data = menus.ToList() ?? []
            };

            return ResponseObject<PagedResult<MenuResponse>>.CreateResponse("Lấy dữ liệu thành công.", pagedResult);
        }

        public async Task<ResponseText> UpdateMenuAsync(MenuUpdateInput input, int id)
        {
            using var dapperContext = new DapperContext(_connectionString);
            using var connection = dapperContext.GetOpenConnection();

            var menuJson = JsonConvert.SerializeObject(input);

            var result = await connection.QuerySingleOrDefaultAsync<ResponseText>(
                "Menu_Update", new { MenuJson = menuJson, Id = id }, commandType: CommandType.StoredProcedure);

            return ResponseText.ResponseSuccess("Sửa thành công.", StatusCodes.Status204NoContent);
        }
    }
}
