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
using TransfersApp.Models;
using TransfersApp.Services;
using TransfersApp.Services.Interfaces;

namespace TransfersApp.Controllers
{
    public class MessageApiController : ApiController
    {
        private IMessageService _service;

        // GET: api/Messag
        public MessageApiController()
        { 
            _service = (IMessageService)DependencyResolver.Current.GetService(typeof(IMessageService));
        
        }

        // GET: api/MessageApi/5
        [System.Web.Http.HttpGet]

        public async Task<IEnumerable<MessageModel>> GetMessageModel(Guid id)
        {
            var messageModels = await _service.GetByTravelId(id);

            return messageModels;
        }

    }
}