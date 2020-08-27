using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TransfersApp.BL.Models;
using TransfersApp.DataAccess.Entities;
using TransfersApp.Entities;
using TransfersApp.Models;

namespace TransfersApp.App_Start
{
    public static class AutomapperConfiguration
    {
        public static void Initialize()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<string, Guid>().ConvertUsing(Guid.Parse);
                cfg.CreateMap<string, Guid?>().ConvertUsing(s => string.IsNullOrWhiteSpace(s) ? (Guid?)null : Guid.Parse(s));


                cfg.CreateMap<Client, ClientModel>();
                cfg.CreateMap<ClientModel, Client>();
                cfg.CreateMap<ClientTransfer, ClientTransferModel>();
                cfg.CreateMap<ClientTransfer, ClientTransfer>();
                cfg.CreateMap<Confirmation, ConfirmationModel>().ForMember(v=>v.Travel, t=>t.Ignore());
                cfg.CreateMap<ConfirmationModel, Confirmation>();

                cfg.CreateMap<Tariff, TariffModel>();
                cfg.CreateMap<TariffModel, Tariff>();
                cfg.CreateMap<TransportClass, TransportClassModel>();
                cfg.CreateMap<TransportClassModel, TransportClass>();
                cfg.CreateMap<Travel, TravelModel>();
                cfg.CreateMap<TravelModel, Travel>();

                cfg.CreateMap<Transfer, TransferModel>();
                cfg.CreateMap<TransferModel, Transfer>();
                cfg.CreateMap<TransferModel, TransferViewModel>();
                cfg.CreateMap<TransferStatus, TransferStatusModel>();
                cfg.CreateMap<TransferStatusModel, TransferStatus>();
                cfg.CreateMap<Wish, WishModel>();
                cfg.CreateMap<WishModel, Wish>();

                cfg.CreateMap<TravelStatus, TravelStatusModel>();
                cfg.CreateMap<TravelStatusModel, TravelStatus>();

                cfg.CreateMap<Shuttle, ShuttleModel>();
                cfg.CreateMap<ShuttleModel, Shuttle>();
                cfg.CreateMap<ShuttleModel, ShuttleViewModel>();
                cfg.CreateMap<Shuttle, ShuttleViewModel>();

                cfg.CreateMap<TimeSlot, TimeSlotModel>();
                cfg.CreateMap<TimeSlotModel, TimeSlot>();

                cfg.CreateMap<Message, MessageModel>();
                cfg.CreateMap<MessageModel, Message>();
            });
        }
    }
}