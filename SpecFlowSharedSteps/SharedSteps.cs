﻿using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Tracing;

namespace SpecFlowSharedSteps
{
    public class TestRunnerInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            if (invocation.Method.Name == "ScenarioSetup" ||
                invocation.Method.Name == "ScenarioTearDown"
                )
                return;
            else
                invocation.Proceed();
        }
    }

    [Binding]
    public class SharedSteps
    {
        IEnumerable<Assembly> stepAssemblies;
        ITestRunner testRunner;
        ProxyGenerator pg = new ProxyGenerator();

        public SharedSteps(ITestRunner testRunner, ITestRunnerManager testRunnerManager)
        {
            // NOTE: 2.1 compatible hack.
            // In SpecFlow 2.2 there are ITestRunnerManager.TestAssembly and .BindingAssemblies.
            var method = typeof(TestRunnerManager).GetMethod("GetBindingAssemblies", BindingFlags.Instance | BindingFlags.NonPublic);
            stepAssemblies = method.Invoke(testRunnerManager, null) as IEnumerable<Assembly>;

            this.testRunner = testRunner;
        }
        

        [StepDefinition(@"I execute background steps of '(.*)'")]
        [StepDefinition(@"I execute background steps of (.*)")]
        public void ExecuteBackgroundSteps(string feature)
        {
            ExecuteScenarioSteps(feature, "FeatureBackground");
        }

        [StepDefinition(@"I execute the steps of:")]
        public void ExecuteScenarioStepsOfTable(Table table)
        {
            if (table.Header.Count != 2)
                throw new ArgumentException($"Please specify features and scenarios in the 2 columns table with a header");

            foreach (var row in table.Rows)
            {
                ExecuteScenarioSteps(row[0], row[1]);
            }
        }

        [StepDefinition(@"I execute the steps of '(.*)'\.'(.*)'")]
        public void ExecuteScenarioSteps(string feature, string scenario)
        {
            // Get type of shared feature class
            var featureType = stepAssemblies.SelectMany(x => x.GetTypes()
                        .Where(type => type.Name.ToLower() == $"{ConvertToIdentifier(feature)}feature"))
                        .FirstOrDefault();


            if (featureType == null)
                throw new ArgumentException($"Cannot find feature with name '{feature}'");

            // Get scenario method from feature class
            var scenarioMethod = featureType.GetMethods()
                .FirstOrDefault(x => x.Name.ToLower() == (ConvertToIdentifier(scenario)));
            if (scenarioMethod == null)
                throw new ArgumentException($"Cannot find scenario with name '{scenario}'");

            // Create shared feature proxy class
            var featureProxy = pg.CreateClassProxy(featureType, new TestRunnerInterceptor());


            // Initialize private field 'testRunner' in autogenerated feature class
            var featureTestRunnerField = featureType.GetField("testRunner", BindingFlags.NonPublic | BindingFlags.Instance);
            featureTestRunnerField.SetValue(featureProxy, testRunner);

            // Invoke scenario method of feature proxy class
            try
            {
                scenarioMethod.Invoke(featureProxy, null);
            }
            catch (Exception ex)
            {
                throw ex.InnerException ?? ex;
            }
        }

        private string ConvertToIdentifier(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(name);

            return name.ToIdentifier().ToLower();
        }
    }
}
