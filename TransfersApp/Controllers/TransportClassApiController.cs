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
    public class TransportClassApiController : ApiController
    {
        private IServiceBase<TransportClassModel, int> _service;

        public TransportClassApiController()
        {
            _service = (IServiceBase<TransportClassModel, int>)DependencyResolver.Current.GetService(typeof(IServiceBase<TransportClassModel, int>));
        }
        public async Task<IEnumerable<TransportClassModel>> c()
        {
            var responce = await _service.Get();
            return responce;
        }

        // GET: api/TransferApi/5
        public async Task<TransportClassModel> GetbyId(int id)
        {
            TransportClassModel model = await _service.GetbyId(id);

            return model;
        }


        // PUT: api/TransferApi/5
        public async Task<TransportClassModel> Update(TransportClassModel model)
        {

            try
            {
                await _service.Update(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await TransportClassModelExists(model.Id)))
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

        // POST: api/TransferApi
        [ResponseType(typeof(TransportClassModel))]
        public async Task<TransportClassModel> Create(TransportClassModel model)
        {
            model.Id = await _service.Create(model);
            TransportClassModel newmodel = await _service.GetbyId(model.Id);

            return newmodel;
        }

        // DELETE: api/TransferApi/5
        public async Task<bool> Delete(int id)
        {
            TransportClassModel model = await _service.GetbyId(id);
            if (model == null)
            {
                return false;
            }
            await _service.Delete(model);

            return true;
        }
        private async Task<bool> TransportClassModelExists(int id)
        {
            return (await _service.GetbyId(id)) != null;
        }
    }
}