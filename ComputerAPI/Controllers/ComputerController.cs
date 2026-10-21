using CmpShopApi.Models.DTOs;
using ComputerAPI.Migrations;
using ComputerAPI.models;
using ComputerAPI.models.DTOs;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Globalization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace ComputerAPI.Controllers
{
    [Route("computer")]
    [ApiController]
    public class ComputerController : ControllerBase
    {
        public CmpShopDbContext context = new CmpShopDbContext();
        [HttpGet("getAll")]
        public object GetAllComputer()
        {
            var computers = context.Computers.ToList();
            return new { message = "Sikeres lekérdezés", result = computers };
        }

        [HttpPost]
        public object AddNewComputer(AddNewComputerDto addNewComputerDto)
        {
            var computer = new Computer
            {
                Id = Guid.NewGuid(),
                Brand = addNewComputerDto.Brand,
                Type = addNewComputerDto.Type,
                Display = addNewComputerDto.Display,
                Memory = addNewComputerDto.Memory,
                CreatedTime = DateTime.Now,
                OsysId = addNewComputerDto.OsysId,
            };

            context.Computers.Add(computer);
            context.SaveChanges();

            return StatusCode(201, new { message = "Sikeres felvétel", result = computer });
        }

        [HttpPut]
        public object UpdateComputers([FromQuery] Guid id, [FromBody] UpdateComputerDto updateComputerDto)
        {
            var computer = context.Computers.FirstOrDefault(computer => computer.Id == id);

            if (computer != null)
            {
                computer.Brand = updateComputerDto.Brand;
                computer.Type = updateComputerDto.Type;
                computer.Display = updateComputerDto.Display;
                computer.Memory = updateComputerDto.Memory;
                computer.OsysId = updateComputerDto.OsysId;

                context.Computers.Update(computer);
                context.SaveChanges();

                return StatusCode(200, new { message = "Sikeres frissítés", result = computer });
            }

            return StatusCode(404, new { message = "Sikertlen frissítés", result = computer });
        }

        [HttpDelete]
        public object DeleteComputer([FromQuery] Guid id)
        {
            var computer = context.Computers.Find(id);

            if (computer != null)
            {
                context.Computers.Remove(computer);
                context.SaveChanges();

                return StatusCode(204, new { message = "Sikeres törlés" });
            }

            return StatusCode(404, new { message = "Sikertlen törlés" });
        }
        [HttpGet("allComputersAndOsystem")]
        public object AllOsystemAndComputers()
        {
            var computers = context.Osystems.Include(o => o.computers).Select(o => new { o.Name,o.Version,o.computers});
            var cmp = context.Osystems.Join(context.Computers,o => o.Id,c => c.OsysId, (o,c) => new {o.Name,o.Version,c.Brand,c.Type});
            if (cmp != null)
            {
                return StatusCode(200, new { message = "Sikeres lekérdezés", result = cmp });
            }
            return StatusCode(404, new { message = "Üres adatbázis", result = cmp });
        }
        [HttpGet("OneComputerOneOsystem")]
        public object OneComputerOneOsystem([FromQuery]Guid id)
        {
            var computers = context.Computers.Join(context.Osystems,c => c.OsysId,o => o.Id,(c,o) => new { c.Id, c.Brand,c.Type,o.Name,o.Version});
            var result = computers.FirstOrDefault(c => c.Id == id);
            if (result != null)
            {
                return StatusCode(200, new { message = "Sikeres lekérdezés", result = result });
            }
            return StatusCode(404, new { message = "Üres adatbázis", result = result });
        }
        //6. Adott márkájú gépek op-rendszerrel(GET api/query/brand/{ brand})
        [HttpGet("OneBrandComputerawithOsystems")]
        public object OneBrandComputerawithOsystems(string Brand)
        {
            var computers = context.Computers.Join(context.Osystems, c => c.OsysId, o => o.Id, (c, o) => new { c.Brand,o.Name});
            var result = computers.FirstOrDefault(c => c.Brand == Brand);
            if (result != null)
            {
                return StatusCode(200, new { message = "Sikeres lekérdezés", result = result });
            }
            return StatusCode(404, new { message = "Üres adatbázis", result = result });
        }
        //7. Minden gép op - rendszerrel, márka majd típus szerint rendezve(GET api/query/computers-sorted)
        [HttpGet("AllBrandComputerawithOsystems")]
        public object AllBrandComputerawithOsystems()
        {
            var computers = context.Computers.Join(context.Osystems, c => c.OsysId, o => o.Id, (c, o) => new { c.Brand,o.Name});
            if (computers != null)
            {
                return StatusCode(200, new { message = "Sikeres lekérdezés", result = computers });
            }
            return StatusCode(404, new { message = "Üres adatbázis", result = computers });
        }
        //8. Adott képernyőméretnél nagyobb gépek op-rendszerrel(GET api/query/min-display/{ size})

        //10. Gépek op-rendszer neve szerint(by-os)

        //12. Legnagyobb memóriájú gép op-rendszerenként

        //13. Memória és OS - verzió szerinti szűrők

        //14. Legutóbb felvett gépek(latest)

        //15. Márkánkénti összesítés(brand-summary)
        [HttpGet("AllBrandComputers")]
        public object AllBrandComputer()
        {
            var computers = context.Computers.Select(o=> o.Brand);
            if (computers != null)
            {
                return StatusCode(200, new { message = "Sikeres lekérdezés", result = computers });
            }
            return StatusCode(404, new { message = "Üres adatbázis", result = computers });
        }
    }
}
