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
using Microsoft.IdentityModel.Tokens;
using static FrightNight.Models.Common;

namespace FrightNight.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _repository;
        private readonly IConfiguration _configuration;

        public CommentService(ICommentRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }
        public async Task<Comments> Create(Comments request)
        {
             return await _repository.Create(request);
        }
        public async Task Update(Comments request)
        {
            _repository.Update(request);
        }
        public async Task<List<Comments>> Get(int Id)
        {
             return await _repository.Get(Id);
        }
        public async Task<Comments> Delete(int Id, string deleteAt)
        {
            return await _repository.Delete(Id,deleteAt);
        }
    }
}