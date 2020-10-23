using System.Threading.Tasks;
using Elsa.Exceptions;
using Elsa.Messages;
using Elsa.Queries;
using Elsa.Services;
using Rebus.Handlers;

namespace Elsa.Consumers
{
    public class RunWorkflowConsumer : IHandleMessages<RunWorkflow>
    {
        private readonly IWorkflowRunner _workflowRunner;
        private readonly IWorkflowInstanceManager _workflowInstanceManager;

        public RunWorkflowConsumer(IWorkflowRunner workflowRunner, IWorkflowInstanceManager workflowInstanceManager)
        {
            _workflowRunner = workflowRunner;
            _workflowInstanceManager = workflowInstanceManager;
        }

        public async Task Handle(RunWorkflow message)
        {
            var workflowInstance = await _workflowInstanceManager.GetByWorkflowInstanceIdAsync(message.WorkflowInstanceId);

            if(workflowInstance == null)
                throw new WorkflowException($"No workflow instance with ID {message.WorkflowInstanceId} was found.");
            
            await _workflowRunner.RunWorkflowAsync(
                workflowInstance,
                message.ActivityId,
                message.Input);
        }
    }
}