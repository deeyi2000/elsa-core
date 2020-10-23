using System.Linq;
using System.Threading.Tasks;
using Elsa.Core.IntegrationTests.Workflows;
using Elsa.Models;
using Elsa.Testing.Shared.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace Elsa.Core.IntegrationTests
{
    public class ForEachWorkflowTests : WorkflowsUnitTestBase
    {
        public ForEachWorkflowTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
        
        [Fact(DisplayName = "Runs one iteration at a time, blocking the Iterate branch when an activity is blocking.")]
        public async Task Test01()
        {
            var items = Enumerable.Range(1, 10).Select(x => $"Item {x}").ToList();
            var workflow = new ForEachWorkflow(items);
            var workflowInstance = await WorkflowRunner.RunWorkflowAsync(workflow);
            var iterationLogs = workflowInstance.ExecutionLog.Where(x => x.ActivityId == "WriteLine").ToList();

            Assert.Equal(WorkflowStatus.Suspended, workflowInstance.Status);
            Assert.Single(iterationLogs);
        }
    }
}