using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TransfersApp.Models;
using TransfersApp.Services;
using TransfersApp.BL.Models;
using TransfersApp.Models.SearchModels;
using TransfersApp.Services.Interfaces;

namespace TransfersApp.Controllers
{
    public class ShuttleController : Controller
    {
        private IShuttleService _service;


        public ShuttleController(IShuttleService service)
        {
            _service = service;
        }
        // GET: ShuttleModels
        public async Task<ActionResult> Index()
        {
            var list = (await _service.GetEntities(v => true)).ToList();

            var modelList = new List<ShuttleViewModel>();

            ViewBag.Items = list;
            ViewBag.PassengersCount = new SelectList(new List<string>() { "All", "1", "2", "3", "4" }, "All");

            return View(new ShuttleSearchModel());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index([Bind(Include = "PassengersCount,NameSearchText")] ShuttleSearchModel model)
        {
            var list = (await _service.GetEntities(v=> true)).ToList();

            var modelList = new List<ShuttleViewModel>();

            foreach (var sh in list)
            {
                if (model != null)
                {
                    bool result = false;
                    if (!string.IsNullOrEmpty(model.NameSearchText))
                    {
                        result = result || sh.Name.ToLower().Contains(model.NameSearchText.ToLower());
                    }
                    else
                        result = true;

                    if (model.PassengersCount != "All")
                    {
                        int count = int.Parse(model.PassengersCount);
                        result = result && sh.Passengers == count;
                    }
                        if (result)
                            modelList.Add(sh);
                }
                else
                    modelList.Add(sh);
            }

            ViewBag.Items = modelList; 
            ViewBag.PassengersCount = new SelectList(new List<string>() { "All", "1", "2", "3", "4" }, "All");

            if (model == null)
                model = new ShuttleSearchModel();

            return View(model);
        }
        // GET: ShuttleModels/Details/5
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

        // GET: ShuttleModels/Create
        public async Task<ActionResult> Create()
        {

            var _serviceRallyPoint = (IServiceBase<RallyPointModel, Guid>)DependencyResolver.Current.GetService(typeof(IServiceBase<RallyPointModel, Guid>));
            var RallyPoints = await _serviceRallyPoint.Get();
            ViewBag.StartRallyPointId = new SelectList(RallyPoints, "Id", "Description");

            var _serviceTimeSlot = (IServiceBase<TimeSlotModel, int>)DependencyResolver.Current.GetService(typeof(IServiceBase<TimeSlotModel, int>));
            var TimeSlots = await _serviceTimeSlot.Get();
            ViewBag.TimeSlotId = new SelectList(TimeSlots, "Id", "Time");

            return View();
        }

        // POST: ShuttleModels/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,TimeSlotId,StartRallyPointId,IsDeleted,Price")] ShuttleModel shuttleModel)
        {
            if (ModelState.IsValid)
            {
                await _service.Create(shuttleModel);
                return RedirectToAction("Index");
            }

            var _serviceRallyPoint = (IServiceBase<RallyPointModel, Guid>)DependencyResolver.Current.GetService(typeof(IServiceBase<RallyPointModel, Guid>));
            var RallyPoints = await _serviceRallyPoint.Get();
            ViewBag.StartRallyPointId = new SelectList(RallyPoints, "Id", "Description", shuttleModel.StartRallyPointId);

            var _serviceTimeSlot = (IServiceBase<TimeSlotModel, int>)DependencyResolver.Current.GetService(typeof(IServiceBase<TimeSlotModel, int>));
            var TimeSlots = await _serviceTimeSlot.Get();
            ViewBag.TimeSlotId = new SelectList(TimeSlots, "Id", "Time", shuttleModel.TimeSlotId);

            return View(shuttleModel);
        }

        // GET: ShuttleModels/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var shuttleModel = await _service.GetbyId(id.Value);
            if (shuttleModel == null)
            {
                return HttpNotFound();
            }

            var _serviceRallyPoint = (IServiceBase<RallyPointModel, Guid>)DependencyResolver.Current.GetService(typeof(IServiceBase<RallyPointModel, Guid>));
            var RallyPoints = await _serviceRallyPoint.Get();
            ViewBag.StartRallyPointId = new SelectList(RallyPoints, "Id", "Description", shuttleModel.StartRallyPointId);

            var _serviceTimeSlot = (IServiceBase<TimeSlotModel, int>)DependencyResolver.Current.GetService(typeof(IServiceBase<TimeSlotModel, int>));
            var TimeSlots = await _serviceTimeSlot.Get();
            ViewBag.TimeSlotId = new SelectList(TimeSlots, "Id", "Time", shuttleModel.TimeSlotId);


            return View(shuttleModel);
        }

        // POST: ShuttleModels/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,TimeSlotId,StartRallyPointId,IsDeleted,Price")] ShuttleModel shuttleModel)
        {
            if (ModelState.IsValid)
            {
                await _service.Update(shuttleModel);
                return RedirectToAction("Index");
            }

            var _serviceRallyPoint = (IServiceBase<RallyPointModel, Guid>)DependencyResolver.Current.GetService(typeof(IServiceBase<RallyPointModel, Guid>));
            var RallyPoints = await _serviceRallyPoint.Get();
            ViewBag.StartRallyPointId = new SelectList(RallyPoints, "Id", "Description", shuttleModel.StartRallyPointId);

            var _serviceTimeSlot = (IServiceBase<TimeSlotModel, int>)DependencyResolver.Current.GetService(typeof(IServiceBase<TimeSlotModel, int>));
            var TimeSlots = await _serviceTimeSlot.Get();
            ViewBag.TimeSlotId = new SelectList(TimeSlots, "Id", "Time", shuttleModel.TimeSlotId);


            return View(shuttleModel);
        }

        // GET: ShuttleModels/Delete/5
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

        // POST: ShuttleModels/Delete/5
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
