using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TransfersApp.BL.Models;
using TransfersApp.DataAccess.Abstractions;
using TransfersApp.DataAccess.Entities;
using TransfersApp.Services.Interfaces;

namespace TransfersApp.Services
{
    public class ClientService : IClientService
    {
        private IRepository<Client, Guid> dbContext;

        public ClientService(IRepository<Client, Guid> _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<Guid> Create(ClientModel model)
        {
            var tariffEntity = AutoMapper.Mapper.Map<Client>(model);
            return await dbContext.Create(tariffEntity);
        }

        public async Task Delete(ClientModel model)
        {
            var entity = AutoMapper.Mapper.Map<Client>(model);
            await dbContext.Delete(entity);
        }

        public async Task<IEnumerable<ClientModel>> Get()
        {
            var listEntities = await dbContext.Get();
            var models = AutoMapper.Mapper.Map<IEnumerable<ClientModel>>(listEntities);
            return models;
        }
        public async Task<IEnumerable<ClientModel>> Get(Func<ClientModel, bool> predicate, bool includeDeeleted = false)
        {
            var listEntities = await dbContext.Get(v => predicate(AutoMapper.Mapper.Map<ClientModel>(v)));
            var models = AutoMapper.Mapper.Map<IEnumerable<ClientModel>>(listEntities);
            return models;
        }
        public async Task<ClientModel> GetbyId(Guid Id)
        {
            var entity = (await dbContext.Get(v => v.Id == Id))?.FirstOrDefault();
            if (entity != null)
            {
                var model = AutoMapper.Mapper.Map<ClientModel>(entity);
                return model;
            }
            else
                return null;
        }

        public async Task<ClientModel> GetClientByName(string name)
        {
            var entity = (await dbContext.Get(v => v.Email == name)).FirstOrDefault();
            if (entity != null)
            {
                var model = AutoMapper.Mapper.Map<ClientModel>(entity);
                return model;
            }
            else
                return null;
        }

        public async Task Update(ClientModel model)
        {
            var entity = AutoMapper.Mapper.Map<Client>(model);
            await dbContext.Update(entity);
        }
    }
}