# AccountRight_windowsMobile_sampleApp #

An example Application for Windows Mobile that uses the **AccountRight Live API**.

This sample also demonstrates how to download and upload photos which are available on the [Contact](http://developer.myob.com/api/accountright/v2/contact/) and [Item](http://developer.myob.com/api/accountright/v2/inventory/item/) resources.

## Purpose ##

To demonstrate how to handle the authorisation process with the assistance of the [AccountRight Live API SDK](http://www.nuget.org/packages/MYOB.AccountRight.API.SDK/) i.e. handling the process where a user allows your application to access their **MYOB Company File** data hosted "in the cloud".

## Getting Started ##

Before running this application on a device or emulator it is necessary to register an application (use **http://desktop** as the Redirect URL) and then amend the supplied **client_id** and **client_secret** in the [Configuration.cs](https://github.com/MYOB-Technology/AccountRight_WindowsMobile_sampleApp/blob/master/myob.sample/Configuration.cs) file.

## Code Description ##

It is assumed the developer examining this code is familiar with .NET development on the windows phone platform, however the ideas tested here can be easily transferred to other platforms.  

This is not a full application and may require extra error handling to make it production ready; contributions from the community will be accepted.

### Step 1 ###

First it is necessary to get the user to grant your application access to their data, this process eventually leads you to a stage where you will have a OAuth token that can be used to access the **AccountRight Live API** (see [http://developer.myob.com/docs/read/getting_started/Authentication](http://developer.myob.com/docs/read/getting_started/Authentication)).

The sample application uses a hosted browser to extract a **code** from the returned HTML once the user has granted your application access to their data (see *OAuthLogin_Navigated* in [MainPage.xaml.cs](https://github.com/MYOB-Technology/AccountRight_WindowsMobile_sampleApp/blob/master/myob.sample/MainPage.xaml.cs).)

To make the login screen render correctly on WP8 it is necessary to inject some JavaScript when navigating to https://secure.myob.com/oauth2/account/authorize

    OAuthLogin.InvokeScript("eval",
        "var msViewportStyle = document.createElement(\"style\");" +
        "msViewportStyle.appendChild(document.createTextNode(\"@-ms-viewport{width:auto!important}\"));" +
        "document.getElementsByTagName(\"head\")[0].appendChild(msViewportStyle);");

as detailed in this issue https://github.com/twbs/bootstrap/issues/9162

### Step 2 ###

Once you have a code it it necessary to use that **code** to fetch an OAuth token that you can use to make requests to the users company files.

This is done using the [AccountRight Live API SDK](http://www.nuget.org/packages/MYOB.AccountRight.API.SDK/) and the *OAuthService* which makes a request to [https://secure.myob.com/oauth2/v1/authorize](https://secure.myob.com/oauth2/v1/authorize) as detailed in the [Authentication documentation](http://developer.myob.com/docs/read/getting_started/Authentication).)

### Step 3 ###

Now that we are in possession of an **OAuth Token** we can use this token to make requests against the **AccountRight Live API**. 

### Recommendations ###

We recommend that when accessing the API that you attempt to use compression in your requests to improve performance and to reduce the data delivered to your clients; this is especially important on mobile devices where bandwidth and data download limits apply. Use the header *Accept-Encoding* set to *gzip* in your requests and **check** for the header *Content-Encoding* set to *gzip* in the response. 

### Attributions ###

# Man Silhouette #

http://commons.wikimedia.org/wiki/File%3AMan_silhouette-gray.svg.png - By Yvwv (Own work) [GFDL (http://www.gnu.org/copyleft/fdl.html), CC-BY-SA-3.0 (http://creativecommons.org/licenses/by-sa/3.0/) or CC-BY-SA-2.5-2.0-1.0 (http://creativecommons.org/licenses/by-sa/2.5-2.0-1.0)], via Wikimedia Commons
