using FluentScheduler;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TransfersApp.BL.Models;
using TransfersApp.Models;
using TransfersApp.Services;
using TransfersApp.Services.Interfaces;

namespace TransfersApp.Jobs
{

    public class TravelsActualizationJob
    {


        private decimal StopFee = 20;
        private decimal PassengerFee = 17;
        private decimal BaseFee = 100;
        private decimal KMFee = 29;
        private int WaitingForConfirmationsStatusId = 1002;
        private int ActiveStatusId = 3;

        static bool _inProgress = false;
        private IClientTransferService _serviceClientTransfer;
        private IServiceBase<ClientPaymentModel, Guid> _serviceClientPayment;
        private ITravelService _serviceTravel;
        private IServiceBase<ConfirmationModel, Guid> _serviceTravelConfirmation;
        private IClientService _serviceClient;
        private ITransferService _serviceTransfer;

        public TravelsActualizationJob()
        {
            _serviceTransfer = (ITransferService)DependencyResolver.Current.GetService(typeof(ITransferService));
            _serviceClientTransfer = (IClientTransferService)DependencyResolver.Current.GetService(typeof(IClientTransferService));
            _serviceClientPayment = (IServiceBase<ClientPaymentModel, Guid>)DependencyResolver.Current.GetService(typeof(IServiceBase<ClientPaymentModel, Guid>));
            _serviceClient = (IClientService)DependencyResolver.Current.GetService(typeof(IClientService));
            _serviceTravel = (ITravelService)DependencyResolver.Current.GetService(typeof(ITravelService));
            _serviceTravelConfirmation = (IServiceBase<ConfirmationModel, Guid>)DependencyResolver.Current.GetService(typeof(IServiceBase<ConfirmationModel, Guid>));
        }
        //[OPTIMIZE] - done
        public async Task ExecuteRemoveForTransfer(TransferModel transfer)
        {
            while (_inProgress)
            {
                await Task.Delay(1000);
            }
            _inProgress = true;
            try
            {
                await _serviceTravel.ExecuteRemoveForTransfer(transfer);
            }
            catch (Exception x)
            {
                Console.WriteLine("Exception ExecuteRemoveForTransfer: " + x.Message);
            }
            finally
            {
                _inProgress = false;
            }
        }

        //[OPTIMIZE] - done
        public async Task ExecuteAddForTransfer(TransferModel transfer)
        {
            while (_inProgress)
            {
                await Task.Delay(1000);
            }
            _inProgress = true;

            try
            {
                await _serviceTravel.ExecuteAddForTransfer(transfer);
            }
            catch (Exception x)
            {
                Console.WriteLine("Exception ExecuteAddForTransfer: " + x.Message);
            }
            finally
            {
                _inProgress = false;
            }
        }

        //[OPTIMIZE] - done
        public async Task UpdateBalanceForShuttleClients(Guid ShuttleId)
        {
            while (_inProgress)
            {
                await Task.Delay(1000);
            }
            _inProgress = true;
            try
            {
                await _serviceTravel.UpdateBalanceForShuttleClients(ShuttleId);
            }
            catch (Exception x)
            {
                Console.WriteLine("Exception UpdateBalanceForShuttleClients: " + x.Message);
            }
            finally
            {
                _inProgress = false;
            }
        }

        //[OPTIMIZE] - done
        public async Task UpdateBackBalanceForShuttleClients(Guid ShuttleId)
        {
            while (_inProgress)
            {
                await Task.Delay(1000);
            }
            _inProgress = true;
            try
            {
                await _serviceTravel.UpdateBackBalanceForShuttleClients(ShuttleId);
            }
            catch (Exception x)
            {
                Console.WriteLine("Exception UpdateBackBalanceForShuttleClients: " + x.Message);
            }
            finally
            {
                _inProgress = false;
            }
        }
        //[OPTIMIZE] - done
        public async Task ExecuteAddForTransfers(List<Guid> transfersIds)
        {
            while (_inProgress)
            {
                await Task.Delay(1000);
            }
            _inProgress = true;
            try
            {
                await _serviceTravel.ExecuteAddForTransfers(transfersIds);
            }
            catch (Exception x)
            {
                Console.WriteLine("Exception ExecuteAddForTransfers: " + x.Message);
            }
            finally
            {
                _inProgress = false;
            }
        }
        //[OPTIMIZE] - done
        public async Task UpdatePricesForShuttle(Guid ShuttleId)
        {
            while (_inProgress)
            {
                await Task.Delay(1000);
            }
            _inProgress = true;
            try
            {
                await _serviceTravel.UpdatePricesForShuttle(ShuttleId);
            }
            catch (Exception x)
            {
                Console.WriteLine("Exception UpdatePricesForShuttle: " + x.Message);
            }
            finally
            {
                _inProgress = false;
            }
        }
    }
}