using System.Linq;
using System.Threading.Tasks;
using Elsa.Core.IntegrationTests.Workflows;
using Elsa.Testing.Shared.Helpers;
using Xunit;
using Xunit.Abstractions;

namespace Elsa.Core.IntegrationTests
{
    public class ForLoopWorkflowTests : WorkflowsUnitTestBase
    {
        public ForLoopWorkflowTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
        
        [Fact(DisplayName = "Runs all iterations, even when an activity is blocking.")]
        public async Task Test01()
        {
            const int loopCount = 10;
            var workflow = new ForLoopWorkflow(loopCount);
            var workflowInstance = await WorkflowRunner.RunWorkflowAsync(workflow);
            var iterationLogs = workflowInstance.ExecutionLog.Where(x => x.ActivityId == "WriteLine").ToList();

            Assert.Equal(loopCount, iterationLogs.Count);
        }
    }
}