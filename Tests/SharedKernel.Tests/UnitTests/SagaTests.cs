using FluentAssertions;
using NUnit.Framework;
using SharedKernel.BaseClasses.Saga;
using SharedKernel.Interfaces.Saga;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Tests.UnitTests
{
    [TestFixture]
    public class SagaTests
    {
        private readonly SagaBuilder _saga;

        public SagaTests()
        {
            this._saga = new SagaBuilder();
        }

        [TearDown]
        public void AfterTest()
        {
            this._saga.Reset();
        }

        [Test]
        public async Task NewSaga_NameAndAmountIsCorrect_WhenPassOneActivity()
        {
            //Arrange
            string expectName = "happyTesting";
            int expectAmount = 1;
            async Task action() => await Task.Run(() => { });
            IActivity activity = this._saga.NewSaga("happyTesting")
                .StepWithoutBackup("test", action);

            this._saga.Amount.Should().Be(expectAmount);

            //Act
            await activity.End();

            //Assert
            this._saga.Name.Should().Be(expectName);
            this._saga.Amount.Should().Be(0);
        }

        [Test]
        public async Task NewSaga_NameAndAmountIsCorrect_WhenPassTwoActivity()
        {
            //Arrange
            string expectName = "happyTesting";
            int expectAmount = 2;
            async Task action() => await Task.Run(() => { });
            IActivity activity = this._saga.NewSaga(expectName)
                .StepWithoutBackup("test", action)
                .StepWithoutBackup("test2", action);

            this._saga.Amount.Should().Be(expectAmount);

            //Act
            await activity.End();

            //Assert
            this._saga.Name.Should().Be(expectName);
            this._saga.Amount.Should().Be(0);
        }

        [Test]
        public async Task NewSaga_CulculateResultShouldBe2_WhenPassAddFunctionTwice()
        {
            //Arrange
            string expectName = "happyTesting";
            int expectAmount = 2;
            int counter = 0;
            async Task action() => await Task.Run(() => counter++);
            IActivity activity = this._saga.NewSaga(expectName)
                .StepWithoutBackup("test", action)
                .StepWithoutBackup("test2", action);

            //Act
            await activity.End();

            //Assert
            this._saga.Name.Should().Be(expectName);
            counter.Should().Be(expectAmount);
        }

        [Test]
        public void NewSaga_CalculateResultIsZero_WhenActivityOccurException()
        {
            //Arrange
            string expectName = "happyTesting";
            int expectAmount = 0;
            int counter = 0;
            async Task action() => await Task.Run(() => counter++);
            async Task compensaction() => await Task.Run(() => counter--);
            IActivity activity = this._saga.NewSaga("happyTesting")
                .Step("test", async () => { await action(); throw new Exception(); })
                .Undo("reverse-test", compensaction);

            //Act
            Func<Task> doIt = async () => await activity.End();

            //Assert
            doIt.Should().ThrowExactly<Exception>();
            this._saga.Name.Should().Be(expectName);
            counter.Should().Be(expectAmount);
        }

        [Test]
        public void NewSaga_CalculateResultIsZero_WhenAcitivyAreExecutedAndOccursException()
        {
            //Arrange
            string expectName = "happyTesting";
            int expectAmount = 0;
            int counter = 0;
            async Task action() => await Task.Run(() => counter++);
            async Task compensaction() => await Task.Run(() => counter--);
            IActivity activity = this._saga.NewSaga("happyTesting")
                .Step("test", action)
                .Undo("reverse-test", compensaction)
                .Step("test2", async () => { await action(); throw new Exception(); })
                .Undo("reverse-test", compensaction);

            //Act
            Func<Task> doIt = async () => await activity.End();

            //Assert
            doIt.Should().ThrowExactly<Exception>();
            this._saga.Name.Should().Be(expectName);
            counter.Should().Be(expectAmount);
        }

        [Test]
        public void NewSaga_CalculateResultIsZero_WhenAcitivyAreExecutedAndOccursExceptionOnFirstOne()
        {
            //Arrange
            string expectName = "happyTesting";
            int expectAmount = 0;
            int counter = 0;
            async Task action() => await Task.Run(() => counter++);
            async Task compensaction() => await Task.Run(() => counter--);
            IActivity activity = this._saga.NewSaga("happyTesting")
                .Step("test", async () => { await action(); throw new Exception(); })
                .Undo("reverse-test", compensaction)
                .Step("test2", action)
                .Undo("reverse-test2", compensaction);

            //Act
            Func<Task> doIt = async () => await activity.End();

            //Assert
            doIt.Should().ThrowExactly<Exception>();
            this._saga.Name.Should().Be(expectName);
            counter.Should().Be(expectAmount);
        }
    }
}