using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NorthwindModel.Models;
using HomeWorkMehmetDogan.Models;
using System.Linq;
using System.Net;
using System.Diagnostics.Metrics;
using static Azure.Core.HttpHeader;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HomeWorkMehmetDogan.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        // GET: api/values
        [HttpGet]
        public IActionResult GetEmployee()
        {
            //NorthwindContext northwind = new NorthwindContext();
            //List<Employee> employees = northwind.Employees.ToList();
            //return Ok(employees.ToList());
            NorthwindContext context = new NorthwindContext();
            #region Query
            var employees = context.Employees.Select(e => new
            {
                FirstName = e.FirstName,
                LastName = e.LastName,
                Title = e.Title,
                HireDate = e.HireDate
            }).ToList();
            #endregion
            return Ok(employees);
        }
        [HttpGet("{EmployeeId}")]
        public IActionResult GetEmployeeById(int EmployeeId)
        {
            NorthwindContext context = new NorthwindContext();

            EmployeeListModel employee = context.Employees.Where(a => a.EmployeeId == EmployeeId)
                .Select(a => new EmployeeListModel()
                {
                    EmployeeId = a.EmployeeId,
                    FirstName = a.FirstName,
                    LastName = a.LastName,
                    Title = a.Title,
                    TitleOfCourtesy = a.TitleOfCourtesy,
                    BirthDate = a.BirthDate,
                    HireDate = a.HireDate,
                    Address = a.Address,
                    City = a.City,
                    Region = a.Region,
                    PostalCode = a.PostalCode,
                    Country = a.Country,
                    HomePhone = a.HomePhone,
                    Extension = a.Extension,
                    Photo = a.Photo,
                    Notes = a.Notes,
                    ReportsTo = a.ReportsTo,
                    PhotoPath = a.PhotoPath
                }).FirstOrDefault();
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }

        [HttpPost]
        public IActionResult AddEmployee(EmployeeListModel employee)
        {
            Employee employeeContext = new Employee()
            {

                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Title = employee.Title,
                TitleOfCourtesy = employee.TitleOfCourtesy,
                BirthDate = employee.BirthDate,
                HireDate = employee.HireDate,
                Address = employee.Address,
                City = employee.City,
                Region = employee.Region,
                PostalCode = employee.PostalCode,
                Country = employee.Country,
                HomePhone = employee.HomePhone,
                Extension = employee.Extension,
                Photo = employee.Photo,
                Notes = employee.Notes,
                ReportsTo = employee.ReportsTo,
                PhotoPath = employee.PhotoPath
            };
            NorthwindContext context = new NorthwindContext();
            context.Employees.Add(employeeContext);
            context.SaveChanges();
            return CreatedAtAction(nameof(GetEmployeeById), new { EmployeeId = employeeContext.EmployeeId }, employeeContext);
        }

        [HttpDelete("{EmployeeId}")]
        public IActionResult DeleteEmployeeById(int EmployeeId)
        {
            NorthwindContext context = new NorthwindContext();
            Employee employee = context.Employees.FirstOrDefault(a => a.EmployeeId == EmployeeId);
            if (employee==null)
            {
                return NotFound();
            }
            try
            {
                context.Employees.Remove(employee);
                context.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }


        [HttpPut]
        public IActionResult UpdateEmployee()
        {
            return Ok();
        }

        [HttpGet("{EmployeeId}")]
        public IActionResult GetOrdersById(int EmployeeId)
        {
            NorthwindContext context = new NorthwindContext();
            List<OrderDetailModel> result = context.Orders.Where(a => a.EmployeeId == EmployeeId).Select(a => new OrderDetailModel
            {
                OrderId = a.OrderId,
                CustomerName = context.Customers.FirstOrDefault(customer => customer.CustomerId == a.CustomerId).ContactName,
                OrderDate = a.OrderDate,
                TotalPrice = context.OrderDetails.Where(b => b.OrderId == a.OrderId).Select(b => b.UnitPrice * (decimal)(1-b.Discount)).Sum()

            }).ToList();
            return Ok(result);
        }

    }
}

