using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc4;
using TransfersApp.Controllers;
using TransfersApp;
using TransfersApp.Services;
using TransfersApp.BL.Models;
using TransfersApp.DataAccess.Abstractions;
using TransfersApp.DataAccess.Repositories;
using TransfersApp.DataAccess.Entities;
using System;
using System.Web.Http;
using System.Web.Http.Controllers;
using TransfersApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using Microsoft.AspNet.Identity.Owin;
using TransfersApp.Services.Interfaces;
using TransfersApp.Entities;

namespace TransfersApp
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IServiceBase<TariffModel, int>, TariffService>();
            container.RegisterType<IRepository<Tariff, int>, EntityRepository<Tariff, int>>();

            container.RegisterType<IServiceBase<ClientModel, Guid>, ClientService>();
            container.RegisterType<IClientService, ClientService>();
            container.RegisterType<IRepository<Client, Guid>, EntityRepository<Client, Guid>>();

            container.RegisterType<IServiceBase<TransferStatusModel, int>, TransferStatusService>();
            container.RegisterType<IRepository<TransferStatus, int>, EntityRepository<TransferStatus, int>>();

            container.RegisterType<IServiceBase<TravelStatusModel, int>, TravelStatusService>();
            container.RegisterType<IRepository<TravelStatus, int>, EntityRepository<TravelStatus, int>>();


            container.RegisterType<IServiceBase<TransportClassModel, int>, TransportClassService>();
            container.RegisterType<IRepository<TransportClass, int>, EntityRepository<TransportClass, int>>();
           
            container.RegisterType<IClientTransferService, ClientTransferService>();
            container.RegisterType<IServiceBase<ClientTransferModel, Guid>, ClientTransferService>();
            container.RegisterType<IRepository<ClientTransfer, Guid>, ClientTransferRepository>();

            container.RegisterType<IServiceBase<ConfirmationModel, Guid>, ConfirmationService>();
            container.RegisterType<IRepository<Confirmation, Guid>, ConfirmationRepository>();

            container.RegisterType<ITravelService, TravelService>();
            container.RegisterType<ITravelRepository, TravelRepository>();

            container.RegisterType<ITransferService, TransferService>();
            container.RegisterType<ITransferRepository, TransferRepository>();

            container.RegisterType<IServiceBase<WishModel, Guid>, WishService>();
            container.RegisterType<IRepository<Wish, Guid>, WishRepository>();

            container.RegisterType<IServiceBase<WishModel, Guid>, WishService>();
            container.RegisterType<IRepository<Wish, Guid>, WishRepository>();

            container.RegisterType<IServiceBase<RallyPointModel, Guid>, RallyPointService>();
            container.RegisterType<IRepository<RallyPoint, Guid>, RallyPointRepository>();

            container.RegisterType<IShuttleService, ShuttleService>();
            container.RegisterType<IShuttleRepository, ShuttleRepository>();

            container.RegisterType<IMessageService, MessageService>();
            container.RegisterType<IServiceBase<MessageModel, Guid>, MessageService>();
            container.RegisterType<IRepository<Message, Guid>, MessageRepository>();
            
            container.RegisterType<IServiceBase<TimeSlotModel, int>, TimeSlotService>();
            container.RegisterType<IRepository<TimeSlot, int>, EntityRepository<TimeSlot, int>>();

            container.RegisterType<DbContext, ApplicationDbContext>(new HierarchicalLifetimeManager());
            container.RegisterType<UserManager<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<SignInManager<ApplicationUser, string>, ApplicationSignInManager>(new HierarchicalLifetimeManager());
            container.RegisterType<ApplicationSignInManager>(new HierarchicalLifetimeManager());

            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<ManageController>(new InjectionConstructor());
            container.RegisterType<AccountApiController>(new InjectionConstructor());

            RegisterTypes(container);

            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {

        }
    }
}