namespace MySportsClubManager.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Data.Contracts;
    using MySportsClubManager.Services.Mapping;
    using MySportsClubManager.Web.ViewModels.Contest;

    public class ContestService : IContestService
    {
        private readonly IDeletableEntityRepository<Contest> contestRepository;
        private readonly IImageService imageService;

        public ContestService(IDeletableEntityRepository<Contest> contestRepository, IImageService imageService)
        {
            this.contestRepository = contestRepository;
            this.imageService = imageService;
        }

        public async Task<List<T>> AllAsync<T>(int page, int itemsPerPage = 8)
        {
            return await this.contestRepository
                .AllAsNoTracking()
                .OrderByDescending(x => x.CreatedOn)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .To<T>()
                .ToListAsync();
        }

        public async Task CreateAsync(CreateContestViewModel model)
        {
            var image = await this.imageService.AddByFile(model.ImageFile, model.ImageFile.FileName);

            var contest = new Contest()
            {
                Name = model.Name,
                Address = model.Address,
                Description = model.Description,
                Date = model.Date,
                Image = image,
                SportId = model.SportId,
            };

            await this.contestRepository.AddAsync(contest);
            await this.contestRepository.SaveChangesAsync();
        }


        public int GetCount()
        {
            return this.contestRepository.All().Count();
        }

        public async Task<T> GetOneAsync<T>(int id)
        {
            var contest = await this.contestRepository.All()
                .Where(s => s.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            if (contest == null)
            {
                throw new ArgumentException();
            }

            return contest;
        }

        public async Task DeleteAsync(int contestId)
        {
            var contest = await this.contestRepository.All()
            .Where(c => c.Id == contestId)
                .FirstOrDefaultAsync();
            if (contest != null)
            {
                this.contestRepository.Delete(contest);
                await this.contestRepository.SaveChangesAsync();
            }
        }
    }
}
