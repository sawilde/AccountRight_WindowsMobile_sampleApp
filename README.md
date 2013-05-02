# AccountRight_windowsMobile_sampleApp #

An example Application for Windows Mobile that uses the **AccountRight Live API**.

## Purpose ##

To demonstrate how to handle the authorisation process i.e. handling the process where a user allows your application to access their **MYOB Company File** data hosted "in the cloud".

## Getting Started ##

Before running this application on a device or emulator it is necessary to register an application (use **http://desktop** as the Redirect Url) and then amend the supplied **client_id** and **client_secret** in the [Configuration.cs](https://github.com/sawilde/AccountRight_WindowsMobile_sampleApp/blob/master/myob.sample/Configuration.cs) file.

## Code Description ##

It is assumed the developer examining this code is familiar with .NET development on the windows phone platform, however the ideas tested here can be easily transferred to other platforms.  

This is not a full application and may require extra error handling to make it production ready; contributions from the community will be accepted.

### Step 1 ###

First it is necessary to get the user to grant your application access to their data, this process eventually leads you to a stage where you will have a OAuth token that can be used to access the **AccountRight Live API** (see [http://developer.myob.com/docs/read/getting_started/Authentication](http://developer.myob.com/docs/read/getting_started/Authentication)).

The sample application uses a hosted browser to extract a **code** from the returned HTML once the user has granted your application access to their data (see *OAuthLogin_Navigated* in [MainPage.xaml.cs](https://github.com/sawilde/AccountRight_WindowsMobile_sampleApp/blob/master/myob.sample/MainPage.xaml.cs).)

### Step 2 ###

Once you have a code it it necessary to use that **code** to fetch an OAuth token that you can use to make requests to the users company files.

This is done by making a request to [https://secure.myob.com/oauth2/v1/authorize](https://secure.myob.com/oauth2/v1/authorize) as detailed in the [Authentication documentation](http://developer.myob.com/docs/read/getting_started/Authentication) (see [OAuthRequestHandler](https://github.com/sawilde/AccountRight_WindowsMobile_sampleApp/blob/master/myob.sample/Communication/OAuthRequestHandler.cs).)

### Step 3 ###

Now that we are in possesion of an **OAuth Token** we can use this token to make requests against the **AccountRight Live API**. The **token** is used in the *Authorization* header (see [ApiRequestHandler](https://github.com/sawilde/AccountRight_WindowsMobile_sampleApp/blob/master/myob.sample/Communication/ApiRequestHandler.cs)) along with other headers such as *x-myobapi-key* and *x-myobapi-cftoken* to access the api. 

### Recommendations ###

We recommend that when accessing the API that you attempt to use compression in your requests to improve performance and to reduce the data delivered to your clients; this is especially important on mobile devices where bandwidth and data download limits apply. Use the header *Accept-Encoding* set to *gzip* in your requests and **check** for the header *Content-Encoding* set to *gzip* in the response. 