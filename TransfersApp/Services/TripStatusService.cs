using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TransfersApp.BL.Models;
using TransfersApp.DataAccess.Abstractions;
using TransfersApp.DataAccess.Entities;

namespace TransfersApp.Services
{
    public class TransferStatusService : IServiceBase<TransferStatusModel, int>
    {
        private IRepository<TransferStatus, int> dbContext;

        public TransferStatusService(IRepository<TransferStatus, int> _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<int> Create(TransferStatusModel model)
        {
            var tariffEntity = AutoMapper.Mapper.Map<TransferStatus>(model);
            return await dbContext.Create(tariffEntity);
        }

        public async Task Delete(TransferStatusModel model)
        {
            var entity = AutoMapper.Mapper.Map<TransferStatus>(model);
            await dbContext.Delete(entity);
        }

        public async Task<IEnumerable<TransferStatusModel>> Get()
        {
            var listEntities = await dbContext.Get();
            var models = AutoMapper.Mapper.Map<IEnumerable<TransferStatusModel>>(listEntities);
            return models;
        }

        public async Task<IEnumerable<TransferStatusModel>> Get(Func<TransferStatusModel, bool> predicate, bool includeDeeleted = false)
        {
            var listEntities = await dbContext.Get(v => predicate(AutoMapper.Mapper.Map<TransferStatusModel>(v)));
            var models = AutoMapper.Mapper.Map<IEnumerable<TransferStatusModel>>(listEntities);
            return models;
        }
        public async Task<TransferStatusModel> GetbyId(int Id)
        {
            var entity = (await dbContext.Get(v => v.Id == Id))?.FirstOrDefault();
            if (entity != null)
            {
                var model = AutoMapper.Mapper.Map<TransferStatusModel>(entity);
                return model;
            }
            else
                return null;
        }


        public async Task Update(TransferStatusModel model)
        {
            var entity = AutoMapper.Mapper.Map<TransferStatus>(model);
            await dbContext.Update(entity);
        }
    }
}