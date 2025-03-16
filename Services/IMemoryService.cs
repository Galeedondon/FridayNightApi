using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FrightNight.Models;
using Microsoft.AspNetCore.Mvc;
using static FrightNight.Models.Common;

namespace FrightNight.Services
{
    public interface IMemoryService
    {
        Task<Memorys> Create(Memorys request);
        Task<Memorys> Get(int Id);
        Task<List<Memorys>> GetByDate(DateTime date);
        Task<List<Memorys>> GetByMonth(DateTime date);
        Task<Memorys> Delete(int Id, string deleteAt);
        Task<string> CreatePicture(IFormFile avatar);
        Task<Pictures> GetPicture(string Id);
        
    }
}