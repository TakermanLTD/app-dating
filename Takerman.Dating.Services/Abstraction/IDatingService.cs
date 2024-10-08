﻿using Takerman.Dating.Data;
using Takerman.Dating.Models.DTOs;

namespace Takerman.Dating.Services.Abstraction
{
    public interface IDatingService
    {
        Task<Date> Get(int id);

        Task<DateCardDto> GetCard(int dateId);

        Task<DateCardDto> GetCard(int? userId, int dateId);

        Task<IEnumerable<Date>> GetAll();

        Task<IEnumerable<DateCardDto>> GetAllAsCards(int? userId, FilterDto filter);

        Task<IEnumerable<Date>> Filter(DateFilterDto filter);

        Task<DateCardDto> GetCardFromDate(Date date);

        Task<DateCardDto> GetCardFromDate(int? userId, int dateId);

        Task<bool> IsBought(int userId, int dateId);

        Task<bool> IsSpotSaved(int userId, int dateId);

        Task<DateCardDto> SaveSpot(int userId, int dateId);

        Task Buy(int userId, int dateId);

        Task<DateCardDto> UnsaveSpot(int userId, int dateId);

        Task<IEnumerable<DateCardDto>> GetSavedSpots(int userId);

        Task<IEnumerable<DateChoiceDto>> GetChoices(int userId, int dateId);

        Task SaveChoices(int userId, int dateId, IEnumerable<DateChoiceDto> choices);

        Task<DateCardDto> SetStatus(DateStatusDto status);

        Task<IEnumerable<int>> GetMatchesIDs(int userId);

        Task<IEnumerable<MatchDto>> GetMatches(int userId);
        
        Task UpdateStatus(int id);
    }
}