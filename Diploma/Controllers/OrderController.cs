using Diploma.Database;
using Diploma.model.equipment;
using Diploma.model.order;
using Diploma.model.provider;
using Diploma.model.warehouse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Diploma.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private EfModel _efModel;

        public OrderController(EfModel model)
        {
            _efModel = model;
        }

        [HttpGet]
        public async Task<List<Order>> GetAll(string? search, bool? warehouse)
        {
            IQueryable<Order> orders = _efModel.Orders
                .Include(u => u.Provider)
                    .ThenInclude(u => u.Post)
                .Include(u => u.Equipment);


            if (!string.IsNullOrEmpty(search))
            {
                var q = search.ToLower().Trim();

                orders = orders.Where(u => u.Equipment.Name.ToLower().Contains(search));
            }

            var list = await orders.ToListAsync();

            if (warehouse != null)
            {
                list = list.Where(u => u.Warehouse == warehouse).ToList();
            }

            return list;
        }

        [HttpGet("Warehouse")]
        public async Task<List<WarehouseOrder>> GetWarehouseAll(string? search, WarehouseState? state)
        {
            IQueryable<WarehouseOrder> orders = _efModel.WarehouseOrders
                .Include(u => u.Provider)
                    .ThenInclude(u => u.Post)
                .Include(u => u.Equipment);

            if (!string.IsNullOrEmpty(search))
            {
                var q = search.ToLower().Trim();

                orders = orders.Where(u => u.Equipment.Name.ToLower().Contains(search));
            }

            if (state != null)
            {
                orders = orders.Where(u => u.State == state);
            }

            return await orders.ToListAsync();
        }


        [Authorize(Roles = "AdminUser")]
        [HttpPost("{id}/Warehouse")]
        public async Task<ActionResult> CreateOrderWarehouse(int id, WarehouseState state)
        {
            var order = await _efModel.Orders.FindAsync(id);

            if (order == null)
                return NotFound();

            var orderWarehouse = new WarehouseOrder
            {
                Id = id,
                State = state,
                SerialNumber = Convert.ToInt32(id + DateTimeOffset.UtcNow.ToUnixTimeSeconds())
            };

            _efModel.Orders.Remove(order);
            await _efModel.WarehouseOrders.AddAsync(orderWarehouse);
            await _efModel.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "AdminUser")]
        [HttpPatch("Warehouse/{id}/State")]
        public async Task<ActionResult> UpdateWarehouseState(WarehouseState state, int id)
        {
            var order = await _efModel.WarehouseOrders.FindAsync(id);

            if (order == null)
                return NotFound();

            order.State = state;

            _efModel.Entry(order).State = EntityState.Modified;
            await _efModel.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "ProviderUser,AdminUser")]
        [HttpPost]
        public async Task<ActionResult> Create(CreateOrderDto dto)
        {
            var provider = await _efModel.Providers.FindAsync(dto.ProviderId);

            if (provider == null)
                return NotFound();

            var equipment = await _efModel.Equipments.FindAsync(dto.EquipmentId);

            if (equipment == null)
                return NotFound();

            var order = new Order
            {
                Equipment = equipment,
                EquipmentCount = dto.EquipmentCount,
                Provider = provider
            };

            await _efModel.Orders.AddAsync(order);
            await _efModel.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "ProviderUser,AdminUser")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Provider>> Update(int id, CreateOrderDto dto)
        {
            var order = await _efModel.Orders.FindAsync(id);

            if (order == null)
                return NotFound();

            var provider = await _efModel.Providers.FindAsync(dto.ProviderId);

            if (provider == null)
                return NotFound();

            var equipment = await _efModel.Equipments.FindAsync(dto.EquipmentId);

            if (equipment == null)
                return NotFound();

            order.EquipmentCount = dto.EquipmentCount;
            order.Equipment = equipment;
            order.Provider = provider;

            _efModel.Entry(provider).State = EntityState.Modified;
            await _efModel.SaveChangesAsync();

            return provider;
        }

        [Authorize(Roles = "ProviderUser,AdminUser")]
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var order = await _efModel.Orders.FindAsync(id);

            if (order == null)
                return NotFound();

            _efModel.Orders.Remove(order);
            await _efModel.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("Equipment")]
        public async Task<ActionResult<List<Equipment>>> GetEquipments(string? search)
        {
            IQueryable<Equipment> equipments = _efModel.Equipments;

            if (!string.IsNullOrEmpty(search))
            {
                equipments = equipments.Where(u => u.Name.ToLower().Contains(search.ToLower().Trim()));
            }

            return await equipments.ToListAsync();
        }
    }
}
