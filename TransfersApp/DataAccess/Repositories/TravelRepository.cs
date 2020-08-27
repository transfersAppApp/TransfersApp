using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TransfersApp.DataAccess.Entities;
using TransfersApp.Models;
using System.Data;
using System.Data.Entity;
using TransfersApp.BL.Models;

namespace TransfersApp.DataAccess.Repositories
{
    public class TravelRepository : EntityRepository<Travel, Guid>, ITravelRepository
    {
        public TravelRepository(ApplicationDbContext dbContext) : base(dbContext)
        {

        }
        public override async Task<ICollection<Travel>> Get(bool includeDeeleted = false)
        {
            using (await Lock.LockAsync())
            {
                var all = (_dbContext.Travels.Include(t => t.TravelStatus).Include(v => v.Confirmations).Include(v => v.Messages).ToList());
                if (!includeDeeleted)
                {
                    all = all.Where(v => !v.IsDeleted).ToList();
                }
                return all.ToList();
            }
        }

        public override async Task<ICollection<Travel>> Get(Func<Travel, bool> predicate, bool includeDeeleted = false)
        {
            using (await Lock.LockAsync())
            {
                var all = (_dbContext.Travels.Include(t => t.TravelStatus).Include(v => v.Confirmations).Include(v => v.Messages)).ToList().Where(predicate).ToList();
                if (!includeDeeleted)
                {
                    all = all.Where(v => !v.IsDeleted).ToList();
                }
                return all.ToList();
            }
        }



        public async Task<bool> ChechIfAlreadyCreated(TransferModel model, DateTime potentialNextDate)
        {
            using (await Lock.LockAsync())
            {
                var nextTravelsForTransfer = _dbContext.Travels.AsNoTracking().Where(v => v.TransferId == model.Id && !v.IsDeleted).ToList().Where(b => b.DateTime.Value.Date == potentialNextDate.Date);
                return nextTravelsForTransfer?.Any() ?? false;
            }
        }

        public async Task<bool> ChechIfAlreadyCreated(Transfer model, DateTime potentialNextDate)
        {
            try
            {
                return (await Get(v => v.TransferId == model.Id)).ToList()?.Any(b => b.DateTime.Value.Date == potentialNextDate.Date && !b.IsDeleted) ?? false;
            }
            catch (Exception x)
            { 

            }
            return false;
        }

        public async Task CleanDuplicates(TransferModel model, DateTime potentialNextDate)
        {
            using (await Lock.LockAsync())
            {
                var nextTravelsForTransfer = (await Get(v => v.TransferId == model.Id && !v.IsDeleted)).ToList().Where(b => b.DateTime.Value.Date == potentialNextDate.Date).Skip(1);

                foreach (var existingEntity in nextTravelsForTransfer)
                {
                    existingEntity.IsDeleted = true;
                    _dbContext.Entry(existingEntity).CurrentValues.SetValues(existingEntity);
                    _dbContext.Entry(existingEntity).State = EntityState.Modified;
                }
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task CleanDuplicates(Transfer model, DateTime potentialNextDate)
        {
            using (await Lock.LockAsync())
            {
                var nextTravelsForTransfer = _dbContext.Travels.Where(v => v.TransferId == model.Id && !v.IsDeleted).ToList().Where(b => b.DateTime.Value.Date == potentialNextDate.Date).Skip(1);

                foreach (var existingEntity in nextTravelsForTransfer)
                {
                    existingEntity.IsDeleted = true;
                    _dbContext.Entry(existingEntity).CurrentValues.SetValues(existingEntity);
                    _dbContext.Entry(existingEntity).State = EntityState.Modified;
                }
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task CleanDuplicates(Transfer model, IEnumerable<DateTime> potentialNextDates)
        {
            using (await Lock.LockAsync())
            {
                foreach (var potentialNextDate in potentialNextDates)
                {
                    var nextTravelsForTransfer = _dbContext.Travels.Where(v => v.TransferId == model.Id && !v.IsDeleted).ToList().Where(b => b.DateTime.Value.Date == potentialNextDate.Date).Skip(1);

                    foreach (var existingEntity in nextTravelsForTransfer)
                    {
                        existingEntity.IsDeleted = true;
                        _dbContext.Entry(existingEntity).CurrentValues.SetValues(existingEntity);
                        _dbContext.Entry(existingEntity).State = EntityState.Modified;
                    }
                }
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task CleanAll(TransferModel model, DateTime potentialNextDate)
        {
            using (await Lock.LockAsync())
            {
                var nextTravelsForTransfer = (await Get(v => v.TransferId == model.Id)).ToList().Where(b => b.DateTime.Value.Date == potentialNextDate.Date);

                foreach (var existingEntity in nextTravelsForTransfer)
                {
                    existingEntity.IsDeleted = true;
                    _dbContext.Entry(existingEntity).CurrentValues.SetValues(existingEntity);
                    _dbContext.Entry(existingEntity).State = EntityState.Modified;
                }
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateBackBalanceForShuttleClients(Guid ShuttleId)
        {

            var NOW = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));

            var travelsToCompute = _dbContext.Travels.ToList().Where(v => v.ShuttleId == ShuttleId && v.DateTime >= NOW && !v.IsDeleted);
            var paidTravels = travelsToCompute.Where(b => b.Paid && !b.IsDeleted).ToList();

            if (paidTravels.Any())
            {
                foreach (var travel in paidTravels)
                {
                    if (travel.Price <= 10)
                    {
                        var confirm = travel.Confirmations.FirstOrDefault();
                        confirm.Client.Balancce += travel.Price;
                        travel.Paid = false;


                        var existingEntity = await _dbContext.Travels.FindAsync(travel.Id);
                        if (existingEntity != null)
                        {
                            _dbContext.Entry(existingEntity).CurrentValues.SetValues(travel);
                            _dbContext.Entry(existingEntity).State = EntityState.Modified;
                        }

                        var existingClient = await _dbContext.Clients.FindAsync(confirm.Client.Id);
                        if (existingClient != null)
                        {
                            existingClient.Balancce = confirm.Client.Balancce;
                            _dbContext.Entry(existingClient).CurrentValues.SetValues(existingClient);
                            _dbContext.Entry(existingClient).State = EntityState.Modified;
                        }
                    }
                }
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task UpdateBalanceForShuttleClients(Guid ShuttleId)
        {


            var NOW = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));

            var travelsToCompute = (_dbContext.Travels.Include(v => v.Confirmations).ToList().Where(v => v.ShuttleId == ShuttleId && v.DateTime >= NOW && !v.IsDeleted)).ToList();


            foreach (var travel in travelsToCompute)
            {
                if (travel.Price <= 10)
                {
                    var confirm = travel.Confirmations.FirstOrDefault();
                    if (confirm != null)
                    {
                        var existingClient = await _dbContext.Clients.FindAsync(confirm.Client.Id);
                        if (existingClient != null)
                        {
                            var clientCurrentBalance = _dbContext.Entry(existingClient).CurrentValues.GetValue<decimal>("Balancce");
                            if (clientCurrentBalance > confirm.Client.InsuranceSum)
                            {
                                if (clientCurrentBalance - travel.Price > 0)
                                {
                                    clientCurrentBalance -= travel.Price;
                                    travel.Paid = clientCurrentBalance > 0;


                                    var existingEntity = await _dbContext.Travels.FindAsync(travel.Id);
                                    if (existingEntity != null)
                                    {
                                        _dbContext.Entry(existingEntity).CurrentValues.SetValues(travel);
                                        _dbContext.Entry(existingEntity).State = EntityState.Modified;
                                    }

                                    existingClient.Balancce = clientCurrentBalance;
                                    _dbContext.Entry(existingClient).CurrentValues.SetValues(existingClient);
                                    _dbContext.Entry(existingClient).State = EntityState.Modified;
                                }
                            }

                        }
                    }
                }

                await _dbContext.SaveChangesAsync();

            }
        }

        public async Task<List<DateTime>> GetNextPeriod(Transfer model, bool includedCreatedDates = false)
        {
            DateTime nextMonday = DateTime.Now;
            DateTime nextTuesday = DateTime.Now;
            DateTime nextWednesday = DateTime.Now;
            DateTime nextThursday = DateTime.Now;
            DateTime nextFriday = DateTime.Now;
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                int i = 0;
                DateTime date = DateTime.Now;
                while (i < 7)
                {
                    if (date.DayOfWeek == DayOfWeek.Monday)
                    {
                        nextMonday = date.Date;
                        nextTuesday = date.AddDays(1).Date;
                        nextWednesday = date.AddDays(2).Date;
                        nextThursday = date.AddDays(3).Date;
                        nextFriday = date.AddDays(4).Date;
                    }
                    i++;
                    date = date.AddDays(1);
                }

                if (!includedCreatedDates)
                {
                    var alreadyCreatedworWeek = await ChechIfAlreadyCreated(model, nextFriday);
                    if (alreadyCreatedworWeek)
                    {
                        return new List<DateTime>();
                    }
                    else
                        return new List<DateTime>() {
                nextMonday,
                nextTuesday,
                nextWednesday,
                nextThursday,
                nextFriday
                };
                }
                else
                    return new List<DateTime>() {
                nextMonday,
                nextTuesday,
                nextWednesday,
                nextThursday,
                nextFriday
                };
            }
            else
            {
                int i = 0;
                DateTime date = DateTime.Now;
                var thisWeek = new List<DateTime>();

                while (date.DayOfWeek != DayOfWeek.Saturday)
                {
                    thisWeek.Add(date.Date);
                    i++;
                    date = date.AddDays(1);
                }

                if (!includedCreatedDates)
                {
                    var alreadyCreatedworWeek = await ChechIfAlreadyCreated(model, thisWeek.Last());
                    if (alreadyCreatedworWeek)
                    {
                        return new List<DateTime>();
                    }
                    else
                        return thisWeek;
                }
                else
                    return thisWeek;
            }
        }
        public List<DateTime> GetNextWeek()
        {
            DateTime nextMonday = DateTime.Now;
            DateTime nextTuesday = DateTime.Now;
            DateTime nextWednesday = DateTime.Now;
            DateTime nextThursday = DateTime.Now;
            DateTime nextFriday = DateTime.Now;
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                int i = 0;
                DateTime date = DateTime.Now;
                while (i < 7)
                {
                    if (date.DayOfWeek == DayOfWeek.Monday)
                    {
                        nextMonday = date.Date;
                        nextTuesday = date.AddDays(1).Date;
                        nextWednesday = date.AddDays(2).Date;
                        nextThursday = date.AddDays(3).Date;
                        nextFriday = date.AddDays(4).Date;
                    }
                    i++;
                    date = date.AddDays(1);
                }

                        return new List<DateTime>() {
                nextMonday,
                nextTuesday,
                nextWednesday,
                nextThursday,
                nextFriday
                };
            }
            else
            {
                int i = 0;
                DateTime date = DateTime.Now;
                var thisWeek = new List<DateTime>();

                while (date.DayOfWeek != DayOfWeek.Saturday)
                {
                    thisWeek.Add(date.Date);
                    i++;
                    date = date.AddDays(1);
                }

                    return thisWeek;
            }
        }

        public async Task<List<DateTime>> GetNextPeriod(TransferModel model, bool includedCreatedDates = false)
        {
            DateTime nextMonday = DateTime.Now;
            DateTime nextTuesday = DateTime.Now;
            DateTime nextWednesday = DateTime.Now;
            DateTime nextThursday = DateTime.Now;
            DateTime nextFriday = DateTime.Now;
            if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            {
                int i = 0;
                DateTime date = DateTime.Now;
                while (i < 7)
                {
                    if (date.DayOfWeek == DayOfWeek.Monday)
                    {
                        nextMonday = date.Date;
                        nextTuesday = date.AddDays(1).Date;
                        nextWednesday = date.AddDays(2).Date;
                        nextThursday = date.AddDays(3).Date;
                        nextFriday = date.AddDays(4).Date;
                    }
                    i++;
                    date = date.AddDays(1);
                }

                if (!includedCreatedDates)
                {
                    var alreadyCreatedworWeek = await ChechIfAlreadyCreated(model, nextFriday);
                    if (alreadyCreatedworWeek)
                    {
                        return new List<DateTime>();
                    }
                    else
                        return new List<DateTime>() {
                nextMonday,
                nextTuesday,
                nextWednesday,
                nextThursday,
                nextFriday
                };
                }
                else
                    return new List<DateTime>() {
                nextMonday,
                nextTuesday,
                nextWednesday,
                nextThursday,
                nextFriday
                };
            }
            else
            {
                int i = 0;
                DateTime date = DateTime.Now;
                var thisWeek = new List<DateTime>();

                while (date.DayOfWeek != DayOfWeek.Saturday)
                {
                    thisWeek.Add(date.Date);
                    i++;
                    date = date.AddDays(1);
                }

                if (!includedCreatedDates)
                {
                    var alreadyCreatedworWeek = await ChechIfAlreadyCreated(model, thisWeek.Last());
                    if (alreadyCreatedworWeek)
                    {
                        return new List<DateTime>();
                    }
                    else
                        return thisWeek;
                }
                else
                    return thisWeek;
            }
        }
        public async Task ExecuteAddForTransfers(List<Guid> transfersIds)
        {
            var allTransfres = _dbContext.Transfers.Where(v => transfersIds.Contains(v.Id) && !v.IsDeleted).ToList();

            foreach (var transfer in allTransfres)
            {
                var nextDates = await GetNextPeriod(transfer);
                foreach (var nextDate in nextDates)
                {
                    if (nextDate.DayOfWeek == DayOfWeek.Saturday || nextDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        continue;
                    }

                    if (!transfer.DepartureTime.HasValue)
                    {
                        transfer.DepartureTime = DateTime.Today;
                    }

                    Travel nextTravel = new Travel()
                    {
                        From = transfer.From,
                        DisplayName = transfer.Title,
                        Destination = transfer.Destination,
                        DateTime = nextDate + transfer.DepartureTime.Value.TimeOfDay,
                        TransferId = transfer.Id,
                        TravelStatusId = 1,
                        TarrifId = 1,
                        ShuttleId = transfer.ShuttleId,
                        StateCarNumber = "not known yet",
                        Price = 0
                    };

                    var entry = _dbContext.Entry(nextTravel);
                    if (entry.State == EntityState.Detached)
                    {
                        _set.Add(nextTravel);
                    }

                }
                await _dbContext.SaveChangesAsync();

                await CleanDuplicates(transfer, GetNextWeek());
            }
        }

        public async Task ExecuteAddForTransfer(TransferModel transfer)
        {
            var nextDates = await GetNextPeriod(transfer);
            foreach (var nextDate in nextDates)
            {
                if (nextDate.DayOfWeek == DayOfWeek.Saturday || nextDate.DayOfWeek == DayOfWeek.Sunday)
                {
                    continue;
                }
                Travel nextTravel = new Travel()
                {
                    From = transfer.From,
                    DisplayName = transfer.Title,
                    Destination = transfer.Destination,
                    DateTime = nextDate + transfer.DepartureTime.Value.TimeOfDay,
                    TransferId = transfer.Id,
                    TravelStatusId = 1,
                    TarrifId = 1,
                    ShuttleId = transfer.ShuttleId,
                    StateCarNumber = "not known yet",
                    Price = 0
                };

                var entry = _dbContext.Entry(nextTravel);
                if (entry.State == EntityState.Detached)
                {
                    _set.Add(nextTravel);
                    await _dbContext.SaveChangesAsync();
                }

                await CleanDuplicates(transfer, nextDate);
            }
        }

        public async Task ExecuteRemoveForTransfer(TransferModel model)
        {
            var hasActiveClientTransfers = _dbContext.ClientTransfers.Where(n => n.TransferId == model.Id && n.IsActive).Any();
            //create next travels for active transfers
            if (!hasActiveClientTransfers)
            {
                var nextDates = await GetNextPeriod(model, true);
                foreach (var nextDate in nextDates)
                {
                    await CleanAll(model, nextDate);
                }
            }
        }

        private decimal StopFee = 20;
        private decimal PassengerFee = 17;
        private decimal BaseFee = 100;
        private decimal KMFee = 29;
        private int WaitingForConfirmationsStatusId = 1002;
        private int ActiveStatusId = 2;
        public class TransferComparer : IEqualityComparer<TransferModel>
        {
            public bool Equals(TransferModel x, TransferModel y)
            {
                return x.Id == y.Id;
            }

            public int GetHashCode(TransferModel obj)
            {
                return obj.GetHashCode();
            }
        }


        public async Task UpdatePricesForShuttle(Guid ShuttleId)
        {
            try
            {
                var shuttleTransfers = _dbContext.Transfers.Where(v => v.ShuttleId == ShuttleId && !v.IsDeleted).AsNoTracking().ToList();

                await ExecuteAddForTransfers(shuttleTransfers.Select(b => b.Id).ToList());

                var NOW = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));

                var travelsToCompute = (_dbContext.Travels.ToList().Where(v => v.ShuttleId == ShuttleId && v.DateTime.Value.Date >= NOW.Date && !v.IsDeleted)).ToList();
                var dayTavels = travelsToCompute.GroupBy(v => v.DateTime.Value.Date);

                foreach (var dayTravel in dayTavels)
                {
                    var countforshuttle = dayTravel.Count();

                    var lengths = shuttleTransfers.Select(v => new { Id = v.Id, Length = v.Length ?? 0, LengthFromLastStop = v.LengthFromLastRallyPoint ?? 0, StopOrder = v.ShuttleStopOrder ?? 0 }).OrderBy(v => v.StopOrder).ToList();

                    if (countforshuttle == 2)
                    {

                        var firstPass = PassengerFee;
                        var secondPass = PassengerFee;
                        //var finalSumm = BaseFee + lengths[0].Length + lengths[1].LengthFromLastStop + StopFee;
                        var kmPrice = KMFee;// (price - StopFee - BaseFee) / (lengths[0].Length + lengths[1].LengthFromLastStop);

                        firstPass += BaseFee / (decimal)2 + kmPrice * lengths[0].Length / (decimal)2 + StopFee;
                        secondPass += BaseFee / (decimal)2 + kmPrice * lengths[0].Length / (decimal)2 + kmPrice * lengths[1].LengthFromLastStop;

                        //var delta = price - firstPass - secondPass;

                        var firstPassTravels = dayTravel.Where(v => v.TransferId == lengths[0].Id);
                        foreach (var travel in firstPassTravels)
                        {
                            travel.Price = firstPass;
                            travel.TravelStatusId = ActiveStatusId;
                            await UpdateWithoutSave(travel);
                        }
                        var secondPassTravels = dayTravel.Where(v => v.TransferId == lengths[1].Id);
                        foreach (var travel in secondPassTravels)
                        {
                            travel.Price = secondPass;
                            travel.TravelStatusId = ActiveStatusId;
                            await UpdateWithoutSave(travel);
                        }
                    }
                    else
                    if (countforshuttle == 3)
                    {
                        var firstPass = PassengerFee;
                        var secondPass = PassengerFee;
                        var thirdPass = PassengerFee;
                        //var finalSumm = BaseFee + lengths[0].Length + lengths[1].LengthFromLastStop + StopFee;
                        var kmPrice = KMFee;// (price - StopFee - BaseFee) / (lengths[0].Length + lengths[1].LengthFromLastStop + lengths[2].LengthFromLastStop);

                        firstPass += BaseFee / (decimal)3 + kmPrice * lengths[0].Length / (decimal)3 + StopFee + StopFee / (decimal)2;
                        secondPass += BaseFee / (decimal)3 + kmPrice * lengths[0].Length / (decimal)3 + kmPrice * lengths[1].LengthFromLastStop / (decimal)2 + StopFee / (decimal)2;
                        thirdPass += BaseFee / (decimal)3 + kmPrice * lengths[0].Length / (decimal)3 + kmPrice * lengths[1].LengthFromLastStop / (decimal)2 + kmPrice * lengths[2].LengthFromLastStop;

                        //var delta = price - firstPass - secondPass - thirdPass;

                        var firstPassTravels = travelsToCompute.Where(v => v.TransferId == lengths[0].Id);
                        foreach (var travel in firstPassTravels)
                        {
                            travel.Price = firstPass;
                            travel.TravelStatusId = ActiveStatusId;
                            await UpdateWithoutSave(travel);
                        }
                        var secondPassTravels = travelsToCompute.Where(v => v.TransferId == lengths[1].Id);
                        foreach (var travel in secondPassTravels)
                        {
                            travel.TravelStatusId = ActiveStatusId;
                            travel.Price = secondPass;
                            await UpdateWithoutSave(travel);
                        }
                        var thirdPassTravels = travelsToCompute.Where(v => v.TransferId == lengths[2].Id);
                        foreach (var travel in thirdPassTravels)
                        {
                            travel.Price = thirdPass;
                            travel.TravelStatusId = ActiveStatusId;
                            await UpdateWithoutSave(travel);
                        }
                    }
                    else
                    if (countforshuttle == 4)
                    {
                        var firstPass = PassengerFee;
                        var secondPass = PassengerFee;
                        var thirdPass = PassengerFee;
                        var fourthPass = PassengerFee;
                        //var finalSumm = BaseFee + lengths[0].Length + lengths[1].LengthFromLastStop + StopFee;
                        var kmPrice = KMFee;// (price - StopFee - BaseFee) / (lengths[0].Length + lengths[1].LengthFromLastStop + lengths[2].LengthFromLastStop);

                        firstPass += BaseFee / (decimal)4 + kmPrice * lengths[0].Length / (decimal)4 + StopFee + StopFee / (decimal)3 + StopFee / (decimal)2;
                        secondPass += BaseFee / (decimal)4 + kmPrice * lengths[0].Length / (decimal)4 + kmPrice * lengths[1].LengthFromLastStop / (decimal)3 + StopFee / (decimal)3 + StopFee / (decimal)2;
                        thirdPass += BaseFee / (decimal)4 + kmPrice * lengths[0].Length / (decimal)4 + kmPrice * lengths[1].LengthFromLastStop / (decimal)3 + kmPrice * lengths[2].LengthFromLastStop / (decimal)2 + StopFee / (decimal)3;
                        fourthPass += BaseFee / (decimal)4 + kmPrice * lengths[0].Length / (decimal)4 + kmPrice * lengths[1].LengthFromLastStop / (decimal)3 + kmPrice * lengths[2].LengthFromLastStop / (decimal)2 + kmPrice * lengths[3].LengthFromLastStop;

                        //var delta = price - firstPass - secondPass - thirdPass - fourthPass;

                        var firstPassTravels = travelsToCompute.Where(v => v.TransferId == lengths[0].Id);
                        foreach (var travel in firstPassTravels)
                        {
                            travel.Price = firstPass;
                            travel.TravelStatusId = ActiveStatusId;
                            await UpdateWithoutSave(travel);
                        }
                        var secondPassTravels = travelsToCompute.Where(v => v.TransferId == lengths[1].Id);
                        foreach (var travel in secondPassTravels)
                        {
                            travel.Price = secondPass;
                            travel.TravelStatusId = ActiveStatusId;
                            await UpdateWithoutSave(travel);
                        }
                        var thirdPassTravels = travelsToCompute.Where(v => v.TransferId == lengths[2].Id);
                        foreach (var travel in thirdPassTravels)
                        {
                            travel.Price = thirdPass;
                            travel.TravelStatusId = ActiveStatusId;
                            await UpdateWithoutSave(travel);
                        }
                        var fourthPassTravels = travelsToCompute.Where(v => v.TransferId == lengths[3].Id);
                        foreach (var travel in fourthPassTravels)
                        {
                            travel.Price = fourthPass;
                            travel.TravelStatusId = ActiveStatusId;
                            await UpdateWithoutSave(travel);
                        }
                    }
                    else
                    {
                        foreach (var transferToDisable in dayTravel)
                        {
                            transferToDisable.TravelStatusId = WaitingForConfirmationsStatusId;
                            await UpdateWithoutSave(transferToDisable);
                        }
                    }
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception x)
            { 
            
            }
        }

        public async Task<ComplexTravelsModel> GetComplexTravelsModel(string userName)
        {
            ComplexTravelsModel responce = new ComplexTravelsModel();

            var NOW = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"));

            ClientModel client = new ClientModel(_dbContext.Clients.FirstOrDefault(v => v.Email == userName));

            responce.Client = client;

            if (client != null)
            {
                List<TransferViewModel> transfers = new List<TransferViewModel>();
                List<TravelModel> travels = new List<TravelModel>();
                List<Guid> confirmedTravelsIds = new List<Guid>();

                confirmedTravelsIds = _dbContext.Confirmations.Where(v => v.ClientId == client.Id && v.TravelId.HasValue).Select(v => v.TravelId.Value).ToList();

                List<ClientTransfer> clientTransfers = _dbContext.ClientTransfers.AsNoTracking().Where(b => b.ClientId == client.Id).ToList();

                await ExecuteAddForTransfers(clientTransfers.Where(v => v.TransferId.HasValue).Select(b => b.TransferId.Value).ToList());
                var Travels = _dbContext.Travels.AsNoTracking().ToList();
                clientTransfers.ForEach(async (ct) =>
                {
                    if (ct.Transfer != null && !ct.Transfer.IsDeleted)
                    {
                        ct.Transfer.Travels = Travels.Where(b => b.TransferId == ct.TransferId && !b.IsDeleted).ToList();
                        var model = new TransferViewModel(ct.Transfer, true);// ct.Transfer as TransferViewModel;
                        model.IsActive = ct.IsActive;
                        transfers.Add(model);
                    }
                    else
                    if (ct.TransferId.HasValue)
                    {
                        var model = new TransferViewModel(_dbContext.Transfers.AsNoTracking().FirstOrDefault(v => v.Id == ct.TransferId.Value), true);
                        if (!model.IsDeleted)
                        {
                            model.IsActive = ct.IsActive;
                            transfers.Add(model);
                        }
                    }
                });

                var alltravels = transfers.SelectMany(b => b.Travels).Where(v=> !v.IsDeleted).ToList();
                var pasttravels = alltravels.Where(v => v.DateTime.HasValue && v.DateTime.Value.Date < NOW.Date).Take(5).ToList();
                var futuretravels = alltravels.Where(v => v.DateTime.HasValue && v.DateTime.Value.Date >= NOW.Date).ToList();
                futuretravels.ForEach(v => v.Confirmations = v.Confirmations?.Where(n => n.ClientId == client.Id && !n.IsDeleted)?.ToList());
                responce.FutureTravels = futuretravels;
                responce.PastTravels = pasttravels;

                transfers.ForEach((tr) =>
                {
                    tr.Travels = null;// tr.Travels/*?.Where(t => confirmedTravelsIds.Contains(t.Id))*/?.ToList();
                });
                responce.Transfers = transfers;
            }
            return responce;
        }

        public async Task<IEnumerable<TransferViewModel>> GetTransfersList(string userName)
        {
            Client client = _dbContext.Clients.FirstOrDefault(v => v.Email == userName);
            List<TransferViewModel> transfers = new List<TransferViewModel>();
            List<TravelModel> travels = new List<TravelModel>();
            List<Guid> confirmedTravelsIds = new List<Guid>();

            confirmedTravelsIds = _dbContext.Confirmations.Where(v => v.ClientId == client.Id && v.TravelId.HasValue).Select(v => v.TravelId.Value).ToList();

            List<ClientTransfer> clientTransfers = _dbContext.ClientTransfers.Where(b => b.ClientId == client.Id).ToList();

            clientTransfers.ForEach(async (ct) =>
            {
                if (ct.Transfer != null)
                {
                    ct.Transfer.Travels = _dbContext.Travels.Where(b => b.TransferId == ct.TransferId).ToList();
                    var model = new TransferViewModel(ct.Transfer, true);
                    model.IsActive = ct.IsActive;
                    transfers.Add(model);
                }
                else
                if (ct.TransferId.HasValue)
                {
                    var model = new TransferViewModel(_dbContext.Transfers.FirstOrDefault(v => v.Id == ct.TransferId.Value), true);
                    model.IsActive = ct.IsActive;
                    transfers.Add(model);
                }
            });

            transfers.ForEach((tr) =>
            {
                tr.Travels = tr.Travels/*?.Where(t => confirmedTravelsIds.Contains(t.Id))*/?.ToList();
            });

            return transfers;
        }
    }
}