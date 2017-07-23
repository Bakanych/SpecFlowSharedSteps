# SpecFlow Shared Steps
[![Build status](https://ci.appveyor.com/api/projects/status/g18m571fd85vtiys?svg=true)](https://ci.appveyor.com/project/Bakanych/specflowsharedsteps) [![NuGet Status](http://img.shields.io/nuget/v/specflowsharedsteps.svg?style=flat)](https://www.nuget.org/packages/specflowsharedsteps/)

Assume you have some set of steps you wanna share between your specflow scenarios.

Now it's as easy as just one step:
```
Given I execute the steps of 'My Feature'.'Some Scenario' 
```
or
```
Given I execute background steps of 'Some Feature'
```
or even
```
Given I execute the steps of:
| feature     | scenario       |
| Feature One | Shared Steps 1 |
| Feature Two | Shared Steps 2 |
```
See more [examples](https://github.com/Bakanych/SpecFlowSharedSteps/blob/master/SpecFlowSharedSteps.Tests/Features/MasterFeature.feature).

### Shared context
Parent Feature and Scenario Context is used by shared steps. Context values created by shared steps are available in parent scenario.
Be careful which context key duplication.
