using API;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios.Basic;
using LightBDD.Framework.Scenarios.Extended;
using LightBDD.NUnit3;
using System.Net.Http;
using System.Threading.Tasks;

namespace BC.Orders.Tests.IntegrationTests.ChangeOrder
{
    [Label("Store-3")]
    [FeatureDescription(
@"In order to modify my order
As a customer
I want to send a ChangeOrderCommand")]
    public partial class ChangeOrder
    {
        private readonly HttpClient _client;

        public ChangeOrder()
        {
            this._client = new TestFixture<Startup>().Client;
        }

        [Label("SCENARIO-1")]
        [Scenario]
        public async Task Template_basic_scenario()
        {
            await Runner.RunScenarioAsync(
                _ => Given_template_method(),
                _ => When_template_method(),
                _ => Then_template_method());
        }
    }
}