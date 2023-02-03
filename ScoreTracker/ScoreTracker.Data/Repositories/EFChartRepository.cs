using Microsoft.EntityFrameworkCore;
using ScoreTracker.Data.Persistence;
using ScoreTracker.Data.Persistence.Entities;
using ScoreTracker.Domain.Enums;
using ScoreTracker.Domain.Models;
using ScoreTracker.Domain.SecondaryPorts;
using ScoreTracker.Domain.ValueTypes;

namespace ScoreTracker.Data.Repositories;

public sealed class EFChartRepository : IChartRepository
{
    private readonly ChartAttemptDbContext _database;

    public EFChartRepository(ChartAttemptDbContext database)
    {
        _database = database;
    }

    public async Task<IEnumerable<Chart>> GetCharts(DifficultyLevel? level = null, ChartType? type = null,
        CancellationToken cancellationToken = default)
    {
        var query = _database.Chart.AsQueryable();
        if (level != null)
        {
            var levelInt = (int)level.Value;
            query = query.Where(c => c.Level == levelInt);
        }

        if (type != null)
        {
            var typeString = type.Value.ToString();
            query = query.Where(c => c.Type == typeString);
        }

        return await (from c in query
                join s in _database.Song on c.SongId equals s.Id
                select new Chart(c.Id, new Song(s.Name, new Uri(s.ImagePath)), Enum.Parse<ChartType>(c.Type), c.Level))
            .ToArrayAsync(cancellationToken);
    }

    public async Task<IEnumerable<Name>> GetSongNames(CancellationToken cancellationToken = default)
    {
        return (await _database.Song.Select(s => s.Name).ToArrayAsync(cancellationToken)).Select(Name.From);
    }

    public async Task<Chart> GetChart(Guid chartId, CancellationToken cancellationToken = default)
    {
        return await (from c in _database.Chart
                join s in _database.Song on c.SongId equals s.Id
                where c.Id == chartId
                select new Chart(c.Id, new Song(s.Name, new Uri(s.ImagePath)), Enum.Parse<ChartType>(c.Type), c.Level))
            .SingleAsync(cancellationToken);
    }


    public async Task<IEnumerable<Chart>> GetChartsForSong(Name songName, CancellationToken cancellationToken = default)
    {
        var nameString = (string)songName;
        return await (from s in _database.Song
                join c in _database.Chart on s.Id equals c.SongId
                where s.Name == nameString
                select new Chart(c.Id, new Song(s.Name, new Uri(s.ImagePath)), Enum.Parse<ChartType>(c.Type), c.Level))
            .ToArrayAsync(cancellationToken);
    }


    public async Task<IEnumerable<Chart>> GetCoOpCharts(CancellationToken cancellationToken = default)
    {
        return await (from c in _database.Chart
                join s in _database.Song on c.SongId equals s.Id
                where c.Type == ChartType.CoOp.ToString()
                select new Chart(c.Id, new Song(s.Name, new Uri(s.ImagePath)), Enum.Parse<ChartType>(c.Type), c.Level))
            .ToArrayAsync(cancellationToken);
    }

    public async Task<IEnumerable<ChartVideoInformation>> GetChartVideoInformation(
        IEnumerable<Guid>? chartIds = default, CancellationToken cancellationToken = default)
    {
        IQueryable<ChartVideoEntity> query = _database.ChartVideo;
        if (chartIds != null)
        {
            var chartIdArray = chartIds.ToArray();
            query = query.Where(c => chartIdArray.Contains(c.ChartId));
        }

        return await query.Select(c => new ChartVideoInformation(c.ChartId, new Uri(c.VideoUrl), c.ChannelName))
            .ToArrayAsync(cancellationToken);
    }
}