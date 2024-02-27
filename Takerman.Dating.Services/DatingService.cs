﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Takerman.Dating.Data;
using Takerman.Dating.Models.DTOs;
using Takerman.Dating.Services.Abstraction;

namespace Takerman.Dating.Services
{
    public class DatingService(DefaultContext context, ILogger<DatingService> logger) : IDatingService
    {
        private readonly DefaultContext _context = context;
        private readonly ILogger<DatingService> _logger = logger;

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

            var dates = await query.ToListAsync();

            foreach (var date in dates)
            {
                var card = await GetCardFromDate(userId, date);
                result.Add(card);
            }
            return result;
        }

        public async Task<DateCardDto> GetCard(int? userId, int dateId)
        {
            var date = await Get(dateId);
            return await GetCardFromDate(userId, date);
        }

        public async Task<DateCardDto> GetCardFromDate(int? userId, Date date)
        {
            var card = _mapper.Map(date, new DateCardDto());
            card.Ethnicity = date.Ethnicity.GetDisplay();
            card.Status = Enum.GetName(date.Status);
            if (userId.HasValue)
            {
                var isSpotSaved = _context.UserSavedSpots.Any(x => x.UserId == userId.Value && x.DateId == date.Id);
                if (isSpotSaved)
                    card.Status = Enum.GetName(DateStatus.SavedSpot);

                var isDateBought = _context.Orders.Any(x => x.UserId == userId.Value && x.DateId == date.Id);
                if (isDateBought)
                    card.Status = Enum.GetName(DateStatus.Bought);
            }
            card.DateType = date.DateType.GetDisplay();
            card.StartsOn = date.StartsOn.HasValue ? date.StartsOn.Value.ToShortDateString() : string.Empty;
            return card;
        }

        public async Task<IEnumerable<DateChoicesDto>> GetChoices(int userId, int dateId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId) ?? throw new UnauthorizedAccessException("There is no user with such Id");
            var myChoices = await _context.DateUserChoices.Where(x => x.UserId == userId && x.DateId == dateId).ToListAsync();
            var dateUserIds = await _context.Orders.Where(x => x.DateId == dateId).Select(x => x.UserId).ToListAsync();
            var dateUsers = await _context.Users.Include(x=>x.Pictures).Where(x => dateUserIds.Contains(x.Id) && x.Gender != user.Gender).ToListAsync();

            var result = new List<DateChoicesDto>();

            foreach (var dateUser in dateUsers)
            {
                var choice = new DateChoicesDto();
                var theirChoice = await _context.DateUserChoices.FirstOrDefaultAsync(x => x.UserId == dateUser.Id && x.DateId == dateId && x.VoteForId == userId);
                var radios = new List<ChoiceRadioDto>()
                {
                    new() { Label = Enum.GetName(typeof(ChoiceType), ChoiceType.Yes), IsCheched = myChoices.FirstOrDefault(x=>x.VoteForId == dateUser.Id && x.ChoiceType == ChoiceType.Yes) != null},
                    new() { Label = Enum.GetName(typeof(ChoiceType), ChoiceType.No), IsCheched = myChoices.FirstOrDefault(x=>x.VoteForId == dateUser.Id && x.ChoiceType == ChoiceType.No) != null},
                    new() { Label = Enum.GetName(typeof(ChoiceType), ChoiceType.Friend), IsCheched = myChoices.FirstOrDefault(x=>x.VoteForId == dateUser.Id && x.ChoiceType == ChoiceType.Friend) != null}
                };
                choice.Radios = radios;
                choice.TheirChoice = theirChoice == null ? string.Empty : Enum.GetName(typeof(ChoiceType), theirChoice.ChoiceType);
                choice.Avatar = dateUser.Pictures.FirstOrDefault()?.Picture;
                choice.Name = dateUser.FirstName + " " + dateUser.LastName;
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

            return result.ToList();
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
    }
}