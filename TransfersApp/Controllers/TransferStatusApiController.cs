using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using TransfersApp.BL.Models;
using TransfersApp.Models;
using TransfersApp.Services;

namespace TransfersApp.Controllers
{
    public class TransferStatusApiController : ApiController
    {
        
        private IServiceBase<TransferStatusModel, int> _service;
        public TransferStatusApiController()
        {
            _service = (IServiceBase<TransferStatusModel, int>)DependencyResolver.Current.GetService(typeof(IServiceBase<TransferStatusModel, int>));
        }
        // GET: api/TransferStatusModels
        public async Task<IEnumerable<TransferStatusModel>> Get()
        {
            var responce = await _service.Get();
            return responce;
        }

        // GET: api/TransferStatusModels/5
        public async Task<TransferStatusModel> GetById(int id)
        {
            TransferStatusModel model = await _service.GetbyId(id);

            return model;
        }

        // PUT: api/TransferStatusModels/5
        public async Task<TransferStatusModel> Update(TransferStatusModel model)
        {
            
            try
            {
                await _service.Update(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await TransferStatusModelExists(model.Id)))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            var newmodel = await _service.GetbyId(model.Id);
            return newmodel;
        }

        // POST: api/TransferStatusModels
        public async Task<TransferStatusModel> Create(TransferStatusModel model)
        {
            model.Id = await _service.Create(model);
            TransferStatusModel newmodel = await _service.GetbyId(model.Id);

            return newmodel;
        }

        // DELETE: api/TransferStatusModels/5
        public async Task<bool> Delete(int id)
        {
            TransferStatusModel model = await _service.GetbyId(id);
            if (model == null)
            {
                return false;
            }
            await _service.Delete(model);

            return true;
        }
        private async Task<bool> TransferStatusModelExists(int id)
        {
            return (await _service.GetbyId(id)) != null;
        }
    }
}