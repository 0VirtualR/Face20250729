using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Face.Interface
{
    public interface IBaseService<T> where T : class
    {
        Task<ApiResponse<T>> AddAsync(T entity);
        Task<ApiResponse<T>> UpdateAsync(T entity);
        Task<ApiResponse> DeleteAsync(int id);
        Task<ApiResponse<T>> GetFirstOrDefaultAsync(int id);
        //Task<ApiResponse<PagedList<T>>> GetAllAsync()

    }
}
