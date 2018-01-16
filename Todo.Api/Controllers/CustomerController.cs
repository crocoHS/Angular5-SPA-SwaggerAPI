using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Todo.Api.Models;
using Todo.Api.Services.Interfaces;

namespace Todo.Api.Controllers
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [Authorize]
        // [Authorize("read:customers")]
        [HttpGet("[action]")]
        public IActionResult GetAll()
        {
            var customers = _customerService.GetAll();
            return new ObjectResult(customers);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult Get(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var customer = _customerService.Get(id);

            if (customer == null)
            {
                return NotFound();
            }

            return new ObjectResult(customer);
        }

        [HttpPost("[action]")]
        public ActionResult Create([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _customerService.Add(customer);

            return Ok(result);
        }

        [HttpPut("[action]")]
        public ActionResult Update([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _customerService.Update(customer);
            return Ok(result);
        }

        [HttpDelete("[action]/{id}")]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var customer = _customerService.Get(id);
            if (customer == null)
            {
                return NotFound();
            }

            _customerService.Delete(id);

            return Ok(id);
        }

        [HttpGet("[action]")]
        public IActionResult GetTechnologyList()
        {
            var technologies = _customerService.GetTechnologyList();
            return new ObjectResult(technologies);
        }

        [Authorize]
        [Authorize("add:technology")]
        [HttpPost("[action]")]
        public ActionResult AddTechnology([FromBody] Technology technology)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _customerService.AddTechnology(technology);

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("[action]/{name}")]
        public ActionResult DeleteTechnology(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return BadRequest();
            }

            var technology = _customerService.GetTechnologyList();
            if (!technology.Any(t => t.TechnologyName == name))
            {
                return NotFound();
            }

            _customerService.DeleteTechnology(name);

            return new OkObjectResult(new { name });
        }
    }
}
