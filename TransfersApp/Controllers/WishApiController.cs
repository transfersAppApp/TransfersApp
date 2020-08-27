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
    public class WishApiController : ApiController
    {
        
        private IServiceBase<WishModel, Guid> _service;
        public WishApiController()
        {
            _service = (IServiceBase<WishModel, Guid>)DependencyResolver.Current.GetService(typeof(IServiceBase<WishModel, Guid>));
        }
        // GET: api/WishApi
        public async Task<IEnumerable<WishModel>> Get()
        {
            var responce = await _service.Get();
            return responce;
        }

        // GET: api/WishApi/5
        public async Task<WishModel> GetById(Guid id)
        {
            WishModel model = await _service.GetbyId(id);

            return model;
        }

        // PUT: api/WishApi/5
        public async Task<WishModel> Update(WishModel model)
        {

            try
            {
                await _service.Update(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await WishModelExists(model.Id)))
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

        // POST: api/WishApi
        public async Task<WishModel> Create(WishModel model)
        {
            model.Id = await _service.Create(model);
            WishModel newmodel = await _service.GetbyId(model.Id);

            return newmodel;
        }

        // DELETE: api/WishApi/5
        public async Task<bool> Delete(Guid id)
        {
            WishModel model = await _service.GetbyId(id);
            if (model == null)
            {
                return false;
            }
            await _service.Delete(model);

            return true;
        }
        private async Task<bool> WishModelExists(Guid id)
        {
            return (await _service.GetbyId(id)) != null;
        }
    }
}