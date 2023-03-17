# TodoApi
# Setup

1. This project can run on docker or IIS express
2. Due to reduce the complexity of setup it, didn't use any db for it.
3. This solution is run on dotnet 6, pleaes ensure installed required sdk from https://dotnet.microsoft.com/en-us/download/dotnet/6.0

# The list of things can implement but dut to time constraint does not included on this solution but should be consider when doing real-life application

1. Should use DB to store data, because of want to make it easy to setup the api, it's using json file to store info.
2. In real-life application, should use either third-party authentication or in-house identify server with in-house authentification to generate token and use redis to share the token to multiple instance of identity server. (JWT/oauth2)
3. In real-life application, todo list should have role permission, different user need to see different things can may or may not modify others todo list by adding roles permission.
4. SignalR for real time collaboration should split into standalone solution to decouple the dependency between signalR and api, if having multiple instance of api could use redis to distributed the message to multiple todo api instance.
5. Might consider store the edit history and action history.
6. Might consider soft delete(for certain duration) todo list instead of hard delete.
7. If have multiple instance of the todo api, could consider perform rolling deployment instead of everytime deploy all instance at once.
8. Could consider to use splunk/kibana or any other monitoring tool to analyse log, log should log as structure logging for better easier search & improve readability
9. Could consider use k8s & docker for increase DevOps efficiency for microservices architecture.
10. To support multiple language, could consider use custom ValidateAttribute for validation.
11. Unit test case should cover for all methods.
