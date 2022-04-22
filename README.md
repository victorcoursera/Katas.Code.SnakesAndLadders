# SnakesAndLadders
This is a project based on the requisites specified in https://github.com/VoxelGroup/Katas.Code.SnakesAndLadders/.

## Things to consider
- The solution was created starting from the template provided in https://github.com/jasontaylordev/CleanArchitecture. This template, created by Jason Taylor, contains a basic web app that follows Clean Architecture's principles and makes use of several useful frameworks to achieve a superb result.
- Many parts of the template were removed for the sake of the understanding of the solution by the reviewers of this exercise.
- The web project (REST API) was removed from the solution because I considered that was not an important part for the exercise but, understanding all the principles and patterns followed in the solution, reviewers could see how easily an entrypoint of this nature (o any other one) could be integrated in the solution.
- I was tempted to add some features like fluent validation or authentication/authorization, but I thought it would be too much taking into account the objective and the description of the exercise. The solution must be as understandable as possible.
- I am aware that I probably didn't handle the logic about rolling dice the best way possible, but I found interesting to add it into the application logic (with the limits set in the domain layer) and let the future decide the best approach for it once we start adding new features.

## Running the application
As I said in the previous section, I didn't include a web app so it is a **console app** the one that directly makes use of the commands and queries provided by the application layer (following Mediator pattern and CQRS).
So, in order to run the whole app with some predefined actions, you just have to run ConsoleApp project.
In test folder, you can also find a test project that allows you to run the tests associated to US 1, US 2 and US 3.

As an enthusiastic developer and a person quite interested in best practices regarding software architecture and clean code, I must say I had quite fun doing this exercise. So, thank you and happy reviewing!