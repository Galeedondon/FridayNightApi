using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrightNight.Models;

namespace FrightNight.Repositories
{
    public interface IMemoryRepository
    {
        Task<Memorys> Create(Memorys req);
        Task<Memorys> Get(int Id);
        Task<List<Memorys>> GetByDate(DateTime date);
        Task<List<Memorys>> GetByMonth(DateTime date);
        Task<Memorys> Delete(int Id, string deleteAt);
        Task<string> CreatePicture(Pictures req);
        Task<Pictures> GetPicture(string Id);

    }
}