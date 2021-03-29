# Pluto Rover Navigation

#### Running the project


1. Run the UnitTests. (Visual Studio Unit test explorer).
2. From Visual Studio select the Web API project as the startup project and hit run.
3. Use the swagger interface to enter commands for the Rover Navigation.
	1. There is validation in place that ensures the provided commands include only the following letters: F,B,L,R.

#### Assestment Feedback

Question:
Do you have any thoughts or feedback you would like to provide for this assessment?

Answer:
In the assestment description it's not clear what is an `obstacle`. 
Who defines the obstacles and how have they been identified?  
For that reason, I assume that the obstacles where gathered from previous trips/feedback to pluto, so I predefined a list with obstacles in the Startup.cs
