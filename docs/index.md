# Chirp

## Design and architecture

### Domain model

- Provide an illustration of your domain model.
- Make sure that it is correct and complete.
- In case you are using ASP.NET Identity, make sure to illustrate that accordingly.

### Architecture — In the small

- Illustrate the organization of your code base.
- That is, illustrate which layers exist in your (onion) architecture.
- Make sure to illustrate which part of your code is residing in which layer.

### Architecture of deployed application

- Illustrate the architecture of your deployed application.
- Remember, you developed a client-server application.
- Illustrate the server component and to where it is deployed, illustrate a client component, and show how these communicate with each other.

### User activities

- Illustrate typical scenarios of a user journey through your _Chirp!_ application.
- That is, start illustrating the first page that is presented to a non-authorized user, illustrate what a non-authorized user can do with your _Chirp!_ application, and finally illustrate what a user can do after authentication.

- Make sure that the illustrations are in line with the actual behavior of your application.

### Sequence of functionality/calls trough _Chirp!_

- With a UML sequence diagram, illustrate the flow of messages and data through your _Chirp!_ application.
- Start with an HTTP request that is send by an unauthorized user to the root endpoint of your application and end with the completely rendered web-page that is returned to the user.

- Make sure that your illustration is complete.
- That is, likely for many of you there will be different kinds of "calls" and responses.
- Some HTTP calls and responses, some calls and responses in C# and likely some more.
- (Note the previous sentence is vague on purpose. I want that you create a complete illustration.)

## Process

### Build, test, release, and deployment

- Illustrate with a UML activity diagram how your _Chirp!_ applications are build, tested, released, and deployed.
- That is, illustrate the flow of activities in your respective GitHub Actions workflows.

- Describe the illustration briefly, i.e., how your application is built, tested, released, and deployed.

### Team work

- Show a screenshot of your project board right before hand-in.
- Briefly describe which tasks are still unresolved, i.e., which features are missing from your applications or which functionality is incomplete.

When a contributor wants to create a new issue, the first thing they will do is go to the GitHub repository on find the `Issues` sections.
The contributor will then find and click on the new issue button and will be promted to select an issue template, where in our case
we only have 1. The template will help them fill out the issue in a generic way with a issue description and some acceptance criteria
if necessary. When the issue is created, it will soon be labeled and assigned to a developer aswell as our Chirp project board.
On the project board it will also be given a status, priority aswell as an optional week, start date and end date. We also used milestones
to keep track of when issues need to be done (our milestones was the project reviews and the project presentation). New a contributor is a

### How to make _Chirp!_ work locally

- There has to be some documentation on how to come from cloning your project to a running system.
- That is, Adrian or Helge have to know precisely what to do in which order.
- Likely, it is best to describe how we clone your project, which commands we have to execute, and what we are supposed to see then.

### How to run test suite locally

When running the test suite locally, you have to follow a few steps.
First, if you haven't already, you need to clone the repository which is described in the previous section. You also need to have .NET 7 SDK installed on your machine. A version can be found [here](https://dotnet.microsoft.com/en-us/download/dotnet/7.0). After those steps are done, you can navigate to the cloned project folder and run the following commands:

1. Setup GitHub oAuth
    ```bash
    cd src/Chirp.Web
    dotnet user-secrets set "GH_CLIENT_ID" YOUR_GITHUB_CLIENT_ID
    dotnet user-secrets set "GH_CLIENT_SECRET" YOUR_GITHUB_CLIENT_SECRET
    ```
2. Restore



## Ethics

### License

- State which software license you chose for your application.

### LLMs, ChatGPT, CoPilot, and others

State which LLM(s) were used during development of your project.
In case you were not using any, just state so.
In case you were using an LLM to support your development, briefly describe when and how it was applied.
Reflect in writing to which degree the responses of the LLM were helpful.
Discuss briefly if application of LLMs sped up your development or if the contrary was the case.

We have used ChatGPT and Github Copilot, which are powered by large language models (LLMs). A big part of utilizing these tools was generating boilerplate code and using them for our frontend development. While CoPilot helped a lot with boilerplate, ChatGPT helped with FrontEnd and BackEnd. ChatGPT assisted when we wanted to have a starting point and get an idea of how to attack the issue. We have a single push where we copy and pasted generated code because of a mistake in codewithme, this was scrapped and changed later on as we had made significant progress before the push.

The responses of the LLM's is very mixed and also depending on the user. They could be to some degree and confusing and if the user does not stay pessimistic it can put you on the wrong track. If being pessimistic and only using it for inspiration, the responses was very helpful.

We feel that leveraging LLMs sped up the process of development mainly in getting started with an issue. If the user of the LLM was too reliant on the LLMs we feel it hindered both the learning outcome but also development

Updated docs

Co-authored-by: Victor <vmem@itu.dk>
Co-authored-by: Philip <phro@itu.dk>
Co-authored-by: Peter <pbjh@itu.dk>>
Co-authored-by: Axel <axlu@itu.dk>>
