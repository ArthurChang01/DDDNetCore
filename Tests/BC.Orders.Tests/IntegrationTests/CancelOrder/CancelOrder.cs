using API;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.Framework.Scenarios.Extended;
using LightBDD.NUnit3;
using System.Net.Http;

namespace BC.Orders.Tests.IntegrationTests.CancelOrder
{
    [Label("Store-2")]
    [FeatureDescription(
@"In order to cancel a exiting order
As a customer
I want to send a CancelOrderCommand")]
    public partial class CancelOrder
    {
        private readonly HttpClient _client;

        public CancelOrder()
        {
            this._client = new TestFixture<Startup>().Client;
        }

        [Label("SCENARIO-1")]
        [Scenario]
        public void Template_basic_scenario()
        {
            Runner.RunScenarioAsync(
                _ => Given_template_method(),
                _ => When_template_method(),
                _ => Then_template_method());
        }
    }
}