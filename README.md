# Octopost
Octopost is a web application, where everybody can freely write posts and vote for posts others have already written. Those posts can be sorted with a variety of different filters. What makes Octopost special is, that every post will be given a class classified by a RNN implemented with TensorFlow. 
The technologies used in this project are ASP.NET Core, Flask, TensorFlow and Angular.
![](./octopost.png)

# How to run?

## Train the RNN
Execute the following command in `Octopost.TopicClassifier`:
> python text_classification.py

## Run the RNN
Execute the following command in `Octopost.TopicClassifier`:
> python text_classification.py --server run

## Run the Web Api
Configure ports, connection string and location api key in `appsettings.json`
> If UClassify Api Key is not set, local Python server will be used for classification
Set the environment either to 'Development' or 'Production':

> set ASPNETCORE_ENVIRONMENT=Development|Production
Start the Web application on port 5000

Run the application from the dotnet CLI:
> dotnet Octopost.WebApi.dll

## Run the Web UI
Execute the following command in `Octopost.WebUI`:
> ng serve

## Run Mobile App (unfinished)
Execute the following command in `Octopost.Mobile`:
> ionic serve

## Build the Web UI
> ng build --prod --aot --verbose --build-optimizer
