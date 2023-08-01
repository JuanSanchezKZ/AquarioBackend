using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PilatesTurnosApi.Data;
using PilatesTurnosApi.Models.Domain;
using PilatesTurnosApi.Models.Domain.DTO;

namespace PilatesTurnosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()

        {
            // Get Data From Database = Domain Models


            var regions = await dbContext.Regions.ToListAsync();


            // Map Domain Models to DTOs

            var regionsDto = new List<RegionDTO>();

            foreach (var region in regions) {

                regionsDto.Add(new RegionDTO()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl,

                });
            
            }


            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id}:Guid")]

        public async Task<IActionResult> GetById([FromRoute] Guid id) {

            // Get Region Domain Model From Database

            var region = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (region == null) {
                return NotFound();
            }

            // Map/Convert Region Domain Model to Region DTO

            var regionDto = new RegionDTO()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl,
            };

              return Ok(regionDto);
        }

        // POST 

        [HttpPost]

        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {

            var regionDomainModel = new Region
            {
                Code = addRegionRequestDTO.Code,
                Name = addRegionRequestDTO.Name,
                RegionImageUrl = addRegionRequestDTO.RegionImageUrl,

            };

            await dbContext.Regions.AddAsync(regionDomainModel);
            await dbContext.SaveChangesAsync();

            var regionDto = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };

            return CreatedAtAction(nameof(GetById), new { id = regionDomainModel.Id}, regionDto);

        }

        // Update region

        [HttpPut]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO )
        {
           var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (regionDomainModel == null) {
                return NotFound();
            }

            // DTO to Domain Model


            regionDomainModel.Code = updateRegionRequestDTO.Code;
            regionDomainModel.Name = updateRegionRequestDTO.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDTO.RegionImageUrl;

            await dbContext.SaveChangesAsync();

            // Convert Domain Model to DTO 

            var regionDto = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };

            return Ok(regionDto);
        }

        // Delete Region

        [HttpDelete]
        [Route("{id:Guid}")]

        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
           var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (regionDomainModel == null)
            {
                return NotFound();
            }

            // Delete Region

            dbContext.Regions.Remove(regionDomainModel);
            await dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
