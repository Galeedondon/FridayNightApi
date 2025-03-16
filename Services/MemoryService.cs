using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FrightNight.Models;
using FrightNight.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static FrightNight.Models.Common;

namespace FrightNight.Services
{
    public class MemoryService : IMemoryService
    {
        private readonly IMemoryRepository _repository;
        private readonly IConfiguration _configuration;

        public MemoryService(IMemoryRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }
        public async Task<Memorys> Create(Memorys request)
        {
            return await _repository.Create(request);
        }
        public async Task<Memorys> Get(int Id)
        {
            return await _repository.Get(Id);
        }
        public async Task<List<Memorys>> GetByDate(DateTime date)
        {
            return await _repository.GetByDate(date);
        }
        public async Task<List<Memorys>> GetByMonth(DateTime date)
        {
            return await _repository.GetByMonth(date);
        }
        public async Task<Memorys> Delete(int Id, string deleteAt)
        {
            return await _repository.Delete(Id, deleteAt);
        }
        public async Task<string> CreatePicture(IFormFile avatar)
        {
            byte[] fileData;
            using (var memoryStream = new MemoryStream())
            {
                await avatar.CopyToAsync(memoryStream);
                fileData = memoryStream.ToArray();
            }

            // 創建 File 實例
            var pictures = new Pictures
            {
                FileName = avatar.FileName,
                FileData = fileData,
                ContentType = avatar.ContentType
            };

            return await _repository.CreatePicture(pictures);
        }

        public async Task<Pictures> GetPicture(string Id)
        {
            return await _repository.GetPicture(Id);

        }
    }
}