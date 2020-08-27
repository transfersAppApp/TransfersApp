using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using TransfersApp.BL.Models;
using TransfersApp.Models;
using TransfersApp.Services;

namespace TransfersApp.Controllers
{
    public class TariffApiController : ApiController
    {
       // 

        private IServiceBase<TariffModel, int> _service;
        public TariffApiController()
        {
            _service = (IServiceBase<TariffModel, int>)DependencyResolver.Current.GetService(typeof(IServiceBase<TariffModel, int>));
        }

        // GET: api/TariffApi
        public async Task<IEnumerable<TariffModel>> Get()
        {
            var responce = await _service.Get();
            return responce;
        }

        // GET: api/TariffApi/5
        public async Task<TariffModel> GetById(int id)
        {
            TariffModel model = await _service.GetbyId(id);
           
            return model;
        }

        // PUT: api/TariffApi/5
        public async Task<TariffModel> Update(TariffModel model)
        {
            try
            {
                await _service.Update(model);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!(await TariffModelExists(model.Id)))
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

        // POST: api/TariffApi
        public async Task<TariffModel> Create(TariffModel model)
        {
            model.Id = await _service.Create(model);
            TariffModel newmodel = await _service.GetbyId(model.Id);

            return newmodel;
        }

        // DELETE: api/TariffApi/5
        public async Task<bool> Delete(int id)
        {
            TariffModel model = await _service.GetbyId(id);
            if (model == null)
            {
                return false;
            }
            await _service.Delete(model);

            return true;
        }

        private async Task<bool> TariffModelExists(int id)
        {
            return (await _service.GetbyId(id)) != null;
        }
    }
}