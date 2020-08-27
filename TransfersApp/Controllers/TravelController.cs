using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TransfersApp.BL.Models;
using TransfersApp.Models;
using TransfersApp.Services;
using TransfersApp.Services.Interfaces;

namespace TransfersApp.Controllers
{
    public class TravelController : Controller
    {
        
        private ITravelService _service;
        public TravelController(ITravelService service)
        {
            _service = service;
        }
        // GET: Travel
        public async Task<ActionResult> Index()
        {
            return View((await _service.Get()).OrderByDescending(v=>v.DateTime));
        }

        // GET: Travel/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = await _service.GetbyId(id.Value);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: Travel/Create
        public async Task<ActionResult> Create()
        {
            var _serviceTravelStatusModel = (IServiceBase<TravelStatusModel, int>)DependencyResolver.Current.GetService(typeof(IServiceBase<TravelStatusModel, int>));
            var TravelStatuses = await _serviceTravelStatusModel.Get();
             ViewBag.StatusId = new SelectList(TravelStatuses, "Id", "Name");
            var _serviceTravel = (ITravelService)DependencyResolver.Current.GetService(typeof(ITravelService));
            var TravelModels = await _serviceTravel.Get();
            ViewBag.TravelId = new SelectList(TravelModels, "Id", "Title");
            return View();
        }

        // POST: Travel/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,TravelId,StatusId,DateTime,DisplayName,From,Destination,StateCarNumber,Price,TarrifId,ShuttleId")] TravelModel model)
        {
            if (ModelState.IsValid)
            {
                await _service.Create(model);
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Travel/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TravelModel travelModel = await _service.GetbyId(id.Value);
            if (travelModel == null)
            {
                return HttpNotFound();
            }
            var _serviceTravelStatusModel = (IServiceBase<TravelStatusModel, int>)DependencyResolver.Current.GetService(typeof(IServiceBase<TravelStatusModel, int>));
            var TravelStatuses = await _serviceTravelStatusModel.Get();
            ViewBag.StatusId = new SelectList(TravelStatuses, "Id", "Name");

            var _serviceTravel = (ITravelService)DependencyResolver.Current.GetService(typeof(ITravelService));
            var TravelModels = await _serviceTravel.Get();
            ViewBag.TravelId = new SelectList(TravelModels, "Id", "Title");

            return View(travelModel);
        }

        // POST: Travel/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,TravelId,StatusId,DateTime,DisplayName,From,Destination,StateCarNumber,Price,TarrifId, ShuttleId")] TravelModel model)
        {
            if (ModelState.IsValid)
            {
                await _service.Update(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Travel/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = await _service.GetbyId(id.Value);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: Travel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            var model = await _service.GetbyId(id);
            await _service.Delete(model);
            return RedirectToAction("Index");
        }

    }
}
