using FullStackApi.Data;
using FullStackApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FullStackApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeController : Controller
    {
        private readonly FullStackDbContext _fullStackDbContext;

        public EmployeController(FullStackDbContext fullStackDbContext)
        {
            _fullStackDbContext = fullStackDbContext;
        }

        [HttpGet]
        public async Task <IActionResult> GetEmployeeData()
        {
           var empData = await _fullStackDbContext.Employees.ToListAsync();
            return Ok(empData);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployeeData([FromBody] Employee employee)
        {
            employee.Id = Guid.NewGuid();

            await _fullStackDbContext.Employees.AddAsync(employee);
            await _fullStackDbContext.SaveChangesAsync();
            return Ok(employee);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetEmployeeDataById([FromRoute] Guid id)
        {
            var empData = await _fullStackDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            if(empData == null)
            {
                return NotFound();
            }

            return Ok(empData);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateEmployeeData([FromRoute] Guid id, Employee employeeRequest)
        {
            var empData = await _fullStackDbContext.Employees.FindAsync(id);
            if (empData == null)
            {
                return NotFound();
            }
            //empData.Id = employeeRequest.Id;
            empData.Name = employeeRequest.Name;
            empData.email = employeeRequest.email;
            empData.Phone = employeeRequest.Phone;
            empData.Salary = employeeRequest.Salary;
            empData.Department = employeeRequest.Department;

            await _fullStackDbContext.SaveChangesAsync();

            return Ok(empData);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteEmployeeDataById([FromRoute] Guid id)
        {
            var empData = await _fullStackDbContext.Employees.FindAsync(id);
            if (empData == null)
            {
                return NotFound();
            }

            _fullStackDbContext.Remove(empData);
            await _fullStackDbContext.SaveChangesAsync();

            return Ok(empData);
        }

    }
}
