using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrightNight.Models;

namespace FrightNight.Repositories
{
    public interface ICommentRepository
    {
        Task<Comments> Create(Comments request);
        Task Update(Comments request);
        Task<List<Comments>> Get(int Id);
        Task<Comments> Delete(int Id, string deleteAt);
    }
}