namespace MySportsClubManager.Services.Data
{
    using System;
    using System.Threading.Tasks;
    using MySportsClubManager.Data.Common.Repositories;
    using MySportsClubManager.Data.Models;
    using MySportsClubManager.Services.Data.Contracts;
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
    }
}
