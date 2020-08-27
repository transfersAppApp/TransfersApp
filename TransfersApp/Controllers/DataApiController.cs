using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using TransfersApp.BL.Models;
using TransfersApp.Models;
using TransfersApp.Services;

namespace TransfersApp.Controllers
{
    public class DataApiController : ApiController
    {
        private IServiceBase<TariffModel, int> _serviceTariff;
        private IServiceBase<TransferStatusModel, int> _serviceTransferStatus;
        private IServiceBase<TravelStatusModel, int> _serviceTravelStatus;
        public DataApiController()
        {
            _serviceTariff = (IServiceBase<TariffModel, int>)DependencyResolver.Current.GetService(typeof(IServiceBase<TariffModel, int>));
            _serviceTransferStatus = (IServiceBase<TransferStatusModel, int>)DependencyResolver.Current.GetService(typeof(IServiceBase<TransferStatusModel, int>));
            _serviceTravelStatus = (IServiceBase<TravelStatusModel, int>)DependencyResolver.Current.GetService(typeof(IServiceBase<TravelStatusModel, int>));
        }

        public async Task<ComplexDataModel> GetData()
        {
            var test = Guid.Empty.ToString();
            ComplexDataModel responce = new ComplexDataModel();
            responce.Tariffs = await _serviceTariff.Get();
            responce.TransferStatuses = await _serviceTransferStatus.Get();
            responce.TravelStatuses = await _serviceTravelStatus.Get();
            return responce;
        }
    }
}
