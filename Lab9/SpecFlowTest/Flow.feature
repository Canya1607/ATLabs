Scenario: Verify success code on request to API
    Given I Send request to https://swapi.dev/api/vehicles/ 
    When request success (200 Code)
    Then test success
    
Scenario: File verify on request to API
    Given I repeat request to https://swapi.dev/api/vehicles/
    When result is similar to previous results saved to file
    Then test success

