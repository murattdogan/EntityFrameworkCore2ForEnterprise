using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Store.Core.DataLayer.Contracts;
using Store.Core.DataLayer.DataContracts;
using Store.Core.EntityLayer.HumanResources;
using Store.Core.EntityLayer.Sales;

namespace Store.Core.DataLayer.Repositories
{
    public class SalesRepository : Repository, ISalesRepository
    {
        public SalesRepository(IUserInfo userInfo, StoreDbContext dbContext)
            : base(userInfo, dbContext)
        {
        }

        public IQueryable<Customer> GetCustomers(Int32 pageSize = 10, Int32 pageNumber = 1)
            => DbContext.Paging<Customer>(pageSize, pageNumber);

        public async Task<Customer> GetCustomerAsync(Customer entity)
        {
            return await DbContext
                .Set<Customer>()
                .FirstOrDefaultAsync(item => item.CustomerID == entity.CustomerID);
        }

        public async Task<Int32> AddCustomerAsync(Customer entity)
        {
            Add(entity);

            return await CommitChangesAsync();
        }

        public async Task<Int32> UpdateCustomerAsync(Customer changes)
        {
            var entity = await GetCustomerAsync(changes);

            if (entity != null)
            {
                entity.CompanyName = changes.CompanyName;
                entity.ContactName = changes.ContactName;
            }

            return await CommitChangesAsync();
        }

        public async Task<Int32> DeleteCustomerAsync(Customer entity)
        {
            Remove(entity);

            return await CommitChangesAsync();
        }

        public IQueryable<OrderInfo> GetOrders(Int32 pageSize = 10, Int32 pageNumber = 1, Int16? orderStatusID = null, Int32? customerID = null, Int32? employeeID = null, Int32? shipperID = null)
        {
            var query = from order in DbContext.Set<Order>()
                        join customer in DbContext.Set<Customer>() on order.CustomerID equals customer.CustomerID
                        join employeeJoin in DbContext.Set<Employee>() on order.EmployeeID equals employeeJoin.EmployeeID into employeeTemp
                        from employee in employeeTemp.Where(relation => relation.EmployeeID == order.EmployeeID).DefaultIfEmpty()
                        join orderStatus in DbContext.Set<OrderStatus>() on order.OrderStatusID equals orderStatus.OrderStatusID
                        join shipperJoin in DbContext.Set<Shipper>() on order.ShipperID equals shipperJoin.ShipperID into shipperTemp
                        from shipper in shipperTemp.Where(relation => relation.ShipperID == order.ShipperID).DefaultIfEmpty()
                        select new OrderInfo
                        {
                            OrderID = order.OrderID,
                            OrderStatusID = order.OrderStatusID,
                            OrderDate = order.OrderDate,
                            CustomerID = order.CustomerID,
                            EmployeeID = order.EmployeeID,
                            ShipperID = order.ShipperID,
                            Total = order.Total,
                            Comments = order.Comments,
                            CreationUser = order.CreationUser,
                            CreationDateTime = order.CreationDateTime,
                            LastUpdateUser = order.LastUpdateUser,
                            LastUpdateDateTime = order.LastUpdateDateTime,
                            Timestamp = order.Timestamp,
                            CustomerCompanyName = customer == null ? String.Empty : customer.CompanyName,
                            CustomerContactName = customer == null ? String.Empty : customer.ContactName,
                            EmployeeFirstName = employee.FirstName,
                            EmployeeMiddleName = employee == null ? String.Empty : employee.MiddleName,
                            EmployeeLastName = employee.LastName,
                            EmployeeBirthDate = employee.BirthDate,
                            OrderStatusDescription = orderStatus.Description,
                            ShipperCompanyName = shipper == null ? String.Empty : shipper.CompanyName,
                            ShipperContactName = shipper == null ? String.Empty : shipper.ContactName,
                        };

            if (orderStatusID.HasValue)
            {
                query = query.Where(item => item.OrderStatusID == orderStatusID);
            }

            if (customerID.HasValue)
            {
                query = query.Where(item => item.CustomerID == customerID);
            }

            if (employeeID.HasValue)
            {
                query = query.Where(item => item.EmployeeID == employeeID);
            }

            if (shipperID.HasValue)
            {
                query = query.Where(item => item.ShipperID == shipperID);
            }

            return query.Paging(pageSize, pageNumber);
        }

        public async Task<Order> GetOrderAsync(Order entity)
        {
            return await DbContext
                .Set<Order>()
                .Include(p => p.OrderDetails)
                .FirstOrDefaultAsync(item => item.OrderID == entity.OrderID);
        }

        public Task<Int32> AddOrderAsync(Order entity)
        {
            Add(entity);

            return CommitChangesAsync();
        }

        public async Task<Int32> UpdateOrderAsync(Order changes)
        {
            var entity = await GetOrderAsync(changes);

            if (entity != null)
            {
                entity.OrderDate = changes.OrderDate;
                entity.CustomerID = changes.CustomerID;
                entity.EmployeeID = changes.EmployeeID;
                entity.ShipperID = changes.ShipperID;
                entity.Total = changes.Total;
                entity.Comments = changes.Comments;

                Update(entity);
            }

            return await CommitChangesAsync();
        }

        public async Task<Int32> DeleteOrderAsync(Order entity)
        {
            Remove(entity);

            return await CommitChangesAsync();
        }

        public async Task<OrderDetail> GetOrderDetailAsync(OrderDetail entity)
        {
            return await DbContext
                .Set<OrderDetail>()
                .FirstOrDefaultAsync(item => item.OrderID == entity.OrderID && item.ProductID == entity.ProductID);
        }

        public Task<Int32> AddOrderDetailAsync(OrderDetail entity)
        {
            Add(entity);

            return CommitChangesAsync();
        }

        public async Task<Int32> UpdateOrderDetailAsync(OrderDetail changes)
        {
            var entity = await GetOrderDetailAsync(changes);

            if (entity != null)
            {
                entity.ProductID = changes.ProductID;
                entity.ProductName = changes.ProductName;
                entity.UnitPrice = changes.UnitPrice;
                entity.Quantity = changes.Quantity;
                entity.Total = changes.Total;
            }

            return await CommitChangesAsync();
        }

        public async Task<Int32> DeleteOrderDetailAsync(OrderDetail entity)
        {
            Remove(entity);

            return await CommitChangesAsync();
        }

        public IQueryable<Shipper> GetShippers(Int32 pageSize, Int32 pageNumber)
            => DbContext.Paging<Shipper>(pageSize, pageNumber);

        public async Task<Shipper> GetShipperAsync(Shipper entity)
        {
            return await DbContext
                .Set<Shipper>()
                .FirstOrDefaultAsync(item => item.ShipperID == entity.ShipperID);
        }

        public async Task<Int32> AddShipperAsync(Shipper entity)
        {
            Add(entity);

            return await CommitChangesAsync();
        }

        public async Task<Int32> UpdateShipperAsync(Shipper changes)
        {
            var entity = await GetShipperAsync(changes);

            if (entity != null)
            {
                entity.CompanyName = changes.CompanyName;
                entity.ContactName = changes.ContactName;
            }

            return await CommitChangesAsync();
        }

        public async Task<Int32> DeleteShipperAsync(Shipper entity)
        {
            Remove(entity);

            return await CommitChangesAsync();
        }

        public IQueryable<OrderStatus> GetOrderStatus(Int32 pageSize = 10, Int32 pageNumber = 1)
            => DbContext.Paging<OrderStatus>(pageSize, pageNumber);

        public async Task<OrderStatus> GetOrderStatusAsync(OrderStatus entity)
            => await DbContext.Set<OrderStatus>().FirstOrDefaultAsync(item => item.OrderStatusID == entity.OrderStatusID);

        public async Task<Int32> AddOrderStatusAsync(OrderStatus entity)
        {
            Add(entity);

            return await CommitChangesAsync();
        }

        public async Task<Int32> UpdateOrderStatusAsync(OrderStatus changes)
        {
            Update(changes);

            return await CommitChangesAsync();
        }

        public async Task<Int32> RemoveOrderStatusAsync(OrderStatus entity)
        {
            Remove(entity);

            return await CommitChangesAsync();
        }
    }
}
