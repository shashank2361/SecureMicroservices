Important Steps https://github.com/IdentityServer/IdentityServer4.Quickstart.UI
From Identyyserver run in termoinal iex ((New-Object System.Net.WebClient).DownloadString('https://raw.githubusercontent.com/IdentityServer/IdentityServer4.Quickstart.UI/main/getmain.ps1'))


MVC movie client uses OPENID connect to log on to access MVC movie controllers.
To access Controllers  code flow ( Id password) is used , The client id is  "movies_mvc_client" Grant type is "Code";

After login MVC Client request  Bearer Token from IS4 with the client id as "movieClient", After getting the token ituses Bearer token to connect to connect to API , The API is OAUTH2 protected, for Authentication (login) it uses bearer token
for Authorization it checks the calim types which is client_id of the MVC client which is "movieClient"


Important Read
https://medium.com/identity-beyond-borders/choose-the-right-oauth2-flow-for-your-application-3c87af1c8512

Authorization code flow - User logs in from client app, authorization server returns an authorization code to the app. The app then exchanges the authorization code for access token.
Authorization Code is generated from an authorization server by calling the ISAM Advanced Access Control (AAC) authorization endpoint. The generated Authorization Code is then used by the 3rd Party application, who then calls the ISAM AAC token endpoint to generate the Access Token. 
That access token can then be used to establish a session on behalf of the user without the 3rd party application needing the user credentials. This method of token generation is more secure since it sends the access token directly to the client without exposing it to others through the user-agent.

Implicit grant flow - User logs in from client app, authorization server issues an access token to the client app directly.
This flow is specifically used for clients with in-browser applications as it minimizes the number of calls needed to obtain an access token. Using Implicit grants provides faster response times,
 however security is a concern since the access token is sent in the URI fragment which can be exposed to unauthorized parties and risk being misused for client impersonation. 
The use of this specification should have some level of security control which can be provided by ISAM AAC.