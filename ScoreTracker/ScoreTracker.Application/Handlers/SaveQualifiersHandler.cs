﻿using MediatR;
using ScoreTracker.Application.Commands;
using ScoreTracker.Domain.SecondaryPorts;

namespace ScoreTracker.Application.Handlers
{
    public sealed class SaveQualifiersHandler : IRequestHandler<SaveQualifiersCommand>
    {
        private readonly IQualifiersRepository _qualifiers;
        private readonly IBotClient _botClient;

        public SaveQualifiersHandler(IQualifiersRepository qualifiers, IBotClient botClient)
        {
            _botClient = botClient;
            _qualifiers = qualifiers;
        }

        public async Task<Unit> Handle(SaveQualifiersCommand request, CancellationToken cancellationToken)
        {
            var previousLeaderboard =
                await _qualifiers.GetAllUserQualifiers(request.Qualifiers.Configuration, cancellationToken);
            await _qualifiers.SaveQualifiers(request.Qualifiers, cancellationToken);
            var newLeaderboard =
                await _qualifiers.GetAllUserQualifiers(request.Qualifiers.Configuration, cancellationToken);

            var user = request.Qualifiers.UserName;
            var orderedOldLeaderboard = previousLeaderboard.OrderByDescending(q => q.CalculateScore())
                .Select((q, i) => (q, i + 1)).ToArray();

            var orderedNewLeaderboard = newLeaderboard.OrderByDescending(q => q.CalculateScore())
                .Select((q, i) => (q, i + 1)).ToArray();

            if (orderedOldLeaderboard.All(o => o.q.UserName != user))
            {
                await _botClient.PublishQualifiersMessage(
                    $"A new challenger approaches! Welcome {user} to the qualifier leaderboard!", cancellationToken);
                return Unit.Value;
            }


            var oldPlace = orderedOldLeaderboard.First(kv => kv.q.UserName == user).Item2;
            var newPlace = orderedNewLeaderboard.First(kv => kv.q.UserName == user).Item2;
            if (oldPlace != newPlace)
                await _botClient.PublishQualifiersMessage($"{user} has progressed to {newPlace} on the leaderboard!",
                    cancellationToken);

            return Unit.Value;
        }
    }
}