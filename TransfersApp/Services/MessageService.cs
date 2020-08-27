using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TransfersApp.BL.Models;
using TransfersApp.DataAccess.Abstractions;
using TransfersApp.DataAccess.Entities;
using TransfersApp.Entities;
using TransfersApp.Models;
using TransfersApp.Services.Interfaces;

namespace TransfersApp.Services
{
    public class MessageService : IMessageService
    {
        private IRepository<Message, Guid> dbContext;

        public MessageService(IRepository<Message, Guid> _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<Guid> Create(MessageModel model)
        {
            var tariffEntity = AutoMapper.Mapper.Map<Message>(model);
            return await dbContext.Create(tariffEntity);
        }

        public async Task Delete(MessageModel model)
        {
            var entity = AutoMapper.Mapper.Map<Message>(model);
            await dbContext.Delete(entity);
        }

        public async Task<IEnumerable<MessageModel>> Get()
        {
            var listEntities = await dbContext.Get();
            var models = AutoMapper.Mapper.Map<IEnumerable<MessageModel>>(listEntities);
            return models;
        }

        public async Task<IEnumerable<MessageModel>> Get(Func<MessageModel, bool> predicate, bool includeDeeleted = false)
        {
            var listEntities = await dbContext.Get(v => predicate(AutoMapper.Mapper.Map<MessageModel>(v)));
            var models = AutoMapper.Mapper.Map<IEnumerable<MessageModel>>(listEntities);
            return models;
        }
        public async Task<MessageModel> GetbyId(Guid Id)
        {
            var entity = (await dbContext.Get(v => v.Id == Id))?.FirstOrDefault();
            if (entity != null)
            {
                var model = AutoMapper.Mapper.Map<MessageModel>(entity);
                return model;
            }
            else
                return null;
        }

        public async Task<IEnumerable<MessageModel>> GetByTravelId(Guid id)
        {
            var listEntities = (await dbContext.Get(v => v.TravelId == id)).ToList();

            var models = AutoMapper.Mapper.Map<IEnumerable<MessageModel>>(listEntities);
            return models;
        }

        public async Task Update(MessageModel model)
        {
            var entity = AutoMapper.Mapper.Map<Message>(model);
            await dbContext.Update(entity);
        }
    }
}