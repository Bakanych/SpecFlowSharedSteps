@testdata
Feature: [Shared] Feature!
Background: 

Scenario: Shared steps 1
	Given Save 1 to scenario context a1

Scenario: Shared steps 2
	Given Save 1 to scenario context a2

Scenario: Access parent context
	Given Read parent1 from scenario context


