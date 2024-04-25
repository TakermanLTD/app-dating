using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Globalization;
using Takerman.Dating.Data;
using Takerman.Dating.Models.DTOs;
using Takerman.Dating.Services.Abstraction;

namespace Takerman.Dating.Services
{
    public class DatingService(DefaultContext _context, ILogger<DatingService> _logger) : IDatingService
    {
        private readonly IMapper _mapper = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DateCardDto, Date>();
            cfg.CreateMap<Date, DateCardDto>();
        }).CreateMapper();

        public async Task Buy(int userId, int dateId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Date>> Filter(DateFilterDto filter)
        {
            throw new NotImplementedException();
        }

        public async Task<Date> Get(int id)
        {
            return await _context.Dates.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Date>> GetAll()
        {
            return await _context.Dates.ToListAsync();
        }

        public async Task<IEnumerable<DateCardDto>> GetAllAsCards(int? userId, FilterDto filter)
        {
            var result = new List<DateCardDto>();

            var query = _context.Dates.AsQueryable();

            if (filter != null)
            {
                if (filter.MinAges != 0)
                    query = query.Where(x => x.MinAges >= filter.MinAges);

                if (filter.MaxAges != 0)
                    query = query.Where(x => x.MaxAges <= filter.MaxAges);

                if (filter.MaxPrice != 0)
                    query = query.Where(x => x.Price <= filter.MaxPrice);

                if (filter.Ethnicity.HasValue && filter.Ethnicity != 0)
                    query = query.Where(x => (int)x.Ethnicity == filter.Ethnicity);

                if (filter.DateType.HasValue && filter.DateType != 0)
                    query = query.Where(x => (int)x.DateType == filter.DateType);
            }

            var dates = await query
                .OrderByDescending(x => x.StartsOn == null)
                .ThenByDescending(x => x.StartsOn)
                .ThenBy(x => x.MinAges)
                .ThenBy(x => x.Ethnicity)
                .ToListAsync();

            foreach (var date in dates)
            {
                // await Maintenance(date);

                var card = await GetCardFromDate(userId, date);
                result.Add(card);
            }
            return result;
        }

        private async Task Maintenance(Date? date)
        {
            if (date.StartsOn == null || !date.StartsOn.HasValue)
            {
                date.Status = DateStatus.NotApproved;
            }
            else if (date.StartsOn > DateTime.Now)
            {
                date.Status = DateStatus.Approved;
            }
            else if (date.StartsOn.Value < DateTime.Now.AddDays(-2))
            {
                date.Status = DateStatus.ResultsRevealed;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<DateCardDto> GetCard(int dateId)
        {
            var date = await Get(dateId);
            return await GetCardFromDate(date);
        }

        public async Task<DateCardDto> GetCard(int? userId, int dateId)
        {
            var date = await Get(dateId);
            return await GetCardFromDate(userId, date);
        }

        public async Task<DateCardDto> GetCardFromDate(Date date)
        {
            var card = _mapper.Map(date, new DateCardDto());
            card.Ethnicity = date.Ethnicity.GetDisplay();
            card.Status = Enum.GetName(date.Status);
            card.DateType = date.DateType.GetDisplay();

            if (date.StartsOn.HasValue)
                card.StartsOn = date.StartsOn.Value.ToString(CultureInfo.InvariantCulture);

            return card;
        }

        public async Task<DateCardDto> GetCardFromDate(int? userId, Date date)
        {
            var card = await GetCardFromDate(date);

            if (userId.HasValue)
            {
                var isSpotSaved = _context.UserSavedSpots.Any(x => x.UserId == userId.Value && x.DateId == date.Id);
                if (isSpotSaved)
                    card.Status = Enum.GetName(DateStatus.SavedSpot);

                var isDateBought = _context.Orders.Any(x => x.UserId == userId.Value && x.DateId == date.Id && date.StartsOn > DateTime.Now);
                if (isDateBought && date.Status != DateStatus.Started)
                    card.Status = Enum.GetName(DateStatus.Bought);
            }

            return card;
        }

        public async Task<IEnumerable<DateChoiceDto>> GetChoices(int userId, int dateId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId) ?? throw new UnauthorizedAccessException("There is no user with such Id");
            var myChoices = await _context.DateUserChoices.Where(x => x.UserId == userId && x.DateId == dateId).ToListAsync();
            var dateUserIds = await _context.Orders.Where(x => x.DateId == dateId).Select(x => x.UserId).ToListAsync();
            var dateUsers = await _context.Users.Include(x => x.Pictures).Where(x => dateUserIds.Contains(x.Id) && x.Gender != user.Gender).ToListAsync();

            var result = new List<DateChoiceDto>();

            foreach (var dateUser in dateUsers)
            {
                var choice = new DateChoiceDto();
                var theirChoice = await _context.DateUserChoices.FirstOrDefaultAsync(x => x.UserId == dateUser.Id && x.DateId == dateId && x.VoteForId == userId);
                var myChoice = myChoices.FirstOrDefault(x => x.VoteForId == dateUser.Id);
                var radios = new List<ChoiceRadioDto>()
                {
                    new() { Label = Enum.GetName(typeof(ChoiceType), ChoiceType.Yes), IsChecked = myChoices.FirstOrDefault(x=>x.VoteForId == dateUser.Id && x.ChoiceType == ChoiceType.Yes) != null},
                    new() { Label = Enum.GetName(typeof(ChoiceType), ChoiceType.No), IsChecked = myChoices.FirstOrDefault(x=>x.VoteForId == dateUser.Id && x.ChoiceType == ChoiceType.No) != null},
                    new() { Label = Enum.GetName(typeof(ChoiceType), ChoiceType.Friend), IsChecked = myChoices.FirstOrDefault(x=>x.VoteForId == dateUser.Id && x.ChoiceType == ChoiceType.Friend) != null}
                };
                choice.VoteForId = dateUser.Id;
                choice.Radios = radios;
                choice.TheirChoice = theirChoice == null ? string.Empty : Enum.GetName(typeof(ChoiceType), theirChoice.ChoiceType);
                choice.Avatar = dateUser.Pictures.FirstOrDefault()?.Url;
                choice.Name = dateUser.FirstName + " " + dateUser.LastName;
                choice.MyChoice = myChoice == null ? string.Empty : Enum.GetName(typeof(ChoiceType), myChoice.ChoiceType);
                result.Add(choice);
            }

            return result;
        }

        public async Task<IEnumerable<DateUserChoice>> GetDateVotesByUser(int userId, int dateId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DateCardDto>> GetSavedSpots(int userId)
        {
            var savedSpots = await _context.UserSavedSpots.Where(x => x.UserId == userId).ToListAsync();
            var result = new List<DateCardDto>();
            foreach (var spot in savedSpots)
                result.Add(await GetCard(userId, spot.DateId));

            return [.. result];
        }

        public async Task SaveChoices(int userId, int dateId, IEnumerable<DateChoiceDto> choices)
        {
            foreach (var choice in choices)
            {
                var existing = await _context.DateUserChoices.FirstOrDefaultAsync(x => x.UserId == userId && x.DateId == dateId && x.VoteForId == choice.VoteForId);

                var choiceForUser = choice.Radios.FirstOrDefault(x => x.IsChecked);

                if (choiceForUser != null)
                {
                    if (existing != null)
                    {
                        existing.ChoiceType = Enum.Parse<ChoiceType>(choiceForUser.Label);
                    }
                    else
                    {
                        await _context.DateUserChoices.AddAsync(new DateUserChoice()
                        {
                            DateId = dateId,
                            UserId = userId,
                            VoteForId = choice.VoteForId,
                            ChoiceType = Enum.Parse<ChoiceType>(choiceForUser.Label)
                        });
                    }
                }
                else
                {
                    _logger.LogWarning("It was tried to insert empty choice per user in the database.");
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<DateCardDto> SaveSpot(int userId, int dateId)
        {
            var existing = _context.UserSavedSpots.Where(x => x.UserId == userId && x.DateId == dateId);
            if (existing.Any())
            {
                _context.UserSavedSpots.RemoveRange(existing);
                await _context.SaveChangesAsync();
            }

            await _context.UserSavedSpots.AddAsync(new UserSavedSpot
            {
                UserId = userId,
                DateId = dateId,
                SavedOn = DateTime.Now
            });

            await _context.SaveChangesAsync();

            var date = await Get(dateId);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (date != null && user != null)
            {
                if (user.Gender == Gender.Male)
                    date.MenCount = (short)(date.MenCount + 1);
                else if (user.Gender == Gender.Female)
                    date.WomenCount = (short)(date.WomenCount + 1);

                _context.Dates.Update(date);
                await _context.SaveChangesAsync();
            }

            return await GetCardFromDate(userId, date);
        }

        public async Task<DateCardDto> UnsaveSpot(int userId, int dateId)
        {
            var existing = _context.UserSavedSpots.Where(x => x.UserId == userId && x.DateId == dateId);
            if (existing.Any())
            {
                _context.UserSavedSpots.RemoveRange(existing);
                await _context.SaveChangesAsync();
            }

            var date = await Get(dateId);
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (date != null && user != null)
            {
                if (user.Gender == Gender.Male)
                    date.MenCount = (short)(date.MenCount - 1);
                else if (user.Gender == Gender.Female)
                    date.WomenCount = (short)(date.WomenCount - 1);

                _context.Dates.Update(date);
                await _context.SaveChangesAsync();
            }

            return await GetCardFromDate(userId, date);
        }

        public async Task Vote(int userId, int choiceId, ChoiceType choiceType)
        {
            throw new NotImplementedException();
        }

        public async Task<DateCardDto> SetStatus(DateStatusDto status)
        {
            var date = await Get(status.Id);
            date.Status = Enum.Parse<DateStatus>(status.Status);
            _context.SaveChanges();

            return await GetCardFromDate(date);
        }

        public async Task<IEnumerable<int>> GetMatchesIDs(int userId)
        {
            var result = new List<int>();
            var myYes = await _context.DateUserChoices.Where(x => x.UserId == userId && x.ChoiceType == ChoiceType.Yes).ToListAsync();
            var theirYes = await _context.DateUserChoices.Where(x => x.VoteForId == userId && x.ChoiceType == ChoiceType.Yes).ToListAsync();
            foreach (var match in theirYes)
            {
                if (myYes.Select(x => x.DateId).Contains(match.DateId))
                {
                    result.Add(match.UserId);
                }
            }
            return result;
        }

        public async Task<IEnumerable<MatchDto>> GetMatches(int userId)
        {
            var matchesIDs = await GetMatchesIDs(userId);

            var result = new List<MatchDto>();

            foreach (var matchId in matchesIDs)
            {
                var user = await _context.Users.Include(x => x.Pictures).FirstOrDefaultAsync(x => x.Id == matchId);

                result.Add(new MatchDto()
                {
                    UserId = matchId,
                    Name = user.FirstName + " " + user.LastName,
                    Avatar = user.Pictures.FirstOrDefault()?.Url
                });
            }

            return result;
        }

        public async Task Save(Date date)
        {
            date.Orders = await _context.Orders.Where(x => x.DateId == date.Id).ToListAsync();
            date.Attendees = await _context.UserSavedSpots.Where(x => x.DateId == date.Id).Select(x => x.User).ToListAsync();
            _context.Dates.Update(date);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var date = await Get(id);
            _context.Remove(date);
            await _context.SaveChangesAsync();
        }

        public async Task<Date> Add(Date date)
        {
            await _context.Dates.AddAsync(date);
            await _context.SaveChangesAsync();
            return date;
        }

        public async Task SaveAll(IEnumerable<Date> dates)
        {
            _context.Dates.UpdateRange(dates);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAll()
        {
            _context.Dates.RemoveRange(_context.Dates);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSpots()
        {
            _context.UserSavedSpots.RemoveRange(_context.UserSavedSpots);
            foreach (var date in _context.Dates)
            {
                date.MenCount = 0;
                date.WomenCount = 0;
            }
            _context.Dates.UpdateRange(_context.Dates);
            await _context.SaveChangesAsync();
        }
    }
}