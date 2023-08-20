using System;
namespace HomeWorkMehmetDogan.Models
{
	public class OrderDetailModel
	{
		public int OrderId { get; set; }
		public string  CustomerName { get; set; }
		public DateTime? OrderDate { get; set; }
		public decimal TotalPrice { get; set; }
	}
}

