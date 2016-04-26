namespace Core.Bot.Genetic.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Core.Bot.Genetic.Implementation.Declaration;
    using Core.Bot.Main.Implementation.Declaration;
    using Core.Bot.Main.Implementation.Helpers;
    using Core.TicTacToe.Constants;
    using Core.TicTacToe.Declaration;
    using Core.TicTacToe.Models;

    using Data.Migration.Contexts;
    using Data.Model.GeneticBot;
    using Data.Model.TicTacToe;

    public class GeneticBotDeveloper : IGeneticBotDeveloper
    {
        private readonly INewGameFieldCreator newGameFieldCreator;

        private readonly IFieldStateConverter fieldStateConverter;

        private readonly IGeneticBot geneticBot;

        private readonly IStepMaker stepMaker;

        private readonly IGameProcessStatisticProvider gameProcessStatisticProvider;

        private readonly IMonkeyBot monkeyBot;

        private readonly Random random;

        private Dictionary<int, int> winFactors { get; set; } 

        public GeneticBotDeveloper(
            INewGameFieldCreator newGameFieldCreator,
            IFieldStateConverter fieldStateConverter,
            IGeneticBot geneticBot,
            IStepMaker stepMaker,
            IGameProcessStatisticProvider gameProcessStatisticProvider,
            IMonkeyBot monkeyBot)
        {
            this.newGameFieldCreator = newGameFieldCreator;
            this.fieldStateConverter = fieldStateConverter;
            this.geneticBot = geneticBot;
            this.stepMaker = stepMaker;
            this.gameProcessStatisticProvider = gameProcessStatisticProvider;
            this.monkeyBot = monkeyBot;
            this.random = new Random();
            this.winFactors = new Dictionary<int, int>();
        }

        public void GenerateNextGeneration(TicTacToeContext context)
        {
            var indivadualIds = this.GetIndividualIds(context);

            foreach (var indivadualId in indivadualIds)
            {
                var isCross = true;

                for (var gameNumber = 0; gameNumber < 50; gameNumber++)
                {
                    var gameId = this.CreateNewGame(context);
                    var isGeneticBotTurn = isCross;
                    GameProcessStatistic statistic = null;

                    do
                    {
                        statistic = isGeneticBotTurn ? this.MakeGeneticBotStep(gameId, context) : this.MakeMonkeyBotStep(gameId, context);
                        isGeneticBotTurn = !isGeneticBotTurn;
                    }
                    while (statistic.GameStatus == GameStatus.InProgress);

                    if (statistic.GameStatus == GameStatus.Draw)
                    {
                        this.TakeDraw(gameId, indivadualId, context);
                    }
                    else
                    {
                        this.Win(gameId, indivadualId, isCross, context);
                    }

                    isCross = !isCross;
                }
            }

            var winners = this.TakeThreeWinners(context);

            this.AddMutations(winners, context);
            this.AddIntersections(winners, context);
            this.AddNewMember(winners[0], context);
        }

        public void AddNewMember(GeneticIndividual individual, TicTacToeContext context)
        {
            var newIndividual = new GeneticIndividual
            {
                GenerationNumber = individual.GenerationNumber,
                ImportanceOrder = 10,
                PlayedGames = 0,
                Score = 0,
            };
            context.Set<GeneticIndividual>().Add(newIndividual);
            context.SaveChanges();

            var factors =
                context.Set<GeneticFactor>()
                    .Where(factor => factor.GeneticIndividualId == individual.GeneticIndividualId)
                    .ToList();

            var newFactors = factors.Select(factor => new GeneticFactor
            {
                GeneticIndividualId = newIndividual.GeneticIndividualId,
                Factor = this.random.Next(0, 100),
                FieldId = factor.FieldId
            }).ToList();

            context.Set<GeneticFactor>().AddRange(newFactors);
            context.SaveChanges();
        }

        public void AddIntersections(List<GeneticIndividual> winners, TicTacToeContext context)
        {
            this.AddIntersection(winners[0], winners[1], context);
            this.AddIntersection(winners[0], winners[2], context);
            this.AddIntersection(winners[1], winners[2], context);
        }

        public void AddIntersection(GeneticIndividual first, GeneticIndividual second, TicTacToeContext context)
        {
            var newIndividual = new GeneticIndividual
            {
                GenerationNumber = first.GenerationNumber,
                ImportanceOrder = 7,
                PlayedGames = 0,
                Score = 0,
            };
            context.Set<GeneticIndividual>().Add(newIndividual);
            context.SaveChanges();

            var firstFactors =
                context.Set<GeneticFactor>()
                    .Where(factor => factor.GeneticIndividualId == first.GeneticIndividualId)
                    .ToList();

            var secondFactors =
                context.Set<GeneticFactor>()
                    .Where(factor => factor.GeneticIndividualId == second.GeneticIndividualId)
                    .ToList();

            var newFactors = (from firstFactor in firstFactors
                             join secondFactor in secondFactors on firstFactor.FieldId equals secondFactor.FieldId
                             select new GeneticFactor
                                        {
                                            Factor = (firstFactor.Factor + secondFactor.Factor) / 2,
                                            FieldId = firstFactor.FieldId,
                                            GeneticIndividualId = newIndividual.GeneticIndividualId
                                        }).ToList();
            context.Set<GeneticFactor>().AddRange(newFactors);
            context.SaveChanges();
        }

        public void AddMutations(List<GeneticIndividual> winners, TicTacToeContext context)
        {
            foreach (var winner in winners)
            {
                this.AddMutation(winner, context);
            }
        }

        public void AddMutation(GeneticIndividual individual, TicTacToeContext context)
        {
            var newIndividual = new GeneticIndividual
                                    {
                                        GenerationNumber = individual.GenerationNumber,
                                        ImportanceOrder = 4,
                                        PlayedGames = 0,
                                        Score = 0,
                                    };
            context.Set<GeneticIndividual>().Add(newIndividual);
            context.SaveChanges();

            var factors =
                context.Set<GeneticFactor>()
                    .Where(factor => factor.GeneticIndividualId == individual.GeneticIndividualId)
                    .ToList();

            var newFactors = factors.Select(factor => new GeneticFactor
                                                         {
                                                             GeneticIndividualId = newIndividual.GeneticIndividualId,
                                                             Factor = this.winFactors.ContainsKey(factor.FieldId) 
                                                                        ? factor.Factor + this.winFactors[factor.FieldId] * 5
                                                                        : factor.Factor + this.random.Next(11) - 5,
                                                             FieldId = factor.FieldId
                                                         })
                                    .Select(factor => new GeneticFactor
                                                          {
                                                              GeneticIndividualId = factor.GeneticIndividualId,
                                                              Factor = factor.Factor > 100 ? 100 : factor.Factor < 0 ? 0 : factor.Factor,
                                                              FieldId = factor.FieldId
                                                          }).ToList();
            context.Set<GeneticFactor>().AddRange(newFactors);
            context.SaveChanges();
        }

        public List<GeneticIndividual> TakeThreeWinners(TicTacToeContext context)
        {
            var individuals = context.Set<GeneticIndividual>()
                .OrderByDescending(individual => individual.Score / individual.PlayedGames)
                .ToList();

            var result = individuals.OrderByDescending(individual => individual.Score / individual.PlayedGames).Take(3).ToList();

            for (var order = 0; order < result.Count; order++)
            {
                result[order].ImportanceOrder = order;
                result[order].GenerationNumber++;

                context.Set<GeneticIndividual>().AddOrUpdate(result[order]);
            }

            var individualsToRemove = individuals.Except(result).ToList();
            foreach (var individual in individualsToRemove)
            {
                var factors =
                    context.Set<GeneticFactor>()
                        .Where(factor => factor.GeneticIndividualId == individual.GeneticIndividualId)
                        .ToList();

                foreach (var factor in factors)
                {
                    context.Set<GeneticFactor>().Remove(factor);
                }

                context.Set<GeneticIndividual>().Remove(individual);
            }

            context.SaveChanges();

            return result;
        } 

        public List<int> GetIndividualIds(TicTacToeContext context)
        {
            var result = context.Set<GeneticIndividual>().Select(individual => individual.GeneticIndividualId).ToList();
            return result;
        }

        public int CreateNewGame(TicTacToeContext context)
        {
            var gameField = this.newGameFieldCreator.CreateNewGameField();
            var fieldCode = this.fieldStateConverter.GameFieldToString(gameField);

            var field = GameHelper.GetFieldByCode(fieldCode, context);
            var number = GameHelper.GetCodeNumber(fieldCode, field);

            var game = new Game { Field = field, FieldNumber = number, Proccess = field.FieldId.ToString() };
            context.Set<Game>().Add(game);
            context.SaveChanges();

            var gameId = game.GameId;

            return gameId;
        }

        public GameProcessStatistic MakeGeneticBotStep(int gameId, TicTacToeContext context)
        {
            var game = context.Set<Game>().Include(game1 => game1.Field).FirstOrDefault(game1 => game1.GameId == gameId);
            var field = game.Field;

            var fieldCode = GameHelper.GetFieldByNumber(game.FieldNumber, field);
            var gameField = this.fieldStateConverter.StringToGameField(fieldCode);

            var nextStep = this.geneticBot.GetStep(gameField, context);
            var nextGameField = this.stepMaker.MakeStep(gameField, nextStep.X, nextStep.Y);

            var nextFieldCode = this.fieldStateConverter.GameFieldToString(nextGameField);
            var nextField = GameHelper.GetFieldByCode(nextFieldCode, context);
            var nextNumber = GameHelper.GetCodeNumber(nextFieldCode, nextField);

            var result = this.gameProcessStatisticProvider.GetGameProcessStatistic(nextGameField);

            game.Field = nextField;
            game.FieldNumber = nextNumber;
            game.Proccess += "|" + nextField.FieldId;
            context.Set<Game>().AddOrUpdate(game);
            context.SaveChanges();

            return result;
        }

        public GameProcessStatistic MakeMonkeyBotStep(int gameId, TicTacToeContext context)
        {
            var game = context.Set<Game>().Include(game1 => game1.Field).FirstOrDefault(game1 => game1.GameId == gameId);
            var field = game.Field;

            var fieldCode = GameHelper.GetFieldByNumber(game.FieldNumber, field);
            var gameField = this.fieldStateConverter.StringToGameField(fieldCode);

            var nextStep = this.monkeyBot.GetStep(gameField);
            var nextGameField = this.stepMaker.MakeStep(gameField, nextStep.X, nextStep.Y);

            var nextFieldCode = this.fieldStateConverter.GameFieldToString(nextGameField);
            var nextField = GameHelper.GetFieldByCode(nextFieldCode, context);
            var nextNumber = GameHelper.GetCodeNumber(nextFieldCode, nextField);

            var result = this.gameProcessStatisticProvider.GetGameProcessStatistic(nextGameField);

            game.Field = nextField;
            game.FieldNumber = nextNumber;
            game.Proccess += "|" + nextField.FieldId;
            context.Set<Game>().AddOrUpdate(game);
            context.SaveChanges();
            
            return result;
        }

        public void TakeDraw(int gameId, int geneticIndividualId, TicTacToeContext context)
        {
            var game = context.Set<Game>().Include(game1 => game1.Field).FirstOrDefault(game1 => game1.GameId == gameId);

            GameHelper.RefreshStatistic(game.GameId, GameStatus.Draw, context);

            context.Set<Game>().Remove(game);
            context.SaveChanges();

            var individual =
                context.Set<GeneticIndividual>()
                    .FirstOrDefault(geneticIndividual => geneticIndividual.GeneticIndividualId == geneticIndividualId);
            individual.Score += 0.5;
            individual.PlayedGames++;

            context.Set<GeneticIndividual>().AddOrUpdate(individual);
            context.SaveChanges();
        }

        public void Win(int gameId, int geneticIndividualId, bool isCross, TicTacToeContext context)
        {
            var game = context.Set<Game>().Include(game1 => game1.Field).FirstOrDefault(game1 => game1.GameId == gameId);
            var field = game.Field;

            var fieldCode = GameHelper.GetFieldByNumber(game.FieldNumber, field);
            var gameField = this.fieldStateConverter.StringToGameField(fieldCode);

            var gameProcessStatistic = this.gameProcessStatisticProvider.GetGameProcessStatistic(gameField);

            GameHelper.RefreshStatistic(game.GameId, gameProcessStatistic.GameStatus, context);

            context.Set<Game>().Remove(game);
            context.SaveChanges();

            var fieldIds = game.Proccess.Split('|').Select(int.Parse).ToList();

            var winPowerFactor = 0.6 - fieldIds.Count * 0.1;
            var isCrossWinner = fieldIds.Count % 2 == 0;

            var individual =
                context.Set<GeneticIndividual>()
                    .FirstOrDefault(geneticIndividual => geneticIndividual.GeneticIndividualId == geneticIndividualId);
            individual.Score += (isCross == isCrossWinner) ? (1 + winPowerFactor) : (0 - winPowerFactor);
            individual.PlayedGames++;

            if (isCross == isCrossWinner)
            {
                if (isCross)
                {
                    for (var i = 1; i < fieldIds.Count; i++)
                    {
                        if (i % 2 != 0)
                        {
                            if (this.winFactors.ContainsKey(fieldIds[i]))
                            {
                                this.winFactors[fieldIds[i]]++;
                            }
                            else
                            {
                                this.winFactors.Add(fieldIds[i], 1);
                            }
                        }
                    }
                }
                else
                {
                    for (var i = 1; i < fieldIds.Count; i++)
                    {
                        if (i % 2 == 0)
                        {
                            if (this.winFactors.ContainsKey(fieldIds[i]))
                            {
                                this.winFactors[fieldIds[i]]++;
                            }
                            else
                            {
                                this.winFactors.Add(fieldIds[i], 1);
                            }
                        }
                    }
                }
            }
            else
            {
                if (isCross)
                {
                    for (var i = 1; i < fieldIds.Count; i++)
                    {
                        if (i % 2 != 0)
                        {
                            if (this.winFactors.ContainsKey(fieldIds[i]))
                            {
                                this.winFactors[fieldIds[i]]--;
                            }
                            else
                            {
                                this.winFactors.Add(fieldIds[i], -1);
                            }
                        }
                    }
                }
                else
                {
                    for (var i = 1; i < fieldIds.Count; i++)
                    {
                        if (i % 2 == 0)
                        {
                            if (this.winFactors.ContainsKey(fieldIds[i]))
                            {
                                this.winFactors[fieldIds[i]]--;
                            }
                            else
                            {
                                this.winFactors.Add(fieldIds[i], -1);
                            }
                        }
                    }
                }
            }

            context.Set<GeneticIndividual>().AddOrUpdate(individual);
            context.SaveChanges();
        }
    }
}
