using Orders.Models.DTOs;
using SharedKernel.BaseClasses;
using SharedKernel.IntegrationEvents.Orders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Orders.Models
{
    public class Order : Entity<string>
    {
        #region Contructors

        public Order()
        {
        }

        public Order(string id, string serverName, IEnumerable<OrderDetail> details, DateTimeOffset createdDate, DateTimeOffset? changedDate = null)
            : this(serverName, details, createdDate, changedDate)
        {
            this.Id = id;
        }

        public Order(string serverName, IEnumerable<OrderDetail> details, DateTimeOffset createdDate, DateTimeOffset? changedDate = null)
        {
            this.ServerName = serverName;
            this.Details = details;
            this.Status = OrderStatus.WorkingOn;
            this.CreatedDate = createdDate;
            this.ChangedDate = changedDate;
        }

        #endregion Contructors

        #region Properties

        public string ServerName { get; private set; }
        public IEnumerable<OrderDetail> Details { get; private set; }
        public double Total => this.Details.Sum(o => o.Price);
        public OrderStatus Status { get; private set; }

        public DateTimeOffset CreatedDate { get; private set; }
        public DateTimeOffset? ChangedDate { get; private set; }

        #endregion Properties

        #region Methods

        public void Create(string serverName, IEnumerable<OrderDetail> details, DateTimeOffset? changedDate = null)
        {
            this.Id = Guid.NewGuid().ToString();
            this.Details = details;
            this.Status = OrderStatus.Cancel;
            this.CreatedDate = DateTimeOffset.Now;
            this.ChangedDate = changedDate;

            this.PushEvent(new OrderCreatedEvent(
                1,
                this.Id,
                this.Details.ToDictionary(
                    key => key.ItemName,
                    val => val.Quntaty)
             ));
        }

        public void ChangeItems(IEnumerable<OrderDetailDTO> changeItems, IEnumerable<OrderDetailDTO> newItems, IEnumerable<OrderDetailDTO> removeItems)
        {
            OrderDetailDTO change = null;
            this.Details = this.Details
                .Select(o =>
                {
                    change = changeItems.FirstOrDefault(c => c.ItemName.Equals(c.ItemName));
                    return change ?? new OrderDetailDTO { ItemName = o.ItemName, Quntity = o.Quntaty, Price = o.Price };
                })
                .Except(newItems)
                .Union(removeItems)
                .Select(o => new OrderDetail(o.ItemName, o.Quntity, o.Price));

            this.PushEvent(new OrderChangedEvent(
                    this.Id,
                    1,
                    newItems.Select(o => Tuple.Create<string, uint, double>(o.ItemName, o.Quntity, o.Price)),
                    changeItems.Select(o => Tuple.Create<string, uint, double>(o.ItemName, o.Quntity, o.Price)),
                    removeItems.Select(o => Tuple.Create<string, uint, double>(o.ItemName, o.Quntity, o.Price))
                ));
        }

        public void Cancel()
        {
            this.Status = OrderStatus.Cancel;

            this.PushEvent(new OrderCanceledEvent(1, this.Id));
        }

        #endregion Methods
    }
}