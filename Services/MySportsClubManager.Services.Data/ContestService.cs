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
    using MySportsClubManager.Web.Infrastructure.Common;
    using MySportsClubManager.Web.ViewModels.Athlete;
    using MySportsClubManager.Web.ViewModels.Contest;

    public class ContestService : IContestService
    {
        private readonly IDeletableEntityRepository<Contest> contestRepository;
        private readonly IRepository<Athlete> athleteRepository;
        private readonly IRepository<Club> clubRepository;
        private readonly IImageService imageService;

        public ContestService(IDeletableEntityRepository<Contest> contestRepository, IImageService imageService, IRepository<Athlete> athleteRepository, IRepository<Club> clubRepository)
        {
            this.contestRepository = contestRepository;
            this.imageService = imageService;
            this.athleteRepository = athleteRepository;
            this.clubRepository = clubRepository;
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

        public async Task EditAsync(EditContestViewModel model)
        {
            var contest = await this.contestRepository.All()
                .Where(x => x.Id == model.Id)
                .FirstOrDefaultAsync();

            if (contest == null)
            {
                throw new ArgumentException();
            }

            Image image = null;
            if (model.ImageFile != null)
            {
                image = await this.imageService.AddByFile(model.ImageFile, model.ImageFile.FileName);
            }
            else
            {
                image = await this.imageService.AddByUrlAsync(model.ImageUrl);
            }

            contest.Name = model.Name;
            contest.Description = model.Description;
            contest.Address = model.Address;
            contest.Date = model.Date;
            contest.Image = image;
            contest.SportId = model.SportId;

            this.contestRepository.Update(contest);
            await this.contestRepository.SaveChangesAsync();
        }

        public async Task Register(int contestId, string userId)
        {
            var contest = await this.contestRepository.All()
            .Where(x => x.Id == contestId)
            .Include(x => x.Participants)
            .FirstOrDefaultAsync();

            if (contest != null)
            {
                if (contest.Date < DateTime.Now)
                {
                    throw new ArgumentException(ExceptionMessages.ContestAlreadyFinished);
                }

                if (contest.Participants.Any(a => a.ApplicationUserId == userId))
                {
                    throw new ArgumentException(ExceptionMessages.AlreadyEnrolledMessage);
                }

                var athlete = await this.athleteRepository.All()
                     .Where(a => a.ApplicationUserId == userId)
                     .FirstOrDefaultAsync();

                contest.Participants.Add(athlete);
                if (athlete.EnrolledClubId != null && !contest.Clubs.Any(x => x.Id == athlete.EnrolledClubId))
                {
                    var club = await this.clubRepository.All()
                        .Where(x => x.Id == athlete.EnrolledClubId)
                        .FirstOrDefaultAsync();

                    contest.Clubs.Add(club);
                }
                else
                {
                    throw new ArgumentException(ExceptionMessages.NotEnrolledMessage);
                }

                this.contestRepository.Update(contest);
                await this.contestRepository.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentNullException();
            }
        }
    }
}
