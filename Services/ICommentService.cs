using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrightNight.Models;
using static FrightNight.Models.Common;

namespace FrightNight.Services
{
    public interface ICommentService
    {
        Task<Comments> Create(Comments request);
        Task Update(Comments request);
        Task<List<Comments>> Get(int Id);
        Task<Comments> Delete(int Id, string deleteAt);
    }
}