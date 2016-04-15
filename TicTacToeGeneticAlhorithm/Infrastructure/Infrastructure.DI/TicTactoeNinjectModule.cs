namespace Infrastructure.DI
{
    using Core.TicTacToe.Declaration;
    using Core.TicTacToe.Implementation;

    using Ninject.Modules;

    public class TicTacToeNinjectModule : NinjectModule
    {
        public override void Load()
        {
            this.Bind<IAllPossibleGameFieldsProvider>().To<AllPossibleGameFieldsProvider>();
            this.Bind<ICorrectCoordinatesChecker>().To<CorrectCoordinatesChecker>();
            this.Bind<IFieldStateConverter>().To<FieldStateConverter>();
            this.Bind<IGameFieldCellsStatisticProvider>().To<GameFieldCellsStatisticProvider>();
            this.Bind<IGameFieldCopyMaker>().To<GameFieldCopyMaker>();
            this.Bind<IGameFieldTransparator>().To<GameFieldTransparator>();
            this.Bind<IGameProcessStatisticProvider>().To<GameProcessStatisticProvider>();
            this.Bind<INewGameFieldCreator>().To<NewGameFieldCreator>();
            this.Bind<INextStepConditionCalculator>().To<NextStepConditionCalculator>();
            this.Bind<IPossibleStepsProvider>().To<PossibleStepsProvider>();
            this.Bind<IStepMaker>().To<StepMaker>();
        }
    }
}
