Feature: Login

To login successfully
As a user
I have to enter correct username and password

@tag1
Scenario: [Scuccessful login with correct username and password]
	Given I have navigated to the login page
	When I enter correct "<username>" and "<password>"
	Then I should be inventory page

	Examples: 
		| username        | password     |
		| standard_user   | secret_sauce |
		| locked_out_user | secret_sauce |
		| problem_uesr    | secret_sauce |
		| visual_user     | secret_sauce |