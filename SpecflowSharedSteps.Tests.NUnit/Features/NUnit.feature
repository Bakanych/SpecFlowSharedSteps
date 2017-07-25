Feature: NUnit

Scenario: Should execute scenario shared steps
	Given Save 1 to scenario context master1

	Given I execute the steps of '[Shared] Feature!'.'Shared Steps 1' 
	And I execute the steps of '[Shared] Feature!'.'Shared Steps 2' 
	
	Then Read master1 from scenario context
	And Read a1 from scenario context
	And Read a2 from scenario context

Scenario: Should execute feature background shared steps
	Given I execute background steps of Shared Background
	Then Read background from scenario context

Scenario: Should execute feature background shared steps (quotes)
	Given I execute background steps of ''Shared' 'Background''
	Then Read background from scenario context

Scenario: Should execute both background and shared scenario steps
	Given I execute background steps of Shared Background
	And I execute the steps of '[Shared] Feature!'.'Shared Steps 1'

@stack @maxDepth:2
Scenario: Should execute scenario hierarchy with maximum depth level allowed
	Given I execute the steps of 'Master'.'Should execute scenario shared steps'
	Then Read master1 from scenario context
	And Read a1 from scenario context
	And Read a2 from scenario context

Scenario: Should execute shared steps from the table
	Given I execute the steps of:
		| feature           | scenario       |
		| [Shared] Feature! | Shared Steps 1 |
		| [Shared] Feature! | Shared Steps 2 |

