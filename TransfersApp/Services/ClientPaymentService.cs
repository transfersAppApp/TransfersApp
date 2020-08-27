using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TransfersApp.BL.Models;
using TransfersApp.DataAccess.Abstractions;
using TransfersApp.DataAccess.Entities;
using TransfersApp.DataAccess.Repositories;
using TransfersApp.Entities;
using TransfersApp.Models;
using TransfersApp.Services.Interfaces;

namespace TransfersApp.Services
{
    public class ClientPaymentService : IClientPaymentService
    {
        private IRepository<ClientPayment, Guid> dbContext;
        private ITravelRepository dbContextPayment;

        public ClientPaymentService(IRepository<ClientPayment, Guid> _dbContext, ITravelRepository _dbContextPayment)
        {
            dbContext = _dbContext;
            dbContextPayment = _dbContextPayment;
        }
        public async Task<Guid> Create(ClientPaymentModel model)
        {
            var entity = AutoMapper.Mapper.Map<ClientPayment>(model);
            var checkPayment = (await dbContext.Get(b=>b.TravelId == model.TravelId)).Any(r=>r.ClientId == model.ClientId);
            if (!checkPayment)
            {
                return await dbContext.Create(entity);
            }
            else
            {
                return Guid.Empty;
            }
        }

        public async Task Delete(ClientPaymentModel model)
        {
           // var entity = AutoMapper.Mapper.Map<ClientPayment>(model);
           /* await dbContext.DeleteById(model.Id);
            if (model.TravelId.HasValue)
            {
                var anyActiveClientForPayment = (await dbContext.Get(v => v.TravelId == model.TravelId)).Any();
                if (!anyActiveClientForPayment)
                {
                    var inativePayment = (await dbContextPayment.Get(f => f.Id == model.ClientId))?.FirstOrDefault();
                    if (inativePayment != null)
                    {
                        await dbContextPayment.DeleteById(model.PaymentId.Value);
                    }
                }
            }*/
        }

        public async Task<IEnumerable<ClientPaymentModel>> Get()
        {
            var listEntities = await dbContext.Get();
            var models = AutoMapper.Mapper.Map<IEnumerable<ClientPaymentModel>>(listEntities);
            return models;
        }

        public async Task<IEnumerable<ClientPaymentModel>> Get(Func<ClientPaymentModel, bool> predicate, bool includeDeeleted = false)
        {
            var listEntities = await dbContext.Get(v=> predicate(AutoMapper.Mapper.Map<ClientPaymentModel>(v)));
            var models = AutoMapper.Mapper.Map<IEnumerable<ClientPaymentModel>>(listEntities);
            return models;
        }

        public async Task<ClientPaymentModel> GetbyId(Guid Id)
        {
            var entity = (await dbContext.Get(v => v.Id == Id))?.FirstOrDefault();
            if (entity != null)
            {
                var model = AutoMapper.Mapper.Map<ClientPaymentModel>(entity);
                return model;
            }
            else
                return null;
        }

        public async Task<ClientPaymentModel> GetByTravelId(Guid Id)
        {
            var entity = (await dbContext.Get(v => v.TravelId.HasValue && v.TravelId.Value == Id))?.FirstOrDefault();
            if (entity != null)
            {
                var model = AutoMapper.Mapper.Map<ClientPaymentModel>(entity);
                return model;
            }
            else
                return null;
        }

        public async Task Update(ClientPaymentModel model)
        {
            var entity = AutoMapper.Mapper.Map<ClientPayment>(model);
            await dbContext.Update(entity);
        }
    }
}