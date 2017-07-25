using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace SpecFlowSharedSteps.Tests
{
    public enum ContextType { feature, scenario }


    [Binding]
    public class Steps
    {
        FeatureContext featureContext;
        ScenarioContext scenarioContext;
        public Steps(FeatureContext featureContext, ScenarioContext scenarioContext)
        {
            this.featureContext = featureContext;
            this.scenarioContext = scenarioContext;

        }

        [BeforeScenario(tags: "stack")]
        public void SetInvocationStack()
        {
            var tagPrefix = "maxDepth:";
            var depthTag = scenarioContext.ScenarioInfo.Tags.First(tag => tag.StartsWith(tagPrefix));

            if (depthTag != null)
                featureContext.Set(new SharedStepsInvocation()
                {
                    Index = 0,
                    Max = int.Parse(depthTag.Replace(tagPrefix, string.Empty))
                });
        }

        [StepDefinition(@"Save (.*) to (.*) context (.*)")]
        public void SaveToContext(object value, ContextType contextType, string key)
        {
            switch (contextType)
            {
                case ContextType.feature:
                    featureContext.Add(key, value);
                    break;
                case ContextType.scenario:
                    scenarioContext.Add(key, value);
                    break;
                default:
                    break;
            }
        }

        [StepDefinition(@"Read (.*) from (.*) context")]
        public void ReadFromContext(string key, ContextType contextType)
        {
            object result;
            switch (contextType)
            {
                case ContextType.feature:
                    result = featureContext[key];
                    break;
                case ContextType.scenario:
                    result = scenarioContext[key];
                    break;
                default:
                    break;
            }
        }

    }
}
