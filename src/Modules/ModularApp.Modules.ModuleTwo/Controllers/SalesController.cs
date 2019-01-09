using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ModularApp.Modules.ModuleTwo.Data;
using ModularApp.Modules.ModuleTwo.Dto;
using ModularApp.Modules.ModuleTwo.Models;

namespace ModularApp.Modules.ModuleTwo
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private SalesDataContext _context;
        private readonly IMapper _mapper;

        public SalesController(SalesDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        [HttpGet("saveNew")]
        public ActionResult SaveNew()
        {
            var sale = new Sale();
            sale.DateOfPurchase = DateTime.Now;
            sale.Price = 11;
            _context.Sales.Add(sale);
            _context.SaveChanges();
            return Ok(sale);
        }

        [HttpGet("getMine")]
        public ActionResult GetMine()
        {
            var sale = _context.Sales.FirstOrDefault(s => s.Price == 11);
            var saleDto = _mapper.Map<SaleDto>(sale);
            return Ok(saleDto);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
